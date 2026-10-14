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
	// Token: 0x020009DF RID: 2527
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Glacier : CardModel
	{
		// Token: 0x06007256 RID: 29270 RVA: 0x0028D21F File Offset: 0x0028B41F
		public Glacier()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E46 RID: 7750
		// (get) Token: 0x06007257 RID: 29271 RVA: 0x0028D22C File Offset: 0x0028B42C
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E47 RID: 7751
		// (get) Token: 0x06007258 RID: 29272 RVA: 0x0028D22F File Offset: 0x0028B42F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x17001E48 RID: 7752
		// (get) Token: 0x06007259 RID: 29273 RVA: 0x0028D242 File Offset: 0x0028B442
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<FrostOrb>()
				});
			}
		}

		// Token: 0x0600725A RID: 29274 RVA: 0x0028D268 File Offset: 0x0028B468
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			for (int i = 0; i < 2; i++)
			{
				await OrbCmd.Channel<FrostOrb>(choiceContext, base.Owner);
			}
		}

		// Token: 0x0600725B RID: 29275 RVA: 0x0028D2BB File Offset: 0x0028B4BB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
