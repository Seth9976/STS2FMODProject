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

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A5B RID: 2651
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Oblivion : CardModel
	{
		// Token: 0x060074F8 RID: 29944 RVA: 0x00292A3C File Offset: 0x00290C3C
		public Oblivion()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F62 RID: 8034
		// (get) Token: 0x060074F9 RID: 29945 RVA: 0x00292A49 File Offset: 0x00290C49
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DoomPower>(3m));
			}
		}

		// Token: 0x17001F63 RID: 8035
		// (get) Token: 0x060074FA RID: 29946 RVA: 0x00292A5C File Offset: 0x00290C5C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x060074FB RID: 29947 RVA: 0x00292A7C File Offset: 0x00290C7C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<OblivionPower>(choiceContext, cardPlay.Target, base.DynamicVars.Doom.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060074FC RID: 29948 RVA: 0x00292ACF File Offset: 0x00290CCF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Doom.UpgradeValueBy(1m);
		}
	}
}
