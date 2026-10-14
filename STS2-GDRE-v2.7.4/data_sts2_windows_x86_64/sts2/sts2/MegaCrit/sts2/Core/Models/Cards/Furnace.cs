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
	// Token: 0x020009D7 RID: 2519
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Furnace : CardModel
	{
		// Token: 0x06007227 RID: 29223 RVA: 0x0028CC5A File Offset: 0x0028AE5A
		public Furnace()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E34 RID: 7732
		// (get) Token: 0x06007228 RID: 29224 RVA: 0x0028CC67 File Offset: 0x0028AE67
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x17001E35 RID: 7733
		// (get) Token: 0x06007229 RID: 29225 RVA: 0x0028CC6E File Offset: 0x0028AE6E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new ForgeVar(5));
			}
		}

		// Token: 0x0600722A RID: 29226 RVA: 0x0028CC7C File Offset: 0x0028AE7C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<FurnacePower>(choiceContext, base.Owner.Creature, base.DynamicVars.Forge.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600722B RID: 29227 RVA: 0x0028CCC7 File Offset: 0x0028AEC7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Forge.UpgradeValueBy(2m);
		}
	}
}
