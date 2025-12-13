using ClanBattleGame.Combat;
using ClanBattleGame.Core;
using ClanBattleGame.Factories;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Model.Units;
using ClanBattleGame.Service;
using System.Linq;

namespace ClanBattleGame.ViewModel
{
    public enum BattleResult
    {
        AttackerWon,
        DefenderWon,
        BothAlive,
        BothDead
    }
    public class BattlePageVM : ObservableObject
    {
        public Clan PlayerClan { get; }
        public Clan EnemyClan { get; }
        public HexFieldVM Field { get; }

        private readonly HexNeighborService _neighbor; //клітинки що поряд
        private readonly HexHighlighter _highlighter; //підсвітка клітин

        private HexCellVM _selectedCell;
        public HexCellVM SelectedCell
        {
            get => _selectedCell;
            set => Set(ref _selectedCell, value);
        }

        private Squad _selectedSquad;
        public Squad SelectedSquad
        {
            get => _selectedSquad;
            set => Set(ref _selectedSquad, value);
        }

        private string _battleLog;
        public string BattleLog
        {
            get => _battleLog;
            set
            {
                _battleLog = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SelectCellCommand { get; }
        public RelayCommand MoveOrAttackCommand { get; }

        public BattlePageVM(NavigationStore navStore, Clan playerClan)
        {
            PlayerClan = playerClan;

            // Генеруємо ворога
            EnemyClan = GenerateEnemyClan();

            // Генеруємо поле бою
            Field = new HexFieldVM(10, 10);

            _neighbor = new HexNeighborService();
            _highlighter = new HexHighlighter(_neighbor);

            // Розміщуємо юнітів
            Field.PlaceInitialUnits(PlayerClan, EnemyClan);
            //команди
            SelectCellCommand = new RelayCommand(o => SelectCell(o as HexCellVM));
            MoveOrAttackCommand = new RelayCommand(o => MoveOrAttack(o as HexCellVM));

            UpdateSelectedInfo();
        }

        private Clan GenerateEnemyClan()
        {
            // тимчасово фіксовано — потім зробимо фабрику для AI
            var factory = new DwarfFactory();
            var builder = new ClanBuilder(factory);

            var leader = new Leader("Ворожий лідер", 120, 25);
            return builder.CreateClan("Ворожий клан", leader);
        }

        private void UpdateSelectedInfo()
        {
           BattleLog =
                ClanInfoPrinter.PrintInfo(PlayerClan) +
                "\n\n" +
                ClanInfoPrinter.PrintInfo(EnemyClan);
        }

        private void SelectCell(HexCellVM cell)
        {
            if (cell == null)
                return;

            // якщо клітинку клацнули повторно → скидаємо все
            if (SelectedCell == cell && cell.State == CellState.Selected)
            {
                _highlighter.ClearAll(Field.Cells);
                SelectedCell = null;
                SelectedSquad = null;
                return;
            }

            // Оновлюємо вибрану клітинку
            SelectedCell = cell;

            // якщо клітинка містить загін гравця → вибір загону
            if (cell.Occupant != null && PlayerClan.Squads.Contains(cell.Occupant))
            {
                SelectedSquad = cell.Occupant;

                // Скидаємо старі підсвітки, але не свою
                _highlighter.ClearAll(Field.Cells);

                cell.State = CellState.Selected;

                // Підсвітка руху
                _highlighter.HighlightNeighbors(Field.Cells, cell);

                // Підсвітка ворогів
                _highlighter.HighlightEnemies(Field.Cells, cell, PlayerClan);

                return;
            }

            // якщо клітинка пуста → просто підсвітити її
            _highlighter.ClearAll(Field.Cells);

            SelectedSquad = null;
            cell.State = CellState.Selected;
        }

        private void MoveOrAttack(HexCellVM targetCell)
        {
            if (targetCell == null || SelectedSquad == null)
                return;

            //  Атака
            if (targetCell.State == CellState.AttackAvailable)
            {
                var atk = SelectedSquad;
                var def = targetCell.Occupant;

                var result = PerformAttack(atk, def);

                switch (result)
                {
                    case BattleResult.AttackerWon:
                        // Атакер займає клітинку ворога
                        Field.GetCellOfSquad(atk).Occupant = null;
                        targetCell.Occupant = atk;
                        break;

                    case BattleResult.DefenderWon:
                        // Атакуючий загін знищено
                        Field.GetCellOfSquad(atk).Occupant = null;
                        break;

                    case BattleResult.BothDead:
                        Field.GetCellOfSquad(atk).Occupant = null;
                        targetCell.Occupant = null;
                        break;
                }

                _highlighter.ClearAll(Field.Cells);

                UpdateSelectedInfo();

                SelectedCell = null;
                SelectedSquad = null;
                return;
            }

            //  Рух
            if (targetCell.State == CellState.MoveAvailable)
            {
                MoveSquadToCell(targetCell);
                SelectedCell = null;
                SelectedSquad = null;

                UpdateSelectedInfo();

                return;
            }
        }

        private void MoveSquadToCell(HexCellVM target)
        {
            var current = Field.Cells.First(c => c.Occupant == SelectedSquad);

            current.Occupant = null;
            target.Occupant = SelectedSquad;

            _highlighter.ClearAll(Field.Cells);
        }

        private BattleResult PerformAttack(Squad attacker, Squad defender)
        {
            var logger = new ConsoleBattleLogger();

            if (attacker == null || defender == null)
                return BattleResult.BothAlive;

            BattleEngine engine = new BattleEngine(logger);
            engine.Fight(attacker, defender);

            bool attackerDead = attacker.IsDead;
            bool defenderDead = defender.IsDead;

            if (attackerDead && defenderDead)
                return BattleResult.BothDead;

            if (attackerDead)
                return BattleResult.DefenderWon;

            if (defenderDead)
                return BattleResult.AttackerWon;

            return BattleResult.BothAlive;
        }
    }
}
