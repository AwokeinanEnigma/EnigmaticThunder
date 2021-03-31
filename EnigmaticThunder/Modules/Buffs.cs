using EnigmaticThunder.Util;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    /// <summary>
    /// Helper class for adding custom buffs to the game. 
    /// </summary>
    public class Buffs : Module
    {
        internal static ObservableCollection<BuffDef> BuffDefDefinitions = new ObservableCollection<BuffDef>();
        internal override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
            IL.RoR2.BuffCatalog.Init += FixBuffCatalog;
        }

        //Credits to Aaron on the RoR2 modding discord.
        internal static void FixBuffCatalog(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.Next.MatchLdsfld(typeof(RoR2Content.Buffs), nameof(RoR2Content.Buffs.buffDefs)))
            {
                LogCore.LogW("Another mod has already fixed BuffCatalog or the game has updated, skipping...");
                return;
            }

            c.Remove();
            c.Emit(OpCodes.Ldsfld, typeof(ContentManager).GetField(nameof(ContentManager.buffDefs)));
        }

        /// <summary>
        /// Registers a buff def to the buff catalog
        /// </summary>
        /// <param name="BuffDef">The buff def you want to register.</param>
        public static void RegisterBuff(BuffDef BuffDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (BuffDefDefinitions.Contains(BuffDef))
            {
                LogCore.LogE(BuffDef.name + " has already been registered, please do not register the same BuffDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            BuffDefDefinitions.Add(BuffDef);
        }

        internal void ModifyContentPack()
        {

            //Make a list of survivor defs (we'll be converting it to an array later)
            List<BuffDef> defs = new List<BuffDef>();
            //Add everything from SurvivorDefinitions to it.
            foreach (BuffDef def in BuffDefDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.buffDefs = defs.ToArray();
        }
    }
}