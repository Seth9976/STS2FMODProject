using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009D3 RID: 2515
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FranticEscape : CardModel
	{
		// Token: 0x0600720E RID: 29198 RVA: 0x0028C902 File Offset: 0x0028AB02
		public FranticEscape()
			: base(1, CardType.Status, CardRarity.Status, TargetType.Self, true)
		{
		}

		// Token: 0x17001E29 RID: 7721
		// (get) Token: 0x0600720F RID: 29199 RVA: 0x0028C910 File Offset: 0x0028AB10
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				Creature sandpitEnemy = this.GetSandpitEnemy();
				SandpitPower sandpitPower = ((sandpitEnemy != null) ? sandpitEnemy.GetPower<SandpitPower>() : null);
				if (sandpitPower == null)
				{
					return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<SandpitPower>(null));
				}
				return sandpitPower.HoverTips;
			}
		}

		// Token: 0x17001E2A RID: 7722
		// (get) Token: 0x06007210 RID: 29200 RVA: 0x0028C94D File Offset: 0x0028AB4D
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001E2B RID: 7723
		// (get) Token: 0x06007211 RID: 29201 RVA: 0x0028C950 File Offset: 0x0028AB50
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007212 RID: 29202 RVA: 0x0028C954 File Offset: 0x0028AB54
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			Creature sandpitEnemy = this.GetSandpitEnemy();
			SandpitPower sandpitPower = ((sandpitEnemy != null) ? sandpitEnemy.Powers.OfType<SandpitPower>().FirstOrDefault<SandpitPower>((SandpitPower s) => s.Target == base.Owner.Creature) : null);
			if (sandpitPower != null)
			{
				await PowerCmd.ModifyAmount(choiceContext, sandpitPower, 1m, sandpitEnemy, this, false);
			}
			base.EnergyCost.AddThisCombat(1, false);
		}

		// Token: 0x06007213 RID: 29203 RVA: 0x0028C99F File Offset: 0x0028AB9F
		[NullableContext(2)]
		private Creature GetSandpitEnemy()
		{
			ICombatState combatState = base.CombatState;
			if (combatState == null)
			{
				return null;
			}
			return combatState.Enemies.FirstOrDefault<Creature>((Creature c) => c.HasPower<SandpitPower>());
		}
	}
}
