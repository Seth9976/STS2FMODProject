using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200095F RID: 2399
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Constellation : CardModel
	{
		// Token: 0x06006F96 RID: 28566 RVA: 0x00287B6F File Offset: 0x00285D6F
		public Constellation()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001D0D RID: 7437
		// (get) Token: 0x06006F97 RID: 28567 RVA: 0x00287B7C File Offset: 0x00285D7C
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001D0E RID: 7438
		// (get) Token: 0x06006F98 RID: 28568 RVA: 0x00287B7F File Offset: 0x00285D7F
		public override int CanonicalStarCost
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001D0F RID: 7439
		// (get) Token: 0x06006F99 RID: 28569 RVA: 0x00287B82 File Offset: 0x00285D82
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(1),
					new EnergyVar(1),
					new BlockVar(9m, ValueProp.Move)
				});
			}
		}

		// Token: 0x17001D10 RID: 7440
		// (get) Token: 0x06006F9A RID: 28570 RVA: 0x00287BB1 File Offset: 0x00285DB1
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x17001D11 RID: 7441
		// (get) Token: 0x06006F9B RID: 28571 RVA: 0x00287BBE File Offset: 0x00285DBE
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006F9C RID: 28572 RVA: 0x00287BC4 File Offset: 0x00285DC4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			Player target = cardPlay.Target.Player;
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.TriggerAnim(cardPlay.Target, "Cast", base.Owner.Character.CastAnimDelay);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, target, false);
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, target);
			await CreatureCmd.GainBlock(target.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x06006F9D RID: 28573 RVA: 0x00287C17 File Offset: 0x00285E17
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
