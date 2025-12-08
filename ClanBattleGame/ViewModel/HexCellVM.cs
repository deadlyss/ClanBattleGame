using ClanBattleGame.Core;
using ClanBattleGame.Model.Etc;

namespace ClanBattleGame.ViewModel
{
    public enum CellState
    {
        Default,
        Selected,
        MoveAvailable,
        AttackAvailable
    }

    public class HexCellVM : ObservableObject
    {
        public int Row { get; }
        public int Col { get; }

        public double X { get; }
        public double Y { get; }

        private CellState _state = CellState.Default;
        public CellState State
        {
            get => _state;
            set => Set(ref _state, value);
        }

        private Squad _occupant;
        public Squad Occupant
        {
            get => _occupant;
            set => Set(ref _occupant, value);
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
