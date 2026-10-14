using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B33 RID: 2867
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Void : CardModel
	{
		// Token: 0x0600797D RID: 31101 RVA: 0x0029BE72 File Offset: 0x0029A072
		public Void()
			: base(-1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x17002135 RID: 8501
		// (get) Token: 0x0600797E RID: 31102 RVA: 0x0029BE7F File Offset: 0x0029A07F
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17002136 RID: 8502
		// (get) Token: 0x0600797F RID: 31103 RVA: 0x0029BE82 File Offset: 0x0029A082
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x17002137 RID: 8503
		// (get) Token: 0x06007980 RID: 31104 RVA: 0x0029BE8F File Offset: 0x0029A08F
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Unplayable,
					CardKeyword.Ethereal
				});
			}
		}

		// Token: 0x06007981 RID: 31105 RVA: 0x0029BEA4 File Offset: 0x0029A0A4
		public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card == this)
			{
				await Cmd.Wait(0.25f, false);
				await PlayerCmd.LoseEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
			}
		}
	}
}
