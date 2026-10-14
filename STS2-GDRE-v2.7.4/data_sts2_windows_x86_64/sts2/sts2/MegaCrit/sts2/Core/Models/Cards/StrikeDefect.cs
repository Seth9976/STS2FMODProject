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
	// Token: 0x02000AF1 RID: 2801
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StrikeDefect : CardModel
	{
		/// <summary>
		/// The only difference between the starting Strike cards are portrait, attack vfx, and color.
		/// </summary>
		// Token: 0x0600781F RID: 30751 RVA: 0x00299289 File Offset: 0x00297489
		public StrikeDefect()
			: base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020AA RID: 8362
		// (get) Token: 0x06007820 RID: 30752 RVA: 0x00299296 File Offset: 0x00297496
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x170020AB RID: 8363
		// (get) Token: 0x06007821 RID: 30753 RVA: 0x002992A5 File Offset: 0x002974A5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x06007822 RID: 30754 RVA: 0x002992B8 File Offset: 0x002974B8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06007823 RID: 30755 RVA: 0x0029930B File Offset: 0x0029750B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
