using ClanBattleGame.Model.Etc;
using ClanBattleGame.Model;

namespace ClanBattleGame.Interface
{
    public interface IClanFormatter
    {
        string FormatClan(Clan clan);
        string FormatSquad(Squad squad);
        string FormatUnit(IUnit unit);
    }
}
