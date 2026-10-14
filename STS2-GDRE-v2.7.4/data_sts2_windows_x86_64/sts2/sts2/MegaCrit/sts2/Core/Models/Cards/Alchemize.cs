using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020008FA RID: 2298
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Alchemize : CardModel
	{
		// Token: 0x06006D94 RID: 28052 RVA: 0x002839CC File Offset: 0x00281BCC
		public Alchemize()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001C38 RID: 7224
		// (get) Token: 0x06006D95 RID: 28053 RVA: 0x002839D9 File Offset: 0x00281BD9
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C39 RID: 7225
		// (get) Token: 0x06006D96 RID: 28054 RVA: 0x002839DC File Offset: 0x00281BDC
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06006D97 RID: 28055 RVA: 0x002839E4 File Offset: 0x00281BE4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PotionCmd.TryToProcure(PotionFactory.CreateRandomPotionInCombat(base.Owner, base.Owner.RunState.Rng.CombatPotionGeneration, null).ToMutable(), base.Owner, -1);
		}

		// Token: 0x06006D98 RID: 28056 RVA: 0x00283A27 File Offset: 0x00281C27
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
