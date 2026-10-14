using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	// Token: 0x02000394 RID: 916
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NRemoteLoadLobbyPlayerContainer.cs")]
	public class NRemoteLoadLobbyPlayerContainer : Control
	{
		// Token: 0x060036CE RID: 14030 RVA: 0x0019B141 File Offset: 0x00199341
		public override void _Ready()
		{
			this._othersLabel = base.GetNodeOrNull<MegaLabel>("OthersLabel");
			this._container = base.GetNode<Control>("Container");
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x0019B170 File Offset: 0x00199370
		[NullableContext(1)]
		public void Initialize(LoadRunLobby runLobby, bool displayLocalPlayer)
		{
			this._lobby = runLobby;
			if (this._othersLabel != null)
			{
				LocString locString = new LocString("main_menu_ui", "MULTIPLAYER_LOAD_MENU.OTHERS");
				locString.Add("others", runLobby.Run.Players.Count - 1);
				this._othersLabel.Text = locString.GetFormattedText();
			}
			foreach (SerializablePlayer serializablePlayer in runLobby.Run.Players)
			{
				if (serializablePlayer.NetId != this._lobby.NetService.NetId || displayLocalPlayer)
				{
					NRemoteLobbyPlayer nremoteLobbyPlayer = NRemoteLobbyPlayer.Create(runLobby, serializablePlayer.NetId, runLobby.NetService.Platform, runLobby.NetService.Type == NetGameType.Singleplayer);
					this._container.AddChildSafely(nremoteLobbyPlayer);
					this._nodes.Add(nremoteLobbyPlayer);
				}
			}
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x0019B26C File Offset: 0x0019946C
		public void OnPlayerConnected(ulong playerId)
		{
			this.OnPlayerChanged(playerId);
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x0019B275 File Offset: 0x00199475
		public void OnPlayerDisconnected(ulong playerId)
		{
			this.OnPlayerChanged(playerId);
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x0019B280 File Offset: 0x00199480
		public void OnPlayerChanged(ulong playerId)
		{
			if (this._lobby == null)
			{
				return;
			}
			if (playerId == this._lobby.NetService.NetId)
			{
				return;
			}
			int num = this._nodes.FindIndex((NRemoteLobbyPlayer p) => p.PlayerId == playerId);
			if (num >= 0)
			{
				NRemoteLobbyPlayer nremoteLobbyPlayer = this._nodes[num];
				nremoteLobbyPlayer.OnPlayerChanged(this._lobby, playerId);
			}
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x0019B2F8 File Offset: 0x001994F8
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
		// Token: 0x060036D4 RID: 14036 RVA: 0x0019B358 File Offset: 0x00199558
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NRemoteLoadLobbyPlayerContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerConnected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerDisconnected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteLoadLobbyPlayerContainer.MethodName.Cleanup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036D5 RID: 14037 RVA: 0x0019B4C8 File Offset: 0x001996C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRemoteLoadLobbyPlayerContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerConnected && args.Count == 1)
			{
				this.OnPlayerConnected(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerDisconnected && args.Count == 1)
			{
				this.OnPlayerDisconnected(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerChanged && args.Count == 1)
			{
				this.OnPlayerChanged(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteLoadLobbyPlayerContainer.MethodName.Cleanup && args.Count == 0)
			{
				this.Cleanup();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060036D6 RID: 14038 RVA: 0x0019B5C4 File Offset: 0x001997C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRemoteLoadLobbyPlayerContainer.MethodName._Ready || (ref method) == NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerConnected || (ref method) == NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerDisconnected || (ref method) == NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerChanged || (ref method) == NRemoteLoadLobbyPlayerContainer.MethodName.Cleanup || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060036D7 RID: 14039 RVA: 0x0019B623 File Offset: 0x00199823
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRemoteLoadLobbyPlayerContainer.PropertyName._othersLabel)
			{
				this._othersLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLoadLobbyPlayerContainer.PropertyName._container)
			{
				this._container = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060036D8 RID: 14040 RVA: 0x0019B664 File Offset: 0x00199864
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRemoteLoadLobbyPlayerContainer.PropertyName._othersLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._othersLabel);
				return true;
			}
			if ((ref name) == NRemoteLoadLobbyPlayerContainer.PropertyName._container)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._container);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060036D9 RID: 14041 RVA: 0x0019B6BC File Offset: 0x001998BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRemoteLoadLobbyPlayerContainer.PropertyName._othersLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteLoadLobbyPlayerContainer.PropertyName._container, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036DA RID: 14042 RVA: 0x0019B712 File Offset: 0x00199912
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRemoteLoadLobbyPlayerContainer.PropertyName._othersLabel, Variant.From<MegaLabel>(ref this._othersLabel));
			info.AddProperty(NRemoteLoadLobbyPlayerContainer.PropertyName._container, Variant.From<Control>(ref this._container));
		}

		/// <inheritdoc />
		// Token: 0x060036DB RID: 14043 RVA: 0x0019B748 File Offset: 0x00199948
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRemoteLoadLobbyPlayerContainer.PropertyName._othersLabel, ref variant))
			{
				this._othersLabel = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRemoteLoadLobbyPlayerContainer.PropertyName._container, ref variant2))
			{
				this._container = variant2.As<Control>();
			}
		}

		// Token: 0x04001535 RID: 5429
		[Nullable(1)]
		private readonly List<NRemoteLobbyPlayer> _nodes = new List<NRemoteLobbyPlayer>();

		// Token: 0x04001536 RID: 5430
		[Nullable(2)]
		private LoadRunLobby _lobby;

		// Token: 0x04001537 RID: 5431
		[Nullable(2)]
		private MegaLabel _othersLabel;

		// Token: 0x04001538 RID: 5432
		[Nullable(1)]
		private Control _container;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001740 RID: 5952
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005ABE RID: 23230
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnPlayerConnected' method.
			/// </summary>
			// Token: 0x04005ABF RID: 23231
			public static readonly StringName OnPlayerConnected = "OnPlayerConnected";

			/// <summary>
			/// Cached name for the 'OnPlayerDisconnected' method.
			/// </summary>
			// Token: 0x04005AC0 RID: 23232
			public static readonly StringName OnPlayerDisconnected = "OnPlayerDisconnected";

			/// <summary>
			/// Cached name for the 'OnPlayerChanged' method.
			/// </summary>
			// Token: 0x04005AC1 RID: 23233
			public static readonly StringName OnPlayerChanged = "OnPlayerChanged";

			/// <summary>
			/// Cached name for the 'Cleanup' method.
			/// </summary>
			// Token: 0x04005AC2 RID: 23234
			public static readonly StringName Cleanup = "Cleanup";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001741 RID: 5953
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_othersLabel' field.
			/// </summary>
			// Token: 0x04005AC3 RID: 23235
			public static readonly StringName _othersLabel = "_othersLabel";

			/// <summary>
			/// Cached name for the '_container' field.
			/// </summary>
			// Token: 0x04005AC4 RID: 23236
			public static readonly StringName _container = "_container";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001742 RID: 5954
		public class SignalName : Control.SignalName
		{
		}
	}
}
