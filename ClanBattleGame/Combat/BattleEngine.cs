
using ClanBattleGame.Combat;
using ClanBattleGame.Interface;
using ClanBattleGame.Model.Etc;
using System.Collections.Generic;

namespace ClanBattleGame.Service
{
    public class BattleEngine
    {
        private readonly List<ICombatStrategy> _strategies;

        public BattleEngine(IBattleLogger logger)
        {
            _strategies = new List<ICombatStrategy>
        {
            new HeavyCombatStrategy(logger),
            new ArcherCombatStrategy(logger)
        };
        }

        public void Fight(Squad squadA, Squad squadB)
        {
            foreach (var strategy in _strategies)
            {
                // A атакує B
                strategy.Execute(squadA, squadB);
                RemoveDead(squadB);
                RemoveDead(squadA);

                //// B атакує A
                //strategy.Execute(squadB, squadA);
                //RemoveDead(squadA);
            }
        }

        private void RemoveDead(Squad squad)
        {
            for (int i = squad.Units.Count - 1; i >= 0; i--)
            {
                if (squad.Units[i].IsDead)
                    squad.Units.RemoveAt(i);
            }
        }
    }
}
