using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B0F RID: 2831
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheHunt : CardModel
	{
		// Token: 0x060078B9 RID: 30905 RVA: 0x0029A7AC File Offset: 0x002989AC
		public TheHunt()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020E6 RID: 8422
		// (get) Token: 0x060078BA RID: 30906 RVA: 0x0029A7B9 File Offset: 0x002989B9
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170020E7 RID: 8423
		// (get) Token: 0x060078BB RID: 30907 RVA: 0x0029A7BC File Offset: 0x002989BC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(10m, ValueProp.Move));
			}
		}

		// Token: 0x170020E8 RID: 8424
		// (get) Token: 0x060078BC RID: 30908 RVA: 0x0029A7D0 File Offset: 0x002989D0
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x170020E9 RID: 8425
		// (get) Token: 0x060078BD RID: 30909 RVA: 0x0029A7D8 File Offset: 0x002989D8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Fatal, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060078BE RID: 30910 RVA: 0x0029A7EC File Offset: 0x002989EC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			AbstractRoom currentRoom = base.CombatState.RunState.CurrentRoom;
			CombatRoom combatRoom = currentRoom as CombatRoom;
			if (combatRoom != null)
			{
				ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
				bool shouldTriggerFatal = cardPlay.Target.Powers.All<PowerModel>((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
				AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
					.WithHitFx("vfx/vfx_attack_slash", null, null)
					.Execute(choiceContext);
				AttackCommand attackCommand2 = attackCommand;
				if (shouldTriggerFatal)
				{
					if (attackCommand2.Results.SelectMany<List<DamageResult>, DamageResult>((List<DamageResult> r) => r).Any<DamageResult>((DamageResult r) => r.WasTargetKilled))
					{
						combatRoom.AddExtraReward(base.Owner, new CardReward(CardCreationOptions.ForRoom(base.Owner, combatRoom.RoomType), 3, base.Owner, null));
						await PowerCmd.Apply<TheHuntPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
					}
				}
			}
		}

		// Token: 0x060078BF RID: 30911 RVA: 0x0029A83F File Offset: 0x00298A3F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(5m);
		}
	}
}
