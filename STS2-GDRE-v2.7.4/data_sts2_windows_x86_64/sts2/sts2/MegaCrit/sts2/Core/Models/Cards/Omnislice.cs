using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A5D RID: 2653
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Omnislice : CardModel
	{
		// Token: 0x06007503 RID: 29955 RVA: 0x00292BA3 File Offset: 0x00290DA3
		public Omnislice()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F67 RID: 8039
		// (get) Token: 0x06007504 RID: 29956 RVA: 0x00292BB0 File Offset: 0x00290DB0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(8m, ValueProp.Move));
			}
		}

		// Token: 0x06007505 RID: 29957 RVA: 0x00292BC4 File Offset: 0x00290DC4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			AttackContext attackContext = await AttackCommand.CreateContextAsync(base.CombatState, choiceContext, cardPlay);
			AttackContext context = attackContext;
			object obj = null;
			int num = 0;
			try
			{
				List<DamageResult> list = (await CreatureCmd.Damage(choiceContext, cardPlay.Target, base.DynamicVars.Damage.BaseValue, ValueProp.Move, this, cardPlay)).ToList<DamageResult>();
				context.AddHit(list);
				DamageResult damageResult = list.FirstOrDefault<DamageResult>();
				if (damageResult != null)
				{
					List<Creature> list2 = (from e in base.CombatState.GetTeammatesOf(damageResult.Receiver).Except<Creature>(new <>z__ReadOnlySingleElementList<Creature>(cardPlay.Target))
						where e.IsHittable
						select e).ToList<Creature>();
					if (list2.Count != 0)
					{
						AttackContext attackContext2 = context;
						attackContext2.AddHit(await CreatureCmd.Damage(choiceContext, list2, damageResult.TotalDamage + damageResult.OverkillDamage, ValueProp.Unpowered | ValueProp.Move, base.Owner.Creature, this, cardPlay));
						attackContext2 = null;
					}
				}
				num = 1;
			}
			catch (object obj)
			{
			}
			if (context != null)
			{
				await context.DisposeAsync();
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
				context = null;
			}
		}

		// Token: 0x06007506 RID: 29958 RVA: 0x00292C17 File Offset: 0x00290E17
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
