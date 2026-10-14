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
	// Token: 0x02000ADE RID: 2782
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpiritOfAsh : CardModel
	{
		// Token: 0x060077BB RID: 30651 RVA: 0x002983EF File Offset: 0x002965EF
		public SpiritOfAsh()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002087 RID: 8327
		// (get) Token: 0x060077BC RID: 30652 RVA: 0x002983FC File Offset: 0x002965FC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("BlockOnExhaust", 4m));
			}
		}

		// Token: 0x17002088 RID: 8328
		// (get) Token: 0x060077BD RID: 30653 RVA: 0x00298413 File Offset: 0x00296613
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromKeyword(CardKeyword.Ethereal),
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x060077BE RID: 30654 RVA: 0x00298438 File Offset: 0x00296638
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<SpiritOfAshPower>(choiceContext, base.Owner.Creature, base.DynamicVars["BlockOnExhaust"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060077BF RID: 30655 RVA: 0x00298483 File Offset: 0x00296683
		protected override void OnUpgrade()
		{
			base.DynamicVars["BlockOnExhaust"].UpgradeValueBy(1m);
		}

		// Token: 0x040028C4 RID: 10436
		private const string _blockOnExhaustKey = "BlockOnExhaust";
	}
}
