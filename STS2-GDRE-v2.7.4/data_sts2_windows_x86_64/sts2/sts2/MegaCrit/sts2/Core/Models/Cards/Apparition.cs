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
	// Token: 0x02000901 RID: 2305
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Apparition : CardModel
	{
		// Token: 0x06006DB5 RID: 28085 RVA: 0x00283DBA File Offset: 0x00281FBA
		public Apparition()
			: base(1, CardType.Skill, CardRarity.Ancient, TargetType.Self, true)
		{
		}

		// Token: 0x17001C43 RID: 7235
		// (get) Token: 0x06006DB6 RID: 28086 RVA: 0x00283DC7 File Offset: 0x00281FC7
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Ethereal,
					CardKeyword.Exhaust
				});
			}
		}

		// Token: 0x17001C44 RID: 7236
		// (get) Token: 0x06006DB7 RID: 28087 RVA: 0x00283DDC File Offset: 0x00281FDC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<IntangiblePower>(null));
			}
		}

		// Token: 0x17001C45 RID: 7237
		// (get) Token: 0x06006DB8 RID: 28088 RVA: 0x00283DFC File Offset: 0x00281FFC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<IntangiblePower>(1m));
			}
		}

		// Token: 0x06006DB9 RID: 28089 RVA: 0x00283E10 File Offset: 0x00282010
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<IntangiblePower>(choiceContext, base.Owner.Creature, base.DynamicVars["IntangiblePower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006DBA RID: 28090 RVA: 0x00283E5B File Offset: 0x0028205B
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Ethereal);
		}
	}
}
