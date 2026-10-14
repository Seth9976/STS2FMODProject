using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B05 RID: 2821
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TagTeam : CardModel
	{
		// Token: 0x06007884 RID: 30852 RVA: 0x00299FEA File Offset: 0x002981EA
		public TagTeam()
			: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020D2 RID: 8402
		// (get) Token: 0x06007885 RID: 30853 RVA: 0x00299FF7 File Offset: 0x002981F7
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x170020D3 RID: 8403
		// (get) Token: 0x06007886 RID: 30854 RVA: 0x00299FFA File Offset: 0x002981FA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(11m, ValueProp.Move));
			}
		}

		// Token: 0x06007887 RID: 30855 RVA: 0x0029A010 File Offset: 0x00298210
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await PowerCmd.Apply<TagTeamPower>(choiceContext, cardPlay.Target, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x06007888 RID: 30856 RVA: 0x0029A063 File Offset: 0x00298263
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
