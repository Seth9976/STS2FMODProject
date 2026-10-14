using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009F1 RID: 2545
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HandOfGreed : CardModel
	{
		// Token: 0x060072B4 RID: 29364 RVA: 0x0028DD85 File Offset: 0x0028BF85
		public HandOfGreed()
			: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E71 RID: 7793
		// (get) Token: 0x060072B5 RID: 29365 RVA: 0x0028DD92 File Offset: 0x0028BF92
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001E72 RID: 7794
		// (get) Token: 0x060072B6 RID: 29366 RVA: 0x0028DD95 File Offset: 0x0028BF95
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(20m, ValueProp.Move),
					new DynamicVar("Gold", 20m)
				});
			}
		}

		// Token: 0x17001E73 RID: 7795
		// (get) Token: 0x060072B7 RID: 29367 RVA: 0x0028DDC6 File Offset: 0x0028BFC6
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Fatal, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060072B8 RID: 29368 RVA: 0x0028DDD8 File Offset: 0x0028BFD8
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(5m);
			base.DynamicVars["Gold"].UpgradeValueBy(5m);
		}

		// Token: 0x060072B9 RID: 29369 RVA: 0x0028DE0C File Offset: 0x0028C00C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			bool shouldTriggerFatal = cardPlay.Target.Powers.All<PowerModel>((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
			Vector2? monsterPos = null;
			if (TestMode.IsOff)
			{
				NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(cardPlay.Target);
				monsterPos = ((creatureNode != null) ? new Vector2?(creatureNode.VfxSpawnPosition) : null);
			}
			AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			AttackCommand attackCommand2 = attackCommand;
			if (shouldTriggerFatal)
			{
				if (attackCommand2.Results.SelectMany<List<DamageResult>, DamageResult>((List<DamageResult> r) => r).Any<DamageResult>((DamageResult r) => r.WasTargetKilled))
				{
					if (monsterPos != null)
					{
						Vector2 value = monsterPos.Value;
						string text = "vfx/vfx_coin_explosion_regular";
						NCombatRoom instance = NCombatRoom.Instance;
						VfxCmd.PlayVfx(value, text, (instance != null) ? instance.CombatVfxContainer : null);
					}
					await PlayerCmd.GainGold(base.DynamicVars["Gold"].IntValue, base.Owner, false);
				}
			}
		}

		// Token: 0x0400287D RID: 10365
		public const int goldAmount = 20;

		// Token: 0x0400287E RID: 10366
		private const string _goldKey = "Gold";
	}
}
