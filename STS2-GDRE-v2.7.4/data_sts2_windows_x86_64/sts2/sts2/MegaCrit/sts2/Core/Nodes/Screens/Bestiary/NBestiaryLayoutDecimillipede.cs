using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary
{
	// Token: 0x02000362 RID: 866
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryLayoutDecimillipede.cs")]
	public class NBestiaryLayoutDecimillipede : NBestiaryLayout
	{
		// Token: 0x0600325A RID: 12890 RVA: 0x0017789C File Offset: 0x00175A9C
		[return: Nullable(2)]
		public static NBestiaryLayoutDecimillipede Create(NBestiary bestiary)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NBestiaryLayoutDecimillipede nbestiaryLayoutDecimillipede = PreloadManager.Cache.GetScene(NBestiaryLayoutDecimillipede._scenePath).Instantiate<NBestiaryLayoutDecimillipede>(0L);
			nbestiaryLayoutDecimillipede._bestiary = bestiary;
			return nbestiaryLayoutDecimillipede;
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x001778D1 File Offset: 0x00175AD1
		public override void _Ready()
		{
			this._creatureContainer = base.GetNode<Control>("%CreatureContainer");
			this._encounterSlots = base.GetNode<Control>("%EncounterSlots");
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x001778FF File Offset: 0x00175AFF
		public override void Cleanup()
		{
			Control encounterVisuals = this._encounterVisuals;
			if (encounterVisuals != null)
			{
				encounterVisuals.QueueFreeSafely();
			}
			this._encounterVisuals = null;
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x0017791C File Offset: 0x00175B1C
		public unsafe override List<BestiaryMonsterMove> Setup(BestiaryEntry entry, Tween tween)
		{
			EncounterModel encounterModel = entry.encounterModel.ToMutable();
			Control encounterVisuals = this._encounterVisuals;
			if (encounterVisuals != null)
			{
				encounterVisuals.QueueFreeSafely();
			}
			encounterModel.GenerateMonstersWithSlots(NullRunState.Instance);
			foreach (ValueTuple<MonsterModel, string> valueTuple in encounterModel.MonstersWithSlots)
			{
				MonsterModel item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				item.Rng = Rng.Chaotic;
				item.SetUpForCombat();
				Creature creature = new Creature(item, CombatSide.Enemy, item2)
				{
					CombatState = NullCombatState.Instance
				};
				NCreature ncreature = NCreature.Create(creature);
				this._creatureContainer.AddChildSafely(ncreature);
				this._creatures.Add(ncreature);
				ncreature.SetupForBestiary();
				ncreature.GlobalPosition = this._encounterSlots.GetNode<Marker2D>(creature.SlotName).GlobalPosition;
			}
			int num = 3;
			List<BestiaryMonsterMove> list = new List<BestiaryMonsterMove>(num);
			CollectionsMarshal.SetCount<BestiaryMonsterMove>(list, num);
			Span<BestiaryMonsterMove> span = CollectionsMarshal.AsSpan<BestiaryMonsterMove>(list);
			int num2 = 0;
			*span[num2] = BestiaryMonsterMove.FromAction(this.GetBestiaryMoveName("WRITHE"), new Func<Task>(this.AnimAttack));
			num2++;
			*span[num2] = BestiaryMonsterMove.FromAction(this.GetBestiaryMoveName("REATTACH"), new Func<Task>(this.AnimReattach));
			num2++;
			*span[num2] = BestiaryMonsterMove.FromAction(this.GetBestiaryMoveName("DEAD"), new Func<Task>(this.AnimDie));
			return list;
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x00177AB8 File Offset: 0x00175CB8
		private LocString GetBestiaryMoveName(string moveId)
		{
			return new LocString("monsters", ModelDb.GetId<DecimillipedeSegment>().Entry + ".moves." + moveId + ".title");
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x00177AE0 File Offset: 0x00175CE0
		private async Task AnimAttack()
		{
			foreach (NCreature ncreature in this._creatures)
			{
				((DecimillipedeSegment)ncreature.Entity.Monster).SegmentAttack();
			}
			Node2D node2D = PreloadManager.Cache.GetScene(DecimillipedeSegment.rocksVfxPath).Instantiate<Node2D>(0L);
			this._bestiary.VfxContainer.AddChildSafely(node2D);
			node2D.GlobalPosition = NGame.Instance.GetViewportRect().Size * 0.5f;
			await Cmd.Wait(0.5f, false);
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x00177B24 File Offset: 0x00175D24
		private async Task AnimReattach()
		{
			foreach (NCreature ncreature in this._creatures)
			{
				NDecimillipedeSegmentVfx specialNode = ncreature.GetSpecialNode<NDecimillipedeSegmentVfx>("%NDecimillipedeSegmentVfx");
				if (specialNode != null)
				{
					specialNode.Regenerate();
				}
				await CreatureCmd.TriggerAnim(ncreature.Entity, "Revive", 0.15f);
			}
			List<NCreature>.Enumerator enumerator = default(List<NCreature>.Enumerator);
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x00177B68 File Offset: 0x00175D68
		private async Task AnimDie()
		{
			foreach (NCreature ncreature in this._creatures)
			{
				await CreatureCmd.TriggerAnim(ncreature.Entity, "Dead", 0.15f);
			}
			List<NCreature>.Enumerator enumerator = default(List<NCreature>.Enumerator);
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x00177BAB File Offset: 0x00175DAB
		public override IEnumerable<NCreature> GetCreatures()
		{
			return this._creatures;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003263 RID: 12899 RVA: 0x00177BB4 File Offset: 0x00175DB4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NBestiaryLayoutDecimillipede.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "bestiary", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NBestiaryLayoutDecimillipede.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryLayoutDecimillipede.MethodName.Cleanup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003264 RID: 12900 RVA: 0x00177C94 File Offset: 0x00175E94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryLayoutDecimillipede.MethodName.Create && args.Count == 1)
			{
				NBestiaryLayoutDecimillipede nbestiaryLayoutDecimillipede = NBestiaryLayoutDecimillipede.Create(VariantUtils.ConvertTo<NBestiary>(args[0]));
				ret = VariantUtils.CreateFrom<NBestiaryLayoutDecimillipede>(ref nbestiaryLayoutDecimillipede);
				return true;
			}
			if ((ref method) == NBestiaryLayoutDecimillipede.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryLayoutDecimillipede.MethodName.Cleanup && args.Count == 0)
			{
				this.Cleanup();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x00177D30 File Offset: 0x00175F30
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryLayoutDecimillipede.MethodName.Create && args.Count == 1)
			{
				NBestiaryLayoutDecimillipede nbestiaryLayoutDecimillipede = NBestiaryLayoutDecimillipede.Create(VariantUtils.ConvertTo<NBestiary>(args[0]));
				ret = VariantUtils.CreateFrom<NBestiaryLayoutDecimillipede>(ref nbestiaryLayoutDecimillipede);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003266 RID: 12902 RVA: 0x00177D7E File Offset: 0x00175F7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBestiaryLayoutDecimillipede.MethodName.Create || (ref method) == NBestiaryLayoutDecimillipede.MethodName._Ready || (ref method) == NBestiaryLayoutDecimillipede.MethodName.Cleanup || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003267 RID: 12903 RVA: 0x00177DB4 File Offset: 0x00175FB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBestiaryLayoutDecimillipede.PropertyName._encounterVisuals)
			{
				this._encounterVisuals = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryLayoutDecimillipede.PropertyName._creatureContainer)
			{
				this._creatureContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryLayoutDecimillipede.PropertyName._encounterSlots)
			{
				this._encounterSlots = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryLayoutDecimillipede.PropertyName._bestiary)
			{
				this._bestiary = VariantUtils.ConvertTo<NBestiary>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003268 RID: 12904 RVA: 0x00177E38 File Offset: 0x00176038
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBestiaryLayoutDecimillipede.PropertyName._encounterVisuals)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._encounterVisuals);
				return true;
			}
			if ((ref name) == NBestiaryLayoutDecimillipede.PropertyName._creatureContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._creatureContainer);
				return true;
			}
			if ((ref name) == NBestiaryLayoutDecimillipede.PropertyName._encounterSlots)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._encounterSlots);
				return true;
			}
			if ((ref name) == NBestiaryLayoutDecimillipede.PropertyName._bestiary)
			{
				value = VariantUtils.CreateFrom<NBestiary>(ref this._bestiary);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003269 RID: 12905 RVA: 0x00177ED0 File Offset: 0x001760D0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NBestiaryLayoutDecimillipede.PropertyName._encounterVisuals, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryLayoutDecimillipede.PropertyName._creatureContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryLayoutDecimillipede.PropertyName._encounterSlots, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryLayoutDecimillipede.PropertyName._bestiary, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600326A RID: 12906 RVA: 0x00177F68 File Offset: 0x00176168
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBestiaryLayoutDecimillipede.PropertyName._encounterVisuals, Variant.From<Control>(ref this._encounterVisuals));
			info.AddProperty(NBestiaryLayoutDecimillipede.PropertyName._creatureContainer, Variant.From<Control>(ref this._creatureContainer));
			info.AddProperty(NBestiaryLayoutDecimillipede.PropertyName._encounterSlots, Variant.From<Control>(ref this._encounterSlots));
			info.AddProperty(NBestiaryLayoutDecimillipede.PropertyName._bestiary, Variant.From<NBestiary>(ref this._bestiary));
		}

		/// <inheritdoc />
		// Token: 0x0600326B RID: 12907 RVA: 0x00177FD4 File Offset: 0x001761D4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBestiaryLayoutDecimillipede.PropertyName._encounterVisuals, ref variant))
			{
				this._encounterVisuals = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBestiaryLayoutDecimillipede.PropertyName._creatureContainer, ref variant2))
			{
				this._creatureContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBestiaryLayoutDecimillipede.PropertyName._encounterSlots, ref variant3))
			{
				this._encounterSlots = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBestiaryLayoutDecimillipede.PropertyName._bestiary, ref variant4))
			{
				this._bestiary = variant4.As<NBestiary>();
			}
		}

		// Token: 0x040013A4 RID: 5028
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary_layout_decimillipede");

		// Token: 0x040013A5 RID: 5029
		[Nullable(2)]
		private Control _encounterVisuals;

		// Token: 0x040013A6 RID: 5030
		private Control _creatureContainer;

		// Token: 0x040013A7 RID: 5031
		private Control _encounterSlots;

		// Token: 0x040013A8 RID: 5032
		private List<NCreature> _creatures = new List<NCreature>();

		// Token: 0x040013A9 RID: 5033
		private NBestiary _bestiary;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001665 RID: 5733
		[NullableContext(0)]
		public new class MethodName : NBestiaryLayout.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400575A RID: 22362
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400575B RID: 22363
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Cleanup' method.
			/// </summary>
			// Token: 0x0400575C RID: 22364
			public new static readonly StringName Cleanup = "Cleanup";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001666 RID: 5734
		[NullableContext(0)]
		public new class PropertyName : NBestiaryLayout.PropertyName
		{
			/// <summary>
			/// Cached name for the '_encounterVisuals' field.
			/// </summary>
			// Token: 0x0400575D RID: 22365
			public static readonly StringName _encounterVisuals = "_encounterVisuals";

			/// <summary>
			/// Cached name for the '_creatureContainer' field.
			/// </summary>
			// Token: 0x0400575E RID: 22366
			public static readonly StringName _creatureContainer = "_creatureContainer";

			/// <summary>
			/// Cached name for the '_encounterSlots' field.
			/// </summary>
			// Token: 0x0400575F RID: 22367
			public static readonly StringName _encounterSlots = "_encounterSlots";

			/// <summary>
			/// Cached name for the '_bestiary' field.
			/// </summary>
			// Token: 0x04005760 RID: 22368
			public static readonly StringName _bestiary = "_bestiary";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001667 RID: 5735
		[NullableContext(0)]
		public new class SignalName : NBestiaryLayout.SignalName
		{
		}
	}
}
