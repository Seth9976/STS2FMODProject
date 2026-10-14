using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MegaCrit.Sts2.Core.Multiplayer.Messages;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Checksums;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	// Token: 0x02000453 RID: 1107
	public static class INetMessageSubtypes
	{
		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06004550 RID: 17744 RVA: 0x00215A6B File Offset: 0x00213C6B
		public static int Count
		{
			get
			{
				return 52;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06004551 RID: 17745 RVA: 0x00215A6F File Offset: 0x00213C6F
		public static IReadOnlyList<Type> All
		{
			get
			{
				return INetMessageSubtypes._subtypes;
			}
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x00215A76 File Offset: 0x00213C76
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
		public static Type Get(int i)
		{
			return INetMessageSubtypes._subtypes[i];
		}

		// Token: 0x04001AE2 RID: 6882
		private static readonly Type _t0 = typeof(ActionEnqueuedMessage);

		// Token: 0x04001AE3 RID: 6883
		private static readonly Type _t1 = typeof(CardRemovedMessage);

		// Token: 0x04001AE4 RID: 6884
		private static readonly Type _t2 = typeof(ChecksumDataMessage);

		// Token: 0x04001AE5 RID: 6885
		private static readonly Type _t3 = typeof(StateDivergenceMessage);

		// Token: 0x04001AE6 RID: 6886
		private static readonly Type _t4 = typeof(CrystalSphereRewardsMessage);

		// Token: 0x04001AE7 RID: 6887
		private static readonly Type _t5 = typeof(ClearMapDrawingsMessage);

		// Token: 0x04001AE8 RID: 6888
		private static readonly Type _t6 = typeof(EndTurnPingMessage);

		// Token: 0x04001AE9 RID: 6889
		private static readonly Type _t7 = typeof(MapDrawingMessage);

		// Token: 0x04001AEA RID: 6890
		private static readonly Type _t8 = typeof(MapDrawingModeChangedMessage);

		// Token: 0x04001AEB RID: 6891
		private static readonly Type _t9 = typeof(MapPingMessage);

		// Token: 0x04001AEC RID: 6892
		private static readonly Type _t10 = typeof(ReactionMessage);

		// Token: 0x04001AED RID: 6893
		private static readonly Type _t11 = typeof(RestSiteOptionHoveredMessage);

		// Token: 0x04001AEE RID: 6894
		private static readonly Type _t12 = typeof(HookActionEnqueuedMessage);

		// Token: 0x04001AEF RID: 6895
		private static readonly Type _t13 = typeof(MerchantCardRemovalMessage);

		// Token: 0x04001AF0 RID: 6896
		private static readonly Type _t14 = typeof(PlayerChoiceMessage);

		// Token: 0x04001AF1 RID: 6897
		private static readonly Type _t15 = typeof(RequestEnqueueActionMessage);

		// Token: 0x04001AF2 RID: 6898
		private static readonly Type _t16 = typeof(RequestEnqueueHookActionMessage);

		// Token: 0x04001AF3 RID: 6899
		private static readonly Type _t17 = typeof(RequestResumeActionAfterPlayerChoiceMessage);

		// Token: 0x04001AF4 RID: 6900
		private static readonly Type _t18 = typeof(ResumeActionAfterPlayerChoiceMessage);

		// Token: 0x04001AF5 RID: 6901
		private static readonly Type _t19 = typeof(RunAbandonedMessage);

		// Token: 0x04001AF6 RID: 6902
		private static readonly Type _t20 = typeof(GoldLostMessage);

		// Token: 0x04001AF7 RID: 6903
		private static readonly Type _t21 = typeof(OptionIndexChosenMessage);

		// Token: 0x04001AF8 RID: 6904
		private static readonly Type _t22 = typeof(PeerInputMessage);

		// Token: 0x04001AF9 RID: 6905
		private static readonly Type _t23 = typeof(RestSiteSkippedMessage);

		// Token: 0x04001AFA RID: 6906
		private static readonly Type _t24 = typeof(RewardObtainedMessage);

		// Token: 0x04001AFB RID: 6907
		private static readonly Type _t25 = typeof(RewardSelectedMessage);

		// Token: 0x04001AFC RID: 6908
		private static readonly Type _t26 = typeof(RewardSetSkippedMessage);

		// Token: 0x04001AFD RID: 6909
		private static readonly Type _t27 = typeof(SharedEventOptionChosenMessage);

		// Token: 0x04001AFE RID: 6910
		private static readonly Type _t28 = typeof(VotedForSharedEventOptionMessage);

		// Token: 0x04001AFF RID: 6911
		private static readonly Type _t29 = typeof(SyncPlayerDataMessage);

		// Token: 0x04001B00 RID: 6912
		private static readonly Type _t30 = typeof(SyncRngMessage);

		// Token: 0x04001B01 RID: 6913
		private static readonly Type _t31 = typeof(TreasureChestOpenedMessage);

		// Token: 0x04001B02 RID: 6914
		private static readonly Type _t32 = typeof(HeartbeatRequestMessage);

		// Token: 0x04001B03 RID: 6915
		private static readonly Type _t33 = typeof(HeartbeatResponseMessage);

		// Token: 0x04001B04 RID: 6916
		private static readonly Type _t34 = typeof(ClientLoadJoinRequestMessage);

		// Token: 0x04001B05 RID: 6917
		private static readonly Type _t35 = typeof(ClientLoadJoinResponseMessage);

		// Token: 0x04001B06 RID: 6918
		private static readonly Type _t36 = typeof(ClientLobbyJoinRequestMessage);

		// Token: 0x04001B07 RID: 6919
		private static readonly Type _t37 = typeof(ClientLobbyJoinResponseMessage);

		// Token: 0x04001B08 RID: 6920
		private static readonly Type _t38 = typeof(ClientRejoinRequestMessage);

		// Token: 0x04001B09 RID: 6921
		private static readonly Type _t39 = typeof(ClientRejoinResponseMessage);

		// Token: 0x04001B0A RID: 6922
		private static readonly Type _t40 = typeof(InitialGameInfoMessage);

		// Token: 0x04001B0B RID: 6923
		private static readonly Type _t41 = typeof(LobbyAscensionChangedMessage);

		// Token: 0x04001B0C RID: 6924
		private static readonly Type _t42 = typeof(LobbyBeginLoadedRunMessage);

		// Token: 0x04001B0D RID: 6925
		private static readonly Type _t43 = typeof(LobbyBeginRunMessage);

		// Token: 0x04001B0E RID: 6926
		private static readonly Type _t44 = typeof(LobbyModifiersChangedMessage);

		// Token: 0x04001B0F RID: 6927
		private static readonly Type _t45 = typeof(LobbyPlayerChangedCharacterMessage);

		// Token: 0x04001B10 RID: 6928
		private static readonly Type _t46 = typeof(LobbyPlayerSetReadyMessage);

		// Token: 0x04001B11 RID: 6929
		private static readonly Type _t47 = typeof(LobbySeedChangedMessage);

		// Token: 0x04001B12 RID: 6930
		private static readonly Type _t48 = typeof(PlayerJoinedMessage);

		// Token: 0x04001B13 RID: 6931
		private static readonly Type _t49 = typeof(PlayerLeftMessage);

		// Token: 0x04001B14 RID: 6932
		private static readonly Type _t50 = typeof(PlayerReconnectedMessage);

		// Token: 0x04001B15 RID: 6933
		private static readonly Type _t51 = typeof(PlayerRejoinedMessage);

		// Token: 0x04001B16 RID: 6934
		private static readonly Type[] _subtypes = new Type[]
		{
			INetMessageSubtypes._t0,
			INetMessageSubtypes._t1,
			INetMessageSubtypes._t2,
			INetMessageSubtypes._t3,
			INetMessageSubtypes._t4,
			INetMessageSubtypes._t5,
			INetMessageSubtypes._t6,
			INetMessageSubtypes._t7,
			INetMessageSubtypes._t8,
			INetMessageSubtypes._t9,
			INetMessageSubtypes._t10,
			INetMessageSubtypes._t11,
			INetMessageSubtypes._t12,
			INetMessageSubtypes._t13,
			INetMessageSubtypes._t14,
			INetMessageSubtypes._t15,
			INetMessageSubtypes._t16,
			INetMessageSubtypes._t17,
			INetMessageSubtypes._t18,
			INetMessageSubtypes._t19,
			INetMessageSubtypes._t20,
			INetMessageSubtypes._t21,
			INetMessageSubtypes._t22,
			INetMessageSubtypes._t23,
			INetMessageSubtypes._t24,
			INetMessageSubtypes._t25,
			INetMessageSubtypes._t26,
			INetMessageSubtypes._t27,
			INetMessageSubtypes._t28,
			INetMessageSubtypes._t29,
			INetMessageSubtypes._t30,
			INetMessageSubtypes._t31,
			INetMessageSubtypes._t32,
			INetMessageSubtypes._t33,
			INetMessageSubtypes._t34,
			INetMessageSubtypes._t35,
			INetMessageSubtypes._t36,
			INetMessageSubtypes._t37,
			INetMessageSubtypes._t38,
			INetMessageSubtypes._t39,
			INetMessageSubtypes._t40,
			INetMessageSubtypes._t41,
			INetMessageSubtypes._t42,
			INetMessageSubtypes._t43,
			INetMessageSubtypes._t44,
			INetMessageSubtypes._t45,
			INetMessageSubtypes._t46,
			INetMessageSubtypes._t47,
			INetMessageSubtypes._t48,
			INetMessageSubtypes._t49,
			INetMessageSubtypes._t50,
			INetMessageSubtypes._t51
		};
	}
}
