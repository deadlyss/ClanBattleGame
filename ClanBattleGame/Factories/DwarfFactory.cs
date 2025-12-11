using ClanBattleGame.Interface;
using ClanBattleGame.Model.Units;

namespace ClanBattleGame.Factories
{
    public class DwarfFactory : IClanFactory
    {
        public Race Race => Race.Dwarf;

        private readonly ArcherUnit _rangedPrototype =
            new ArcherUnit("Dwarf Crossbowman", 50, 18, "Crossbow");

        public IUnit CreateArcherUnit() => _rangedPrototype.DeepCopy();
    }
}
