using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200064D RID: 1613
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CorrosiveWavePower : PowerModel
	{
		// Token: 0x170011E6 RID: 4582
		// (get) Token: 0x06005764 RID: 22372 RVA: 0x0024CC8A File Offset: 0x0024AE8A
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x06005765 RID: 22373 RVA: 0x0024CC8D File Offset: 0x0024AE8D
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x06005766 RID: 22374 RVA: 0x0024CC90 File Offset: 0x0024AE90
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x0024CCB0 File Offset: 0x0024AEB0
		public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card.Owner.Creature == base.Owner)
			{
				base.Flash();
				foreach (Creature creature in base.CombatState.HittableEnemies)
				{
					NCombatRoom instance = NCombatRoom.Instance;
					NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
					if (ncreature != null)
					{
						NGaseousImpactVfx ngaseousImpactVfx = NGaseousImpactVfx.Create(ncreature.VfxSpawnPosition, new Color("83eb85"));
						NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(ngaseousImpactVfx);
					}
				}
				await PowerCmd.Apply<PoisonPower>(choiceContext, base.CombatState.HittableEnemies, base.Amount, base.Owner, null, false);
			}
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x0024CD04 File Offset: 0x0024AF04
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
