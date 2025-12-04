using ClanBattleGame.Interface;

namespace ClanBattleGame.Model.Units
{
    public class Archer : IWarrior
    {
        public string Name => "Archer";
        public int Health { get; set; } = 80;
        public int Attack => 20;

        public IWarrior Clone()
            => new Archer { Health = this.Health };
    }
}
