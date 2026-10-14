using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	/// <summary>
	/// Tracks every time a creature receives damage.
	/// </summary>
	// Token: 0x02000D52 RID: 3410
	[NullableContext(1)]
	[Nullable(0)]
	public class DamageReceivedEntry : CombatHistoryEntry
	{
		// Token: 0x170025E4 RID: 9700
		// (get) Token: 0x06008A0D RID: 35341 RVA: 0x002E3BFD File Offset: 0x002E1DFD
		public DamageResult Result { get; }

		// Token: 0x170025E5 RID: 9701
		// (get) Token: 0x06008A0E RID: 35342 RVA: 0x002E3C05 File Offset: 0x002E1E05
		[Nullable(2)]
		public Creature Dealer
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170025E6 RID: 9702
		// (get) Token: 0x06008A0F RID: 35343 RVA: 0x002E3C0D File Offset: 0x002E1E0D
		[Nullable(2)]
		public CardModel CardSource
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170025E7 RID: 9703
		// (get) Token: 0x06008A10 RID: 35344 RVA: 0x002E3C15 File Offset: 0x002E1E15
		public Creature Receiver
		{
			get
			{
				return base.Actor;
			}
		}

		// Token: 0x170025E8 RID: 9704
		// (get) Token: 0x06008A11 RID: 35345 RVA: 0x002E3C20 File Offset: 0x002E1E20
		public override string Description
		{
			get
			{
				string id = DamageReceivedEntry.GetId(this.Receiver);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (this.Dealer != null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 3);
					defaultInterpolatedStringHandler.AppendFormatted(DamageReceivedEntry.GetId(this.Dealer));
					defaultInterpolatedStringHandler.AppendLiteral(" dealt ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.Result.UnblockedDamage);
					defaultInterpolatedStringHandler.AppendLiteral(" damage to ");
					defaultInterpolatedStringHandler.AppendFormatted(id);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
				defaultInterpolatedStringHandler.AppendFormatted(id);
				defaultInterpolatedStringHandler.AppendLiteral(" took ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Result.UnblockedDamage);
				defaultInterpolatedStringHandler.AppendLiteral(" damage");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x06008A12 RID: 35346 RVA: 0x002E3CDA File Offset: 0x002E1EDA
		public DamageReceivedEntry(DamageResult result, Creature receiver, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(receiver, roundNumber, currentSide, history, players)
		{
			this.Result = result;
			this.Dealer = dealer;
			this.CardSource = cardSource;
		}

		// Token: 0x06008A13 RID: 35347 RVA: 0x002E3D01 File Offset: 0x002E1F01
		private static string GetId(Creature creature)
		{
			if (!creature.IsPlayer)
			{
				return creature.Monster.Id.Entry;
			}
			return creature.Player.Character.Id.Entry;
		}
	}
}
