using ClanBattleGame.Interface;

namespace ClanBattleGame.Strategy
{
    public class HealthUpgradeStrategy : IUpgradeStrategy
    {
        public void Apply(IUnit unit)
        {
            unit.BonusHealth += 10;
        }

        public void Revert(IUnit unit)
        {
            if (unit.BonusHealth >= 10)
                unit.BonusHealth -= 10;
        }
    }
}
