using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary
{
	// Token: 0x02000363 RID: 867
	[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryLayoutDefault.cs")]
	public class NBestiaryLayoutDefault : NBestiaryLayout
	{
		// Token: 0x0600326E RID: 12910 RVA: 0x0017807C File Offset: 0x0017627C
		[NullableContext(2)]
		public static NBestiaryLayoutDefault Create()
		{
			return PreloadManager.Cache.GetScene(NBestiaryLayoutDefault._scenePath).Instantiate<NBestiaryLayoutDefault>(0L);
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x00178094 File Offset: 0x00176294
		public override void _Ready()
		{
			this._creatureContainer = base.GetNode<Control>("%MonsterVisualsContainer");
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x001780AC File Offset: 0x001762AC
		public override void Cleanup()
		{
			NCreature creature = this._creature;
			if (creature != null)
			{
				creature.QueueFreeSafely();
			}
			this._creature = null;
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x001780C8 File Offset: 0x001762C8
		[NullableContext(1)]
		public override List<BestiaryMonsterMove> Setup(BestiaryEntry entry, Tween tween)
		{
			MonsterModel monsterModel = entry.monsterModel.ToMutable();
			monsterModel.Rng = Rng.Chaotic;
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
			this._creatureContainer.AddChildSafely(this._creature);
			this._creature.SetupForBestiary();
			this._creature.Position = new Vector2(0f, this._creature.Hitbox.Size.Y * 0.5f);
			this._creature.Modulate = StsColors.transparentBlack;
			tween.TweenProperty(this._creature, "modulate", Colors.White, 0.25);
			return monsterModel.GenerateBestiaryMoveList(this._creature.Visuals);
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x001781BC File Offset: 0x001763BC
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
		// Token: 0x06003273 RID: 12915 RVA: 0x001781E8 File Offset: 0x001763E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NBestiaryLayoutDefault.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NBestiaryLayoutDefault.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryLayoutDefault.MethodName.Cleanup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003274 RID: 12916 RVA: 0x00178298 File Offset: 0x00176498
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryLayoutDefault.MethodName.Create && args.Count == 0)
			{
				NBestiaryLayoutDefault nbestiaryLayoutDefault = NBestiaryLayoutDefault.Create();
				ret = VariantUtils.CreateFrom<NBestiaryLayoutDefault>(ref nbestiaryLayoutDefault);
				return true;
			}
			if ((ref method) == NBestiaryLayoutDefault.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryLayoutDefault.MethodName.Cleanup && args.Count == 0)
			{
				this.Cleanup();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x00178324 File Offset: 0x00176524
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryLayoutDefault.MethodName.Create && args.Count == 0)
			{
				NBestiaryLayoutDefault nbestiaryLayoutDefault = NBestiaryLayoutDefault.Create();
				ret = VariantUtils.CreateFrom<NBestiaryLayoutDefault>(ref nbestiaryLayoutDefault);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003276 RID: 12918 RVA: 0x00178364 File Offset: 0x00176564
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBestiaryLayoutDefault.MethodName.Create || (ref method) == NBestiaryLayoutDefault.MethodName._Ready || (ref method) == NBestiaryLayoutDefault.MethodName.Cleanup || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003277 RID: 12919 RVA: 0x0017839A File Offset: 0x0017659A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBestiaryLayoutDefault.PropertyName._creature)
			{
				this._creature = VariantUtils.ConvertTo<NCreature>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryLayoutDefault.PropertyName._creatureContainer)
			{
				this._creatureContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003278 RID: 12920 RVA: 0x001783DC File Offset: 0x001765DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBestiaryLayoutDefault.PropertyName._creature)
			{
				value = VariantUtils.CreateFrom<NCreature>(ref this._creature);
				return true;
			}
			if ((ref name) == NBestiaryLayoutDefault.PropertyName._creatureContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._creatureContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003279 RID: 12921 RVA: 0x00178434 File Offset: 0x00176634
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NBestiaryLayoutDefault.PropertyName._creature, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryLayoutDefault.PropertyName._creatureContainer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600327A RID: 12922 RVA: 0x0017848A File Offset: 0x0017668A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBestiaryLayoutDefault.PropertyName._creature, Variant.From<NCreature>(ref this._creature));
			info.AddProperty(NBestiaryLayoutDefault.PropertyName._creatureContainer, Variant.From<Control>(ref this._creatureContainer));
		}

		/// <inheritdoc />
		// Token: 0x0600327B RID: 12923 RVA: 0x001784C0 File Offset: 0x001766C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBestiaryLayoutDefault.PropertyName._creature, ref variant))
			{
				this._creature = variant.As<NCreature>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBestiaryLayoutDefault.PropertyName._creatureContainer, ref variant2))
			{
				this._creatureContainer = variant2.As<Control>();
			}
		}

		// Token: 0x040013AA RID: 5034
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary_layout_default");

		// Token: 0x040013AB RID: 5035
		[Nullable(2)]
		private NCreature _creature;

		// Token: 0x040013AC RID: 5036
		[Nullable(1)]
		private Control _creatureContainer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200166B RID: 5739
		public new class MethodName : NBestiaryLayout.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400576F RID: 22383
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005770 RID: 22384
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Cleanup' method.
			/// </summary>
			// Token: 0x04005771 RID: 22385
			public new static readonly StringName Cleanup = "Cleanup";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200166C RID: 5740
		public new class PropertyName : NBestiaryLayout.PropertyName
		{
			/// <summary>
			/// Cached name for the '_creature' field.
			/// </summary>
			// Token: 0x04005772 RID: 22386
			public static readonly StringName _creature = "_creature";

			/// <summary>
			/// Cached name for the '_creatureContainer' field.
			/// </summary>
			// Token: 0x04005773 RID: 22387
			public static readonly StringName _creatureContainer = "_creatureContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200166D RID: 5741
		public new class SignalName : NBestiaryLayout.SignalName
		{
		}
	}
}
