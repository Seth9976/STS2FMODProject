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
	// Token: 0x0200091C RID: 2332
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BladeDance : CardModel
	{
		// Token: 0x06006E3F RID: 28223 RVA: 0x00284FE7 File Offset: 0x002831E7
		public BladeDance()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001C7F RID: 7295
		// (get) Token: 0x06006E40 RID: 28224 RVA: 0x00284FF4 File Offset: 0x002831F4
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001C80 RID: 7296
		// (get) Token: 0x06006E41 RID: 28225 RVA: 0x00284FFC File Offset: 0x002831FC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x17001C81 RID: 7297
		// (get) Token: 0x06006E42 RID: 28226 RVA: 0x00285009 File Offset: 0x00283209
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(false));
			}
		}

		// Token: 0x06006E43 RID: 28227 RVA: 0x00285018 File Offset: 0x00283218
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
			{
				await Shiv.CreateInHand(base.Owner, base.CombatState, null);
				await Cmd.Wait(0.1f, false);
			}
		}

		// Token: 0x06006E44 RID: 28228 RVA: 0x0028505B File Offset: 0x0028325B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
