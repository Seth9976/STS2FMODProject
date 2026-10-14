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
	// Token: 0x0200094A RID: 2378
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Chaos : CardModel
	{
		// Token: 0x06006F25 RID: 28453 RVA: 0x00286D46 File Offset: 0x00284F46
		public Chaos()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CDB RID: 7387
		// (get) Token: 0x06006F26 RID: 28454 RVA: 0x00286D53 File Offset: 0x00284F53
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new RepeatVar(1));
			}
		}

		// Token: 0x17001CDC RID: 7388
		// (get) Token: 0x06006F27 RID: 28455 RVA: 0x00286D60 File Offset: 0x00284F60
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06006F28 RID: 28456 RVA: 0x00286D74 File Offset: 0x00284F74
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			for (int i = 0; i < base.DynamicVars.Repeat.IntValue; i++)
			{
				await OrbCmd.Channel(choiceContext, OrbModel.GetRandomOrb(base.Owner.RunState.Rng.CombatOrbGeneration).ToMutable(0), base.Owner);
			}
		}

		// Token: 0x06006F29 RID: 28457 RVA: 0x00286DBF File Offset: 0x00284FBF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Repeat.UpgradeValueBy(1m);
		}
	}
}
