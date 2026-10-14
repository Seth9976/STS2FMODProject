using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A4F RID: 2639
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NegativePulse : CardModel
	{
		// Token: 0x060074B5 RID: 29877 RVA: 0x00292113 File Offset: 0x00290313
		public NegativePulse()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001F43 RID: 8003
		// (get) Token: 0x060074B6 RID: 29878 RVA: 0x00292120 File Offset: 0x00290320
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F44 RID: 8004
		// (get) Token: 0x060074B7 RID: 29879 RVA: 0x00292123 File Offset: 0x00290323
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(5m, ValueProp.Move),
					new PowerVar<DoomPower>(7m)
				});
			}
		}

		// Token: 0x17001F45 RID: 8005
		// (get) Token: 0x060074B8 RID: 29880 RVA: 0x00292150 File Offset: 0x00290350
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x060074B9 RID: 29881 RVA: 0x00292170 File Offset: 0x00290370
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			foreach (Creature creature in base.CombatState.HittableEnemies)
			{
				await PowerCmd.Apply<DoomPower>(choiceContext, creature, base.DynamicVars.Doom.BaseValue, base.Owner.Creature, this, false);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x060074BA RID: 29882 RVA: 0x002921C3 File Offset: 0x002903C3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(1m);
			base.DynamicVars.Doom.UpgradeValueBy(4m);
		}
	}
}
