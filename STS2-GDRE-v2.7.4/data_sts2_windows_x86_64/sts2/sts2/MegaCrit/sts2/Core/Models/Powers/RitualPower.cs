using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006DE RID: 1758
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RitualPower : PowerModel
	{
		// Token: 0x17001372 RID: 4978
		// (get) Token: 0x06005AA9 RID: 23209 RVA: 0x00252D0A File Offset: 0x00250F0A
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x06005AAA RID: 23210 RVA: 0x00252D0D File Offset: 0x00250F0D
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001374 RID: 4980
		// (get) Token: 0x06005AAB RID: 23211 RVA: 0x00252D10 File Offset: 0x00250F10
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new IHoverTip[] { HoverTipFactory.FromPower<StrengthPower>(null) };
			}
		}

		// Token: 0x17001375 RID: 4981
		// (get) Token: 0x06005AAC RID: 23212 RVA: 0x00252D34 File Offset: 0x00250F34
		// (set) Token: 0x06005AAD RID: 23213 RVA: 0x00252D3C File Offset: 0x00250F3C
		private bool WasJustAppliedByEnemy
		{
			get
			{
				return this._wasJustAppliedByEnemy;
			}
			set
			{
				base.AssertMutable();
				this._wasJustAppliedByEnemy = value;
			}
		}

		// Token: 0x06005AAE RID: 23214 RVA: 0x00252D4B File Offset: 0x00250F4B
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			if (base.Owner.IsEnemy)
			{
				this.WasJustAppliedByEnemy = true;
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x00252D68 File Offset: 0x00250F68
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				if (this.WasJustAppliedByEnemy)
				{
					this.WasJustAppliedByEnemy = false;
				}
				else
				{
					base.Flash();
					await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, base.Amount, base.Owner, null, false);
				}
			}
		}

		// Token: 0x0400250F RID: 9487
		private bool _wasJustAppliedByEnemy;
	}
}
