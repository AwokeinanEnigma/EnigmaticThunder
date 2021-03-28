using EnigmaticThunder.Util;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System;
using System.IO;
using BepInEx;

namespace EnigmaticThunder.Modules
{
    public class Languages : Module
    {
        private static Dictionary<string, Dictionary<string, string>> customLanguage = new Dictionary<string, Dictionary<string, string>>();
        private static Dictionary<string, Dictionary<string, string>> overlayLanuage = new Dictionary<string, Dictionary<string, string>>();
        private static List<LanguageOverlay> temporaryOverlays = new List<LanguageOverlay>();
        private const string genericLanguage = "generic";

        public override void Load()
        {
            On.RoR2.Language.GetLocalizedStringByToken += Language_GetLocalizedStringByToken;
            On.RoR2.Language.TokenIsRegistered += Language_TokenIsRegistered;
        }

        private static bool Language_TokenIsRegistered(On.RoR2.Language.orig_TokenIsRegistered orig, Language self, string token)
        {
            var languagename = self.name;
            if (overlayLanuage.ContainsKey(languagename))
            {
                if (overlayLanuage[languagename].ContainsKey(token))
                {
                    return true;
                }
            }
            if (overlayLanuage.ContainsKey(genericLanguage))
            {
                if (overlayLanuage[genericLanguage].ContainsKey(token))
                {
                    return true;
                }
            }
            if (customLanguage.ContainsKey(languagename))
            {
                if (customLanguage[languagename].ContainsKey(token))
                {
                    return true;
                }
            }
            if (customLanguage.ContainsKey(genericLanguage))
            {
                if (customLanguage[genericLanguage].ContainsKey(token))
                {
                    return true;
                }
            }
            return orig(self, token);
        }

        private static string Language_GetLocalizedStringByToken(On.RoR2.Language.orig_GetLocalizedStringByToken orig, Language self, string token)
        {
            var languagename = self.name;
            if (overlayLanuage.ContainsKey(languagename))
            {
                if (overlayLanuage[languagename].ContainsKey(token))
                {
                    return overlayLanuage[languagename][token];
                }
            }
            if (overlayLanuage.ContainsKey(genericLanguage))
            {
                if (overlayLanuage[genericLanguage].ContainsKey(token))
                {
                    return overlayLanuage[genericLanguage][token];
                }
            }
            if (customLanguage.ContainsKey(languagename))
            {
                if (customLanguage[languagename].ContainsKey(token))
                {
                    return customLanguage[languagename][token];
                }
            }
            if (customLanguage.ContainsKey(genericLanguage))
            {
                if (customLanguage[genericLanguage].ContainsKey(token))
                {
                    return customLanguage[genericLanguage][token];
                }
            }
            return orig(self, token);
        }

        /// <summary>
        /// Adds a single languagetoken and its associated value to all languages
        /// </summary>
        /// <param name="key">Token the game asks</param>
        /// <param name="value">Value it gives back</param>
        public static void Add(string key, string value)
        {
            if (key == null)
            {
                throw new NullReferenceException($"param {nameof(key)} is null");
            }
            if (value == null)
            {
                throw new NullReferenceException($"param {nameof(value)} is null");
            }

            Add(key, value, genericLanguage);
        }

        /// <summary>
        /// Adds a single languagetoken and value to a specific language
        /// </summary>
        /// <param name="key">Token the game asks</param>
        /// <param name="value">Value it gives back</param>
        /// <param name="language">Language you want to add this to</param>
        public static void Add(string key, string value, string language)
        {
            if (key == null)
            {
                throw new NullReferenceException($"param {nameof(key)} is null");
            }
            if (value == null)
            {
                throw new NullReferenceException($"param {nameof(value)} is null");
            }
            if (language == null)
            {
                throw new NullReferenceException($"param {nameof(language)} is null");
            }

            if (!customLanguage.ContainsKey(language))
            {
                customLanguage.Add(language, new Dictionary<string, string>());
            }
            var languagedict = customLanguage[language];
            if (!languagedict.ContainsKey(key))
            {
                languagedict.Add(key, value);
            }
        }

        /// <summary>
        /// Adds multiple languagetokens and value
        /// </summary>
        /// <param name="tokenDictionary">dictionaries of key-value (eg ["mytoken"]="mystring")</param>
        public static void Add(Dictionary<string, string> tokenDictionary)
        {

            Add(tokenDictionary, genericLanguage);
        }

