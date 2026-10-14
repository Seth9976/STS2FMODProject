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
	// Token: 0x02000B09 RID: 2825
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Tempest : CardModel
	{
		// Token: 0x06007898 RID: 30872 RVA: 0x0029A2FB File Offset: 0x002984FB
		public Tempest()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x170020DA RID: 8410
		// (get) Token: 0x06007899 RID: 30873 RVA: 0x0029A308 File Offset: 0x00298508
		protected override bool HasEnergyCostX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170020DB RID: 8411
		// (get) Token: 0x0600789A RID: 30874 RVA: 0x0029A30B File Offset: 0x0029850B
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

		// Token: 0x0600789B RID: 30875 RVA: 0x0029A330 File Offset: 0x00298530
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			int numOfOrbs = base.ResolveEnergyXValue();
			if (base.IsUpgraded)
			{
				numOfOrbs++;
			}
			for (int i = 0; i < numOfOrbs; i++)
			{
				await OrbCmd.Channel<LightningOrb>(choiceContext, base.Owner);
			}
		}
	}
}
