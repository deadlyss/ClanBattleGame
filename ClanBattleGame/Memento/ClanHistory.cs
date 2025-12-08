using ClanBattleGame.Model;
using System.Collections.Generic;

namespace ClanBattleGame.Memento
{
    public class ClanHistory
    {
        private Stack<ClanMemento> _history = new Stack<ClanMemento>();

        public void Save(Clan clan)
        {
            _history.Push(new ClanMemento(clan));
        }

        public Clan Restore()
        {
            return _history.Pop().SavedState;
        }

        public bool CanUndo => _history.Count > 0;
    }
}
