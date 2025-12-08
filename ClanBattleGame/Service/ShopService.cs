using ClanBattleGame.Interface;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Strategy;
using System.Linq;

namespace ClanBattleGame.Service
{
    public enum UnitType
    {
        Archer,
        Light,
        Heavy
    }

    public class ShopService
    {
        private readonly GameStateService _state;
        private readonly IClanFactory _factory;

        public ShopService(IClanFactory factory)
        {
            _state = GameStateService.Instance;
            _factory = factory;
        }

        // -----------------------
        //         ЦІНИ
        // -----------------------
        public int GetPrice(UnitType type)
        {
            switch (type)
            {
                case UnitType.Archer: return 10;
                case UnitType.Light: return 15;
                case UnitType.Heavy: return 25;
            }
            return 0;
        }

        public bool CanBuy(UnitType type)
        {
            return _state.Money >= GetPrice(type);
        }

        // -----------------------
        //  ДОДАТИ ЮНІТА В ЗАГІН
        // -----------------------
        public bool AddUnit(Squad squad, UnitType type)
        {
            if (squad == null || !CanBuy(type))
                return false;

            IUnit unit = null;

            switch (type)
            {
                case UnitType.Archer:
                    unit = _factory.CreateArcherUnit();
                    break;

                case UnitType.Light:
                    unit = _factory.CreateLightUnit();
                    break;

                case UnitType.Heavy:
                    unit = _factory.CreateHeavyUnit();
                    break;
            }

            squad.Units.Add(unit);
            _state.Money -= GetPrice(type);

            return true;
        }

        // -----------------------
        //  ВИДАЛИТИ ЮНІТА
        // -----------------------
        public bool RemoveUnit(Squad squad, UnitType type)
        {
            if (squad == null)
                return false;

            var unit = squad.Units.FirstOrDefault(u => u.Type == type.ToString());
            if (unit == null)
                return false;

            squad.Units.Remove(unit);
            return true;
        }

        // -----------------------
        //      ЗАГОНИ
        // -----------------------
        public void AddSquad()
        {
            _state.Squads.Add(new Squad("Новий загін"));
        }

        public void RemoveSquad(Squad squad)
        {
            if (squad != null)
                _state.Squads.Remove(squad);
        }

        public bool UpgradeUnit(IUnit unit, IUpgradeStrategy strategy)
        {
            if (unit == null)
                return false;

            strategy.Apply(unit);
            return true;
        }

        public bool DowngradeUnit(IUnit unit, IUpgradeStrategy strategy)
        {
            if (unit == null)
                return false;

            strategy.Revert(unit);
            return true;
        }
    }
}
