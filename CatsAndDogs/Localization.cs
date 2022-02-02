﻿using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using System;

namespace CatsAndDogs
{
	[HarmonyPatch]
	public class WLLocal
	{
		private static Localization lcl;
		public static Dictionary<string, string> t;
		private static Dictionary<string, string> english = new Dictionary<string, string>() {

			{"animal_cat_cd", "Cat"},
			{"animal_cat_kit_cd", "Kitten"}

		};
		private static Dictionary<string, string> russian = new Dictionary<string, string>() {

			{"animal_cat_cd", "Cat"},
			{"animal_cat_kit_cd", "Kitten"}

			};
		private static Dictionary<string, string> german = new Dictionary<string, string>() {

			{"animal_cat_cd", "Cat"},
			{"animal_cat_kit_cd", "Kitten"}

			};

		public static void init(string lang, Localization l)
		{
			lcl = l;
			if (lang == "Russian")
			{
				t = russian;
			}
			else if (lang == "English")
			{
				t = english;
			}
			else
			{
				t = german;
			}
		}
		public static void AddWord(object[] element)
		{
			MethodInfo meth = AccessTools.Method(typeof(Localization), "AddWord", null, null);
			meth.Invoke(lcl, element);
		}
		public static void UpdateDictinary()
		{
			string missing = "Missing Words:";
			foreach (var el in english)
			{
				if (t.ContainsKey(el.Key))
				{
					AddWord(new object[] { el.Key, t[el.Key] });
					continue;
				}
				AddWord(new object[] { el.Key, el.Value });
				missing += el.Key;
			}
		}

		[HarmonyPatch(typeof(Localization), "SetupLanguage")]
		public static class WLLocalizationPatch
		{
			public static void Postfix(Localization __instance, string language)
			{
				init(language, __instance);
				UpdateDictinary();
			}
		}
	}
}