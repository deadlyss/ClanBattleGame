using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanBattleGame.Interface
{
    public interface IClanFactory
    {
        IWarrior CreateLightUnit();
        IWarrior CreateHeavyUnit();
        IWarrior CreateEliteUnit();
    }
}
