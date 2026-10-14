using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000937 RID: 2359
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Bulwark : CardModel
	{
		// Token: 0x06006ECF RID: 28367 RVA: 0x00286356 File Offset: 0x00284556
		public Bulwark()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CBB RID: 7355
		// (get) Token: 0x06006ED0 RID: 28368 RVA: 0x00286363 File Offset: 0x00284563
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CBC RID: 7356
		// (get) Token: 0x06006ED1 RID: 28369 RVA: 0x00286366 File Offset: 0x00284566
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(12m, ValueProp.Move),
					new ForgeVar(10)
				});
			}
		}

		// Token: 0x17001CBD RID: 7357
		// (get) Token: 0x06006ED2 RID: 28370 RVA: 0x0028638D File Offset: 0x0028458D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x06006ED3 RID: 28371 RVA: 0x00286394 File Offset: 0x00284594
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await ForgeCmd.Forge(base.DynamicVars.Forge.IntValue, base.Owner, this);
		}

		// Token: 0x06006ED4 RID: 28372 RVA: 0x002863DF File Offset: 0x002845DF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
			base.DynamicVars.Forge.UpgradeValueBy(3m);
		}
	}
}
