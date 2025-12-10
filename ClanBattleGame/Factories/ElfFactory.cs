using ClanBattleGame.Interface;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Model.Units;

namespace ClanBattleGame.Factories
{
    public class ElfFactory : IClanFactory
    {
        public Race Race => Race.Elf;

        private readonly ArcherUnit _rangedPrototype =
            new ArcherUnit("Elf Archer", 55, 22, "Longbow");

        public IUnit CreateArcherUnit() => _rangedPrototype.Clone();
    }
}
