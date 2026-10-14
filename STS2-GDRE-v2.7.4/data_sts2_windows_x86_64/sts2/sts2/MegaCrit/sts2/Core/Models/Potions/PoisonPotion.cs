using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200076C RID: 1900
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PoisonPotion : PotionModel
	{
		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x06005E32 RID: 24114 RVA: 0x00258C7F File Offset: 0x00256E7F
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x06005E33 RID: 24115 RVA: 0x00258C82 File Offset: 0x00256E82
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x06005E34 RID: 24116 RVA: 0x00258C85 File Offset: 0x00256E85
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyEnemy;
			}
		}

		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x06005E35 RID: 24117 RVA: 0x00258C88 File Offset: 0x00256E88
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PoisonPower>(6m));
			}
		}

		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x06005E36 RID: 24118 RVA: 0x00258C9C File Offset: 0x00256E9C
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x06005E37 RID: 24119 RVA: 0x00258CBC File Offset: 0x00256EBC
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(target) : null);
			if (ncreature != null)
			{
				NGaseousImpactVfx ngaseousImpactVfx = NGaseousImpactVfx.Create(ncreature.VfxSpawnPosition, new Color("83eb85"));
				NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(ngaseousImpactVfx);
			}
			await PowerCmd.Apply<PoisonPower>(choiceContext, target, base.DynamicVars.Poison.BaseValue, base.Owner.Creature, null, false);
		}
	}
}
