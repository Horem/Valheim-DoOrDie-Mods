using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System;

namespace DoOrDieBiomes
{
    [HarmonyPatch]
    public class Local
    {
        private static Localization lcl;
        public static Dictionary<string, string> t;
        private static Dictionary<string, string> english = new Dictionary<string, string>() {

			{ "spawner_firedraketower_dod", "Drake Tower" },
			{ "spawner_arena_dod", "Arena Spawner" },
			{ "piece_bitterstumpaltar_dod", "Bitterstump Altar" },
			{ "object_glowmetal_dod", "Glowing Metal" },
			{ "object_rock_dod", "Rock" },
			{ "prop_worldlevel_dod", "World Level" },
			{ "prop_worldlevel_description_dod", "Level 1: Day 15 - Level 2: Day 25 - Level 3: Day 50 - Level 4: Day 100 - Level 5: Day 200" },
			{ "prop_cavemushroom_dod", "Cave Mushroom" },
			{ "item_cavemushroom_dod", "Cave Mushroom" },
			{ "item_cavemushroom_description_dod", "These come from the underworld." },
			{ "lore_underworld_dod", "The Underworld is a vast subterranean realm inhabited strange and sinister creatures. It is a place where few humans go and from where even fewer return...." },
			{ "enemy_pin_underworld_dod", "Underworld" },
			{ "location_mysterycaveb_dod", "Icy Cavern" },
			{ "location_mysterycavet_dod", "Chilly Cavern" },
			{ "location_mysterycavem_dod", "Fiery Cavern" },
			{ "location_mysterycave_dod", "Entrance to the Underworld" },
			{ "piece_startstone_dod", "Welcome to Do or Die" },
			{ "lore_start_label_dod", "Do or Die Difficulty" },
			{ "lore_start_dod", "Scaling difficulty is always active. The longer you are in the world and the more bosses you kill, the harder the world will become. Bosses will level up one tier per world level, after you kill them the first time. Biome creatures level up once, after you kill the Vanilla boss in that biome. All creatures gain 5% Damage and Health every world level. Maximum Stars is increased to 10 plus 3 from sector level ups." },

			{"piece_vegvisir_dod", "Runestone"},
			{"prop_altar_bitterstump_dod", "Bitterstump Altar"},
			{"item_token_skull_dod", "Skull Token"},
			{"item_token_skull_description_dod", "Knarr the Trader likes these."},
			{"location_bitterstumpcave_dod", "Bitterstump Altar"},
			{"location_bitterstumpcave_text_dod", "Requires 5 Greydwarf Hearts"},
			{"enemy_pin_bitterstump_dod", "Bitterstump's Cave"},
			{"location_bitterstump_dod", "Bitterstump's Cave"},
			{"lore_bitterstump_dod", "Bitterstump - Unknown"},

            {"prop_bluemushroom_dod", "Blewit"},
            {"prop_bluemushroom_description_dod", "Found in Mistlands"},
            {"prop_purpmushroom_dod", "Blushing Bracket"},
            {"prop_purpmushroom_description_dod", "Found in Mistlands"},
            {"prop_walnuttree_dod", "Walnut"},
            {"prop_appletree_dod", "Apple"},
            {"prop_redcherries_dod", "Cherry"},
            {"prop_banana_dod", "Banana"},
            {"prop_bananatree_dod", "Banana Tree"},

            {"prop_mistlandsoak_dod", "Mistlands Oak"},
            {"prop_mistlandsbush_dod", "Bush"},
            {"prop_oldoak_dod", "Old Oak"},
            {"prop_poplar_dod", "Poplar"},
            {"prop_willow_dod", "Willow"},
            {"prop_northernpine_dod", "Northern Pine"},

            {"location_mistlandscave_dod", "Mistlands Cave"},
            {"location_mistlandstower_dod", "Shadow Tower"},
            {"location_secretentrance_mistland_dod","Under Construction" },
            {"location_castlearenain_dod", "Arena"},
            {"location_castlearenaout_dod", "You left the Arena"},
            {"location_castlearena_dod", "Castle Arena"},

			{"location_ramborecave_dod", "Ram'Bore Altar"},
			{"location_ramborecave_text_dod", "Requires 5 Boar Tusks"},
			{"enemy_pin_rambore_dod", "Ram'Bore's Cave"},
			{"location_rambore_dod", "Ram'Bore's Cave"},
			{"lore_rambore_dod", "Ram'Bore - Unknown"},

			{"piece_lorestone_dod", "Lorestone"},

			{"item_walnuts_dod", "Walnuts"},
            {"item_walnuts_description_dod", "Gatherd from a Walnut Tree in the Mistlands."},
            {"item_apple_dod", "Apples"},
            {"item_apple_description_dod", "Gatherd from an Apple Tree in the Mistlands."},
            {"item_redberries_dod", "Cherries"},
            {"item_redberries_description_dod", "Gatherd from a Cherry Tree in the Mistlands."},
        };
        private static Dictionary<string, string> russian = new Dictionary<string, string>() {

			{ "spawner_firedraketower_dod", "Башня Дрейка" },
			{ "spawner_arena_dod", "Arena Spawner" },
			{ "piece_bitterstumpaltar_dod", "Алтарь Горького пня" },
			{ "object_glowmetal_dod", "Светящийся металл" },
			{ "object_rock_dod", "Камень" },
			{ "prop_worldlevel_dod", "Мировой уровень" },
			{ "prop_worldlevel_description_dod", "Уровень 1: День 15 - Уровень 2: День 25 - Уровень 3: День 50 - Уровень 4: День 100 - Уровень 5: День 200" },
			{ "prop_cavemushroom_dod", "Пещерный гриб" },
			{ "item_cavemushroom_dod", "Пещерный гриб" },
			{ "item_cavemushroom_description_dod", "Они пришли из подземного мира." },

			{"lore_underworld_dod", "Подземный мир - это огромное подземное царство, населенное странными и зловещими существами. Это место, куда немногие люди уходят и откуда еще меньше возвращается ...."},
			{"enemy_pin_underworld_dod", "Подземный мир"},
			{"location_mysterycaveb_dod", "Ледяная пещера"},
			{"location_mysterycavet_dod", "Холодная пещера"},
			{"location_mysterycavem_dod", "Огненная пещера"},
			{"location_mysterycave_dod", "Вход в подземный мир"},
			{"piece_startstone_dod", "Добро пожаловать в Do or Die"},
			{"lore_start_label_dod", "Сложность в Do or Die"},
			{"lore_start_dod", "Сложность масштабирования всегда активна. Чем дольше вы находитесь в мире и чем больше боссов вы убиваете, тем сложнее станет мир. Боссы будут повышаться на один уровень за каждый уровень мира после того, как вы убьете их в первый раз. Существа биома повышаются на один уровень после того, как вы убиваете босса Ванили в этом биоме. Все существа получают 5% урона и здоровья на каждом из пяти мировых уровней. Максимальное количество звезд увеличивается до 10 плюс 3 при повышении уровня сектора »." },
			{"piece_vegvisir_dod", "Рунический камень"},
			{"prop_altar_bitterstump_dod", "Алтарь Горького Пня"},
			{"item_token_skull_dod", "Знак черепа"},
			{"item_token_skull_description_dod", "Торговцу Кнарру это нравится."},
			{"location_bitterstumpcave_dod", "Алтарь Горького Пня"},
			{"location_bitterstumpcave_text_dod", "Требуется 5 сердец Грейдварфа"},
			{"enemy_pin_bitterstump_dod", "Пещера Горького Пня"},
			{"location_bitterstump_dod", "Пещера Горького Пня"},
			{"lore_bitterstump_dod", "Горький Пень - Неизвестно"},
			{"prop_bluemushroom_dod","Блюит" },
			{"prop_bluemushroom_description_dod", "Найдено в Туманных Землях" },
			{"prop_purpmushroom_dod", "Кронштейн для покраски" },
			{"prop_purpmushroom_description_dod", "Найдено в Туманных Землях" },
			{"prop_walnuttree_dod",	"Грецкий орех" },
			{"prop_appletree_dod",	"Яблоко" },
			{"prop_redcherries_dod", "Вишня" },
			{"prop_banana_dod", "Банан" },
			{"prop_bananatree_dod","Банановое дерево"},
			{"prop_mistlandsoak_dod","Дуб Туманных земель"},
			{"prop_mistlandsbush_dod","Куст"},
			{"prop_oldoak_dod","Старый дуб"},
			{"prop_poplar_dod","Тополь"},
			{"prop_willow_dod","Ива"},
			{ "prop_northernpine_dod", "Северная сосна" },
			{"location_mistlandscave_dod","Туманная пещера"},
			{"location_mistlandstower_dod","Башня теней"},
			{"location_secretentrance_mistland_dod","Строится"},
			{"location_castlearenain_dod","Арена"},
			{"location_castlearenaout_dod","Вы покинули Арену"},
			{"location_castlearena_dod","Замковая арена"},
			{ "location_ramborecave_dod", "Алтарь Рам'Бора" },
			{ "location_ramborecave_text_dod", "Требуется 5 бивней кабана" },
			{ "enemy_pin_rambore_dod", "Пещера Рам'Бора" },
			{ "location_rambore_dod", "Пещера Рам'Бора" },
			{ "lore_rambore_dod", "Рам'Бор - Неизвестно" },
			{ "piece_lorestone_dod", "Камень знаний" },
			{"item_walnuts_dod","Грецкие орехи"},
			{ "item_walnuts_description_dod", "Собраны с Ореха в Туманных Землях." },
			{ "item_apple_dod", "Яблоки" },
			{ "item_apple_description_dod", "Собраны с Яблони в Туманных землях." },
			{ "item_redberries_dod", "Вишни" },
			{ "item_redberries_description_dod", "Собраны с Вишни в Туманных землях." }
		};

