using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A0C RID: 2572
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImitationLearning : CardModel
	{
		// Token: 0x06007344 RID: 29508 RVA: 0x0028F0D0 File Offset: 0x0028D2D0
		public ImitationLearning()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001EAC RID: 7852
		// (get) Token: 0x06007345 RID: 29509 RVA: 0x0028F0DD File Offset: 0x0028D2DD
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001EAD RID: 7853
		// (get) Token: 0x06007346 RID: 29510 RVA: 0x0028F0E0 File Offset: 0x0028D2E0
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001EAE RID: 7854
		// (get) Token: 0x06007347 RID: 29511 RVA: 0x0028F0E8 File Offset: 0x0028D2E8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<ImitationLearningPower>(2m));
			}
		}

		// Token: 0x06007348 RID: 29512 RVA: 0x0028F0FC File Offset: 0x0028D2FC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			ImitationLearningPower imitationLearningPower = base.Owner.Creature.Powers.OfType<ImitationLearningPower>().FirstOrDefault<ImitationLearningPower>((ImitationLearningPower s) => s.PlayerTarget == cardPlay.Target.Player);
			decimal baseValue = base.DynamicVars["ImitationLearningPower"].BaseValue;
			if (imitationLearningPower != null)
			{
				await PowerCmd.ModifyAmount(choiceContext, imitationLearningPower, baseValue, base.Owner.Creature, this, false);
			}
			else
			{
				imitationLearningPower = await PowerCmd.Apply<ImitationLearningPower>(choiceContext, base.Owner.Creature, baseValue, base.Owner.Creature, this, false);
				if (imitationLearningPower != null)
				{
					imitationLearningPower.PlayerTarget = cardPlay.Target.Player;
				}
			}
		}

		// Token: 0x06007349 RID: 29513 RVA: 0x0028F14F File Offset: 0x0028D34F
		protected override void OnUpgrade()
		{
			base.DynamicVars["ImitationLearningPower"].UpgradeValueBy(1m);
		}
	}
}
