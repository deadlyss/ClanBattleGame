using ClanBattleGame.Core;
using ClanBattleGame.Factories;
using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Service;
using ClanBattleGame.Strategy;
using System.Collections.ObjectModel;

namespace ClanBattleGame.ViewModel
{
    public class HomeVM : ObservableObject
    {
        private readonly ShopService _shop;
        public IUnit Leader { get; set; }
        public int UpgradeATKCost { get; set; } = ShopEconomic.UpgradeATKBase;
        public int UpgradeHPCost { get; set; } = ShopEconomic.UpgradeHPBase;

        public int AddArcherCost { get; set; } = ShopEconomic.AddArcherBase;
        public int AddLightCost { get; set; } = ShopEconomic.AddLightBase;
        public int AddHeavyCost { get; set; } = ShopEconomic.AddHeavyBase;

        public int AddSquadCost { get; set; } = ShopEconomic.AddSquadBase;

        // -------------------------
        //       ПРОФІЛЬ КЛАНУ
        // -------------------------
        public string ClanName { get; set; }
        public Race Race { get; set; }
        public string LeaderName { get; set; }

        private int _money;
        public int Money
        {
            get => _money;
            set { _money = value; OnPropertyChanged(); }
        }

        public int Fights { get; set; }

        // -------------------------
        //        ЗАГОНИ
        // -------------------------
        private Squad _selectedSquad;
        public Squad SelectedSquad
        {
            get => _selectedSquad;
            set
            {
                _selectedSquad = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Units));
            }
        }

        private IUnit _selectedUnit;
        public IUnit SelectedUnit
        {
            get => _selectedUnit;
            set
            {
                _selectedUnit = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Squad> Squads { get; set; }

        // Юніти вибраного загону
        public ObservableCollection<IUnit> Units
        {
            get
            {
                if (SelectedSquad != null)
                    return SelectedSquad.Units;

                return new ObservableCollection<IUnit>();
            }
        }

        // -------------------------
        //         КОМАНДИ
        // -------------------------
        public RelayCommand AddArcherCommand { get; }
        public RelayCommand AddLightCommand { get; }
        public RelayCommand AddHeavyCommand { get; }

        public RelayCommand DeleteUnitCommand { get; }

        public RelayCommand AddSquadCommand { get; }
        public RelayCommand RemoveSquadCommand { get; }

        public RelayCommand GoFightCommand { get; }


        // -------------------------
        //   КОМАНДИ АПГРЕЙДУ ЮНІТІВ
        // -------------------------

        public RelayCommand UpgradeATK { get; }
        public RelayCommand DowngradeATK { get; }

        public RelayCommand UpgradeHP { get; }
        public RelayCommand DowngradeHP { get; }

        // -------------------------
        //        КОНСТРУКТОР
        // -------------------------
        public HomeVM(NavigationStore navStore)
        {
            var state = GameStateService.Instance;

            ClanName = state.ClanName;
            Race = state.Race;
            LeaderName = state.LeaderName;
            Money = state.Money;
            Fights = state.Fights;

            Squads = state.Squads;

            var leaderObj = Model.Units.Leader.Get(ClanName);
            if (leaderObj != null)
                Leader = leaderObj.Unit;

            // Фабрика залежно від раси
            IClanFactory factory;

            if (Race == Race.Elf)
                factory = new ElfFactory();
            else
                factory = new DwarfFactory();

            // Створюємо магазин
            _shop = new ShopService(factory);

            GoFightCommand = new RelayCommand(_ =>
            {
                var game = GameStateService.Instance;

                // 1) Зберігаємо Memento клана гравця
                game.History.Save(game.GetPlayerClan());

                // 2) Генеруємо ворога
                game.GenerateEnemyClan();

                // 3) Формуємо клан гравця (копія)
                Clan playerClan = game.GetPlayerClan();

                // 4) Переходимо в бій
                new NavigationService(navStore,
                    () => new BattleVM(navStore, playerClan, game.EnemyClan))
                .Navigate();
            });

            // -------------------------
            //    ДОДАВАННЯ БІЙЦІВ
            // -------------------------

            AddArcherCommand = new RelayCommand(_ =>
            {
                if (Money < AddArcherCost)
                    return;

                if (_shop.AddUnit(SelectedSquad, UnitType.Archer))
                {
                    Money -= AddArcherCost;
                    AddArcherCost += ShopEconomic.AddUnitIncrease;

                    OnPropertyChanged(nameof(Units));
                    OnPropertyChanged(nameof(AddArcherCost));
                }
            });

            AddLightCommand = new RelayCommand(_ =>
            {
                if (Money < AddLightCost)
                    return;

                if (_shop.AddUnit(SelectedSquad, UnitType.Light))
                {
                    Money -= AddLightCost;
                    AddLightCost += ShopEconomic.AddUnitIncrease;

                    OnPropertyChanged(nameof(Units));
                    OnPropertyChanged(nameof(AddLightCost));
                }
            });

            AddHeavyCommand = new RelayCommand(_ =>
            {
                if (Money < AddHeavyCost)
                    return;

                if (_shop.AddUnit(SelectedSquad, UnitType.Heavy))
                {
                    Money -= AddHeavyCost;
                    AddHeavyCost += ShopEconomic.AddUnitIncrease;

                    OnPropertyChanged(nameof(Units));
                    OnPropertyChanged(nameof(AddHeavyCost));
                }
            });

            DeleteUnitCommand = new RelayCommand(_ =>
            {
                if (SelectedUnit == null || SelectedSquad == null)
                    return;

                //Не дозволяємо видаляти лідера
                if (ReferenceEquals(SelectedUnit, Leader))
                    return;

                SelectedSquad.Units.Remove(SelectedUnit);

                SelectedUnit = null;
                OnPropertyChanged(nameof(Units));
            });

            // -------------------------
            //     SQUAD UPGRADES
            // -------------------------

            AddSquadCommand = new RelayCommand(_ =>
            {
                if (Money < AddSquadCost)
                    return;

                _shop.AddSquad();
                Money -= AddSquadCost;
                AddSquadCost += ShopEconomic.AddSquadIncrease;

                OnPropertyChanged(nameof(Squads));
                OnPropertyChanged(nameof(AddSquadCost));
            });

            RemoveSquadCommand = new RelayCommand(_ =>
            {
                _shop.RemoveSquad(SelectedSquad);
                OnPropertyChanged(nameof(Squads));
            });

            // -------------------------
            //     UNIT UPGRADES
            // -------------------------

            UpgradeATK = new RelayCommand(_ =>
            {
                if (SelectedUnit == null || Money < UpgradeATKCost)
                    return;

                if (_shop.UpgradeUnit(SelectedUnit, new AttackUpgradeStrategy()))
                {
                    Money -= UpgradeATKCost;
                    UpgradeATKCost += ShopEconomic.UpgradeATKIncrease;

                    OnPropertyChanged(nameof(SelectedUnit));
                    OnPropertyChanged(nameof(UpgradeATKCost));
                }
            });

            DowngradeATK = new RelayCommand(_ =>
            {
                if (_shop.DowngradeUnit(SelectedUnit, new AttackUpgradeStrategy()))
                    OnPropertyChanged(nameof(SelectedUnit));
            });

            UpgradeHP = new RelayCommand(_ =>
            {
                if (SelectedUnit == null || Money < UpgradeHPCost)
                    return;

                if (_shop.UpgradeUnit(SelectedUnit, new HealthUpgradeStrategy()))
                {
                    Money -= UpgradeHPCost;
                    UpgradeHPCost += ShopEconomic.UpgradeHPIncrease;

                    OnPropertyChanged(nameof(SelectedUnit));
                    OnPropertyChanged(nameof(UpgradeHPCost));
                }
            });

            DowngradeHP = new RelayCommand(_ =>
            {
                if (_shop.DowngradeUnit(SelectedUnit, new HealthUpgradeStrategy()))
                    OnPropertyChanged(nameof(SelectedUnit));
            });
        }
    }
}
