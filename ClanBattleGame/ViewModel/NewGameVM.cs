using ClanBattleGame.Core;
using ClanBattleGame.Factories;
using ClanBattleGame.Interface;
using ClanBattleGame.Service;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ClanBattleGame.ViewModel
{
    public class NewGameVM : ObservableObject
    {
        private string _clanName = "Greek"; // значення за замовчуванням!
        public string ClanName
        {
            get => _clanName;
            set
            {
                if (Set(ref _clanName, value))
                    CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _leaderName = "Adolf"; // значення за замовчуванням!
        public string LeaderName
        {
            get => _leaderName;
            set
            {
                if (Set(ref _leaderName, value))
                    CommandManager.InvalidateRequerySuggested();
            }
        }

        public Array RaceList => Enum.GetValues(typeof(Race));

        private Race _selectedRace = Race.Elf; // значення за замовчуванням
        public Race SelectedRace
        {
            get => _selectedRace;
            set => Set(ref _selectedRace, value);
        }

        public RelayCommand Back { get; }
        public RelayCommand Next { get; }

        public NewGameVM(NavigationStore navStore)
        {
            Back = new RelayCommand(o =>
            {
                new NavigationService(navStore, () => new MainMenuVM(navStore)).Navigate();
            });

            Next = new RelayCommand(o =>
    {
        var state = GameState.Instance;

        // 1) Встановлюємо базові дані
        string clanName = ClanName;
        Race race = SelectedRace;
        string leaderName = LeaderName;

        // 2) Вибираємо фабрику за расою
        IClanFactory factory = race == Race.Elf
                ? (IClanFactory)new ElfFactory() : new DwarfFactory();

        // 3) Запускаємо нову гру через централізований метод
        state.StartNewGame(clanName, leaderName, factory);

        // 4) Переходимо додому
        new NavigationService(navStore, () => new HomeVM(navStore)).Navigate();
    },
    o => IsValid(ClanName) && IsValid(LeaderName)
);

        }

        private bool IsValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            return name.Length > 3 && Regex.IsMatch(name, @"^[A-Za-zА-Яа-яІіЇїЄєҐґ]+$");
        }
    }
}
