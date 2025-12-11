using ClanBattleGame.Factories;
using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Model.Units;
using System;

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

        public Clan CreateClan(string clanName, Leader leader)
        {
            Clan clan = new Clan(clanName, leader, _factory.Race);

            int squadCount = _rnd.Next(2, 4); // 2–3 загони
            

            for (int i = 0; i < squadCount; i++)
                clan.Squads.Add(CreateSquad(i + 1));

            clan.Squads[0].Units.Insert(0, leader); //додаємо лідера до першого загону

            return clan;
        }

        private Squad CreateSquad(int index)
        {
            Squad squad = new Squad($"Загін {index}");

            int unitCount = _rnd.Next(3, 8); // 3 - 7 юнітів

            for (int i = 0; i < unitCount; i++)
                squad.Units.Add(CreateUnit());

            return squad;
        }

        private IUnit CreateUnit()
        {
            return _factory.CreateArcherUnit().DeepCopy();
        }
    }
}
