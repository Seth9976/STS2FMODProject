using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.GameActions
{
	/// <summary>
	/// Enqueued when a player picks a relic at a multiplayer treasure room.
	/// </summary>
	// Token: 0x02000C25 RID: 3109
	[NullableContext(1)]
	[Nullable(0)]
	public class PickRelicAction : GameAction
	{
		// Token: 0x17002323 RID: 8995
		// (get) Token: 0x06008064 RID: 32868 RVA: 0x002B8A06 File Offset: 0x002B6C06
		public override ulong OwnerId
		{
			get
			{
				return this._player.NetId;
			}
		}

		// Token: 0x17002324 RID: 8996
		// (get) Token: 0x06008065 RID: 32869 RVA: 0x002B8A13 File Offset: 0x002B6C13
		public override GameActionType ActionType
		{
			get
			{
				return GameActionType.NonCombat;
			}
		}

		/// <summary>
		/// Used to inject a relic picking synchronizer in tests.
		/// </summary>
		// Token: 0x17002325 RID: 8997
		// (get) Token: 0x06008066 RID: 32870 RVA: 0x002B8A16 File Offset: 0x002B6C16
		// (set) Token: 0x06008067 RID: 32871 RVA: 0x002B8A1E File Offset: 0x002B6C1E
		[Nullable(2)]
		public TreasureRoomRelicSynchronizer TestSynchronizer
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x06008068 RID: 32872 RVA: 0x002B8A27 File Offset: 0x002B6C27
		public PickRelicAction(Player player, int? relicIndex)
		{
			this._player = player;
			this._relicIndex = relicIndex;
		}

		// Token: 0x06008069 RID: 32873 RVA: 0x002B8A40 File Offset: 0x002B6C40
		protected override Task ExecuteAction()
		{
			TreasureRoomRelicSynchronizer treasureRoomRelicSynchronizer = this.TestSynchronizer ?? RunManager.Instance.TreasureRoomRelicSynchronizer;
			treasureRoomRelicSynchronizer.OnPicked(this._player, this._relicIndex);
			return Task.CompletedTask;
		}

		// Token: 0x0600806A RID: 32874 RVA: 0x002B8A7C File Offset: 0x002B6C7C
		public override INetAction ToNetAction()
		{
			return new NetPickRelicAction
			{
				relicIndex = this._relicIndex
			};
		}

		// Token: 0x0600806B RID: 32875 RVA: 0x002B8AA4 File Offset: 0x002B6CA4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 3);
			defaultInterpolatedStringHandler.AppendFormatted("NetPickRelicAction");
			defaultInterpolatedStringHandler.AppendLiteral(" for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" index ");
			defaultInterpolatedStringHandler.AppendFormatted<int?>(this._relicIndex);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002AF8 RID: 11000
		private readonly Player _player;

		// Token: 0x04002AF9 RID: 11001
		private readonly int? _relicIndex;
	}
}
