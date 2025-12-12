using System.Collections.Generic;

namespace ClanBattleGame.MementoNew
{
    public class BackupManager<T>
    {
        private readonly Stack<Snapshot<T>> _snapshots = new Stack<Snapshot<T>>();

        public void Save(IDataBackup<T> obj)
        {
            if (obj == null) return;
            _snapshots.Push(obj.Save());
        }

        public void Load(IDataBackup<T> obj)
        {
            if (obj == null || _snapshots.Count == 0) return;
            obj.Load(_snapshots.Pop());
        }
    }
}