using ClanBattleGame.Core;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Service;
using ClanBattleGame.Service.Printers;
using System.Linq;

namespace ClanBattleGame.ViewModel
{
    public class BattleVM : ObservableObject
    {
        // -----------------------------
        //           ПОЛЯ
        // -----------------------------
        private readonly HexNeighborService _neighbor;
        private readonly HexHighlighter _highlighter;
        private readonly BattleEngine _engine;

        private readonly SquadPrinter _printer;

        private HexCellVM _selectedCell;
        private Squad _selectedSquad;

        public Clan PlayerClan { get; }
        public Clan EnemyClan { get; }

        public HexFieldVM HexField { get; }

        // -----------------------------
        //    ВЛАСТИВОСТІ
        // -----------------------------
        public HexCellVM SelectedCell
        {
            get => _selectedCell;
            set => Set(ref _selectedCell, value);
        }

        public Squad SelectedSquad
        {
            get => _selectedSquad;
            set
            {
                Set(ref _selectedSquad, value);
                OnPropertyChanged();
            }
        }

        private string _battleLog;
        public string BattleLog
        {
            get => _battleLog;
            set => Set(ref _battleLog, value);
        }

        private string _selectedInfo;
        public string SelectedInfo
        {
            get => _selectedInfo;
            set => Set(ref _selectedInfo, value);
        }

        private bool _isBattleFinished;
        public bool IsBattleFinished
        {
            get => _isBattleFinished;
            set => Set(ref _isBattleFinished, value);
        }

        // -----------------------------
        //         HP для UI
        // -----------------------------
        public int PlayerTotalHP =>
            PlayerClan?.Squads.Sum(s => s.Units.Sum(u => u.TotalHealth)) ?? 0;

        public int PlayerCurrentHP =>
            PlayerClan?.Squads.Sum(s => s.Units.Sum(u => u.CurrentHealth)) ?? 0;

        public int EnemyTotalHP =>
            EnemyClan?.Squads.Sum(s => s.Units.Sum(u => u.TotalHealth)) ?? 0;

        public int EnemyCurrentHP =>
            EnemyClan?.Squads.Sum(s => s.Units.Sum(u => u.CurrentHealth)) ?? 0;

        public void RefreshHP()
        {
            OnPropertyChanged(nameof(PlayerTotalHP));
            OnPropertyChanged(nameof(PlayerCurrentHP));
            OnPropertyChanged(nameof(EnemyTotalHP));
            OnPropertyChanged(nameof(EnemyCurrentHP));
        }

        // -----------------------------
        //        КОМАНДИ
        // -----------------------------
        public RelayCommand SelectCellCommand { get; }
        public RelayCommand MoveOrAttackCommand { get; }
        public RelayCommand ReturnCommand { get; }

        // -----------------------------
        //       КОНСТРУКТОР
        // -----------------------------
        public BattleVM(NavigationStore navStore, Clan player, Clan enemy)
        {
            PlayerClan = player.DeepCopy();
            EnemyClan = enemy.DeepCopy();

            _printer = new SquadPrinter();
            _neighbor = new HexNeighborService();
            _highlighter = new HexHighlighter(_neighbor);
            _engine = new BattleEngine();

            HexField = new HexFieldVM();
            HexField.PlaceInitialUnits(PlayerClan, EnemyClan);

            // Команди
            SelectCellCommand = new RelayCommand(OnCellSelected);
            MoveOrAttackCommand = new RelayCommand(OnMoveOrAttack);

            ReturnCommand = new RelayCommand(_ =>
            {
                new NavigationService(navStore, () => new HomeVM(navStore)).Navigate();
            });
        }

        // -----------------------------
        //      ВИБІР КЛІТИНКИ
        // -----------------------------
        private void OnCellSelected(object param)
        {
            var cell = param as HexCellVM;
            if (cell == null) return;

            // Скинути підсвітку
            _highlighter.ClearAll(HexField.Cells);

            // Повторний клік → скинути вибір
            if (SelectedCell == cell)
            {
                SelectedCell = null;
                SelectedSquad = null;
                SelectedInfo = "";
                return;
            }

            SelectedCell = cell;
            SelectedInfo = BuildCellInfo(cell);

            // Якщо в клітинці наш загін
            if (cell.Occupant != null && PlayerClan.Squads.Contains(cell.Occupant))
            {
                SelectedSquad = cell.Occupant;
                cell.State = CellState.Selected;

                _highlighter.HighlightNeighbors(HexField.Cells, cell);
                _highlighter.HighlightEnemies(HexField.Cells, cell, PlayerClan);
                return;
            }

            // Якщо клітинка підсвічена → рух або атака
            if (cell.State == CellState.MoveAvailable ||
                cell.State == CellState.AttackAvailable)
            {
                MoveOrAttackCommand.Execute(cell);
            }
        }

