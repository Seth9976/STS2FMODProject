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

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A8F RID: 2703
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Rainbow : CardModel
	{
		// Token: 0x060075FC RID: 30204 RVA: 0x00294CB8 File Offset: 0x00292EB8
		public Rainbow()
			: base(2, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001FC8 RID: 8136
		// (get) Token: 0x060075FD RID: 30205 RVA: 0x00294CC5 File Offset: 0x00292EC5
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001FC9 RID: 8137
		// (get) Token: 0x060075FE RID: 30206 RVA: 0x00294CCD File Offset: 0x00292ECD
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<LightningOrb>(),
					HoverTipFactory.FromOrb<FrostOrb>(),
					HoverTipFactory.FromOrb<DarkOrb>()
				});
			}
		}

		// Token: 0x060075FF RID: 30207 RVA: 0x00294D00 File Offset: 0x00292F00
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await OrbCmd.Channel<LightningOrb>(choiceContext, base.Owner);
			await OrbCmd.Channel<FrostOrb>(choiceContext, base.Owner);
			await OrbCmd.Channel<DarkOrb>(choiceContext, base.Owner);
		}

		// Token: 0x06007600 RID: 30208 RVA: 0x00294D4B File Offset: 0x00292F4B
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
