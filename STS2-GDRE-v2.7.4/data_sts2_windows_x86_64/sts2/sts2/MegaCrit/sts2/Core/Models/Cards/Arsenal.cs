using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000903 RID: 2307
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Arsenal : CardModel
	{
		// Token: 0x06006DBF RID: 28095 RVA: 0x00283EDB File Offset: 0x002820DB
		public Arsenal()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001C48 RID: 7240
		// (get) Token: 0x06006DC0 RID: 28096 RVA: 0x00283EE8 File Offset: 0x002820E8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x17001C49 RID: 7241
		// (get) Token: 0x06006DC1 RID: 28097 RVA: 0x00283F08 File Offset: 0x00282108
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<ArsenalPower>(1m));
			}
		}

		// Token: 0x06006DC2 RID: 28098 RVA: 0x00283F1C File Offset: 0x0028211C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<ArsenalPower>(choiceContext, base.Owner.Creature, base.DynamicVars["ArsenalPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006DC3 RID: 28099 RVA: 0x00283F67 File Offset: 0x00282167
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Innate);
		}
	}
}
