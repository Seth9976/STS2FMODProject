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
	// Token: 0x0200090B RID: 2315
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BadLuck : CardModel
	{
		// Token: 0x06006DE9 RID: 28137 RVA: 0x002843DF File Offset: 0x002825DF
		public BadLuck()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001C5C RID: 7260
		// (get) Token: 0x06006DEA RID: 28138 RVA: 0x002843ED File Offset: 0x002825ED
		public override bool CanBeGeneratedByModifiers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C5D RID: 7261
		// (get) Token: 0x06006DEB RID: 28139 RVA: 0x002843F0 File Offset: 0x002825F0
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001C5E RID: 7262
		// (get) Token: 0x06006DEC RID: 28140 RVA: 0x002843F3 File Offset: 0x002825F3
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Eternal,
					CardKeyword.Unplayable
				});
			}
		}

		// Token: 0x17001C5F RID: 7263
		// (get) Token: 0x06006DED RID: 28141 RVA: 0x00284408 File Offset: 0x00282608
		public override bool HasTurnEndInHandEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001C60 RID: 7264
		// (get) Token: 0x06006DEE RID: 28142 RVA: 0x0028440B File Offset: 0x0028260B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HpLossVar(13m));
			}
		}

		// Token: 0x06006DEF RID: 28143 RVA: 0x00284420 File Offset: 0x00282620
		protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			await Cmd.Wait(0.25f, false);
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, null);
		}
	}
}
