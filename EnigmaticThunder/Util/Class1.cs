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
        private  = new ContentPack();
        public string identifier => "EnigmaticThunder.ModdedContent";

        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)    
        {
            yield break;
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            yield break;
        }

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            args.
            yield break;
        }
    }
}