		private static Dictionary<string, string> german = new Dictionary<string, string>() {
			{ "spawner_firedraketower_dod", "Drake Tower" },
			{ "spawner_arena_dod", "Arena Spawner" },
			{ "piece_bitterstumpaltar_dod", "Bitterstump Altar" },
			{ "object_glowmetal_dod", "Glowing Metal" },
			{ "object_rock_dod", "Rock" },
			{ "prop_worldlevel_dod", "World Level" },
			{ "prop_worldlevel_description_dod", "Level 1: Day 15 - Level 2: Day 25 - Level 3: Day 50 - Level 4: Day 100 - Level 5: Day 200" },
			{ "prop_cavemushroom_dod", "Cave Mushroom" },
			{ "item_cavemushroom_dod", "Cave Mushroom" },
			{ "item_cavemushroom_description_dod", "These come from the underworld." },

			{ "lore_underworld_dod", "Die Unterwelt ist ein riesiges unterirdisches Reich, das von seltsamen und unheimlichen Kreaturen bewohnt wird. Es ist ein Ort, an den nur wenige Menschen gehen und von dem noch weniger zurückkehren...." },
			{ "enemy_pin_underworld_dod", "Unterwelt" },
			{ "location_mysterycaveb_dod", "Eishöhle" },
			{ "location_mysterycavet_dod", "Kühle Höhle" },
			{ "location_mysterycavem_dod", "Feurige Höhle" },
			{ "location_mysterycave_dod", "Eingang zur Unterwelt" },
			{ "piece_startstone_dod", "Willkommen bei Do or Die" },
			{ "lore_start_label_dod", "Do or Die Schwierigkeit" },
			{ "lore_start_dod", "Skalierung der Schwierigkeit ist immer aktiv. Je länger du in der Welt bist und je mehr Bosse du tötest, desto schwieriger wird die Welt. Bosse steigen eine Stufe pro Weltlevel auf, nachdem du sie das erste Mal getötet hast. Biom-Kreaturen steigen einmal auf, nachdem du den Vanilla-Boss in diesem Biom getötet hast. Alle Kreaturen erhalten 5% Schaden und Gesundheit auf jeder der 5 Weltstufen. Die maximale Anzahl an Sternen wird auf 10 plus 3 von Sektorstufen-Aufstiegen erhöht." },

			{"piece_vegvisir_dod", "Runenstein"},
			{"prop_altar_bitterstump_dod", "Bitterstumpf-Altar"},
			{"item_token_skull_dod", "Schädel-Token"},
			{"item_token_skull_description_dod", "Knarr der Händler mag diese."},
			{"location_bitterstumpcave_dod", "Bitterstumpf-Altar"},
			{"location_bitterstumpcave_text_dod", "Erfordert 5 Grauzwergherzen"},
			{"enemy_pin_bitterstump_dod", "Bitterstumpfs Höhle"},
			{"location_bitterstump_dod", "Bitterstumpfs Höhle"},
			{"lore_bitterstump_dod", "Bitterstumpf - Unbekannt"},

			{"prop_bluemushroom_dod", "Platzer"},
			{"prop_bluemushroom_description_dod", "In Nebelland gefunden"},
			{"prop_purpmushroom_dod", "Schüchterner Armleuchter"},
			{"prop_purpmushroom_description_dod", "In Nebelland gefunden"},
			{"prop_walnuttree_dod", "Walnuss"},
			{"prop_appletree_dod", "Apfel"},
			{"prop_redcherries_dod", "Kische"},
			{"prop_banana_dod", "Banane"},
			{"prop_bananatree_dod", "Bananenbaum"},

			{"prop_mistlandsoak_dod", "Nebellandeiche"},
			{"prop_mistlandsbush_dod", "Busch"},
			{"prop_oldoak_dod", "Alte Eiche"},
			{"prop_poplar_dod", "Pappel"},
			{"prop_willow_dod", "Weide"},
			{"prop_northernpine_dod", "Nordkiefer"},

			{"location_mistlandscave_dod", "Mistlands-Höhle"},
			{"location_mistlandstower_dod", "Schattenturm"},
			{"location_secretentrance_mistland_dod", "Im Bau"},
			{"location_castlearenain_dod", "Arena"},
			{"location_castlearenaout_dod", "Du hast die Arena verlassen"},
			{"location_castlearena_dod", "Schlossarena"},

			{"location_ramborecave_dod", "Ram'Bore Altar"},
			{"location_ramborecave_text_dod", "Erfordert 5 Wildschweinhauer"},
			{"enemy_pin_rambore_dod", "Ram'Bores Höhle"},
			{"location_rambore_dod", "Ram'Bores Höhle"},
			{"lore_rambore_dod", "Ram'Bore - Unbekannt"},
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
        public static class DoDBLocalizationPatch
        {
            public static void Postfix(Localization __instance, string language)
            {
                init(language, __instance);
                UpdateDictinary();
            }
        }
    }
}
