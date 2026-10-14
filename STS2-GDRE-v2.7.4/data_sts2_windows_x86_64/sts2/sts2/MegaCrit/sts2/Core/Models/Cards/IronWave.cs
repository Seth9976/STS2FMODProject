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
	// Token: 0x02000A17 RID: 2583
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IronWave : CardModel
	{
		// Token: 0x0600737D RID: 29565 RVA: 0x0028F707 File Offset: 0x0028D907
		public IronWave()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001EC6 RID: 7878
		// (get) Token: 0x0600737E RID: 29566 RVA: 0x0028F714 File Offset: 0x0028D914
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001EC7 RID: 7879
		// (get) Token: 0x0600737F RID: 29567 RVA: 0x0028F717 File Offset: 0x0028D917
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(5m, ValueProp.Move),
					new BlockVar(5m, ValueProp.Move)
				});
			}
		}

		// Token: 0x06007380 RID: 29568 RVA: 0x0028F744 File Offset: 0x0028D944
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_flying_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06007381 RID: 29569 RVA: 0x0028F797 File Offset: 0x0028D997
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars.Block.UpgradeValueBy(2m);
		}
	}
}
