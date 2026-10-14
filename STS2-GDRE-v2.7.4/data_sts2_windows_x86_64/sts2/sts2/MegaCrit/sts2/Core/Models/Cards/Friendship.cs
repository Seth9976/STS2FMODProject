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
	// Token: 0x020009D4 RID: 2516
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Friendship : CardModel
	{
		// Token: 0x06007215 RID: 29205 RVA: 0x0028C9EB File Offset: 0x0028ABEB
		public Friendship()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E2C RID: 7724
		// (get) Token: 0x06007216 RID: 29206 RVA: 0x0028C9F8 File Offset: 0x0028ABF8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<StrengthPower>(2m),
					new EnergyVar(1)
				});
			}
		}

		// Token: 0x17001E2D RID: 7725
		// (get) Token: 0x06007217 RID: 29207 RVA: 0x0028CA1C File Offset: 0x0028AC1C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06007218 RID: 29208 RVA: 0x0028CA2C File Offset: 0x0028AC2C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, -base.DynamicVars["StrengthPower"].BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<FriendshipPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007219 RID: 29209 RVA: 0x0028CA77 File Offset: 0x0028AC77
		protected override void OnUpgrade()
		{
			base.DynamicVars["StrengthPower"].UpgradeValueBy(-1m);
		}
	}
}
