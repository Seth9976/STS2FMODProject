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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AC6 RID: 2758
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Shroud : CardModel
	{
		// Token: 0x0600772F RID: 30511 RVA: 0x002972DC File Offset: 0x002954DC
		public Shroud()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x1700204C RID: 8268
		// (get) Token: 0x06007730 RID: 30512 RVA: 0x002972EC File Offset: 0x002954EC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<DoomPower>(null),
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x1700204D RID: 8269
		// (get) Token: 0x06007731 RID: 30513 RVA: 0x00297323 File Offset: 0x00295523
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(2m, ValueProp.Unpowered));
			}
		}

		// Token: 0x06007732 RID: 30514 RVA: 0x00297338 File Offset: 0x00295538
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<ShroudPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Block.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007733 RID: 30515 RVA: 0x00297383 File Offset: 0x00295583
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(1m);
		}
	}
}
