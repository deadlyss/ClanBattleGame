namespace ClanBattleGame.Interface
{
    public interface IUnit
    {
        string Name { get; }
        string Type { get; }
        int Health { get; }
        int Attack { get; }
        string Weapon { get; }
        int BonusAttack { get; set; }
        int BonusHealth { get; set; }

        int CurrentHealth { get; set; }
        int TotalAttack { get; }
        int TotalHealth { get; }
        IUnit Clone(); // Prototype
        IUnit CloneWithName(string newName);
    }
}
