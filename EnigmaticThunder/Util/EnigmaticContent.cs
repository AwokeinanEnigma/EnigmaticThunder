using EnigmaticThunder.Modules;
using RoR2.ContentManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaticThunder.Util
{
    class EnigmaticContent : IContentPackProvider
    {
        private ContentPack contentPack = new ContentPack();
        public string identifier => "EnigmaticThunder.ModdedContent";

        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)    
        {
            //we do a large amount of trolling
            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(this.contentPack, args.output);
            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            //we do a CONSIDERABLE amount of trolling.
            contentPack.artifactDefs.Add(Artifacts.DumpContent());
            args.ReportProgress(1f);
            contentPack.bodyPrefabs.Add(Bodies.DumpContent());
            args.ReportProgress(1f);
            contentPack.buffDefs.Add(Buffs.DumpContent());
            args.ReportProgress(1f);
            contentPack.effectDefs.Add(Effects.DumpContent());
            args.ReportProgress(1f);
            contentPack.eliteDefs.Add(Elites.DumpContent());
            args.ReportProgress(1f);
            contentPack.gameEndingDefs.Add(GameEndings.DumpContent());
            args.ReportProgress(1f);
            contentPack.entityStateConfigurations.Add(Loadouts.DumpConfigs());
            args.ReportProgress(1f);
            contentPack.entityStateTypes.Add(Loadouts.DumpEntityStates());
            args.ReportProgress(1f);
            contentPack.skillFamilies.Add(Loadouts.DumpContentSkillFamilies());
            args.ReportProgress(1f);
            contentPack.skillDefs.Add(Loadouts.DumpContentSkillDefs());
            args.ReportProgress(1f);
            contentPack.survivorDefs.Add(Loadouts.DumpSurvivorDefs());
            args.ReportProgress(1f);
            contentPack.masterPrefabs.Add(Masters.DumpContent());
            args.ReportProgress(1f);
            contentPack.musicTrackDefs.Add(MusicTracks.DumpContent());
            args.ReportProgress(1f);
            contentPack.networkedObjectPrefabs.Add(NetworkPrefabs.DumpContent());
            args.ReportProgress(1f);
            contentPack.networkSoundEventDefs.Add(NetworkSoundEvents.DumpContent());
            args.ReportProgress(1f);
            contentPack.itemDefs.Add(Pickups.DumpContentItems());
            args.ReportProgress(1f);
            contentPack.equipmentDefs.Add(Pickups.DumpContentEquipment());
            args.ReportProgress(1f);
            contentPack.projectilePrefabs.Add(Projectiles.DumpContent());
            args.ReportProgress(1f);
            contentPack.gameModePrefabs.Add(Runs.DumpContent());
            args.ReportProgress(1f);
            contentPack.sceneDefs.Add(Scenes.DumpContent());
            args.ReportProgress(1f);
            contentPack.surfaceDefs.Add(SurfaceDefinitions.DumpContent());
            args.ReportProgress(1f);
            contentPack.unlockableDefs.Add(Unlockables.DumpContent());
            args.ReportProgress(1f);

            yield break;
        }   
    }
}
