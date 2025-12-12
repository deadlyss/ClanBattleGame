namespace ClanBattleGame.MementoNew
{
    public class Snapshot<T>
    {
        private readonly T _value;

        public Snapshot(T value)
        {
            _value = value;
        }

        public T Value { get; }
    }
}