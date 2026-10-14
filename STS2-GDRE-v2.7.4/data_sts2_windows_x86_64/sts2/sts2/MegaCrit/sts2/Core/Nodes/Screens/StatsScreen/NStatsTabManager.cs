using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen
{
	// Token: 0x02000278 RID: 632
	[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NStatsTabManager.cs")]
	public class NStatsTabManager : Control
	{
		// Token: 0x06002175 RID: 8565 RVA: 0x000E3BCC File Offset: 0x000E1DCC
		public override void _Ready()
		{
			this._leftTriggerIcon = base.GetNode<Control>("LeftTriggerIcon");
			this._rightTriggerIcon = base.GetNode<Control>("RightTriggerIcon");
			this._tabContainer = base.GetNode<Control>("TabContainer");
			this._tabs = this._tabContainer.GetChildren(false).OfType<NSettingsTab>().ToList<NSettingsTab>();
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			using (List<NSettingsTab>.Enumerator enumerator = this._tabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NSettingsTab nSettingsTab = enumerator.Current;
					nSettingsTab.Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(delegate(NClickableControl _)
					{
						this.SwitchToTab(nSettingsTab);
					}), 0U);
				}
			}
			this.UpdateControllerButton();
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x000E3D18 File Offset: 0x000E1F18
		public void ResetTabs()
		{
			this.SwitchToTab(this._tabContainer.GetChild<NSettingsTab>(0, false));
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000E3D30 File Offset: 0x000E1F30
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (NDevConsole.IsConsoleVisible)
			{
				return;
			}
			Control control = base.GetViewport().GuiGetFocusOwner();
			bool flag = control is TextEdit || control is LineEdit;
			if (flag)
			{
				return;
			}
			if (inputEvent.IsActionPressed(NStatsTabManager._tabLeftHotkey, false, false))
			{
				int num = this._tabs.IndexOf(this._currentTab) - 1;
				if (num >= 0)
				{
					this._tabs[num].ForceTabPressed();
					this.SwitchToTab(this._tabs[num]);
				}
			}
			if (inputEvent.IsActionPressed(NStatsTabManager._tabRightHotkey, false, false))
			{
				int num2 = Math.Min(this._tabs.Count - 1, this._tabs.IndexOf(this._currentTab) + 1);
				if (num2 < this._tabs.Count)
				{
					this._tabs[num2].ForceTabPressed();
				}
			}
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x000E3E14 File Offset: 0x000E2014
		[NullableContext(1)]
		private void SwitchToTab(NSettingsTab tab)
		{
			this._currentTab = tab;
			foreach (NSettingsTab nsettingsTab in this._tabs)
			{
				if (nsettingsTab != this._currentTab)
				{
					nsettingsTab.Deselect();
				}
				else
				{
					nsettingsTab.Select();
				}
			}
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000E3E80 File Offset: 0x000E2080
		private void UpdateControllerButton()
		{
			this._leftTriggerIcon.Visible = NControllerManager.Instance.IsUsingController;
			this._rightTriggerIcon.Visible = NControllerManager.Instance.IsUsingController;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600217A RID: 8570 RVA: 0x000E3EAC File Offset: 0x000E20AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NStatsTabManager.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStatsTabManager.MethodName.ResetTabs, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStatsTabManager.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NStatsTabManager.MethodName.SwitchToTab, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tab", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NStatsTabManager.MethodName.UpdateControllerButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600217B RID: 8571 RVA: 0x000E400C File Offset: 0x000E220C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NStatsTabManager.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStatsTabManager.MethodName.ResetTabs && args.Count == 0)
			{
				this.ResetTabs();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStatsTabManager.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStatsTabManager.MethodName.SwitchToTab && args.Count == 1)
			{
				this.SwitchToTab(VariantUtils.ConvertTo<NSettingsTab>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStatsTabManager.MethodName.UpdateControllerButton && args.Count == 0)
			{
				this.UpdateControllerButton();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600217C RID: 8572 RVA: 0x000E40F8 File Offset: 0x000E22F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NStatsTabManager.MethodName._Ready || (ref method) == NStatsTabManager.MethodName.ResetTabs || (ref method) == NStatsTabManager.MethodName._Input || (ref method) == NStatsTabManager.MethodName.SwitchToTab || (ref method) == NStatsTabManager.MethodName.UpdateControllerButton || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600217D RID: 8573 RVA: 0x000E4158 File Offset: 0x000E2358
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NStatsTabManager.PropertyName._leftTriggerIcon)
			{
				this._leftTriggerIcon = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NStatsTabManager.PropertyName._rightTriggerIcon)
			{
				this._rightTriggerIcon = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NStatsTabManager.PropertyName._tabContainer)
			{
				this._tabContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NStatsTabManager.PropertyName._currentTab)
			{
				this._currentTab = VariantUtils.ConvertTo<NSettingsTab>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600217E RID: 8574 RVA: 0x000E41DC File Offset: 0x000E23DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NStatsTabManager.PropertyName._leftTriggerIcon)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._leftTriggerIcon);
				return true;
			}
			if ((ref name) == NStatsTabManager.PropertyName._rightTriggerIcon)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rightTriggerIcon);
				return true;
			}
			if ((ref name) == NStatsTabManager.PropertyName._tabContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._tabContainer);
				return true;
			}
			if ((ref name) == NStatsTabManager.PropertyName._currentTab)
			{
				value = VariantUtils.CreateFrom<NSettingsTab>(ref this._currentTab);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600217F RID: 8575 RVA: 0x000E4274 File Offset: 0x000E2474
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NStatsTabManager.PropertyName._leftTriggerIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatsTabManager.PropertyName._rightTriggerIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatsTabManager.PropertyName._tabContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatsTabManager.PropertyName._currentTab, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002180 RID: 8576 RVA: 0x000E430C File Offset: 0x000E250C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NStatsTabManager.PropertyName._leftTriggerIcon, Variant.From<Control>(ref this._leftTriggerIcon));
			info.AddProperty(NStatsTabManager.PropertyName._rightTriggerIcon, Variant.From<Control>(ref this._rightTriggerIcon));
			info.AddProperty(NStatsTabManager.PropertyName._tabContainer, Variant.From<Control>(ref this._tabContainer));
			info.AddProperty(NStatsTabManager.PropertyName._currentTab, Variant.From<NSettingsTab>(ref this._currentTab));
		}

		/// <inheritdoc />
		// Token: 0x06002181 RID: 8577 RVA: 0x000E4378 File Offset: 0x000E2578
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NStatsTabManager.PropertyName._leftTriggerIcon, ref variant))
			{
				this._leftTriggerIcon = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NStatsTabManager.PropertyName._rightTriggerIcon, ref variant2))
			{
				this._rightTriggerIcon = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NStatsTabManager.PropertyName._tabContainer, ref variant3))
			{
				this._tabContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NStatsTabManager.PropertyName._currentTab, ref variant4))
			{
				this._currentTab = variant4.As<NSettingsTab>();
			}
		}

		// Token: 0x04000D15 RID: 3349
		[Nullable(1)]
		private static readonly StringName _tabLeftHotkey = MegaInput.viewDeckAndTabLeft;

		// Token: 0x04000D16 RID: 3350
		[Nullable(1)]
		private static readonly StringName _tabRightHotkey = MegaInput.viewExhaustPileAndTabRight;

		// Token: 0x04000D17 RID: 3351
		[Nullable(1)]
		private Control _leftTriggerIcon;

		// Token: 0x04000D18 RID: 3352
		[Nullable(1)]
		private Control _rightTriggerIcon;

		// Token: 0x04000D19 RID: 3353
		[Nullable(1)]
		private Control _tabContainer;

		// Token: 0x04000D1A RID: 3354
		[Nullable(1)]
		private List<NSettingsTab> _tabs;

		// Token: 0x04000D1B RID: 3355
		[Nullable(2)]
		private NSettingsTab _currentTab;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200130D RID: 4877
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004A19 RID: 18969
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ResetTabs' method.
			/// </summary>
			// Token: 0x04004A1A RID: 18970
			public static readonly StringName ResetTabs = "ResetTabs";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04004A1B RID: 18971
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'SwitchToTab' method.
			/// </summary>
			// Token: 0x04004A1C RID: 18972
			public static readonly StringName SwitchToTab = "SwitchToTab";

			/// <summary>
			/// Cached name for the 'UpdateControllerButton' method.
			/// </summary>
			// Token: 0x04004A1D RID: 18973
			public static readonly StringName UpdateControllerButton = "UpdateControllerButton";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200130E RID: 4878
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_leftTriggerIcon' field.
			/// </summary>
			// Token: 0x04004A1E RID: 18974
			public static readonly StringName _leftTriggerIcon = "_leftTriggerIcon";

			/// <summary>
			/// Cached name for the '_rightTriggerIcon' field.
			/// </summary>
			// Token: 0x04004A1F RID: 18975
			public static readonly StringName _rightTriggerIcon = "_rightTriggerIcon";

			/// <summary>
			/// Cached name for the '_tabContainer' field.
			/// </summary>
			// Token: 0x04004A20 RID: 18976
			public static readonly StringName _tabContainer = "_tabContainer";

			/// <summary>
			/// Cached name for the '_currentTab' field.
			/// </summary>
			// Token: 0x04004A21 RID: 18977
			public static readonly StringName _currentTab = "_currentTab";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200130F RID: 4879
		public class SignalName : Control.SignalName
		{
		}
	}
}
