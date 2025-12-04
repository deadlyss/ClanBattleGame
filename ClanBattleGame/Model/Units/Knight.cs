using ClanBattleGame.Interface;

namespace ClanBattleGame.Model.Units
{
    public class Knight : IWarrior
    {
        public string Name => "Knight";
        public int Health { get; set; } = 80;
        public int Attack => 20;

        public IWarrior Clone()
            => new Knight { Health = this.Health };
    }
}
