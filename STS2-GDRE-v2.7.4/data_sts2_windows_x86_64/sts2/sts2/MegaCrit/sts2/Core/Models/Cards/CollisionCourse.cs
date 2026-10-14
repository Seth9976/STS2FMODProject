using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000957 RID: 2391
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CollisionCourse : CardModel
	{
		// Token: 0x06006F6B RID: 28523 RVA: 0x0028758B File Offset: 0x0028578B
		public CollisionCourse()
			: base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001CFA RID: 7418
		// (get) Token: 0x06006F6C RID: 28524 RVA: 0x00287598 File Offset: 0x00285798
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Debris>(false));
			}
		}

		// Token: 0x17001CFB RID: 7419
		// (get) Token: 0x06006F6D RID: 28525 RVA: 0x002875A5 File Offset: 0x002857A5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(10m, ValueProp.Move));
			}
		}

		// Token: 0x06006F6E RID: 28526 RVA: 0x002875BC File Offset: 0x002857BC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			await CardPileCmd.AddGeneratedCardToCombat(base.CombatState.CreateCard<Debris>(base.Owner), PileType.Hand, base.Owner, CardPilePosition.Bottom);
			await Cmd.Wait(0.5f, false);
		}

		// Token: 0x06006F6F RID: 28527 RVA: 0x0028760F File Offset: 0x0028580F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
