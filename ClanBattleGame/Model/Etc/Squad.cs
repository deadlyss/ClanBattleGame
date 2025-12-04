using ClanBattleGame.Interface;
using System;
using System.Collections.Generic;

namespace ClanBattleGame.Model.Etc
{
    [Serializable]
    public class Squad
    {
        public string Name { get; set; }
        public List<IUnit> Units { get; set; } = new List<IUnit>();

        public Squad(string name)
        {
            Name = name;
        }

        public Squad() { }

        public Squad DeepCopy()
        {
            var newSquad = new Squad(this.Name);

            foreach (var unit in Units)
                newSquad.Units.Add(unit.Clone()); // твій Prototype ідеально підходить

            return newSquad;
        }
    }
}