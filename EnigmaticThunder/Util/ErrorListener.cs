using BepInEx.Logging;
using RoR2;

namespace EnigmaticThunder.Util
{

    public class ErrorListener : ILogListener
    {
        public static EnigmaticList<LogMessage> modErrors = new EnigmaticList<LogMessage>();
            public static EnigmaticList<LogMessage> vanillaErrors = new EnigmaticList<LogMessage>();
            public static EnigmaticList<LogMessage> totalErrors = new EnigmaticList<LogMessage>();
            public static EnigmaticList<LogMessage> logs = new EnigmaticList<LogMessage>();

            public struct LogMessage
            {
                public bool isVanillaError;
                public string message;
                public LogLevel level;
                public LogEventArgs args;

            }
        public void Dispose() { }

        public void LogEvent(object sender, LogEventArgs eventArgs)
        {
            var msg = eventArgs.Data.ToString();
            var level = eventArgs.Level;

            var logMessage = new LogMessage()
            {
                message = msg,
                level = level,
                args = eventArgs,
            };
            logMessage.isVanillaError = IsLogVanilla(logMessage);

            logs.Add(logMessage);
            if (logMessage.level == LogLevel.Error)
            {
                if (!logMessage.isVanillaError)
                {
                    modErrors.Add(logMessage);
                }
                else
                {
                    vanillaErrors.Add(logMessage);
                }
                totalErrors.Add(logMessage);
            }
        }
        public static bool IsLogVanilla(LogMessage log)
        {
            bool isVanilla = false;
            if (SceneCatalog.availability.available != false && SceneCatalog.GetSceneDefForCurrentScene() && SceneCatalog.GetSceneDefForCurrentScene().baseSceneName == "lobby")
            {
                isVanilla = true;
            }
            if (log.message == "Skin skinCrocoDefault (RoR2.SkinDef) has an empty renderer field in its rendererInfos.") {
                isVanilla = true;
            }
            return isVanilla;
        }
    }

}
