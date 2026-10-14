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
	// Token: 0x02000AF3 RID: 2803
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StrikeNecrobinder : CardModel
	{
		/// <summary>
		/// The only difference between the starting Strike cards are portrait, attack vfx, and color.
		/// </summary>
		// Token: 0x06007829 RID: 30761 RVA: 0x002993BF File Offset: 0x002975BF
		public StrikeNecrobinder()
			: base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020AE RID: 8366
		// (get) Token: 0x0600782A RID: 30762 RVA: 0x002993CC File Offset: 0x002975CC
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x170020AF RID: 8367
		// (get) Token: 0x0600782B RID: 30763 RVA: 0x002993DB File Offset: 0x002975DB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x0600782C RID: 30764 RVA: 0x002993F0 File Offset: 0x002975F0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x0600782D RID: 30765 RVA: 0x00299443 File Offset: 0x00297643
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
