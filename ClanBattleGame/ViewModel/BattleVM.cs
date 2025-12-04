using ClanBattleGame.Model.Etc;
using ClanBattleGame.Core;
using ClanBattleGame.Factories;
using ClanBattleGame.Service;
using ClanBattleGame.Model.Units;
using ClanBattleGame.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ClanBattleGame.Interface;
using ClanBattleGame.Bridge;
using System;
using ClanBattleGame.Memento;

namespace ClanBattleGame.ViewModel
{
    public class BattleVM : ObservableObject
    {
        private readonly BattleEngine _engine = new BattleEngine();

        public Clan ClanA { get; }
        public Clan ClanB { get; }

        public ObservableCollection<Squad> SquadsA { get; }
        public ObservableCollection<Squad> SquadsB { get; }

        public BattleState State { get; private set; }

        public BattleHistory History { get; } = new BattleHistory();

        private string _battleLog = "";
        public string BattleLog
        {
            get => _battleLog;
            set { _battleLog = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand RestoreCommand { get; }
        public ICommand RoundCommand { get; }
        public ICommand FullBattleCommand { get; }

        public BattleVM()
        {
            ClanBuilder builderA = new ClanBuilder(new ElfFactory());
            ClanBuilder builderB = new ClanBuilder(new DwarfFactory());

            ClanA = builderA.Build("Elven Clan");
            ClanB = builderB.Build("Dwarven Clan");

            // ==============================
            //   ВИБІР ЛІДЕРА З ОБОХ КЛАНІВ
            // ==============================
            var allUnits = ClanA.GetAllUnits().Concat(ClanB.GetAllUnits()).ToList();

            Random rnd = new Random();
            IUnit chosenLeader = allUnits[rnd.Next(allUnits.Count)];

            Leader.Create(chosenLeader);

            // визначаємо, до кого він належить
            if (ClanA.GetAllUnits().Contains(chosenLeader))
            {
                ClanA.Leader = chosenLeader;
                builderA.ApplyLeaderBonuses(ClanA);
            }
            else
            {
                ClanB.Leader = chosenLeader;
                builderB.ApplyLeaderBonuses(ClanB);
            }

            // Колекції для UI
            SquadsA = new ObservableCollection<Squad>(ClanA.Squads);
            SquadsB = new ObservableCollection<Squad>(ClanB.Squads);

            // Ініціалізація бою
            State = _engine.Initialize(ClanA, ClanB);

            RoundCommand = new RelayCommand(o => DoRound());
            FullBattleCommand = new RelayCommand(o => DoFullBattle());

            var formatter = new TextClanFormatter();
            var printer = new ClanInfoPrinter(formatter);

            BattleLog += "=== Інформація про Клан A ===\n";
            BattleLog += printer.PrintClan(ClanA) + "\n\n";

            BattleLog += "=== Інформація про Клан B ===\n";
            BattleLog += printer.PrintClan(ClanB) + "\n\n";

            SaveCommand = new RelayCommand(o => SaveCheckpoint());
            RestoreCommand = new RelayCommand(o => LoadCheckpoint());
        }

        private void DoRound()
        {
            string log = _engine.Step(State);
            BattleLog += log + "\n";
        }

        private void DoFullBattle()
        {
            string log = _engine.FightSquadFully(State);
            BattleLog += log + "\n";
        }

        private void SaveCheckpoint()
        {
            var memento = new BattleMemento(ClanA, ClanB, State, Leader.Instance.Unit.Name);
            History.Save(memento);
            BattleLog += "Контрольна точка збережена.\n";
        }

        private void LoadCheckpoint()
        {
            var memento = History.Restore();
            if (memento == null)
            {
                BattleLog += "Немає збережених контрольних точок.\n";
                return;
            }

            ClanA.Squads.Clear();
            foreach (var s in memento.SavedClanA.Squads)
                ClanA.Squads.Add(s);

            ClanB.Squads.Clear();
            foreach (var s in memento.SavedClanB.Squads)
                ClanB.Squads.Add(s);

            State = memento.SavedState.DeepCopy();

            var allUnits = ClanA.GetAllUnits().Concat(ClanB.GetAllUnits()).ToList();
            var restoredLeader = allUnits.FirstOrDefault(u => u.Name == memento.LeaderName);

            Leader.Reset();
            Leader.Create(restoredLeader);

            ClanA.Leader = ClanA.GetAllUnits().Contains(restoredLeader) ? restoredLeader : null;
            ClanB.Leader = ClanB.GetAllUnits().Contains(restoredLeader) ? restoredLeader : null;

            OnPropertyChanged(nameof(ClanA));
            OnPropertyChanged(nameof(ClanB));
            OnPropertyChanged(nameof(State));

            BattleLog += "Стан гри відновлено!\n";
        }
    }
}
