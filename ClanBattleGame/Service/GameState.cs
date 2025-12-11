using ClanBattleGame.Factories;
using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Units;
using System;

namespace ClanBattleGame.Service
{
    public sealed class GameState
    {
        private static readonly Lazy<GameState> _instance =
            new Lazy<GameState>(() => new GameState());

        public static GameState Instance => _instance.Value;

        private GameState() { }

        public Clan CurrentClan { get; private set; }

        public void StartNewGame(string clanName, string leaderName, IClanFactory factory)
        {
            //Створюємо будівельник, раса передається через IClanFactory
            ClanBuilder builder = new ClanBuilder(factory);

            //Створюємо лідера
            Leader leader = new Leader(leaderName, 100, 20);

            //Будуємо клан із юнітами та загонами
            Clan clan = builder.CreateClan(clanName, leader);

            //Зберігаємо клан як поточний стан гри
            CurrentClan = clan;
        }
    }
}
