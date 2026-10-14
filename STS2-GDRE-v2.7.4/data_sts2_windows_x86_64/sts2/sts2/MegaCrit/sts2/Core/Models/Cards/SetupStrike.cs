using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000ABA RID: 2746
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SetupStrike : CardModel
	{
		// Token: 0x060076E5 RID: 30437 RVA: 0x0029699C File Offset: 0x00294B9C
		public SetupStrike()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002029 RID: 8233
		// (get) Token: 0x060076E6 RID: 30438 RVA: 0x002969AC File Offset: 0x00294BAC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x1700202A RID: 8234
		// (get) Token: 0x060076E7 RID: 30439 RVA: 0x002969CC File Offset: 0x00294BCC
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x1700202B RID: 8235
		// (get) Token: 0x060076E8 RID: 30440 RVA: 0x002969DB File Offset: 0x00294BDB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(7m, ValueProp.Move),
					new PowerVar<StrengthPower>(3m)
				});
			}
		}

		// Token: 0x060076E9 RID: 30441 RVA: 0x00296A08 File Offset: 0x00294C08
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await PowerCmd.Apply<SetupStrikePower>(choiceContext, base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060076EA RID: 30442 RVA: 0x00296A5B File Offset: 0x00294C5B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars.Strength.UpgradeValueBy(1m);
		}
	}
}
