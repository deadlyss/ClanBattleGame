using ClanBattleGame.Core;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClanBattleGame.ViewModel
{
    public class HexFieldVM : ObservableObject
    {
        public ObservableCollection<HexCellVM> Cells { get; }
            = new ObservableCollection<HexCellVM>();

        private const double CellWidth = 70;
        private const double CellHeight = 60.62;

        private const double StepX = CellWidth;
        private const double StepY = CellHeight * 0.72;

        private int _rows;
        private int _cols;

        public RelayCommand SelectCellCommand { get; }
        public RelayCommand MoveCommand { get; }

        public Clan PlayerClan { get; set; }
        public event Action PlayerTurnEnded;
        public BattleEngine BattleEngine { get; set; }
        public Clan EnemyClan { get; set; }
        public BattleVM OwnerVM { get; set; }
        private HexCellVM _selectedCell;
        public HexCellVM SelectedCell
        {
            get => _selectedCell;
            set
            {
                if (_selectedCell != null)
                    _selectedCell.IsSelected = false;

                _selectedCell = value;

                if (_selectedCell != null)
                    _selectedCell.IsSelected = true;

                OnPropertyChanged();
            }
        }

        public Squad SelectedPlayerSquad { get; set; }

        public HexFieldVM(int rows = 10, int cols = 10)
        {
            _rows = rows;
            _cols = cols;

            GenerateGrid(rows, cols);

            SelectCellCommand = new RelayCommand(cellObj =>
            {
                var cell = cellObj as HexCellVM;
                if (cell == null)
                    return;

                // Якщо клік по клітинці з нашим загоном → вибір
                if (cell.Occupant != null && PlayerClan.Squads.Contains(cell.Occupant))
                {
                    SelectedPlayerSquad = cell.Occupant;

                    HighlightNeighbors(cell);       // зелене підсвічування руху
                    HighlightEnemiesAround(cell);   // червоне підсвічування ворогів

                    return;
                }

                // якщо клік по пустій клітинці — пробуємо рух
                MoveCommand.Execute(cell);
            });

            MoveCommand = new RelayCommand(cellObj =>
            {
                var target = cellObj as HexCellVM;
                if (target == null)
                    return;

                if (SelectedPlayerSquad == null)
                    return;

                // Можемо рухати тільки свої загони
                if (!PlayerClan.Squads.Contains(SelectedPlayerSquad))
                    return;

                var current = Cells.FirstOrDefault(c => c.Occupant == SelectedPlayerSquad);
                if (current == null)
                    return;

                var enemySquad = target.Occupant;

                // ------------------------------------------------------
                //                        А Т А К А
                // ------------------------------------------------------
                if (enemySquad != null && EnemyClan.Squads.Contains(enemySquad))
                {
                    // Провести один раунд бою
                    var outcome = BattleEngine.FightOnce(SelectedPlayerSquad, enemySquad);

                    // Записати лог у BattleVM
                    if (OwnerVM != null)
                        OwnerVM.BattleLog = outcome.Log;

                    // --- Видаляємо мертвих юнітів в enemySquad ---
                    for (int i = enemySquad.Units.Count - 1; i >= 0; i--)
                        if (enemySquad.Units[i].CurrentHealth <= 0)
                            enemySquad.Units.RemoveAt(i);

                    // --- Видаляємо мертвих юнітів в SelectedPlayerSquad ---
                    for (int i = SelectedPlayerSquad.Units.Count - 1; i >= 0; i--)
                        if (SelectedPlayerSquad.Units[i].CurrentHealth <= 0)
                            SelectedPlayerSquad.Units.RemoveAt(i);

                    switch (outcome.Result)
                    {
                        // --------------------------------------------------
                        //            АТАКУЮЧИЙ ВИГРАВ, ВОРОГ МЕРТВИЙ
                        // --------------------------------------------------
                        case BattleResult.AttackerWins:
                        case BattleResult.DefenderDies:

                            // Якщо ворог повністю мертвий — прибрати загін з клану
                            if (!enemySquad.Units.Any())
                            {
                                EnemyClan.Squads.Remove(enemySquad);

                                // Звільнити клітинку
                                target.Occupant = null;

                                // Захопити клітинку
                                current.Occupant = null;
                                target.Occupant = SelectedPlayerSquad;
                            }
                            break;

                        // --------------------------------------------------
                        //            АТАКУЮЧИЙ ПОМЕР
                        // --------------------------------------------------
                        case BattleResult.AttackerDies:

                            if (!SelectedPlayerSquad.Units.Any())
                            {
                                PlayerClan.Squads.Remove(SelectedPlayerSquad);
                                current.Occupant = null;
                            }
                            break;

                        // --------------------------------------------------
                        //            ОБИДВА ЗАГОНА ПОМЕРЛИ
                        // --------------------------------------------------
                        case BattleResult.BothDie:

                            PlayerClan.Squads.Remove(SelectedPlayerSquad);
                            EnemyClan.Squads.Remove(enemySquad);

                            current.Occupant = null;
                            target.Occupant = null;
                            break;

                        // --------------------------------------------------
                        //            ОБИДВА ЖИВІ → нічого не міняємо
                        // --------------------------------------------------
                        case BattleResult.BothAlive:
                            break;
                    }

                    // Очистити підсвітку та завершити хід
                    ClearHighlightsAfterAction();
                    PlayerTurnEnded?.Invoke();
                    SelectedPlayerSquad = null;

                    return; // Атака завжди завершує хід
                }

                // ------------------------------------------------------
                //                        Р У Х
                // ------------------------------------------------------
                if (enemySquad == null)
                {
                    if (!IsNeighbor(current, target))
                        return;

                    current.Occupant = null;
                    target.Occupant = SelectedPlayerSquad;

                    ClearHighlightsAfterAction();
                    PlayerTurnEnded?.Invoke();
                    SelectedPlayerSquad = null;
                }
            });
        }

        private void ClearHighlightsAfterAction()
        {
            foreach (var c in Cells)
            {
                c.IsHighlighted = false;
                c.IsEnemyInRange = false;
            }
        }
        public void HighlightEnemiesAround(HexCellVM cell)
        {
            // спочатку прибираємо стару підсвітку
            foreach (var c in Cells)
                c.IsEnemyInRange = false;

            if (cell == null)
                return;

            foreach (var other in Cells)
            {
                // Перевіряємо лише сусідні клітинки
                if (!IsNeighbor(cell, other))
                    continue;

                // Якщо в Occupant стоїть ворог → підсвічуємо
                if (other.Occupant != null && !PlayerClan.Squads.Contains(other.Occupant))
                    other.IsEnemyInRange = true;
            }
        }

        public void HighlightNeighbors(HexCellVM cell)
        {
            foreach (var c in Cells)
            {
                c.IsHighlighted = false;
                c.IsEnemyInRange = false;
            }
            // спочатку знімаємо підсвітку зі всіх
            foreach (var c in Cells)
                c.IsHighlighted = false;

            if (cell == null)
                return;

            foreach (var other in Cells)
            {
                if (IsNeighbor(cell, other) && other.Occupant == null)
                    other.IsHighlighted = true;
            }
        }

        public bool HasMovedThisTurn { get; private set; }

        public void ResetTurn()
        {
            HasMovedThisTurn = false;
        }

        private bool IsNeighbor(HexCellVM a, HexCellVM b)
        {
            // прямі сусіди
            if (a.Row == b.Row && Math.Abs(a.Col - b.Col) == 1)
                return true;

            // сусіди зверху/знизу
            if (Math.Abs(a.Row - b.Row) == 1)
            {
                // якщо ряд непарний → зміщення вправо
                if (a.Row % 2 == 1)
                {
                    if (b.Col == a.Col || b.Col == a.Col + 1)
                        return true;
                }
                else
                {
                    if (b.Col == a.Col || b.Col == a.Col - 1)
                        return true;
                }
            }

            return false;
        }

        private void GenerateGrid(int rows, int cols)
        {
            Cells.Clear();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    double x = c * StepX;
                    double y = r * StepY;

                    if (r % 2 == 1)
                        x += CellWidth / 2;

                    Cells.Add(new HexCellVM(r, c, x, y));
                }
            }
        }

        public void PlaceInitialUnits(Clan player, Clan enemy)
        {
            // --------------------------
            // РОЗМІЩЕННЯ ЗАГОНІВ ГРАВЦЯ
            // --------------------------
            int playerCol = 0;

            foreach (var squad in player.Squads)
            {
                if (squad.Units.Count == 0)
                    continue; // пропускаємо пусті загони

                int row = _rows - 1;
                int col = playerCol;

                if (col < _cols)
                {
                    var cell = GetCell(row, col);
                    cell.Occupant = squad;
                    playerCol++;
                }
                else
                    break; // немає місця на рядку
            }

            // --------------------------
            // РОЗМІЩЕННЯ ЗАГОНІВ ВОРОГА
            // --------------------------
            int enemyCol = _cols - 1;

            foreach (var squad in enemy.Squads)
            {
                if (squad.Units.Count == 0)
                    continue; // пропускаємо пусті загони

                int row = 0;
                int col = enemyCol;

                if (col >= 0)
                {
                    var cell = GetCell(row, col);
                    cell.Occupant = squad;
                    enemyCol--;
                }
                else
                    break;
            }
        }

        private HexCellVM GetCell(int row, int col)
        {
            return Cells.First(c => c.Row == row && c.Col == col);
        }
    }
}
