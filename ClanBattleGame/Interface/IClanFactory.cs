using ClanBattleGame.Model;

namespace ClanBattleGame.Interface
{
    public interface IClanFactory
    {
        Race Race { get; }

        IUnit CreateLightUnit();
        IUnit CreateHeavyUnit();
        IUnit CreateArcherUnit();
    }
}
