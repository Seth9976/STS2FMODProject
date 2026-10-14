using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A46 RID: 2630
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Misery : CardModel
	{
		// Token: 0x0600748A RID: 29834 RVA: 0x00291B86 File Offset: 0x0028FD86
		public Misery()
			: base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F32 RID: 7986
		// (get) Token: 0x0600748B RID: 29835 RVA: 0x00291B93 File Offset: 0x0028FD93
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(7m, ValueProp.Move));
			}
		}

		// Token: 0x0600748C RID: 29836 RVA: 0x00291BA8 File Offset: 0x0028FDA8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			Dictionary<PowerModel, int> debuffAmounts = (from p in cardPlay.Target.Powers
				where p.TypeForCurrentAmount == PowerType.Debuff
				select new ValueTuple<PowerModel, int>((PowerModel)p.ClonePreservingMutability(), p.Amount)).ToDictionary<PowerModel, int>();
			foreach (KeyValuePair<PowerModel, int> keyValuePair in debuffAmounts)
			{
				PowerModel powerModel = keyValuePair.Key;
				ITemporaryPower temporaryPower = powerModel as ITemporaryPower;
				if (temporaryPower != null)
				{
					KeyValuePair<PowerModel, int> keyValuePair2 = debuffAmounts.FirstOrDefault<KeyValuePair<PowerModel, int>>((KeyValuePair<PowerModel, int> p) => p.Key.Id == temporaryPower.InternallyAppliedPower.Id);
					if (keyValuePair2.Key != null)
					{
						Dictionary<PowerModel, int> dictionary = debuffAmounts;
						powerModel = keyValuePair2.Key;
						dictionary[powerModel] += keyValuePair.Value;
					}
				}
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			foreach (Creature enemy in base.CombatState.HittableEnemies)
			{
				if (enemy != cardPlay.Target)
				{
					foreach (KeyValuePair<PowerModel, int> keyValuePair3 in debuffAmounts)
					{
						if (keyValuePair3.Value != 0)
						{
							PowerModel powerModel2 = PowerCmd.FindExistingInstanceForStacking(keyValuePair3.Key, enemy, keyValuePair3.Key.Applier);
							if (powerModel2 != null)
							{
								await PowerCmd.ModifyAmount(choiceContext, powerModel2, keyValuePair3.Value, keyValuePair3.Key.Applier, this, false);
							}
							else
							{
								await PowerCmd.Apply(choiceContext, (PowerModel)keyValuePair3.Key.ClonePreservingMutability(), enemy, keyValuePair3.Value, keyValuePair3.Key.Applier, this, false);
							}
						}
					}
					Dictionary<PowerModel, int>.Enumerator enumerator3 = default(Dictionary<PowerModel, int>.Enumerator);
					enemy = null;
				}
			}
			IEnumerator<Creature> enumerator2 = null;
		}

		// Token: 0x0600748D RID: 29837 RVA: 0x00291BFB File Offset: 0x0028FDFB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.AddKeyword(CardKeyword.Retain);
		}
	}
}
