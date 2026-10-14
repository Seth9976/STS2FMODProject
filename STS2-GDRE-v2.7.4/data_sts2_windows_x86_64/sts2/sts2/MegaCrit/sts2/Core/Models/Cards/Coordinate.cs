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
	// Token: 0x02000964 RID: 2404
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Coordinate : CardModel
	{
		// Token: 0x06006FB2 RID: 28594 RVA: 0x00287EDA File Offset: 0x002860DA
		public Coordinate()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001D1A RID: 7450
		// (get) Token: 0x06006FB3 RID: 28595 RVA: 0x00287EE7 File Offset: 0x002860E7
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001D1B RID: 7451
		// (get) Token: 0x06006FB4 RID: 28596 RVA: 0x00287EEA File Offset: 0x002860EA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(5m));
			}
		}

		// Token: 0x17001D1C RID: 7452
		// (get) Token: 0x06006FB5 RID: 28597 RVA: 0x00287EFC File Offset: 0x002860FC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06006FB6 RID: 28598 RVA: 0x00287F1C File Offset: 0x0028611C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			decimal baseValue = base.DynamicVars.Strength.BaseValue;
			await PowerCmd.Apply<CoordinatePower>(choiceContext, cardPlay.Target, baseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006FB7 RID: 28599 RVA: 0x00287F6F File Offset: 0x0028616F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Strength.UpgradeValueBy(3m);
		}
	}
}
