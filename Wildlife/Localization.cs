using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using System;

namespace Wildlife
{
	[HarmonyPatch]
	public class WLLocal
	{
		private static Localization lcl;
		public static Dictionary<string, string> t;
		private static Dictionary<string, string> english = new Dictionary<string, string>() {

			{"animal_sheep_dod", "Sheep"},
			{"animal_goat_dod", "Goat"},
			{"animal_goose_dod", "Goose"},
			{"animal_penguin_dod", "Penguin"},
			{"animal_salamander_dod", "Fire Salamander"},
			{"animal_turtle_dod", "Box Turtle"},
			{"animal_rat_dod", "Rat"},
			{"animal_greenlizard_dod", "Green Lizard"},
			{"animal_brownlizard_dod", "Brown Lizard"},
			{"animal_spottedlizard_dod", "Spotted Lizard"},
			{"animal_greenfrog_dod", "Green Frog"},
			{"animal_blackfrog_dod", "Black Frog"},
			{"animal_spottedfrog_dod", "Spotted Frog"},
			{"animal_greyrabbit_dod", "Grey Rabbit"},
			{"animal_brownrabbit_dod", "Brown Rabbit"},
			{"animal_giantsnail_dod", "Giant Snail"},
		};
		private static Dictionary<string, string> russian = new Dictionary<string, string>() {

			{"animal_sheep_dod", "Овца"},
			{"animal_goat_dod", "Козел"},
			{"animal_goose_dod", "Гусь"},
			{"animal_penguin_dod", "Пингвин"},
			{"animal_salamander_dod", "Огненная саламандра"},
			{"animal_turtle_dod", "Коробчатая черепаха"},
			{"animal_rat_dod", "Крыса"},
			{"animal_greenlizard_dod", "Зеленая ящерица"},
			{"animal_brownlizard_dod", "Коричневая ящерица"},
			{"animal_spottedlizard_dod", "Пятнистая ящерица"},
			{"animal_greenfrog_dod", "Зеленая лягушка"},
			{"animal_blackfrog_dod", "Черная лягушка"},
			{"animal_spottedfrog_dod", "Пятнистая лягушка"},
			{"animal_greyrabbit_dod", "Серый кролик"},
			{"animal_brownrabbit_dod", "Коричневый кролик"},
			{"animal_giantsnail_dod", "Гигантская улитка"},

			};
		private static Dictionary<string, string> turkish = new Dictionary<string, string>() {

			{"animal_sheep_dod", "Koyun"},
			{"animal_goat_dod", "Keçi"},
			{"animal_goose_dod", "Kaz"},
			{"animal_penguin_dod", "Penguen"},
			{"animal_salamander_dod", "Ateş Semenderi"},
			{"animal_turtle_dod", "Kaplumbağa"},
			{"animal_rat_dod", "Fare"},
			{"animal_greenlizard_dod", "Yeşil Kertenkele"},
			{"animal_brownlizard_dod", "Kahverengi Kertenkele"},
			{"animal_spottedlizard_dod", "Benekli Kertenkele"},
			{"animal_greenfrog_dod", "Yeşil Kurbağa"},
			{"animal_blackfrog_dod", "Black Kurbağa"},
			{"animal_spottedfrog_dod", "Benekli Kurbağa"},
			{"animal_greyrabbit_dod", "Grey Tavşan"},
			{"animal_brownrabbit_dod", "Kahverengi Tavşan"},
			{"animal_giantsnail_dod", "Dev Salyangoz"},
		};
		private static Dictionary<string, string> german = new Dictionary<string, string>() {

			{"animal_sheep_dod", "Schaf"},
			{"animal_goat_dod", "Ziege"},
			{"animal_goose_dod", "Gans"},
			{"animal_penguin_dod", "Pinguin"},
			{"animal_salamander_dod", "Feuersalamander"},
			{"animal_turtle_dod", "Dosenschildkröte"},
			{"animal_rat_dod", "Ratte"},
			{"animal_greenlizard_dod", "Grüne Eidechse"},
			{"animal_brownlizard_dod", "Braune Eidechse"},
			{"animal_spottedlizard_dod", "Gefleckte Eidechse"},
			{"animal_greenfrog_dod", "Grüner Frosch"},
			{"animal_blackfrog_dod", "Schwarzer Frosch"},
			{"animal_spottedfrog_dod", "Gefleckter Frosch"},
			{"animal_greyrabbit_dod", "Graues Kaninchen"},
			{"animal_brownrabbit_dod", "Braunes Kaninchen"},
			{"animal_giantsnail_dod", "Riesenschnecke"},

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
			else if (lang == "Turkish")
			{
				t = turkish;
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
