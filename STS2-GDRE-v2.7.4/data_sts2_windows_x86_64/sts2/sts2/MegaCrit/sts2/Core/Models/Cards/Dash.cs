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
	// Token: 0x02000976 RID: 2422
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Dash : CardModel
	{
		// Token: 0x0600700A RID: 28682 RVA: 0x00288A68 File Offset: 0x00286C68
		public Dash()
			: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D3C RID: 7484
		// (get) Token: 0x0600700B RID: 28683 RVA: 0x00288A75 File Offset: 0x00286C75
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D3D RID: 7485
		// (get) Token: 0x0600700C RID: 28684 RVA: 0x00288A78 File Offset: 0x00286C78
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(10m, ValueProp.Move),
					new BlockVar(10m, ValueProp.Move)
				});
			}
		}

		// Token: 0x0600700D RID: 28685 RVA: 0x00288AA8 File Offset: 0x00286CA8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_flying_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x0600700E RID: 28686 RVA: 0x00288AFB File Offset: 0x00286CFB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
