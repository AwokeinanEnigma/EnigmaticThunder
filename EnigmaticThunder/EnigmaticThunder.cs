using BepInEx;
using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace EnigmaticThunder
{
    [BepInPlugin(guid, modName, version)]
    public class EnigmaticThunder : BaseUnityPlugin
    {
        //be unique, though you're the same here.

        public const string guid = "com.EnigmaDev.EnigmaticThunder";
        public const string modName = "Enigmatic Thunder";
        public const string version = "0.1.0";

        public static EnigmaticThunder instance;

        /// <summary>
        /// Called BEFORE the first frame of the game.
        /// </summary>
        public static event Action awake;
        /// <summary>
        /// Called on the first frame of the game.
        /// </summary>
        internal static event Action start;

        /// <summary>
        /// Called when the mod is disabled
        /// </summary>
        internal static event Action onDisable;
        /// <summary>
        /// Called on the mod's FixedUpdate
        /// </summary>
        internal static event Action onFixedUpdate;

        private static int vanillaErrors = 0;
        private static int modErrors = 0;

        private ContentPack internalContentPack = new ContentPack();

        public event Action preContentPackLoad;
        public event Action postContentPackLoad;

        internal static ObservableCollection<Util.Module> modules = new ObservableCollection<Util.Module>(); 

        private void AddContent(On.RoR2.ContentManager.orig_SetContentPacks orig, List<ContentPack> newContentPacks)
        {
            preContentPackLoad?.Invoke();

            //Modify content pack.
            foreach (Util.Module module in modules) {
                LogCore.LogI(module + " is modifying the content pack.");
                module.ModifyContentPack(internalContentPack);
            }
            newContentPacks.Add(internalContentPack);

            postContentPackLoad?.Invoke();

            orig(newContentPacks);
        }

        //@El Conserje call it ConserjeCore or I'll scream
        public EnigmaticThunder()
        {
            LogCore.logger = base.Logger;

            //Add listeners.
            BepInEx.Logging.Logger.Listeners.Add(new ErrorListener());
            BepInEx.Logging.Logger.Listeners.Add(new ChainLoaderListener());

            SingletonHelper.Assign<EnigmaticThunder>(ref EnigmaticThunder.instance, this);

            GatherModules();

            ErrorListener.vanillaErrors.addition += VanillaErrors_addition;
            ErrorListener.modErrors.addition += ModErrors_addition;

            ChainLoaderListener.OnChainLoaderFinished += OnChainLoaderFinished;

            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void GatherModules() {
            List<Type> gatheredModules = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(Util.Module))).ToList();
            foreach (Type module in gatheredModules) {
                //Create instance.
                Util.Module item = (Util.Module)Activator.CreateInstance(module);
                //Log
                LogCore.LogI("Enabling module: " + item);
                //Add to collection
                modules.Add(item);

            }

        }

        private void OnChainLoaderFinished()
        {
            //Wait until all mods have loaded.
            On.RoR2.ContentManager.SetContentPacks += AddContent;
        }



        private void ModErrors_addition(ErrorListener.LogMessage objectRemoved)
        {
            modErrors++;
        }

        private void VanillaErrors_addition(ErrorListener.LogMessage msg)
        {
            vanillaErrors++;
        }


        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name == "title")
            {
                var menu = GameObject.Find("MainMenu");
                //LogCore.LogI(menu.name)
                var title = menu.transform.Find("MENU: Title/TitleMenu/SafeZone/ImagePanel (JUICED)/LogoImage");
                var indicator = menu.transform.Find("MENU: Title/TitleMenu/MiscInfo/Copyright/Copyright (1)");

                var build = indicator.GetComponent<HGTextMeshProUGUI>();

                build.fontSize += 6;
                build.text = build.text + Environment.NewLine + $"EnigmaticThunder Version: {version}";
                //build.text = build.text + Environment.NewLine + $"R2API Version: { R2API.R2API.PluginVersion }";
                build.text = build.text + Environment.NewLine + $"Vanilla Errors: {vanillaErrors.ToString()}";
                build.text = build.text + Environment.NewLine + $"Mod Errors: {modErrors.ToString()}";
            }
        }
  
        #region Events
        public void Awake()
        {
            Action awake = EnigmaticThunder.awake;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void FixedUpdate()
        {


            Action fixedUpdate = EnigmaticThunder.onFixedUpdate;
            if (fixedUpdate == null)
            {
                return;
            }
            fixedUpdate();
        }

        public void Start()
        {
            Action awake = EnigmaticThunder.start;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void OnDisable()
        {
            SingletonHelper.Unassign<EnigmaticThunder>(EnigmaticThunder.instance, this);
            Action awake = EnigmaticThunder.onDisable;
            if (awake == null)
            {
                return;
            }
            awake();
        }
        #endregion
    }
}