using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x0200030C RID: 780
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMultiplayerHostSubmenu.cs")]
	public class NMultiplayerHostSubmenu : NSubmenu
	{
		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002B02 RID: 11010 RVA: 0x0012F125 File Offset: 0x0012D325
		protected override Control InitialFocusedControl
		{
			get
			{
				return this._standardButton;
			}
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x0012F12D File Offset: 0x0012D32D
		[NullableContext(2)]
		public static NMultiplayerHostSubmenu Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NMultiplayerHostSubmenu._scenePath).Instantiate<NMultiplayerHostSubmenu>(0L);
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x0012F150 File Offset: 0x0012D350
		public override void _Ready()
		{
			this.ConnectSignals();
			this._loadingOverlay = base.GetNode<Control>("%LoadingOverlay");
			this._standardButton = base.GetNode<NSubmenuButton>("StandardButton");
			this._standardButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnStandardPressed)), 0U);
			this._standardButton.SetIconAndLocalization("STANDARD_MP");
			this._dailyButton = base.GetNode<NSubmenuButton>("DailyButton");
			this._dailyButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnDailyPressed)), 0U);
			this._dailyButton.SetIconAndLocalization("DAILY_MP");
			this._customButton = base.GetNode<NSubmenuButton>("CustomRunButton");
			this._customButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCustomPressed)), 0U);
			this._customButton.SetIconAndLocalization("CUSTOM_MP");
		}

		/// <summary>
		/// Checks the unlock state of Custom and Daily to determine whether they should be locked or not.
		/// Is not checked only at _Ready() because the MainMenu isn't recreated upon returning from the Timeline.
		/// </summary>
		// Token: 0x06002B05 RID: 11013 RVA: 0x0012F254 File Offset: 0x0012D454
		private void RefreshButtons()
		{
			this._dailyButton.SetEnabled(SaveManager.Instance.IsEpochRevealed<DailyRunEpoch>());
			this._customButton.SetEnabled(SaveManager.Instance.IsEpochRevealed<CustomAndSeedsEpoch>());
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x0012F280 File Offset: 0x0012D480
		public override void OnSubmenuOpened()
		{
			this.RefreshButtons();
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x0012F288 File Offset: 0x0012D488
		private void OnStandardPressed(NButton _)
		{
			this.StartHost(GameMode.Standard);
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x0012F291 File Offset: 0x0012D491
		private void OnDailyPressed(NButton _)
		{
			this.StartHost(GameMode.Daily);
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x0012F29A File Offset: 0x0012D49A
		private void OnCustomPressed(NButton _)
		{
			this.StartHost(GameMode.Custom);
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x0012F2A3 File Offset: 0x0012D4A3
		public void StartHost(GameMode gameMode)
		{
			TaskHelper.RunSafely(NMultiplayerHostSubmenu.StartHostAsync(gameMode, this._loadingOverlay, this._stack));
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x0012F2C0 File Offset: 0x0012D4C0
		public static async Task StartHostAsync(GameMode gameMode, Control loadingOverlay, NSubmenuStack stack)
		{
			PlatformType platformType = ((SteamInitializer.Initialized && !CommandLineHelper.HasArg("fastmp")) ? PlatformType.Steam : PlatformType.None);
			loadingOverlay.Visible = true;
			try
			{
				NetHostGameService netService = new NetHostGameService();
				NetErrorInfo? netErrorInfo = null;
				if (platformType == PlatformType.Steam)
				{
					NetErrorInfo? netErrorInfo2 = await netService.StartSteamHost(4);
					netErrorInfo = netErrorInfo2;
				}
				else
				{
					netService.StartENetHost(33771, 4);
				}
				if (netErrorInfo == null)
				{
					if (gameMode == GameMode.Standard)
					{
						NCharacterSelectScreen submenuType = stack.GetSubmenuType<NCharacterSelectScreen>();
						submenuType.InitializeMultiplayerAsHost(netService, 4);
						stack.Push(submenuType);
					}
					else if (gameMode == GameMode.Daily)
					{
						NDailyRunScreen submenuType2 = stack.GetSubmenuType<NDailyRunScreen>();
						submenuType2.InitializeMultiplayerAsHost(netService);
						stack.Push(submenuType2);
					}
					else
					{
						NCustomRunScreen submenuType3 = stack.GetSubmenuType<NCustomRunScreen>();
						submenuType3.InitializeMultiplayerAsHost(netService, 4);
						stack.Push(submenuType3);
					}
				}
				else
				{
					NErrorPopup nerrorPopup = NErrorPopup.Create(netErrorInfo.Value);
					if (nerrorPopup != null)
					{
						NModalContainer.Instance.Add(nerrorPopup, true);
					}
				}
				netService = null;
			}
			catch
			{
				NErrorPopup nerrorPopup2 = NErrorPopup.Create(new NetErrorInfo(NetError.InternalError, false));
				if (nerrorPopup2 != null)
				{
					NModalContainer.Instance.Add(nerrorPopup2, true);
				}
				throw;
			}
			finally
			{
				loadingOverlay.Visible = false;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B0C RID: 11020 RVA: 0x0012F314 File Offset: 0x0012D514
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NMultiplayerHostSubmenu.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NMultiplayerHostSubmenu.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerHostSubmenu.MethodName.RefreshButtons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerHostSubmenu.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerHostSubmenu.MethodName.OnStandardPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerHostSubmenu.MethodName.OnDailyPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerHostSubmenu.MethodName.OnCustomPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerHostSubmenu.MethodName.StartHost, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "gameMode", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B0D RID: 11021 RVA: 0x0012F564 File Offset: 0x0012D764
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerHostSubmenu.MethodName.Create && args.Count == 0)
			{
				NMultiplayerHostSubmenu nmultiplayerHostSubmenu = NMultiplayerHostSubmenu.Create();
				ret = VariantUtils.CreateFrom<NMultiplayerHostSubmenu>(ref nmultiplayerHostSubmenu);
				return true;
			}
			if ((ref method) == NMultiplayerHostSubmenu.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerHostSubmenu.MethodName.RefreshButtons && args.Count == 0)
			{
				this.RefreshButtons();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerHostSubmenu.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerHostSubmenu.MethodName.OnStandardPressed && args.Count == 1)
			{
				this.OnStandardPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerHostSubmenu.MethodName.OnDailyPressed && args.Count == 1)
			{
				this.OnDailyPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerHostSubmenu.MethodName.OnCustomPressed && args.Count == 1)
			{
				this.OnCustomPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerHostSubmenu.MethodName.StartHost && args.Count == 1)
			{
				this.StartHost(VariantUtils.ConvertTo<GameMode>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x0012F6E0 File Offset: 0x0012D8E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerHostSubmenu.MethodName.Create && args.Count == 0)
			{
				NMultiplayerHostSubmenu nmultiplayerHostSubmenu = NMultiplayerHostSubmenu.Create();
				ret = VariantUtils.CreateFrom<NMultiplayerHostSubmenu>(ref nmultiplayerHostSubmenu);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002B0F RID: 11023 RVA: 0x0012F720 File Offset: 0x0012D920
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerHostSubmenu.MethodName.Create || (ref method) == NMultiplayerHostSubmenu.MethodName._Ready || (ref method) == NMultiplayerHostSubmenu.MethodName.RefreshButtons || (ref method) == NMultiplayerHostSubmenu.MethodName.OnSubmenuOpened || (ref method) == NMultiplayerHostSubmenu.MethodName.OnStandardPressed || (ref method) == NMultiplayerHostSubmenu.MethodName.OnDailyPressed || (ref method) == NMultiplayerHostSubmenu.MethodName.OnCustomPressed || (ref method) == NMultiplayerHostSubmenu.MethodName.StartHost || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002B10 RID: 11024 RVA: 0x0012F7AC File Offset: 0x0012D9AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerHostSubmenu.PropertyName._standardButton)
			{
				this._standardButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerHostSubmenu.PropertyName._dailyButton)
			{
				this._dailyButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerHostSubmenu.PropertyName._customButton)
			{
				this._customButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerHostSubmenu.PropertyName._loadingOverlay)
			{
				this._loadingOverlay = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002B11 RID: 11025 RVA: 0x0012F830 File Offset: 0x0012DA30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerHostSubmenu.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NMultiplayerHostSubmenu.PropertyName._standardButton)
			{
				value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._standardButton);
				return true;
			}
			if ((ref name) == NMultiplayerHostSubmenu.PropertyName._dailyButton)
			{
				value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._dailyButton);
				return true;
			}
			if ((ref name) == NMultiplayerHostSubmenu.PropertyName._customButton)
			{
				value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._customButton);
				return true;
			}
			if ((ref name) == NMultiplayerHostSubmenu.PropertyName._loadingOverlay)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._loadingOverlay);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B12 RID: 11026 RVA: 0x0012F8E8 File Offset: 0x0012DAE8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMultiplayerHostSubmenu.PropertyName._standardButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerHostSubmenu.PropertyName._dailyButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerHostSubmenu.PropertyName._customButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerHostSubmenu.PropertyName._loadingOverlay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerHostSubmenu.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B13 RID: 11027 RVA: 0x0012F9A4 File Offset: 0x0012DBA4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMultiplayerHostSubmenu.PropertyName._standardButton, Variant.From<NSubmenuButton>(ref this._standardButton));
			info.AddProperty(NMultiplayerHostSubmenu.PropertyName._dailyButton, Variant.From<NSubmenuButton>(ref this._dailyButton));
			info.AddProperty(NMultiplayerHostSubmenu.PropertyName._customButton, Variant.From<NSubmenuButton>(ref this._customButton));
			info.AddProperty(NMultiplayerHostSubmenu.PropertyName._loadingOverlay, Variant.From<Control>(ref this._loadingOverlay));
		}

		/// <inheritdoc />
		// Token: 0x06002B14 RID: 11028 RVA: 0x0012FA10 File Offset: 0x0012DC10
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerHostSubmenu.PropertyName._standardButton, ref variant))
			{
				this._standardButton = variant.As<NSubmenuButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMultiplayerHostSubmenu.PropertyName._dailyButton, ref variant2))
			{
				this._dailyButton = variant2.As<NSubmenuButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMultiplayerHostSubmenu.PropertyName._customButton, ref variant3))
			{
				this._customButton = variant3.As<NSubmenuButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMultiplayerHostSubmenu.PropertyName._loadingOverlay, ref variant4))
			{
				this._loadingOverlay = variant4.As<Control>();
			}
		}

		// Token: 0x0400102A RID: 4138
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/multiplayer_host_submenu");

		// Token: 0x0400102B RID: 4139
		private NSubmenuButton _standardButton;

		// Token: 0x0400102C RID: 4140
		private NSubmenuButton _dailyButton;

		// Token: 0x0400102D RID: 4141
		private NSubmenuButton _customButton;

		// Token: 0x0400102E RID: 4142
		private const string _keyStandard = "STANDARD_MP";

		// Token: 0x0400102F RID: 4143
		private const string _keyDaily = "DAILY_MP";

		// Token: 0x04001030 RID: 4144
		private const string _keyCustom = "CUSTOM_MP";

		// Token: 0x04001031 RID: 4145
		private Control _loadingOverlay;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200151A RID: 5402
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400508D RID: 20621
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400508E RID: 20622
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshButtons' method.
			/// </summary>
			// Token: 0x0400508F RID: 20623
			public static readonly StringName RefreshButtons = "RefreshButtons";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04005090 RID: 20624
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnStandardPressed' method.
			/// </summary>
			// Token: 0x04005091 RID: 20625
			public static readonly StringName OnStandardPressed = "OnStandardPressed";

			/// <summary>
			/// Cached name for the 'OnDailyPressed' method.
			/// </summary>
			// Token: 0x04005092 RID: 20626
			public static readonly StringName OnDailyPressed = "OnDailyPressed";

			/// <summary>
			/// Cached name for the 'OnCustomPressed' method.
			/// </summary>
			// Token: 0x04005093 RID: 20627
			public static readonly StringName OnCustomPressed = "OnCustomPressed";

			/// <summary>
			/// Cached name for the 'StartHost' method.
			/// </summary>
			// Token: 0x04005094 RID: 20628
			public static readonly StringName StartHost = "StartHost";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200151B RID: 5403
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04005095 RID: 20629
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_standardButton' field.
			/// </summary>
			// Token: 0x04005096 RID: 20630
			public static readonly StringName _standardButton = "_standardButton";

			/// <summary>
			/// Cached name for the '_dailyButton' field.
			/// </summary>
			// Token: 0x04005097 RID: 20631
			public static readonly StringName _dailyButton = "_dailyButton";

			/// <summary>
			/// Cached name for the '_customButton' field.
			/// </summary>
			// Token: 0x04005098 RID: 20632
			public static readonly StringName _customButton = "_customButton";

			/// <summary>
			/// Cached name for the '_loadingOverlay' field.
			/// </summary>
			// Token: 0x04005099 RID: 20633
			public static readonly StringName _loadingOverlay = "_loadingOverlay";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200151C RID: 5404
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
