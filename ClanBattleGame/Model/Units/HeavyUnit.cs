using ClanBattleGame.Core;
using ClanBattleGame.Interface;
using System;

namespace ClanBattleGame.Model.Units
{
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
                if (Set(ref _health, value))
                    OnPropertyChanged(nameof(TotalHealth));
            }
        }

        private int _bonusAttack;
        public int BonusAttack
        {
            get => _bonusAttack;
            set
            {
                if (Set(ref _bonusAttack, value))
                    OnPropertyChanged(nameof(TotalAttack));
            }
        }

        public bool IsDead => CurrentHealth <= 0;

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

        public HeavyUnit() // дефолт конструктор
        {
            Name = "Heavy";
            Health = 30;
            Attack = 15;
            Weapon = "Hammer";
            BonusAttack = 0;
            BonusHealth = 0;
        }

        public IUnit DeepCopy()
        {
            var clone = new HeavyUnit
            {
                Name = String.Copy(this.Name),
                Health = this.Health,
                Attack = this.Attack,
                Weapon = String.Copy(this.Weapon),

                BonusAttack = this.BonusAttack,
                BonusHealth = this.BonusHealth,
                CurrentHealth = this.CurrentHealth
            };
            return clone;

        }
    }
}
