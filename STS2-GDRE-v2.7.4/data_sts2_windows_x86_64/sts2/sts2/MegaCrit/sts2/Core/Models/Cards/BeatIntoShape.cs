using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000915 RID: 2325
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BeatIntoShape : CardModel
	{
		// Token: 0x06006E1D RID: 28189 RVA: 0x00284B4E File Offset: 0x00282D4E
		public BeatIntoShape()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001C70 RID: 7280
		// (get) Token: 0x06006E1E RID: 28190 RVA: 0x00284B5C File Offset: 0x00282D5C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[4];
				array[0] = new DamageVar(5m, ValueProp.Move);
				array[1] = new CalculationBaseVar(5m);
				array[2] = new CalculationExtraVar(5m);
				array[3] = new CalculatedVar("CalculatedForge").WithMultiplier((CardModel card, [Nullable(2)] Creature target) => CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Count<DamageReceivedEntry>((DamageReceivedEntry e) => e.Receiver == target && e.Dealer == card.Owner.Creature && e.Result.Props.IsPoweredAttack() && e.HappenedThisTurn(card.CombatState)));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x17001C71 RID: 7281
		// (get) Token: 0x06006E1F RID: 28191 RVA: 0x00284BD0 File Offset: 0x00282DD0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x06006E20 RID: 28192 RVA: 0x00284BD8 File Offset: 0x00282DD8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			AttackCommand attackCommand2 = attackCommand;
			decimal num = ((CalculatedVar)base.DynamicVars["CalculatedForge"]).Calculate(cardPlay.Target);
			num -= attackCommand2.Results.Count<List<DamageResult>>() * base.DynamicVars.CalculationExtra.BaseValue;
			await ForgeCmd.Forge(num, base.Owner, this);
		}

		// Token: 0x06006E21 RID: 28193 RVA: 0x00284C2C File Offset: 0x00282E2C
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars.CalculationBase.UpgradeValueBy(2m);
			base.DynamicVars.CalculationExtra.UpgradeValueBy(2m);
		}

		// Token: 0x04002848 RID: 10312
		private const string _calculatedForgeKey = "CalculatedForge";
	}
}
