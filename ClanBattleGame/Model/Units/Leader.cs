using ClanBattleGame.Interface;
using System;
using System.Collections.Generic;

namespace ClanBattleGame.Model.Units
{
    [Serializable]
    public sealed class Leader
    {
        // MULTITON: ключ → лідер
        private static Dictionary<string, Leader> _leaders = new Dictionary<string, Leader>();

        public IUnit Unit { get; private set; }
        public string ClanName { get; private set; }

        private Leader() { }

        public static Leader Create(string clanName, IUnit unit)
        {
            if (!_leaders.ContainsKey(clanName))
                _leaders[clanName] = new Leader();

            _leaders[clanName].Unit = unit;
            _leaders[clanName].ClanName = clanName;

            return _leaders[clanName];
        }

        public static Leader Get(string clanName)
        {
            return _leaders.ContainsKey(clanName) ? _leaders[clanName] : null;
        }

        // 🔥 ТУТ головний метод, якого у тебе немає
        public static IEnumerable<Leader> GetAll()
        {
            return _leaders.Values;
        }

        public static void Reset()
        {
            _leaders.Clear();
        }
    }
}
