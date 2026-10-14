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
using MegaCrit.Sts2.Core.Models.Characters;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A27 RID: 2599
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LegionOfBone : CardModel
	{
		// Token: 0x060073CA RID: 29642 RVA: 0x0029018F File Offset: 0x0028E38F
		public LegionOfBone()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies, true)
		{
		}

		// Token: 0x17001EE1 RID: 7905
		// (get) Token: 0x060073CB RID: 29643 RVA: 0x0029019C File Offset: 0x0028E39C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new SummonVar(6m));
			}
		}

		// Token: 0x17001EE2 RID: 7906
		// (get) Token: 0x060073CC RID: 29644 RVA: 0x002901AE File Offset: 0x0028E3AE
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }));
			}
		}

		// Token: 0x17001EE3 RID: 7907
		// (get) Token: 0x060073CD RID: 29645 RVA: 0x002901D0 File Offset: 0x0028E3D0
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x060073CE RID: 29646 RVA: 0x002901D4 File Offset: 0x0028E3D4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(base.Owner.Character), Necrobinder.GetSummonDelayIfApplicable(base.Owner.Character));
			IEnumerable<Creature> enumerable = base.CombatState.PlayerCreatures.Where<Creature>((Creature c) => c != null && c.IsAlive).ToList<Creature>();
			foreach (Creature creature in enumerable)
			{
				await OstyCmd.Summon(choiceContext, creature.Player, base.DynamicVars.Summon.BaseValue, this);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x060073CF RID: 29647 RVA: 0x0029021F File Offset: 0x0028E41F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Summon.UpgradeValueBy(2m);
		}
	}
}
