using ClanBattleGame.Interface;

namespace ClanBattleGame.Model.Units
{
    public class DwarfBerserk : IWarrior
    {
        public string Name => "Dwarf Berserk";

        public int Health { get; set; } = 110;
        public int Attack => 35;
        public string Weapon => "Dual Axes";
        public string Movement => "Frenzied Dash";

        public IWarrior Clone()
        {
            return new DwarfBerserk { Health = this.Health };
        }
    }
}
