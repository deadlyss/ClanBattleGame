namespace ClanBattleGame.MementoNew
{
    public interface IDataBackup<T>
    {
        Snapshot<T> Save();
        void Load(Snapshot<T> snapshot);
    }
}