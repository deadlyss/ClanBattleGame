using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClanBattleGame.Service
{
    public class BattleEngine
    {
        private Random rnd = new Random();

        public BattleState Initialize(Clan clanA, Clan clanB)
        {
            return new BattleState
            {
                ClanA = clanA,
                ClanB = clanB,
                IsFinished = false
            };
        }

        private bool CheckLeaderDeath(Clan clan)
        {
            return clan?.Leader?.Health <= 0;
        }

        private List<Squad> GetAliveSquads(Clan clan)
        {
            return clan.Squads
                       .Where(s => s.Units.Any(u => u.Health > 0))
                       .ToList();
        }

        public string Step(BattleState state, Squad playerSelectedSquad)
        {
            if (state.IsFinished)
                return "Бій завершено.";

            StringBuilder log = new StringBuilder();

            // Перевірка, чи є живі загони у обох сторін
            var aliveA = GetAliveSquads(state.ClanA);
            var aliveB = GetAliveSquads(state.ClanB);

            if (!aliveA.Any())
            {
                state.IsFinished = true;
                return "У твоєму клані не залишилося живих загонів! Поразка!";
            }

            if (!aliveB.Any())
            {
                state.IsFinished = true;
                return "У противника не залишилося живих загонів! Перемога!";
            }

            bool enemyFirst = rnd.Next(2) == 0;

            Squad attacker, defender;

            if (enemyFirst)
            {
                // Противник атакує
                attacker = aliveB[rnd.Next(aliveB.Count)];

                // Гравець обирає свій загін
                if (playerSelectedSquad == null || !aliveA.Contains(playerSelectedSquad))
                    defender = aliveA.First(); // запасний варіант
                else
                    defender = playerSelectedSquad;

                log.AppendLine($"Противник ходить першим! Атакує загін {attacker.Name}");
            }
            else
            {
                // Гравець атакує
                if (playerSelectedSquad == null || !aliveA.Contains(playerSelectedSquad))
                    attacker = aliveA.First();
                else
                    attacker = playerSelectedSquad;

                defender = aliveB[rnd.Next(aliveB.Count)];

                log.AppendLine($"Ти ходиш першим! Атакує загін {attacker.Name}");
            }

            FightRound(attacker, defender, log);

            // Видаляємо загін, якщо мертвий
            if (!attacker.Units.Any(u => u.Health > 0))
            {
                log.AppendLine($"Загін {attacker.Name} вибув із гри!");
            }

            if (!defender.Units.Any(u => u.Health > 0))
            {
                log.AppendLine($"Загін {defender.Name} вибув із гри!");
            }

            // Перевірка смерті лідерів
            if (CheckLeaderDeath(state.ClanA))
            {
                state.IsFinished = true;
                log.AppendLine("‼ Лідер твого клану загинув! Поразка!");
            }

            if (CheckLeaderDeath(state.ClanB))
            {
                state.IsFinished = true;
                log.AppendLine("‼ Лідер противника загинув! Перемога!");
            }

            return log.ToString();
        }


        private void FightRound(Squad a, Squad b, StringBuilder log)
        {
            var aliveA = a.Units.Where(u => u.Health > 0).ToList();
            var aliveB = b.Units.Where(u => u.Health > 0).ToList();

            if (aliveA.Count == 0 || aliveB.Count == 0)
                return;

            int max = Math.Max(aliveA.Count, aliveB.Count);

            for (int i = 0; i < max; i++)
            {
                aliveA = a.Units.Where(u => u.Health > 0).ToList();
                aliveB = b.Units.Where(u => u.Health > 0).ToList();

                if (aliveA.Count == 0 || aliveB.Count == 0)
                    return;

                var A = aliveA[i % aliveA.Count];
                var B = aliveB[i % aliveB.Count];

                // A атакує B
                B.Health -= A.Attack;
                if (B.Health < 0) B.Health = 0;

                log.AppendLine($"{A.Name} → {B.Name} (-{A.Attack})  HP={B.Health}");

                if (B.Health == 0)
                {
                    log.AppendLine($"{B.Name} загинув!");
                    continue;
                }

                // B контратака
                A.Health -= B.Attack;
                if (A.Health < 0) A.Health = 0;

                log.AppendLine($"{B.Name} → {A.Name} (-{B.Attack})  HP={A.Health}");

                if (A.Health == 0)
                    log.AppendLine($"{A.Name} загинув!");
            }
        }
    }
}
