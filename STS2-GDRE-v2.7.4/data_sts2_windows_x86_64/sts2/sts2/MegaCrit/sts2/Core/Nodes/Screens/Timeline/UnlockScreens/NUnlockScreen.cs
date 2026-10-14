using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens
{
	/// <summary>
	/// Abstract class of the Unlock Screens. Used for general animation and some logistics required for the Timeline Screen.
	/// All Unlock Screens in the Timeline must use this!
	/// </summary>
	// Token: 0x0200026F RID: 623
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockScreen.cs")]
	public abstract class NUnlockScreen : Control, IScreenContext
	{
		// Token: 0x060020DC RID: 8412 RVA: 0x000DECC4 File Offset: 0x000DCEC4
		public override void _Ready()
		{
			if (base.GetType() != typeof(NUnlockScreen))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000DED1C File Offset: 0x000DCF1C
		protected void ConnectSignals()
		{
			this._unlockConfirmButton = base.GetNode<NUnlockConfirmButton>("ConfirmButton");
			this._unlockConfirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(delegate(NClickableControl _)
			{
				TaskHelper.RunSafely(this.Close());
			}), 0U);
		}

		/// <summary>
		/// Useful for UI stuff because this Node is in the Scene so the positions are accurate.
		/// See: NUnlockEpochScreen where we animate the Epochs on screen open!
		/// </summary>
		// Token: 0x060020DE RID: 8414 RVA: 0x000DED58 File Offset: 0x000DCF58
		public virtual void Open()
		{
			NTimelineScreen.Instance.DisableInput();
			NTimelineScreen.Instance.CurrentUnlockScreen = this;
			NUnlockConfirmButton unlockConfirmButton = this._unlockConfirmButton;
			if (unlockConfirmButton != null)
			{
				unlockConfirmButton.Disable();
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.FastForwardToCompletion();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.5);
			this._tween.Chain().TweenCallback(Callable.From(delegate
			{
				NUnlockConfirmButton unlockConfirmButton2 = this._unlockConfirmButton;
				if (unlockConfirmButton2 == null)
				{
					return;
				}
				unlockConfirmButton2.Enable();
			}));
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x000DEDF4 File Offset: 0x000DCFF4
		[NullableContext(1)]
		protected async Task Close()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Closing: ");
			defaultInterpolatedStringHandler.AppendFormatted<StringName>(base.Name);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			if (NTimelineScreen.Instance.CurrentUnlockScreen == this)
			{
				NTimelineScreen.Instance.CurrentUnlockScreen = null;
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.FastForwardToCompletion();
			}
			this.OnScreenPreClose();
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate", StsColors.transparentBlack, 1.0);
			TaskAwaiter<bool> taskAwaiter = this._tween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this.OnScreenClose();
				if (NTimelineScreen.Instance.IsScreenQueued())
				{
					NTimelineScreen.Instance.OpenQueuedScreen();
				}
				else
				{
					await NTimelineScreen.Instance.HideBackstopAndShowUi(true);
				}
				this.QueueFreeSafely();
			}
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000DEE37 File Offset: 0x000DD037
		protected virtual void OnScreenPreClose()
		{
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000DEE39 File Offset: 0x000DD039
		protected virtual void OnScreenClose()
		{
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060020E2 RID: 8418 RVA: 0x000DEE3B File Offset: 0x000DD03B
		[Nullable(2)]
		public virtual Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060020E3 RID: 8419 RVA: 0x000DEE40 File Offset: 0x000DD040
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NUnlockScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockScreen.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockScreen.MethodName.Open, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockScreen.MethodName.OnScreenPreClose, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockScreen.MethodName.OnScreenClose, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060020E4 RID: 8420 RVA: 0x000DEF40 File Offset: 0x000DD140
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockScreen.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockScreen.MethodName.Open && args.Count == 0)
			{
				this.Open();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockScreen.MethodName.OnScreenPreClose && args.Count == 0)
			{
				this.OnScreenPreClose();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockScreen.MethodName.OnScreenClose && args.Count == 0)
			{
				this.OnScreenClose();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060020E5 RID: 8421 RVA: 0x000DF010 File Offset: 0x000DD210
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUnlockScreen.MethodName._Ready || (ref method) == NUnlockScreen.MethodName.ConnectSignals || (ref method) == NUnlockScreen.MethodName.Open || (ref method) == NUnlockScreen.MethodName.OnScreenPreClose || (ref method) == NUnlockScreen.MethodName.OnScreenClose || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060020E6 RID: 8422 RVA: 0x000DF06F File Offset: 0x000DD26F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUnlockScreen.PropertyName._unlockConfirmButton)
			{
				this._unlockConfirmButton = VariantUtils.ConvertTo<NUnlockConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NUnlockScreen.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060020E7 RID: 8423 RVA: 0x000DF0B0 File Offset: 0x000DD2B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUnlockScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NUnlockScreen.PropertyName._unlockConfirmButton)
			{
				value = VariantUtils.CreateFrom<NUnlockConfirmButton>(ref this._unlockConfirmButton);
				return true;
			}
			if ((ref name) == NUnlockScreen.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060020E8 RID: 8424 RVA: 0x000DF128 File Offset: 0x000DD328
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUnlockScreen.PropertyName._unlockConfirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockScreen.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060020E9 RID: 8425 RVA: 0x000DF19F File Offset: 0x000DD39F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUnlockScreen.PropertyName._unlockConfirmButton, Variant.From<NUnlockConfirmButton>(ref this._unlockConfirmButton));
			info.AddProperty(NUnlockScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x060020EA RID: 8426 RVA: 0x000DF1D4 File Offset: 0x000DD3D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUnlockScreen.PropertyName._unlockConfirmButton, ref variant))
			{
				this._unlockConfirmButton = variant.As<NUnlockConfirmButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NUnlockScreen.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
		}

		// Token: 0x04000CCF RID: 3279
		[Nullable(2)]
		private NUnlockConfirmButton _unlockConfirmButton;

		// Token: 0x04000CD0 RID: 3280
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012EB RID: 4843
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040049AA RID: 18858
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x040049AB RID: 18859
			public static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x040049AC RID: 18860
			public static readonly StringName Open = "Open";

			/// <summary>
			/// Cached name for the 'OnScreenPreClose' method.
			/// </summary>
			// Token: 0x040049AD RID: 18861
			public static readonly StringName OnScreenPreClose = "OnScreenPreClose";

			/// <summary>
			/// Cached name for the 'OnScreenClose' method.
			/// </summary>
			// Token: 0x040049AE RID: 18862
			public static readonly StringName OnScreenClose = "OnScreenClose";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012EC RID: 4844
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040049AF RID: 18863
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_unlockConfirmButton' field.
			/// </summary>
			// Token: 0x040049B0 RID: 18864
			public static readonly StringName _unlockConfirmButton = "_unlockConfirmButton";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040049B1 RID: 18865
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012ED RID: 4845
		public class SignalName : Control.SignalName
		{
		}
	}
}
