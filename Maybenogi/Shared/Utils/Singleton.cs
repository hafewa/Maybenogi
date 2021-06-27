using System.Collections.Generic;
using System.Text;

namespace Maybenogi.Shared.Utils
{
    public class Singleton<T> where T : class, new()
    {
        private static T instance = default;

        public static T Instance
        {
            get
            {
                if (instance == default)
                {
                    instance = new T();
                }

                return instance;
            }
        }
    }
}