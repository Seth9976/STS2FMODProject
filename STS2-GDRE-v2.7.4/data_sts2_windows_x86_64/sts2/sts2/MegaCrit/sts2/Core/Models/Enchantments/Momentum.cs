using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008D9 RID: 2265
	public sealed class Momentum : EnchantmentModel
	{
		// Token: 0x06006C70 RID: 27760 RVA: 0x00281E67 File Offset: 0x00280067
		public override bool CanEnchantCardType(CardType cardType)
		{
			return cardType == CardType.Attack;
		}

		// Token: 0x17001B7C RID: 7036
		// (get) Token: 0x06006C71 RID: 27761 RVA: 0x00281E6D File Offset: 0x0028006D
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B7D RID: 7037
		// (get) Token: 0x06006C72 RID: 27762 RVA: 0x00281E70 File Offset: 0x00280070
		public override bool ShowAmount
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B7E RID: 7038
		// (get) Token: 0x06006C73 RID: 27763 RVA: 0x00281E73 File Offset: 0x00280073
		// (set) Token: 0x06006C74 RID: 27764 RVA: 0x00281E7B File Offset: 0x0028007B
		private int ExtraDamage
		{
			get
			{
				return this._extraDamage;
			}
			set
			{
				base.AssertMutable();
				this._extraDamage = value;
			}
		}

		// Token: 0x06006C75 RID: 27765 RVA: 0x00281E8A File Offset: 0x0028008A
		[NullableContext(1)]
		public override Task OnPlay(PlayerChoiceContext choiceContext, [Nullable(2)] CardPlay cardPlay)
		{
			this.ExtraDamage += base.Amount;
			return Task.CompletedTask;
		}

		// Token: 0x06006C76 RID: 27766 RVA: 0x00281EA4 File Offset: 0x002800A4
		public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
		{
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			return this.ExtraDamage;
		}

		// Token: 0x04002835 RID: 10293
		private int _extraDamage;
	}
}
