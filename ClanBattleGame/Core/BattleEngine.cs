using ClanBattleGame.Model;
using System.Linq;

namespace ClanBattleGame.Core
{
    public class BattleEngine
    {
        private readonly Clan _clan1;
        private readonly Clan _clan2;

        public BattleEngine(Clan c1, Clan c2)
        {
            _clan1 = c1;
            _clan2 = c2;
        }

        public string NextTurn()
        {
            var unit1 = _clan1.Units.FirstOrDefault(u => u.Health > 0);
            var unit2 = _clan2.Units.FirstOrDefault(u => u.Health > 0);

            if (unit1 == null || unit2 == null)
                return "Бій завершено.";

            // Атакує юніт 1
            unit2.Health -= unit1.Attack;

            return $"{unit1.Name} ({unit1.Weapon}, рух: {unit1.Movement}) вдарив {unit2.Name} на {unit1.Attack}";
        }
    }
}
