using ClanBattleGame.Interface;

namespace ClanBattleGame.Strategy
{
    public class AttackUpgradeStrategy : IUpgradeStrategy
    {
        public void Apply(IUnit unit)
        {
            unit.BonusAttack += 1;
        }

        public void Revert(IUnit unit)
        {
            if (unit.BonusAttack > 0)
                unit.BonusAttack -= 1;
        }
    }
}
