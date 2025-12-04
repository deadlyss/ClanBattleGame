namespace ClanBattleGame.Interface
{
    public interface IUnit
    {
        string Name { get; }
        string Type { get; }
        int Health { get; set; }
        int Attack { get; }
        string Weapon { get; }
        int BonusAttack { get; set; }
        IUnit Clone(); // Prototype
        IUnit CloneWithName(string newName);
    }
}
