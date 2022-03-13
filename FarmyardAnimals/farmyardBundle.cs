﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;
using SpawnThat.Spawners;
using SpawnThat.Spawners.LocalSpawner;
using SpawnThat.Spawners.WorldSpawner;

namespace FarmyardAnimals
{
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
	[BepInDependency("com.jotunn.jotunn", BepInDependency.DependencyFlags.HardDependency)]
	internal class farmyardBundle : BaseUnityPlugin
	{
		public const string PluginGUID = "horemvore.FarmyardAnimals";

		public const string PluginName = "FarmyardAnimals";

		public const string PluginVersion = "0.0.1";

		internal static ManualLogSource Log;
		// Animals
		public static GameObject Sheep;
		public static GameObject Lamb;
		public static GameObject Goat;
		public static GameObject Goose;
		public static GameObject Gosling;
		public static GameObject ChickenB;
		public static GameObject ChickB;
		public static GameObject EggB;
		public static GameObject ChickenBW;
		public static GameObject ChickBW;
		public static GameObject EggBW;
		public static GameObject ChickenW;
		public static GameObject ChickW;
		public static GameObject EggW;
		public static GameObject CowBW;
		public static GameObject LonghornB;
		public static GameObject LonghornW;
		public static GameObject Highland;
		public static GameObject CowB;
		public static GameObject Chester;
		public static GameObject PiggletC;
		public static GameObject Oxford;
		public static GameObject PiggletO;
		public static GameObject Mulefoot;
		public static GameObject PiggletM;
		public static GameObject OldSpots;
		public static GameObject PiggletOS;
		public static GameObject EggG;
		// Carcass
		public static GameObject Poultry;
		public static GameObject LegS;
		public static GameObject PieceS;
		public static GameObject QuarterS;
		// Materials
		public static GameObject PoultryLeg;
		public static GameObject PoultryBreast;
		public static GameObject PoultryWhole;
		public static GameObject MeatRoll;
		public static GameObject SmallSteak;
		public static GameObject Steak;
		public static GameObject BurgerMeat;
		public static GameObject MeatChunks;
		public static GameObject PrimeCut;
		// Food
		public static GameObject BurgerRound;
		public static GameObject Chop;
		public static GameObject CookedSteak;
		public static GameObject FriedSteak;
		public static GameObject FriedMeat;
		public static GameObject CookedJoint;
		public static GameObject Milk;
		public static GameObject Drumstick;
		public static GameObject CookedBreast;
		public static GameObject RoastedPoultry;
		// Stations
		public static GameObject ButcherStation;
		// Pieces
		public static GameObject MilkCow;
		// Asset Bundles
		public AssetBundle FarmyardBundle;
		// Harmony (for localization)
		private Harmony _harmony;
		public static AssetBundle GetAssetBundleFromResources(string fileName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string text = executingAssembly.GetManifestResourceNames().Single((string str) => str.EndsWith(fileName));
			using Stream stream = executingAssembly.GetManifestResourceStream(text);
			return AssetBundle.LoadFromStream(stream);
		}
		private void Awake()
		{
			_harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "horemvore.FarmyardAnimals");
			Log = Logger;
			LoadBundle();
			LoadAssets();
			CreateStations();
			CreateCarcassParts();
			CreateMaterials();
			AddRecipes();
			CreatePieces();
			AddFoodItems();
			UpdateOven();
			AddNewAnimals();
			UnloadBundle();
			try
			{
				SpawnerConfigurationManager.OnConfigure += ConfigureBiomeSpawners;
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
			}
		}
		public void LoadBundle()
		{
			FarmyardBundle = AssetUtils.LoadAssetBundleFromResources("farmyard", Assembly.GetExecutingAssembly());
		}
		private void LoadAssets()
		{
			Debug.Log("FarmyardAnimals: Carcass Parts");
			Poultry = FarmyardBundle.LoadAsset<GameObject>("PoultryCarcass_FYA");
			LegS = FarmyardBundle.LoadAsset<GameObject>("LegS_FYA");
			PieceS = FarmyardBundle.LoadAsset<GameObject>("PieceS_FYA");
			QuarterS = FarmyardBundle.LoadAsset<GameObject>("QuarterS_FYA");

			Debug.Log("FarmyardAnimals: Materials");
			PoultryLeg = FarmyardBundle.LoadAsset<GameObject>("PoultryLeg_FYA");
			PoultryBreast = FarmyardBundle.LoadAsset<GameObject>("PoultryBreast_FYA");
			PoultryWhole = FarmyardBundle.LoadAsset<GameObject>("PoultryWhole_FYA");
			MeatRoll = FarmyardBundle.LoadAsset<GameObject>("MeatRoll_FYA");
			SmallSteak = FarmyardBundle.LoadAsset<GameObject>("SmallSteak_FYA");
			Steak = FarmyardBundle.LoadAsset<GameObject>("Steak_FYA");
			BurgerMeat = FarmyardBundle.LoadAsset<GameObject>("BurgerMeat_FYA");
			MeatChunks = FarmyardBundle.LoadAsset<GameObject>("DicedMeat_FYA");
			PrimeCut = FarmyardBundle.LoadAsset<GameObject>("PrimeCut_FYA");

			Debug.Log("FarmyardAnimals: Food");
			BurgerRound = FarmyardBundle.LoadAsset<GameObject>("BurgerRound_FYA");
			Chop = FarmyardBundle.LoadAsset<GameObject>("Chop_FYA");
			CookedSteak = FarmyardBundle.LoadAsset<GameObject>("CookedSteak_FYA");
			FriedSteak = FarmyardBundle.LoadAsset<GameObject>("FriedSteak_FYA");
			FriedMeat = FarmyardBundle.LoadAsset<GameObject>("FriedMeat_FYA");
			CookedJoint = FarmyardBundle.LoadAsset<GameObject>("CookedJoint_FYA");
			Drumstick = FarmyardBundle.LoadAsset<GameObject>("Drumstick_FYA");
			CookedBreast = FarmyardBundle.LoadAsset<GameObject>("CookedBreast_FYA");
			RoastedPoultry = FarmyardBundle.LoadAsset<GameObject>("RoastPoultry_FYA");
			Milk = FarmyardBundle.LoadAsset<GameObject>("Milk_FYA");

			Debug.Log("FarmyardAnimals: Stations");
			ButcherStation = FarmyardBundle.LoadAsset<GameObject>("ButchersBench_FYA");

			Debug.Log("FarmyardAnimals: Pieces");
			MilkCow = FarmyardBundle.LoadAsset<GameObject>("CowStall_FYA");

			Debug.Log("FarmyardAnimals: Creatures");
			Sheep = FarmyardBundle.LoadAsset<GameObject>("Sheep_FYA");
			Lamb = FarmyardBundle.LoadAsset<GameObject>("Lamb_FYA");
			Goat = FarmyardBundle.LoadAsset<GameObject>("Goat_FYA");
			Gosling = FarmyardBundle.LoadAsset<GameObject>("Gosling_FYA");
			Goose = FarmyardBundle.LoadAsset<GameObject>("Goose_FYA");
			ChickenB = FarmyardBundle.LoadAsset<GameObject>("ChickenB_FYA");
			ChickB = FarmyardBundle.LoadAsset<GameObject>("ChickB_FYA");
			EggB = FarmyardBundle.LoadAsset<GameObject>("EggB_FYA");
			ChickenBW = FarmyardBundle.LoadAsset<GameObject>("ChickenBW_FYA");
			ChickBW = FarmyardBundle.LoadAsset<GameObject>("ChickBW_FYA");
			EggBW = FarmyardBundle.LoadAsset<GameObject>("EggBW_FYA");
			ChickenW = FarmyardBundle.LoadAsset<GameObject>("ChickenW_FYA");
			ChickW = FarmyardBundle.LoadAsset<GameObject>("ChickW_FYA");
			EggW = FarmyardBundle.LoadAsset<GameObject>("EggW_FYA");
			CowB = FarmyardBundle.LoadAsset<GameObject>("CowB_FYA");
			CowBW = FarmyardBundle.LoadAsset<GameObject>("CowBW_FYA");
			LonghornB = FarmyardBundle.LoadAsset<GameObject>("LonghornB_FYA");
			LonghornW = FarmyardBundle.LoadAsset<GameObject>("LonghornW_FYA");
			Highland = FarmyardBundle.LoadAsset<GameObject>("Highland_FYA");
			Chester = FarmyardBundle.LoadAsset<GameObject>("Chester_FYA");
			PiggletC = FarmyardBundle.LoadAsset<GameObject>("PiggletC_FYA");
			Oxford = FarmyardBundle.LoadAsset<GameObject>("Oxford_FYA");
			PiggletO = FarmyardBundle.LoadAsset<GameObject>("PiggletO_FYA");
			Mulefoot = FarmyardBundle.LoadAsset<GameObject>("Mulefoot_FYA");
			PiggletM = FarmyardBundle.LoadAsset<GameObject>("PiggletM_FYA");
			OldSpots = FarmyardBundle.LoadAsset<GameObject>("OldSpots_FYA");
			PiggletOS = FarmyardBundle.LoadAsset<GameObject>("PiggletOS_FYA");
			EggG = FarmyardBundle.LoadAsset<GameObject>("EggG_FYA");

			Debug.Log("FarmyardAnimals: SFX");
			GameObject SFXCattle1 = FarmyardBundle.LoadAsset<GameObject>("SFX_Cattle_Idle_WL");
			PrefabManager.Instance.AddPrefab(SFXCattle1);
			GameObject SFXCattle2 = FarmyardBundle.LoadAsset<GameObject>("SFX_Cattle_Hit_WL");
			PrefabManager.Instance.AddPrefab(SFXCattle2);
			GameObject SFXCattle3 = FarmyardBundle.LoadAsset<GameObject>("SFX_Cattle_GetHit_WL");
			PrefabManager.Instance.AddPrefab(SFXCattle3);
			GameObject SFXLonghorn1 = FarmyardBundle.LoadAsset<GameObject>("SFX_Longhorn_Idle_WL");
			PrefabManager.Instance.AddPrefab(SFXLonghorn1);
			GameObject SFXHighland1 = FarmyardBundle.LoadAsset<GameObject>("SFX_Scotland_Idle_WL");
			PrefabManager.Instance.AddPrefab(SFXHighland1);
			GameObject SFXChicken1 = FarmyardBundle.LoadAsset<GameObject>("SFX_Chicken_Idle_WL");
			PrefabManager.Instance.AddPrefab(SFXChicken1);
			GameObject SFXChicken2 = FarmyardBundle.LoadAsset<GameObject>("SFX_Chicken_Hit_WL");
			PrefabManager.Instance.AddPrefab(SFXChicken2);
			GameObject SFXChicken3 = FarmyardBundle.LoadAsset<GameObject>("SFX_Chicken_GetHit_WL");
			PrefabManager.Instance.AddPrefab(SFXChicken3);
			GameObject SFXPig1 = FarmyardBundle.LoadAsset<GameObject>("SFX_Pig_Idle_WL");
			PrefabManager.Instance.AddPrefab(SFXPig1);
			GameObject SFXPig2 = FarmyardBundle.LoadAsset<GameObject>("SFX_Pig_Hit_WL");
			PrefabManager.Instance.AddPrefab(SFXPig2);
			GameObject SFXPig3 = FarmyardBundle.LoadAsset<GameObject>("SFX_Pig_GetHit_WL");
			PrefabManager.Instance.AddPrefab(SFXPig3);
			GameObject SFXButcherChop = FarmyardBundle.LoadAsset<GameObject>("SFX_ButcherChop_FYA");
			PrefabManager.Instance.AddPrefab(SFXButcherChop);

			Debug.Log("FarmyardAnimals: VFX");
			GameObject VFXCarcass = FarmyardBundle.LoadAsset<GameObject>("VFX_Carcass_Destruction_FYA");
			PrefabManager.Instance.AddPrefab(VFXCarcass);
			GameObject VFXCorpse = FarmyardBundle.LoadAsset<GameObject>("VFX_Corpse_Destruction_FYA");
			PrefabManager.Instance.AddPrefab(VFXCorpse);

			Debug.Log("FarmyardAnimals: Carcass");
			GameObject Corpse = FarmyardBundle.LoadAsset<GameObject>("CarcassS_FYA");
			PrefabManager.Instance.AddPrefab(Corpse);
		}
		private void CreateCarcassParts()
		{
			GameObject dropable5 = QuarterS;
			CustomItem customItem5 = new CustomItem(dropable5, false);
			ItemManager.Instance.AddItem(customItem5);
			GameObject dropable4 = PieceS;
			CustomItem customItem4 = new CustomItem(dropable4, false);
			ItemManager.Instance.AddItem(customItem4);
			GameObject dropable3 = LegS;
			CustomItem customItem3 = new CustomItem(dropable3, false);
			ItemManager.Instance.AddItem(customItem3);
			GameObject dropable2 = Milk;
			CustomItem customItem2 = new CustomItem(dropable2, false);
			ItemManager.Instance.AddItem(customItem2);
			GameObject dropable1 = Poultry;
			CustomItem customItem1 = new CustomItem(dropable1, false);
			ItemManager.Instance.AddItem(customItem1);
		}
		private void CreateMaterials()
		{
			// Burger Meat
			Debug.Log("FarmyardAnimals: BurgerMeat");
			GameObject material9 = BurgerMeat;
			CustomItem customMat9 = new CustomItem(material9, false, new ItemConfig
			{
				Amount = 4,
				CraftingStation = "ButchersBench_FYA",
				MinStationLevel = 1,
				Requirements = new RequirementConfig[1]
				{
					new RequirementConfig
					{
						Item = "DicedMeat_FYA",
						Amount = 6
					}
				}
			});
			ItemManager.Instance.AddItem(customMat9);
			// Chop
			Debug.Log("FarmyardAnimals: PrimeCut");
			GameObject material8 = PrimeCut;
			CustomItem customMat8 = new CustomItem(material8, false, new ItemConfig
			{
				Amount = 8,
				CraftingStation = "ButchersBench_FYA",
				MinStationLevel = 1,
				Requirements = new RequirementConfig[1]
				{
					new RequirementConfig
					{
						Item = "QuarterS_FYA",
						Amount = 1
					}
				}
			});
			ItemManager.Instance.AddItem(customMat8);
			// Small Steak
			Debug.Log("FarmyardAnimals: SmallSteak");
			GameObject material7 = SmallSteak;
			CustomItem customMat7 = new CustomItem(material7, false, new ItemConfig
			{
				Amount = 4,
				CraftingStation = "ButchersBench_FYA",
				MinStationLevel = 1,
				Requirements = new RequirementConfig[1]
				{
					new RequirementConfig
					{
						Item = "QuarterS_FYA",
						Amount = 1
					}
				}
			});
			ItemManager.Instance.AddItem(customMat7);
			// Steak
			Debug.Log("FarmyardAnimals: Steak");
			GameObject material6 = Steak;
			CustomItem customMat6 = new CustomItem(material6, false, new ItemConfig
			{
				Amount = 4,
				CraftingStation = "ButchersBench_FYA",
				MinStationLevel = 1,
				Requirements = new RequirementConfig[1]
				{
					new RequirementConfig
					{
						Item = "QuarterS_FYA",
						Amount = 1
					}
				}
			});
			ItemManager.Instance.AddItem(customMat6);
			// Meat Chunks
			Debug.Log("FarmyardAnimals: MeatChunks");
			GameObject material5 = MeatChunks;
			CustomItem customMat5 = new CustomItem(material5, false, new ItemConfig
			{
				Amount = 4,
				CraftingStation = "ButchersBench_FYA",
				MinStationLevel = 1,
				Requirements = new RequirementConfig[1]
				{
					new RequirementConfig
					{
						Item = "LegS_FYA",
						Amount = 1
					}
				}
			});
			ItemManager.Instance.AddItem(customMat5);
			// Meat Roll
			Debug.Log("FarmyardAnimals: MeatRoll");
			GameObject material4 = MeatRoll;
			CustomItem customMat4 = new CustomItem(material4, false, new ItemConfig
			{
				Amount = 1,
				CraftingStation = "ButchersBench_FYA",
				MinStationLevel = 1,
				Requirements = new RequirementConfig[1]
				{
					new RequirementConfig
					{
						Item = "LegS_FYA",
						Amount = 1
					}
				}
			});
			ItemManager.Instance.AddItem(customMat4);
			// Poultry Whole
			Debug.Log("FarmyardAnimals: PoultryWhole");
			GameObject material3 = PoultryWhole;
			CustomItem customMat3 = new CustomItem(material3, false, new ItemConfig
			{
				Amount = 1,
				CraftingStation = "ButchersBench_FYA",
				MinStationLevel = 1,
				Requirements = new RequirementConfig[1]
				{
					new RequirementConfig
					{
						Item = "PoultryCarcass_FYA",
						Amount = 1
					}
				}
			});
			ItemManager.Instance.AddItem(customMat3);
			// Poultry Breast
			Debug.Log("FarmyardAnimals: PoultryBreast");
			GameObject material2 = PoultryBreast;
			CustomItem customMat2 = new CustomItem(material2, false, new ItemConfig
			{
				Amount = 2,
				CraftingStation = "ButchersBench_FYA",
				MinStationLevel = 1,
				Requirements = new RequirementConfig[1]
				{
					new RequirementConfig
					{
						Item = "PoultryCarcass_FYA",
						Amount = 1
					}
				}
			});
			ItemManager.Instance.AddItem(customMat2);
			// Poultry Leg
			Debug.Log("FarmyardAnimals: PoultryLeg");
			GameObject material1 = PoultryLeg;
			CustomItem customMat1 = new CustomItem(material1, false, new ItemConfig
			{
				Amount = 2,
				CraftingStation = "ButchersBench_FYA",
				MinStationLevel = 1,
				Requirements = new RequirementConfig[1]
				{
					new RequirementConfig
					{
						Item = "PoultryCarcass_FYA",
						Amount = 1
					}
				}
			});
			ItemManager.Instance.AddItem(customMat1);
		}
		private void AddRecipes()
		{
			// Meat Chunks Small
			CustomRecipe meatRecipe1 = new CustomRecipe(new RecipeConfig()
			{
				Name = "$item_meatchunks3_fya",
				Amount = 3,
				Item = "DicedMeat_FYA",
				CraftingStation = "ButchersBench_FYA",
				Requirements = new RequirementConfig[]
				{
					new RequirementConfig { Item = "PieceS_FYA", Amount = 1 }
				}
			});
			ItemManager.Instance.AddRecipe(meatRecipe1);
			// Meat Chunks Small
			CustomRecipe meatRecipe2 = new CustomRecipe(new RecipeConfig()
			{
				Name = "$item_meatchunks16_fya",
				Amount = 16,
				Item = "DicedMeat_FYA",
				CraftingStation = "ButchersBench_FYA",
				Requirements = new RequirementConfig[]
				{
					new RequirementConfig { Item = "QuarterS_FYA", Amount = 1 }
				}
			});
			ItemManager.Instance.AddRecipe(meatRecipe2);
		}
		private void AddFoodItems()
		{
			GameObject dropable9 = RoastedPoultry;
			CustomItem customItem9 = new CustomItem(dropable9, false);
			ItemManager.Instance.AddItem(customItem9);
			GameObject dropable8 = CookedBreast;
			CustomItem customItem8 = new CustomItem(dropable8, false);
			ItemManager.Instance.AddItem(customItem8);
			GameObject dropable7 = Drumstick;
			CustomItem customItem7 = new CustomItem(dropable7, false);
			ItemManager.Instance.AddItem(customItem7);
			GameObject dropable6 = CookedJoint;
			CustomItem customItem6 = new CustomItem(dropable6, false);
			ItemManager.Instance.AddItem(customItem6);
			GameObject dropable5 = FriedMeat;
			CustomItem customItem5 = new CustomItem(dropable5, false);
			ItemManager.Instance.AddItem(customItem5);
			GameObject dropable4 = FriedSteak;
			CustomItem customItem4 = new CustomItem(dropable4, false);
			ItemManager.Instance.AddItem(customItem4);
			GameObject dropable3 = CookedSteak;
			CustomItem customItem3 = new CustomItem(dropable3, false);
			ItemManager.Instance.AddItem(customItem3);
			GameObject dropable2 = Chop;
			CustomItem customItem2 = new CustomItem(dropable2, false);
			ItemManager.Instance.AddItem(customItem2);
			GameObject dropable1 = BurgerRound;
			CustomItem customItem1 = new CustomItem(dropable1, false);
			ItemManager.Instance.AddItem(customItem1);
		}
		private void UpdateOven()
		{
			CustomItemConversion food9 = new CustomItemConversion(new CookingConversionConfig
			{
				Station = "piece_oven",
				FromItem = "SmallSteak_FYA",
				ToItem = "FriedSteak_FYA",
				CookTime = 15f
			});
			ItemManager.Instance.AddItemConversion(food9);
			CustomItemConversion food8 = new CustomItemConversion(new CookingConversionConfig
			{
				Station = "piece_oven",
				FromItem = "DicedMeat_FYA",
				ToItem = "FriedMeat_FYA",
				CookTime = 20f
			});
			ItemManager.Instance.AddItemConversion(food8);
			CustomItemConversion food7 = new CustomItemConversion(new CookingConversionConfig
			{
				Station = "piece_oven",
				FromItem = "MeatRoll_FYA",
				ToItem = "CookedJoint_FYA",
				CookTime = 180f
			});
			ItemManager.Instance.AddItemConversion(food7);
			CustomItemConversion food6 = new CustomItemConversion(new CookingConversionConfig
			{
				Station = "piece_oven",
				FromItem = "Steak_FYA",
				ToItem = "CookedSteak_FYA",
				CookTime = 25f
			});
			ItemManager.Instance.AddItemConversion(food6);
			CustomItemConversion food5 = new CustomItemConversion(new CookingConversionConfig
			{
				Station = "piece_oven",
				FromItem = "PrimeCut_FYA",
				ToItem = "Chop_FYA",
				CookTime = 20f
			});
			ItemManager.Instance.AddItemConversion(food5);
			CustomItemConversion food4 = new CustomItemConversion(new CookingConversionConfig
			{
				Station = "piece_oven",
				FromItem = "BurgerMeat_FYA",
				ToItem = "BurgerRound_FYA",
				CookTime = 15f
			});
			ItemManager.Instance.AddItemConversion(food4);
			CustomItemConversion food3 = new CustomItemConversion(new CookingConversionConfig
			{
				Station = "piece_oven",
				FromItem = "PoultryBreast_FYA",
				ToItem = "CookedBreast_FYA",
				CookTime = 25f
			});
			ItemManager.Instance.AddItemConversion(food3);
			CustomItemConversion food2 = new CustomItemConversion(new CookingConversionConfig
			{
				Station = "piece_oven",
				FromItem = "PoultryLeg_FYA",
				ToItem = "Drumstick_FYA",
				CookTime = 15f
			});
			ItemManager.Instance.AddItemConversion(food2);
			CustomItemConversion food1 = new CustomItemConversion(new CookingConversionConfig
			{
				Station = "piece_oven",
				FromItem = "PoultryWhole_FYA",
				ToItem = "RoastPoultry_FYA",
				CookTime = 120f
			});
			ItemManager.Instance.AddItemConversion(food1);
		}
		private void CreateStations()
		{
			var customPiece1 = new CustomPiece(ButcherStation, false, new PieceConfig
			{
				PieceTable = "_HammerPieceTable",
				Category = "Crafting",
				Requirements = new RequirementConfig[3
				]
				{
					new RequirementConfig
					{
						Item = "LeatherScraps",
						Amount = 25,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 10,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 50,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece1);
		}
		private void CreatePieces()
		{
			var customPiece1 = new CustomPiece(MilkCow, false, new PieceConfig
			{
				PieceTable = "_HammerPieceTable",
				Category = "Crafting",
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Carrot",
						Amount = 20,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 40,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece1);
		}
		private void AddNewAnimals()
		{
			GameObject animal28 = PiggletOS;
			CustomPrefab critter28 = new CustomPrefab(animal28, true);
			PrefabManager.Instance.AddPrefab(critter28);
			GameObject animal27 = OldSpots;
			CustomPrefab critter27 = new CustomPrefab(animal27, true);
			PrefabManager.Instance.AddPrefab(critter27);
			GameObject animal26 = PiggletM;
			CustomPrefab critter26 = new CustomPrefab(animal26, true);
			PrefabManager.Instance.AddPrefab(critter26);
			GameObject animal25 = Mulefoot;
			CustomPrefab critter25 = new CustomPrefab(animal25, true);
			PrefabManager.Instance.AddPrefab(critter25);
			GameObject animal24 = PiggletC;
			CustomPrefab critter24 = new CustomPrefab(animal24, true);
			PrefabManager.Instance.AddPrefab(critter24);
			GameObject animal23 = Chester;
			CustomPrefab critter23 = new CustomPrefab(animal23, true);
			PrefabManager.Instance.AddPrefab(critter23);
			GameObject animal22 = PiggletO;
			CustomPrefab critter22 = new CustomPrefab(animal22, true);
			PrefabManager.Instance.AddPrefab(critter22);
			GameObject animal21 = Oxford;
			CustomPrefab critter21 = new CustomPrefab(animal21, true);
			PrefabManager.Instance.AddPrefab(critter21);
			GameObject animal20 = Highland;
			CustomPrefab critter20 = new CustomPrefab(animal20, true);
			PrefabManager.Instance.AddPrefab(critter20);
			GameObject animal19 = LonghornW;
			CustomPrefab critter19 = new CustomPrefab(animal19, true);
			PrefabManager.Instance.AddPrefab(critter19);
			GameObject animal18 = LonghornB;
			CustomPrefab critter18 = new CustomPrefab(animal18, true);
			PrefabManager.Instance.AddPrefab(critter18);
			GameObject animal17 = CowBW;
			CustomPrefab critter17 = new CustomPrefab(animal17, true);
			PrefabManager.Instance.AddPrefab(critter17);
			GameObject animal16 = CowB;
			CustomPrefab critter16 = new CustomPrefab(animal16, true);
			PrefabManager.Instance.AddPrefab(critter16);
			GameObject animal15 = EggW;
			CustomPrefab critter15 = new CustomPrefab(animal15, true);
			PrefabManager.Instance.AddPrefab(critter15);
			GameObject animal14 = ChickW;
			CustomPrefab critter14 = new CustomPrefab(animal14, true);
			PrefabManager.Instance.AddPrefab(critter14);
			GameObject animal13 = ChickenW;
			CustomPrefab critter13 = new CustomPrefab(animal13, true);
			PrefabManager.Instance.AddPrefab(critter13);
			GameObject animal12 = EggBW;
			CustomPrefab critter12 = new CustomPrefab(animal12, true);
			PrefabManager.Instance.AddPrefab(critter12);
			GameObject animal11 = ChickBW;
			CustomPrefab critter11 = new CustomPrefab(animal11, true);
			PrefabManager.Instance.AddPrefab(critter11);
			GameObject animal10 = ChickenBW;
			CustomPrefab critter10 = new CustomPrefab(animal10, true);
			PrefabManager.Instance.AddPrefab(critter10);
			GameObject animal9 = EggB;
			CustomPrefab critter9 = new CustomPrefab(animal9, true);
			PrefabManager.Instance.AddPrefab(critter9);
			GameObject animal8 = ChickB;
			CustomPrefab critter8 = new CustomPrefab(animal8, true);
			PrefabManager.Instance.AddPrefab(critter8);
			GameObject animal7 = ChickenB;
			CustomPrefab critter7 = new CustomPrefab(animal7, true);
			PrefabManager.Instance.AddPrefab(critter7);
			GameObject animal6 = Goat;
			CustomPrefab critter6 = new CustomPrefab(animal6, true);
			PrefabManager.Instance.AddPrefab(critter6);
			GameObject animal4 = Lamb;
			CustomPrefab critter4 = new CustomPrefab(animal4, true);
			PrefabManager.Instance.AddPrefab(critter4);
			GameObject animal3 = Sheep;
			CustomPrefab critter3 = new CustomPrefab(animal3, true);
			PrefabManager.Instance.AddPrefab(critter3);
			GameObject animal5 = EggG;
			CustomPrefab critter5 = new CustomPrefab(animal5, true);
			PrefabManager.Instance.AddPrefab(critter5);
			GameObject animal2 = Gosling;
			CustomPrefab critter2 = new CustomPrefab(animal2, true);
			PrefabManager.Instance.AddPrefab(critter2);
			GameObject animal1 = Goose;
			CustomPrefab critter1 = new CustomPrefab(animal1, true);
			PrefabManager.Instance.AddPrefab(critter1);
		}
		private void UnloadBundle()
		{
			FarmyardBundle?.Unload(unloadAllLoadedObjects: false);
		}
		public static void ConfigureBiomeSpawners(ISpawnerConfigurationCollection config)
		{
			Debug.Log("Farmyard Animals: Configure Spawns");
			try
			{
				ConfigureWorldSpawner(config);
			}
			catch (Exception e)
			{
				System.Console.WriteLine($"Something went horribly wrong: {e.Message}\nStackTrace:\n{e.StackTrace}");
			}
		}
		private static void ConfigureWorldSpawner(ISpawnerConfigurationCollection config)
		{
			Debug.Log("Farmyard Animals: Create Spawns");
			try
			{
				config.ConfigureWorldSpawner(25_015)
					.SetPrefabName("LonghornB_FYA")
					.SetTemplateName("Longhorn Brown")
					.SetConditionBiomes(Heightmap.Biome.Plains)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionEnvironments("Rain")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("StoneHenge3")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					.SetModifierFaction(Character.Faction.PlainsMonsters)
					;
				config.ConfigureWorldSpawner(25_014)
					.SetPrefabName("LonghornW_FYA")
					.SetTemplateName("Longhorn White")
					.SetConditionBiomes(Heightmap.Biome.Plains)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionEnvironments("Rain")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("StoneHenge4")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					.SetModifierFaction(Character.Faction.PlainsMonsters)
					;
				config.ConfigureWorldSpawner(25_013)
					.SetPrefabName("Highland_FYA")
					.SetTemplateName("Highland")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionEnvironments("Rain")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodFarm1")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_012)
					.SetPrefabName("CowBW_FYA")
					.SetTemplateName("Cattle")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionEnvironments("Rain")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodFarm1")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_011)
					.SetPrefabName("CowB_FYA")
					.SetTemplateName("Cattle")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionEnvironments("Clear")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodFarm1")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_010)
					.SetPrefabName("OldSpots_FYA")
					.SetTemplateName("Oxford Pig")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionEnvironments("LightRain")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodFarm1")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_009)
					.SetPrefabName("Mulefoot_FYA")
					.SetTemplateName("Oxford Pig")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionEnvironments("Clear")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodFarm1")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_008)
					.SetPrefabName("Oxford_FYA")
					.SetTemplateName("Oxford Pig")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionEnvironments("Clear")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodVillage1")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_007)
					.SetPrefabName("Chester_FYA")
					.SetTemplateName("Chester Pig")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionEnvironments("LightRain")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodVillage1")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_006)
					.SetPrefabName("Goose_FYA")
					.SetTemplateName("Goose")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(2)
					.SetPackSizeMax(4)
					.SetMaxSpawned(2)
					.SetSpawnDuringNight(false)
					.SetConditionEnvironments("LightRain")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodVillage1")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_005)
					.SetPrefabName("Goat_FYA")
					.SetTemplateName("Goat")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(1)
					.SetPackSizeMax(3)
					.SetMaxSpawned(2)
					.SetSpawnDuringNight(false)
					.SetConditionEnvironments("Clear")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodVillage1")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_004)
					.SetPrefabName("Sheep_FYA")
					.SetTemplateName("Sheep")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(1)
					.SetPackSizeMax(3)
					.SetMaxSpawned(2)
					.SetSpawnDuringNight(false)
					.SetConditionEnvironments("Misty")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodVillage1")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_003)
					.SetPrefabName("Goose_FYA")
					.SetTemplateName("Goose")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(2)
					.SetPackSizeMax(4)
					.SetMaxSpawned(2)
					.SetSpawnDuringNight(false)
					.SetConditionEnvironments("Misty")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodHouse8")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_002)
					.SetPrefabName("ChickenW_FYA")
					.SetTemplateName("White Chicken")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(2)
					.SetPackSizeMax(4)
					.SetMaxSpawned(2)
					.SetConditionEnvironments("Twilight_Clear")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodHouse8")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_001)
					.SetPrefabName("ChickenBW_FYA")
					.SetTemplateName("Brown White Chicken")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(2)
					.SetPackSizeMax(4)
					.SetMaxSpawned(2)
					.SetSpawnDuringNight(false)
					.SetConditionEnvironments("LightRain")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodHouse8")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
				config.ConfigureWorldSpawner(25_000)
					.SetPrefabName("ChickenB_FYA")
					.SetTemplateName("Brown Chicken")
					.SetConditionBiomes(Heightmap.Biome.Meadows)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(300))
					.SetPackSizeMin(2)
					.SetPackSizeMax(4)
					.SetMaxSpawned(2)
					.SetSpawnDuringNight(false)
					.SetConditionEnvironments("Clear")
					.SetConditionAltitudeMin(10)
					.SetConditionAltitudeMax(65)
					.SetConditionLocation("WoodHouse8")
					.SetSpawnAtDistanceToPlayerMin(60)
					.SetSpawnAtDistanceToPlayerMax(100)
					;
			}
			catch (Exception e)
			{
				Log.LogError(e);
			}
		}
	}
}