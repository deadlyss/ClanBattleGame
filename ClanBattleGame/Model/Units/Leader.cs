using ClanBattleGame.Interface;
using System;

namespace ClanBattleGame.Model.Units
{
    [Serializable]
    public sealed class Leader
    {
        private static Leader _instance;

        public IUnit Unit { get; private set; }

        private Leader() { }

        public static Leader Create(IUnit unit)
        {
            if (_instance == null)
            {
                _instance = new Leader();
            }

            _instance.Unit = unit;   // оновлюємо лідера (важливо для Restore)
            return _instance;
        }

        public static Leader Instance => _instance;

        // 🔹 Ось цього методу тобі не вистачало
        public static void Reset()
        {
            _instance = null;
        }
    }

}
