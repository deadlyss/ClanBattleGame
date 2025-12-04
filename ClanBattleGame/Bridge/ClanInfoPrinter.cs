using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using System.Text;

namespace ClanBattleGame.Bridge
{
    public class ClanInfoPrinter
    {
        protected IClanFormatter formatter;

        public ClanInfoPrinter(IClanFormatter formatter)
        {
            this.formatter = formatter;
        }

        public string PrintClan(Clan clan)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(formatter.FormatClan(clan));

            foreach (var squad in clan.Squads)
            {
                sb.AppendLine(formatter.FormatSquad(squad));

                foreach (var unit in squad.Units)
                    sb.AppendLine(formatter.FormatUnit(unit));
            }

            return sb.ToString();
        }
    }
}
