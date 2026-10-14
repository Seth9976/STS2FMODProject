using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000918 RID: 2328
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BelieveInYou : CardModel
	{
		// Token: 0x06006E2A RID: 28202 RVA: 0x00284D57 File Offset: 0x00282F57
		public BelieveInYou()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001C76 RID: 7286
		// (get) Token: 0x06006E2B RID: 28203 RVA: 0x00284D64 File Offset: 0x00282F64
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001C77 RID: 7287
		// (get) Token: 0x06006E2C RID: 28204 RVA: 0x00284D67 File Offset: 0x00282F67
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x17001C78 RID: 7288
		// (get) Token: 0x06006E2D RID: 28205 RVA: 0x00284D74 File Offset: 0x00282F74
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x06006E2E RID: 28206 RVA: 0x00284D84 File Offset: 0x00282F84
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, cardPlay.Target.Player);
		}

		// Token: 0x06006E2F RID: 28207 RVA: 0x00284DCF File Offset: 0x00282FCF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
