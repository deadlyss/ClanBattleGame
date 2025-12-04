using ClanBattleGame.Interface;

namespace ClanBattleGame.Model.Units
{
    public class Warrior : IWarrior
    {
        public string Name => "Warrior";
        public int Health { get; set; } = 100;
        public int Attack => 22;
        public string Weapon => "Steel Sword";
        public string Movement => "Shield Wall";

        public IWarrior Clone()
        {
            return new Warrior { Health = this.Health };
        }
    }
}
