using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D3C RID: 3388
	[NullableContext(1)]
	[Nullable(0)]
	public static class VfxCmd
	{
		// Token: 0x17002579 RID: 9593
		// (get) Token: 0x06008883 RID: 34947 RVA: 0x002DF81C File Offset: 0x002DDA1C
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				IEnumerable<string> enumerable = new string[]
				{
					"vfx/vfx_block", "vfx/vfx_attack_slash", "vfx/vfx_attack_blunt", "vfx/vfx_gaze", "vfx/vfx_bloody_impact", "vfx/vfx_bite", "vfx/vfx_chain", "vfx/vfx_flying_slash", "vfx/vfx_coin_explosion_small", "vfx/vfx_coin_explosion_regular",
					"vfx/vfx_coin_explosion_jumbo", "vfx/vfx_adrenaline", "vfx/vfx_rock_shatter", "vfx/vfx_scratch", "vfx/vfx_sandy_impact", "vfx/vfx_attack_lightning", "vfx/vfx_giant_horizontal_slash", "vfx/vfx_dagger_throw", "vfx/vfx_dagger_spray", "vfx/vfx_dramatic_stab",
					"vfx/vfx_slime_impact", "vfx/vfx_thrash", "vfx/vfx_cross_heal", "vfx/vfx_heavy_blunt", "vfx/vfx_starry_impact", "vfx/vfx_scream", "vfx/vfx_spooky_scream"
				};
				Func<string, string> func;
				if ((func = VfxCmd.<>O.<0>__GetScenePath) == null)
				{
					func = (VfxCmd.<>O.<0>__GetScenePath = new Func<string, string>(SceneHelper.GetScenePath));
				}
				return enumerable.Select<string, string>(func).Concat<string>(new <>z__ReadOnlyArray<string>(new string[]
				{
					NBigSlashImpactVfx.scenePath,
					NBigSlashVfx.scenePath,
					NCardExhaustVfx.scenePath,
					NCardExhaustQuickVfx.scenePath,
					NCardRemoveVfx.scenePath,
					NCardTransformShineVfx.scenePath,
					NDaggerSprayFlurryVfx.scenePath,
					NDaggerSprayImpactVfx.scenePath,
					NFireBurningVfx.scenePath,
					NFireBurstVfx.scenePath,
					NGaseousImpactVfx.scenePath,
					NGoopyImpactVfx.scenePath,
					NHyperbeamImpactVfx.scenePath,
					NHyperbeamVfx.scenePath,
					NItemThrowVfx.scenePath,
					NLargeMagicMissileVfx.scenePath,
					NLiquidOverlayVfx.scenePath,
					NLowHpBorderVfx.scenePath,
					NMinionDiveBombVfx.scenePath,
					NPoisonImpactVfx.scenePath,
					NScratchVfx.scenePath,
					NShivThrowVfx.scenePath,
					NSmallMagicMissileVfx.scenePath,
					NSplashVfx.scenePath,
					NSporeImpactVfx.scenePath,
					NSweepingBeamImpactVfx.scenePath,
					NSweepingBeamVfx.scenePath,
					NWormyImpactVfx.scenePath
				})).Concat<string>(NHealNumVfx.AssetPaths);
			}
		}

		// Token: 0x06008884 RID: 34948 RVA: 0x002DFA48 File Offset: 0x002DDC48
		public static void PlayFullScreenInCombat(string path, [Nullable(2)] Creature spawner)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			Control control = ((spawner != null) ? spawner.GetVfxContainer() : null);
			if (control == null)
			{
				return;
			}
			string scenePath = SceneHelper.GetScenePath(path);
			Node2D node2D = PreloadManager.Cache.GetScene(scenePath).Instantiate<Node2D>(0L);
			control.AddChildSafely(node2D);
			node2D.GlobalPosition = NGame.Instance.GetViewportRect().Size * 0.5f;
		}

		// Token: 0x06008885 RID: 34949 RVA: 0x002DFAB4 File Offset: 0x002DDCB4
		public static Vector2? GetSideCenter(CombatSide side, ICombatState combatState)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (combatState.IsLiveCombat())
			{
				Vector2 vector = Vector2.Zero;
				IReadOnlyList<Creature> readOnlyList = (from c in combatState.GetCreaturesOnSide(side)
					where c.IsHittable
					select c).ToList<Creature>();
				foreach (Creature creature in readOnlyList)
				{
					NCreature creatureNode = creature.GetCreatureNode();
					if (creatureNode != null)
					{
						vector += creatureNode.VfxSpawnPosition;
					}
				}
				vector /= (float)readOnlyList.Count;
				return new Vector2?(vector);
			}
			if (side == CombatSide.Enemy && NBestiary.Instance != null)
			{
				return new Vector2?(NBestiary.Instance.GetSideCenter());
			}
			return new Vector2?(Vector2.Zero);
		}

		// Token: 0x06008886 RID: 34950 RVA: 0x002DFB9C File Offset: 0x002DDD9C
		public static Vector2? GetSideCenterFloor(CombatSide side, ICombatState combatState)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (combatState.IsLiveCombat())
			{
				Vector2 zero = Vector2.Zero;
				IReadOnlyList<Creature> readOnlyList = (from c in combatState.GetCreaturesOnSide(side)
					where c.IsHittable
					select c).ToList<Creature>();
				foreach (Creature creature in readOnlyList)
				{
					NCreature creatureNode = creature.GetCreatureNode();
					if (creatureNode != null)
					{
						if (creatureNode.GetBottomOfHitbox().Y > zero.Y)
						{
							zero.Y = creatureNode.GetBottomOfHitbox().Y;
						}
						zero.X += creatureNode.GetBottomOfHitbox().X;
					}
				}
				zero.X /= (float)readOnlyList.Count;
				return new Vector2?(zero);
			}
			if (side == CombatSide.Enemy && NBestiary.Instance != null)
			{
				return new Vector2?(NBestiary.Instance.GetSideFloor());
			}
			return new Vector2?(Vector2.Zero);
		}

		// Token: 0x06008887 RID: 34951 RVA: 0x002DFCB8 File Offset: 0x002DDEB8
		public static void PlayOnSide(CombatSide side, string path, ICombatState combatState)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			if (!combatState.IsLiveCombat() && side != CombatSide.Enemy)
			{
				return;
			}
			Vector2? sideCenter = VfxCmd.GetSideCenter(side, combatState);
			if (sideCenter == null)
			{
				return;
			}
			Control control;
			if (combatState.IsLiveCombat())
			{
				NCombatRoom instance = NCombatRoom.Instance;
				control = ((instance != null) ? instance.CombatVfxContainer : null);
			}
			else
			{
				NBestiary instance2 = NBestiary.Instance;
				control = ((instance2 != null) ? instance2.VfxContainer : null);
			}
			VfxCmd.PlayVfx(sideCenter.Value, path, control);
		}

		// Token: 0x06008888 RID: 34952 RVA: 0x002DFD28 File Offset: 0x002DDF28
		public static void PlayOnCreatureCenters(IEnumerable<Creature> targets, string path)
		{
			foreach (Creature creature in targets)
			{
				VfxCmd.PlayOnCreatureCenter(creature, path);
			}
		}

		// Token: 0x06008889 RID: 34953 RVA: 0x002DFD70 File Offset: 0x002DDF70
		public static void PlayOnCreatureCenter(Creature target, string path)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			if (target.IsDead)
			{
				return;
			}
			NCreature creatureNode = target.GetCreatureNode();
			if (creatureNode == null)
			{
				return;
			}
			VfxCmd.PlayVfx(creatureNode.VfxSpawnPosition, path, target.GetVfxContainer());
		}

		// Token: 0x0600888A RID: 34954 RVA: 0x002DFDAC File Offset: 0x002DDFAC
		public static void PlayOnCreatures(IEnumerable<Creature> targets, string path)
		{
			foreach (Creature creature in targets)
			{
				VfxCmd.PlayOnCreature(creature, path);
			}
		}

		// Token: 0x0600888B RID: 34955 RVA: 0x002DFDF4 File Offset: 0x002DDFF4
		public static void PlayOnCreature(Creature target, string path)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			if (target.IsDead)
			{
				return;
			}
			NCreature creatureNode = target.GetCreatureNode();
			if (creatureNode == null)
			{
				return;
			}
			VfxCmd.PlayVfx(creatureNode.GlobalPosition, path, target.GetVfxContainer());
		}

		// Token: 0x0600888C RID: 34956 RVA: 0x002DFE30 File Offset: 0x002DE030
		public static void PlayVfx(Vector2 position, string path, [Nullable(2)] Control vfxContainer)
		{
			if (TestMode.IsOn)
			{
				return;
			}
			string scenePath = SceneHelper.GetScenePath(path);
			Node2D node2D = PreloadManager.Cache.GetScene(scenePath).Instantiate<Node2D>(0L);
			if (vfxContainer != null)
			{
				vfxContainer.AddChildSafely(node2D);
			}
			node2D.GlobalPosition = position;
		}

		// Token: 0x0600888D RID: 34957 RVA: 0x002DFE70 File Offset: 0x002DE070
		[return: Nullable(2)]
		public static Node2D PlayNonCombatVfx(Node container, Vector2 position, string path)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			string scenePath = SceneHelper.GetScenePath(path);
			Node2D node2D = PreloadManager.Cache.GetScene(scenePath).Instantiate<Node2D>(0L);
			container.AddChildSafely(node2D);
			node2D.GlobalPosition = position;
			return node2D;
		}

		// Token: 0x04002F62 RID: 12130
		public const string adrenalinePath = "vfx/vfx_adrenaline";

		// Token: 0x04002F63 RID: 12131
		public const string bitePath = "vfx/vfx_bite";

		// Token: 0x04002F64 RID: 12132
		public const string blockPath = "vfx/vfx_block";

		// Token: 0x04002F65 RID: 12133
		public const string bloodyImpactPath = "vfx/vfx_bloody_impact";

		// Token: 0x04002F66 RID: 12134
		public const string bluntPath = "vfx/vfx_attack_blunt";

		// Token: 0x04002F67 RID: 12135
		public const string chainPath = "vfx/vfx_chain";

		// Token: 0x04002F68 RID: 12136
		public const string coinExplosionSmallPath = "vfx/vfx_coin_explosion_small";

		// Token: 0x04002F69 RID: 12137
		public const string coinExplosionRegularPath = "vfx/vfx_coin_explosion_regular";

		// Token: 0x04002F6A RID: 12138
		public const string coinExplosionJumboPath = "vfx/vfx_coin_explosion_jumbo";

		// Token: 0x04002F6B RID: 12139
		public const string daggerThrowPath = "vfx/vfx_dagger_throw";

		// Token: 0x04002F6C RID: 12140
		public const string daggerSprayPath = "vfx/vfx_dagger_spray";

		// Token: 0x04002F6D RID: 12141
		public const string dramaticStabPath = "vfx/vfx_dramatic_stab";

		// Token: 0x04002F6E RID: 12142
		public const string flyingSlashPath = "vfx/vfx_flying_slash";

		// Token: 0x04002F6F RID: 12143
		public const string gazePath = "vfx/vfx_gaze";

		// Token: 0x04002F70 RID: 12144
		public const string giantHorizontalSlashPath = "vfx/vfx_giant_horizontal_slash";

		// Token: 0x04002F71 RID: 12145
		public const string healPath = "vfx/vfx_cross_heal";

		// Token: 0x04002F72 RID: 12146
		public const string lightningPath = "vfx/vfx_attack_lightning";

		// Token: 0x04002F73 RID: 12147
		public const string thrashPath = "vfx/vfx_thrash";

		// Token: 0x04002F74 RID: 12148
		public const string rockShatterPath = "vfx/vfx_rock_shatter";

		// Token: 0x04002F75 RID: 12149
		public const string sandyImpactPath = "vfx/vfx_sandy_impact";

		// Token: 0x04002F76 RID: 12150
		public const string scratchPath = "vfx/vfx_scratch";

		// Token: 0x04002F77 RID: 12151
		public const string slashPath = "vfx/vfx_attack_slash";

		// Token: 0x04002F78 RID: 12152
		public const string slimeImpactVfxPath = "vfx/vfx_slime_impact";

		// Token: 0x04002F79 RID: 12153
		public const string hellraiserSwordVfxPath = "vfx/hellraiser_attack_vfx";

		// Token: 0x04002F7A RID: 12154
		public const string heavyBluntPath = "vfx/vfx_heavy_blunt";

		// Token: 0x04002F7B RID: 12155
		public const string starryImpactVfx = "vfx/vfx_starry_impact";

		// Token: 0x04002F7C RID: 12156
		public const string screamVfx = "vfx/vfx_scream";

		// Token: 0x04002F7D RID: 12157
		public const string spookyScreamVfx = "vfx/vfx_spooky_scream";

		// Token: 0x020024D2 RID: 9426
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04009D04 RID: 40196
			[Nullable(0)]
			public static Func<string, string> <0>__GetScenePath;
		}
	}
}
