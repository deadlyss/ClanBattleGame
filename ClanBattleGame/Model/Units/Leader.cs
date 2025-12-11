using ClanBattleGame.Core;
using ClanBattleGame.Interface;
using System;
using System.Xml.Linq;

namespace ClanBattleGame.Model.Units
{
    public class Leader : ObservableObject, IUnit //не може бути одинаком бо зміни в ХП
    {                                             //після бою
        public string Name { get; private set; }
        public string Type => "Leader";

        public int Health { get; private set; }
        public int Attack { get; private set; }

        private int _currentHealth;
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                int newValue = Math.Max(0, value);

                if (Set(ref _currentHealth, newValue))
                    OnPropertyChanged(nameof(IsDead));
            }
        }

        public int TotalHealth => Health;
        public int TotalAttack => Attack;

        public bool IsDead => CurrentHealth <= 0;

        public Leader(string name, int health, int attack)
        {
            Name = name;
            Health = health;
            Attack = attack;
            _currentHealth = health;
        }
        public Leader()
        {
            Name = "Leader Name";
            Health = 100;
            Attack = 20;
            _currentHealth = Health;
        }

        public IUnit DeepCopy()
        {
            var copy = new Leader(Name, Health, Attack);
            copy.CurrentHealth = this.CurrentHealth;
            return copy;
        }
    }
}
