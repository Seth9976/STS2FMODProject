using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000653 RID: 1619
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CrimsonMantlePower : PowerModel
	{
		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x06005789 RID: 22409 RVA: 0x0024D0AB File Offset: 0x0024B2AB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x0600578A RID: 22410 RVA: 0x0024D0AE File Offset: 0x0024B2AE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x0600578B RID: 22411 RVA: 0x0024D0B1 File Offset: 0x0024B2B1
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x0600578C RID: 22412 RVA: 0x0024D0C3 File Offset: 0x0024B2C3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar("SelfDamage", 0m, ValueProp.Unblockable | ValueProp.Unpowered));
			}
		}

		// Token: 0x0600578D RID: 22413 RVA: 0x0024D0DC File Offset: 0x0024B2DC
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner.Player)
			{
				base.Flash();
				DamageVar damageVar = (DamageVar)base.DynamicVars["SelfDamage"];
				await CreatureCmd.Damage(choiceContext, base.Owner, damageVar.BaseValue, damageVar.Props, base.Owner);
				await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
			}
		}

		// Token: 0x0600578E RID: 22414 RVA: 0x0024D130 File Offset: 0x0024B330
		public void IncrementSelfDamage()
		{
			base.AssertMutable();
			DynamicVar dynamicVar = base.DynamicVars["SelfDamage"];
			decimal baseValue = dynamicVar.BaseValue;
			dynamicVar.BaseValue = baseValue + 1m;
		}

		// Token: 0x040024F6 RID: 9462
		private const string _selfDamageKey = "SelfDamage";
	}
}
