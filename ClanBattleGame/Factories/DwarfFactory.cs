using ClanBattleGame.Interface;
using ClanBattleGame.Model.Units;

namespace ClanBattleGame.Factories
{
    public class DwarfFactory : IClanFactory
    {
        public Race Race => Race.Dwarf;

        private readonly ArcherUnit _rangedPrototype =
            new ArcherUnit("Dwarf Crossbowman", 30, 25, "Crossbow");

        private readonly HeavyUnit _heavyPrototype =
            new HeavyUnit("Dwarf Heavy", 75, 15, "Hammer");

        public IUnit CreateArcherUnit() => _rangedPrototype.DeepCopy();
        public IUnit CreateHeavyUnit() => _heavyPrototype.DeepCopy();
    }
}
