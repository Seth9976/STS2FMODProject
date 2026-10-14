using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Achievements
{
	/// <summary>
	/// Grants an achievement when the player exhausts a large number of cards in a single turn.
	/// </summary>
	// Token: 0x02000B86 RID: 2950
	[NullableContext(1)]
	[Nullable(0)]
	public class SkillIronclad1Achievement : AchievementModel
	{
		// Token: 0x06007B94 RID: 31636 RVA: 0x002A0ECF File Offset: 0x0029F0CF
		public override Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
		{
			if (!LocalContext.IsMine(card))
			{
				return Task.CompletedTask;
			}
			this._cardsExhaustedThisCombat++;
			if (this._cardsExhaustedThisCombat >= 20)
			{
				AchievementsUtil.Unlock(Achievement.CharacterSkillIronclad1, card.Owner);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06007B95 RID: 31637 RVA: 0x002A0F09 File Offset: 0x0029F109
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			this._cardsExhaustedThisCombat = 0;
			return Task.CompletedTask;
		}

		// Token: 0x04002909 RID: 10505
		private const int _exhaustRequirement = 20;

		// Token: 0x0400290A RID: 10506
		private int _cardsExhaustedThisCombat;
	}
}
