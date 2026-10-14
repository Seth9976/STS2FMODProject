using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary
{
	// Token: 0x02000364 RID: 868
	[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryLayoutKaiserCrab.cs")]
	public class NBestiaryLayoutKaiserCrab : NBestiaryLayout
	{
		// Token: 0x0600327E RID: 12926 RVA: 0x00178525 File Offset: 0x00176725
		[NullableContext(2)]
		public static NBestiaryLayoutKaiserCrab Create()
		{
			return PreloadManager.Cache.GetScene(NBestiaryLayoutKaiserCrab._scenePath).Instantiate<NBestiaryLayoutKaiserCrab>(0L);
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x0017853D File Offset: 0x0017673D
		public override void _Ready()
		{
			this._background = base.GetNode<NKaiserCrabBossBackground>("%KaiserCrab");
			this._backgroundPosition = this._background.Position;
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x00178566 File Offset: 0x00176766
		public override void Cleanup()
		{
			NCreature creature = this._creature;
			if (creature != null)
			{
				creature.QueueFreeSafely();
			}
			this._creature = null;
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x00178580 File Offset: 0x00176780
		[NullableContext(1)]
		public override List<BestiaryMonsterMove> Setup(BestiaryEntry entry, Tween tween)
		{
			MonsterModel monsterModel = entry.monsterModel.ToMutable();
			monsterModel.Rng = Rng.Chaotic;
			if (monsterModel is Crusher)
			{
				this._background.Position = this._backgroundPosition + 1020f * Vector2.Right * 0.5f / this._background.Scale;
			}
			else if (monsterModel is Rocket)
			{
				this._background.Position = this._backgroundPosition + 1240f * Vector2.Left * 0.5f / this._background.Scale;
			}
			this._background.SetVisible(true);
			NCreature creature = this._creature;
			if (creature != null)
			{
				creature.QueueFreeSafely();
			}
			monsterModel.SetUpForCombat();
			Creature creature2 = new Creature(monsterModel, CombatSide.Enemy, null)
			{
				CombatState = NullCombatState.Instance
			};
			this._creature = NCreature.Create(creature2);
			this.AddChildSafely(this._creature);
			this._creature.SetupForBestiary();
			this._creature.Position = new Vector2(0f, this._creature.Hitbox.Size.Y * 0.5f);
			this._creature.Modulate = StsColors.transparentBlack;
			tween.TweenProperty(this._creature, "modulate", Colors.White, 0.25);
			List<BestiaryMonsterMove> list = monsterModel.GenerateBestiaryMoveList(this._creature.Visuals);
			list.Add(BestiaryMonsterMove.FromAction(new LocString("bestiary", "ACTION_NAME.hurt"), new Func<Task>(this.AnimHurt)));
			list.Add(BestiaryMonsterMove.FromAction(new LocString("bestiary", "ACTION_NAME.die"), new Func<Task>(this.AnimDie)));
			return list;
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x00178757 File Offset: 0x00176957
		[NullableContext(1)]
		public Task AnimHurt()
		{
			this._background.PlayHurtAnim((this._creature.Entity.Monster is Crusher) ? NKaiserCrabBossBackground.ArmSide.Left : NKaiserCrabBossBackground.ArmSide.Right);
			return Task.CompletedTask;
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x00178784 File Offset: 0x00176984
		[NullableContext(1)]
		public Task AnimDie()
		{
			NAudioManager.Instance.PlayOneShot(this._creature.Entity.Monster.DeathSfx, 1f);
			this._background.PlayArmDeathAnim((this._creature.Entity.Monster is Crusher) ? NKaiserCrabBossBackground.ArmSide.Left : NKaiserCrabBossBackground.ArmSide.Right);
			return Task.CompletedTask;
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x001787E0 File Offset: 0x001769E0
		[NullableContext(1)]
		public override IEnumerable<NCreature> GetCreatures()
		{
			if (this._creature == null)
			{
				return Array.Empty<NCreature>();
			}
			return new <>z__ReadOnlySingleElementList<NCreature>(this._creature);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003285 RID: 12933 RVA: 0x0017880C File Offset: 0x00176A0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NBestiaryLayoutKaiserCrab.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NBestiaryLayoutKaiserCrab.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryLayoutKaiserCrab.MethodName.Cleanup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003286 RID: 12934 RVA: 0x001788BC File Offset: 0x00176ABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryLayoutKaiserCrab.MethodName.Create && args.Count == 0)
			{
				NBestiaryLayoutKaiserCrab nbestiaryLayoutKaiserCrab = NBestiaryLayoutKaiserCrab.Create();
				ret = VariantUtils.CreateFrom<NBestiaryLayoutKaiserCrab>(ref nbestiaryLayoutKaiserCrab);
				return true;
			}
			if ((ref method) == NBestiaryLayoutKaiserCrab.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryLayoutKaiserCrab.MethodName.Cleanup && args.Count == 0)
			{
				this.Cleanup();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x00178948 File Offset: 0x00176B48
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryLayoutKaiserCrab.MethodName.Create && args.Count == 0)
			{
				NBestiaryLayoutKaiserCrab nbestiaryLayoutKaiserCrab = NBestiaryLayoutKaiserCrab.Create();
				ret = VariantUtils.CreateFrom<NBestiaryLayoutKaiserCrab>(ref nbestiaryLayoutKaiserCrab);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003288 RID: 12936 RVA: 0x00178988 File Offset: 0x00176B88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBestiaryLayoutKaiserCrab.MethodName.Create || (ref method) == NBestiaryLayoutKaiserCrab.MethodName._Ready || (ref method) == NBestiaryLayoutKaiserCrab.MethodName.Cleanup || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003289 RID: 12937 RVA: 0x001789C0 File Offset: 0x00176BC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBestiaryLayoutKaiserCrab.PropertyName._creature)
			{
				this._creature = VariantUtils.ConvertTo<NCreature>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryLayoutKaiserCrab.PropertyName._background)
			{
				this._background = VariantUtils.ConvertTo<NKaiserCrabBossBackground>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryLayoutKaiserCrab.PropertyName._backgroundPosition)
			{
				this._backgroundPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600328A RID: 12938 RVA: 0x00178A28 File Offset: 0x00176C28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBestiaryLayoutKaiserCrab.PropertyName._creature)
			{
				value = VariantUtils.CreateFrom<NCreature>(ref this._creature);
				return true;
			}
			if ((ref name) == NBestiaryLayoutKaiserCrab.PropertyName._background)
			{
				value = VariantUtils.CreateFrom<NKaiserCrabBossBackground>(ref this._background);
				return true;
			}
			if ((ref name) == NBestiaryLayoutKaiserCrab.PropertyName._backgroundPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._backgroundPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600328B RID: 12939 RVA: 0x00178AA0 File Offset: 0x00176CA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NBestiaryLayoutKaiserCrab.PropertyName._creature, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryLayoutKaiserCrab.PropertyName._background, 0L, "", 4096L, false),
				new PropertyInfo(5L, NBestiaryLayoutKaiserCrab.PropertyName._backgroundPosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600328C RID: 12940 RVA: 0x00178B18 File Offset: 0x00176D18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBestiaryLayoutKaiserCrab.PropertyName._creature, Variant.From<NCreature>(ref this._creature));
			info.AddProperty(NBestiaryLayoutKaiserCrab.PropertyName._background, Variant.From<NKaiserCrabBossBackground>(ref this._background));
			info.AddProperty(NBestiaryLayoutKaiserCrab.PropertyName._backgroundPosition, Variant.From<Vector2>(ref this._backgroundPosition));
		}

		/// <inheritdoc />
		// Token: 0x0600328D RID: 12941 RVA: 0x00178B70 File Offset: 0x00176D70
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBestiaryLayoutKaiserCrab.PropertyName._creature, ref variant))
			{
				this._creature = variant.As<NCreature>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBestiaryLayoutKaiserCrab.PropertyName._background, ref variant2))
			{
				this._background = variant2.As<NKaiserCrabBossBackground>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBestiaryLayoutKaiserCrab.PropertyName._backgroundPosition, ref variant3))
			{
				this._backgroundPosition = variant3.As<Vector2>();
			}
		}

		// Token: 0x040013AD RID: 5037
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary_layout_kaiser_crab");

		// Token: 0x040013AE RID: 5038
		[Nullable(2)]
		private NCreature _creature;

		// Token: 0x040013AF RID: 5039
		[Nullable(1)]
		private NKaiserCrabBossBackground _background;

		// Token: 0x040013B0 RID: 5040
		private Vector2 _backgroundPosition;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200166E RID: 5742
		public new class MethodName : NBestiaryLayout.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005774 RID: 22388
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005775 RID: 22389
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Cleanup' method.
			/// </summary>
			// Token: 0x04005776 RID: 22390
			public new static readonly StringName Cleanup = "Cleanup";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200166F RID: 5743
		public new class PropertyName : NBestiaryLayout.PropertyName
		{
			/// <summary>
			/// Cached name for the '_creature' field.
			/// </summary>
			// Token: 0x04005777 RID: 22391
			public static readonly StringName _creature = "_creature";

			/// <summary>
			/// Cached name for the '_background' field.
			/// </summary>
			// Token: 0x04005778 RID: 22392
			public static readonly StringName _background = "_background";

			/// <summary>
			/// Cached name for the '_backgroundPosition' field.
			/// </summary>
			// Token: 0x04005779 RID: 22393
			public static readonly StringName _backgroundPosition = "_backgroundPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001670 RID: 5744
		public new class SignalName : NBestiaryLayout.SignalName
		{
		}
	}
}
