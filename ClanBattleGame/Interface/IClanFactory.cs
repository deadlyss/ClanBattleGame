using ClanBattleGame.Model.Etc;

namespace ClanBattleGame.Interface
{
    public interface IClanFactory
    {
        Race Race { get; }

        IUnit CreateArcherUnit();
    }
}
