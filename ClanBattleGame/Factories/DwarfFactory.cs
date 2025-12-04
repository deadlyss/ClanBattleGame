using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Units;

namespace ClanBattleGame.Factories
{
    public class DwarfFactory : IClanFactory
    {
        public Race Race => Race.Dwarf;

        private readonly LightUnit _lightPrototype =
            new LightUnit("Dwarf Miner", 60, 8, "Pickaxe");

        private readonly HeavyUnit _heavyPrototype =
            new HeavyUnit("Dwarf Warrior", 100, 20, "Warhammer");

        private readonly ArcherUnit _rangedPrototype =
            new ArcherUnit("Dwarf Crossbowman", 50, 18, "Crossbow");

        public IUnit CreateLightUnit() => _lightPrototype.Clone();
        public IUnit CreateHeavyUnit() => _heavyPrototype.Clone();
        public IUnit CreateArcherUnit() => _rangedPrototype.Clone();
    }
}
