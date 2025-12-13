using ClanBattleGame.Interface;
using ClanBattleGame.Model.Etc;
using System;
using System.Linq;

namespace ClanBattleGame.Combat
{
    public class ArcherCombatStrategy : ICombatStrategy
    {
        private readonly Random _rnd = new Random();
        private readonly IBattleLogger _logger;

        public ArcherCombatStrategy(IBattleLogger logger)
        {
            _logger = logger;
        }

        public void Execute(Squad attacker, Squad defender)
        {
            var archers = attacker.Units.Where(u => u.Type == "Archer").ToList();
            var leader = defender.Units.FirstOrDefault(u => u.Type == "Leader");

            var heavy = defender.Units.Where(u => u.Type == "Heavy").ToList();
            var ranged = defender.Units.Where(u => u.Type == "Archer").ToList();

            foreach (var archer in archers)
            {
                if (leader != null && !leader.IsDead && _rnd.NextDouble() < 0.10)
                {
                    if (_rnd.NextDouble() < 0.30)
                        DealDamage(archer, leader);
                    else
                        LogMiss(archer, leader, "Leader shot failed (30%)");

                    continue;
                }

                if (heavy.Any())
                {
                    DealDamage(archer, heavy[_rnd.Next(heavy.Count)]);
                    continue;
                }

                if (ranged.Any())
                {
                    DealDamage(archer, ranged[_rnd.Next(ranged.Count)]);
                }
            }
        }

        private void DealDamage(IUnit attacker, IUnit defender)
        {
            int damage = attacker.TotalAttack;

            defender.CurrentHealth -= damage;
            if (defender.CurrentHealth < 0)
                defender.CurrentHealth = 0;

            _logger?.Log(new BattleLogEntry
            {
                Attacker = attacker.Name,
                Defender = defender.Name,
                Damage = damage,
                DefenderHpAfter = defender.CurrentHealth,
                Phase = "Archer",
                Result = "Hit"
            });
        }

        private void LogMiss(IUnit attacker, IUnit defender, string reason)
        {
            _logger?.Log(new BattleLogEntry
            {
                Attacker = attacker.Name,
                Defender = defender.Name,
                Damage = 0,
                DefenderHpAfter = defender.CurrentHealth,
                Phase = "Archer",
                Result = "Miss",
                Reason = reason
            });
        }
    }
}
