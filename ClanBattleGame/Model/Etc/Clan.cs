using ClanBattleGame.Interface;
using System.Collections.Generic;
using ClanBattleGame.Model.Etc;
using System;

namespace ClanBattleGame.Model
{
    [Serializable]
    public class Clan
    {
        public string Name { get; }
        public List<Squad> Squads { get; } = new List<Squad>();

        public IUnit Leader { get; set; }

        public Clan(string name)
        {
            Name = name;
        }

        public List<IUnit> GetAllUnits()
        {
            List<IUnit> units = new List<IUnit>();

            foreach (var squad in Squads)
                units.AddRange(squad.Units);

            return units;
        }

        public Clan DeepCopy()
        {
            Clan copy = new Clan(this.Name);

            // копіюємо загони
            foreach (var squad in this.Squads)
                copy.Squads.Add(squad.DeepCopy());

            // копіюємо лідера, якщо він є
            if (this.Leader != null)
            {
                // шукаємо клона лідера у нових копіях загонів
                copy.Leader = copy
                    .GetAllUnits()
                    .Find(u => u.Name == this.Leader.Name);
            }

            return copy;
        }
    }
}
