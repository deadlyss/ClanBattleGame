using ClanBattleGame.Core;
using ClanBattleGame.Factories;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Units;
using ClanBattleGame.Service;

namespace ClanBattleGame.ViewModel
{
    public class BattlePageVM : ObservableObject
    {
        public Clan PlayerClan { get; }
        public Clan EnemyClan { get; }
        public HexFieldVM Field { get; }

        public BattlePageVM(NavigationStore navStore, Clan playerClan)
        {
            PlayerClan = playerClan;

            // Генеруємо ворога
            EnemyClan = GenerateEnemyClan();

            // Генеруємо поле бою
            Field = new HexFieldVM(10, 10);

            // Розміщуємо юнітів
            Field.PlaceInitialUnits(PlayerClan, EnemyClan);
        }

        private Clan GenerateEnemyClan()
        {
            // тимчасово фіксовано — потім зробимо фабрику для AI
            var factory = new DwarfFactory();
            var builder = new ClanBuilder(factory);

            var leader = new Leader("Ворожий лідер", 120, 25);
            return builder.CreateClan("Ворожий клан", leader);
        }
    }
}
