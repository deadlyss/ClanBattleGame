using ClanBattleGame.Core;
using ClanBattleGame.Interface;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ClanBattleGame.Model.Etc
{
    public class Squad : ObservableObject
    {
        public string Name { get; set; }
        public ObservableCollection<IUnit> Units { get; set; }

        private int _totalAttack;
        public int TotalAttack
        {
            get => _totalAttack;
            private set
            {
                _totalAttack = value;
                OnPropertyChanged();
            }
        }

        private int _totalHP;
        public int TotalHP
        {
            get => _totalHP;
            private set
            {
                _totalHP = value;
                OnPropertyChanged();
            }
        }

        public bool IsDead => Units.Count == 0 || Units.All(u => u.IsDead);

        public Squad(string name = "Squad Name") // треба взяти на замітку
        {
            Name = name;
            Units = new ObservableCollection<IUnit>();

            Units.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                    foreach (IUnit u in e.NewItems)
                        SubscribeToUnit(u);

                if (e.OldItems != null)
                    foreach (IUnit u in e.OldItems)
                        UnsubscribeFromUnit(u);

                RecalculateStats();
                OnPropertyChanged(nameof(IsDead));
            };
        }

        private void SubscribeToUnit(IUnit unit)
        {
            if (unit is ObservableObject o)
                o.PropertyChanged += Unit_PropertyChanged;
        }

        private void UnsubscribeFromUnit(IUnit unit)
        {
            if (unit is ObservableObject o)
                o.PropertyChanged -= Unit_PropertyChanged;
        }

        private void Unit_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IUnit.TotalAttack) ||
                e.PropertyName == nameof(IUnit.TotalHealth) ||
                e.PropertyName == nameof(IUnit.CurrentHealth) ||
                e.PropertyName == nameof(IUnit.IsDead))
            {
                RecalculateStats();
                OnPropertyChanged(nameof(IsDead));
            }
        }

        private void RecalculateStats()
        {
            TotalAttack = Units.Sum(u => u.TotalAttack);
            TotalHP = Units.Sum(u => u.TotalHealth);
        }
    }
}