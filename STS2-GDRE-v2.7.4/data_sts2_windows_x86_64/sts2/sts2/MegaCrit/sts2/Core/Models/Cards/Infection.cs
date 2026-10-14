using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A0F RID: 2575
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Infection : CardModel
	{
		// Token: 0x06007355 RID: 29525 RVA: 0x0028F2B0 File Offset: 0x0028D4B0
		public Infection()
			: base(-1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x17001EB4 RID: 7860
		// (get) Token: 0x06007356 RID: 29526 RVA: 0x0028F2BD File Offset: 0x0028D4BD
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001EB5 RID: 7861
		// (get) Token: 0x06007357 RID: 29527 RVA: 0x0028F2C0 File Offset: 0x0028D4C0
		public override bool HasBuiltInOverlay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001EB6 RID: 7862
		// (get) Token: 0x06007358 RID: 29528 RVA: 0x0028F2C3 File Offset: 0x0028D4C3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(3m, ValueProp.Unpowered | ValueProp.Move));
			}
		}

		// Token: 0x17001EB7 RID: 7863
		// (get) Token: 0x06007359 RID: 29529 RVA: 0x0028F2D7 File Offset: 0x0028D4D7
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x17001EB8 RID: 7864
		// (get) Token: 0x0600735A RID: 29530 RVA: 0x0028F2DF File Offset: 0x0028D4DF
		public override bool HasTurnEndInHandEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600735B RID: 29531 RVA: 0x0028F2E4 File Offset: 0x0028D4E4
		protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_bloody_impact");
			NWormyImpactVfx nwormyImpactVfx = NWormyImpactVfx.Create(base.Owner.Creature);
			if (nwormyImpactVfx != null)
			{
				NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(nwormyImpactVfx);
			}
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.Damage, this, null);
		}
	}
}
