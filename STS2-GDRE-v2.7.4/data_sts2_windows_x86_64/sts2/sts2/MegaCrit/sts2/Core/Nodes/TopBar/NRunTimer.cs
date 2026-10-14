using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.TopBar
{
	// Token: 0x02000242 RID: 578
	[ScriptPath("res://src/Core/Nodes/TopBar/NRunTimer.cs")]
	public class NRunTimer : Control
	{
		// Token: 0x06001D6D RID: 7533 RVA: 0x000BF86F File Offset: 0x000BDA6F
		public override void _Ready()
		{
			this._timerLabel = base.GetNode<MegaLabel>("TimerLabel");
			this.ToggleTimer(false);
			base.CallDeferred("DeferredInit", Array.Empty<Variant>());
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x000BF8A4 File Offset: 0x000BDAA4
		private void DeferredInit()
		{
			NMapScreen.Instance.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.RefreshVisibility)), 0U);
			NCapstoneContainer.Instance.Connect(NCapstoneContainer.SignalName.Changed, Callable.From(new Action(this.RefreshVisibility)), 0U);
			this._timer = new Timer();
			this._timer.WaitTime = 1.0;
			this._timer.Autostart = false;
			this._timer.Connect(Timer.SignalName.Timeout, Callable.From(delegate
			{
				this.RefreshVisibility();
				this.OnTimerTimeout();
			}), 0U);
			this.AddChildSafely(this._timer);
			this._timer.Start(-1.0);
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000BF963 File Offset: 0x000BDB63
		public override void _ExitTree()
		{
			this._timer.Stop();
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x000BF970 File Offset: 0x000BDB70
		public void RefreshVisibility()
		{
			if (SaveManager.Instance.PrefsSave.ShowRunTimer)
			{
				this.ToggleTimer(true);
				return;
			}
			this.ToggleTimer(NCapstoneContainer.Instance.InUse || NMapScreen.Instance.Visible);
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x000BF9AA File Offset: 0x000BDBAA
		private void ToggleTimer(bool on)
		{
			base.Visible = on;
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x000BF9B3 File Offset: 0x000BDBB3
		private void OnTimerTimeout()
		{
			if (!RunManager.Instance.IsGameOver)
			{
				this._timerLabel.SetTextAutoSize(TimeFormatting.Format((float)RunManager.Instance.RunTime));
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D73 RID: 7539 RVA: 0x000BF9DC File Offset: 0x000BDBDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NRunTimer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunTimer.MethodName.DeferredInit, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunTimer.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunTimer.MethodName.RefreshVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunTimer.MethodName.ToggleTimer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "on", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRunTimer.MethodName.OnTimerTimeout, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D74 RID: 7540 RVA: 0x000BFB30 File Offset: 0x000BDD30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunTimer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunTimer.MethodName.DeferredInit && args.Count == 0)
			{
				this.DeferredInit();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunTimer.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunTimer.MethodName.RefreshVisibility && args.Count == 0)
			{
				this.RefreshVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunTimer.MethodName.ToggleTimer && args.Count == 1)
			{
				this.ToggleTimer(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunTimer.MethodName.OnTimerTimeout && args.Count == 0)
			{
				this.OnTimerTimeout();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001D75 RID: 7541 RVA: 0x000BFC34 File Offset: 0x000BDE34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRunTimer.MethodName._Ready || (ref method) == NRunTimer.MethodName.DeferredInit || (ref method) == NRunTimer.MethodName._ExitTree || (ref method) == NRunTimer.MethodName.RefreshVisibility || (ref method) == NRunTimer.MethodName.ToggleTimer || (ref method) == NRunTimer.MethodName.OnTimerTimeout || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001D76 RID: 7542 RVA: 0x000BFCA2 File Offset: 0x000BDEA2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRunTimer.PropertyName._timerLabel)
			{
				this._timerLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunTimer.PropertyName._timer)
			{
				this._timer = VariantUtils.ConvertTo<Timer>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001D77 RID: 7543 RVA: 0x000BFCE4 File Offset: 0x000BDEE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRunTimer.PropertyName._timerLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._timerLabel);
				return true;
			}
			if ((ref name) == NRunTimer.PropertyName._timer)
			{
				value = VariantUtils.CreateFrom<Timer>(ref this._timer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D78 RID: 7544 RVA: 0x000BFD3C File Offset: 0x000BDF3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRunTimer.PropertyName._timerLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunTimer.PropertyName._timer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D79 RID: 7545 RVA: 0x000BFD92 File Offset: 0x000BDF92
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRunTimer.PropertyName._timerLabel, Variant.From<MegaLabel>(ref this._timerLabel));
			info.AddProperty(NRunTimer.PropertyName._timer, Variant.From<Timer>(ref this._timer));
		}

		/// <inheritdoc />
		// Token: 0x06001D7A RID: 7546 RVA: 0x000BFDC8 File Offset: 0x000BDFC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRunTimer.PropertyName._timerLabel, ref variant))
			{
				this._timerLabel = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRunTimer.PropertyName._timer, ref variant2))
			{
				this._timer = variant2.As<Timer>();
			}
		}

		// Token: 0x04000B53 RID: 2899
		[Nullable(1)]
		private MegaLabel _timerLabel;

		// Token: 0x04000B54 RID: 2900
		[Nullable(1)]
		private Timer _timer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200123B RID: 4667
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004682 RID: 18050
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'DeferredInit' method.
			/// </summary>
			// Token: 0x04004683 RID: 18051
			public static readonly StringName DeferredInit = "DeferredInit";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004684 RID: 18052
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'RefreshVisibility' method.
			/// </summary>
			// Token: 0x04004685 RID: 18053
			public static readonly StringName RefreshVisibility = "RefreshVisibility";

			/// <summary>
			/// Cached name for the 'ToggleTimer' method.
			/// </summary>
			// Token: 0x04004686 RID: 18054
			public static readonly StringName ToggleTimer = "ToggleTimer";

			/// <summary>
			/// Cached name for the 'OnTimerTimeout' method.
			/// </summary>
			// Token: 0x04004687 RID: 18055
			public static readonly StringName OnTimerTimeout = "OnTimerTimeout";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200123C RID: 4668
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_timerLabel' field.
			/// </summary>
			// Token: 0x04004688 RID: 18056
			public static readonly StringName _timerLabel = "_timerLabel";

			/// <summary>
			/// Cached name for the '_timer' field.
			/// </summary>
			// Token: 0x04004689 RID: 18057
			public static readonly StringName _timer = "_timer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200123D RID: 4669
		public class SignalName : Control.SignalName
		{
		}
	}
}
