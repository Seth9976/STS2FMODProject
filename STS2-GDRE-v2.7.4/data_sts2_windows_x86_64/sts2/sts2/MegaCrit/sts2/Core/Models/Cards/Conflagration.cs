using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200095D RID: 2397
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Conflagration : CardModel
	{
		// Token: 0x06006F8C RID: 28556 RVA: 0x00287A3F File Offset: 0x00285C3F
		public Conflagration()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001D09 RID: 7433
		// (get) Token: 0x06006F8D RID: 28557 RVA: 0x00287A4C File Offset: 0x00285C4C
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NGroundFireVfx.AssetPaths;
			}
		}

		// Token: 0x17001D0A RID: 7434
		// (get) Token: 0x06006F8E RID: 28558 RVA: 0x00287A53 File Offset: 0x00285C53
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(2m, ValueProp.Move),
					new RepeatVar(4)
				});
			}
		}

		// Token: 0x06006F8F RID: 28559 RVA: 0x00287A78 File Offset: 0x00285C78
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
			foreach (Creature creature in hittableEnemies)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(creature, VfxColor.Red));
				}
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue).FromCard(this, cardPlay)
				.TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x06006F90 RID: 28560 RVA: 0x00287ACB File Offset: 0x00285CCB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Repeat.UpgradeValueBy(1m);
		}
	}
}
