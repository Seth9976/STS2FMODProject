using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A2B RID: 2603
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LightningRod : CardModel
	{
		// Token: 0x060073E1 RID: 29665 RVA: 0x0029042F File Offset: 0x0028E62F
		public LightningRod()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001EEC RID: 7916
		// (get) Token: 0x060073E2 RID: 29666 RVA: 0x0029043C File Offset: 0x0028E63C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<LightningOrb>()
				});
			}
		}

		// Token: 0x17001EED RID: 7917
		// (get) Token: 0x060073E3 RID: 29667 RVA: 0x0029045F File Offset: 0x0028E65F
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001EEE RID: 7918
		// (get) Token: 0x060073E4 RID: 29668 RVA: 0x00290462 File Offset: 0x0028E662
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(4m, ValueProp.Move),
					new PowerVar<LightningRodPower>(2m)
				});
			}
		}

		// Token: 0x060073E5 RID: 29669 RVA: 0x0029048C File Offset: 0x0028E68C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<LightningRodPower>(choiceContext, base.Owner.Creature, base.DynamicVars["LightningRodPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060073E6 RID: 29670 RVA: 0x002904DF File Offset: 0x0028E6DF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
