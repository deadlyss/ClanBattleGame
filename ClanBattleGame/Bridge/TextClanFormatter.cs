using ClanBattleGame.Interface;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Model;

namespace ClanBattleGame.Bridge
{
    public class TextClanFormatter : IClanFormatter
    {
        public string FormatClan(Clan clan)
        {
            return $"Клан: {clan.Name}, Загонів: {clan.Squads.Count}";
        }

        public string FormatSquad(Squad squad)
        {
            return $"  {squad.Name}, Юнітів: {squad.Units.Count}";
        }

        public string FormatUnit(IUnit unit)
        {
            return $"    {unit.Name} ({unit.Type}) HP={unit.Health} ATK={unit.Attack}";
        }
    }
}
