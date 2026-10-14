using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200063E RID: 1598
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CacophonyPower : PowerModel
	{
		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x06005711 RID: 22289 RVA: 0x0024C2EF File Offset: 0x0024A4EF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x06005712 RID: 22290 RVA: 0x0024C2F2 File Offset: 0x0024A4F2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x06005713 RID: 22291 RVA: 0x0024C2F5 File Offset: 0x0024A4F5
		public override int DisplayAmount
		{
			get
			{
				return base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x06005714 RID: 22292 RVA: 0x0024C307 File Offset: 0x0024A507
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x06005715 RID: 22293 RVA: 0x0024C30A File Offset: 0x0024A50A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(33));
			}
		}

		// Token: 0x06005716 RID: 22294 RVA: 0x0024C318 File Offset: 0x0024A518
		public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			CardsVar cards = base.DynamicVars.Cards;
			decimal baseValue = cards.BaseValue;
			cards.BaseValue = baseValue - 1m;
			base.InvokeDisplayAmountChanged();
			if (base.DynamicVars.Cards.IntValue <= 0)
			{
				await Cmd.Wait(0.5f, false);
				Creature creature = base.Owner.Player.RunState.Rng.CombatTargets.NextItem<Creature>(base.CombatState.HittableEnemies);
				if (creature != null)
				{
					await CreatureCmd.Damage(choiceContext, creature, base.Amount, ValueProp.Unpowered, base.Owner);
				}
				base.DynamicVars.Cards.BaseValue = 33m;
				base.InvokeDisplayAmountChanged();
			}
		}
	}
}
