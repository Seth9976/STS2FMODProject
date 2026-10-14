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
	// Token: 0x02000934 RID: 2356
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BulkUp : CardModel
	{
		// Token: 0x06006EC2 RID: 28354 RVA: 0x002860F7 File Offset: 0x002842F7
		public BulkUp()
			: base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CB7 RID: 7351
		// (get) Token: 0x06006EC3 RID: 28355 RVA: 0x00286104 File Offset: 0x00284304
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.FromPower<DexterityPower>(null)
				});
			}
		}

		// Token: 0x17001CB8 RID: 7352
		// (get) Token: 0x06006EC4 RID: 28356 RVA: 0x0028613E File Offset: 0x0028433E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("OrbSlots", 1m),
					new PowerVar<StrengthPower>(2m),
					new PowerVar<DexterityPower>(2m)
				});
			}
		}

		// Token: 0x06006EC5 RID: 28357 RVA: 0x0028617C File Offset: 0x0028437C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			OrbCmd.RemoveSlots(base.Owner, base.DynamicVars["OrbSlots"].IntValue);
			await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006EC6 RID: 28358 RVA: 0x002861C7 File Offset: 0x002843C7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Strength.UpgradeValueBy(1m);
			base.DynamicVars.Dexterity.UpgradeValueBy(1m);
		}

		// Token: 0x0400284E RID: 10318
		private const string _orbSlotVar = "OrbSlots";
	}
}
