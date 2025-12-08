using ClanBattleGame.Model.Etc;
using System.Linq;
using System.Text;

namespace ClanBattleGame.Service.Printers
{
    public class SquadPrinter
    {
        public string PrintSquadInfo(Squad squad, bool isPlayer)
        {
            if (squad == null)
                return "Порожня клітинка.";

            var sb = new StringBuilder();

            // Заголовок
            sb.AppendLine(isPlayer
                ? $"[Твій загін] {squad.Name}"
                : $"[Ворожий загін] {squad.Name}");

            // Загальна інформація
            int totalHP = squad.Units.Sum(u => u.TotalHealth);
            int currentHP = squad.Units.Sum(u => u.CurrentHealth);

            sb.AppendLine($"Юнітів: {squad.Units.Count}");
            sb.AppendLine($"HP: {currentHP}/{totalHP}");

            // --------------------------------------
            //   Якщо загін ворога → більше нічого
            // --------------------------------------
            if (!isPlayer)
                return sb.ToString();

            // --------------------------------------
            //   Деталі лише для загону гравця
            // --------------------------------------
            sb.AppendLine("--------------------------------");

            foreach (var u in squad.Units)
            {
                sb.AppendLine($"{u.Name} ({u.Type})");
                sb.AppendLine($"HP: {u.CurrentHealth}/{u.TotalHealth}   Atk: {u.TotalAttack}");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
