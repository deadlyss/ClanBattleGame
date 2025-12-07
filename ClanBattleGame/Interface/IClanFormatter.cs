using ClanBattleGame.Model.Etc;
using ClanBattleGame.Model;

namespace ClanBattleGame.Interface
{
    public interface IClanFormatter
    {
        string FormatClan(Clan clan); //Bridge
        string FormatSquad(Squad squad);
        string FormatUnit(IUnit unit);
    }
}
