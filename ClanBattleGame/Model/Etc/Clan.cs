using ClanBattleGame.Factories;
using ClanBattleGame.Interface;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Model.Units;
using System;
using System.Collections.Generic;

namespace ClanBattleGame.Model
{
    public class Clan
    {
        public string Name { get; }
        public Leader Leader { get; }
        public Race Race { get; }
        public List<Squad> Squads { get; } = new List<Squad>();
        public Clan(string name, Leader leader, Race race)
        {
            Name = name;
            Leader = leader;
            Race = race;
        }
        public IEnumerable<IUnit> GetAllUnits()
        {
            foreach (var squad in Squads)
                foreach (var unit in squad.Units)
                    yield return unit;

            yield return Leader;
        }

        public IEnumerable<Squad> GetAllSquads()
        {
            foreach (var squad in Squads)
                yield return squad;
        }
        public bool IsDefeated => Leader.IsDead;
        public void AddSquad(Squad squad)
        {
            Squads.Add(squad);
        }
        
    }
}


