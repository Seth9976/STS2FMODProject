using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008E1 RID: 2273
	public sealed class Sown : EnchantmentModel
	{
		// Token: 0x17001B85 RID: 7045
		// (get) Token: 0x06006C93 RID: 27795 RVA: 0x00282164 File Offset: 0x00280364
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006C94 RID: 27796 RVA: 0x00282168 File Offset: 0x00280368
		[NullableContext(1)]
		public override async Task OnPlay(PlayerChoiceContext choiceContext, [Nullable(2)] CardPlay cardPlay)
		{
			if (base.Status == EnchantmentStatus.Normal)
			{
				base.Status = EnchantmentStatus.Disabled;
				await PlayerCmd.GainEnergy(base.Amount, base.Card.Owner);
			}
		}
	}
}
