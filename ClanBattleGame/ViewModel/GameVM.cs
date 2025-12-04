using ClanBattleGame.Core;
using ClanBattleGame.Factories;
using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using System.Data.Odbc;
using ClanBattleGame.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ClanBattleGame.ViewModel
{
    public class GameVM : ObservableObject
    {
        private string _log;
        public string Log
        {
            get => _log;
            set { _log = value; OnPropertyChanged(); }
        }

        public ICommand NextTurnCommand { get; }

        private BattleEngine _engine;

        public GameVM()
        {
            // Створюємо кланів через фабрики
            var elfFactory = new ElfFactory();
            //var orcFactory = new OrcFactory();
            var dwarfFactory = new DwarfFactory();
            var random = new Random();

            var clan1 = new Clan { Name = "Ельфи" };
            clan1.Units.Add(elfFactory.CreateLightUnit());
            clan1.Units.Add(elfFactory.CreateHeavyUnit());

            var clan2 = new Clan { Name = "Ельфи2" };
            clan2.Units.Add(elfFactory.CreateLightUnit());
            clan2.Units.Add(elfFactory.CreateHeavyUnit());
            clan1 = BuildClan("Клан А", elfFactory, dwarfFactory, random);
            clan2 = BuildClan("Клан Б", elfFactory, dwarfFactory, random);

            _engine = new BattleEngine(clan1, clan2);

            NextTurnCommand = new RelayCommand(o =>
            {
                Log += _engine.NextTurn() + "\n";
            });
        }

        private static Clan BuildClan(string name, IClanFactory elfFactory, IClanFactory dwarfFactory, Random random)
        {
            var clan = new Clan { Name = name };
            var positions = Enumerable.Range(1, 3).OrderBy(_ => random.Next()).ToList();

            clan.Squads.Add(CreateSquad("Загін воїнів", positions[0], random, () => new Warrior()));
            clan.Squads.Add(CreateSquad("Загін ельфів", positions[1], random, () => RandomElf(elfFactory, random)));
            clan.Squads.Add(CreateSquad("Загін гномів", positions[2], random, () => RandomDwarf(dwarfFactory, random)));

            foreach (var squad in clan.Squads.OrderBy(s => s.FrontlinePosition))
            {
                clan.Units.AddRange(squad.Members);
            }

            clan.Leader = clan.Units[random.Next(clan.Units.Count)];
            return clan;
        }

        private static Squad CreateSquad(string name, int position, Random random, Func<IWarrior> generator)
        {
            var squad = new Squad
            {
                Name = name,
                FrontlinePosition = position
            };

            var memberCount = random.Next(2, 6);
            for (int i = 0; i < memberCount; i++)
            {
                squad.Members.Add(generator());
            }

            return squad;
        }

        private static IWarrior RandomElf(IClanFactory factory, Random random)
        {
            var options = new List<IWarrior>
            {
                factory.CreateLightUnit(),
                factory.CreateHeavyUnit(),
                factory.CreateEliteUnit()
            };

            return options[random.Next(options.Count)];
        }

        private static IWarrior RandomDwarf(IClanFactory factory, Random random)
        {
            var options = new List<IWarrior>
            {
                factory.CreateLightUnit(),
                factory.CreateHeavyUnit(),
                factory.CreateEliteUnit()
            };

            return options[random.Next(options.Count)];
        }
    }
}
