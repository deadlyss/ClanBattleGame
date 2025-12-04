using ClanBattleGame.Interface;
using System.Collections.Generic;

namespace ClanBattleGame.Model
{
    public class Clan
    {
        public string Name { get; set; }
        public List<IWarrior> Units { get; set; } = new List<IWarrior>();
    }
}
