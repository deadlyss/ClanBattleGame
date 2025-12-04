using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClanBattleGame.Service
{
    public class BattleEngine
    {
        // -------------------------------
        //         ІНІЦІАЛІЗАЦІЯ БОЮ
        // -------------------------------
        public BattleState Initialize(Clan clanA, Clan clanB)
        {
            var state = new BattleState
            {
                ClanA = clanA,
                ClanB = clanB,

                QueueA = new Queue<Squad>(clanA.Squads),
                QueueB = new Queue<Squad>(clanB.Squads),

                IsFinished = false
            };

            state.CurrentA = state.QueueA.Dequeue();
            state.CurrentB = state.QueueB.Dequeue();

            return state;
        }


        // ----------------------------------------------------------
        //     ПЕРЕВІРКА СМЕРТІ ЛІДЕРА (ЗАВЕРШУЄ БІЙ МИТТЄВО)
        // ----------------------------------------------------------
        private bool CheckLeaderDeath(Clan clan)
        {
            if (clan == null)
                return false;

            if (clan.Leader == null)
                return false;

            return clan.Leader.Health <= 0;
        }


        // -------------------------------
        //          РАУНД БОЮ
        // -------------------------------
        public string Step(BattleState state)
        {
            if (state.IsFinished)
                return "Бій завершено.";

            StringBuilder log = new StringBuilder();

            log.AppendLine($"=== РАУНД ===");
            log.AppendLine($"{state.CurrentA.Name} vs {state.CurrentB.Name}");


            // Один раунд
            FightRound(state.CurrentA, state.CurrentB, log);


            // --- Перевірка смерті лідера ---
            if (CheckLeaderDeath(state.ClanA))
            {
                state.IsFinished = true;
                log.AppendLine("‼ Лідер Клану A загинув!");
                log.AppendLine("Переможець: Clan B");
                return log.ToString();
            }

            if (CheckLeaderDeath(state.ClanB))
            {
                state.IsFinished = true;
                log.AppendLine("‼ Лідер Клану B загинув!");
                log.AppendLine("Переможець: Clan A");
                return log.ToString();
            }


            // --- Перевірка вибуття загонів ---
            if (!state.CurrentA.Units.Any(u => u.Health > 0))
            {
                log.AppendLine($"Загін {state.CurrentA.Name} програв!");

                if (state.QueueA.Count == 0)
                {
                    state.IsFinished = true;
                    log.AppendLine("Переможець: Clan B");
                }
                else
                {
                    state.CurrentA = state.QueueA.Dequeue();
                    log.AppendLine($"Новий загін A: {state.CurrentA.Name}");
                }
            }

            if (!state.CurrentB.Units.Any(u => u.Health > 0))
            {
                log.AppendLine($"Загін {state.CurrentB.Name} програв!");

                if (state.QueueB.Count == 0)
                {
                    state.IsFinished = true;
                    log.AppendLine("Переможець: Clan A");
                }
                else
                {
                    state.CurrentB = state.QueueB.Dequeue();
                    log.AppendLine($"Новий загін B: {state.CurrentB.Name}");
                }
            }

            return log.ToString();
        }


        // ----------------------------------------
        //      ПОВНИЙ БІЙ ДО СМЕРТІ ЗАГОНУ
        // ----------------------------------------
        public string FightSquadFully(BattleState state)
        {
            if (state.IsFinished)
                return "Бій завершено.";

            var A = state.CurrentA;
            var B = state.CurrentB;

            StringBuilder log = new StringBuilder();
            log.AppendLine($"=== ПОВНИЙ БІЙ {A.Name} vs {B.Name} ===");


            while (A.Units.Any(u => u.Health > 0) &&
                   B.Units.Any(u => u.Health > 0))
            {
                FightRound(A, B, log);
                log.AppendLine("— Кінець раунду —");


                // --- Перевірка смерті лідера ---
                if (CheckLeaderDeath(state.ClanA))
                {
                    state.IsFinished = true;
                    log.AppendLine("‼ Лідер Клану A загинув!");
                    log.AppendLine("Переможець: Clan B");
                    return log.ToString();
                }

                if (CheckLeaderDeath(state.ClanB))
                {
                    state.IsFinished = true;
                    log.AppendLine("‼ Лідер Клану B загинув!");
                    log.AppendLine("Переможець: Clan A");
                    return log.ToString();
                }
            }


            // --- Хтось програв ---
            if (!A.Units.Any(u => u.Health > 0))
            {
                log.AppendLine($"Загін {A.Name} програв!");

                if (state.QueueA.Count == 0)
                {
                    state.IsFinished = true;
                    log.AppendLine("Переможець клану: Clan B");
                }
                else
                {
                    state.CurrentA = state.QueueA.Dequeue();
                    log.AppendLine($"Новий загін A: {state.CurrentA.Name}");
                }

                return log.ToString();
            }

            log.AppendLine($"Загін {B.Name} програв!");

            if (state.QueueB.Count == 0)
            {
                state.IsFinished = true;
                log.AppendLine("Переможець клану: Clan A");
            }
            else
            {
                state.CurrentB = state.QueueB.Dequeue();
                log.AppendLine($"Новий загін B: {state.CurrentB.Name}");
            }

            return log.ToString();
        }


        // ----------------------------------------
        //         ОДИН РАУНД БІЙЦІВ
        // ----------------------------------------
        private void FightRound(Squad a, Squad b, StringBuilder log)
        {
            var aliveA = a.Units.Where(u => u.Health > 0).ToList();
            var aliveB = b.Units.Where(u => u.Health > 0).ToList();

            if (aliveA.Count == 0 || aliveB.Count == 0)
                return;

            int max = aliveA.Count > aliveB.Count ? aliveA.Count : aliveB.Count;

            for (int i = 0; i < max; i++)
            {
                aliveA = a.Units.Where(u => u.Health > 0).ToList();
                aliveB = b.Units.Where(u => u.Health > 0).ToList();

                if (aliveA.Count == 0 || aliveB.Count == 0)
                    return;

                IUnit A = aliveA[i % aliveA.Count];
                IUnit B = aliveB[i % aliveB.Count];


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
