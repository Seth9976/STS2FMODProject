using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200093B RID: 2363
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Burst : CardModel
	{
		// Token: 0x06006EE6 RID: 28390 RVA: 0x00286596 File Offset: 0x00284796
		public Burst()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001CC7 RID: 7367
		// (get) Token: 0x06006EE7 RID: 28391 RVA: 0x002865A3 File Offset: 0x002847A3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Skills", 1m));
			}
		}

		// Token: 0x06006EE8 RID: 28392 RVA: 0x002865BC File Offset: 0x002847BC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<BurstPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Skills"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006EE9 RID: 28393 RVA: 0x00286607 File Offset: 0x00284807
		protected override void OnUpgrade()
		{
			base.DynamicVars["Skills"].UpgradeValueBy(1m);
		}

		// Token: 0x0400284F RID: 10319
		private const string _skills = "Skills";
	}
}
