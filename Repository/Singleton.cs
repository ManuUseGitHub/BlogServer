namespace Repository
{
    public class Singleton<T> where T : new()
    {
        protected Singleton() { }
        public static T Instance { get { return (T)Nested.instance; } }

        #region Singleton stuffs
        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly T instance = new T();
        }
        #endregion
    }
}
