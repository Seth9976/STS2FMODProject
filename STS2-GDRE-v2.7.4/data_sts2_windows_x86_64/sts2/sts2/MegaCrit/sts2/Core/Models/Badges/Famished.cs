using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Won with Max HP reduced to less than half of your starting HP.
	/// </summary>
	// Token: 0x02000B65 RID: 2917
	public class Famished : Badge
	{
		/// <summary>
		/// Won with Max HP reduced to less than half of your starting HP.
		/// </summary>
		// Token: 0x06007AD1 RID: 31441 RVA: 0x0029F62D File Offset: 0x0029D82D
		[NullableContext(1)]
		public Famished(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "FAMISHED", true, false)
		{
		}

		// Token: 0x170021E1 RID: 8673
		// (get) Token: 0x06007AD2 RID: 31442 RVA: 0x0029F63F File Offset: 0x0029D83F
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007AD3 RID: 31443 RVA: 0x0029F644 File Offset: 0x0029D844
		public override bool IsObtained()
		{
			int num = (SaveUtil.CharacterOrDeprecated(this._localPlayer.CharacterId).StartingHp + 1) / 2;
			return this._localPlayer.MaxHp < num;
		}
	}
}
