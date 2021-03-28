using System;
using System.Collections.Generic;
using BepInEx.Logging;
using RoR2;

namespace EnigmaticThunder.Util
{
    #pragma warning disable
    public class ChainLoaderListener : ILogListener
    {
        public static event Action OnChainLoaderFinished;

        public void Dispose() { }

        public void LogEvent(object sender, LogEventArgs eventArgs)
        {
            var msg = eventArgs.Data.ToString();
            var level = eventArgs.Level;

            if (level == LogLevel.Message && msg == "Chainloader startup complete") {
                //don't invoke if null
                OnChainLoaderFinished?.Invoke();
            }
        }
    }
}