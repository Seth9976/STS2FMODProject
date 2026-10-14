using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009EE RID: 2542
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GunkUp : CardModel
	{
		// Token: 0x060072A5 RID: 29349 RVA: 0x0028DBD3 File Offset: 0x0028BDD3
		public GunkUp()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E6B RID: 7787
		// (get) Token: 0x060072A6 RID: 29350 RVA: 0x0028DBE0 File Offset: 0x0028BDE0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Slimed>(false));
			}
		}

		// Token: 0x17001E6C RID: 7788
		// (get) Token: 0x060072A7 RID: 29351 RVA: 0x0028DBED File Offset: 0x0028BDED
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(4m, ValueProp.Move),
					new RepeatVar(3)
				});
			}
		}

		// Token: 0x060072A8 RID: 29352 RVA: 0x0028DC14 File Offset: 0x0028BE14
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			AttackCommand attackCommand = DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx(null, null, "blunt_attack.mp3");
			Func<Creature, Node2D> func;
			if ((func = GunkUp.<>O.<0>__Create) == null)
			{
				func = (GunkUp.<>O.<0>__Create = new Func<Creature, Node2D>(NGoopyImpactVfx.Create));
			}
			await attackCommand.WithHitVfxNode(func).Execute(choiceContext);
			CardModel cardModel = base.CombatState.CreateCard<Slimed>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Discard, base.Owner, CardPilePosition.Bottom);
			CardCmd.PreviewCardPileAdd(cardPileAddResult, 1.2f, CardPreviewStyle.HorizontalLayout);
			await Cmd.Wait(0.5f, false);
		}

		// Token: 0x060072A9 RID: 29353 RVA: 0x0028DC67 File Offset: 0x0028BE67
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
		}

		// Token: 0x020020E8 RID: 8424
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04008864 RID: 34916
			[Nullable(new byte[] { 0, 1, 2 })]
			public static Func<Creature, Node2D> <0>__Create;
		}
	}
}
