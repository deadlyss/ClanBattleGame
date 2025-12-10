using System;
using System.Collections.Generic;

namespace ClanBattleGame.Model.Units
{
    public enum LeaderType
    {
        Player,
        Enemy
    }
    public sealed class Leader
    {
        private static readonly Dictionary<LeaderType, Leader> _instances =
            new Dictionary<LeaderType, Leader>();

        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Attack { get; private set; }

        private Leader(string name, int health, int attack)
        {
            Name = name;
            Health = health;
            Attack = attack;
        }

        public static Leader GetInstance(LeaderType type)
        {
            Leader instance;

            if (_instances.TryGetValue(type, out instance))
                return instance;

            switch (type)
            {
                case LeaderType.Player:
                    instance = new Leader("Player Leader", 120, 40);
                    break;

                case LeaderType.Enemy:
                    instance = new Leader("Enemy Leader", 140, 35);
                    break;

                default:
                    throw new Exception("Unknown leader type");
            }

            _instances[type] = instance;
            return instance;
        }

        public Leader DeepCopy()//копія для битви
        {
            return new Leader(
                String.Copy(this.Name),
                this.Health,
                this.Attack
            );
        }
    }
}
