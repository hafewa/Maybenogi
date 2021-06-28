using System.Collections.Generic;
using System.Text;

namespace Maybenogi.Shared.Utils
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T instance = default;

        public static T Instance
        {
            get
            {
                if (instance == default)
                {
                    instance = new T();
                    instance.ctor();
                }

                return instance;
            }
        }

        protected Singleton()
        {
            
        }

        protected virtual void ctor()
        {

        }
    }
}