using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009DE RID: 2526
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GiantRock : CardModel
	{
		// Token: 0x06007252 RID: 29266 RVA: 0x0028D191 File Offset: 0x0028B391
		public GiantRock()
			: base(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E45 RID: 7749
		// (get) Token: 0x06007253 RID: 29267 RVA: 0x0028D19E File Offset: 0x0028B39E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(20m, ValueProp.Move));
			}
		}

		// Token: 0x06007254 RID: 29268 RVA: 0x0028D1B4 File Offset: 0x0028B3B4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_rock_shatter", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x06007255 RID: 29269 RVA: 0x0028D207 File Offset: 0x0028B407
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
