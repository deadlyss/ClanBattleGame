namespace ClanBattleGame.Service
{
    public static class ShopEconomic
    {
        // ============================
        //  BASE COSTS (початкова ціна)
        // ============================

        public const int UpgradeATKBase = 10;
        public const int UpgradeHPBase = 15;

        public const int AddArcherBase = 20;
        public const int AddLightBase = 25;
        public const int AddHeavyBase = 30;

        public const int AddSquadBase = 50;

        // ============================
        //  PRICE INCREASE (модифікатори)
        // ============================

        public const int UpgradeATKIncrease = 5;
        public const int UpgradeHPIncrease = 5;

        public const int AddUnitIncrease = 2; // +2 до ціни після кожного юніта
        public const int AddSquadIncrease = 10; // +10 після кожного загону
    }
}
