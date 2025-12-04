using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Model.Units;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ClanBattleGame.Service
{
    public class ClanBuilder
    {
        private readonly IClanFactory _factory;
        private readonly Random _rnd = new Random();

        public ClanBuilder(IClanFactory factory)
        {
            _factory = factory;
        }

        public Clan Build(string clanName)
        {
            Clan clan = new Clan(clanName);

            int squadCount = _rnd.Next(2, 4); // 2–3 загони

            for (int i = 0; i < squadCount; i++)
                clan.Squads.Add(CreateSquad(i + 1));

            return clan;
        }

        private Squad CreateSquad(int index)
        {
            Squad squad = new Squad($"Загін {index}");

            int unitCount = _rnd.Next(3, 8);

            for (int i = 0; i < unitCount; i++)
                squad.Units.Add(CreateRandomUnit());

            return squad;
        }

        private IUnit CreateRandomUnit()
        {
            IUnit prototype;

            switch (_rnd.Next(3))
            {
                case 0: prototype = _factory.CreateLightUnit(); break;
                case 1: prototype = _factory.CreateHeavyUnit(); break;
                default: prototype = _factory.CreateArcherUnit(); break;
            }

            Race race = _factory.Race;
            string fantasyName = GenerateFantasyName(race);

            return prototype.CloneWithName(fantasyName);
        }

        private readonly Dictionary<Race, string[]> FirstNames = new Dictionary<Race, string[]>
        {
            { Race.Elf, new [] { "Elarion", "Faelis", "Loramir", "Aelar", "Thalindra", "Elenora", "Myrian" } },
            { Race.Dwarf, new [] { "Borin", "Durgan", "Thorin", "Grimdor", "Balrim", "Brundur", "Kazgrim" } }
        };

        private readonly Dictionary<Race, string[]> LastNames = new Dictionary<Race, string[]>
        {
            { Race.Elf, new [] { "Swiftwind", "Moonsong", "Leafblade", "Starbloom", "Nightwhisper" } },
            { Race.Dwarf, new [] { "Ironfist", "Stonehelm", "Deepdelver", "Goldbeard", "Hammerborn" } }
        };

        private string GenerateFantasyName(Race race)
        {
            string first = FirstNames[race][_rnd.Next(FirstNames[race].Length)];
            string last = LastNames[race][_rnd.Next(LastNames[race].Length)];
            return $"{first} {last}";
        }

        // застосування бонусів виконуватиметься в BattleVM після вибору лідера
        public void ApplyLeaderBonuses(Clan clan)
        {
            if (clan.Leader == null)
                return;

            var leader = clan.Leader;

            // Загін лідера
            Squad leaderSquad = null;

            foreach (var squad in clan.Squads)
            {
                if (squad.Units.Contains(leader))
                {
                    leaderSquad = squad;
                    break;
                }
            }

            if (leaderSquad == null)
                return;

            // +10% ATK загону
            foreach (var unit in leaderSquad.Units)
                unit.BonusAttack = (int)(unit.Attack * 0.10);

            // +5% HP всьому клану
            foreach (var squad in clan.Squads)
            {
                foreach (var unit in squad.Units)
                    unit.Health = (int)(unit.Health * 1.05);
            }
        }
    }
}
