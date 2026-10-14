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
	// Token: 0x020008F0 RID: 2288
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Abrasive : CardModel
	{
		// Token: 0x06006D63 RID: 28003 RVA: 0x002833F1 File Offset: 0x002815F1
		public Abrasive()
			: base(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001C24 RID: 7204
		// (get) Token: 0x06006D64 RID: 28004 RVA: 0x00283400 File Offset: 0x00281600
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<DexterityPower>(null),
					HoverTipFactory.FromPower<ThornsPower>(null)
				});
			}
		}

		// Token: 0x17001C25 RID: 7205
		// (get) Token: 0x06006D65 RID: 28005 RVA: 0x0028343A File Offset: 0x0028163A
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Sly);
			}
		}

		// Token: 0x17001C26 RID: 7206
		// (get) Token: 0x06006D66 RID: 28006 RVA: 0x00283442 File Offset: 0x00281642
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<ThornsPower>(4m),
					new PowerVar<DexterityPower>(1m)
				});
			}
		}

		// Token: 0x06006D67 RID: 28007 RVA: 0x0028346C File Offset: 0x0028166C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<ThornsPower>(choiceContext, base.Owner.Creature, base.DynamicVars["ThornsPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006D68 RID: 28008 RVA: 0x002834B7 File Offset: 0x002816B7
		protected override void OnUpgrade()
		{
			base.DynamicVars["ThornsPower"].UpgradeValueBy(2m);
		}
	}
}
