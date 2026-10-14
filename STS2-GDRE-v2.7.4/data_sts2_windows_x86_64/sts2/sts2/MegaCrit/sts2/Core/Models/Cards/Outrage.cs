using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A63 RID: 2659
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Outrage : CardModel
	{
		// Token: 0x06007520 RID: 29984 RVA: 0x00292F06 File Offset: 0x00291106
		public Outrage()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F72 RID: 8050
		// (get) Token: 0x06007521 RID: 29985 RVA: 0x00292F13 File Offset: 0x00291113
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001F73 RID: 8051
		// (get) Token: 0x06007522 RID: 29986 RVA: 0x00292F16 File Offset: 0x00291116
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(9m, ValueProp.Move));
			}
		}

		// Token: 0x06007523 RID: 29987 RVA: 0x00292F2C File Offset: 0x0029112C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			if (base.CombatState != null)
			{
				IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
					where c != null && c.IsAlive && c.IsPlayer
					select c;
				foreach (Creature creature in enumerable)
				{
					CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(base.CreateCloneForPlayer(creature.Player), PileType.Discard, base.Owner, CardPilePosition.Bottom), 2.2f, CardPreviewStyle.HorizontalLayout);
				}
				IEnumerator<Creature> enumerator = null;
			}
		}

		// Token: 0x06007524 RID: 29988 RVA: 0x00292F7F File Offset: 0x0029117F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
