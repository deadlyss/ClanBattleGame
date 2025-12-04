using ClanBattleGame.Interface;
using System.Collections.Generic;

namespace ClanBattleGame.Model
{
    public class Squad
    {
        public string Name { get; set; }
        public int FrontlinePosition { get; set; }
        public List<IWarrior> Members { get; set; } = new List<IWarrior>();
    }
}