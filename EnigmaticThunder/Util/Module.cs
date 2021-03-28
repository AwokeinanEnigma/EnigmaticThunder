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

    /// <summary>
    /// Base module.
    /// </summary>
    public abstract class Module
    {
        internal virtual void Load()
        {
            EnigmaticThunder.start += Start;
        }

        internal virtual void Start(object _, EventArgs __)
        {

        }

        internal virtual void ModifyContentPack(ContentPack pack)
        {

        }
    }
}
