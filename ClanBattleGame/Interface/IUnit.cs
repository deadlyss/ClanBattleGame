namespace ClanBattleGame.Interface
{
    public interface IUnit
    {
        string Name { get; }
        string Type { get; }
        int Health { get; }
        int Attack { get; }

        int CurrentHealth { get; set; }
        int TotalAttack { get; }
        int TotalHealth { get; }

        IUnit DeepCopy();
    }
}
