using ClanBattleGame.Interface;

namespace ClanBattleGame.Model.Units
{
    public class DwarfMiner : IWarrior
    {
        public string Name => "Dwarf Miner";

        public int Health { get; set; } = 90;
        public int Attack => 15;
        public string Weapon => "Pickaxe";
        public string Movement => "Tunnel Crawl";

        public IWarrior Clone()
        {
            return new DwarfMiner { Health = this.Health };
        }
    }
}
