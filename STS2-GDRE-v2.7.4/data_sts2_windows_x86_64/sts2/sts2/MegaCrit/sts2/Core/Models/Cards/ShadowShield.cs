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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000ABE RID: 2750
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShadowShield : CardModel
	{
		// Token: 0x060076FA RID: 30458 RVA: 0x00296C49 File Offset: 0x00294E49
		public ShadowShield()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17002032 RID: 8242
		// (get) Token: 0x060076FB RID: 30459 RVA: 0x00296C56 File Offset: 0x00294E56
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002033 RID: 8243
		// (get) Token: 0x060076FC RID: 30460 RVA: 0x00296C59 File Offset: 0x00294E59
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(11m, ValueProp.Move));
			}
		}

		// Token: 0x17002034 RID: 8244
		// (get) Token: 0x060076FD RID: 30461 RVA: 0x00296C6D File Offset: 0x00294E6D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<DarkOrb>()
				});
			}
		}

		// Token: 0x060076FE RID: 30462 RVA: 0x00296C90 File Offset: 0x00294E90
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await OrbCmd.Channel<DarkOrb>(choiceContext, base.Owner);
		}

		// Token: 0x060076FF RID: 30463 RVA: 0x00296CE3 File Offset: 0x00294EE3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(4m);
		}
	}
}
