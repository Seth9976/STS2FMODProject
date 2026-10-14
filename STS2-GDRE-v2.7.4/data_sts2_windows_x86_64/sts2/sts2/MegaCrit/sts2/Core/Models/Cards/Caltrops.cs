using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000944 RID: 2372
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Caltrops : CardModel
	{
		// Token: 0x06006F0B RID: 28427 RVA: 0x002869E4 File Offset: 0x00284BE4
		public Caltrops()
			: base(1, CardType.Power, CardRarity.Event, TargetType.Self, true)
		{
		}

		// Token: 0x17001CD2 RID: 7378
		// (get) Token: 0x06006F0C RID: 28428 RVA: 0x002869F4 File Offset: 0x00284BF4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<ThornsPower>(null));
			}
		}

		// Token: 0x17001CD3 RID: 7379
		// (get) Token: 0x06006F0D RID: 28429 RVA: 0x00286A14 File Offset: 0x00284C14
		public override CardPoolModel VisualCardPool
		{
			get
			{
				return ModelDb.CardPool<SilentCardPool>();
			}
		}

		// Token: 0x17001CD4 RID: 7380
		// (get) Token: 0x06006F0E RID: 28430 RVA: 0x00286A1B File Offset: 0x00284C1B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<ThornsPower>(3m));
			}
		}

		// Token: 0x06006F0F RID: 28431 RVA: 0x00286A30 File Offset: 0x00284C30
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<ThornsPower>(choiceContext, base.Owner.Creature, base.DynamicVars["ThornsPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006F10 RID: 28432 RVA: 0x00286A7B File Offset: 0x00284C7B
		protected override void OnUpgrade()
		{
			base.DynamicVars["ThornsPower"].UpgradeValueBy(2m);
		}
	}
}
