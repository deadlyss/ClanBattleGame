using ClanBattleGame.Interface;
using ClanBattleGame.Model.Units;

namespace ClanBattleGame.Factories
{
    public class ElfFactory : IClanFactory
    {
        public Race Race => Race.Elf;

        private readonly ArcherUnit _rangedPrototype =
            new ArcherUnit("Elf Archer", 55, 22, "Longbow");

        private readonly HeavyUnit _heavyPrototype =
            new HeavyUnit("Elf Heavy", 75, 15, "Hammer");

        public IUnit CreateArcherUnit() => _rangedPrototype.DeepCopy();

        public IUnit CreateHeavyUnit() => _heavyPrototype.DeepCopy();
    }
}
