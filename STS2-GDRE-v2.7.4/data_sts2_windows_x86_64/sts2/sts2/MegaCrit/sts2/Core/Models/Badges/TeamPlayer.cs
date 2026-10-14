using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	// Token: 0x02000B73 RID: 2931
	public class TeamPlayer : Badge
	{
		// Token: 0x06007AFB RID: 31483 RVA: 0x002A018B File Offset: 0x0029E38B
		[NullableContext(1)]
		public TeamPlayer(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "TEAM_PLAYER", false, true)
		{
		}

		// Token: 0x170021EF RID: 8687
		// (get) Token: 0x06007AFC RID: 31484 RVA: 0x002A019D File Offset: 0x0029E39D
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Silver;
			}
		}

		// Token: 0x06007AFD RID: 31485 RVA: 0x002A01A0 File Offset: 0x0029E3A0
		public override bool IsObtained()
		{
			int num = 0;
			foreach (SerializableCard serializableCard in this._localPlayer.Deck)
			{
				if (SaveUtil.CardOrDeprecated(serializableCard.Id).MultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly)
				{
					num++;
				}
			}
			return num >= 3;
		}
	}
}
