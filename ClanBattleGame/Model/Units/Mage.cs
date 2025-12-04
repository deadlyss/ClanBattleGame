using ClanBattleGame.Interface;

namespace ClanBattleGame.Model.Units
{
    public class Mage : IWarrior
    {
        public string Name => "Mage";
        public int Health { get; set; } = 80;
        public int Attack => 20;
        public string Weapon => "Staff";
        public string Movement => "Teleport";

        public IWarrior Clone()
            => new Mage { Health = this.Health };
    }

}
