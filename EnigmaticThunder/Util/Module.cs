using RoR2;
using System;

namespace EnigmaticThunder.Util
{
    internal abstract class Module<T> : Module where T : Module<T>
    {
        public static T instance { get; private set; }

        public Module()
        {
            if (instance != null) throw new InvalidOperationException("Singleton public class \"" + typeof(T).Name + "\" was instantiated twice!");
            instance = this as T;
        }
    }

    public abstract class Module
    {
        public virtual void Load()
        {
            EnigmaticThunder.start += Start;
        }

        public virtual void Start()
        {

        }

        public virtual void ModifyContentPack(ContentPack pack)
        {

        }
    }
}
