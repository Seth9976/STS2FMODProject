using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009D8 RID: 2520
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Fusion : CardModel
	{
		// Token: 0x0600722C RID: 29228 RVA: 0x0028CCDF File Offset: 0x0028AEDF
		public Fusion()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E36 RID: 7734
		// (get) Token: 0x0600722D RID: 29229 RVA: 0x0028CCEC File Offset: 0x0028AEEC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<PlasmaOrb>()
				});
			}
		}

		// Token: 0x17001E37 RID: 7735
		// (get) Token: 0x0600722E RID: 29230 RVA: 0x0028CD0F File Offset: 0x0028AF0F
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x0600722F RID: 29231 RVA: 0x0028CD18 File Offset: 0x0028AF18
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await OrbCmd.Channel<PlasmaOrb>(choiceContext, base.Owner);
		}

		// Token: 0x06007230 RID: 29232 RVA: 0x0028CD63 File Offset: 0x0028AF63
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
