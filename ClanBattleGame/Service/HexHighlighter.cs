using ClanBattleGame.Model;
using ClanBattleGame.ViewModel;
using System.Collections.ObjectModel;

namespace ClanBattleGame.Service
{
    public class HexHighlighter
    {
        private readonly HexNeighborService _neighbor;

        public HexHighlighter(HexNeighborService neighbor)
        {
            _neighbor = neighbor;
        }

        // -------------------------
        //   Reset → Default
        // -------------------------
        public void ClearAll(ObservableCollection<HexCellVM> cells)
        {
            foreach (var c in cells)
                c.State = CellState.Default;
        }

        // -------------------------
        //  Підсвітка руху
        // -------------------------
        public void HighlightNeighbors(ObservableCollection<HexCellVM> cells, HexCellVM center)
        {
            // Спочатку прибрати старе
            foreach (var c in cells)
                if (c.State != CellState.Selected)
                    c.State = CellState.Default;

            if (center == null)
                return;

            foreach (var other in cells)
            {
                if (_neighbor.IsNeighbor(center, other) && other.Occupant == null)
                {
                    other.State = CellState.MoveAvailable;
                }
            }
        }

        // -------------------------
        //   Підсвітка атаки
        // -------------------------
        public void HighlightEnemies(
            ObservableCollection<HexCellVM> cells,
            HexCellVM center,
            Clan playerClan)
        {
            if (center == null)
                return;

            foreach (var cell in cells)
            {
                if (_neighbor.IsNeighbor(center, cell) &&
                    cell.Occupant != null &&
                    !playerClan.Squads.Contains(cell.Occupant))
                {
                    cell.State = CellState.AttackAvailable;
                }
            }
        }
    }
}
