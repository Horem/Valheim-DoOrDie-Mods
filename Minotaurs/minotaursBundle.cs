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

namespace Minotaurs
{
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
	[BepInDependency("com.jotunn.jotunn", BepInDependency.DependencyFlags.HardDependency)]
	internal class minotaursBundle : BaseUnityPlugin
	{
		public const string PluginGUID = "horemvore.Minotaurs";

		public const string PluginName = "Minotaurs";

		public const string PluginVersion = "0.0.1";

		public static GameObject Minotaur1;
		public static GameObject Minotaur2;
		public static GameObject Minotaur3;
		public static GameObject Minotaur4;
		public static GameObject MAttack1;
		public static GameObject MAttack2;
		public static GameObject MAttack3;
		public static GameObject FMAttack1;
		public static GameObject FMAttack2;
		public static GameObject FMAttack3;
		public static GameObject SFX1;
		public static GameObject SFX2;
		public static GameObject SFX3;
		public static GameObject SFX4;
		public static GameObject SFX5;
		//public static GameObject SFX6;
		public static GameObject VFX1;
		public static GameObject VFX2;
		public static GameObject VFX3;

		public AssetBundle MinotaursBundle;
		private Harmony _harmony;
		internal static ManualLogSource Log;
		public static AssetBundle GetAssetBundleFromResources(string fileName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string text = executingAssembly.GetManifestResourceNames().Single((string str) => str.EndsWith(fileName));
			using Stream stream = executingAssembly.GetManifestResourceStream(text);
			return AssetBundle.LoadFromStream(stream);
		}
		private void Awake()
		{
			Log = Logger;
			_harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "horemvore.Giants");
			LoadBundle();
			LoadAssets();
			AddGiants();
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
			MinotaursBundle = AssetUtils.LoadAssetBundleFromResources("minotaurs", Assembly.GetExecutingAssembly());
		}
		private void LoadAssets()
		{
			// Mobs
			Debug.Log("Minotaurs: Mobs");
			Minotaur4 = MinotaursBundle.LoadAsset<GameObject>("Minotaur_HM");
			Minotaur3 = MinotaursBundle.LoadAsset<GameObject>("ArmouredMinotaur_HM");
			Minotaur2 = MinotaursBundle.LoadAsset<GameObject>("FrostMinotaur_HM");
			Minotaur1 = MinotaursBundle.LoadAsset<GameObject>("ArmouredFrostMinotaur_HM");
			// Attacks
			Debug.Log("Minotaurs: Attacks");
			MAttack1 = MinotaursBundle.LoadAsset<GameObject>("Minotaur_Attack1_HM");
			MAttack2 = MinotaursBundle.LoadAsset<GameObject>("Minotaur_Attack2_HM");
			MAttack3 = MinotaursBundle.LoadAsset<GameObject>("Minotaur_Attack3_HM");
			FMAttack1 = MinotaursBundle.LoadAsset<GameObject>("FrostMinotaur_Attack1_HM");
			FMAttack2 = MinotaursBundle.LoadAsset<GameObject>("FrostMinotaur_Attack1_HM");
			FMAttack3 = MinotaursBundle.LoadAsset<GameObject>("FrostMinotaur_Attack1_HM");
			GameObject attack1 = MAttack1;
			CustomPrefab HGattack1 = new CustomPrefab(attack1, false);
			PrefabManager.Instance.AddPrefab(HGattack1);
			GameObject attack2 = MAttack2;
			CustomPrefab HGattack2 = new CustomPrefab(attack2, false);
			PrefabManager.Instance.AddPrefab(HGattack2);
			GameObject attack3 = MAttack3;
			CustomPrefab HGattack3 = new CustomPrefab(attack3, false);
			PrefabManager.Instance.AddPrefab(HGattack3);
			GameObject attack4 = FMAttack1;
			CustomPrefab MGattack1 = new CustomPrefab(attack4, false);
			PrefabManager.Instance.AddPrefab(MGattack1);
			GameObject attack5 = FMAttack2;
			CustomPrefab MGattack2 = new CustomPrefab(attack5, false);
			PrefabManager.Instance.AddPrefab(MGattack2);
			GameObject attack6 = FMAttack3;
			CustomPrefab MGattack3 = new CustomPrefab(attack6, false);
			PrefabManager.Instance.AddPrefab(MGattack3);
			//SFX
			Debug.Log("Minotaurs: SFX");
			SFX1 = MinotaursBundle.LoadAsset<GameObject>("SFX_MinotaurAlert_HM");
			SFX2 = MinotaursBundle.LoadAsset<GameObject>("SFX_MinotaurAttack_HM");
			SFX3 = MinotaursBundle.LoadAsset<GameObject>("SFX_MinotaurDeath_HM");
			SFX4 = MinotaursBundle.LoadAsset<GameObject>("SFX_MinotaurHit_HM");
			SFX5 = MinotaursBundle.LoadAsset<GameObject>("SFX_MinotaurIdle_HM");
			CustomPrefab sfx1 = new CustomPrefab(SFX1, false);
			PrefabManager.Instance.AddPrefab(sfx1);
			CustomPrefab sfx2 = new CustomPrefab(SFX2, false);
			PrefabManager.Instance.AddPrefab(sfx2);
			CustomPrefab sfx3 = new CustomPrefab(SFX3, false);
			PrefabManager.Instance.AddPrefab(sfx3);
			CustomPrefab sfx4 = new CustomPrefab(SFX4, false);
			PrefabManager.Instance.AddPrefab(sfx4);
			CustomPrefab sfx5 = new CustomPrefab(SFX5, false);
			PrefabManager.Instance.AddPrefab(sfx5);
			//VFX
			Debug.Log("Minotaurs: VFX");
			VFX1 = MinotaursBundle.LoadAsset<GameObject>("VFX_Blood_Hit_HM");
			VFX2 = MinotaursBundle.LoadAsset<GameObject>("VFX_Corpse_Destruction_HM");
			VFX3 = MinotaursBundle.LoadAsset<GameObject>("VFX_HitSparks_HM");
			CustomPrefab vfx1 = new CustomPrefab(VFX1, false);
			PrefabManager.Instance.AddPrefab(vfx1);
			CustomPrefab vfx2 = new CustomPrefab(VFX2, false);
			PrefabManager.Instance.AddPrefab(vfx2);
			CustomPrefab vfx3 = new CustomPrefab(VFX3, false);
			PrefabManager.Instance.AddPrefab(vfx3);
		}
		private void AddGiants()
		{
			try
			{
				Debug.Log("Minotaurs: Minotaur");
				var MinotaurMob = new CustomCreature(Minotaur4, false,
					new CreatureConfig
					{
						DropConfigs = new[]
						{
							new DropConfig
							{
								Item = "Coins",
								MinAmount = 23,
								MaxAmount = 78,
								Chance = 100
							},
							new DropConfig
							{
								Item = "SilverNecklace",
								MinAmount = 3,
								MaxAmount = 5,
								Chance = 15
							},
							new DropConfig
							{
								Item = "FreezeGland",
								MinAmount = 2,
								MaxAmount = 4,
								Chance = 25
							},
							new DropConfig
							{
								Item = "YmirRemains",
								MinAmount = 1,
								MaxAmount = 3,
								Chance = 5
							}
						}
					});
				CreatureManager.Instance.AddCreature(MinotaurMob);
				Debug.Log("Minotaurs: Armoured Minotaur");
				var ArmorMinotaurMob = new CustomCreature(Minotaur3, false,
					new CreatureConfig
					{
						DropConfigs = new[]
						{
							new DropConfig
							{
								Item = "Coins",
								MinAmount = 17,
								MaxAmount = 41,
								Chance = 100
							},
							new DropConfig
							{
								Item = "LoxPelt",
								MinAmount = 2,
								MaxAmount = 4,
								Chance = 10
							},
							new DropConfig
							{
								Item = "CookedLoxMeat",
								MinAmount = 2,
								MaxAmount = 8,
								Chance = 10
							},
							new DropConfig
							{
								Item = "Cloudberry",
								MinAmount = 3,
								MaxAmount = 12,
								Chance = 25
							}
						}
					});
				CreatureManager.Instance.AddCreature(ArmorMinotaurMob);
				Debug.Log("Minotaurs: Frost Minotaur");
				var FrostMinotaurMob = new CustomCreature(Minotaur2, false,
					new CreatureConfig
					{
						DropConfigs = new[]
						{
							new DropConfig
							{
								Item = "Coins",
								MinAmount = 10,
								MaxAmount = 28,
								Chance = 100
							},
							new DropConfig
							{
								Item = "WolfPelt",
								MinAmount = 2,
								MaxAmount = 4,
								Chance = 15
							},
							new DropConfig
							{
								Item = "FreezeGland",
								MinAmount = 2,
								MaxAmount = 4,
								Chance = 10
							},
							new DropConfig
							{
								Item = "CookedWolfMeat",
								MinAmount = 3,
								MaxAmount = 5,
								Chance = 25
							}
						}
					});
				CreatureManager.Instance.AddCreature(FrostMinotaurMob);
				Debug.Log("Minotaurs: Armoured Frost Minotaur");
				var ArmorFrostMinotaurMob = new CustomCreature(Minotaur1, false,
					new CreatureConfig
					{
						DropConfigs = new[]
						{
							new DropConfig
							{
								Item = "Coins",
								MinAmount = 10,
								MaxAmount = 28,
								Chance = 100
							},
							new DropConfig
							{
								Item = "WolfPelt",
								MinAmount = 2,
								MaxAmount = 4,
								Chance = 15
							},
							new DropConfig
							{
								Item = "FreezeGland",
								MinAmount = 2,
								MaxAmount = 4,
								Chance = 10
							},
							new DropConfig
							{
								Item = "CookedWolfMeat",
								MinAmount = 3,
								MaxAmount = 5,
								Chance = 25
							}
						}
					});
				CreatureManager.Instance.AddCreature(ArmorFrostMinotaurMob);
			}
			catch (Exception ex)
			{
				Logger.LogWarning($"Exception caught while adding custom creatures: {ex}");
			}
			finally
			{
				MinotaursBundle.Unload(false);
			}
		}
		public static void ConfigureBiomeSpawners(ISpawnerConfigurationCollection config)
		{
			Debug.Log("Minotaurs: Configure Spawns");
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
			Debug.Log("Minotaurs: Create Spawns");
			try
			{
				config.ConfigureWorldSpawner(26_103)
					.SetPrefabName("ArmouredFrostMinotaur_HM")
					.SetTemplateName("Armoured Frost Minotaur")
					.SetConditionBiomes(Heightmap.Biome.DeepNorth)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(210))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionAltitudeMin(5)
					.SetSpawnAtDistanceToPlayerMin(75)
					.SetSpawnAtDistanceToPlayerMax(125)
					;
				config.ConfigureWorldSpawner(26_102)
					.SetPrefabName("FrostMinotaur_HM")
					.SetTemplateName("Frost Minotaur")
					.SetConditionBiomes(Heightmap.Biome.DeepNorth)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(210))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionAltitudeMin(5)
					.SetSpawnAtDistanceToPlayerMin(75)
					.SetSpawnAtDistanceToPlayerMax(125)
					;
				config.ConfigureWorldSpawner(26_101)
					.SetPrefabName("ArmouredMinotaur_HM")
					.SetTemplateName("Armoured Minotaur")
					.SetConditionBiomes(Heightmap.Biome.Mistlands)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(210))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionAltitudeMin(5)
					.SetSpawnAtDistanceToPlayerMin(75)
					.SetSpawnAtDistanceToPlayerMax(125)
					;
				config.ConfigureWorldSpawner(26_100)
					.SetPrefabName("Minotaur_HM")
					.SetTemplateName("Minotaur")
					.SetConditionBiomes(Heightmap.Biome.Mistlands)
					.SetSpawnChance(8)
					.SetSpawnInterval(TimeSpan.FromSeconds(210))
					.SetPackSizeMin(1)
					.SetPackSizeMax(2)
					.SetMaxSpawned(2)
					.SetConditionAltitudeMin(5)
					.SetSpawnAtDistanceToPlayerMin(75)
					.SetSpawnAtDistanceToPlayerMax(125)
					;
			}
			catch (Exception e)
			{
				Log.LogError(e);
			}
		}
	}
}