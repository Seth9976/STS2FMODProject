using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000ACE RID: 2766
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Slimed : CardModel
	{
		// Token: 0x06007756 RID: 30550 RVA: 0x002977CB File Offset: 0x002959CB
		public Slimed()
			: base(1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x1700205B RID: 8283
		// (get) Token: 0x06007757 RID: 30551 RVA: 0x002977D8 File Offset: 0x002959D8
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700205C RID: 8284
		// (get) Token: 0x06007758 RID: 30552 RVA: 0x002977DB File Offset: 0x002959DB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));
			}
		}

		// Token: 0x1700205D RID: 8285
		// (get) Token: 0x06007759 RID: 30553 RVA: 0x002977E8 File Offset: 0x002959E8
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x0600775A RID: 30554 RVA: 0x002977F0 File Offset: 0x002959F0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			NGoopyImpactVfx ngoopyImpactVfx = NGoopyImpactVfx.Create(base.Owner.Creature);
			if (ngoopyImpactVfx != null)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.CombatVfxContainer.AddChildSafely(ngoopyImpactVfx);
				}
			}
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}
	}
}
