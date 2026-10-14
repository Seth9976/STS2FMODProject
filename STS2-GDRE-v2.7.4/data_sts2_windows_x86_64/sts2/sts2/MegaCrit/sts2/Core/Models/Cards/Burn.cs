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
	// Token: 0x02000939 RID: 2361
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Burn : CardModel
	{
		// Token: 0x06006EDA RID: 28378 RVA: 0x0028648A File Offset: 0x0028468A
		public Burn()
			: base(-1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x17001CC0 RID: 7360
		// (get) Token: 0x06006EDB RID: 28379 RVA: 0x00286497 File Offset: 0x00284697
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001CC1 RID: 7361
		// (get) Token: 0x06006EDC RID: 28380 RVA: 0x0028649A File Offset: 0x0028469A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(2m, ValueProp.Unpowered | ValueProp.Move));
			}
		}

		// Token: 0x17001CC2 RID: 7362
		// (get) Token: 0x06006EDD RID: 28381 RVA: 0x002864AE File Offset: 0x002846AE
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x17001CC3 RID: 7363
		// (get) Token: 0x06006EDE RID: 28382 RVA: 0x002864B6 File Offset: 0x002846B6
		public override bool HasTurnEndInHandEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CC4 RID: 7364
		// (get) Token: 0x06006EDF RID: 28383 RVA: 0x002864B9 File Offset: 0x002846B9
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NGroundFireVfx.AssetPaths;
			}
		}

		// Token: 0x06006EE0 RID: 28384 RVA: 0x002864C0 File Offset: 0x002846C0
		protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(base.Owner.Creature, VfxColor.Red));
			}
			SfxCmd.Play("event:/sfx/characters/attack_fire", 1f);
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.Damage, this, null);
		}
	}
}
