using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Overlays
{
	/// <summary>
	/// Node class that manages the ordering of all overlay screens.
	/// Any overlays added to the stack will sit on top of previously opened overlays,
	/// Only the top overlay is considered active
	/// overlays below it will not be active again until the overlays  on top of it are closed
	/// </summary>
	// Token: 0x020002E3 RID: 739
	[ScriptPath("res://src/Core/Nodes/Screens/Overlays/NOverlayStack.cs")]
	public class NOverlayStack : Control
	{
		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x0010EACC File Offset: 0x0010CCCC
		[Nullable(2)]
		public static NOverlayStack Instance
		{
			[NullableContext(2)]
			get
			{
				NRun instance = NRun.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.GlobalUi.Overlays;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002790 RID: 10128 RVA: 0x0010EAE3 File Offset: 0x0010CCE3
		public int ScreenCount
		{
			get
			{
				return this._overlays.Count;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002791 RID: 10129 RVA: 0x0010EAF0 File Offset: 0x0010CCF0
		private static bool StackIsCovered
		{
			get
			{
				NMapScreen instance = NMapScreen.Instance;
				if (instance == null || !instance.IsOpen)
				{
					NCapstoneContainer instance2 = NCapstoneContainer.Instance;
					return instance2 != null && instance2.InUse;
				}
				return true;
			}
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x0010EB18 File Offset: 0x0010CD18
		public override void _Ready()
		{
			this._backstop = base.GetNode<Control>("OverlayBackstop");
			this._backstop.Modulate = Colors.Transparent;
			this._backstop.MouseFilter = 2L;
			Callable.From<Error>(() => NMapScreen.Instance.Connect(NMapScreen.SignalName.Opened, Callable.From(new Action(this.HideOverlays)), 0U)).CallDeferred(Array.Empty<Variant>());
			Callable.From<Error>(() => NMapScreen.Instance.Connect(NMapScreen.SignalName.Closed, Callable.From(new Action(this.ShowOverlays)), 0U)).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x0010EB94 File Offset: 0x0010CD94
		public override void _EnterTree()
		{
			ActiveScreenContext.Instance.Updated += this.OnActiveScreenChanged;
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x0010EBAC File Offset: 0x0010CDAC
		public override void _ExitTree()
		{
			ActiveScreenContext.Instance.Updated -= this.OnActiveScreenChanged;
			this.Clear();
		}

		/// <summary>
		/// Adds a new screen to the top of the stack.
		/// Hides the previously open screen if there was one.
		/// </summary>
		/// <param name="screen">overlay screen that is being added to the stack.</param>
		// Token: 0x06002795 RID: 10133 RVA: 0x0010EBCC File Offset: 0x0010CDCC
		[NullableContext(1)]
		public void Push(IOverlayScreen screen)
		{
			IOverlayScreen overlayScreen = this.Peek();
			if (overlayScreen != null)
			{
				overlayScreen.AfterOverlayHidden();
			}
			this.AddChildSafely((Node)screen);
			this._overlays.Add(screen);
			screen.AfterOverlayOpened();
			screen.AfterOverlayShown();
			this._backstop.MouseFilter = 0L;
			Tween backstopFade = this._backstopFade;
			if (backstopFade != null)
			{
				backstopFade.Kill();
			}
			this.MoveChildSafely(this._backstop, this._overlays.IndexOf(screen));
			bool stackIsCovered = NOverlayStack.StackIsCovered;
			if (stackIsCovered)
			{
				screen.AfterOverlayHidden();
			}
			if (!screen.UseSharedBackstop || stackIsCovered)
			{
				this._backstop.Modulate = Colors.Transparent;
			}
			else if (this.ScreenCount == 1)
			{
				this.ShowBackstop();
			}
			else
			{
				this._backstop.Modulate = Colors.White;
			}
			ActiveScreenContext.Instance.Update();
			base.EmitSignal(NOverlayStack.SignalName.Changed, Array.Empty<Variant>());
		}

		/// <summary>
		/// Removes the specified screen from the stack.
		/// Sets up the previously open screen if there is one and the top screen was removed.
		/// Note: We have this instead of Pop because a screen is responsible for removing itself, and we can't guarantee
		/// that it'll be at the top of the stack when it needs to do this.
		/// </summary>
		/// <param name="screen">Overlay screen to be removed.</param>
		// Token: 0x06002796 RID: 10134 RVA: 0x0010ECB0 File Offset: 0x0010CEB0
		[NullableContext(1)]
		public void Remove(IOverlayScreen screen)
		{
			bool flag = screen == this.Peek();
			if (flag)
			{
				this.HideBackstop();
				screen.AfterOverlayHidden();
			}
			screen.AfterOverlayClosed();
			this._overlays.Remove(screen);
			if (flag)
			{
				IOverlayScreen overlayScreen = this.Peek();
				if (overlayScreen != null)
				{
					this._backstop.MouseFilter = 0L;
					this.MoveChildSafely(this._backstop, this._overlays.IndexOf(overlayScreen));
					if (overlayScreen.UseSharedBackstop)
					{
						this._backstop.Modulate = Colors.White;
					}
					else
					{
						this.HideBackstop();
					}
					overlayScreen.AfterOverlayShown();
				}
				else
				{
					this.HideBackstop();
				}
			}
			ActiveScreenContext.Instance.Update();
			base.EmitSignal(NOverlayStack.SignalName.Changed, Array.Empty<Variant>());
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x0010ED64 File Offset: 0x0010CF64
		public void Clear()
		{
			for (IOverlayScreen overlayScreen = this.Peek(); overlayScreen != null; overlayScreen = this.Peek())
			{
				this.Remove(overlayScreen);
			}
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x0010ED8B File Offset: 0x0010CF8B
		public void HideOverlays()
		{
			this._backstop.Modulate = Colors.Transparent;
			IOverlayScreen overlayScreen = this.Peek();
			if (overlayScreen == null)
			{
				return;
			}
			overlayScreen.AfterOverlayHidden();
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x0010EDB0 File Offset: 0x0010CFB0
		public void ShowOverlays()
		{
			IOverlayScreen overlayScreen = this.Peek();
			if (overlayScreen == null)
			{
				return;
			}
			if (NMapScreen.Instance.IsOpen)
			{
				return;
			}
			this._backstop.Modulate = (overlayScreen.UseSharedBackstop ? Colors.White : Colors.Transparent);
			overlayScreen.AfterOverlayShown();
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x0010EDFC File Offset: 0x0010CFFC
		public void ShowBackstop()
		{
			IOverlayScreen overlayScreen = this.Peek();
			if (overlayScreen != null && !overlayScreen.UseSharedBackstop)
			{
				return;
			}
			this._backstop.MouseFilter = 0L;
			Tween backstopFade = this._backstopFade;
			if (backstopFade != null)
			{
				backstopFade.Kill();
			}
			this._backstopFade = base.CreateTween();
			this._backstopFade.TweenProperty(this._backstop, "modulate:a", 1f, 0.5).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x0010EE88 File Offset: 0x0010D088
		public void HideBackstop()
		{
			IOverlayScreen overlayScreen = this.Peek();
			if (overlayScreen != null && !overlayScreen.UseSharedBackstop)
			{
				return;
			}
			this._backstop.MouseFilter = 2L;
			Tween backstopFade = this._backstopFade;
			if (backstopFade != null)
			{
				backstopFade.Kill();
			}
			if (this.ScreenCount <= 1)
			{
				this._backstopFade = base.CreateTween();
				this._backstopFade.TweenProperty(this._backstop, "modulate:a", 0f, 0.5).SetEase(1L).SetTrans(7L);
				return;
			}
			this._backstop.Modulate = Colors.Transparent;
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x0010EF2E File Offset: 0x0010D12E
		[NullableContext(2)]
		public IOverlayScreen Peek()
		{
			return this._overlays.LastOrDefault<IOverlayScreen>();
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x0010EF3C File Offset: 0x0010D13C
		private void OnActiveScreenChanged()
		{
			IOverlayScreen overlayScreen = this.Peek();
			if (overlayScreen == null)
			{
				return;
			}
			if (ActiveScreenContext.Instance.IsCurrent(overlayScreen))
			{
				base.FocusBehaviorRecursive = 2L;
				return;
			}
			base.FocusBehaviorRecursive = 1L;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600279E RID: 10142 RVA: 0x0010EF74 File Offset: 0x0010D174
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NOverlayStack.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOverlayStack.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOverlayStack.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOverlayStack.MethodName.Clear, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOverlayStack.MethodName.HideOverlays, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOverlayStack.MethodName.ShowOverlays, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOverlayStack.MethodName.ShowBackstop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOverlayStack.MethodName.HideBackstop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOverlayStack.MethodName.OnActiveScreenChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600279F RID: 10143 RVA: 0x0010F134 File Offset: 0x0010D334
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NOverlayStack.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOverlayStack.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOverlayStack.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOverlayStack.MethodName.Clear && args.Count == 0)
			{
				this.Clear();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOverlayStack.MethodName.HideOverlays && args.Count == 0)
			{
				this.HideOverlays();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOverlayStack.MethodName.ShowOverlays && args.Count == 0)
			{
				this.ShowOverlays();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOverlayStack.MethodName.ShowBackstop && args.Count == 0)
			{
				this.ShowBackstop();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOverlayStack.MethodName.HideBackstop && args.Count == 0)
			{
				this.HideBackstop();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOverlayStack.MethodName.OnActiveScreenChanged && args.Count == 0)
			{
				this.OnActiveScreenChanged();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060027A0 RID: 10144 RVA: 0x0010F298 File Offset: 0x0010D498
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NOverlayStack.MethodName._Ready || (ref method) == NOverlayStack.MethodName._EnterTree || (ref method) == NOverlayStack.MethodName._ExitTree || (ref method) == NOverlayStack.MethodName.Clear || (ref method) == NOverlayStack.MethodName.HideOverlays || (ref method) == NOverlayStack.MethodName.ShowOverlays || (ref method) == NOverlayStack.MethodName.ShowBackstop || (ref method) == NOverlayStack.MethodName.HideBackstop || (ref method) == NOverlayStack.MethodName.OnActiveScreenChanged || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060027A1 RID: 10145 RVA: 0x0010F333 File Offset: 0x0010D533
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NOverlayStack.PropertyName._backstop)
			{
				this._backstop = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NOverlayStack.PropertyName._backstopFade)
			{
				this._backstopFade = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060027A2 RID: 10146 RVA: 0x0010F374 File Offset: 0x0010D574
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NOverlayStack.PropertyName.ScreenCount)
			{
				int screenCount = this.ScreenCount;
				value = VariantUtils.CreateFrom<int>(ref screenCount);
				return true;
			}
			if ((ref name) == NOverlayStack.PropertyName._backstop)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._backstop);
				return true;
			}
			if ((ref name) == NOverlayStack.PropertyName._backstopFade)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._backstopFade);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060027A3 RID: 10147 RVA: 0x0010F3EC File Offset: 0x0010D5EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NOverlayStack.PropertyName.ScreenCount, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOverlayStack.PropertyName._backstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOverlayStack.PropertyName._backstopFade, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060027A4 RID: 10148 RVA: 0x0010F464 File Offset: 0x0010D664
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NOverlayStack.PropertyName._backstop, Variant.From<Control>(ref this._backstop));
			info.AddProperty(NOverlayStack.PropertyName._backstopFade, Variant.From<Tween>(ref this._backstopFade));
			info.AddSignalEventDelegate(NOverlayStack.SignalName.Changed, this.backing_Changed);
		}

		/// <inheritdoc />
		// Token: 0x060027A5 RID: 10149 RVA: 0x0010F4B8 File Offset: 0x0010D6B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NOverlayStack.PropertyName._backstop, ref variant))
			{
				this._backstop = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NOverlayStack.PropertyName._backstopFade, ref variant2))
			{
				this._backstopFade = variant2.As<Tween>();
			}
			NOverlayStack.ChangedEventHandler changedEventHandler;
			if (info.TryGetSignalEventDelegate<NOverlayStack.ChangedEventHandler>(NOverlayStack.SignalName.Changed, ref changedEventHandler))
			{
				this.backing_Changed = changedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060027A6 RID: 10150 RVA: 0x0010F51C File Offset: 0x0010D71C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NOverlayStack.SignalName.Changed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Overlays.NOverlayStack.ChangedEventHandler" />
		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060027A7 RID: 10151 RVA: 0x0010F560 File Offset: 0x0010D760
		// (remove) Token: 0x060027A8 RID: 10152 RVA: 0x0010F579 File Offset: 0x0010D779
		public event NOverlayStack.ChangedEventHandler Changed
		{
			add
			{
				this.backing_Changed = (NOverlayStack.ChangedEventHandler)Delegate.Combine(this.backing_Changed, value);
			}
			remove
			{
				this.backing_Changed = (NOverlayStack.ChangedEventHandler)Delegate.Remove(this.backing_Changed, value);
			}
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x0010F592 File Offset: 0x0010D792
		protected void EmitSignalChanged()
		{
			base.EmitSignal(NOverlayStack.SignalName.Changed, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x060027AA RID: 10154 RVA: 0x0010F5A5 File Offset: 0x0010D7A5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NOverlayStack.SignalName.Changed) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NOverlayStack.ChangedEventHandler changedEventHandler = this.backing_Changed;
			if (changedEventHandler == null)
			{
				return;
			}
			changedEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x060027AB RID: 10155 RVA: 0x0010F5D6 File Offset: 0x0010D7D6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NOverlayStack.SignalName.Changed || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000EC0 RID: 3776
		[Nullable(1)]
		private readonly List<IOverlayScreen> _overlays = new List<IOverlayScreen>();

		// Token: 0x04000EC1 RID: 3777
		[Nullable(1)]
		private Control _backstop;

		// Token: 0x04000EC2 RID: 3778
		[Nullable(2)]
		private Tween _backstopFade;

		// Token: 0x04000EC3 RID: 3779
		private NOverlayStack.ChangedEventHandler backing_Changed;

		// Token: 0x0200147D RID: 5245
		// (Invoke) Token: 0x0600A0E8 RID: 41192
		[Signal]
		public delegate void ChangedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200147E RID: 5246
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004DC5 RID: 19909
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04004DC6 RID: 19910
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004DC7 RID: 19911
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Clear' method.
			/// </summary>
			// Token: 0x04004DC8 RID: 19912
			public static readonly StringName Clear = "Clear";

			/// <summary>
			/// Cached name for the 'HideOverlays' method.
			/// </summary>
			// Token: 0x04004DC9 RID: 19913
			public static readonly StringName HideOverlays = "HideOverlays";

			/// <summary>
			/// Cached name for the 'ShowOverlays' method.
			/// </summary>
			// Token: 0x04004DCA RID: 19914
			public static readonly StringName ShowOverlays = "ShowOverlays";

			/// <summary>
			/// Cached name for the 'ShowBackstop' method.
			/// </summary>
			// Token: 0x04004DCB RID: 19915
			public static readonly StringName ShowBackstop = "ShowBackstop";

			/// <summary>
			/// Cached name for the 'HideBackstop' method.
			/// </summary>
			// Token: 0x04004DCC RID: 19916
			public static readonly StringName HideBackstop = "HideBackstop";

			/// <summary>
			/// Cached name for the 'OnActiveScreenChanged' method.
			/// </summary>
			// Token: 0x04004DCD RID: 19917
			public static readonly StringName OnActiveScreenChanged = "OnActiveScreenChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200147F RID: 5247
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenCount' property.
			/// </summary>
			// Token: 0x04004DCE RID: 19918
			public static readonly StringName ScreenCount = "ScreenCount";

			/// <summary>
			/// Cached name for the '_backstop' field.
			/// </summary>
			// Token: 0x04004DCF RID: 19919
			public static readonly StringName _backstop = "_backstop";

			/// <summary>
			/// Cached name for the '_backstopFade' field.
			/// </summary>
			// Token: 0x04004DD0 RID: 19920
			public static readonly StringName _backstopFade = "_backstopFade";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001480 RID: 5248
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'Changed' signal.
			/// </summary>
			// Token: 0x04004DD1 RID: 19921
			public static readonly StringName Changed = "Changed";
		}
	}
}
