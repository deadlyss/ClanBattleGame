using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Units;

namespace ClanBattleGame.Factories
{
    public class ElfFactory : IClanFactory
    {
        public Race Race => Race.Elf;

        private readonly LightUnit _lightPrototype =
            new LightUnit("Elf Scout", 45, 12, "Dagger");

        private readonly HeavyUnit _heavyPrototype =
            new HeavyUnit("Elf Guardian", 80, 18, "Spear");

        private readonly ArcherUnit _rangedPrototype =
            new ArcherUnit("Elf Archer", 55, 22, "Longbow");

        public IUnit CreateLightUnit() => _lightPrototype.Clone();
        public IUnit CreateHeavyUnit() => _heavyPrototype.Clone();
        public IUnit CreateArcherUnit() => _rangedPrototype.Clone();
    }
}
