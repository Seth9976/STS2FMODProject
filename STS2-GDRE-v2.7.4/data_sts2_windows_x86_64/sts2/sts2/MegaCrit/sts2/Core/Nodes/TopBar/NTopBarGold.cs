using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.sts2.Core.Nodes.TopBar
{
	// Token: 0x0200001B RID: 27
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarGold.cs")]
	public class NTopBarGold : NClickableControl
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00007914 File Offset: 0x00005B14
		public override void _Ready()
		{
			this._goldLabel = base.GetNode<MegaLabel>("%GoldLabel");
			this._goldPopupLabel = base.GetNode<MegaLabel>("%GoldPopup");
			this._goldPopupLabel.Modulate = Colors.Transparent;
			this.ConnectSignals();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00007963 File Offset: 0x00005B63
		public override void _ExitTree()
		{
			base._ExitTree();
			CancellationTokenSource animCts = this._animCts;
			if (animCts != null)
			{
				animCts.Cancel();
			}
			if (this._player != null)
			{
				this._player.GoldChanged -= this.UpdateGold;
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000799C File Offset: 0x00005B9C
		[NullableContext(1)]
		public void Initialize(Player player)
		{
			this._player = player;
			this._currentGold = this._player.Gold;
			MegaLabel goldLabel = this._goldLabel;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._currentGold);
			goldLabel.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			this._player.GoldChanged += this.UpdateGold;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00007A00 File Offset: 0x00005C00
		private void UpdateGold()
		{
			TaskHelper.RunSafely(this.UpdateGoldAnim());
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00007A10 File Offset: 0x00005C10
		[NullableContext(1)]
		private Task UpdateGoldAnim()
		{
			NTopBarGold.<UpdateGoldAnim>d__11 <UpdateGoldAnim>d__;
			<UpdateGoldAnim>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<UpdateGoldAnim>d__.<>4__this = this;
			<UpdateGoldAnim>d__.<>1__state = -1;
			<UpdateGoldAnim>d__.<>t__builder.Start<NTopBarGold.<UpdateGoldAnim>d__11>(ref <UpdateGoldAnim>d__);
			return <UpdateGoldAnim>d__.<>t__builder.Task;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00007A54 File Offset: 0x00005C54
		protected override void OnFocus()
		{
			HoverTip hoverTip = new HoverTip(new LocString("static_hover_tips", "MONEY_POUCH.title"), new LocString("static_hover_tips", "MONEY_POUCH.description"), null);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + new Vector2(0f, base.Size.Y + 20f), false);
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00007AC5 File Offset: 0x00005CC5
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060000DD RID: 221 RVA: 0x00007AD0 File Offset: 0x00005CD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NTopBarGold.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarGold.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarGold.MethodName.UpdateGold, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarGold.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarGold.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060000DE RID: 222 RVA: 0x00007BD0 File Offset: 0x00005DD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBarGold.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarGold.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarGold.MethodName.UpdateGold && args.Count == 0)
			{
				this.UpdateGold();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarGold.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarGold.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060000DF RID: 223 RVA: 0x00007CA0 File Offset: 0x00005EA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBarGold.MethodName._Ready || (ref method) == NTopBarGold.MethodName._ExitTree || (ref method) == NTopBarGold.MethodName.UpdateGold || (ref method) == NTopBarGold.MethodName.OnFocus || (ref method) == NTopBarGold.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060000E0 RID: 224 RVA: 0x00007D00 File Offset: 0x00005F00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTopBarGold.PropertyName._goldLabel)
			{
				this._goldLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NTopBarGold.PropertyName._goldPopupLabel)
			{
				this._goldPopupLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NTopBarGold.PropertyName._currentGold)
			{
				this._currentGold = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NTopBarGold.PropertyName._additionalGold)
			{
				this._additionalGold = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NTopBarGold.PropertyName._alreadyRunning)
			{
				this._alreadyRunning = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060000E1 RID: 225 RVA: 0x00007D9C File Offset: 0x00005F9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBarGold.PropertyName._goldLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._goldLabel);
				return true;
			}
			if ((ref name) == NTopBarGold.PropertyName._goldPopupLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._goldPopupLabel);
				return true;
			}
			if ((ref name) == NTopBarGold.PropertyName._currentGold)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentGold);
				return true;
			}
			if ((ref name) == NTopBarGold.PropertyName._additionalGold)
			{
				value = VariantUtils.CreateFrom<int>(ref this._additionalGold);
				return true;
			}
			if ((ref name) == NTopBarGold.PropertyName._alreadyRunning)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._alreadyRunning);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060000E2 RID: 226 RVA: 0x00007E54 File Offset: 0x00006054
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTopBarGold.PropertyName._goldLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBarGold.PropertyName._goldPopupLabel, 0L, "", 4096L, false),
				new PropertyInfo(2L, NTopBarGold.PropertyName._currentGold, 0L, "", 4096L, false),
				new PropertyInfo(2L, NTopBarGold.PropertyName._additionalGold, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTopBarGold.PropertyName._alreadyRunning, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060000E3 RID: 227 RVA: 0x00007F0C File Offset: 0x0000610C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTopBarGold.PropertyName._goldLabel, Variant.From<MegaLabel>(ref this._goldLabel));
			info.AddProperty(NTopBarGold.PropertyName._goldPopupLabel, Variant.From<MegaLabel>(ref this._goldPopupLabel));
			info.AddProperty(NTopBarGold.PropertyName._currentGold, Variant.From<int>(ref this._currentGold));
			info.AddProperty(NTopBarGold.PropertyName._additionalGold, Variant.From<int>(ref this._additionalGold));
			info.AddProperty(NTopBarGold.PropertyName._alreadyRunning, Variant.From<bool>(ref this._alreadyRunning));
		}

		/// <inheritdoc />
		// Token: 0x060000E4 RID: 228 RVA: 0x00007F90 File Offset: 0x00006190
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTopBarGold.PropertyName._goldLabel, ref variant))
			{
				this._goldLabel = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTopBarGold.PropertyName._goldPopupLabel, ref variant2))
			{
				this._goldPopupLabel = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTopBarGold.PropertyName._currentGold, ref variant3))
			{
				this._currentGold = variant3.As<int>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTopBarGold.PropertyName._additionalGold, ref variant4))
			{
				this._additionalGold = variant4.As<int>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTopBarGold.PropertyName._alreadyRunning, ref variant5))
			{
				this._alreadyRunning = variant5.As<bool>();
			}
		}

		// Token: 0x04000054 RID: 84
		[Nullable(2)]
		private Player _player;

		// Token: 0x04000055 RID: 85
		[Nullable(1)]
		private MegaLabel _goldLabel;

		// Token: 0x04000056 RID: 86
		[Nullable(1)]
		private MegaLabel _goldPopupLabel;

		// Token: 0x04000057 RID: 87
		private int _currentGold;

		// Token: 0x04000058 RID: 88
		private int _additionalGold;

		// Token: 0x04000059 RID: 89
		private bool _alreadyRunning;

		// Token: 0x0400005A RID: 90
		[Nullable(2)]
		private CancellationTokenSource _animCts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DDF RID: 3551
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040031B9 RID: 12729
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040031BA RID: 12730
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdateGold' method.
			/// </summary>
			// Token: 0x040031BB RID: 12731
			public static readonly StringName UpdateGold = "UpdateGold";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040031BC RID: 12732
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040031BD RID: 12733
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DE0 RID: 3552
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the '_goldLabel' field.
			/// </summary>
			// Token: 0x040031BE RID: 12734
			public static readonly StringName _goldLabel = "_goldLabel";

			/// <summary>
			/// Cached name for the '_goldPopupLabel' field.
			/// </summary>
			// Token: 0x040031BF RID: 12735
			public static readonly StringName _goldPopupLabel = "_goldPopupLabel";

			/// <summary>
			/// Cached name for the '_currentGold' field.
			/// </summary>
			// Token: 0x040031C0 RID: 12736
			public static readonly StringName _currentGold = "_currentGold";

			/// <summary>
			/// Cached name for the '_additionalGold' field.
			/// </summary>
			// Token: 0x040031C1 RID: 12737
			public static readonly StringName _additionalGold = "_additionalGold";

			/// <summary>
			/// Cached name for the '_alreadyRunning' field.
			/// </summary>
			// Token: 0x040031C2 RID: 12738
			public static readonly StringName _alreadyRunning = "_alreadyRunning";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DE1 RID: 3553
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
