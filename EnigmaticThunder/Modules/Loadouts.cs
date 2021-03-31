using EnigmaticThunder.Util;
using EntityStates;
using MonoMod.RuntimeDetour;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace EnigmaticThunder.Modules
{

    /// <summary>
    /// Helper class for adding entity states, skill families, skill defs, survivor defs, skins, and entity state configurations.
    /// </summary>
    public class Loadouts : Util.Module
    {
        internal static ObservableCollection<SkillFamily> SkillFamilyDefinitions = new ObservableCollection<SkillFamily>();
        internal static ObservableCollection<Type> EntityStateDefinitions = new ObservableCollection<Type>();
        internal static ObservableCollection<SkillDef> SkillDefDefinitions = new ObservableCollection<SkillDef>();
        internal static ObservableCollection<SurvivorDef> SurvivorDefinitions = new ObservableCollection<SurvivorDef>();
        internal static ObservableCollection<EntityStateConfiguration> EntityStateConfigurationDefinitions = new ObservableCollection<EntityStateConfiguration>();
        internal static readonly HashSet<SkinDef> AddedSkins = new HashSet<SkinDef>();
        internal override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }



    
        public static bool RegisterEntityStateConfig(EntityStateConfiguration entityStateConfiguration)
        {
            //Check if the EntityStateConfiguration has already been registered.
            if (EntityStateConfigurationDefinitions.Contains(entityStateConfiguration))
            {
                string error = entityStateConfiguration.name + " has already been registered, please do not register the same EntityStateConfiguration twice.";
                if (entityStateConfiguration.targetType == default)
                {
                    error = error + " And/Or, the target type has not been set. Please make sure your target has been set before creating your SurvivorDef.";
                }
                LogCore.LogE(error);
                return false;
            }
            //If not, add it to our EntityStateConfigurationDefinitions
            EntityStateConfigurationDefinitions.Add(entityStateConfiguration);
            return true;
        }

        /// <summary>
        /// Registers a SkillFamily to the SkillCatalog.
        /// Must be called before Catalog init (during Awake() or OnEnable())
        /// </summary>
        /// <param name="skillFamily">The SkillDef to add</param>
        /// <returns>True if the event was registered</returns>
        public static bool RegisterSkillFamily(SkillFamily skillFamily)
        {
            //Check if the SurvivorDef has already been registered.
            if (SkillFamilyDefinitions.Contains(skillFamily))
            {
                LogCore.LogE(skillFamily + " has already been registered to the SkillFamily Catalog, please do not register the same SkillFamily twice.");
                return false;
            }
            //If not, add it to our SurvivorDefinitions
            SkillFamilyDefinitions.Add(skillFamily);
            return true;
        }

        /// <summary>
        /// Adds the type of an EntityState to the EntityStateCatalog.
        /// State must derive from EntityStates.EntityState.
        /// Note that SkillDefs and SkillFamiles must also be added seperately.
        /// </summary>
        /// <param name="entityState">The type to add</param>
        /// <returns>True if succesfully added</returns>
        public static bool RegisterEntityState(Type entityState)
        {
            //Check if the entity state has already been registered, is abstract, or is not a subclass of the base EntityState
            if (EntityStateDefinitions.Contains(entityState) || !entityState.IsSubclassOf(typeof(EntityState)) || entityState.IsAbstract)
            {
                LogCore.LogE(entityState.AssemblyQualifiedName + " is either abstract, not a subclass of an entity state, or has already been registered.");
                LogCore.LogI("Is Abstract: " + entityState.IsAbstract + " Is not Subclass: " + !entityState.IsSubclassOf(typeof(EntityState)) + " Is already added: " + EntityStateDefinitions.Contains(entityState));
                return false; 
            }
            //If not, add it to our EntityStateDefinitions
            EntityStateDefinitions.Add(entityState);
            return true;
        }

        /// <summary>
        /// Registers a SkillDef to the SkillCatalog.
        /// Must be called before Catalog init (during Awake() or OnEnable())
        /// </summary>
        /// <param name="skillDef">The SkillDef to add</param>
        /// <returns>True if the event was registered</returns>
        public static bool RegisterSkillDef(SkillDef skillDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (SkillDefDefinitions.Contains(skillDef))
            {
                LogCore.LogE(skillDef + " has already been registered to the SkillDef Catalog, please do not register the same SkillDef twice.");
                return false; 
            }
            //If not, add it to our SurvivorDefinitions
            SkillDefDefinitions.Add(skillDef);
            return true;
        }

        /// <summary>
        /// Add a SurvivorDef to the list of available survivors.
        /// This must be called before the SurvivorCatalog inits, so before plugin.Start().
        /// If this is called after the SurvivorCatalog inits then this will return false and ignore the survivor.        /// The survivor prefab must be non-null
        /// </summary>
        /// <param name="survivorDef">The survivor to add.</param>
        /// <returns>true if survivor will be added</returns>
        public static bool RegisterSurvivorDef(SurvivorDef survivorDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (SurvivorDefinitions.Contains(survivorDef) || !survivorDef.bodyPrefab)
            {
                string error = Language.GetString(survivorDef.displayNameToken) + " has already been registered, please do not register the same SurvivorDef twice.";
                if (!survivorDef.bodyPrefab)
                {
                    error = error + " And/Or, the body prefab is null. Please make sure your body prefab is not null before creating your SurvivorDef.";
                }
                LogCore.LogE(error);

                return false;
            }
            //If not, add it to our SurvivorDefinitions
            SurvivorDefinitions.Add(survivorDef);
            return true;
        }
        internal static SkillFamily[] DumpContentSkillFamilies() {

            List<SkillFamily> skillFamilies = new List<SkillFamily>();
            //Add everything from SkillFamilyDefinitions to it.
            foreach (SkillFamily def in SkillFamilyDefinitions)
            {
                skillFamilies.Add(def);
            }
            return skillFamilies.ToArray(); 
        }

        internal static Type[] DumpEntityStates()
        {
            List<Type> entityStates = new List<Type>();
            //Add everything from EntityStateDefinitions to it.
            foreach (Type def in EntityStateDefinitions)
            {
                entityStates.Add(def);
            }
            return entityStates.ToArray();
        }

        internal static SurvivorDef[] DumpSurvivorDefs() {

            List<SurvivorDef> survivorDefs = new List<SurvivorDef>();
            //Add everything from SurvivorDefinitions to it.
            foreach (SurvivorDef def in SurvivorDefinitions)
            {
                survivorDefs.Add(def);
            }
            return survivorDefs.ToArray(); 
        }

        internal static EntityStateConfiguration[] DumpConfigs() {

            List<EntityStateConfiguration> entityStateConfigs = new List<EntityStateConfiguration>();
            //Add everything from SurvivorDefinitions to it.
            foreach (EntityStateConfiguration def in EntityStateConfigurationDefinitions)
            {
                entityStateConfigs.Add(def);
            }
            return entityStateConfigs.ToArray();
        }

        internal static SkillDef[] DumpContentSkillDefs()
        {

            //Make a lists full of added content

            List<SkillDef> skillDefs = new List<SkillDef>();
            //Add everything from SkillDefDefinitions to it.
            foreach (SkillDef def in SkillDefDefinitions)
            {
                skillDefs.Add(def);
            }
            return skillDefs.ToArray();
        }
        #region Adding Skins
        /// <summary>
        /// Creates a skin icon sprite styled after the ones already in the game.
        /// </summary>
        /// <param name="top">The color of the top portion</param>
        /// <param name="right">The color of the right portion</param>
        /// <param name="bottom">The color of the bottom portion</param>
        /// <param name="left">The color of the left portion</param>
        /// <returns>The icon sprite</returns>
        public static Sprite CreateSkinIcon(Color top, Color right, Color bottom, Color left)
        {
            return CreateSkinIcon(top, right, bottom, left, new Color(0.6f, 0.6f, 0.6f));
        }
        /// <summary>
        /// Creates a skin icon sprite styled after the ones already in the game.
        /// </summary>
        /// <param name="top">The color of the top portion</param>
        /// <param name="right">The color of the right portion</param>
        /// <param name="bottom">The color of the bottom portion</param>
        /// <param name="left">The color of the left portion</param>
        /// <param name="line">The color of the dividing lines</param>
        /// <returns></returns>
        public static Sprite CreateSkinIcon(Color top, Color right, Color bottom, Color left, Color line)
        {
            var tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
            new IconTexJob
            {
                Top = top,
                Bottom = bottom,
                Right = right,
                Left = left,
                Line = line,
                TexOutput = tex.GetRawTextureData<Color32>()
            }.Schedule(16384, 1).Complete();
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
        }
        private struct IconTexJob : IJobParallelFor
        {
            [ReadOnly]
            public Color32 Top;
            [ReadOnly]
            public Color32 Right;
            [ReadOnly]
            public Color32 Bottom;
            [ReadOnly]
            public Color32 Left;
            [ReadOnly]
            public Color32 Line;
            public NativeArray<Color32> TexOutput;
            public void Execute(int index)
            {
                int x = index % 128 - 64;
                int y = index / 128 - 64;

                if (Math.Abs(Math.Abs(y) - Math.Abs(x)) <= 2)
                {
                    TexOutput[index] = Line;
                    return;
                }
                if (y > x && y > -x)
                {
                    TexOutput[index] = Top;
                    return;
                }
                if (y < x && y < -x)
                {
                    TexOutput[index] = Bottom;
                    return;
                }
                if (y > x && y < -x)
                {
                    TexOutput[index] = Left;
                    return;
                }
                if (y < x && y > -x)
                {
                    TexOutput[index] = Right;
                }
            }
        }

        /// <summary>
        /// A container struct for all SkinDef parameters.
        /// Use this to set skinDef values, then call CreateNewSkinDef().
        /// </summary>
        public struct SkinDefInfo
        {
#pragma warning disable 
            public SkinDef[] BaseSkins;
            public Sprite Icon;
            public string NameToken;

            [Obsolete("Use SkinDefInfo.UnlockableDef instead")]
            public string UnlockableName;

            public UnlockableDef UnlockableDef;

            public GameObject RootObject;
            public CharacterModel.RendererInfo[] RendererInfos;
            public SkinDef.MeshReplacement[] MeshReplacements;
            public SkinDef.GameObjectActivation[] GameObjectActivations;
            public SkinDef.ProjectileGhostReplacement[] ProjectileGhostReplacements;
            public SkinDef.MinionSkinReplacement[] MinionSkinReplacements;
            public string Name;
        }

        /// <summary>
        /// Creates a new SkinDef from a SkinDefInfo.
        /// Note that this prevents null-refs by disabling SkinDef awake while the SkinDef is being created.
        /// The things that occur during awake are performed when first applied to a character instead.
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static SkinDef CreateNewSkinDef(SkinDefInfo skin)
        {
            On.RoR2.SkinDef.Awake += DoNothing;

            var newSkin = ScriptableObject.CreateInstance<SkinDef>();

            newSkin.baseSkins = skin.BaseSkins;
            newSkin.icon = skin.Icon;
            newSkin.unlockableDef = skin.UnlockableDef;
            newSkin.unlockableName = skin.UnlockableName;

            newSkin.rootObject = skin.RootObject;
            newSkin.rendererInfos = skin.RendererInfos;
            newSkin.gameObjectActivations = skin.GameObjectActivations;
            newSkin.meshReplacements = skin.MeshReplacements;
            newSkin.projectileGhostReplacements = skin.ProjectileGhostReplacements;
            newSkin.minionSkinReplacements = skin.MinionSkinReplacements;
            newSkin.nameToken = skin.NameToken;
            newSkin.name = skin.Name;

            On.RoR2.SkinDef.Awake -= DoNothing;

            AddedSkins.Add(newSkin);
            return newSkin;
        }

        /// <summary>
        /// Adds a skin to the body prefab for a character.
        /// Will attempt to create a default skin if one is not present.
        /// Must be called during plugin Awake or OnEnable. If called afterwards the new skins must be added to bodycatalog manually.
        /// </summary>
        /// <param name="bodyPrefab">The body to add the skin to</param>
        /// <param name="skin">The SkinDefInfo for the skin to add</param>
        /// <returns>True if successful</returns>
        public static bool AddSkinToCharacter(GameObject bodyPrefab, SkinDefInfo skin)
        {
            var skinDef = CreateNewSkinDef(skin);
            return AddSkinToCharacter(bodyPrefab, skinDef);
        }

        /// <summary>
        /// Adds a skin to the body prefab for a character.
        /// Will attempt to create a default skin if one is not present.
        /// Must be called during plugin Awake or OnEnable. If called afterwards the new skins must be added to bodycatalog manually.
        /// </summary>
        /// <param name="bodyPrefab">The body to add the skin to</param>
        /// <param name="skin">The SkinDef to add</param>
        /// <returns>True if successful</returns>
        public static bool AddSkinToCharacter(GameObject bodyPrefab, SkinDef skin)
        {
            if (bodyPrefab == null)
            {
                LogCore.LogE("Tried to add skin to null body prefab.");
                return false;
            }

            if (skin == null)
            {
                LogCore.LogE("Tried to add invalid skin.");
                return false;
            }
            AddedSkins.Add(skin);

            var modelLocator = bodyPrefab.GetComponent<ModelLocator>();
            if (modelLocator == null)
            {
                LogCore.LogE("Tried to add skin to invalid body prefab (No ModelLocator).");
                return false;
            }

            var model = modelLocator.modelTransform;
            if (model == null)
            {
                LogCore.LogE("Tried to add skin to body prefab with no modelTransform.");
                return false;
            }

            if (skin.rootObject != model.gameObject)
            {
                LogCore.LogE("Tried to add skin with improper root object set.");
                return false;
            }

            var modelSkins = model.GetComponent<ModelSkinController>();

            if (modelSkins == null)
            {
                LogCore.LogW(bodyPrefab.name + " does not have a modelSkinController.\nAdding a new one and attempting to populate the default skin.\nHighly recommended you set the controller up manually.");
                var charModel = model.GetComponent<CharacterModel>();
                if (charModel == null)
                {
                    LogCore.LogE("Unable to locate CharacterModel, default skin creation aborted.");
                    return false;
                }

                var skinnedRenderer = charModel.GetFieldValue<SkinnedMeshRenderer>("mainSkinnedMeshRenderer");
                if (skinnedRenderer == null)
                {
                    LogCore.LogE("CharacterModel did not contain a main SkinnedMeshRenderer, default skin creation aborted.");
                    return false;
                }

                var baseRenderInfos = charModel.baseRendererInfos;
                if (baseRenderInfos == null || baseRenderInfos.Length == 0)
                {
                    LogCore.LogE("CharacterModel rendererInfos are invalid, default skin creation aborted.");
                    return false;
                }

                modelSkins = model.gameObject.AddComponent<ModelSkinController>();

                var skinDefInfo = new SkinDefInfo
                {
                    BaseSkins = Array.Empty<SkinDef>(),
                    GameObjectActivations = Array.Empty<SkinDef.GameObjectActivation>(),
                    Icon = CreateDefaultSkinIcon(),
                    Name = "skin" + bodyPrefab.name + "Default",
                    NameToken = bodyPrefab.name.ToUpper() + "_DEFAULT_SKIN_NAME",
                    RootObject = model.gameObject,
                    UnlockableName = "",
                    UnlockableDef = null,
                    MeshReplacements = new[]
                    {
                        new SkinDef.MeshReplacement {
                            renderer = skinnedRenderer,
                            mesh = skinnedRenderer.sharedMesh
                        }
                    },
                    RendererInfos = charModel.baseRendererInfos,
                    ProjectileGhostReplacements = Array.Empty<SkinDef.ProjectileGhostReplacement>(),
                    MinionSkinReplacements = Array.Empty<SkinDef.MinionSkinReplacement>()
                };

                var defaultSkinDef = CreateNewSkinDef(skinDefInfo);

                modelSkins.skins = new[] {
                    defaultSkinDef
                };
            }

            //Array.Resize(ref modelSkins.skins, modelSkins.skins.Length + 1);
            //modelSkins.skins[modelSkins.skins.Length - 1] = skin;

            Array.Resize(ref modelSkins.skins, modelSkins.skins.Length + 1);
            modelSkins.skins[modelSkins.skins.Length - 1] = skin;
            return true;
        }

        private static Sprite CreateDefaultSkinIcon()
        {
            return CreateSkinIcon(Color.red, Color.green, Color.blue, Color.black);
        }

        private static void DoNothing(On.RoR2.SkinDef.orig_Awake orig, SkinDef self)
        {
            //Intentionally do nothing
        }
        #endregion
    }
}
