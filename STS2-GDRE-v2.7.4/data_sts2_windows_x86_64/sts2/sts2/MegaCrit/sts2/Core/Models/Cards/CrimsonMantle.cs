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
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200096C RID: 2412
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CrimsonMantle : CardModel
	{
		// Token: 0x06006FDA RID: 28634 RVA: 0x002883EE File Offset: 0x002865EE
		public CrimsonMantle()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001D2A RID: 7466
		// (get) Token: 0x06006FDB RID: 28635 RVA: 0x002883FB File Offset: 0x002865FB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<CrimsonMantlePower>(7m));
			}
		}

		// Token: 0x17001D2B RID: 7467
		// (get) Token: 0x06006FDC RID: 28636 RVA: 0x0028840D File Offset: 0x0028660D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06006FDD RID: 28637 RVA: 0x00288420 File Offset: 0x00286620
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			NPowerUpVfx.CreateNormal(base.Owner.Creature);
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			CrimsonMantlePower crimsonMantlePower = await PowerCmd.Apply<CrimsonMantlePower>(choiceContext, base.Owner.Creature, base.DynamicVars["CrimsonMantlePower"].BaseValue, base.Owner.Creature, this, false);
			if (crimsonMantlePower != null)
			{
				crimsonMantlePower.IncrementSelfDamage();
			}
		}

		// Token: 0x06006FDE RID: 28638 RVA: 0x0028846B File Offset: 0x0028666B
		protected override void OnUpgrade()
		{
			base.DynamicVars["CrimsonMantlePower"].UpgradeValueBy(3m);
		}
	}
}
