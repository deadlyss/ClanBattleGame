using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClanBattleGame.Service
{
    [Serializable]
    public class BattleState
    {
        public Clan ClanA { get; set; }
        public Clan ClanB { get; set; }
        public bool IsFinished { get; set; }
    }
}
