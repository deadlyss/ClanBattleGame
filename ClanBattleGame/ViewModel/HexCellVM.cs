using ClanBattleGame.Core;
using ClanBattleGame.Model.Etc;

namespace ClanBattleGame.ViewModel
{
    public class HexCellVM : ObservableObject
    {
        public int Row { get; }
        public int Col { get; }

        private double _x;
        public double X
        {
            get => _x;
            set { _x = value; OnPropertyChanged(); }
        }

        private double _y;
        public double Y
        {
            get => _y;
            set { _y = value; OnPropertyChanged(); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        private Squad _occupant;
        public Squad Occupant
        {
            get => _occupant;
            set { _occupant = value; OnPropertyChanged(); }
        }

        private bool _isHighlighted;
        public bool IsHighlighted
        {
            get => _isHighlighted;
            set { _isHighlighted = value; OnPropertyChanged(); }
        }

        private bool _isEnemyInRange;
        public bool IsEnemyInRange
        {
            get => _isEnemyInRange;
            set { _isEnemyInRange = value; OnPropertyChanged(); }
        }

        public HexCellVM(int row, int col, double x, double y)
        {
            Row = row;
            Col = col;
            X = x;
            Y = y;
        }
    }
}
