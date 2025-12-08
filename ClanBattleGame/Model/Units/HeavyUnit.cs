using ClanBattleGame.Core;
using ClanBattleGame.Interface;
using System;
using System.Windows.Media.TextFormatting;

namespace ClanBattleGame.Model.Units
{
    [Serializable]
    public class HeavyUnit : ObservableObject, IUnit
    {
        public string Name { get; private set; }
        public string Type => "Heavy";
        public int Attack { get; private set; }
        public int Health { get; private set; }
        public string Weapon { get; private set; }

        private int _health;
        public int BonusHealth
        {
            get => _health;
            set
            {
                if (_health != value)
                {
                    _health = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalHealth));
                }
            }
        }

        private int _bonusAttack;
        public int BonusAttack
        {
            get => _bonusAttack;
            set
            {
                if (_bonusAttack != value)
                {
                    _bonusAttack = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalAttack));
                }
            }
        }

        private int _currentHealth;
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (_currentHealth != value)
                {
                    _currentHealth = Math.Max(0, value);
                    OnPropertyChanged();
                }
            }
        }

        public int TotalHealth => Health + BonusHealth;
        public int TotalAttack => Attack + BonusAttack;

        public HeavyUnit(string name, int health, int attack, string weapon)
        {
            Name = name;
            Health = health;
            Attack = attack;
            Weapon = weapon;
            BonusAttack = 0;
            BonusHealth = 0;
            CurrentHealth = TotalHealth;
        }

        public HeavyUnit()
        {
            Name = "Heavy";
            Health = 60;
            Attack = 10;
            Weapon = "Axe";
            BonusAttack = 0;
            BonusHealth = 0;
        }

        public IUnit Clone()
        {
            return (IUnit)this.MemberwiseClone();
        }

        public IUnit CloneWithName(string newName)
        {
            return new HeavyUnit(newName, this.Health, this.Attack, this.Weapon);
        }
    }
}
