using ClanBattleGame.Interface;

namespace ClanBattleGame.Model.Units
{
    public class DwarfWarrior : IWarrior
    {
        public string Name => "Dwarf Warrior";

        public int Health { get; set; } = 140;
        public int Attack => 25;

        public IWarrior Clone()
        {
            return new DwarfWarrior { Health = this.Health };
        }
    }
}
