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
	// Token: 0x020008FF RID: 2303
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Anticipate : CardModel
	{
		// Token: 0x06006DAC RID: 28076 RVA: 0x00283C7C File Offset: 0x00281E7C
		public Anticipate()
			: base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001C40 RID: 7232
		// (get) Token: 0x06006DAD RID: 28077 RVA: 0x00283C89 File Offset: 0x00281E89
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DexterityPower>(2m));
			}
		}

		// Token: 0x17001C41 RID: 7233
		// (get) Token: 0x06006DAE RID: 28078 RVA: 0x00283C9C File Offset: 0x00281E9C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>(null));
			}
		}

		// Token: 0x06006DAF RID: 28079 RVA: 0x00283CBC File Offset: 0x00281EBC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<AnticipatePower>(choiceContext, base.Owner.Creature, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006DB0 RID: 28080 RVA: 0x00283D07 File Offset: 0x00281F07
		protected override void OnUpgrade()
		{
			base.DynamicVars.Dexterity.UpgradeValueBy(2m);
		}
	}
}
