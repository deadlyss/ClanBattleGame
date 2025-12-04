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
        public Queue<Squad> QueueA;
        public Queue<Squad> QueueB;

        public Squad CurrentA;
        public Squad CurrentB;

        public Clan ClanA;
        public Clan ClanB;

        public bool IsFinished;

        public BattleState DeepCopy()
        {
            return new BattleState
            {
                QueueA = new Queue<Squad>(this.QueueA.Select(s => s.DeepCopy())),
                QueueB = new Queue<Squad>(this.QueueB.Select(s => s.DeepCopy())),
                CurrentA = this.CurrentA.DeepCopy(),
                CurrentB = this.CurrentB.DeepCopy(),
                IsFinished = this.IsFinished
            };
        }
    }
}
