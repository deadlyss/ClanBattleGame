using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClanBattleGame.Service
{
    public enum BattleResult
    {
        AttackerWins,
        DefenderWins,
        AttackerDies,
        DefenderDies,
        BothDie,
        BothAlive
    }

    public class BattleEngine
    {
        private Random rnd = new Random();

        public BattleOutcome FightOnce(Squad attacker, Squad defender)
        {
            StringBuilder log = new StringBuilder();

            var aliveA = attacker.Units.Where(u => u.CurrentHealth > 0).ToList();
            var aliveB = defender.Units.Where(u => u.CurrentHealth > 0).ToList();

            if (aliveA.Count == 0)
                return new BattleOutcome { Result = BattleResult.AttackerDies, Log = "Атакуючий загін вже мертвий." };

            if (aliveB.Count == 0)
                return new BattleOutcome { Result = BattleResult.DefenderDies, Log = "Захисник вже мертвий." };

            int max = Math.Max(aliveA.Count, aliveB.Count);

            for (int i = 0; i < max; i++)
            {
                aliveA = attacker.Units.Where(u => u.CurrentHealth > 0).ToList();
                aliveB = defender.Units.Where(u => u.CurrentHealth > 0).ToList();

                if (aliveA.Count == 0 || aliveB.Count == 0)
                    break;

                var A = aliveA[i % aliveA.Count];
                var B = aliveB[i % aliveB.Count];

                // А атакує B
                int dmgA = A.TotalAttack;
                B.CurrentHealth -= dmgA;
                if (B.CurrentHealth < 0) B.CurrentHealth = 0;

                log.AppendLine($"{A.Name} → {B.Name} ({dmgA})   HP={B.CurrentHealth}");

                if (B.CurrentHealth == 0)
                {
                    log.AppendLine($"{B.Name} загинув!");
                    continue;
                }

                // Контратака
                int dmgB = B.TotalAttack;
                A.CurrentHealth -= dmgB;
                if (A.CurrentHealth < 0) A.CurrentHealth = 0;

                log.AppendLine($"{B.Name} → {A.Name} ({dmgB})   HP={A.CurrentHealth}");

                if (A.CurrentHealth == 0)
                    log.AppendLine($"{A.Name} загинув!");
            }

            bool attackerAlive = attacker.Units.Any(u => u.CurrentHealth > 0);
            bool defenderAlive = defender.Units.Any(u => u.CurrentHealth > 0);

            BattleResult result;

            if (attackerAlive && !defenderAlive) result = BattleResult.AttackerWins;
            else if (!attackerAlive && defenderAlive) result = BattleResult.DefenderWins;
            else if (!attackerAlive && !defenderAlive) result = BattleResult.BothDie;
            else result = BattleResult.BothAlive;

            return new BattleOutcome
            {
                Result = result,
                Log = log.ToString()
            };
        }
    }
}