        /// <summary>
        /// Adds multiple languagetokens and value to a specific language
        /// </summary>
        /// <param name="tokenDictionary">dictionaries of key-value (eg ["mytoken"]="mystring")</param>
        /// <param name="language">Language you want to add this to</param>
        public static void Add(Dictionary<string, string> tokenDictionary, string language)
        {

            if (tokenDictionary == null)
            {
                throw new NullReferenceException($"param {nameof(tokenDictionary)} is null");
            }

            foreach (var item in tokenDictionary)
            {
                if (item.Value == null)
                {
                    continue;
                }
                Add(item.Key, item.Value, language);
            }
        }

        /// <summary>
        /// Adds multiple languagetokens and value to languages
        /// </summary>
        /// <param name="languageDictionary">dictionary of languages containing dictionaries of key-value (eg ["en"]["mytoken"]="mystring")</param>
        public static void Add(Dictionary<string, Dictionary<string, string>> languageDictionary)
        {

            if (languageDictionary == null)
            {
                throw new NullReferenceException($"param {nameof(languageDictionary)} is null");
            }

            foreach (var language in languageDictionary)
            {
                Add(language.Value, language.Key);
            }
        }

        /// <summary>
        /// Manages temporary language token changes.
        /// </summary>
        public class LanguageOverlay
        {
            internal LanguageOverlay(List<OverlayTokenData> data)
            {
                overlayTokenDatas = data;
                readOnlyOverlays = overlayTokenDatas.AsReadOnly();
                temporaryOverlays.Add(this);
                this.Add();
            }
            /// <summary>Contains information about the language token changes this LanguageOverlay makes.</summary>
            public readonly ReadOnlyCollection<OverlayTokenData> readOnlyOverlays;
            private List<OverlayTokenData> overlayTokenDatas;

            private void Add()
            {
                foreach (var item in readOnlyOverlays)
                {
                    if (!overlayLanuage.ContainsKey(item.lang))
                    {
                        overlayLanuage.Add(item.lang, new Dictionary<string, string>());
                    }
                    var langdict = overlayLanuage[item.lang];
                    langdict[item.key] = item.value;
                }
            }

            /// <summary>Undoes this LanguageOverlay's language token changes; you may safely dispose it afterwards. Requires a language reload to take effect.</summary>
            public void Remove()
            {
                temporaryOverlays.Remove(this);
                overlayLanuage.Clear();
                foreach (var item in temporaryOverlays)
                {
                    item.Add();
                }
            }
        }

        /// <summary>
        /// Adds a single temporary language token, and its associated value, to all languages. Please add multiple instead (dictionary- or file-based signatures) where possible. Language-specific tokens, as well as overlays added later in time, will take precedence. Call LanguageOverlay.Remove() on the result to undo your change to this language token.
        /// </summary>
        /// <param name="key">Token the game asks</param>
        /// <param name="value">Value it gives back</param>
        /// <returns>A LanguageOverlay representing your language addition/override; call .Remove() on it to undo the change. May be safely disposed after calling .Remove().</returns>
        public static LanguageOverlay AddOverlay(string key, string value)
        {

            if (key == null)
            {
                throw new NullReferenceException($"param {nameof(key)} is null");
            }
            if (value == null)
            {
                throw new NullReferenceException($"param {nameof(value)} is null");
            }

            return AddOverlay(key, value, genericLanguage);
        }

        /// <summary>
        /// Adds a single temporary language token, and its associated value, to a specific language. Please add multiple instead (dictionary- or file-based signatures) where possible. Overlays added later in time will take precedence. Call LanguageOverlay.Remove() on the result to undo your change to this language token.
        /// </summary>
        /// <param name="key">Token the game asks</param>
        /// <param name="value">Value it gives back</param>
        /// <param name="lang">Language you want to add this to</param>
        /// <returns>A LanguageOverlay representing your language addition/override; call .Remove() on it to undo the change. May be safely disposed after calling .Remove().</returns>
        public static LanguageOverlay AddOverlay(string key, string value, string lang)
        {

            if (key == null)
            {
                throw new NullReferenceException($"param {nameof(key)} is null");
            }
            if (value == null)
            {
                throw new NullReferenceException($"param {nameof(value)} is null");
            }
            if (lang == null)
            {
                throw new NullReferenceException($"param {nameof(lang)} is null");
            }

            var list = new List<OverlayTokenData>(1);
            list.Add(new OverlayTokenData(key, value, lang));

            return new LanguageOverlay(list);
        }

