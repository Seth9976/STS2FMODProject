using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Capstones
{
	/// <summary>
	/// Node class that manages the current capstone screen.
	/// If a new Capstone screen is opened, any previously open capstone screens are destroyed.
	/// If a new Capstone screen is opened, any previously open overlay screens are hidden until there are no more capstone screens.
	/// </summary>
	// Token: 0x0200035B RID: 859
	[ScriptPath("res://src/Core/Nodes/Screens/Capstones/NCapstoneContainer.cs")]
	public class NCapstoneContainer : Control
	{
		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x060031A8 RID: 12712 RVA: 0x00170E94 File Offset: 0x0016F094
		// (set) Token: 0x060031A9 RID: 12713 RVA: 0x00170E9C File Offset: 0x0016F09C
		[Nullable(2)]
		public ICapstoneScreen CurrentCapstoneScreen
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060031AA RID: 12714 RVA: 0x00170EA5 File Offset: 0x0016F0A5
		public bool InUse
		{
			get
			{
				return this.CurrentCapstoneScreen != null;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060031AB RID: 12715 RVA: 0x00170EB0 File Offset: 0x0016F0B0
		[Nullable(2)]
		public static NCapstoneContainer Instance
		{
			[NullableContext(2)]
			get
			{
				NRun instance = NRun.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.GlobalUi.CapstoneContainer;
			}
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x00170EC7 File Offset: 0x0016F0C7
		public override void _Ready()
		{
			this._backstop = base.GetNode<Control>("CapstoneBackstop");
			this._backstop.Modulate = Colors.Transparent;
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x00170EEF File Offset: 0x0016F0EF
		public override void _EnterTree()
		{
			ActiveScreenContext.Instance.Updated += this.OnActiveScreenChanged;
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x00170F07 File Offset: 0x0016F107
		public override void _ExitTree()
		{
			ActiveScreenContext.Instance.Updated -= this.OnActiveScreenChanged;
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x00170F20 File Offset: 0x0016F120
		[NullableContext(1)]
		public void Open(ICapstoneScreen screen)
		{
			NHoverTipSet.Clear();
			bool flag = this.CurrentCapstoneScreen != null;
			if (flag)
			{
				this.CloseInternal();
			}
			Tween backstopFade = this._backstopFade;
			if (backstopFade != null)
			{
				backstopFade.Kill();
			}
			NOverlayStack.Instance.HideOverlays();
			if (!screen.UseSharedBackstop)
			{
				this._backstop.Modulate = Colors.Transparent;
			}
			else if (flag || NOverlayStack.Instance.ScreenCount > 0)
			{
				this._backstop.Modulate = Colors.White;
			}
			else
			{
				this._backstopFade = base.CreateTween();
				this._backstopFade.TweenProperty(this._backstop, "modulate:a", 1f, 0.5).SetEase(1L).SetTrans(7L);
			}
			this.CurrentCapstoneScreen = screen;
			if (!base.GetChildren(false).Contains((Node)screen))
			{
				this.AddChildSafely((Node)screen);
			}
			((Node)screen).ProcessMode = 0L;
			screen.AfterCapstoneOpened();
			if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
			{
				CombatManager.Instance.Pause();
			}
			ActiveScreenContext.Instance.Update();
			base.EmitSignal(NCapstoneContainer.SignalName.Changed, Array.Empty<Variant>());
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x00171050 File Offset: 0x0016F250
		public void Close()
		{
			if (this.CurrentCapstoneScreen == null)
			{
				return;
			}
			this.CloseInternal();
			ActiveScreenContext.Instance.Update();
			base.EmitSignal(NCapstoneContainer.SignalName.CapstoneClosed, Array.Empty<Variant>());
			base.EmitSignal(NCapstoneContainer.SignalName.Changed, Array.Empty<Variant>());
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x00171090 File Offset: 0x0016F290
		private void CloseInternal()
		{
			if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
			{
				CombatManager.Instance.Unpause();
			}
			NOverlayStack.Instance.ShowOverlays();
			if (NOverlayStack.Instance.ScreenCount > 0)
			{
				this._backstop.Modulate = Colors.Transparent;
			}
			else
			{
				Tween backstopFade = this._backstopFade;
				if (backstopFade != null)
				{
					backstopFade.Kill();
				}
				this._backstopFade = base.CreateTween();
				this._backstopFade.TweenProperty(this._backstop, "modulate:a", 0f, 0.5).SetEase(1L).SetTrans(7L);
			}
			ICapstoneScreen currentCapstoneScreen = this.CurrentCapstoneScreen;
			this.CurrentCapstoneScreen = null;
			Node node = currentCapstoneScreen as Node;
			if (node != null)
			{
				node.ProcessMode = 4L;
			}
			if (currentCapstoneScreen != null)
			{
				currentCapstoneScreen.AfterCapstoneClosed();
			}
			NHoverTipSet.Clear();
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x00171164 File Offset: 0x0016F364
		public void DisableBackstopInstantly()
		{
			Tween backstopFade = this._backstopFade;
			if (backstopFade != null)
			{
				backstopFade.Kill();
			}
			this._backstop.Modulate = Colors.Transparent;
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x00171187 File Offset: 0x0016F387
		public void EnableBackstopInstantly()
		{
			Tween backstopFade = this._backstopFade;
			if (backstopFade != null)
			{
				backstopFade.Kill();
			}
			this._backstop.Modulate = Colors.White;
		}

		/// <summary>
		/// Called just before the run is disposed.
		/// </summary>
		// Token: 0x060031B4 RID: 12724 RVA: 0x001711AA File Offset: 0x0016F3AA
		public void CleanUp()
		{
			if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
			{
				CombatManager.Instance.Unpause();
			}
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x001711C2 File Offset: 0x0016F3C2
		private void OnActiveScreenChanged()
		{
			if (!this.InUse)
			{
				return;
			}
			if (ActiveScreenContext.Instance.IsCurrent(this.CurrentCapstoneScreen))
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
		// Token: 0x060031B6 RID: 12726 RVA: 0x001711F0 File Offset: 0x0016F3F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NCapstoneContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneContainer.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneContainer.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneContainer.MethodName.Close, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneContainer.MethodName.CloseInternal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneContainer.MethodName.DisableBackstopInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneContainer.MethodName.EnableBackstopInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneContainer.MethodName.CleanUp, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneContainer.MethodName.OnActiveScreenChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060031B7 RID: 12727 RVA: 0x001713B0 File Offset: 0x0016F5B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCapstoneContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCapstoneContainer.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCapstoneContainer.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCapstoneContainer.MethodName.Close && args.Count == 0)
			{
				this.Close();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCapstoneContainer.MethodName.CloseInternal && args.Count == 0)
			{
				this.CloseInternal();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCapstoneContainer.MethodName.DisableBackstopInstantly && args.Count == 0)
			{
				this.DisableBackstopInstantly();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCapstoneContainer.MethodName.EnableBackstopInstantly && args.Count == 0)
			{
				this.EnableBackstopInstantly();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCapstoneContainer.MethodName.CleanUp && args.Count == 0)
			{
				this.CleanUp();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCapstoneContainer.MethodName.OnActiveScreenChanged && args.Count == 0)
			{
				this.OnActiveScreenChanged();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060031B8 RID: 12728 RVA: 0x00171514 File Offset: 0x0016F714
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCapstoneContainer.MethodName._Ready || (ref method) == NCapstoneContainer.MethodName._EnterTree || (ref method) == NCapstoneContainer.MethodName._ExitTree || (ref method) == NCapstoneContainer.MethodName.Close || (ref method) == NCapstoneContainer.MethodName.CloseInternal || (ref method) == NCapstoneContainer.MethodName.DisableBackstopInstantly || (ref method) == NCapstoneContainer.MethodName.EnableBackstopInstantly || (ref method) == NCapstoneContainer.MethodName.CleanUp || (ref method) == NCapstoneContainer.MethodName.OnActiveScreenChanged || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060031B9 RID: 12729 RVA: 0x001715AF File Offset: 0x0016F7AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCapstoneContainer.PropertyName._backstop)
			{
				this._backstop = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCapstoneContainer.PropertyName._backstopFade)
			{
				this._backstopFade = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060031BA RID: 12730 RVA: 0x001715F0 File Offset: 0x0016F7F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCapstoneContainer.PropertyName.InUse)
			{
				bool inUse = this.InUse;
				value = VariantUtils.CreateFrom<bool>(ref inUse);
				return true;
			}
			if ((ref name) == NCapstoneContainer.PropertyName._backstop)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._backstop);
				return true;
			}
			if ((ref name) == NCapstoneContainer.PropertyName._backstopFade)
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
		// Token: 0x060031BB RID: 12731 RVA: 0x00171668 File Offset: 0x0016F868
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NCapstoneContainer.PropertyName.InUse, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCapstoneContainer.PropertyName._backstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCapstoneContainer.PropertyName._backstopFade, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060031BC RID: 12732 RVA: 0x001716E0 File Offset: 0x0016F8E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCapstoneContainer.PropertyName._backstop, Variant.From<Control>(ref this._backstop));
			info.AddProperty(NCapstoneContainer.PropertyName._backstopFade, Variant.From<Tween>(ref this._backstopFade));
			info.AddSignalEventDelegate(NCapstoneContainer.SignalName.Changed, this.backing_Changed);
			info.AddSignalEventDelegate(NCapstoneContainer.SignalName.CapstoneClosed, this.backing_CapstoneClosed);
		}

		/// <inheritdoc />
		// Token: 0x060031BD RID: 12733 RVA: 0x00171744 File Offset: 0x0016F944
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCapstoneContainer.PropertyName._backstop, ref variant))
			{
				this._backstop = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCapstoneContainer.PropertyName._backstopFade, ref variant2))
			{
				this._backstopFade = variant2.As<Tween>();
			}
			NCapstoneContainer.ChangedEventHandler changedEventHandler;
			if (info.TryGetSignalEventDelegate<NCapstoneContainer.ChangedEventHandler>(NCapstoneContainer.SignalName.Changed, ref changedEventHandler))
			{
				this.backing_Changed = changedEventHandler;
			}
			NCapstoneContainer.CapstoneClosedEventHandler capstoneClosedEventHandler;
			if (info.TryGetSignalEventDelegate<NCapstoneContainer.CapstoneClosedEventHandler>(NCapstoneContainer.SignalName.CapstoneClosed, ref capstoneClosedEventHandler))
			{
				this.backing_CapstoneClosed = capstoneClosedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060031BE RID: 12734 RVA: 0x001717BC File Offset: 0x0016F9BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCapstoneContainer.SignalName.Changed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneContainer.SignalName.CapstoneClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Capstones.NCapstoneContainer.ChangedEventHandler" />
		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060031BF RID: 12735 RVA: 0x0017182F File Offset: 0x0016FA2F
		// (remove) Token: 0x060031C0 RID: 12736 RVA: 0x00171848 File Offset: 0x0016FA48
		public event NCapstoneContainer.ChangedEventHandler Changed
		{
			add
			{
				this.backing_Changed = (NCapstoneContainer.ChangedEventHandler)Delegate.Combine(this.backing_Changed, value);
			}
			remove
			{
				this.backing_Changed = (NCapstoneContainer.ChangedEventHandler)Delegate.Remove(this.backing_Changed, value);
			}
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x00171861 File Offset: 0x0016FA61
		protected void EmitSignalChanged()
		{
			base.EmitSignal(NCapstoneContainer.SignalName.Changed, Array.Empty<Variant>());
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Capstones.NCapstoneContainer.CapstoneClosedEventHandler" />
		// Token: 0x1400002B RID: 43
		// (add) Token: 0x060031C2 RID: 12738 RVA: 0x00171874 File Offset: 0x0016FA74
		// (remove) Token: 0x060031C3 RID: 12739 RVA: 0x0017188D File Offset: 0x0016FA8D
		public event NCapstoneContainer.CapstoneClosedEventHandler CapstoneClosed
		{
			add
			{
				this.backing_CapstoneClosed = (NCapstoneContainer.CapstoneClosedEventHandler)Delegate.Combine(this.backing_CapstoneClosed, value);
			}
			remove
			{
				this.backing_CapstoneClosed = (NCapstoneContainer.CapstoneClosedEventHandler)Delegate.Remove(this.backing_CapstoneClosed, value);
			}
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x001718A6 File Offset: 0x0016FAA6
		protected void EmitSignalCapstoneClosed()
		{
			base.EmitSignal(NCapstoneContainer.SignalName.CapstoneClosed, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x060031C5 RID: 12741 RVA: 0x001718BC File Offset: 0x0016FABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NCapstoneContainer.SignalName.Changed && args.Count == 0)
			{
				NCapstoneContainer.ChangedEventHandler changedEventHandler = this.backing_Changed;
				if (changedEventHandler == null)
				{
					return;
				}
				changedEventHandler();
				return;
			}
			else
			{
				if (!((ref signal) == NCapstoneContainer.SignalName.CapstoneClosed) || args.Count != 0)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NCapstoneContainer.CapstoneClosedEventHandler capstoneClosedEventHandler = this.backing_CapstoneClosed;
				if (capstoneClosedEventHandler == null)
				{
					return;
				}
				capstoneClosedEventHandler();
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x060031C6 RID: 12742 RVA: 0x0017191F File Offset: 0x0016FB1F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NCapstoneContainer.SignalName.Changed || (ref signal) == NCapstoneContainer.SignalName.CapstoneClosed || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04001356 RID: 4950
		[Nullable(1)]
		private Control _backstop;

		// Token: 0x04001357 RID: 4951
		[Nullable(2)]
		private Tween _backstopFade;

		// Token: 0x04001358 RID: 4952
		private NCapstoneContainer.ChangedEventHandler backing_Changed;

		// Token: 0x04001359 RID: 4953
		private NCapstoneContainer.CapstoneClosedEventHandler backing_CapstoneClosed;

		// Token: 0x0200164C RID: 5708
		// (Invoke) Token: 0x0600A479 RID: 42105
		[Signal]
		public delegate void ChangedEventHandler();

		// Token: 0x0200164D RID: 5709
		// (Invoke) Token: 0x0600A47D RID: 42109
		[Signal]
		public delegate void CapstoneClosedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200164E RID: 5710
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040056D4 RID: 22228
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040056D5 RID: 22229
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040056D6 RID: 22230
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Close' method.
			/// </summary>
			// Token: 0x040056D7 RID: 22231
			public static readonly StringName Close = "Close";

			/// <summary>
			/// Cached name for the 'CloseInternal' method.
			/// </summary>
			// Token: 0x040056D8 RID: 22232
			public static readonly StringName CloseInternal = "CloseInternal";

			/// <summary>
			/// Cached name for the 'DisableBackstopInstantly' method.
			/// </summary>
			// Token: 0x040056D9 RID: 22233
			public static readonly StringName DisableBackstopInstantly = "DisableBackstopInstantly";

			/// <summary>
			/// Cached name for the 'EnableBackstopInstantly' method.
			/// </summary>
			// Token: 0x040056DA RID: 22234
			public static readonly StringName EnableBackstopInstantly = "EnableBackstopInstantly";

			/// <summary>
			/// Cached name for the 'CleanUp' method.
			/// </summary>
			// Token: 0x040056DB RID: 22235
			public static readonly StringName CleanUp = "CleanUp";

			/// <summary>
			/// Cached name for the 'OnActiveScreenChanged' method.
			/// </summary>
			// Token: 0x040056DC RID: 22236
			public static readonly StringName OnActiveScreenChanged = "OnActiveScreenChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200164F RID: 5711
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InUse' property.
			/// </summary>
			// Token: 0x040056DD RID: 22237
			public static readonly StringName InUse = "InUse";

			/// <summary>
			/// Cached name for the '_backstop' field.
			/// </summary>
			// Token: 0x040056DE RID: 22238
			public static readonly StringName _backstop = "_backstop";

			/// <summary>
			/// Cached name for the '_backstopFade' field.
			/// </summary>
			// Token: 0x040056DF RID: 22239
			public static readonly StringName _backstopFade = "_backstopFade";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001650 RID: 5712
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'Changed' signal.
			/// </summary>
			// Token: 0x040056E0 RID: 22240
			public static readonly StringName Changed = "Changed";

			/// <summary>
			/// Cached name for the 'CapstoneClosed' signal.
			/// </summary>
			// Token: 0x040056E1 RID: 22241
			public static readonly StringName CapstoneClosed = "CapstoneClosed";
		}
	}
}
