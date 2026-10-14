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
	// Token: 0x02000A74 RID: 2676
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PillarOfCreation : CardModel
	{
		// Token: 0x06007577 RID: 30071 RVA: 0x00293A5B File Offset: 0x00291C5B
		public PillarOfCreation()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F95 RID: 8085
		// (get) Token: 0x06007578 RID: 30072 RVA: 0x00293A68 File Offset: 0x00291C68
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17001F96 RID: 8086
		// (get) Token: 0x06007579 RID: 30073 RVA: 0x00293A7A File Offset: 0x00291C7A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(5m, ValueProp.Unpowered));
			}
		}

		// Token: 0x0600757A RID: 30074 RVA: 0x00293A90 File Offset: 0x00291C90
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<PillarOfCreationPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Block.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600757B RID: 30075 RVA: 0x00293ADB File Offset: 0x00291CDB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
		}
	}
}