        /// <summary>
        /// Adds multiple temporary language tokens, and corresponding values, to all languages. Language-specific tokens, as well as overlays added later in time, will take precedence. Call LanguageOverlay.Remove() on the result to remove your changes to these language tokens.
        /// </summary>
        /// <param name="tokenDictionary">dictionaries of key-value (eg ["mytoken"]="mystring")</param>
        /// <returns>A LanguageOverlay representing your language addition/override; call .Remove() on it to undo the change.</returns>
        public static LanguageOverlay AddOverlay(Dictionary<string, string> tokenDictionary)
        {

            if (tokenDictionary == null)
            {
                throw new NullReferenceException($"param {nameof(tokenDictionary)} is null");
            }

            return AddOverlay(tokenDictionary, genericLanguage);
        }

        /// <summary>
        /// Adds multiple temporary language tokens, and corresponding values, to a specific language. Overlays added later in time will take precedence. Call LanguageOverlay.Remove() on the result to remove your changes to these language tokens.
        /// </summary>
        /// <param name="tokenDictionary">dictionaries of key-value (eg ["mytoken"]="mystring")</param>
        /// <param name="language">Language you want to add this to</param>
        /// <returns>A LanguageOverlay representing your language addition/override; call .Remove() on it to undo the change.</returns>
        public static LanguageOverlay AddOverlay(Dictionary<string, string> tokenDictionary, string language)
        {

            if (tokenDictionary == null)
            {
                throw new NullReferenceException($"param {nameof(tokenDictionary)} is null");
            }

            if (language == null)
            {
                throw new NullReferenceException($"param {nameof(language)} is null");
            }

            var list = new List<OverlayTokenData>(tokenDictionary.Count);
            foreach (var item in tokenDictionary)
            {
                if (item.Value == null)
                {
                    continue;
                }
                list.Add(new OverlayTokenData(item.Key, item.Value, language));
            }
            return new LanguageOverlay(list);
        }

        /// <summary>
        /// Adds multiple temporary language tokens, and corresponding values, to mixed languages. Overlays added later in time will take precedence. Call LanguageOverlay.Remove() on the result to remove your changes to these language tokens.
        /// </summary>
        /// <param name="languageDictionary">dictionary of languages containing dictionaries of key-value (eg ["en"]["mytoken"]="mystring")</param>
        /// <returns>A LanguageOverlay representing your language addition/override; call .Remove() on it to undo the change.</returns>
        public static LanguageOverlay AddOverlay(Dictionary<string, Dictionary<string, string>> languageDictionary)
        {

            if (languageDictionary == null)
            {
                throw new NullReferenceException($"param {nameof(languageDictionary)} is null");
            }

            var list = new List<OverlayTokenData>();
            foreach (var language in languageDictionary)
            {
                if (language.Value == null)
                {
                    continue;
                }

                foreach (var tokenvalue in language.Value)
                {
                    if (tokenvalue.Value == null)
                    {
                        continue;
                    }

                    list.Add(new OverlayTokenData(language.Key, tokenvalue.Key, tokenvalue.Value));
                }
            }

            return new LanguageOverlay(list);
        }

        /// <summary>
        /// Contains information about a single temporary language token change.
        /// </summary>
        public struct OverlayTokenData
        {
            /// <summary>The token identifier to add/replace the value of.</summary>
            public string key;
            /// <summary>The value to set the target token to.</summary>
            public string value;
            /// <summary>The language which the target token belongs to, if isGeneric = false.</summary>
            public string lang;
            /// <summary>Whether the target token is generic (applies to all languages which don't contain the token).</summary>
            public bool isGeneric;

            internal OverlayTokenData(string _key, string _value, string _lang)
            {
                key = _key;
                value = _value;
                if (_lang == genericLanguage)
                {
                    isGeneric = true;
                }
                else
                {
                    isGeneric = false;
                }
                lang = _lang;
            }
            internal OverlayTokenData(string _key, string _value)
            {
                key = _key;
                value = _value;
                lang = genericLanguage;
                isGeneric = true;
            }
        }
    }
}
    
