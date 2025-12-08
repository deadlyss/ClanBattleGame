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
            BattleOutcome outcome = new BattleOutcome();

            var aliveA = attacker.Units.Where(u => u.CurrentHealth > 0).ToList();
            var aliveB = defender.Units.Where(u => u.CurrentHealth > 0).ToList();

            if (aliveA.Count == 0)
            {
                outcome.Reports.Add("❌ Атакуючий загін вже мертвий.");
                outcome.Result = BattleResult.AttackerDies;
                return outcome;
            }

            if (aliveB.Count == 0)
            {
                outcome.Reports.Add("❌ Захисник вже мертвий.");
                outcome.Result = BattleResult.DefenderDies;
                return outcome;
            }

            int max = Math.Max(aliveA.Count, aliveB.Count);

            for (int i = 0; i < max; i++)
            {
                aliveA = attacker.Units.Where(u => u.CurrentHealth > 0).ToList();
                aliveB = defender.Units.Where(u => u.CurrentHealth > 0).ToList();

                if (aliveA.Count == 0 || aliveB.Count == 0)
                    break;

                var A = aliveA[i % aliveA.Count];
                var B = aliveB[i % aliveB.Count];

                // -------------------------------
                //      АТАКА АТАКУЮЧОГО
                // -------------------------------
                int dmgA = A.TotalAttack;
                B.CurrentHealth -= dmgA;
                if (B.CurrentHealth < 0) B.CurrentHealth = 0;

                outcome.Reports.Add($"{A.Name} атакує {B.Name} на {dmgA} урону.  (HP {B.CurrentHealth})");

                if (B.CurrentHealth == 0)
                {
                    outcome.Reports.Add($"💀 {B.Name} загинув!");
                    continue; // B не може контратакувати
                }

                // -------------------------------
                //          КОНТРАТАКА
                // -------------------------------
                int dmgB = B.TotalAttack;
                A.CurrentHealth -= dmgB;
                if (A.CurrentHealth < 0) A.CurrentHealth = 0;

                outcome.Reports.Add($"{B.Name} контратакує {A.Name} на {dmgB} урону.  (HP {A.CurrentHealth})");

                if (A.CurrentHealth == 0)
                {
                    outcome.Reports.Add($"💀 {A.Name} загинув!");
                }
            }

            bool attackerAlive = attacker.Units.Any(u => u.CurrentHealth > 0);
            bool defenderAlive = defender.Units.Any(u => u.CurrentHealth > 0);

            if (attackerAlive && !defenderAlive) outcome.Result = BattleResult.AttackerWins;
            else if (!attackerAlive && defenderAlive) outcome.Result = BattleResult.DefenderWins;
            else if (!attackerAlive && !defenderAlive) outcome.Result = BattleResult.BothDie;
            else outcome.Result = BattleResult.BothAlive;

            return outcome;
        }
    }
}
