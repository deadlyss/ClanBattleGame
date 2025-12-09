using ClanBattleGame.Core;
using ClanBattleGame.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClanBattleGame.ViewModel
{
    public class HexFieldVM : ObservableObject
    {
        public ObservableCollection<HexCellVM> Cells { get; }
            = new ObservableCollection<HexCellVM>();

        private readonly int _rows;
        private readonly int _cols;

        // Геометрія
        private const double CellWidth = 70;
        private const double CellHeight = 60.62;

        private const double StepX = CellWidth;
        private const double StepY = CellHeight * 0.72;


        public HexFieldVM(int rows = 10, int cols = 10)
        {
            _rows = rows;
            _cols = cols;

            GenerateGrid();
        }

        private void GenerateGrid()
        {
            Cells.Clear();

            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    double x = c * StepX;
                    double y = r * StepY;

                    if (r % 2 == 1)
                        x += CellWidth / 2;

                    Cells.Add(new HexCellVM(r, c, x, y));
                }
            }
        }

        public HexCellVM GetCell(int row, int col)
        {
            return Cells.First(c => c.Row == row && c.Col == col);
        }

        // 🔹 ОЦЕ МИ ПОВЕРТАЄМО 🔹
        public void PlaceInitialUnits(Clan player, Clan enemy)
        {
            // Гравець знизу
            int playerCol = 0;

            foreach (var squad in player.Squads)
            {
                if (squad.Units.Count == 0)
                    continue;

                int row = _rows - 1;
                int col = playerCol;

                if (col < _cols)
                {
                    var cell = GetCell(row, col);
                    cell.Occupant = squad;
                    playerCol++;
                }
                else
                    break;
            }

            // Ворог зверху
            int enemyCol = _cols - 1;

            foreach (var squad in enemy.Squads)
            {
                if (squad.Units.Count == 0)
                    continue;

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
    }
}
