using ClanBattleGame.Interface;

namespace ClanBattleGame.Strategy
{
    public interface IUpgradeStrategy
    {
        void Apply(IUnit unit);
        void Revert(IUnit unit); // для кнопки "-"
    }
}
