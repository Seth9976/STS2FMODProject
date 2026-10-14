using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x0200030D RID: 781
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMultiplayerSubmenu.cs")]
	public class NMultiplayerSubmenu : NSubmenu
	{
		// Token: 0x06002B17 RID: 11031 RVA: 0x0012FAAD File Offset: 0x0012DCAD
		[NullableContext(2)]
		public static NMultiplayerSubmenu Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NMultiplayerSubmenu._scenePath).Instantiate<NMultiplayerSubmenu>(0L);
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x0012FAD0 File Offset: 0x0012DCD0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._loadingOverlay = base.GetNode<Control>("%LoadingOverlay");
			this._hostButton = base.GetNode<NSubmenuButton>("ButtonContainer/HostButton");
			this._hostButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnHostPressed)), 0U);
			this._hostButton.SetIconAndLocalization("HOST");
			this._loadButton = base.GetNode<NSubmenuButton>("ButtonContainer/LoadButton");
			this._loadButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.StartLoad)), 0U);
			this._loadButton.SetIconAndLocalization("MP_LOAD");
			this._joinButton = base.GetNode<NSubmenuButton>("ButtonContainer/JoinButton");
			this._joinButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenJoinFriendsScreen)), 0U);
			this._joinButton.SetIconAndLocalization("JOIN");
			this._abandonButton = base.GetNode<NSubmenuButton>("ButtonContainer/AbandonButton");
			this._abandonButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.AbandonRun)), 0U);
			this._abandonButton.SetIconAndLocalization("MP_ABANDON");
			this.UpdateButtons();
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x0012FC24 File Offset: 0x0012DE24
		private void UpdateButtons()
		{
			this._hostButton.Visible = !SaveManager.Instance.HasMultiplayerRunSave;
			this._loadButton.Visible = SaveManager.Instance.HasMultiplayerRunSave;
			this._abandonButton.Visible = SaveManager.Instance.HasMultiplayerRunSave;
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x0012FC73 File Offset: 0x0012DE73
		protected override Control InitialFocusedControl
		{
			get
			{
				if (SaveManager.Instance.HasMultiplayerRunSave)
				{
					return this._loadButton;
				}
				return this._hostButton;
			}
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x0012FC8E File Offset: 0x0012DE8E
		private void AbandonRun(NButton _)
		{
			TaskHelper.RunSafely(this.TryAbandonMultiplayerRun());
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x0012FC9C File Offset: 0x0012DE9C
		private async Task TryAbandonMultiplayerRun()
		{
			LocString locString = new LocString("main_menu_ui", "ABANDON_RUN_CONFIRMATION.header");
			LocString locString2 = new LocString("main_menu_ui", "ABANDON_RUN_CONFIRMATION.body");
			LocString locString3 = new LocString("main_menu_ui", "GENERIC_POPUP.confirm");
			LocString locString4 = new LocString("main_menu_ui", "GENERIC_POPUP.cancel");
			NGenericPopup ngenericPopup = NGenericPopup.Create();
			NModalContainer.Instance.Add(ngenericPopup, true);
			TaskAwaiter<bool> taskAwaiter = ngenericPopup.WaitForConfirmation(locString2, locString, locString4, locString3).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				ulong localPlayerId = PlatformUtil.GetLocalPlayerId(PlatformUtil.PrimaryPlatform);
				ReadSaveResult<SerializableRun> readSaveResult = SaveManager.Instance.LoadAndCanonicalizeMultiplayerRunSave(localPlayerId);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (readSaveResult.Success && readSaveResult.SaveData != null)
				{
					try
					{
						SerializableRun saveData = readSaveResult.SaveData;
						SaveManager.Instance.UpdateProgressWithRunData(saveData, false);
						RunHistoryUtilities.CreateRunHistoryEntry(saveData, false, true, saveData.PlatformType);
						if (saveData.DailyTime != null)
						{
							int num = ScoreUtility.CalculateDailyScore(saveData, localPlayerId, false);
							TaskHelper.RunSafely(DailyRunUtility.UploadScore(saveData.DailyTime.Value, num, saveData.Players));
						}
						goto IL_01E7;
					}
					catch (Exception ex)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
						defaultInterpolatedStringHandler.AppendLiteral("ERROR: Failed to upload run history/metrics: ");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						goto IL_01E7;
					}
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ERROR: Failed to load multiplayer run save: status=");
				defaultInterpolatedStringHandler.AppendFormatted<ReadSaveStatus>(readSaveResult.Status);
				defaultInterpolatedStringHandler.AppendLiteral(". Deleting current run...");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				IL_01E7:
				SaveManager.Instance.DeleteCurrentMultiplayerRun();
				this.UpdateButtons();
			}
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x0012FCE0 File Offset: 0x0012DEE0
		private void StartLoad(NButton _)
		{
			PlatformType platformType = ((SteamInitializer.Initialized && !CommandLineHelper.HasArg("fastmp")) ? PlatformType.Steam : PlatformType.None);
			ReadSaveResult<SerializableRun> readSaveResult = SaveManager.Instance.LoadAndCanonicalizeMultiplayerRunSave(PlatformUtil.GetLocalPlayerId(platformType));
			if (!readSaveResult.Success || readSaveResult.SaveData == null)
			{
				Log.Warn("Broken multiplayer run save detected, disabling button", 2);
				this._loadButton.Disable();
				NErrorPopup nerrorPopup = NErrorPopup.Create(new LocString("main_menu_ui", "INVALID_SAVE_POPUP.title"), new LocString("main_menu_ui", "INVALID_SAVE_POPUP.description_run"), new LocString("main_menu_ui", "INVALID_SAVE_POPUP.dismiss"), true);
				NModalContainer.Instance.Add(nerrorPopup, true);
				NModalContainer.Instance.ShowBackstop();
				return;
			}
			this.StartHost(readSaveResult.SaveData);
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x0012FD93 File Offset: 0x0012DF93
		private void OnHostPressed(NButton _)
		{
			if (SaveManager.Instance.Progress.NumberOfRuns > 0)
			{
				this._stack.PushSubmenuType<NMultiplayerHostSubmenu>();
				return;
			}
			TaskHelper.RunSafely(NMultiplayerHostSubmenu.StartHostAsync(GameMode.Standard, this._loadingOverlay, this._stack));
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x0012FDCC File Offset: 0x0012DFCC
		public void FastHost(GameMode gameMode)
		{
			NMultiplayerHostSubmenu nmultiplayerHostSubmenu = this._stack.PushSubmenuType<NMultiplayerHostSubmenu>();
			nmultiplayerHostSubmenu.StartHost(gameMode);
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x0012FDEC File Offset: 0x0012DFEC
		public void StartHost(SerializableRun run)
		{
			TaskHelper.RunSafely(this.StartHostAsync(run));
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x0012FDFC File Offset: 0x0012DFFC
		private async Task StartHostAsync(SerializableRun run)
		{
			PlatformType platformType = ((SteamInitializer.Initialized && !CommandLineHelper.HasArg("fastmp")) ? PlatformType.Steam : PlatformType.None);
			this._loadingOverlay.Visible = true;
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
					GameMode gameMode = run.GameMode;
					if (gameMode != GameMode.Daily)
					{
						if (gameMode != GameMode.Custom)
						{
							NMultiplayerLoadGameScreen submenuType = this._stack.GetSubmenuType<NMultiplayerLoadGameScreen>();
							submenuType.InitializeAsHost(netService, run);
							this._stack.Push(submenuType);
						}
						else
						{
							NCustomRunLoadScreen submenuType2 = this._stack.GetSubmenuType<NCustomRunLoadScreen>();
							submenuType2.InitializeAsHost(netService, run);
							this._stack.Push(submenuType2);
						}
					}
					else
					{
						NDailyRunLoadScreen submenuType3 = this._stack.GetSubmenuType<NDailyRunLoadScreen>();
						submenuType3.InitializeAsHost(netService, run);
						this._stack.Push(submenuType3);
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
			finally
			{
				this._loadingOverlay.Visible = false;
			}
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x0012FE47 File Offset: 0x0012E047
		private void OpenJoinFriendsScreen(NButton _)
		{
			this.OnJoinFriendsPressed();
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x0012FE50 File Offset: 0x0012E050
		public NJoinFriendScreen OnJoinFriendsPressed()
		{
			return this._stack.PushSubmenuType<NJoinFriendScreen>();
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x0012FE60 File Offset: 0x0012E060
		protected override void OnSubmenuShown()
		{
			base.OnSubmenuShown();
			bool flag = !SaveManager.Instance.SeenFtue("multiplayer_warning") && SaveManager.Instance.Progress.NumberOfRuns == 0;
			if (flag && !CommandLineHelper.HasArg("fastmp"))
			{
				NMultiplayerWarningPopup nmultiplayerWarningPopup = NMultiplayerWarningPopup.Create();
				NModalContainer.Instance.Add(nmultiplayerWarningPopup, true);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B25 RID: 11045 RVA: 0x0012FEBC File Offset: 0x0012E0BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NMultiplayerSubmenu.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NMultiplayerSubmenu.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerSubmenu.MethodName.UpdateButtons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerSubmenu.MethodName.AbandonRun, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerSubmenu.MethodName.StartLoad, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerSubmenu.MethodName.OnHostPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerSubmenu.MethodName.FastHost, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "gameMode", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerSubmenu.MethodName.OpenJoinFriendsScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerSubmenu.MethodName.OnJoinFriendsPressed, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null),
				new MethodInfo(NMultiplayerSubmenu.MethodName.OnSubmenuShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B26 RID: 11046 RVA: 0x001301A4 File Offset: 0x0012E3A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerSubmenu.MethodName.Create && args.Count == 0)
			{
				NMultiplayerSubmenu nmultiplayerSubmenu = NMultiplayerSubmenu.Create();
				ret = VariantUtils.CreateFrom<NMultiplayerSubmenu>(ref nmultiplayerSubmenu);
				return true;
			}
			if ((ref method) == NMultiplayerSubmenu.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerSubmenu.MethodName.UpdateButtons && args.Count == 0)
			{
				this.UpdateButtons();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerSubmenu.MethodName.AbandonRun && args.Count == 1)
			{
				this.AbandonRun(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerSubmenu.MethodName.StartLoad && args.Count == 1)
			{
				this.StartLoad(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerSubmenu.MethodName.OnHostPressed && args.Count == 1)
			{
				this.OnHostPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerSubmenu.MethodName.FastHost && args.Count == 1)
			{
				this.FastHost(VariantUtils.ConvertTo<GameMode>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerSubmenu.MethodName.OpenJoinFriendsScreen && args.Count == 1)
			{
				this.OpenJoinFriendsScreen(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerSubmenu.MethodName.OnJoinFriendsPressed && args.Count == 0)
			{
				NJoinFriendScreen njoinFriendScreen = this.OnJoinFriendsPressed();
				ret = VariantUtils.CreateFrom<NJoinFriendScreen>(ref njoinFriendScreen);
				return true;
			}
			if ((ref method) == NMultiplayerSubmenu.MethodName.OnSubmenuShown && args.Count == 0)
			{
				this.OnSubmenuShown();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x00130380 File Offset: 0x0012E580
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerSubmenu.MethodName.Create && args.Count == 0)
			{
				NMultiplayerSubmenu nmultiplayerSubmenu = NMultiplayerSubmenu.Create();
				ret = VariantUtils.CreateFrom<NMultiplayerSubmenu>(ref nmultiplayerSubmenu);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002B28 RID: 11048 RVA: 0x001303C0 File Offset: 0x0012E5C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerSubmenu.MethodName.Create || (ref method) == NMultiplayerSubmenu.MethodName._Ready || (ref method) == NMultiplayerSubmenu.MethodName.UpdateButtons || (ref method) == NMultiplayerSubmenu.MethodName.AbandonRun || (ref method) == NMultiplayerSubmenu.MethodName.StartLoad || (ref method) == NMultiplayerSubmenu.MethodName.OnHostPressed || (ref method) == NMultiplayerSubmenu.MethodName.FastHost || (ref method) == NMultiplayerSubmenu.MethodName.OpenJoinFriendsScreen || (ref method) == NMultiplayerSubmenu.MethodName.OnJoinFriendsPressed || (ref method) == NMultiplayerSubmenu.MethodName.OnSubmenuShown || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002B29 RID: 11049 RVA: 0x0013046C File Offset: 0x0012E66C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerSubmenu.PropertyName._hostButton)
			{
				this._hostButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerSubmenu.PropertyName._loadButton)
			{
				this._loadButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerSubmenu.PropertyName._abandonButton)
			{
				this._abandonButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerSubmenu.PropertyName._joinButton)
			{
				this._joinButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerSubmenu.PropertyName._loadingOverlay)
			{
				this._loadingOverlay = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002B2A RID: 11050 RVA: 0x00130508 File Offset: 0x0012E708
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerSubmenu.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NMultiplayerSubmenu.PropertyName._hostButton)
			{
				value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._hostButton);
				return true;
			}
			if ((ref name) == NMultiplayerSubmenu.PropertyName._loadButton)
			{
				value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._loadButton);
				return true;
			}
			if ((ref name) == NMultiplayerSubmenu.PropertyName._abandonButton)
			{
				value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._abandonButton);
				return true;
			}
			if ((ref name) == NMultiplayerSubmenu.PropertyName._joinButton)
			{
				value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._joinButton);
				return true;
			}
			if ((ref name) == NMultiplayerSubmenu.PropertyName._loadingOverlay)
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
		// Token: 0x06002B2B RID: 11051 RVA: 0x001305E0 File Offset: 0x0012E7E0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMultiplayerSubmenu.PropertyName._hostButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerSubmenu.PropertyName._loadButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerSubmenu.PropertyName._abandonButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerSubmenu.PropertyName._joinButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerSubmenu.PropertyName._loadingOverlay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerSubmenu.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B2C RID: 11052 RVA: 0x001306BC File Offset: 0x0012E8BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMultiplayerSubmenu.PropertyName._hostButton, Variant.From<NSubmenuButton>(ref this._hostButton));
			info.AddProperty(NMultiplayerSubmenu.PropertyName._loadButton, Variant.From<NSubmenuButton>(ref this._loadButton));
			info.AddProperty(NMultiplayerSubmenu.PropertyName._abandonButton, Variant.From<NSubmenuButton>(ref this._abandonButton));
			info.AddProperty(NMultiplayerSubmenu.PropertyName._joinButton, Variant.From<NSubmenuButton>(ref this._joinButton));
			info.AddProperty(NMultiplayerSubmenu.PropertyName._loadingOverlay, Variant.From<Control>(ref this._loadingOverlay));
		}

		/// <inheritdoc />
		// Token: 0x06002B2D RID: 11053 RVA: 0x00130740 File Offset: 0x0012E940
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerSubmenu.PropertyName._hostButton, ref variant))
			{
				this._hostButton = variant.As<NSubmenuButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMultiplayerSubmenu.PropertyName._loadButton, ref variant2))
			{
				this._loadButton = variant2.As<NSubmenuButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMultiplayerSubmenu.PropertyName._abandonButton, ref variant3))
			{
				this._abandonButton = variant3.As<NSubmenuButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMultiplayerSubmenu.PropertyName._joinButton, ref variant4))
			{
				this._joinButton = variant4.As<NSubmenuButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMultiplayerSubmenu.PropertyName._loadingOverlay, ref variant5))
			{
				this._loadingOverlay = variant5.As<Control>();
			}
		}

		// Token: 0x04001032 RID: 4146
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/multiplayer_submenu");

		// Token: 0x04001033 RID: 4147
		private NSubmenuButton _hostButton;

		// Token: 0x04001034 RID: 4148
		private NSubmenuButton _loadButton;

		// Token: 0x04001035 RID: 4149
		private NSubmenuButton _abandonButton;

		// Token: 0x04001036 RID: 4150
		private NSubmenuButton _joinButton;

		// Token: 0x04001037 RID: 4151
		private const string _keyHost = "HOST";

		// Token: 0x04001038 RID: 4152
		private const string _keyLoad = "MP_LOAD";

		// Token: 0x04001039 RID: 4153
		private const string _keyJoin = "JOIN";

		// Token: 0x0400103A RID: 4154
		private const string _keyAbandon = "MP_ABANDON";

		// Token: 0x0400103B RID: 4155
		private Control _loadingOverlay;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200151E RID: 5406
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040050A1 RID: 20641
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040050A2 RID: 20642
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'UpdateButtons' method.
			/// </summary>
			// Token: 0x040050A3 RID: 20643
			public static readonly StringName UpdateButtons = "UpdateButtons";

			/// <summary>
			/// Cached name for the 'AbandonRun' method.
			/// </summary>
			// Token: 0x040050A4 RID: 20644
			public static readonly StringName AbandonRun = "AbandonRun";

			/// <summary>
			/// Cached name for the 'StartLoad' method.
			/// </summary>
			// Token: 0x040050A5 RID: 20645
			public static readonly StringName StartLoad = "StartLoad";

			/// <summary>
			/// Cached name for the 'OnHostPressed' method.
			/// </summary>
			// Token: 0x040050A6 RID: 20646
			public static readonly StringName OnHostPressed = "OnHostPressed";

			/// <summary>
			/// Cached name for the 'FastHost' method.
			/// </summary>
			// Token: 0x040050A7 RID: 20647
			public static readonly StringName FastHost = "FastHost";

			/// <summary>
			/// Cached name for the 'OpenJoinFriendsScreen' method.
			/// </summary>
			// Token: 0x040050A8 RID: 20648
			public static readonly StringName OpenJoinFriendsScreen = "OpenJoinFriendsScreen";

			/// <summary>
			/// Cached name for the 'OnJoinFriendsPressed' method.
			/// </summary>
			// Token: 0x040050A9 RID: 20649
			public static readonly StringName OnJoinFriendsPressed = "OnJoinFriendsPressed";

			/// <summary>
			/// Cached name for the 'OnSubmenuShown' method.
			/// </summary>
			// Token: 0x040050AA RID: 20650
			public new static readonly StringName OnSubmenuShown = "OnSubmenuShown";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200151F RID: 5407
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x040050AB RID: 20651
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_hostButton' field.
			/// </summary>
			// Token: 0x040050AC RID: 20652
			public static readonly StringName _hostButton = "_hostButton";

			/// <summary>
			/// Cached name for the '_loadButton' field.
			/// </summary>
			// Token: 0x040050AD RID: 20653
			public static readonly StringName _loadButton = "_loadButton";

			/// <summary>
			/// Cached name for the '_abandonButton' field.
			/// </summary>
			// Token: 0x040050AE RID: 20654
			public static readonly StringName _abandonButton = "_abandonButton";

			/// <summary>
			/// Cached name for the '_joinButton' field.
			/// </summary>
			// Token: 0x040050AF RID: 20655
			public static readonly StringName _joinButton = "_joinButton";

			/// <summary>
			/// Cached name for the '_loadingOverlay' field.
			/// </summary>
			// Token: 0x040050B0 RID: 20656
			public static readonly StringName _loadingOverlay = "_loadingOverlay";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001520 RID: 5408
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
