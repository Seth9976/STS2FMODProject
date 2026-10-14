using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	// Token: 0x02000395 RID: 917
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NRemoteLobbyPlayer.cs")]
	public class NRemoteLobbyPlayer : Control
	{
		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060036DD RID: 14045 RVA: 0x0019B7A7 File Offset: 0x001999A7
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NRemoteLobbyPlayer._scenePath);
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x060036DE RID: 14046 RVA: 0x0019B7B3 File Offset: 0x001999B3
		public ulong PlayerId
		{
			get
			{
				return this._playerId;
			}
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x0019B7BC File Offset: 0x001999BC
		public static NRemoteLobbyPlayer Create(LobbyPlayer player, PlatformType platform, bool isSingleplayer)
		{
			NRemoteLobbyPlayer nremoteLobbyPlayer = PreloadManager.Cache.GetScene(NRemoteLobbyPlayer._scenePath).Instantiate<NRemoteLobbyPlayer>(0L);
			nremoteLobbyPlayer._playerId = player.id;
			nremoteLobbyPlayer._platform = platform;
			nremoteLobbyPlayer._isSingleplayer = isSingleplayer;
			nremoteLobbyPlayer._character = player.character;
			nremoteLobbyPlayer._isReady = player.isReady;
			nremoteLobbyPlayer._isConnected = true;
			return nremoteLobbyPlayer;
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x0019B81C File Offset: 0x00199A1C
		public static NRemoteLobbyPlayer Create(LoadRunLobby runLobby, ulong playerId, PlatformType platform, bool isSingleplayer)
		{
			NRemoteLobbyPlayer nremoteLobbyPlayer = PreloadManager.Cache.GetScene(NRemoteLobbyPlayer._scenePath).Instantiate<NRemoteLobbyPlayer>(0L);
			nremoteLobbyPlayer._playerId = playerId;
			nremoteLobbyPlayer._isSingleplayer = isSingleplayer;
			nremoteLobbyPlayer._platform = runLobby.NetService.Platform;
			nremoteLobbyPlayer._character = ModelDb.GetById<CharacterModel>(runLobby.Run.Players.First<SerializablePlayer>((SerializablePlayer p) => p.NetId == playerId).CharacterId);
			nremoteLobbyPlayer._isReady = runLobby.IsPlayerReady(playerId);
			nremoteLobbyPlayer._isConnected = runLobby.ConnectedPlayerIds.Contains(playerId);
			return nremoteLobbyPlayer;
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x0019B8C8 File Offset: 0x00199AC8
		public override void _Ready()
		{
			this._nameplateLabel = base.GetNode<MegaLabel>("%NameplateLabel");
			this._characterLabel = base.GetNode<MegaLabel>("%CharacterLabel");
			this._characterIcon = base.GetNode<TextureRect>("%CharacterIcon");
			this._readyIndicator = base.GetNode<TextureRect>("%ReadyIndicator");
			this._disconnectedIndicator = base.GetNode<TextureRect>("%DisconnectedIndicator");
			if (!this._isSingleplayer)
			{
				this._nameplateLabel.SetTextAutoSize(PlatformUtil.GetPlayerNameRaw(this._platform, this._playerId));
			}
			else
			{
				this._characterLabel.SetTextAutoSize(string.Empty);
			}
			this.RefreshVisuals();
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x0019B97F File Offset: 0x00199B7F
		public void OnPlayerChanged(LobbyPlayer lobbyPlayer)
		{
			this._playerId = lobbyPlayer.id;
			this.SetCharacter(lobbyPlayer.character);
			this._isReady = lobbyPlayer.isReady;
			this._isConnected = true;
			this.RefreshVisuals();
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x0019B9B4 File Offset: 0x00199BB4
		public void OnPlayerChanged(LoadRunLobby runLobby, ulong playerId)
		{
			SerializablePlayer serializablePlayer = runLobby.Run.Players.First<SerializablePlayer>((SerializablePlayer p) => p.NetId == playerId);
			this.SetCharacter(ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId));
			this._isReady = runLobby.IsPlayerReady(playerId);
			this._isConnected = runLobby.ConnectedPlayerIds.Contains(playerId);
			this.RefreshVisuals();
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x0019BA2C File Offset: 0x00199C2C
		private void RefreshVisuals()
		{
			if (this._isSingleplayer)
			{
				this._nameplateLabel.SetTextAutoSize(this._character.Title.GetFormattedText());
			}
			else
			{
				this._characterLabel.SetTextAutoSize(this._character.Title.GetFormattedText());
			}
			this._characterIcon.Texture = this._character.IconTexture;
			this._readyIndicator.Visible = this._isReady;
			this._disconnectedIndicator.Visible = !this._isConnected;
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x0019BAB4 File Offset: 0x00199CB4
		private void SetCharacter(CharacterModel character)
		{
			if (this._character != character)
			{
				ScreenPunchInstance shake = this._shake;
				if (shake != null)
				{
					shake.Cancel();
				}
				this.CancelShake();
				this._originalPosition = new Vector2?(base.Position);
				this._shake = new ScreenPunchInstance(3f, 0.4000000059604645, 90f);
				this._character = character;
			}
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x0019BB17 File Offset: 0x00199D17
		public void CancelShake()
		{
			this._shake = null;
			if (this._originalPosition != null)
			{
				base.Position = this._originalPosition.Value;
				this._originalPosition = null;
			}
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x0019BB4C File Offset: 0x00199D4C
		public override void _Process(double delta)
		{
			ScreenPunchInstance shake = this._shake;
			if (shake != null && !shake.IsDone)
			{
				ScreenPunchInstance shake2 = this._shake;
				Vector2 vector = ((shake2 != null) ? shake2.Update(delta) : Vector2.Zero);
				base.Position = this._originalPosition.Value + vector;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060036E8 RID: 14056 RVA: 0x0019BB9C File Offset: 0x00199D9C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NRemoteLobbyPlayer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteLobbyPlayer.MethodName.RefreshVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteLobbyPlayer.MethodName.CancelShake, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteLobbyPlayer.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036E9 RID: 14057 RVA: 0x0019BC94 File Offset: 0x00199E94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRemoteLobbyPlayer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteLobbyPlayer.MethodName.RefreshVisuals && args.Count == 0)
			{
				this.RefreshVisuals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteLobbyPlayer.MethodName.CancelShake && args.Count == 0)
			{
				this.CancelShake();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteLobbyPlayer.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060036EA RID: 14058 RVA: 0x0019BD4C File Offset: 0x00199F4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRemoteLobbyPlayer.MethodName._Ready || (ref method) == NRemoteLobbyPlayer.MethodName.RefreshVisuals || (ref method) == NRemoteLobbyPlayer.MethodName.CancelShake || (ref method) == NRemoteLobbyPlayer.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060036EB RID: 14059 RVA: 0x0019BD9C File Offset: 0x00199F9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._characterIcon)
			{
				this._characterIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._readyIndicator)
			{
				this._readyIndicator = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._disconnectedIndicator)
			{
				this._disconnectedIndicator = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._nameplateLabel)
			{
				this._nameplateLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._characterLabel)
			{
				this._characterLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._platform)
			{
				this._platform = VariantUtils.ConvertTo<PlatformType>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._isSingleplayer)
			{
				this._isSingleplayer = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._playerId)
			{
				this._playerId = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._isReady)
			{
				this._isReady = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._isConnected)
			{
				this._isConnected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060036EC RID: 14060 RVA: 0x0019BEC0 File Offset: 0x0019A0C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRemoteLobbyPlayer.PropertyName.PlayerId)
			{
				ulong playerId = this.PlayerId;
				value = VariantUtils.CreateFrom<ulong>(ref playerId);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._characterIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._characterIcon);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._readyIndicator)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._readyIndicator);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._disconnectedIndicator)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._disconnectedIndicator);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._nameplateLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._nameplateLabel);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._characterLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._characterLabel);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._platform)
			{
				value = VariantUtils.CreateFrom<PlatformType>(ref this._platform);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._isSingleplayer)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isSingleplayer);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._playerId)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._playerId);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._isReady)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isReady);
				return true;
			}
			if ((ref name) == NRemoteLobbyPlayer.PropertyName._isConnected)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isConnected);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060036ED RID: 14061 RVA: 0x0019C038 File Offset: 0x0019A238
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRemoteLobbyPlayer.PropertyName._characterIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteLobbyPlayer.PropertyName._readyIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteLobbyPlayer.PropertyName._disconnectedIndicator, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteLobbyPlayer.PropertyName._nameplateLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteLobbyPlayer.PropertyName._characterLabel, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRemoteLobbyPlayer.PropertyName._platform, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRemoteLobbyPlayer.PropertyName._isSingleplayer, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRemoteLobbyPlayer.PropertyName._playerId, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRemoteLobbyPlayer.PropertyName._isReady, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRemoteLobbyPlayer.PropertyName._isConnected, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRemoteLobbyPlayer.PropertyName.PlayerId, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036EE RID: 14062 RVA: 0x0019C1B4 File Offset: 0x0019A3B4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRemoteLobbyPlayer.PropertyName._characterIcon, Variant.From<TextureRect>(ref this._characterIcon));
			info.AddProperty(NRemoteLobbyPlayer.PropertyName._readyIndicator, Variant.From<Control>(ref this._readyIndicator));
			info.AddProperty(NRemoteLobbyPlayer.PropertyName._disconnectedIndicator, Variant.From<Control>(ref this._disconnectedIndicator));
			info.AddProperty(NRemoteLobbyPlayer.PropertyName._nameplateLabel, Variant.From<MegaLabel>(ref this._nameplateLabel));
			info.AddProperty(NRemoteLobbyPlayer.PropertyName._characterLabel, Variant.From<MegaLabel>(ref this._characterLabel));
			info.AddProperty(NRemoteLobbyPlayer.PropertyName._platform, Variant.From<PlatformType>(ref this._platform));
			info.AddProperty(NRemoteLobbyPlayer.PropertyName._isSingleplayer, Variant.From<bool>(ref this._isSingleplayer));
			info.AddProperty(NRemoteLobbyPlayer.PropertyName._playerId, Variant.From<ulong>(ref this._playerId));
			info.AddProperty(NRemoteLobbyPlayer.PropertyName._isReady, Variant.From<bool>(ref this._isReady));
			info.AddProperty(NRemoteLobbyPlayer.PropertyName._isConnected, Variant.From<bool>(ref this._isConnected));
		}

		/// <inheritdoc />
		// Token: 0x060036EF RID: 14063 RVA: 0x0019C2A4 File Offset: 0x0019A4A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._characterIcon, ref variant))
			{
				this._characterIcon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._readyIndicator, ref variant2))
			{
				this._readyIndicator = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._disconnectedIndicator, ref variant3))
			{
				this._disconnectedIndicator = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._nameplateLabel, ref variant4))
			{
				this._nameplateLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._characterLabel, ref variant5))
			{
				this._characterLabel = variant5.As<MegaLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._platform, ref variant6))
			{
				this._platform = variant6.As<PlatformType>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._isSingleplayer, ref variant7))
			{
				this._isSingleplayer = variant7.As<bool>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._playerId, ref variant8))
			{
				this._playerId = variant8.As<ulong>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._isReady, ref variant9))
			{
				this._isReady = variant9.As<bool>();
			}
			Variant variant10;
			if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._isConnected, ref variant10))
			{
				this._isConnected = variant10.As<bool>();
			}
		}

		// Token: 0x04001539 RID: 5433
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/remote_lobby_player");

		// Token: 0x0400153A RID: 5434
		private TextureRect _characterIcon;

		// Token: 0x0400153B RID: 5435
		private Control _readyIndicator;

		// Token: 0x0400153C RID: 5436
		private Control _disconnectedIndicator;

		// Token: 0x0400153D RID: 5437
		private MegaLabel _nameplateLabel;

		// Token: 0x0400153E RID: 5438
		private MegaLabel _characterLabel;

		// Token: 0x0400153F RID: 5439
		private PlatformType _platform;

		// Token: 0x04001540 RID: 5440
		private bool _isSingleplayer;

		// Token: 0x04001541 RID: 5441
		[Nullable(2)]
		private ScreenPunchInstance _shake;

		// Token: 0x04001542 RID: 5442
		private Vector2? _originalPosition;

		// Token: 0x04001543 RID: 5443
		private ulong _playerId;

		// Token: 0x04001544 RID: 5444
		private CharacterModel _character;

		// Token: 0x04001545 RID: 5445
		private bool _isReady;

		// Token: 0x04001546 RID: 5446
		private bool _isConnected;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001744 RID: 5956
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005AC6 RID: 23238
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshVisuals' method.
			/// </summary>
			// Token: 0x04005AC7 RID: 23239
			public static readonly StringName RefreshVisuals = "RefreshVisuals";

			/// <summary>
			/// Cached name for the 'CancelShake' method.
			/// </summary>
			// Token: 0x04005AC8 RID: 23240
			public static readonly StringName CancelShake = "CancelShake";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005AC9 RID: 23241
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001745 RID: 5957
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'PlayerId' property.
			/// </summary>
			// Token: 0x04005ACA RID: 23242
			public static readonly StringName PlayerId = "PlayerId";

			/// <summary>
			/// Cached name for the '_characterIcon' field.
			/// </summary>
			// Token: 0x04005ACB RID: 23243
			public static readonly StringName _characterIcon = "_characterIcon";

			/// <summary>
			/// Cached name for the '_readyIndicator' field.
			/// </summary>
			// Token: 0x04005ACC RID: 23244
			public static readonly StringName _readyIndicator = "_readyIndicator";

			/// <summary>
			/// Cached name for the '_disconnectedIndicator' field.
			/// </summary>
			// Token: 0x04005ACD RID: 23245
			public static readonly StringName _disconnectedIndicator = "_disconnectedIndicator";

			/// <summary>
			/// Cached name for the '_nameplateLabel' field.
			/// </summary>
			// Token: 0x04005ACE RID: 23246
			public static readonly StringName _nameplateLabel = "_nameplateLabel";

			/// <summary>
			/// Cached name for the '_characterLabel' field.
			/// </summary>
			// Token: 0x04005ACF RID: 23247
			public static readonly StringName _characterLabel = "_characterLabel";

			/// <summary>
			/// Cached name for the '_platform' field.
			/// </summary>
			// Token: 0x04005AD0 RID: 23248
			public static readonly StringName _platform = "_platform";

			/// <summary>
			/// Cached name for the '_isSingleplayer' field.
			/// </summary>
			// Token: 0x04005AD1 RID: 23249
			public static readonly StringName _isSingleplayer = "_isSingleplayer";

			/// <summary>
			/// Cached name for the '_playerId' field.
			/// </summary>
			// Token: 0x04005AD2 RID: 23250
			public static readonly StringName _playerId = "_playerId";

			/// <summary>
			/// Cached name for the '_isReady' field.
			/// </summary>
			// Token: 0x04005AD3 RID: 23251
			public static readonly StringName _isReady = "_isReady";

			/// <summary>
			/// Cached name for the '_isConnected' field.
			/// </summary>
			// Token: 0x04005AD4 RID: 23252
			public static readonly StringName _isConnected = "_isConnected";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001746 RID: 5958
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
