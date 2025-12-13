using ClanBattleGame.Factories;

namespace ClanBattleGame.Interface
{
    public interface IClanFactory
    {
        Race Race { get; }

        IUnit CreateArcherUnit();
        IUnit CreateHeavyUnit();
    }
}
