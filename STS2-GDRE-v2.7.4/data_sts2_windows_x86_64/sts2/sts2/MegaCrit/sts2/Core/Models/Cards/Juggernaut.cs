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
	// Token: 0x02000A1B RID: 2587
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Juggernaut : CardModel
	{
		// Token: 0x0600738F RID: 29583 RVA: 0x0028F967 File Offset: 0x0028DB67
		public Juggernaut()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001ECC RID: 7884
		// (get) Token: 0x06007390 RID: 29584 RVA: 0x0028F974 File Offset: 0x0028DB74
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17001ECD RID: 7885
		// (get) Token: 0x06007391 RID: 29585 RVA: 0x0028F986 File Offset: 0x0028DB86
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<JuggernautPower>(6m));
			}
		}

		// Token: 0x06007392 RID: 29586 RVA: 0x0028F998 File Offset: 0x0028DB98
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<JuggernautPower>(choiceContext, base.Owner.Creature, base.DynamicVars["JuggernautPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007393 RID: 29587 RVA: 0x0028F9E3 File Offset: 0x0028DBE3
		protected override void OnUpgrade()
		{
			base.DynamicVars["JuggernautPower"].UpgradeValueBy(2m);
		}
	}
}
