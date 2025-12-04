using ClanBattleGame.Interface;
using ClanBattleGame.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanBattleGame.Factories
{
    public class ElfFactory : IClanFactory
    {
        public IWarrior CreateLightUnit() => new Archer();
        public IWarrior CreateHeavyUnit() => new Knight();
        public IWarrior CreateEliteUnit() => new Mage();
    }
}
