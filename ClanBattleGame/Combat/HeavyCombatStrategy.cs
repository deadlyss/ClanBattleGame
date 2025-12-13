using ClanBattleGame.Interface;
using ClanBattleGame.Model.Etc;
using System;
using System.Linq;

namespace ClanBattleGame.Combat
{
    public class HeavyCombatStrategy : ICombatStrategy
    {
        private readonly Random _rnd = new Random();
        private readonly IBattleLogger _logger;

        public HeavyCombatStrategy(IBattleLogger logger)
        {
            _logger = logger;
        }

        public void Execute(Squad attacker, Squad defender)
        {
            var atkHeavy = attacker.Units.Where(u => u.Type == "Heavy").ToList();
            var defHeavy = defender.Units.Where(u => u.Type == "Heavy").ToList();

            int pairs = Math.Min(atkHeavy.Count, defHeavy.Count);

            // ───────── Heavy vs Heavy ─────────
            for (int i = 0; i < pairs; i++)
            {
                DealDamage(atkHeavy[i], defHeavy[i]);

                if (!defHeavy[i].IsDead)
                    DealDamage(defHeavy[i], atkHeavy[i]);
            }

            for (int i = pairs; i < atkHeavy.Count; i++)
            {
                if (defHeavy.Any())
                    DealDamage(atkHeavy[i], defHeavy[_rnd.Next(defHeavy.Count)]);
            }

            for (int i = pairs; i < defHeavy.Count; i++)
            {
                if (atkHeavy.Any())
                    DealDamage(defHeavy[i], atkHeavy[_rnd.Next(atkHeavy.Count)]);
            }

            if (defHeavy.Any())
                return;

            // ───────── FALLBACK ─────────
            var archers = defender.Units.Where(u => u.Type == "Archer").ToList();
            var leader = defender.Units.FirstOrDefault(u => u.Type == "Leader");

            foreach (var heavy in atkHeavy)
            {
                // 20% шанс захотіти вбити лідера
                if (leader != null && !leader.IsDead && _rnd.NextDouble() < 0.20)
                {
                    if (_rnd.NextDouble() < 0.33)
                        DealDamage(heavy, leader);
                    else
                        LogMiss(heavy, leader, "Leader attack failed (33%)");

                    continue;
                }

                // Archer
                if (archers.Any())
                {
                    var target = archers[_rnd.Next(archers.Count)];

                    if (_rnd.NextDouble() < 0.50)
                        DealDamage(heavy, target);
                    else
                        LogMiss(heavy, target, "Archer attack failed (50%)");

                    continue;
                }

                // Тільки Leader
                if (leader != null && !leader.IsDead)
                {
                    if (_rnd.NextDouble() < 0.33)
                        DealDamage(heavy, leader);
                    else
                        LogMiss(heavy, leader, "Leader attack failed (33%)");
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
                Phase = "Heavy",
                Result = "Hit"
            });
        }

        private void LogMiss(IUnit attacker, IUnit defender, string reason)
        {
            _logger?.Log(new BattleLogEntry
            {
                Attacker = attacker.Name,
                Defender = defender?.Name ?? "-",
                Damage = 0,
                DefenderHpAfter = defender?.CurrentHealth ?? 0,
                Phase = "Heavy",
                Result = "Miss",
                Reason = reason
            });
        }
    }
}
