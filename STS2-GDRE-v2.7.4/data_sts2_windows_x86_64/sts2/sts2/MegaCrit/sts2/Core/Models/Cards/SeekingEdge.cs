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
	// Token: 0x02000AB7 RID: 2743
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SeekingEdge : CardModel
	{
		// Token: 0x060076D7 RID: 30423 RVA: 0x0029680B File Offset: 0x00294A0B
		public SeekingEdge()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002024 RID: 8228
		// (get) Token: 0x060076D8 RID: 30424 RVA: 0x00296818 File Offset: 0x00294A18
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x17002025 RID: 8229
		// (get) Token: 0x060076D9 RID: 30425 RVA: 0x0029681F File Offset: 0x00294A1F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new ForgeVar(7));
			}
		}

		// Token: 0x060076DA RID: 30426 RVA: 0x0029682C File Offset: 0x00294A2C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<SeekingEdgePower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
			await ForgeCmd.Forge(base.DynamicVars.Forge.IntValue, base.Owner, this);
		}

		// Token: 0x060076DB RID: 30427 RVA: 0x00296877 File Offset: 0x00294A77
		protected override void OnUpgrade()
		{
			base.DynamicVars.Forge.UpgradeValueBy(4m);
		}
	}
}
