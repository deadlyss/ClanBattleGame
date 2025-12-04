namespace ClanBattleGame.Interface
{
    public interface IWarrior
    {
        string Name { get; }
        int Health { get; set; }
        int Attack { get; }
        string Weapon { get; }
        string Movement { get; }
        IWarrior Clone(); // Prototype
    }
}
