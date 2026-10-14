using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200091A RID: 2330
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BigBang : CardModel
	{
		// Token: 0x06006E35 RID: 28213 RVA: 0x00284EA3 File Offset: 0x002830A3
		public BigBang()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001C7B RID: 7291
		// (get) Token: 0x06006E36 RID: 28214 RVA: 0x00284EB0 File Offset: 0x002830B0
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001C7C RID: 7292
		// (get) Token: 0x06006E37 RID: 28215 RVA: 0x00284EB8 File Offset: 0x002830B8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(1),
					new EnergyVar(1),
					new StarsVar(1),
					new ForgeVar(5)
				});
			}
		}

		// Token: 0x17001C7D RID: 7293
		// (get) Token: 0x06006E38 RID: 28216 RVA: 0x00284EE9 File Offset: 0x002830E9
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				List<IHoverTip> list = new List<IHoverTip>();
				list.Add(base.EnergyHoverTip);
				list.AddRange(HoverTipFactory.FromForge());
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x06006E39 RID: 28217 RVA: 0x00284F0C File Offset: 0x0028310C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
			await ForgeCmd.Forge(base.DynamicVars.Forge.IntValue, base.Owner, this);
		}

		// Token: 0x06006E3A RID: 28218 RVA: 0x00284F57 File Offset: 0x00283157
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Innate);
		}
	}
}
