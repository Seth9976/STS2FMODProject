using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200096E RID: 2414
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CrushUnder : CardModel
	{
		// Token: 0x06006FE4 RID: 28644 RVA: 0x00288531 File Offset: 0x00286731
		public CrushUnder()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001D2E RID: 7470
		// (get) Token: 0x06006FE5 RID: 28645 RVA: 0x00288540 File Offset: 0x00286740
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x17001D2F RID: 7471
		// (get) Token: 0x06006FE6 RID: 28646 RVA: 0x00288560 File Offset: 0x00286760
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(7m, ValueProp.Move),
					new DynamicVar("StrengthLoss", 1m)
				});
			}
		}

		// Token: 0x06006FE7 RID: 28647 RVA: 0x00288590 File Offset: 0x00286790
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.AttackAnimDelay);
			IReadOnlyList<Creature> enemies = base.CombatState.HittableEnemies;
			foreach (Creature creature in enemies)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.CombatVfxContainer.AddChildSafely(NSpikeSplashVfx.Create(creature, VfxColor.Red));
				}
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_heavy_blunt", null, "blunt_attack.mp3")
				.WithHitVfxSpawnedAtBase()
				.Execute(choiceContext);
			await PowerCmd.Apply<CrushUnderPower>(choiceContext, enemies, base.DynamicVars["StrengthLoss"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006FE8 RID: 28648 RVA: 0x002885E3 File Offset: 0x002867E3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
			base.DynamicVars["StrengthLoss"].UpgradeValueBy(1m);
		}

		// Token: 0x0400285A RID: 10330
		private const string _strengthLossKey = "StrengthLoss";
	}
}
