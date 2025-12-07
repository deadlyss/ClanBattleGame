using System.Collections.ObjectModel;

namespace ClanBattleGame.ViewModel
{
    public class HexFieldVM
    {
        public ObservableCollection<(double x, double y)> Cells { get; }
            = new ObservableCollection<(double, double)>();

        private const double CellWidth = 70;
        private const double CellHeight = 60.62;

        private const double StepX = CellWidth * 1.0;
        private const double StepY = CellHeight * 0.72;

        public HexFieldVM()
        {
            GenerateHexGrid(10, 10);
        }

        private void GenerateHexGrid(int rows, int cols)
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

                    Cells.Add((x, y));
                }
            }
        }
    }
}
