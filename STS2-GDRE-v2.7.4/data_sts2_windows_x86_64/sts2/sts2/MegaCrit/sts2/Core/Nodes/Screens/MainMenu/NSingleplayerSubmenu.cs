using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x02000312 RID: 786
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NSingleplayerSubmenu.cs")]
	public class NSingleplayerSubmenu : NSubmenu
	{
		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002B88 RID: 11144 RVA: 0x00133D50 File Offset: 0x00131F50
		protected override Control InitialFocusedControl
		{
			get
			{
				return this._standardButton;
			}
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x00133D58 File Offset: 0x00131F58
		[NullableContext(2)]
		public static NSingleplayerSubmenu Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NSingleplayerSubmenu._scenePath).Instantiate<NSingleplayerSubmenu>(0L);
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x00133D7C File Offset: 0x00131F7C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._standardButton = base.GetNode<NSubmenuButton>("StandardButton");
			this._standardButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenCharacterSelect)), 0U);
			this._standardButton.SetIconAndLocalization("STANDARD");
			this._dailyButton = base.GetNode<NSubmenuButton>("DailyButton");
			this._dailyButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenDailyScreen)), 0U);
			this._dailyButton.SetIconAndLocalization("DAILY");
			this._customButton = base.GetNode<NSubmenuButton>("CustomRunButton");
			this._customButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenCustomScreen)), 0U);
			this._customButton.SetIconAndLocalization("CUSTOM");
		}

		/// <summary>
		/// Checks the unlock state of Custom and Daily to determine whether they should be locked or not.
		/// Is not checked only at _Ready() because the MainMenu isn't recreated upon returning from the Timeline.
		/// </summary>
		// Token: 0x06002B8B RID: 11147 RVA: 0x00133E6C File Offset: 0x0013206C
		private void RefreshButtons()
		{
			this._dailyButton.SetEnabled(SaveManager.Instance.IsEpochRevealed<DailyRunEpoch>());
			this._customButton.SetEnabled(SaveManager.Instance.IsEpochRevealed<CustomAndSeedsEpoch>());
			this._dailyButton.RefreshLabels();
			this._customButton.RefreshLabels();
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x00133EB9 File Offset: 0x001320B9
		public override void OnSubmenuOpened()
		{
			this.RefreshButtons();
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x00133EC4 File Offset: 0x001320C4
		private void OpenCharacterSelect(NButton _)
		{
			NCharacterSelectScreen submenuType = this._stack.GetSubmenuType<NCharacterSelectScreen>();
			submenuType.InitializeSingleplayer();
			this._stack.Push(submenuType);
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x00133EEF File Offset: 0x001320EF
		private void OpenDailyScreen(NButton _)
		{
			this.OpenDailyScreen();
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x00133EF8 File Offset: 0x001320F8
		private void OpenDailyScreen()
		{
			NDailyRunScreen submenuType = this._stack.GetSubmenuType<NDailyRunScreen>();
			submenuType.InitializeSingleplayer();
			this._stack.Push(submenuType);
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x00133F24 File Offset: 0x00132124
		private void OpenCustomScreen(NButton _)
		{
			NCustomRunScreen submenuType = this._stack.GetSubmenuType<NCustomRunScreen>();
			submenuType.InitializeSingleplayer();
			this._stack.Push(submenuType);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B91 RID: 11153 RVA: 0x00133F50 File Offset: 0x00132150
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NSingleplayerSubmenu.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NSingleplayerSubmenu.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSingleplayerSubmenu.MethodName.RefreshButtons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSingleplayerSubmenu.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSingleplayerSubmenu.MethodName.OpenCharacterSelect, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSingleplayerSubmenu.MethodName.OpenDailyScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSingleplayerSubmenu.MethodName.OpenDailyScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSingleplayerSubmenu.MethodName.OpenCustomScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B92 RID: 11154 RVA: 0x0013417C File Offset: 0x0013237C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSingleplayerSubmenu.MethodName.Create && args.Count == 0)
			{
				NSingleplayerSubmenu nsingleplayerSubmenu = NSingleplayerSubmenu.Create();
				ret = VariantUtils.CreateFrom<NSingleplayerSubmenu>(ref nsingleplayerSubmenu);
				return true;
			}
			if ((ref method) == NSingleplayerSubmenu.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSingleplayerSubmenu.MethodName.RefreshButtons && args.Count == 0)
			{
				this.RefreshButtons();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSingleplayerSubmenu.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSingleplayerSubmenu.MethodName.OpenCharacterSelect && args.Count == 1)
			{
				this.OpenCharacterSelect(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSingleplayerSubmenu.MethodName.OpenDailyScreen && args.Count == 1)
			{
				this.OpenDailyScreen(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSingleplayerSubmenu.MethodName.OpenDailyScreen && args.Count == 0)
			{
				this.OpenDailyScreen();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSingleplayerSubmenu.MethodName.OpenCustomScreen && args.Count == 1)
			{
				this.OpenCustomScreen(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x001342EC File Offset: 0x001324EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSingleplayerSubmenu.MethodName.Create && args.Count == 0)
			{
				NSingleplayerSubmenu nsingleplayerSubmenu = NSingleplayerSubmenu.Create();
				ret = VariantUtils.CreateFrom<NSingleplayerSubmenu>(ref nsingleplayerSubmenu);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002B94 RID: 11156 RVA: 0x0013432C File Offset: 0x0013252C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSingleplayerSubmenu.MethodName.Create || (ref method) == NSingleplayerSubmenu.MethodName._Ready || (ref method) == NSingleplayerSubmenu.MethodName.RefreshButtons || (ref method) == NSingleplayerSubmenu.MethodName.OnSubmenuOpened || (ref method) == NSingleplayerSubmenu.MethodName.OpenCharacterSelect || (ref method) == NSingleplayerSubmenu.MethodName.OpenDailyScreen || (ref method) == NSingleplayerSubmenu.MethodName.OpenCustomScreen || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002B95 RID: 11157 RVA: 0x001343AC File Offset: 0x001325AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSingleplayerSubmenu.PropertyName._standardButton)
			{
				this._standardButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NSingleplayerSubmenu.PropertyName._dailyButton)
			{
				this._dailyButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NSingleplayerSubmenu.PropertyName._customButton)
			{
				this._customButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002B96 RID: 11158 RVA: 0x00134414 File Offset: 0x00132614
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSingleplayerSubmenu.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NSingleplayerSubmenu.PropertyName._standardButton)
			{
				value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._standardButton);
				return true;
			}
			if ((ref name) == NSingleplayerSubmenu.PropertyName._dailyButton)
			{
				value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._dailyButton);
				return true;
			}
			if ((ref name) == NSingleplayerSubmenu.PropertyName._customButton)
			{
				value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._customButton);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B97 RID: 11159 RVA: 0x001344AC File Offset: 0x001326AC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSingleplayerSubmenu.PropertyName._standardButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSingleplayerSubmenu.PropertyName._dailyButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSingleplayerSubmenu.PropertyName._customButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSingleplayerSubmenu.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B98 RID: 11160 RVA: 0x00134544 File Offset: 0x00132744
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSingleplayerSubmenu.PropertyName._standardButton, Variant.From<NSubmenuButton>(ref this._standardButton));
			info.AddProperty(NSingleplayerSubmenu.PropertyName._dailyButton, Variant.From<NSubmenuButton>(ref this._dailyButton));
			info.AddProperty(NSingleplayerSubmenu.PropertyName._customButton, Variant.From<NSubmenuButton>(ref this._customButton));
		}

		/// <inheritdoc />
		// Token: 0x06002B99 RID: 11161 RVA: 0x0013459C File Offset: 0x0013279C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSingleplayerSubmenu.PropertyName._standardButton, ref variant))
			{
				this._standardButton = variant.As<NSubmenuButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSingleplayerSubmenu.PropertyName._dailyButton, ref variant2))
			{
				this._dailyButton = variant2.As<NSubmenuButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSingleplayerSubmenu.PropertyName._customButton, ref variant3))
			{
				this._customButton = variant3.As<NSubmenuButton>();
			}
		}

		// Token: 0x04001060 RID: 4192
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/singleplayer_submenu");

		// Token: 0x04001061 RID: 4193
		private NSubmenuButton _standardButton;

		// Token: 0x04001062 RID: 4194
		private NSubmenuButton _dailyButton;

		// Token: 0x04001063 RID: 4195
		private NSubmenuButton _customButton;

		// Token: 0x04001064 RID: 4196
		private const string _keyStandard = "STANDARD";

		// Token: 0x04001065 RID: 4197
		private const string _keyDaily = "DAILY";

		// Token: 0x04001066 RID: 4198
		private const string _keyCustom = "CUSTOM";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001530 RID: 5424
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005101 RID: 20737
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005102 RID: 20738
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshButtons' method.
			/// </summary>
			// Token: 0x04005103 RID: 20739
			public static readonly StringName RefreshButtons = "RefreshButtons";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04005104 RID: 20740
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OpenCharacterSelect' method.
			/// </summary>
			// Token: 0x04005105 RID: 20741
			public static readonly StringName OpenCharacterSelect = "OpenCharacterSelect";

			/// <summary>
			/// Cached name for the 'OpenDailyScreen' method.
			/// </summary>
			// Token: 0x04005106 RID: 20742
			public static readonly StringName OpenDailyScreen = "OpenDailyScreen";

			/// <summary>
			/// Cached name for the 'OpenCustomScreen' method.
			/// </summary>
			// Token: 0x04005107 RID: 20743
			public static readonly StringName OpenCustomScreen = "OpenCustomScreen";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001531 RID: 5425
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04005108 RID: 20744
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_standardButton' field.
			/// </summary>
			// Token: 0x04005109 RID: 20745
			public static readonly StringName _standardButton = "_standardButton";

			/// <summary>
			/// Cached name for the '_dailyButton' field.
			/// </summary>
			// Token: 0x0400510A RID: 20746
			public static readonly StringName _dailyButton = "_dailyButton";

			/// <summary>
			/// Cached name for the '_customButton' field.
			/// </summary>
			// Token: 0x0400510B RID: 20747
			public static readonly StringName _customButton = "_customButton";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001532 RID: 5426
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
