namespace ClanBattleGame.Interface
{
    public interface IUnit
    {
        string Name { get; }
        string Type { get; }
        int Health { get; }
        int CurrentHealth { get; set; }
        int Attack { get; }

        int TotalAttack { get; }
        int TotalHealth { get; }
        bool IsDead { get; }

        IUnit DeepCopy();
    }
}
