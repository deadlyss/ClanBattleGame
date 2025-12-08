using ClanBattleGame.Core;
using ClanBattleGame.Factories;
using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using ClanBattleGame.Service;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ClanBattleGame.ViewModel
{
    public class NewGameVM : ObservableObject
    {
        private string _clanName;
        public string ClanName
        {
            get => _clanName;
            set
            {
                _clanName = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _leaderName;
        public string LeaderName
        {
            get => _leaderName;
            set
            {
                _leaderName = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public Array RaceList => Enum.GetValues(typeof(Race));

        private Race _selectedRace;
        public Race SelectedRace
        {
            get => _selectedRace;
            set
            {
                _selectedRace = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand Back { get; }
        public RelayCommand Next { get; }

        public NewGameVM(NavigationStore navStore)
        {
            SelectedRace = Race.Elf; // значення за замовчуванням

            Back = new RelayCommand(_ =>
            {
                new NavigationService(navStore, () => new MainMenuVM(navStore)).Navigate();
            });

            Next = new RelayCommand(
    _ =>
    {
        var state = GameStateService.Instance;

        // 1) Встановлюємо базові дані
        string clanName = ClanName;
        Race race = SelectedRace;
        string leaderName = LeaderName;

        // 2) Вибираємо фабрику за расою
        IClanFactory factory =
            race == Race.Elf
                ? (IClanFactory)new ElfFactory()
                : new DwarfFactory();

        // 3) Запускаємо нову гру через централізований метод
        state.StartNewGame(clanName, race, leaderName, factory);

        // 4) Переходимо додому
        new NavigationService(navStore, () => new HomeVM(navStore)).Navigate();
    },
    _ => IsValidName(ClanName) && IsValidName(LeaderName)
);

        }

        private bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            return name.Length > 3 && Regex.IsMatch(name, @"^[A-Za-zА-Яа-яІіЇїЄєҐґ]+$");
        }
    }
}
