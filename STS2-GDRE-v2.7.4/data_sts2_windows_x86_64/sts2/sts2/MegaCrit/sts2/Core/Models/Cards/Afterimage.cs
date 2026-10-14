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
	// Token: 0x020008F7 RID: 2295
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Afterimage : CardModel
	{
		// Token: 0x06006D86 RID: 28038 RVA: 0x00283836 File Offset: 0x00281A36
		public Afterimage()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001C33 RID: 7219
		// (get) Token: 0x06006D87 RID: 28039 RVA: 0x00283843 File Offset: 0x00281A43
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<AfterimagePower>(1m));
			}
		}

		// Token: 0x17001C34 RID: 7220
		// (get) Token: 0x06006D88 RID: 28040 RVA: 0x00283854 File Offset: 0x00281A54
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06006D89 RID: 28041 RVA: 0x00283868 File Offset: 0x00281A68
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<AfterimagePower>(choiceContext, base.Owner.Creature, base.DynamicVars["AfterimagePower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006D8A RID: 28042 RVA: 0x002838B3 File Offset: 0x00281AB3
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Innate);
		}
	}
}
