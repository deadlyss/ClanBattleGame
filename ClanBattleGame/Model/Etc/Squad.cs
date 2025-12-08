using ClanBattleGame.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClanBattleGame.Model.Etc
{
    [Serializable]
    public class Squad
    {
        public string Name { get; set; }
        public ObservableCollection<IUnit> Units { get; set; }

        public Squad(string name)
        {
            Name = name;
            Units = new ObservableCollection<IUnit>(); // ← важливо!
        }

        public Squad()
        {
            Units = new ObservableCollection<IUnit>(); // ← важливо!
        }

        public Squad DeepCopy()
        {
            var newSquad = new Squad(this.Name);

            foreach (var unit in Units)
                newSquad.Units.Add(unit.Clone()); // твій Prototype ідеально підходить

            return newSquad;
        }
    }
}