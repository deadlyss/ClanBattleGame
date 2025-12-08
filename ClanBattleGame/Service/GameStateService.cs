using ClanBattleGame.Factories;
using ClanBattleGame.Interface;
using ClanBattleGame.Memento;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Model.Units;
using System.Collections.ObjectModel;

namespace ClanBattleGame.Service
{
    public class GameStateService
    {
        private static GameStateService _instance;
        public ClanHistory History { get; } = new ClanHistory();
        public static GameStateService Instance => _instance ?? (_instance = new GameStateService());

        private GameStateService() { }

        public string ClanName { get; set; }
        public Race Race { get; set; }
        public string LeaderName { get; set; }

        public int Money { get; set; }
        public int Fights { get; set; }

        public Clan EnemyClan { get; set; }

        public ObservableCollection<Squad> Squads { get; set; } = new ObservableCollection<Squad>();

        public void StartNewGame(string clanName, Race race, string leaderName, IClanFactory factory)
        {
            ClanName = clanName;
            Race = race;
            LeaderName = leaderName;

            Money = 9999;
            Fights = 0;

            Squads.Clear();

            IUnit baseLeaderUnit = factory.CreateHeavyUnit();
            IUnit leaderUnit = baseLeaderUnit.CloneWithName(leaderName);

            // Додаємо бонуси лідеру
            leaderUnit.BonusAttack += 5;
            leaderUnit.BonusHealth += 20;

            // Реєструємо в Multiton
            Leader.Create(clanName, leaderUnit);

            // Додаємо в перший загін
            var squad = new Squad("Загін лідера");
            squad.Units.Add(leaderUnit);
            Squads.Add(squad);
        }

        public void GenerateEnemyClan()
        {
            IClanFactory factory;

            if (Race == Race.Elf)
                factory = new DwarfFactory();
            else
                factory = new ElfFactory();

            var builder = new ClanBuilder(factory);
            EnemyClan = builder.Build("Ворожий клан");
        }

        public Clan GetPlayerClan()
        {
            var clan = new Clan(ClanName);

            // Лідер з Multiton
            var leaderObj = Leader.Get(ClanName);
            if (leaderObj != null)
                clan.Leader = leaderObj.Unit;

            // Копіюємо загони (глибока копія найкраще)
            foreach (var s in Squads)
                clan.Squads.Add(s.DeepCopy());

            return clan;
        }
    }
}
