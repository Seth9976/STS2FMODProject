using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	// Token: 0x02000396 RID: 918
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NRemoteLobbyPlayerContainer.cs")]
	public class NRemoteLobbyPlayerContainer : Control
	{
		// Token: 0x060036F2 RID: 14066 RVA: 0x0019C3EC File Offset: 0x0019A5EC
		public override void _Ready()
		{
			this._soloLabel = base.GetNode<MegaLabel>("%SoloLabel");
			this._container = base.GetNode<Container>("Container");
			this._inviteButton = base.GetNode<NInvitePlayersButton>("%InviteButton");
			this._soloLabel.SetTextAutoSize(new LocString("main_menu_ui", "MULTIPLAYER_CHAR_SELECT.SOLO").GetFormattedText());
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x0019C45C File Offset: 0x0019A65C
		[NullableContext(1)]
		public void Initialize(StartRunLobby lobby, bool displayLocalPlayer)
		{
			foreach (NRemoteLobbyPlayer nremoteLobbyPlayer in this._nodes)
			{
				nremoteLobbyPlayer.QueueFreeSafely();
			}
			this._nodes.Clear();
			if (!lobby.NetService.Type.IsMultiplayer())
			{
				return;
			}
			this._displayLocalPlayer = displayLocalPlayer;
			this._inviteButton.Initialize(lobby);
			this._lobby = lobby;
			foreach (LobbyPlayer lobbyPlayer in this._lobby.Players)
			{
				this.OnPlayerConnected(lobbyPlayer);
			}
			this.RefreshSoloLabelVisibility();
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x0019C534 File Offset: 0x0019A734
		public void OnPlayerConnected(LobbyPlayer player)
		{
			StartRunLobby lobby = this._lobby;
			if (lobby == null)
			{
				return;
			}
			if (player.id == lobby.LocalPlayer.id && !this._displayLocalPlayer)
			{
				return;
			}
			NRemoteLobbyPlayer nremoteLobbyPlayer = NRemoteLobbyPlayer.Create(player, lobby.NetService.Platform, lobby.NetService.Type == NetGameType.Singleplayer);
			this._container.AddChildSafely(nremoteLobbyPlayer);
			this._container.MoveChildSafely(this._inviteButton.GetParent(), this._container.GetChildCount(false) - 1);
			this._nodes.Add(nremoteLobbyPlayer);
			this.RefreshSoloLabelVisibility();
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x0019C5CC File Offset: 0x0019A7CC
		public void OnPlayerDisconnected(LobbyPlayer player)
		{
			if (this._lobby == null)
			{
				return;
			}
			int num = this._nodes.FindIndex((NRemoteLobbyPlayer p) => p.PlayerId == player.id);
			if (num >= 0)
			{
				this._container.RemoveChildSafely(this._nodes[num]);
				this._nodes.RemoveAt(num);
				foreach (NRemoteLobbyPlayer nremoteLobbyPlayer in this._nodes)
				{
					nremoteLobbyPlayer.CancelShake();
				}
			}
			this.RefreshSoloLabelVisibility();
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x0019C67C File Offset: 0x0019A87C
		public void OnPlayerChanged(LobbyPlayer player)
		{
			StartRunLobby lobby = this._lobby;
			if (lobby == null)
			{
				return;
			}
			if (player.id == lobby.LocalPlayer.id && !this._displayLocalPlayer)
			{
				return;
			}
			NRemoteLobbyPlayer nremoteLobbyPlayer = this._nodes.FirstOrDefault<NRemoteLobbyPlayer>((NRemoteLobbyPlayer p) => p.PlayerId == player.id);
			if (nremoteLobbyPlayer == null)
			{
				return;
			}
			nremoteLobbyPlayer.OnPlayerChanged(player);
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x0019C6EC File Offset: 0x0019A8EC
		private void RefreshSoloLabelVisibility()
		{
			StartRunLobby lobby = this._lobby;
			this._soloLabel.Visible = (lobby == null || lobby.NetService.Type != NetGameType.Singleplayer) && lobby != null && lobby.Players.Count == 1;
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x0019C73C File Offset: 0x0019A93C
		public void Cleanup()
		{
			foreach (NRemoteLobbyPlayer nremoteLobbyPlayer in this._nodes)
			{
				nremoteLobbyPlayer.QueueFreeSafely();
			}
			this._nodes.Clear();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060036F9 RID: 14073 RVA: 0x0019C79C File Offset: 0x0019A99C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NRemoteLobbyPlayerContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteLobbyPlayerContainer.MethodName.RefreshSoloLabelVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteLobbyPlayerContainer.MethodName.Cleanup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036FA RID: 14074 RVA: 0x0019C840 File Offset: 0x0019AA40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRemoteLobbyPlayerContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteLobbyPlayerContainer.MethodName.RefreshSoloLabelVisibility && args.Count == 0)
			{
				this.RefreshSoloLabelVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteLobbyPlayerContainer.MethodName.Cleanup && args.Count == 0)
			{
				this.Cleanup();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060036FB RID: 14075 RVA: 0x0019C8C5 File Offset: 0x0019AAC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRemoteLobbyPlayerContainer.MethodName._Ready || (ref method) == NRemoteLobbyPlayerContainer.MethodName.RefreshSoloLabelVisibility || (ref method) == NRemoteLobbyPlayerContainer.MethodName.Cleanup || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060036FC RID: 14076 RVA: 0x0019C8FC File Offset: 0x0019AAFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRemoteLobbyPlayerContainer.PropertyName._inviteButton)
			{
				this._inviteButton = VariantUtils.ConvertTo<NInvitePlayersButton>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayerContainer.PropertyName._soloLabel)
			{
				this._soloLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayerContainer.PropertyName._container)
			{
				this._container = VariantUtils.ConvertTo<Container>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayerContainer.PropertyName._displayLocalPlayer)
			{
				this._displayLocalPlayer = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060036FD RID: 14077 RVA: 0x0019C980 File Offset: 0x0019AB80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRemoteLobbyPlayerContainer.PropertyName._inviteButton)
			{
				value = VariantUtils.CreateFrom<NInvitePlayersButton>(ref this._inviteButton);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayerContainer.PropertyName._soloLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._soloLabel);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayerContainer.PropertyName._container)
			{
				value = VariantUtils.CreateFrom<Container>(ref this._container);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayerContainer.PropertyName._displayLocalPlayer)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._displayLocalPlayer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060036FE RID: 14078 RVA: 0x0019CA18 File Offset: 0x0019AC18
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRemoteLobbyPlayerContainer.PropertyName._inviteButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteLobbyPlayerContainer.PropertyName._soloLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteLobbyPlayerContainer.PropertyName._container, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRemoteLobbyPlayerContainer.PropertyName._displayLocalPlayer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036FF RID: 14079 RVA: 0x0019CAB0 File Offset: 0x0019ACB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRemoteLobbyPlayerContainer.PropertyName._inviteButton, Variant.From<NInvitePlayersButton>(ref this._inviteButton));
			info.AddProperty(NRemoteLobbyPlayerContainer.PropertyName._soloLabel, Variant.From<MegaLabel>(ref this._soloLabel));
			info.AddProperty(NRemoteLobbyPlayerContainer.PropertyName._container, Variant.From<Container>(ref this._container));
			info.AddProperty(NRemoteLobbyPlayerContainer.PropertyName._displayLocalPlayer, Variant.From<bool>(ref this._displayLocalPlayer));
		}

		/// <inheritdoc />
		// Token: 0x06003700 RID: 14080 RVA: 0x0019CB1C File Offset: 0x0019AD1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRemoteLobbyPlayerContainer.PropertyName._inviteButton, ref variant))
			{
				this._inviteButton = variant.As<NInvitePlayersButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRemoteLobbyPlayerContainer.PropertyName._soloLabel, ref variant2))
			{
				this._soloLabel = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRemoteLobbyPlayerContainer.PropertyName._container, ref variant3))
			{
				this._container = variant3.As<Container>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRemoteLobbyPlayerContainer.PropertyName._displayLocalPlayer, ref variant4))
			{
				this._displayLocalPlayer = variant4.As<bool>();
			}
		}

		// Token: 0x04001547 RID: 5447
		[Nullable(1)]
		private readonly List<NRemoteLobbyPlayer> _nodes = new List<NRemoteLobbyPlayer>();

		// Token: 0x04001548 RID: 5448
		[Nullable(2)]
		private StartRunLobby _lobby;

		// Token: 0x04001549 RID: 5449
		[Nullable(1)]
		private NInvitePlayersButton _inviteButton;

		// Token: 0x0400154A RID: 5450
		[Nullable(1)]
		private MegaLabel _soloLabel;

		// Token: 0x0400154B RID: 5451
		[Nullable(1)]
		private Container _container;

		// Token: 0x0400154C RID: 5452
		private bool _displayLocalPlayer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001749 RID: 5961
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005AD7 RID: 23255
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshSoloLabelVisibility' method.
			/// </summary>
			// Token: 0x04005AD8 RID: 23256
			public static readonly StringName RefreshSoloLabelVisibility = "RefreshSoloLabelVisibility";

			/// <summary>
			/// Cached name for the 'Cleanup' method.
			/// </summary>
			// Token: 0x04005AD9 RID: 23257
			public static readonly StringName Cleanup = "Cleanup";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200174A RID: 5962
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_inviteButton' field.
			/// </summary>
			// Token: 0x04005ADA RID: 23258
			public static readonly StringName _inviteButton = "_inviteButton";

			/// <summary>
			/// Cached name for the '_soloLabel' field.
			/// </summary>
			// Token: 0x04005ADB RID: 23259
			public static readonly StringName _soloLabel = "_soloLabel";

			/// <summary>
			/// Cached name for the '_container' field.
			/// </summary>
			// Token: 0x04005ADC RID: 23260
			public static readonly StringName _container = "_container";

			/// <summary>
			/// Cached name for the '_displayLocalPlayer' field.
			/// </summary>
			// Token: 0x04005ADD RID: 23261
			public static readonly StringName _displayLocalPlayer = "_displayLocalPlayer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200174B RID: 5963
		public class SignalName : Control.SignalName
		{
		}
	}
}
