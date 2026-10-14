using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000923 RID: 2339
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Bludgeon : CardModel
	{
		// Token: 0x06006E67 RID: 28263 RVA: 0x00285457 File Offset: 0x00283657
		public Bludgeon()
			: base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001C92 RID: 7314
		// (get) Token: 0x06006E68 RID: 28264 RVA: 0x00285464 File Offset: 0x00283664
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(32m, ValueProp.Move));
			}
		}

		// Token: 0x06006E69 RID: 28265 RVA: 0x00285478 File Offset: 0x00283678
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(base.Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(base.Owner.Character), null)
				.WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
				.WithHitVfxSpawnedAtBase()
				.Execute(choiceContext);
		}

		// Token: 0x06006E6A RID: 28266 RVA: 0x002854CB File Offset: 0x002836CB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(10m);
		}
	}
}
