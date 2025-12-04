using ClanBattleGame.Core;
using ClanBattleGame.Factories;
using ClanBattleGame.Model;
using System.Data.Odbc;
using System.Windows.Input;

namespace ClanBattleGame.ViewModel
{
    public class GameVM : ObservableObject
    {
        private string _log;
        public string Log
        {
            get => _log;
            set { _log = value; OnPropertyChanged(); }
        }

        public ICommand NextTurnCommand { get; }

        private BattleEngine _engine;

        public GameVM()
        {
            // Створюємо кланів через фабрики
            var elfFactory = new ElfFactory();
            //var orcFactory = new OrcFactory();

            var clan1 = new Clan { Name = "Ельфи" };
            clan1.Units.Add(elfFactory.CreateLightUnit());
            clan1.Units.Add(elfFactory.CreateHeavyUnit());

            var clan2 = new Clan { Name = "Ельфи2" };
            clan2.Units.Add(elfFactory.CreateLightUnit());
            clan2.Units.Add(elfFactory.CreateHeavyUnit());

            _engine = new BattleEngine(clan1, clan2);

            NextTurnCommand = new RelayCommand(o =>
            {
                Log += _engine.NextTurn() + "\n";
            });
        }
    }
}
