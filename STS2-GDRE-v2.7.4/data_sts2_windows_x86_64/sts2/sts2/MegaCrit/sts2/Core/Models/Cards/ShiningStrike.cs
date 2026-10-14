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
	// Token: 0x02000AC3 RID: 2755
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShiningStrike : CardModel
	{
		// Token: 0x06007718 RID: 30488 RVA: 0x00296F93 File Offset: 0x00295193
		public ShiningStrike()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002042 RID: 8258
		// (get) Token: 0x06007719 RID: 30489 RVA: 0x00296FA0 File Offset: 0x002951A0
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x17002043 RID: 8259
		// (get) Token: 0x0600771A RID: 30490 RVA: 0x00296FAF File Offset: 0x002951AF
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(8m, ValueProp.Move),
					new StarsVar(2)
				});
			}
		}

		// Token: 0x0600771B RID: 30491 RVA: 0x00296FD4 File Offset: 0x002951D4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_starry_impact", null, null)
				.Execute(choiceContext);
			await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
		}

		// Token: 0x0600771C RID: 30492 RVA: 0x00297028 File Offset: 0x00295228
		protected override CardLocation GetResultLocationForCardPlay()
		{
			CardLocation resultLocationForCardPlay = base.GetResultLocationForCardPlay();
			if (resultLocationForCardPlay.pileType == PileType.Discard)
			{
				resultLocationForCardPlay.pileType = PileType.Draw;
				resultLocationForCardPlay.position = CardPilePosition.Top;
			}
			return resultLocationForCardPlay;
		}

		// Token: 0x0600771D RID: 30493 RVA: 0x00297056 File Offset: 0x00295256
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
