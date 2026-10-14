using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu
{
	// Token: 0x020002E0 RID: 736
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/PauseMenu/NPauseMenu.cs")]
	public class NPauseMenu : NSubmenu
	{
		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x0600275B RID: 10075 RVA: 0x0010CB5C File Offset: 0x0010AD5C
		protected override Control InitialFocusedControl
		{
			get
			{
				return this._resumeButton;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x0600275C RID: 10076 RVA: 0x0010CB64 File Offset: 0x0010AD64
		private NPauseMenuButton[] Buttons
		{
			get
			{
				return new NPauseMenuButton[] { this._resumeButton, this._settingsButton, this._compendiumButton, this._giveUpButton, this._disconnectButton, this._saveAndQuitButton };
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x0600275D RID: 10077 RVA: 0x0010CBA2 File Offset: 0x0010ADA2
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x0600275E RID: 10078 RVA: 0x0010CBA5 File Offset: 0x0010ADA5
		public NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.PauseMenu;
			}
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x0010CBAC File Offset: 0x0010ADAC
		public override void _Ready()
		{
			this.ConnectSignals();
			this._buttonContainer = base.GetNode<Control>("%ButtonContainer");
			this._resumeButton = this._buttonContainer.GetNode<NPauseMenuButton>("Resume");
			this._settingsButton = this._buttonContainer.GetNode<NPauseMenuButton>("Settings");
			this._compendiumButton = this._buttonContainer.GetNode<NPauseMenuButton>("Compendium");
			this._giveUpButton = this._buttonContainer.GetNode<NPauseMenuButton>("GiveUp");
			this._disconnectButton = this._buttonContainer.GetNode<NPauseMenuButton>("Disconnect");
			this._saveAndQuitButton = this._buttonContainer.GetNode<NPauseMenuButton>("SaveAndQuit");
			this._backButton = base.GetNode<NBackButton>("%BackButton");
			this._pausedLabel = base.GetNode<MegaLabel>("%PausedText/Label");
			this.RefreshLabels();
			this._resumeButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnBackOrResumeButtonPressed)), 0U);
			this._settingsButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnSettingsButtonPressed)), 0U);
			this._compendiumButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCompendiumButtonPressed)), 0U);
			this._giveUpButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnGiveUpButtonPressed)), 0U);
			this._disconnectButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnDisconnectButtonPressed)), 0U);
			this._saveAndQuitButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnSaveAndQuitButtonPressed)), 0U);
			this._backButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnBackOrResumeButtonPressed)), 0U);
			this._backButton.Disable();
			this._giveUpButton.Visible = RunManager.Instance.NetService.Type != NetGameType.Client;
			this._saveAndQuitButton.Visible = RunManager.Instance.NetService.Type != NetGameType.Client;
			this._disconnectButton.Visible = RunManager.Instance.NetService.Type == NetGameType.Client;
			for (int i = 0; i < this.Buttons.Length; i++)
			{
				this.Buttons[i].FocusNeighborLeft = this.Buttons[i].GetPath();
				this.Buttons[i].FocusNeighborRight = this.Buttons[i].GetPath();
				Control control = this.Buttons[i];
				NodePath nodePath;
				if (i <= 0)
				{
					NPauseMenuButton[] buttons = this.Buttons;
					nodePath = buttons[buttons.Length - 1].GetPath();
				}
				else
				{
					nodePath = this.Buttons[i - 1].GetPath();
				}
				control.FocusNeighborTop = nodePath;
				this.Buttons[i].FocusNeighborBottom = ((i < this.Buttons.Length - 1) ? this.Buttons[i + 1].GetPath() : this.Buttons[0].GetPath());
			}
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x0010CEBE File Offset: 0x0010B0BE
		public override void _ExitTree()
		{
			RunManager.Instance.IsPaused = false;
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x0010CECC File Offset: 0x0010B0CC
		private void RefreshLabels()
		{
			this._pausedLabel.SetTextAutoSize(NPauseMenu._pausedLoc.GetFormattedText());
			this._resumeButton.GetNode<MegaLabel>("Label").SetTextAutoSize(NPauseMenu._resumeLoc.GetFormattedText());
			this._settingsButton.GetNode<MegaLabel>("Label").SetTextAutoSize(NPauseMenu._settingsLoc.GetFormattedText());
			this._compendiumButton.GetNode<MegaLabel>("Label").SetTextAutoSize(NPauseMenu._compendiumLoc.GetFormattedText());
			this._giveUpButton.GetNode<MegaLabel>("Label").SetTextAutoSize(NPauseMenu._giveUpLoc.GetFormattedText());
			this._disconnectButton.GetNode<MegaLabel>("Label").SetTextAutoSize(NPauseMenu._disconnectLoc.GetFormattedText());
			this._saveAndQuitButton.GetNode<MegaLabel>("Label").SetTextAutoSize(NPauseMenu._saveAndQuitLoc.GetFormattedText());
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x0010CFC8 File Offset: 0x0010B1C8
		public void Initialize(IRunState runState)
		{
			this._runState = runState;
			if (!RunManager.Instance.IsInProgress || this._runState.IsGameOver)
			{
				this._giveUpButton.Disable();
			}
			else
			{
				this._giveUpButton.Enable();
			}
			this._compendiumButton.Visible = !NGame.IsReleaseGame() || SaveManager.Instance.IsCompendiumAvailable();
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x0010D02C File Offset: 0x0010B22C
		private void OnBackOrResumeButtonPressed(NButton _)
		{
			SfxCmd.Play("event:/sfx/ui/map/map_close", 1f);
			NCapstoneContainer.Instance.Close();
			NRun.Instance.GlobalUi.TopBar.Pause.ToggleAnimState();
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x0010D060 File Offset: 0x0010B260
		private void OnSettingsButtonPressed(NButton _)
		{
			this._stack.PushSubmenuType<NSettingsScreen>();
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x0010D070 File Offset: 0x0010B270
		private void OnCompendiumButtonPressed(NButton _)
		{
			NCompendiumSubmenu submenuType = this._stack.GetSubmenuType<NCompendiumSubmenu>();
			submenuType.Initialize(this._runState);
			this._stack.Push(submenuType);
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x0010D0A1 File Offset: 0x0010B2A1
		private void OnGiveUpButtonPressed(NButton _)
		{
			NModalContainer.Instance.Add(NAbandonRunConfirmPopup.Create(null), true);
		}

		/// <summary>
		/// For multiplayer!
		/// </summary>
		// Token: 0x06002767 RID: 10087 RVA: 0x0010D0B4 File Offset: 0x0010B2B4
		private void OnDisconnectButtonPressed(NButton _)
		{
			if (RunManager.Instance.NetService.IsConnected)
			{
				NModalContainer.Instance.Add(NDisconnectConfirmPopup.Create(), true);
				return;
			}
			TaskHelper.RunSafely(NGame.Instance.ReturnToMainMenuAfterRun());
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x0010D0E8 File Offset: 0x0010B2E8
		private void OnSaveAndQuitButtonPressed(NButton _)
		{
			TaskHelper.RunSafely(this.CloseToMenu());
		}

		/// <summary>
		/// Helper method to quit to main menu as we require an async Task.
		/// </summary>
		// Token: 0x06002769 RID: 10089 RVA: 0x0010D0F8 File Offset: 0x0010B2F8
		private async Task CloseToMenu()
		{
			this._resumeButton.Disable();
			this._settingsButton.Disable();
			this._compendiumButton.Disable();
			this._giveUpButton.Disable();
			this._disconnectButton.Disable();
			this._saveAndQuitButton.Disable();
			this._backButton.Disable();
			NRunMusicController.Instance.StopMusic();
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.StopAll();
			}
			await NGame.Instance.ReturnToMainMenu();
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x0010D13B File Offset: 0x0010B33B
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			RunManager.Instance.IsPaused = true;
			NHotkeyManager.Instance.AddBlockingScreen(this);
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x0010D159 File Offset: 0x0010B359
		public override void OnSubmenuClosed()
		{
			this._backButton.Disable();
			base.Visible = false;
			RunManager.Instance.IsPaused = false;
			NHotkeyManager.Instance.RemoveBlockingScreen(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600276C RID: 10092 RVA: 0x0010D184 File Offset: 0x0010B384
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NPauseMenu.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPauseMenu.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPauseMenu.MethodName.RefreshLabels, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPauseMenu.MethodName.OnBackOrResumeButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPauseMenu.MethodName.OnSettingsButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPauseMenu.MethodName.OnCompendiumButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPauseMenu.MethodName.OnGiveUpButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPauseMenu.MethodName.OnDisconnectButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPauseMenu.MethodName.OnSaveAndQuitButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPauseMenu.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPauseMenu.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600276D RID: 10093 RVA: 0x0010D4C0 File Offset: 0x0010B6C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPauseMenu.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenu.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenu.MethodName.RefreshLabels && args.Count == 0)
			{
				this.RefreshLabels();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenu.MethodName.OnBackOrResumeButtonPressed && args.Count == 1)
			{
				this.OnBackOrResumeButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenu.MethodName.OnSettingsButtonPressed && args.Count == 1)
			{
				this.OnSettingsButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenu.MethodName.OnCompendiumButtonPressed && args.Count == 1)
			{
				this.OnCompendiumButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenu.MethodName.OnGiveUpButtonPressed && args.Count == 1)
			{
				this.OnGiveUpButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenu.MethodName.OnDisconnectButtonPressed && args.Count == 1)
			{
				this.OnDisconnectButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenu.MethodName.OnSaveAndQuitButtonPressed && args.Count == 1)
			{
				this.OnSaveAndQuitButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenu.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenu.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600276E RID: 10094 RVA: 0x0010D6C4 File Offset: 0x0010B8C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPauseMenu.MethodName._Ready || (ref method) == NPauseMenu.MethodName._ExitTree || (ref method) == NPauseMenu.MethodName.RefreshLabels || (ref method) == NPauseMenu.MethodName.OnBackOrResumeButtonPressed || (ref method) == NPauseMenu.MethodName.OnSettingsButtonPressed || (ref method) == NPauseMenu.MethodName.OnCompendiumButtonPressed || (ref method) == NPauseMenu.MethodName.OnGiveUpButtonPressed || (ref method) == NPauseMenu.MethodName.OnDisconnectButtonPressed || (ref method) == NPauseMenu.MethodName.OnSaveAndQuitButtonPressed || (ref method) == NPauseMenu.MethodName.OnSubmenuOpened || (ref method) == NPauseMenu.MethodName.OnSubmenuClosed || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600276F RID: 10095 RVA: 0x0010D780 File Offset: 0x0010B980
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPauseMenu.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._buttonContainer)
			{
				this._buttonContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._resumeButton)
			{
				this._resumeButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._settingsButton)
			{
				this._settingsButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._compendiumButton)
			{
				this._compendiumButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._giveUpButton)
			{
				this._giveUpButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._disconnectButton)
			{
				this._disconnectButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._saveAndQuitButton)
			{
				this._saveAndQuitButton = VariantUtils.ConvertTo<NPauseMenuButton>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._pausedLabel)
			{
				this._pausedLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002770 RID: 10096 RVA: 0x0010D888 File Offset: 0x0010BA88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPauseMenu.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName.Buttons)
			{
				GodotObject[] buttons = this.Buttons;
				value = VariantUtils.CreateFromSystemArrayOfGodotObject(buttons);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._buttonContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._buttonContainer);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._resumeButton)
			{
				value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._resumeButton);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._settingsButton)
			{
				value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._settingsButton);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._compendiumButton)
			{
				value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._compendiumButton);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._giveUpButton)
			{
				value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._giveUpButton);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._disconnectButton)
			{
				value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._disconnectButton);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._saveAndQuitButton)
			{
				value = VariantUtils.CreateFrom<NPauseMenuButton>(ref this._saveAndQuitButton);
				return true;
			}
			if ((ref name) == NPauseMenu.PropertyName._pausedLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._pausedLabel);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002771 RID: 10097 RVA: 0x0010DA48 File Offset: 0x0010BC48
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPauseMenu.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenu.PropertyName._buttonContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenu.PropertyName._resumeButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenu.PropertyName._settingsButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenu.PropertyName._compendiumButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenu.PropertyName._giveUpButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenu.PropertyName._disconnectButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenu.PropertyName._saveAndQuitButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenu.PropertyName._pausedLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenu.PropertyName.InitialFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(28L, NPauseMenu.PropertyName.Buttons, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPauseMenu.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(2L, NPauseMenu.PropertyName.ScreenType, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002772 RID: 10098 RVA: 0x0010DC08 File Offset: 0x0010BE08
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPauseMenu.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NPauseMenu.PropertyName._buttonContainer, Variant.From<Control>(ref this._buttonContainer));
			info.AddProperty(NPauseMenu.PropertyName._resumeButton, Variant.From<NPauseMenuButton>(ref this._resumeButton));
			info.AddProperty(NPauseMenu.PropertyName._settingsButton, Variant.From<NPauseMenuButton>(ref this._settingsButton));
			info.AddProperty(NPauseMenu.PropertyName._compendiumButton, Variant.From<NPauseMenuButton>(ref this._compendiumButton));
			info.AddProperty(NPauseMenu.PropertyName._giveUpButton, Variant.From<NPauseMenuButton>(ref this._giveUpButton));
			info.AddProperty(NPauseMenu.PropertyName._disconnectButton, Variant.From<NPauseMenuButton>(ref this._disconnectButton));
			info.AddProperty(NPauseMenu.PropertyName._saveAndQuitButton, Variant.From<NPauseMenuButton>(ref this._saveAndQuitButton));
			info.AddProperty(NPauseMenu.PropertyName._pausedLabel, Variant.From<MegaLabel>(ref this._pausedLabel));
		}

		/// <inheritdoc />
		// Token: 0x06002773 RID: 10099 RVA: 0x0010DCE4 File Offset: 0x0010BEE4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPauseMenu.PropertyName._backButton, ref variant))
			{
				this._backButton = variant.As<NBackButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPauseMenu.PropertyName._buttonContainer, ref variant2))
			{
				this._buttonContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPauseMenu.PropertyName._resumeButton, ref variant3))
			{
				this._resumeButton = variant3.As<NPauseMenuButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPauseMenu.PropertyName._settingsButton, ref variant4))
			{
				this._settingsButton = variant4.As<NPauseMenuButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPauseMenu.PropertyName._compendiumButton, ref variant5))
			{
				this._compendiumButton = variant5.As<NPauseMenuButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPauseMenu.PropertyName._giveUpButton, ref variant6))
			{
				this._giveUpButton = variant6.As<NPauseMenuButton>();
			}
			Variant variant7;
			if (info.TryGetProperty(NPauseMenu.PropertyName._disconnectButton, ref variant7))
			{
				this._disconnectButton = variant7.As<NPauseMenuButton>();
			}
			Variant variant8;
			if (info.TryGetProperty(NPauseMenu.PropertyName._saveAndQuitButton, ref variant8))
			{
				this._saveAndQuitButton = variant8.As<NPauseMenuButton>();
			}
			Variant variant9;
			if (info.TryGetProperty(NPauseMenu.PropertyName._pausedLabel, ref variant9))
			{
				this._pausedLabel = variant9.As<MegaLabel>();
			}
		}

		// Token: 0x04000EA3 RID: 3747
		private static readonly LocString _pausedLoc = new LocString("gameplay_ui", "PAUSE_MENU.PAUSED");

		// Token: 0x04000EA4 RID: 3748
		private static readonly LocString _resumeLoc = new LocString("gameplay_ui", "PAUSE_MENU.RESUME");

		// Token: 0x04000EA5 RID: 3749
		private static readonly LocString _settingsLoc = new LocString("gameplay_ui", "PAUSE_MENU.SETTINGS");

		// Token: 0x04000EA6 RID: 3750
		private static readonly LocString _compendiumLoc = new LocString("gameplay_ui", "PAUSE_MENU.COMPENDIUM");

		// Token: 0x04000EA7 RID: 3751
		private static readonly LocString _giveUpLoc = new LocString("gameplay_ui", "PAUSE_MENU.GIVE_UP");

		// Token: 0x04000EA8 RID: 3752
		private static readonly LocString _disconnectLoc = new LocString("gameplay_ui", "PAUSE_MENU.DISCONNECT");

		// Token: 0x04000EA9 RID: 3753
		private static readonly LocString _saveAndQuitLoc = new LocString("gameplay_ui", "PAUSE_MENU.SAVE_AND_QUIT");

		// Token: 0x04000EAA RID: 3754
		private NBackButton _backButton;

		// Token: 0x04000EAB RID: 3755
		private Control _buttonContainer;

		// Token: 0x04000EAC RID: 3756
		private NPauseMenuButton _resumeButton;

		// Token: 0x04000EAD RID: 3757
		private NPauseMenuButton _settingsButton;

		// Token: 0x04000EAE RID: 3758
		private NPauseMenuButton _compendiumButton;

		// Token: 0x04000EAF RID: 3759
		private NPauseMenuButton _giveUpButton;

		// Token: 0x04000EB0 RID: 3760
		private NPauseMenuButton _disconnectButton;

		// Token: 0x04000EB1 RID: 3761
		private NPauseMenuButton _saveAndQuitButton;

		// Token: 0x04000EB2 RID: 3762
		private MegaLabel _pausedLabel;

		// Token: 0x04000EB3 RID: 3763
		private IRunState _runState;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001476 RID: 5238
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004D9B RID: 19867
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004D9C RID: 19868
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'RefreshLabels' method.
			/// </summary>
			// Token: 0x04004D9D RID: 19869
			public static readonly StringName RefreshLabels = "RefreshLabels";

			/// <summary>
			/// Cached name for the 'OnBackOrResumeButtonPressed' method.
			/// </summary>
			// Token: 0x04004D9E RID: 19870
			public static readonly StringName OnBackOrResumeButtonPressed = "OnBackOrResumeButtonPressed";

			/// <summary>
			/// Cached name for the 'OnSettingsButtonPressed' method.
			/// </summary>
			// Token: 0x04004D9F RID: 19871
			public static readonly StringName OnSettingsButtonPressed = "OnSettingsButtonPressed";

			/// <summary>
			/// Cached name for the 'OnCompendiumButtonPressed' method.
			/// </summary>
			// Token: 0x04004DA0 RID: 19872
			public static readonly StringName OnCompendiumButtonPressed = "OnCompendiumButtonPressed";

			/// <summary>
			/// Cached name for the 'OnGiveUpButtonPressed' method.
			/// </summary>
			// Token: 0x04004DA1 RID: 19873
			public static readonly StringName OnGiveUpButtonPressed = "OnGiveUpButtonPressed";

			/// <summary>
			/// Cached name for the 'OnDisconnectButtonPressed' method.
			/// </summary>
			// Token: 0x04004DA2 RID: 19874
			public static readonly StringName OnDisconnectButtonPressed = "OnDisconnectButtonPressed";

			/// <summary>
			/// Cached name for the 'OnSaveAndQuitButtonPressed' method.
			/// </summary>
			// Token: 0x04004DA3 RID: 19875
			public static readonly StringName OnSaveAndQuitButtonPressed = "OnSaveAndQuitButtonPressed";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04004DA4 RID: 19876
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x04004DA5 RID: 19877
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001477 RID: 5239
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04004DA6 RID: 19878
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the 'Buttons' property.
			/// </summary>
			// Token: 0x04004DA7 RID: 19879
			public static readonly StringName Buttons = "Buttons";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04004DA8 RID: 19880
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x04004DA9 RID: 19881
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04004DAA RID: 19882
			public new static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_buttonContainer' field.
			/// </summary>
			// Token: 0x04004DAB RID: 19883
			public static readonly StringName _buttonContainer = "_buttonContainer";

			/// <summary>
			/// Cached name for the '_resumeButton' field.
			/// </summary>
			// Token: 0x04004DAC RID: 19884
			public static readonly StringName _resumeButton = "_resumeButton";

			/// <summary>
			/// Cached name for the '_settingsButton' field.
			/// </summary>
			// Token: 0x04004DAD RID: 19885
			public static readonly StringName _settingsButton = "_settingsButton";

			/// <summary>
			/// Cached name for the '_compendiumButton' field.
			/// </summary>
			// Token: 0x04004DAE RID: 19886
			public static readonly StringName _compendiumButton = "_compendiumButton";

			/// <summary>
			/// Cached name for the '_giveUpButton' field.
			/// </summary>
			// Token: 0x04004DAF RID: 19887
			public static readonly StringName _giveUpButton = "_giveUpButton";

			/// <summary>
			/// Cached name for the '_disconnectButton' field.
			/// </summary>
			// Token: 0x04004DB0 RID: 19888
			public static readonly StringName _disconnectButton = "_disconnectButton";

			/// <summary>
			/// Cached name for the '_saveAndQuitButton' field.
			/// </summary>
			// Token: 0x04004DB1 RID: 19889
			public static readonly StringName _saveAndQuitButton = "_saveAndQuitButton";

			/// <summary>
			/// Cached name for the '_pausedLabel' field.
			/// </summary>
			// Token: 0x04004DB2 RID: 19890
			public static readonly StringName _pausedLabel = "_pausedLabel";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001478 RID: 5240
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
