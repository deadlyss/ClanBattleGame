using ClanBattleGame.Model.Etc;

namespace ClanBattleGame.Interface
{
    public interface ICombatStrategy
    {
        void Execute(Squad attacker, Squad defender);
    }
}
