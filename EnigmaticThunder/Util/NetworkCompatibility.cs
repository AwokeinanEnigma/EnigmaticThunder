using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnigmaticThunder.Utils
{

    /// <summary>
    /// Enum used for telling whether or not the mod should be needed by everyone in multiplayer games.
    /// Also can specify if the mod does not work in multiplayer.
    /// </summary>
    public enum CompatibilityLevel
    {
        NoNeedForSync,
        EveryoneMustHaveMod
    }

    /// <summary>
    /// Enum used for telling whether or not the same mod version should be used by both the server and the clients.
    /// This enum is only useful if CompatibilityLevel.EveryoneMustHaveMod was chosen.
    /// </summary>
    public enum VersionStrictness
    {
        DifferentModVersionsAreOk,
        EveryoneNeedSameModVersion
    }

    /// <summary>
    /// Attribute to have at the top of your BaseUnityPlugin class if
    /// you want to specify if the mod should be installed by everyone in multiplayer games or not.
    /// If the mod is required to be installed by everyone, you'll need to also specify if the same mod version should be used by everyone or not.
    /// By default, it's supposed that everyone needs the mod and the same version.
    /// e.g: [NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
    public class NetworkCompatibility : Attribute
    {

        /// <summary>
        /// Used for telling whether or not the mod should be needed by everyone in multiplayer games.
        /// </summary>
        public CompatibilityLevel CompatibilityLevel { get; internal set; }

        /// <summary>
        /// Enum used for telling whether or not the same mod version should be used by both the server and the clients.
        /// This enum is only useful if CompatibilityLevel.EveryoneMustHaveMod was chosen.
        /// </summary>
        public VersionStrictness VersionStrictness { get; internal set; }

        public NetworkCompatibility(
            CompatibilityLevel compatibility = CompatibilityLevel.EveryoneMustHaveMod,
            VersionStrictness versionStrictness = VersionStrictness.EveryoneNeedSameModVersion)
        {
            CompatibilityLevel = compatibility;
            VersionStrictness = versionStrictness;
        }
    }

    internal class NetworkCompatibilityHandler
    {
        internal const char ModGuidAndModVersionSeparator = ';';
        internal readonly HashSet<string> ModList = new HashSet<string>();

        internal void BuildModList()
        {
            EnigmaticThunderPlugin.start += ScanPluginsForNetworkCompat;
        }

        private void ScanPluginsForNetworkCompat(object _, EventArgs __)
        {
            foreach (var row in BepInEx.Bootstrap.Chainloader.PluginInfos)
            {
                var pluginInfo = row.Value;
                try
                {
                    if (pluginInfo.Dependencies.All(dependency => dependency.DependencyGUID != EnigmaticThunderPlugin.guid))
                    {
                        continue;
                    }

                    var modGuid = pluginInfo.Metadata.GUID;
                    var modVer = pluginInfo.Metadata.Version;
                    
                    TryGetNetworkCompatibility(pluginInfo.Instance.GetType(), out var networkCompatibility);
                    if (networkCompatibility.CompatibilityLevel == CompatibilityLevel.EveryoneMustHaveMod)
                    {
                        ModList.Add(networkCompatibility.VersionStrictness == VersionStrictness.EveryoneNeedSameModVersion
                            ? modGuid + ModGuidAndModVersionSeparator + modVer
                            : modGuid);
                    }
                }
                catch (Exception e)
                {
                    LogCore.logger.LogError($"Exception in ScanPluginsForNetworkCompat while scanning plugin {pluginInfo.Metadata.GUID}");
                    LogCore.logger.LogError("EnigmaticThunder Failed to properly scan the assembly." + Environment.NewLine +
                                          "Please make sure you are compiling against net standard 2.0 " +
                                          "and not anything else when making a plugin for Risk of Rain 2 !" +
                                          Environment.NewLine + e);
                }
            }

            AddToNetworkModList();
            EnigmaticThunderPlugin.start -= ScanPluginsForNetworkCompat;
        }

        private static void TryGetNetworkCompatibility(Type baseUnityPluginType, out NetworkCompatibility networkCompatibility)
        {
            networkCompatibility = new NetworkCompatibility();

            foreach (var assemblyAttribute in baseUnityPluginType.Assembly.CustomAttributes)
            {
                if (ProcessAttribute(assemblyAttribute, networkCompatibility))
                {
                    return;
                }
            }

            foreach (var attribute in baseUnityPluginType.CustomAttributes)
            {
                if (ProcessAttribute(attribute, networkCompatibility))
                {
                    return;
                }
            }
        }

        private static bool ProcessAttribute(CustomAttributeData attribute, in NetworkCompatibility networkCompatibility)
        {
            if (attribute.AttributeType == typeof(NetworkCompatibility))
            {
                networkCompatibility.CompatibilityLevel = (CompatibilityLevel)attribute.ConstructorArguments[0].Value;
                networkCompatibility.VersionStrictness = (VersionStrictness)attribute.ConstructorArguments[1].Value;

                return true;
            }

            return false;
        }

        private void AddToNetworkModList()
        {
            if (ModList.Count != 0)
            {
                var sortedModList = ModList.ToList();
                sortedModList.Sort(StringComparer.InvariantCulture);
                LogCore.logger.LogInfo("[NetworkCompatibility] Adding to the networkModList : ");
                foreach (var mod in sortedModList)
                {
                    LogCore.logger.LogInfo(mod);
                    NetworkModCompatibilityHelper.networkModList = NetworkModCompatibilityHelper.networkModList.Append(mod);
                }
            }
        }
    }
}