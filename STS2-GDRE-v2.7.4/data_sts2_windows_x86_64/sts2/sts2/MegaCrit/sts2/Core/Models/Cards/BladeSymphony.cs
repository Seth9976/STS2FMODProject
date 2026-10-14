using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200091E RID: 2334
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BladeSymphony : CardModel
	{
		// Token: 0x06006E4A RID: 28234 RVA: 0x0028510A File Offset: 0x0028330A
		public BladeSymphony()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies, true)
		{
		}

		// Token: 0x17001C84 RID: 7300
		// (get) Token: 0x06006E4B RID: 28235 RVA: 0x00285117 File Offset: 0x00283317
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(false));
			}
		}

		// Token: 0x17001C85 RID: 7301
		// (get) Token: 0x06006E4C RID: 28236 RVA: 0x00285124 File Offset: 0x00283324
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001C86 RID: 7302
		// (get) Token: 0x06006E4D RID: 28237 RVA: 0x00285127 File Offset: 0x00283327
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x06006E4E RID: 28238 RVA: 0x00285134 File Offset: 0x00283334
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			if (base.CombatState != null)
			{
				IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
					where c != null && c.IsAlive && c.IsPlayer
					select c;
				foreach (Creature teammate in enumerable)
				{
					for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
					{
						await Shiv.CreateInHand(teammate.Player, base.CombatState, null);
						await Cmd.Wait(0.1f, false);
					}
					teammate = null;
				}
				IEnumerator<Creature> enumerator = null;
			}
		}

		// Token: 0x06006E4F RID: 28239 RVA: 0x00285177 File Offset: 0x00283377
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
