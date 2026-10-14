using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Increased Max HP by a great amount and won.
	/// </summary>
	// Token: 0x02000B66 RID: 2918
	public class Glutton : Badge
	{
		/// <summary>
		/// Increased Max HP by a great amount and won.
		/// </summary>
		// Token: 0x06007AD4 RID: 31444 RVA: 0x0029F679 File Offset: 0x0029D879
		[NullableContext(1)]
		public Glutton(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "GLUTTON", true, false)
		{
		}

		// Token: 0x170021E2 RID: 8674
		// (get) Token: 0x06007AD5 RID: 31445 RVA: 0x0029F68C File Offset: 0x0029D88C
		public override BadgeRarity Rarity
		{
			get
			{
				int startingHp = SaveUtil.CharacterOrDeprecated(this._localPlayer.CharacterId).StartingHp;
				int num = this._localPlayer.MaxHp - startingHp;
				BadgeRarity badgeRarity;
				if (num >= 30)
				{
					if (num < 50)
					{
						badgeRarity = BadgeRarity.Silver;
					}
					else
					{
						badgeRarity = BadgeRarity.Gold;
					}
				}
				else if (num < 15)
				{
					badgeRarity = BadgeRarity.None;
				}
				else
				{
					badgeRarity = BadgeRarity.Bronze;
				}
				return badgeRarity;
			}
		}

		// Token: 0x06007AD6 RID: 31446 RVA: 0x0029F6DF File Offset: 0x0029D8DF
		public override bool IsObtained()
		{
			return this.Rarity > BadgeRarity.None;
		}
	}
}
