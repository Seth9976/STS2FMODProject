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
	// Token: 0x02000B43 RID: 2883
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Zap : CardModel
	{
		// Token: 0x060079D5 RID: 31189 RVA: 0x0029C885 File Offset: 0x0029AA85
		public Zap()
			: base(1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
		{
		}

		// Token: 0x1700215F RID: 8543
		// (get) Token: 0x060079D6 RID: 31190 RVA: 0x0029C892 File Offset: 0x0029AA92
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

		// Token: 0x060079D7 RID: 31191 RVA: 0x0029C8B8 File Offset: 0x0029AAB8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await OrbCmd.Channel<LightningOrb>(choiceContext, base.Owner);
		}

		// Token: 0x060079D8 RID: 31192 RVA: 0x0029C903 File Offset: 0x0029AB03
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
