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
	// Token: 0x02000A2A RID: 2602
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Lift : CardModel
	{
		// Token: 0x060073DB RID: 29659 RVA: 0x002903A5 File Offset: 0x0028E5A5
		public Lift()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001EE9 RID: 7913
		// (get) Token: 0x060073DC RID: 29660 RVA: 0x002903B2 File Offset: 0x0028E5B2
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001EEA RID: 7914
		// (get) Token: 0x060073DD RID: 29661 RVA: 0x002903B5 File Offset: 0x0028E5B5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(11m, ValueProp.Move));
			}
		}

		// Token: 0x17001EEB RID: 7915
		// (get) Token: 0x060073DE RID: 29662 RVA: 0x002903C9 File Offset: 0x0028E5C9
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060073DF RID: 29663 RVA: 0x002903CC File Offset: 0x0028E5CC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.GainBlock(cardPlay.Target, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x060073E0 RID: 29664 RVA: 0x00290417 File Offset: 0x0028E617
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(5m);
		}
	}
}
