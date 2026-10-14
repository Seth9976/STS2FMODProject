using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009A3 RID: 2467
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EchoingSlash : CardModel
	{
		// Token: 0x06007104 RID: 28932 RVA: 0x0028A708 File Offset: 0x00288908
		public EchoingSlash()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001DB4 RID: 7604
		// (get) Token: 0x06007105 RID: 28933 RVA: 0x0028A715 File Offset: 0x00288915
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(10m, ValueProp.Move));
			}
		}

		// Token: 0x06007106 RID: 28934 RVA: 0x0028A72C File Offset: 0x0028892C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			AttackContext attackContext2 = await AttackCommand.CreateContextAsync(base.CombatState, choiceContext, cardPlay);
			AttackContext attackContext = attackContext2;
			object obj = null;
			int num = 0;
			try
			{
				IEnumerable<DamageResult> enumerable;
				for (int attackCount = 1; attackCount > 0; attackCount += enumerable.Count<DamageResult>((DamageResult r) => r.WasTargetKilled))
				{
					attackCount--;
					ICombatState combatState = base.CombatState;
					enumerable = await CreatureCmd.Damage(choiceContext, (combatState != null) ? combatState.HittableEnemies : null, base.DynamicVars.Damage, base.Owner.Creature, this, cardPlay);
					attackContext.AddHit(enumerable);
				}
				num = 1;
			}
			catch (object obj)
			{
			}
			if (attackContext != null)
			{
				await attackContext.DisposeAsync();
			}
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			if (num != 1)
			{
				obj = null;
				attackContext = null;
			}
		}

		// Token: 0x06007107 RID: 28935 RVA: 0x0028A77F File Offset: 0x0028897F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
