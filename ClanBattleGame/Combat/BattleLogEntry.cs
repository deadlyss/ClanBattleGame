using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanBattleGame.Combat
{
    public interface IBattleLogger
    {
        void Log(BattleLogEntry entry);
    }

    public class BattleLogEntry
    {
        public string Attacker { get; set; }
        public string Defender { get; set; }

        public int Damage { get; set; }          // 0 = miss
        public int DefenderHpAfter { get; set; }

        public string Phase { get; set; }         // Heavy / Archer
        public string Result { get; set; }        // Hit / Miss / Skip
        public string Reason { get; set; }        // RNG fail, No target, etc.


    }

    public class ConsoleBattleLogger : IBattleLogger
    {
        public void Log(BattleLogEntry entry)
        {
            Console.WriteLine(
                $"[{entry.Phase}][{entry.Result ?? "Hit"}] " +
                $"{entry.Attacker} → {entry.Defender} | " +
                $"{entry.Damage} dmg | HP after: {entry.DefenderHpAfter}" +
                (string.IsNullOrEmpty(entry.Reason) ? "" : $" | {entry.Reason}")
            );
        }
    }
}
