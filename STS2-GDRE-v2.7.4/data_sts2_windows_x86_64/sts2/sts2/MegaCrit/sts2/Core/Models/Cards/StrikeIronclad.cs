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
	/// <summary>
	/// The only difference between the starting Strike cards are portrait, attack vfx, and color.
	/// </summary>
	// Token: 0x02000AF2 RID: 2802
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StrikeIronclad : CardModel
	{
		/// <summary>
		/// The only difference between the starting Strike cards are portrait, attack vfx, and color.
		/// </summary>
		// Token: 0x06007824 RID: 30756 RVA: 0x00299323 File Offset: 0x00297523
		public StrikeIronclad()
			: base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020AC RID: 8364
		// (get) Token: 0x06007825 RID: 30757 RVA: 0x00299330 File Offset: 0x00297530
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x170020AD RID: 8365
		// (get) Token: 0x06007826 RID: 30758 RVA: 0x0029933F File Offset: 0x0029753F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x06007827 RID: 30759 RVA: 0x00299354 File Offset: 0x00297554
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06007828 RID: 30760 RVA: 0x002993A7 File Offset: 0x002975A7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
