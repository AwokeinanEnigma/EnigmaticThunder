using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    /// <summary>
    /// Helper class for adding items and equipment.
    /// </summary>
    public class Pickups : Module
    {
        internal static ObservableCollection<EquipmentDef> EquipmentDefDefinitions = new ObservableCollection<EquipmentDef>();
        internal static ObservableCollection<ItemDef> ItemDefDefinitions = new ObservableCollection<ItemDef>();
        internal override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        /// <summary>
        /// Registers an item def to the item catalog
        /// </summary>
        /// <param name="itemDef">The item def you want to register.</param>
        public static void RegisterItem(ItemDef itemDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (ItemDefDefinitions.Contains(itemDef))
            {
                LogCore.LogE(itemDef + " has already been registered, please do not register the same ItemDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            ItemDefDefinitions.Add(itemDef);
        }
        /// <summary>
        /// Registers an equipment def to the item catalog
        /// </summary>
        /// <param name="equipmentDef">The equipment def you want to register.</param>
        public static void RegisterEquipment(EquipmentDef equipmentDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (EquipmentDefDefinitions.Contains(equipmentDef))
            {
                LogCore.LogE(equipmentDef + " has already been registered, please do not register the same EquipmentDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            EquipmentDefDefinitions.Add(equipmentDef);
        }
        internal void ModifyContentPack()
        {


            List<ItemDef> itemDefs = new List<ItemDef>();
            //Add everything from ItemDefDefinitions to it.
            foreach (ItemDef def in ItemDefDefinitions)
            {
                itemDefs.Add(def);
            }

            List<EquipmentDef> equipmentDefs = new List<EquipmentDef>();
            //Add everything from EquipmentDefDefinitions to it.
            foreach (EquipmentDef def in EquipmentDefDefinitions)
            {
                equipmentDefs.Add(def);
            }

            //Convert our lists into arrays and give it to the ContentPack.
            pack.itemDefs = itemDefs.ToArray();
            pack.equipmentDefs = equipmentDefs.ToArray();
        }
    }
}