        // -----------------------------
        //     РУХ АБО АТАКА
        // -----------------------------
        private void OnMoveOrAttack(object param)
        {
            var target = param as HexCellVM;
            if (target == null || SelectedSquad == null)
                return;

            var current = HexField.Cells.First(c => c.Occupant == SelectedSquad);

            // АТАКА
            if (target.State == CellState.AttackAvailable)
            {
                var enemySquad = target.Occupant;

                // ----------------------------
                //    ЛОГ ПЕРЕД АТАКОЮ
                // ----------------------------
                BattleLog += $"\n[{SelectedSquad.Name}] атакує [{enemySquad.Name}]!\n";

                // Бій
                var outcome = _engine.FightOnce(SelectedSquad, enemySquad);

                // Лог урону по кожному юніту
                foreach (var report in outcome.Reports)
                {
                    BattleLog += $"- {report}\n";
                }

                RefreshHP();

                // Видалення мертвих ворогів
                for (int i = enemySquad.Units.Count - 1; i >= 0; i--)
                {
                    if (enemySquad.Units[i].CurrentHealth <= 0)
                    {
                        BattleLog += $"Ворог {enemySquad.Units[i].Name} помер\n";
                        enemySquad.Units.RemoveAt(i);
                    }
                }

                // Видалення мертвих своїх
                for (int i = SelectedSquad.Units.Count - 1; i >= 0; i--)
                {
                    if (SelectedSquad.Units[i].CurrentHealth <= 0)
                    {
                        BattleLog += $"Твій юніт {SelectedSquad.Units[i].Name} загинув\n";
                        SelectedSquad.Units.RemoveAt(i);
                    }
                }

                bool attackerDead = !SelectedSquad.Units.Any();
                bool defenderDead = !enemySquad.Units.Any();

                // Атакуючий мертвий
                if (attackerDead)
                {
                    PlayerClan.Squads.Remove(SelectedSquad);
                    current.Occupant = null;
                }

                // Захисник мертвий
                if (defenderDead)
                {
                    EnemyClan.Squads.Remove(enemySquad);
                    target.Occupant = null;
                }

                // Якщо захисник мертвий → атакуючий займає клітинку
                if (!attackerDead && defenderDead)
                {
                    target.Occupant = SelectedSquad;
                    current.Occupant = null;
                }

                // Якщо обидва загону мертві
                if (attackerDead && defenderDead)
                {
                    // Обидві клітинки стають пустими
                    current.Occupant = null;
                    target.Occupant = null;
                }

                // ----------------------------
                //    РОЗМІЩЕННЯ ЗАГОНІВ
                // ----------------------------
                switch (outcome.Result)
                {
                    case BattleResult.AttackerWins:
                    case BattleResult.DefenderDies:
                        if (!enemySquad.Units.Any())
                        {
                            BattleLog += $"[{SelectedSquad.Name}] знищив [{enemySquad.Name}]!\n";
                            EnemyClan.Squads.Remove(enemySquad);
                            target.Occupant = SelectedSquad;
                            current.Occupant = null;
                        }
                        break;

                    case BattleResult.AttackerDies:
                        if (!SelectedSquad.Units.Any())
                        {
                            BattleLog += $"Твій загін [{SelectedSquad.Name}] загинув!\n";
                            PlayerClan.Squads.Remove(SelectedSquad);
                            current.Occupant = null;
                        }
                        break;

                    case BattleResult.BothDie:
                        BattleLog += $"Обидва загони повністю знищені!\n";
                        PlayerClan.Squads.Remove(SelectedSquad);
                        EnemyClan.Squads.Remove(enemySquad);
                        current.Occupant = null;
                        target.Occupant = null;
                        break;
                }

                // ----------------------------
                //    ПЕРЕВІРКА ЗАВЕРШЕННЯ БОЮ
                // ----------------------------
                CheckForEndGame();

                // ----------------------------
                //    ОЧИЩЕННЯ ВИБОРУ
                // ----------------------------
                _highlighter.ClearAll(HexField.Cells);
                SelectedCell = null;
                SelectedSquad = null;
                return;
            }

            // РУХ
            if (target.State == CellState.MoveAvailable)
            {
                BattleLog += $"\n[{SelectedSquad.Name}] переміщується на ({target.Row}, {target.Col})\n";

                current.Occupant = null;
                target.Occupant = SelectedSquad;

                _highlighter.ClearAll(HexField.Cells);
                SelectedCell = null;
                SelectedSquad = null;
            }
        }

        // -----------------------------
        //     ІНФОРМАЦІЯ ПРО КЛІТИНКУ
        // -----------------------------
        private string BuildCellInfo(HexCellVM cell)
        {
            if (cell == null || cell.Occupant == null)
                return $"Клітинка [{cell?.Row}, {cell?.Col}]\nПорожня";

            var squad = cell.Occupant;
            bool isPlayer = PlayerClan.Squads.Contains(squad);

            return _printer.PrintSquadInfo(squad, isPlayer);
        }

        // -----------------------------
        //     ПЕРЕВІРКА КІНЦЯ БОЮ
        // -----------------------------
        private void CheckForEndGame()
        {
            if (!EnemyClan.Squads.Any())
            {
                EndBattle("Перемога! Всі вороги знищені.");
            }

            if (!PlayerClan.Squads.Any())
            {
                EndBattle("Поразка! У тебе не лишилось загонів.");
            }
        }

        public void EndBattle(string msg)
        {
            BattleLog += "\n" + msg;
            IsBattleFinished = true;
        }
    }
}
