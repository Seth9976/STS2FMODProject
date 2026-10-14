using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009A2 RID: 2466
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EchoForm : CardModel
	{
		// Token: 0x060070FF RID: 28927 RVA: 0x0028A686 File Offset: 0x00288886
		public EchoForm()
			: base(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001DB2 RID: 7602
		// (get) Token: 0x06007100 RID: 28928 RVA: 0x0028A693 File Offset: 0x00288893
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("EchoForm", 1m));
			}
		}

		// Token: 0x17001DB3 RID: 7603
		// (get) Token: 0x06007101 RID: 28929 RVA: 0x0028A6A9 File Offset: 0x002888A9
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
			}
		}

		// Token: 0x06007102 RID: 28930 RVA: 0x0028A6B4 File Offset: 0x002888B4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<EchoFormPower>(choiceContext, base.Owner.Creature, base.DynamicVars["EchoForm"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007103 RID: 28931 RVA: 0x0028A6FF File Offset: 0x002888FF
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Ethereal);
		}

		// Token: 0x04002864 RID: 10340
		private const string _echoFormKey = "EchoForm";
	}
}
