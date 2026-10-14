using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Debug;

namespace MegaCrit.Sts2.Core.DevConsole
{
	// Token: 0x02000CD7 RID: 3287
	[NullableContext(1)]
	[Nullable(0)]
	public class ConsoleCmdGameAction : GameAction
	{
		// Token: 0x1700246A RID: 9322
		// (get) Token: 0x06008537 RID: 34103 RVA: 0x002CC043 File Offset: 0x002CA243
		public override ulong OwnerId
		{
			get
			{
				return this.Player.NetId;
			}
		}

		// Token: 0x1700246B RID: 9323
		// (get) Token: 0x06008538 RID: 34104 RVA: 0x002CC050 File Offset: 0x002CA250
		public override GameActionType ActionType
		{
			get
			{
				if (!this.InCombat)
				{
					return GameActionType.Any;
				}
				return GameActionType.CombatPlayPhaseOnly;
			}
		}

		// Token: 0x1700246C RID: 9324
		// (get) Token: 0x06008539 RID: 34105 RVA: 0x002CC05D File Offset: 0x002CA25D
		public Player Player { get; }

		// Token: 0x1700246D RID: 9325
		// (get) Token: 0x0600853A RID: 34106 RVA: 0x002CC065 File Offset: 0x002CA265
		public string Cmd { get; }

		// Token: 0x1700246E RID: 9326
		// (get) Token: 0x0600853B RID: 34107 RVA: 0x002CC06D File Offset: 0x002CA26D
		public bool InCombat { get; }

		// Token: 0x0600853C RID: 34108 RVA: 0x002CC075 File Offset: 0x002CA275
		public ConsoleCmdGameAction(Player player, string cmd, bool inCombat)
		{
			this.Player = player;
			this.Cmd = cmd;
			this.InCombat = inCombat;
		}

		// Token: 0x0600853D RID: 34109 RVA: 0x002CC094 File Offset: 0x002CA294
		protected override async Task ExecuteAction()
		{
			await NDevConsole.Instance.ProcessNetCommand(this.Player, this.Cmd);
		}

		// Token: 0x0600853E RID: 34110 RVA: 0x002CC0D8 File Offset: 0x002CA2D8
		public override INetAction ToNetAction()
		{
			return new NetConsoleCmdGameAction
			{
				cmd = this.Cmd,
				inCombat = this.InCombat
			};
		}

		// Token: 0x0600853F RID: 34111 RVA: 0x002CC110 File Offset: 0x002CA310
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 4);
			defaultInterpolatedStringHandler.AppendFormatted("ConsoleCmdGameAction");
			defaultInterpolatedStringHandler.AppendLiteral(" player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.Player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" cmd ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Cmd);
			defaultInterpolatedStringHandler.AppendLiteral(" InCombat ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.InCombat);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
