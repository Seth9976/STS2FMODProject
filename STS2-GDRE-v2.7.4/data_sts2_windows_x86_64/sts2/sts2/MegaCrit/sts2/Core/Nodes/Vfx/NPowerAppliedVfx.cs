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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001DE RID: 478
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NPowerAppliedVfx.cs")]
	public class NPowerAppliedVfx : Control
	{
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001830 RID: 6192 RVA: 0x00097414 File Offset: 0x00095614
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/vfx/power_applied_vfx.tscn");
			}
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x00097420 File Offset: 0x00095620
		public override void _Ready()
		{
			this._icon = base.GetNode<TextureRect>("Icon");
			this._iconEcho = base.GetNode<TextureRect>("Icon/IconEcho");
			this._powerField = base.GetNode<MegaLabel>("Label");
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(this._power.Owner) : null);
			if (ncreature == null)
			{
				this.QueueFreeSafely();
				return;
			}
			base.GlobalPosition = ncreature.VfxSpawnPosition;
			TaskHelper.RunSafely(this.StartVfx());
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x000974B0 File Offset: 0x000956B0
		[return: Nullable(2)]
		public static NPowerAppliedVfx Create(PowerModel power, int amount, bool isBuff)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (NCombatUi.IsDebugHideTextVfx)
			{
				return null;
			}
			if (!power.ShouldPlayVfx)
			{
				return null;
			}
			NPowerAppliedVfx npowerAppliedVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/power_applied_vfx.tscn").Instantiate<NPowerAppliedVfx>(0L);
			npowerAppliedVfx._power = power;
			npowerAppliedVfx._amount = amount;
			npowerAppliedVfx._isBuff = isBuff;
			return npowerAppliedVfx;
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x00097506 File Offset: 0x00095706
		public override void _ExitTree()
		{
			Tween spriteTween = this._spriteTween;
			if (spriteTween != null)
			{
				spriteTween.Kill();
			}
			Tween textTween = this._textTween;
			if (textTween == null)
			{
				return;
			}
			textTween.Kill();
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0009752C File Offset: 0x0009572C
		private async Task StartVfx()
		{
			this._powerField.SetTextAutoSize(this._power.Title.GetFormattedText());
			this._icon.Texture = this._power.BigIcon;
			this._iconEcho.Texture = this._power.BigIcon;
			this._powerField.Modulate = (this._isBuff ? StsColors.green : StsColors.red);
			this._powerField.Position = new Vector2(this._powerField.Position.X, this._powerField.Position.Y + NCreature.PowerAppliedVfxPositionOffset.Y);
			this._spriteTween = base.CreateTween().SetParallel(true);
			this._spriteTween.TweenProperty(this._icon, "scale", Vector2.One * 0.8f, 1.25).SetEase(1L).SetTrans(5L)
				.From(Vector2.One * 0.4f);
			this._spriteTween.TweenProperty(this._icon, "modulate:a", 0.5f, 0.25).SetEase(1L).SetTrans(1L);
			this._spriteTween.TweenProperty(this._icon, "modulate:a", 0f, 1.0).SetEase(0L).SetTrans(1L)
				.SetDelay(0.25);
			float num = this._powerField.Position.Y + (this._isBuff ? (-100f) : 100f);
			this._textTween = base.CreateTween().SetParallel(true);
			this._textTween.TweenProperty(this._powerField, "position:y", num, 1.25).SetEase(1L).SetTrans(7L);
			this._textTween.TweenProperty(this._powerField, "modulate:a", 1f, 0.25).SetEase(1L).SetTrans(5L);
			this._textTween.TweenProperty(this._powerField, "modulate:a", 0f, 0.75).SetEase(0L).SetTrans(5L)
				.From(1f)
				.SetDelay(0.25);
			bool flag = await this._spriteTween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001835 RID: 6197 RVA: 0x00097570 File Offset: 0x00095770
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NPowerAppliedVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPowerAppliedVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001836 RID: 6198 RVA: 0x000975E4 File Offset: 0x000957E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPowerAppliedVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPowerAppliedVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001837 RID: 6199 RVA: 0x00097644 File Offset: 0x00095844
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPowerAppliedVfx.MethodName._Ready || (ref method) == NPowerAppliedVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001838 RID: 6200 RVA: 0x0009766C File Offset: 0x0009586C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPowerAppliedVfx.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._iconEcho)
			{
				this._iconEcho = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._powerField)
			{
				this._powerField = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._amount)
			{
				this._amount = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._isBuff)
			{
				this._isBuff = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._textTween)
			{
				this._textTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._spriteTween)
			{
				this._spriteTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001839 RID: 6201 RVA: 0x00097740 File Offset: 0x00095940
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPowerAppliedVfx.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._iconEcho)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._iconEcho);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._powerField)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._powerField);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._amount)
			{
				value = VariantUtils.CreateFrom<int>(ref this._amount);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._isBuff)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isBuff);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._textTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._textTween);
				return true;
			}
			if ((ref name) == NPowerAppliedVfx.PropertyName._spriteTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._spriteTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600183A RID: 6202 RVA: 0x00097838 File Offset: 0x00095A38
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPowerAppliedVfx.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerAppliedVfx.PropertyName._iconEcho, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerAppliedVfx.PropertyName._powerField, 0L, "", 4096L, false),
				new PropertyInfo(2L, NPowerAppliedVfx.PropertyName._amount, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPowerAppliedVfx.PropertyName._isBuff, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerAppliedVfx.PropertyName._textTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerAppliedVfx.PropertyName._spriteTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600183B RID: 6203 RVA: 0x00097934 File Offset: 0x00095B34
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPowerAppliedVfx.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NPowerAppliedVfx.PropertyName._iconEcho, Variant.From<TextureRect>(ref this._iconEcho));
			info.AddProperty(NPowerAppliedVfx.PropertyName._powerField, Variant.From<MegaLabel>(ref this._powerField));
			info.AddProperty(NPowerAppliedVfx.PropertyName._amount, Variant.From<int>(ref this._amount));
			info.AddProperty(NPowerAppliedVfx.PropertyName._isBuff, Variant.From<bool>(ref this._isBuff));
			info.AddProperty(NPowerAppliedVfx.PropertyName._textTween, Variant.From<Tween>(ref this._textTween));
			info.AddProperty(NPowerAppliedVfx.PropertyName._spriteTween, Variant.From<Tween>(ref this._spriteTween));
		}

		/// <inheritdoc />
		// Token: 0x0600183C RID: 6204 RVA: 0x000979E4 File Offset: 0x00095BE4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._iconEcho, ref variant2))
			{
				this._iconEcho = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._powerField, ref variant3))
			{
				this._powerField = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._amount, ref variant4))
			{
				this._amount = variant4.As<int>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._isBuff, ref variant5))
			{
				this._isBuff = variant5.As<bool>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._textTween, ref variant6))
			{
				this._textTween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NPowerAppliedVfx.PropertyName._spriteTween, ref variant7))
			{
				this._spriteTween = variant7.As<Tween>();
			}
		}

		// Token: 0x040008B6 RID: 2230
		private const string _scenePath = "res://scenes/vfx/power_applied_vfx.tscn";

		// Token: 0x040008B7 RID: 2231
		private TextureRect _icon;

		// Token: 0x040008B8 RID: 2232
		private TextureRect _iconEcho;

		// Token: 0x040008B9 RID: 2233
		private MegaLabel _powerField;

		// Token: 0x040008BA RID: 2234
		private PowerModel _power;

		// Token: 0x040008BB RID: 2235
		private int _amount;

		// Token: 0x040008BC RID: 2236
		private bool _isBuff;

		// Token: 0x040008BD RID: 2237
		[Nullable(2)]
		private Tween _textTween;

		// Token: 0x040008BE RID: 2238
		[Nullable(2)]
		private Tween _spriteTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010EB RID: 4331
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400423C RID: 16956
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400423D RID: 16957
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010EC RID: 4332
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x0400423E RID: 16958
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_iconEcho' field.
			/// </summary>
			// Token: 0x0400423F RID: 16959
			public static readonly StringName _iconEcho = "_iconEcho";

			/// <summary>
			/// Cached name for the '_powerField' field.
			/// </summary>
			// Token: 0x04004240 RID: 16960
			public static readonly StringName _powerField = "_powerField";

			/// <summary>
			/// Cached name for the '_amount' field.
			/// </summary>
			// Token: 0x04004241 RID: 16961
			public static readonly StringName _amount = "_amount";

			/// <summary>
			/// Cached name for the '_isBuff' field.
			/// </summary>
			// Token: 0x04004242 RID: 16962
			public static readonly StringName _isBuff = "_isBuff";

			/// <summary>
			/// Cached name for the '_textTween' field.
			/// </summary>
			// Token: 0x04004243 RID: 16963
			public static readonly StringName _textTween = "_textTween";

			/// <summary>
			/// Cached name for the '_spriteTween' field.
			/// </summary>
			// Token: 0x04004244 RID: 16964
			public static readonly StringName _spriteTween = "_spriteTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010ED RID: 4333
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
