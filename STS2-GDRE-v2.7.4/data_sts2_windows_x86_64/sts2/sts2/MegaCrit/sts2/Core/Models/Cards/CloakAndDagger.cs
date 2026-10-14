using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000953 RID: 2387
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CloakAndDagger : CardModel
	{
		// Token: 0x06006F57 RID: 28503 RVA: 0x00287387 File Offset: 0x00285587
		public CloakAndDagger()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001CF0 RID: 7408
		// (get) Token: 0x06006F58 RID: 28504 RVA: 0x00287394 File Offset: 0x00285594
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CF1 RID: 7409
		// (get) Token: 0x06006F59 RID: 28505 RVA: 0x00287397 File Offset: 0x00285597
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(6m, ValueProp.Move),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x17001CF2 RID: 7410
		// (get) Token: 0x06006F5A RID: 28506 RVA: 0x002873BC File Offset: 0x002855BC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(false));
			}
		}

		// Token: 0x06006F5B RID: 28507 RVA: 0x002873CC File Offset: 0x002855CC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
			{
				await Shiv.CreateInHand(base.Owner, base.CombatState, null);
				await Cmd.Wait(0.25f, false);
			}
		}

		// Token: 0x06006F5C RID: 28508 RVA: 0x00287417 File Offset: 0x00285617
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
