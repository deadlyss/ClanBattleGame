namespace ClanBattleGame.Interface
{
    public interface IWarrior
    {
        string Name { get; }
        int Health { get; set; }
        int Attack { get; }
        IWarrior Clone(); // Prototype
    }
}
