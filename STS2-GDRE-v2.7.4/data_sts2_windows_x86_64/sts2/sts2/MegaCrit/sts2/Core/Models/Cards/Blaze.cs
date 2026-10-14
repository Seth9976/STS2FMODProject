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
	// Token: 0x0200091F RID: 2335
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Blaze : CardModel
	{
		// Token: 0x06006E50 RID: 28240 RVA: 0x00285185 File Offset: 0x00283385
		public Blaze()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001C87 RID: 7303
		// (get) Token: 0x06006E51 RID: 28241 RVA: 0x00285192 File Offset: 0x00283392
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001C88 RID: 7304
		// (get) Token: 0x06006E52 RID: 28242 RVA: 0x00285198 File Offset: 0x00283398
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x17001C89 RID: 7305
		// (get) Token: 0x06006E53 RID: 28243 RVA: 0x002851B8 File Offset: 0x002833B8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(5m));
			}
		}

		// Token: 0x06006E54 RID: 28244 RVA: 0x002851CC File Offset: 0x002833CC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006E55 RID: 28245 RVA: 0x0028521F File Offset: 0x0028341F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Strength.UpgradeValueBy(2m);
		}
	}
}
