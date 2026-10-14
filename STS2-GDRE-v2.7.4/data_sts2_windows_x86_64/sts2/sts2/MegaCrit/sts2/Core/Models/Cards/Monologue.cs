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
	// Token: 0x02000A4B RID: 2635
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Monologue : CardModel
	{
		// Token: 0x060074A3 RID: 29859 RVA: 0x00291EA9 File Offset: 0x002900A9
		public Monologue()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F3C RID: 7996
		// (get) Token: 0x060074A4 RID: 29860 RVA: 0x00291EB6 File Offset: 0x002900B6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 1m));
			}
		}

		// Token: 0x17001F3D RID: 7997
		// (get) Token: 0x060074A5 RID: 29861 RVA: 0x00291ECC File Offset: 0x002900CC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x060074A6 RID: 29862 RVA: 0x00291EEC File Offset: 0x002900EC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			MonologuePower monologuePower = await PowerCmd.Apply<MonologuePower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
			if (monologuePower != null)
			{
				monologuePower.DynamicVars.Strength.BaseValue = base.DynamicVars["Power"].BaseValue;
			}
		}

		// Token: 0x060074A7 RID: 29863 RVA: 0x00291F37 File Offset: 0x00290137
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Retain);
		}

		// Token: 0x040028A5 RID: 10405
		private const string _powerKey = "Power";
	}
}
