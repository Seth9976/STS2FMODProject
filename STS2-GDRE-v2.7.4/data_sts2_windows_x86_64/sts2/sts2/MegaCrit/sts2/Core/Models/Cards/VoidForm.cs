using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B34 RID: 2868
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VoidForm : CardModel
	{
		// Token: 0x06007982 RID: 31106 RVA: 0x0029BEEF File Offset: 0x0029A0EF
		public VoidForm()
			: base(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002138 RID: 8504
		// (get) Token: 0x06007983 RID: 31107 RVA: 0x0029BEFC File Offset: 0x0029A0FC
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
			}
		}

		// Token: 0x17002139 RID: 8505
		// (get) Token: 0x06007984 RID: 31108 RVA: 0x0029BF04 File Offset: 0x0029A104
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<VoidFormPower>(2m));
			}
		}

		// Token: 0x06007985 RID: 31109 RVA: 0x0029BF18 File Offset: 0x0029A118
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<VoidFormPower>(choiceContext, base.Owner.Creature, base.DynamicVars["VoidFormPower"].BaseValue, base.Owner.Creature, this, false);
			PlayerCmd.EndTurn(base.Owner, false, null);
		}

		// Token: 0x06007986 RID: 31110 RVA: 0x0029BF63 File Offset: 0x0029A163
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Ethereal);
		}
	}
}
