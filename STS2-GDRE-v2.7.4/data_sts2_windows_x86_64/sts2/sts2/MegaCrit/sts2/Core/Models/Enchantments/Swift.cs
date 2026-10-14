using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008E4 RID: 2276
	public sealed class Swift : EnchantmentModel
	{
		// Token: 0x17001B89 RID: 7049
		// (get) Token: 0x06006C9E RID: 27806 RVA: 0x00282266 File Offset: 0x00280466
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B8A RID: 7050
		// (get) Token: 0x06006C9F RID: 27807 RVA: 0x00282269 File Offset: 0x00280469
		public override bool ShowAmount
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006CA0 RID: 27808 RVA: 0x0028226C File Offset: 0x0028046C
		[NullableContext(1)]
		public override async Task OnPlay(PlayerChoiceContext choiceContext, [Nullable(2)] CardPlay cardPlay)
		{
			if (base.Status == EnchantmentStatus.Normal)
			{
				base.Status = EnchantmentStatus.Disabled;
				await CardPileCmd.Draw(choiceContext, base.Amount, base.Card.Owner, false);
			}
		}
	}
}
