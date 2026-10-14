using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A6C RID: 2668
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ParticleWall : CardModel
	{
		// Token: 0x0600754D RID: 30029 RVA: 0x0029348A File Offset: 0x0029168A
		public ParticleWall()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F84 RID: 8068
		// (get) Token: 0x0600754E RID: 30030 RVA: 0x00293497 File Offset: 0x00291697
		public override int CanonicalStarCost
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001F85 RID: 8069
		// (get) Token: 0x0600754F RID: 30031 RVA: 0x0029349A File Offset: 0x0029169A
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F86 RID: 8070
		// (get) Token: 0x06007550 RID: 30032 RVA: 0x0029349D File Offset: 0x0029169D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(9m, ValueProp.Move));
			}
		}

		// Token: 0x06007551 RID: 30033 RVA: 0x002934B4 File Offset: 0x002916B4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await Cmd.Wait(0.25f, false);
		}

		// Token: 0x06007552 RID: 30034 RVA: 0x00293500 File Offset: 0x00291700
		protected override CardLocation GetResultLocationForCardPlay()
		{
			CardLocation resultLocationForCardPlay = base.GetResultLocationForCardPlay();
			if (resultLocationForCardPlay.pileType == PileType.Discard)
			{
				resultLocationForCardPlay.pileType = PileType.Hand;
			}
			return resultLocationForCardPlay;
		}

		// Token: 0x06007553 RID: 30035 RVA: 0x00293526 File Offset: 0x00291726
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
