using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A06 RID: 2566
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HowlFromBeyond : CardModel
	{
		// Token: 0x06007321 RID: 29473 RVA: 0x0028EC18 File Offset: 0x0028CE18
		public HowlFromBeyond()
			: base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001E9E RID: 7838
		// (get) Token: 0x06007322 RID: 29474 RVA: 0x0028EC25 File Offset: 0x0028CE25
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(18m, ValueProp.Move));
			}
		}

		// Token: 0x17001E9F RID: 7839
		// (get) Token: 0x06007323 RID: 29475 RVA: 0x0028EC39 File Offset: 0x0028CE39
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06007324 RID: 29476 RVA: 0x0028EC48 File Offset: 0x0028CE48
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithAttackerAnim("Cast", base.Owner.Character.CastAnimDelay, null)
				.WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x06007325 RID: 29477 RVA: 0x0028EC9C File Offset: 0x0028CE9C
		public override async Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
		{
			CardPile pile = base.Pile;
			if (pile != null && pile.Type == PileType.Exhaust)
			{
				if (player == base.Owner)
				{
					await CardCmd.AutoPlay(choiceContext, this, null, AutoPlayType.Default, false, false);
				}
			}
		}

		// Token: 0x06007326 RID: 29478 RVA: 0x0028ECEF File Offset: 0x0028CEEF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(6m);
		}
	}
}
