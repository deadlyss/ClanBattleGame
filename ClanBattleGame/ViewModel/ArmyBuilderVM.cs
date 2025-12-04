using ClanBattleGame.Core;
using ClanBattleGame.Factories;
using ClanBattleGame.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanBattleGame.ViewModel
{
    public class ArmyBuilderVM : ObservableObject
    {
        // Фабрики для кожної армії
        private IClanFactory _factory1;
        private IClanFactory _factory2;

        // Списки армій
        public ObservableCollection<IWarrior> Army1 { get; set; }
        public ObservableCollection<IWarrior> Army2 { get; set; }

        // Доступні типи юнітів
        public List<string> UnitTypes { get; } = new()
        {
            "Light",
            "Heavy",
            "Elite"
        };

        // Вибрані типи
        private string _selectedUnitType1;
        public string SelectedUnitType1
        {
            get => _selectedUnitType1;
            set { _selectedUnitType1 = value; OnPropertyChanged(); }
        }

        private string _selectedUnitType2;
        public string SelectedUnitType2
        {
            get => _selectedUnitType2;
            set { _selectedUnitType2 = value; OnPropertyChanged(); }
        }

        // Команди
        public RelayCommand AddUnit1Command { get; }
        public RelayCommand AddUnit2Command { get; }
        public RelayCommand CloneUnit1Command { get; }
        public RelayCommand CloneUnit2Command { get; }
        public RelayCommand RemoveUnit1Command { get; }
        public RelayCommand RemoveUnit2Command { get; }

        // ------------------------------------------------------------

        public ArmyBuilderVM()
        {
            // Поки фабрики 2 — Elf & Dwarf
            _factory1 = new ElfFactory();
            _factory2 = new DwarfFactory();

            // Колекції армій
            Army1 = new ObservableCollection<IWarrior>();
            Army2 = new ObservableCollection<IWarrior>();

            // Команди
            AddUnit1Command = new RelayCommand(o => AddUnit(Army1, SelectedUnitType1, _factory1));
            AddUnit2Command = new RelayCommand(o => AddUnit(Army2, SelectedUnitType2, _factory2));

            CloneUnit1Command = new RelayCommand(o => CloneUnit(Army1));
            CloneUnit2Command = new RelayCommand(o => CloneUnit(Army2));

            RemoveUnit1Command = new RelayCommand(o => RemoveUnit(Army1));
            RemoveUnit2Command = new RelayCommand(o => RemoveUnit(Army2));
        }

        // ------------------------------------------------------------
        // Додати юніта
        private void AddUnit(ObservableCollection<IWarrior> army, string type, IClanFactory factory)
        {
            if (type == "Light") army.Add(factory.CreateLightUnit());
            if (type == "Heavy") army.Add(factory.CreateHeavyUnit());
            if (type == "Elite") army.Add(factory.CreateEliteUnit());
        }

        // Клонувати (Prototype)
        private void CloneUnit(ObservableCollection<IWarrior> army)
        {
            if (army.Count == 0) return;
            var clone = army.Last().Clone();
            army.Add(clone);
        }

        // Видалити останнього
        private void RemoveUnit(ObservableCollection<IWarrior> army)
        {
            if (army.Count == 0) return;
            army.RemoveAt(army.Count - 1);
        }
    }
}
