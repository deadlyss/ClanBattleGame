using ClanBattleGame.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanBattleGame.Memento
{
    public class BattleHistory
    {
        private readonly Stack<BattleMemento> _history = new Stack<BattleMemento>();

        public void Save(BattleMemento m)
        {
            _history.Push(m);
        }

        public BattleMemento Restore()
        {
            return _history.Count > 0 ? _history.Pop() : null;
        }
    }
}
