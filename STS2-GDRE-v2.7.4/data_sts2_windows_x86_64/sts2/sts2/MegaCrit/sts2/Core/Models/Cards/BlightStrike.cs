using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000920 RID: 2336
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BlightStrike : CardModel
	{
		// Token: 0x06006E56 RID: 28246 RVA: 0x00285237 File Offset: 0x00283437
		public BlightStrike()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001C8A RID: 7306
		// (get) Token: 0x06006E57 RID: 28247 RVA: 0x00285244 File Offset: 0x00283444
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x17001C8B RID: 7307
		// (get) Token: 0x06006E58 RID: 28248 RVA: 0x00285253 File Offset: 0x00283453
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(8m, ValueProp.Move));
			}
		}

		// Token: 0x17001C8C RID: 7308
		// (get) Token: 0x06006E59 RID: 28249 RVA: 0x00285268 File Offset: 0x00283468
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x06006E5A RID: 28250 RVA: 0x00285288 File Offset: 0x00283488
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			AttackCommand attackCommand2 = attackCommand;
			await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, attackCommand2.Results.SelectMany<List<DamageResult>, DamageResult>((List<DamageResult> r) => r).Sum<DamageResult>((DamageResult r) => r.TotalDamage), base.Owner.Creature, this, false);
		}

		// Token: 0x06006E5B RID: 28251 RVA: 0x002852DB File Offset: 0x002834DB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}
	}
}
