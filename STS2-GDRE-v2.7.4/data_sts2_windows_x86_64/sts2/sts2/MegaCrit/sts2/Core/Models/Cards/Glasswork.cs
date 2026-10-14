using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009E0 RID: 2528
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Glasswork : CardModel
	{
		// Token: 0x0600725C RID: 29276 RVA: 0x0028D2D3 File Offset: 0x0028B4D3
		public Glasswork()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E49 RID: 7753
		// (get) Token: 0x0600725D RID: 29277 RVA: 0x0028D2E0 File Offset: 0x0028B4E0
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E4A RID: 7754
		// (get) Token: 0x0600725E RID: 29278 RVA: 0x0028D2E3 File Offset: 0x0028B4E3
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<GlassOrb>()
				});
			}
		}

		// Token: 0x17001E4B RID: 7755
		// (get) Token: 0x0600725F RID: 29279 RVA: 0x0028D306 File Offset: 0x0028B506
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(5m, ValueProp.Move));
			}
		}

		// Token: 0x06007260 RID: 29280 RVA: 0x0028D31C File Offset: 0x0028B51C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await OrbCmd.Channel<GlassOrb>(choiceContext, base.Owner);
		}

		// Token: 0x06007261 RID: 29281 RVA: 0x0028D36F File Offset: 0x0028B56F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
