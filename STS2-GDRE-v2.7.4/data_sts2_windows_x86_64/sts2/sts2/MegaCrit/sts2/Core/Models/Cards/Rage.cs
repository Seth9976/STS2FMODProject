using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A8E RID: 2702
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Rage : CardModel
	{
		// Token: 0x060075F7 RID: 30199 RVA: 0x00294C1A File Offset: 0x00292E1A
		public Rage()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001FC6 RID: 8134
		// (get) Token: 0x060075F8 RID: 30200 RVA: 0x00294C27 File Offset: 0x00292E27
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 3m));
			}
		}

		// Token: 0x17001FC7 RID: 8135
		// (get) Token: 0x060075F9 RID: 30201 RVA: 0x00294C3E File Offset: 0x00292E3E
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060075FA RID: 30202 RVA: 0x00294C50 File Offset: 0x00292E50
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<RagePower>(choiceContext, base.Owner.Creature, base.DynamicVars["Power"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060075FB RID: 30203 RVA: 0x00294C9B File Offset: 0x00292E9B
		protected override void OnUpgrade()
		{
			base.DynamicVars["Power"].UpgradeValueBy(2m);
		}

		// Token: 0x040028B3 RID: 10419
		private const string _powerKey = "Power";
	}
}
