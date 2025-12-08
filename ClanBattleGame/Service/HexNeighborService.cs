using ClanBattleGame.ViewModel;
using System;

namespace ClanBattleGame.Service
{
    public class HexNeighborService
    {
        public bool IsNeighbor(HexCellVM a, HexCellVM b)
        {
            if (a.Row == b.Row && Math.Abs(a.Col - b.Col) == 1)
                return true;

            if (Math.Abs(a.Row - b.Row) == 1)
            {
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
    }
}
