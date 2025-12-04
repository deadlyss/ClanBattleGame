using ClanBattleGame.Core;
using ClanBattleGame.Interface;
using System;

namespace ClanBattleGame.Model.Units
{
    [Serializable]
    public class HeavyUnit : ObservableObject, IUnit
    {
        public string Name { get; private set; }
        public string Type => "Heavy";
        private int _health;
        public int Attack { get; private set; }
        public string Weapon { get; private set; }
        public int BonusAttack { get; set; } = 0;
        public int TotalAttack => Attack + BonusAttack;

        public int Health
        {
            get => _health;
            set
            {
                if (_health != value)
                {
                    _health = value;
                    OnPropertyChanged();
                }
            }
        }

        public HeavyUnit(string name, int health, int attack, string weapon)
        {
            Name = name;
            Health = health;
            Attack = attack;
            Weapon = weapon;
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
