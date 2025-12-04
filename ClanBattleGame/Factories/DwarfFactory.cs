using ClanBattleGame.Interface;
using ClanBattleGame.Model.Units;

namespace ClanBattleGame.Factories
{
    public class DwarfFactory : IClanFactory
    {
        public IWarrior CreateLightUnit()
        {
            return new DwarfMiner();
        }

        public IWarrior CreateHeavyUnit()
        {
            return new DwarfWarrior();
        }

        public IWarrior CreateEliteUnit()
        {
            return new DwarfBerserk();
        }
    }
}
