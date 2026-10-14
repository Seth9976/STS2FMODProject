using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A58 RID: 2648
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NotYet : CardModel
	{
		// Token: 0x060074E8 RID: 29928 RVA: 0x00292811 File Offset: 0x00290A11
		public NotYet()
			: base(2, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F5B RID: 8027
		// (get) Token: 0x060074E9 RID: 29929 RVA: 0x0029281E File Offset: 0x00290A1E
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001F5C RID: 8028
		// (get) Token: 0x060074EA RID: 29930 RVA: 0x00292821 File Offset: 0x00290A21
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(10m));
			}
		}

		// Token: 0x17001F5D RID: 8029
		// (get) Token: 0x060074EB RID: 29931 RVA: 0x00292834 File Offset: 0x00290A34
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x060074EC RID: 29932 RVA: 0x0029283C File Offset: 0x00290A3C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.BaseValue, true);
		}

		// Token: 0x060074ED RID: 29933 RVA: 0x0029287F File Offset: 0x00290A7F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Heal.UpgradeValueBy(3m);
		}
	}
}
