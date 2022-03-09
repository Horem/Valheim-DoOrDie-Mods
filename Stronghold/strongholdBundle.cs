﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;

namespace Stronghold
{
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
	[BepInDependency("com.jotunn.jotunn", BepInDependency.DependencyFlags.HardDependency)]
	internal class strongholdBundle : BaseUnityPlugin
	{
		public const string PluginGUID = "horemvore.Stronghold";

		public const string PluginName = "Stronghold";

		public const string PluginVersion = "0.0.1";

		public AssetBundle StrongholdAssets;
		private Harmony _harmony;

		public static GameObject piece1;
		public static GameObject piece2;
		public static GameObject piece3;
		public static GameObject piece4;
		public static GameObject piece5;
		public static GameObject piece6;
		public static GameObject piece7;
		public static GameObject piece8;
		public static GameObject piece9;
		public static GameObject piece10;
		public static GameObject piece11;
		public static GameObject piece12;
		public static GameObject piece13;
		public static GameObject piece14;
		public static GameObject piece15;
		public static GameObject piece16;
		public static GameObject piece17;
		public static GameObject piece18;
		public static GameObject piece19;
		public static GameObject piece20;
		public static GameObject piece21;
		public static GameObject piece22;
		public static GameObject piece23;

		public static AssetBundle GetAssetBundleFromResources(string fileName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string text = executingAssembly.GetManifestResourceNames().Single((string str) => str.EndsWith(fileName));
			using Stream stream = executingAssembly.GetManifestResourceStream(text);
			return AssetBundle.LoadFromStream(stream);
		}
		private void Awake()
		{
			Debug.Log("Stronghold: Loading and Creating Assets");
			_harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "horemvore.Stronghold");
			LoadBundle();
			LoadAssets();
			CreatePieces();
			UnloadBundle();
		}
		public void LoadBundle()
		{
			StrongholdAssets = AssetUtils.LoadAssetBundleFromResources("stronghold", Assembly.GetExecutingAssembly());
		}
		public void LoadAssets()
		{
			piece1 = StrongholdAssets.LoadAsset<GameObject>("SHGateHouse");
			piece2 = StrongholdAssets.LoadAsset<GameObject>("SHWallMusteringHall");
			piece3 = StrongholdAssets.LoadAsset<GameObject>("SHTowerSquareTwoFloorCenter");
			piece4 = StrongholdAssets.LoadAsset<GameObject>("SHTowerSquareTwoFloorCorner");
			piece5 = StrongholdAssets.LoadAsset<GameObject>("SHTowerSquareTwoFloorJunction");
			piece6 = StrongholdAssets.LoadAsset<GameObject>("SHWallOpenTwoFloorWithLadder");
			piece7 = StrongholdAssets.LoadAsset<GameObject>("SHWallOpenTwoFloorWithNest");
			piece8 = StrongholdAssets.LoadAsset<GameObject>("SHWallOpenTwoFloorWithNestCapped");
			piece9 = StrongholdAssets.LoadAsset<GameObject>("SHWallOpenTwoFloor");
			piece10 = StrongholdAssets.LoadAsset<GameObject>("SHEnclosedTower");
			piece11 = StrongholdAssets.LoadAsset<GameObject>("SHBunkhouse");
			piece12 = StrongholdAssets.LoadAsset<GameObject>("SHWell");
			piece13 = StrongholdAssets.LoadAsset<GameObject>("SHOuterWallCovered");
			piece14 = StrongholdAssets.LoadAsset<GameObject>("SHOuterWallOpenCapped");
			piece15 = StrongholdAssets.LoadAsset<GameObject>("SHOuterWallOpen");
			piece16 = StrongholdAssets.LoadAsset<GameObject>("SHOuterWallTowerSquareCenter");
			piece17 = StrongholdAssets.LoadAsset<GameObject>("SHOuterWallTowerTransition");
			piece18 = StrongholdAssets.LoadAsset<GameObject>("SHOuterWallTowerRound");
			piece19 = StrongholdAssets.LoadAsset<GameObject>("SHOuterWallGate");
			piece20 = StrongholdAssets.LoadAsset<GameObject>("SHWatchtower");
			piece21 = StrongholdAssets.LoadAsset<GameObject>("SHTowerRoundWallEnd");
			piece22 = StrongholdAssets.LoadAsset<GameObject>("SHOuterWallCoverdCapped");
			piece23 = StrongholdAssets.LoadAsset<GameObject>("TowerDoorWT_SH");
		}
		private void CreatePieces()
		{
			Debug.Log("Stronghold: TowerDoorWT_SH");
			var customPiece23 = new CustomPiece(piece23, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 20,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Iron",
						Amount = 15,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece23);
			Debug.Log("Stronghold: SHOuterWallCoverdCapped");
			var customPiece22 = new CustomPiece(piece22, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 200,
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
			PieceManager.Instance.AddPiece(customPiece22);
			Debug.Log("Stronghold: SHTowerRoundWallEnd");
			var customPiece21 = new CustomPiece(piece21, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[3]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 110,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 80,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Iron",
						Amount = 50,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece21);
			Debug.Log("Stronghold: SHWatchtower");
			var customPiece20 = new CustomPiece(piece20, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 110,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 80,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece20);
			Debug.Log("Stronghold: SHOuterWallGate");
			var customPiece19 = new CustomPiece(piece19, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 125,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 75,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece19);
			Debug.Log("Stronghold: SHOuterWallTowerRound");
			var customPiece18 = new CustomPiece(piece18, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[3]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 110,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 80,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Iron",
						Amount = 50,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece18);
			Debug.Log("Stronghold: SHOuterWallTowerTransition");
			var customPiece17 = new CustomPiece(piece17, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[3]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 125,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 100,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Iron",
						Amount = 100,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece17);
			Debug.Log("Stronghold: SHOuterWallTowerSquareCenter");
			var customPiece16 = new CustomPiece(piece16, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[3]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 125,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 100,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Iron",
						Amount = 100,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece16);
			Debug.Log("Stronghold: SHOuterWallOpen");
			var customPiece15 = new CustomPiece(piece15, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[1]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 150,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece15);
			Debug.Log("Stronghold: SHOuterWallOpenCapped");
			var customPiece14 = new CustomPiece(piece14, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[1]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 200,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece14);
			Debug.Log("Stronghold: SHOuterWallCovered");
			var customPiece13 = new CustomPiece(piece13, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 150,
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
			PieceManager.Instance.AddPiece(customPiece13);
			Debug.Log("Stronghold: SHWell");
			var customPiece12 = new CustomPiece(piece12, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 200,
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
			PieceManager.Instance.AddPiece(customPiece12);
			Debug.Log("Stronghold: SHBunkhouse");
			var customPiece11 = new CustomPiece(piece11, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 300,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 200,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece11);
			Debug.Log("Stronghold: SHEnclosedTower");
			var customPiece10 = new CustomPiece(piece10, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 150,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 150,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece10);
			Debug.Log("Stronghold: SHWallOpenTwoFloor");
			var customPiece9 = new CustomPiece(piece9, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 75,
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
			PieceManager.Instance.AddPiece(customPiece9);
			Debug.Log("Stronghold: SHWallOpenTwoFloorWithNestCapped");
			var customPiece8 = new CustomPiece(piece8, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 125,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 75,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece8);
			Debug.Log("Stronghold: SHWallOpenTwoFloorWithNest");
			var customPiece7 = new CustomPiece(piece7, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 100,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 75,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece7);
			Debug.Log("Stronghold: SHWallOpenTwoFloorWithLadder");
			var customPiece6 = new CustomPiece(piece6, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 75,
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
			PieceManager.Instance.AddPiece(customPiece6);
			Debug.Log("Stronghold: SHTowerSquareTwoFloorJunction");
			var customPiece5 = new CustomPiece(piece5, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 75,
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
			PieceManager.Instance.AddPiece(customPiece5);
			Debug.Log("Stronghold: SHTowerSquareTwoFloorCorner");
			var customPiece4 = new CustomPiece(piece4, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 125,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 100,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece4);
			Debug.Log("Stronghold: SHTowerSquareTwoFloorCenter");
			var customPiece3 = new CustomPiece(piece3, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 125,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 100,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece3);
			Debug.Log("Stronghold: SHWallMusteringHall");
			var customPiece2 = new CustomPiece(piece2, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 200,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 125,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece2);
			Debug.Log("Stronghold: SHGateHouse");
			var customPiece1 = new CustomPiece(piece1, true, new PieceConfig
			{
				PieceTable = "Hammer",
				Category = "Stronghold",
				AllowedInDungeons = true,
				Requirements = new RequirementConfig[2]
				{
					new RequirementConfig
					{
						Item = "Stone",
						Amount = 150,
						Recover = true
					},
					new RequirementConfig
					{
						Item = "Wood",
						Amount = 150,
						Recover = true
					}
				}
			});
			PieceManager.Instance.AddPiece(customPiece1);
		}
		private void UnloadBundle()
		{
			StrongholdAssets?.Unload(unloadAllLoadedObjects: false);
		}
	}
}