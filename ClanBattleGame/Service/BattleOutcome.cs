using System.Collections.Generic;

namespace ClanBattleGame.Service
{
    public class BattleOutcome
    {
        public BattleResult Result { get; set; }

        // Детальний лог бою
        public List<string> Reports { get; set; } = new List<string>();

        // Загальний текст (необов’язково)
        public string Log => string.Join("\n", Reports);
    }
}
