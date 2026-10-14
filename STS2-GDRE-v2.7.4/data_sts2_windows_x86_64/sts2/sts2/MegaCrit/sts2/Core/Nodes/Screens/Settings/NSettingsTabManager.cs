using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002BC RID: 700
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsTabManager.cs")]
	public class NSettingsTabManager : Control
	{
		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002520 RID: 9504 RVA: 0x000FBE51 File Offset: 0x000FA051
		[Nullable(1)]
		private NSettingsPanel CurrentlyDisplayedPanel
		{
			[NullableContext(1)]
			get
			{
				return this._tabs[this._currentTab];
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x000FBE64 File Offset: 0x000FA064
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._currentTab == null)
				{
					return null;
				}
				return this._tabs[this._currentTab].DefaultFocusedControl;
			}
		}

		/// <summary>
		/// Hooks up the Tab button callbacks.
		/// </summary>
		// Token: 0x06002522 RID: 9506 RVA: 0x000FBE88 File Offset: 0x000FA088
		public override void _Ready()
		{
			this._leftTriggerIcon = base.GetNode<TextureRect>("LeftTriggerIcon");
			this._rightTriggerIcon = base.GetNode<TextureRect>("RightTriggerIcon");
			this._scrollContainer = base.GetNode<NScrollableContainer>("%ScrollContainer");
			this._scrollContainer.DisableScrollingIfContentFits();
			NSettingsTab nsettingsTab = base.GetNode<NSettingsTab>("General");
			nsettingsTab.SetLabel(new LocString("settings_ui", "TAB_GENERAL").GetFormattedText());
			this._tabs.Add(nsettingsTab, base.GetNode<NSettingsPanel>("%GeneralSettings"));
			nsettingsTab = base.GetNode<NSettingsTab>("Graphics");
			nsettingsTab.SetLabel(new LocString("settings_ui", "TAB_GRAPHICS").GetFormattedText());
			this._tabs.Add(nsettingsTab, base.GetNode<NSettingsPanel>("%GraphicsSettings"));
			nsettingsTab = base.GetNode<NSettingsTab>("Sound");
			nsettingsTab.SetLabel(new LocString("settings_ui", "TAB_SOUND").GetFormattedText());
			this._tabs.Add(nsettingsTab, base.GetNode<NSettingsPanel>("%SoundSettings"));
			nsettingsTab = base.GetNode<NSettingsTab>("Input");
			nsettingsTab.SetLabel(new LocString("settings_ui", "TAB_INPUT").GetFormattedText());
			this._tabs.Add(nsettingsTab, base.GetNode<NSettingsPanel>("%InputSettings"));
			using (Dictionary<NSettingsTab, NSettingsPanel>.KeyCollection.Enumerator enumerator = this._tabs.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NSettingsTab tab = enumerator.Current;
					tab.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
					{
						this.SwitchTabTo(tab);
					}), 0U);
				}
			}
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			this.UpdateControllerButton();
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x000FC0E4 File Offset: 0x000FA2E4
		public void ResetTabs()
		{
			this._tabs.First<KeyValuePair<NSettingsTab, NSettingsPanel>>().Key.Select();
			this.SwitchTabTo(this._tabs.First<KeyValuePair<NSettingsTab, NSettingsPanel>>().Key);
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x000FC124 File Offset: 0x000FA324
		public void Enable()
		{
			NHotkeyManager.Instance.PushHotkeyPressedBinding(NSettingsTabManager._tabLeftHotkey, new Action(this.TabLeft));
			NHotkeyManager.Instance.PushHotkeyPressedBinding(NSettingsTabManager._tabRightHotkey, new Action(this.TabRight));
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x000FC174 File Offset: 0x000FA374
		public void Disable()
		{
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(NSettingsTabManager._tabLeftHotkey, new Action(this.TabLeft));
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(NSettingsTabManager._tabRightHotkey, new Action(this.TabRight));
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x000FC1C4 File Offset: 0x000FA3C4
		private void TabLeft()
		{
			List<NSettingsTab> list = this._tabs.Keys.ToList<NSettingsTab>();
			int num = list.IndexOf(this._currentTab) - 1;
			if (num >= 0)
			{
				this.SwitchTabTo(list[num]);
			}
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x000FC204 File Offset: 0x000FA404
		private void TabRight()
		{
			List<NSettingsTab> list = this._tabs.Keys.ToList<NSettingsTab>();
			int num = Math.Min(list.Count - 1, list.IndexOf(this._currentTab) + 1);
			if (num < list.Count)
			{
				this.SwitchTabTo(list[num]);
			}
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000FC254 File Offset: 0x000FA454
		[NullableContext(1)]
		private void SwitchTabTo(NSettingsTab selectedTab)
		{
			if (selectedTab != this._currentTab)
			{
				foreach (NSettingsTab nsettingsTab in this._tabs.Keys)
				{
					nsettingsTab.Deselect();
					this._tabs[nsettingsTab].Visible = false;
				}
				selectedTab.Select();
				this._tabs[selectedTab].Visible = true;
				this._currentTab = selectedTab;
				this._scrollContainer.SetContent(this.CurrentlyDisplayedPanel, 20f, 30f);
				this._scrollContainer.InstantlyScrollToTop();
				Tween scrollbarTween = this._scrollbarTween;
				if (scrollbarTween != null)
				{
					scrollbarTween.Kill();
				}
				this._scrollbarTween = base.CreateTween().SetParallel(true);
				this._scrollbarTween.TweenProperty(this._scrollContainer.Scrollbar, "modulate", Colors.White, 0.5).From(StsColors.transparentBlack).SetEase(1L)
					.SetTrans(7L);
			}
			ActiveScreenContext.Instance.Update();
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000FC38C File Offset: 0x000FA58C
		private void UpdateControllerButton()
		{
			this._leftTriggerIcon.Visible = NControllerManager.Instance.IsUsingController;
			this._rightTriggerIcon.Visible = NControllerManager.Instance.IsUsingController;
			this._leftTriggerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(MegaInput.viewDeckAndTabLeft);
			this._rightTriggerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(MegaInput.viewExhaustPileAndTabRight);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600252A RID: 9514 RVA: 0x000FC404 File Offset: 0x000FA604
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NSettingsTabManager.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTabManager.MethodName.ResetTabs, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTabManager.MethodName.Enable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTabManager.MethodName.Disable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTabManager.MethodName.TabLeft, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTabManager.MethodName.TabRight, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTabManager.MethodName.SwitchTabTo, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "selectedTab", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSettingsTabManager.MethodName.UpdateControllerButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600252B RID: 9515 RVA: 0x000FC5C4 File Offset: 0x000FA7C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSettingsTabManager.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTabManager.MethodName.ResetTabs && args.Count == 0)
			{
				this.ResetTabs();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTabManager.MethodName.Enable && args.Count == 0)
			{
				this.Enable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTabManager.MethodName.Disable && args.Count == 0)
			{
				this.Disable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTabManager.MethodName.TabLeft && args.Count == 0)
			{
				this.TabLeft();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTabManager.MethodName.TabRight && args.Count == 0)
			{
				this.TabRight();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTabManager.MethodName.SwitchTabTo && args.Count == 1)
			{
				this.SwitchTabTo(VariantUtils.ConvertTo<NSettingsTab>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTabManager.MethodName.UpdateControllerButton && args.Count == 0)
			{
				this.UpdateControllerButton();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600252C RID: 9516 RVA: 0x000FC710 File Offset: 0x000FA910
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSettingsTabManager.MethodName._Ready || (ref method) == NSettingsTabManager.MethodName.ResetTabs || (ref method) == NSettingsTabManager.MethodName.Enable || (ref method) == NSettingsTabManager.MethodName.Disable || (ref method) == NSettingsTabManager.MethodName.TabLeft || (ref method) == NSettingsTabManager.MethodName.TabRight || (ref method) == NSettingsTabManager.MethodName.SwitchTabTo || (ref method) == NSettingsTabManager.MethodName.UpdateControllerButton || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600252D RID: 9517 RVA: 0x000FC79C File Offset: 0x000FA99C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSettingsTabManager.PropertyName._currentTab)
			{
				this._currentTab = VariantUtils.ConvertTo<NSettingsTab>(ref value);
				return true;
			}
			if ((ref name) == NSettingsTabManager.PropertyName._scrollContainer)
			{
				this._scrollContainer = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
				return true;
			}
			if ((ref name) == NSettingsTabManager.PropertyName._leftTriggerIcon)
			{
				this._leftTriggerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NSettingsTabManager.PropertyName._rightTriggerIcon)
			{
				this._rightTriggerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NSettingsTabManager.PropertyName._scrollbarTween)
			{
				this._scrollbarTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600252E RID: 9518 RVA: 0x000FC838 File Offset: 0x000FAA38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSettingsTabManager.PropertyName.CurrentlyDisplayedPanel)
			{
				NSettingsPanel currentlyDisplayedPanel = this.CurrentlyDisplayedPanel;
				value = VariantUtils.CreateFrom<NSettingsPanel>(ref currentlyDisplayedPanel);
				return true;
			}
			if ((ref name) == NSettingsTabManager.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NSettingsTabManager.PropertyName._currentTab)
			{
				value = VariantUtils.CreateFrom<NSettingsTab>(ref this._currentTab);
				return true;
			}
			if ((ref name) == NSettingsTabManager.PropertyName._scrollContainer)
			{
				value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._scrollContainer);
				return true;
			}
			if ((ref name) == NSettingsTabManager.PropertyName._leftTriggerIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._leftTriggerIcon);
				return true;
			}
			if ((ref name) == NSettingsTabManager.PropertyName._rightTriggerIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._rightTriggerIcon);
				return true;
			}
			if ((ref name) == NSettingsTabManager.PropertyName._scrollbarTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._scrollbarTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600252F RID: 9519 RVA: 0x000FC934 File Offset: 0x000FAB34
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSettingsTabManager.PropertyName._currentTab, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTabManager.PropertyName._scrollContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTabManager.PropertyName.CurrentlyDisplayedPanel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTabManager.PropertyName._leftTriggerIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTabManager.PropertyName._rightTriggerIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTabManager.PropertyName._scrollbarTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTabManager.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002530 RID: 9520 RVA: 0x000FCA30 File Offset: 0x000FAC30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSettingsTabManager.PropertyName._currentTab, Variant.From<NSettingsTab>(ref this._currentTab));
			info.AddProperty(NSettingsTabManager.PropertyName._scrollContainer, Variant.From<NScrollableContainer>(ref this._scrollContainer));
			info.AddProperty(NSettingsTabManager.PropertyName._leftTriggerIcon, Variant.From<TextureRect>(ref this._leftTriggerIcon));
			info.AddProperty(NSettingsTabManager.PropertyName._rightTriggerIcon, Variant.From<TextureRect>(ref this._rightTriggerIcon));
			info.AddProperty(NSettingsTabManager.PropertyName._scrollbarTween, Variant.From<Tween>(ref this._scrollbarTween));
			info.AddSignalEventDelegate(NSettingsTabManager.SignalName.TabChanged, this.backing_TabChanged);
		}

		/// <inheritdoc />
		// Token: 0x06002531 RID: 9521 RVA: 0x000FCAC4 File Offset: 0x000FACC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSettingsTabManager.PropertyName._currentTab, ref variant))
			{
				this._currentTab = variant.As<NSettingsTab>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSettingsTabManager.PropertyName._scrollContainer, ref variant2))
			{
				this._scrollContainer = variant2.As<NScrollableContainer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSettingsTabManager.PropertyName._leftTriggerIcon, ref variant3))
			{
				this._leftTriggerIcon = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSettingsTabManager.PropertyName._rightTriggerIcon, ref variant4))
			{
				this._rightTriggerIcon = variant4.As<TextureRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSettingsTabManager.PropertyName._scrollbarTween, ref variant5))
			{
				this._scrollbarTween = variant5.As<Tween>();
			}
			NSettingsTabManager.TabChangedEventHandler tabChangedEventHandler;
			if (info.TryGetSignalEventDelegate<NSettingsTabManager.TabChangedEventHandler>(NSettingsTabManager.SignalName.TabChanged, ref tabChangedEventHandler))
			{
				this.backing_TabChanged = tabChangedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002532 RID: 9522 RVA: 0x000FCB7C File Offset: 0x000FAD7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NSettingsTabManager.SignalName.TabChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Settings.NSettingsTabManager.TabChangedEventHandler" />
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06002533 RID: 9523 RVA: 0x000FCBC0 File Offset: 0x000FADC0
		// (remove) Token: 0x06002534 RID: 9524 RVA: 0x000FCBD9 File Offset: 0x000FADD9
		public event NSettingsTabManager.TabChangedEventHandler TabChanged
		{
			add
			{
				this.backing_TabChanged = (NSettingsTabManager.TabChangedEventHandler)Delegate.Combine(this.backing_TabChanged, value);
			}
			remove
			{
				this.backing_TabChanged = (NSettingsTabManager.TabChangedEventHandler)Delegate.Remove(this.backing_TabChanged, value);
			}
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000FCBF2 File Offset: 0x000FADF2
		protected void EmitSignalTabChanged()
		{
			base.EmitSignal(NSettingsTabManager.SignalName.TabChanged, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06002536 RID: 9526 RVA: 0x000FCC05 File Offset: 0x000FAE05
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NSettingsTabManager.SignalName.TabChanged) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NSettingsTabManager.TabChangedEventHandler tabChangedEventHandler = this.backing_TabChanged;
			if (tabChangedEventHandler == null)
			{
				return;
			}
			tabChangedEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x06002537 RID: 9527 RVA: 0x000FCC36 File Offset: 0x000FAE36
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NSettingsTabManager.SignalName.TabChanged || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000DDF RID: 3551
		private const float _scrollPaddingTop = 20f;

		// Token: 0x04000DE0 RID: 3552
		private const float _scrollPaddingBottom = 30f;

		// Token: 0x04000DE1 RID: 3553
		[Nullable(1)]
		private static readonly StringName _tabLeftHotkey = MegaInput.viewDeckAndTabLeft;

		// Token: 0x04000DE2 RID: 3554
		[Nullable(1)]
		private static readonly StringName _tabRightHotkey = MegaInput.viewExhaustPileAndTabRight;

		// Token: 0x04000DE3 RID: 3555
		[Nullable(2)]
		private NSettingsTab _currentTab;

		// Token: 0x04000DE4 RID: 3556
		[Nullable(1)]
		private NScrollableContainer _scrollContainer;

		// Token: 0x04000DE5 RID: 3557
		[Nullable(1)]
		private readonly Dictionary<NSettingsTab, NSettingsPanel> _tabs = new Dictionary<NSettingsTab, NSettingsPanel>();

		// Token: 0x04000DE6 RID: 3558
		[Nullable(1)]
		private TextureRect _leftTriggerIcon;

		// Token: 0x04000DE7 RID: 3559
		[Nullable(1)]
		private TextureRect _rightTriggerIcon;

		// Token: 0x04000DE8 RID: 3560
		[Nullable(2)]
		private Tween _scrollbarTween;

		// Token: 0x04000DE9 RID: 3561
		private NSettingsTabManager.TabChangedEventHandler backing_TabChanged;

		// Token: 0x020013EF RID: 5103
		// (Invoke) Token: 0x06009FCD RID: 40909
		[Signal]
		public delegate void TabChangedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013F0 RID: 5104
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C1F RID: 19487
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ResetTabs' method.
			/// </summary>
			// Token: 0x04004C20 RID: 19488
			public static readonly StringName ResetTabs = "ResetTabs";

			/// <summary>
			/// Cached name for the 'Enable' method.
			/// </summary>
			// Token: 0x04004C21 RID: 19489
			public static readonly StringName Enable = "Enable";

			/// <summary>
			/// Cached name for the 'Disable' method.
			/// </summary>
			// Token: 0x04004C22 RID: 19490
			public static readonly StringName Disable = "Disable";

			/// <summary>
			/// Cached name for the 'TabLeft' method.
			/// </summary>
			// Token: 0x04004C23 RID: 19491
			public static readonly StringName TabLeft = "TabLeft";

			/// <summary>
			/// Cached name for the 'TabRight' method.
			/// </summary>
			// Token: 0x04004C24 RID: 19492
			public static readonly StringName TabRight = "TabRight";

			/// <summary>
			/// Cached name for the 'SwitchTabTo' method.
			/// </summary>
			// Token: 0x04004C25 RID: 19493
			public static readonly StringName SwitchTabTo = "SwitchTabTo";

			/// <summary>
			/// Cached name for the 'UpdateControllerButton' method.
			/// </summary>
			// Token: 0x04004C26 RID: 19494
			public static readonly StringName UpdateControllerButton = "UpdateControllerButton";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013F1 RID: 5105
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CurrentlyDisplayedPanel' property.
			/// </summary>
			// Token: 0x04004C27 RID: 19495
			public static readonly StringName CurrentlyDisplayedPanel = "CurrentlyDisplayedPanel";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004C28 RID: 19496
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_currentTab' field.
			/// </summary>
			// Token: 0x04004C29 RID: 19497
			public static readonly StringName _currentTab = "_currentTab";

			/// <summary>
			/// Cached name for the '_scrollContainer' field.
			/// </summary>
			// Token: 0x04004C2A RID: 19498
			public static readonly StringName _scrollContainer = "_scrollContainer";

			/// <summary>
			/// Cached name for the '_leftTriggerIcon' field.
			/// </summary>
			// Token: 0x04004C2B RID: 19499
			public static readonly StringName _leftTriggerIcon = "_leftTriggerIcon";

			/// <summary>
			/// Cached name for the '_rightTriggerIcon' field.
			/// </summary>
			// Token: 0x04004C2C RID: 19500
			public static readonly StringName _rightTriggerIcon = "_rightTriggerIcon";

			/// <summary>
			/// Cached name for the '_scrollbarTween' field.
			/// </summary>
			// Token: 0x04004C2D RID: 19501
			public static readonly StringName _scrollbarTween = "_scrollbarTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013F2 RID: 5106
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'TabChanged' signal.
			/// </summary>
			// Token: 0x04004C2E RID: 19502
			public static readonly StringName TabChanged = "TabChanged";
		}
	}
}
