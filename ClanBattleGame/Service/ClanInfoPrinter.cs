using ClanBattleGame.Model;
using System.Text;

namespace ClanBattleGame.Service
{
    public static class ClanInfoPrinter
    {
        public static string PrintInfo(Clan clan)
        {
            if (clan == null)
                return "Клан ще не створений.";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"=== КЛАН {clan.Name} ({clan.Race}) ===");
            sb.AppendLine($"Лідер: HP={clan.Leader.CurrentHealth}/{clan.Leader.TotalHealth}");
            sb.AppendLine();

            int squadIndex = 1;

            foreach (var squad in clan.Squads)
            {
                sb.AppendLine($"Загін {squadIndex++}: {squad.Name}");
                sb.AppendLine($" - Юнітів: {squad.Units.Count}");
                sb.AppendLine($" - Total HP: {squad.TotalHP}");
                sb.AppendLine($" - Total Attack: {squad.TotalAttack}");
                sb.AppendLine($"   Юніти:");

                foreach (var unit in squad.Units)
                {
                    sb.AppendLine(
                        $"     • {unit.Name} | HP: {unit.CurrentHealth}/{unit.TotalHealth} | ATK: {unit.TotalAttack}");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
