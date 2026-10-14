using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000200 RID: 512
	[ScriptPath("res://src/Core/Nodes/Vfx/NStunnedVfx.cs")]
	public class NStunnedVfx : Node2D
	{
		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001A5F RID: 6751 RVA: 0x000A91AC File Offset: 0x000A73AC
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/vfx/stunned_vfx.tscn");
			}
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x000A91B8 File Offset: 0x000A73B8
		public override void _Ready()
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(this._creature) : null);
			if (ncreature == null)
			{
				this.QueueFreeSafely();
				return;
			}
			this._label = base.GetNode<MegaLabel>("%Label");
			base.GlobalPosition = ncreature.GetTopOfHitbox();
			TaskHelper.RunSafely(this.StartVfx());
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x000A9215 File Offset: 0x000A7415
		public override void _ExitTree()
		{
			Tween textTween = this._textTween;
			if (textTween != null)
			{
				textTween.Kill();
			}
			Tween positionTween = this._positionTween;
			if (positionTween == null)
			{
				return;
			}
			positionTween.Kill();
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x000A9238 File Offset: 0x000A7438
		[NullableContext(1)]
		private async Task StartVfx()
		{
			this._label.SetTextAutoSize(NStunnedVfx._stunnedLoc.GetFormattedText());
			this._textTween = base.CreateTween();
			this._textTween.TweenProperty(this._label, "modulate:a", 1f, 0.25).SetEase(1L).SetTrans(5L);
			this._textTween.TweenInterval(0.5);
			this._textTween.TweenProperty(this._label, "modulate:a", 0f, 1.0).SetEase(1L).SetTrans(0L);
			this._positionTween = base.CreateTween();
			this._positionTween.TweenProperty(this._label, "position:y", this._label.Position.Y - 100f, 2.0).SetEase(1L).SetTrans(3L);
			bool flag = await this._textTween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x000A927C File Offset: 0x000A747C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NStunnedVfx Create(Creature creature)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (NCombatUi.IsDebugHideTextVfx)
			{
				return null;
			}
			NStunnedVfx nstunnedVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/stunned_vfx.tscn").Instantiate<NStunnedVfx>(0L);
			nstunnedVfx._creature = creature;
			return nstunnedVfx;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A64 RID: 6756 RVA: 0x000A92BC File Offset: 0x000A74BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NStunnedVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStunnedVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A65 RID: 6757 RVA: 0x000A9330 File Offset: 0x000A7530
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NStunnedVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStunnedVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001A66 RID: 6758 RVA: 0x000A9390 File Offset: 0x000A7590
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NStunnedVfx.MethodName._Ready || (ref method) == NStunnedVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001A67 RID: 6759 RVA: 0x000A93B8 File Offset: 0x000A75B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NStunnedVfx.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NStunnedVfx.PropertyName._textTween)
			{
				this._textTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NStunnedVfx.PropertyName._positionTween)
			{
				this._positionTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001A68 RID: 6760 RVA: 0x000A9420 File Offset: 0x000A7620
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NStunnedVfx.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NStunnedVfx.PropertyName._textTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._textTween);
				return true;
			}
			if ((ref name) == NStunnedVfx.PropertyName._positionTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._positionTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A69 RID: 6761 RVA: 0x000A9498 File Offset: 0x000A7698
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NStunnedVfx.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStunnedVfx.PropertyName._textTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStunnedVfx.PropertyName._positionTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A6A RID: 6762 RVA: 0x000A9510 File Offset: 0x000A7710
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NStunnedVfx.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NStunnedVfx.PropertyName._textTween, Variant.From<Tween>(ref this._textTween));
			info.AddProperty(NStunnedVfx.PropertyName._positionTween, Variant.From<Tween>(ref this._positionTween));
		}

		/// <inheritdoc />
		// Token: 0x06001A6B RID: 6763 RVA: 0x000A9568 File Offset: 0x000A7768
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NStunnedVfx.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NStunnedVfx.PropertyName._textTween, ref variant2))
			{
				this._textTween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NStunnedVfx.PropertyName._positionTween, ref variant3))
			{
				this._positionTween = variant3.As<Tween>();
			}
		}

		// Token: 0x040009C7 RID: 2503
		[Nullable(1)]
		private const string _scenePath = "res://scenes/vfx/stunned_vfx.tscn";

		// Token: 0x040009C8 RID: 2504
		[Nullable(1)]
		private static LocString _stunnedLoc = new LocString("vfx", "STUNNED");

		// Token: 0x040009C9 RID: 2505
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x040009CA RID: 2506
		[Nullable(1)]
		private Creature _creature;

		// Token: 0x040009CB RID: 2507
		[Nullable(2)]
		private Tween _textTween;

		// Token: 0x040009CC RID: 2508
		[Nullable(2)]
		private Tween _positionTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200116E RID: 4462
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040043FF RID: 17407
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004400 RID: 17408
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200116F RID: 4463
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004401 RID: 17409
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_textTween' field.
			/// </summary>
			// Token: 0x04004402 RID: 17410
			public static readonly StringName _textTween = "_textTween";

			/// <summary>
			/// Cached name for the '_positionTween' field.
			/// </summary>
			// Token: 0x04004403 RID: 17411
			public static readonly StringName _positionTween = "_positionTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001170 RID: 4464
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
