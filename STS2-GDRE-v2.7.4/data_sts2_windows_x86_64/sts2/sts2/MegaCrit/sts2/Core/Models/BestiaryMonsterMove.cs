using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Models
{
	/// <summary>
	/// Encapsulates the data needed to play an animation and accompanied sfx for a
	/// "move" for a given creature in the Bestiary.
	/// </summary>
	// Token: 0x020004DB RID: 1243
	[NullableContext(1)]
	[Nullable(0)]
	public struct BestiaryMonsterMove
	{
		// Token: 0x06004ADC RID: 19164 RVA: 0x00227FA4 File Offset: 0x002261A4
		public static BestiaryMonsterMove FromAnim(string animId, [Nullable(2)] string sfx)
		{
			BestiaryMonsterMove bestiaryMonsterMove = default(BestiaryMonsterMove);
			bestiaryMonsterMove.animId = animId;
			string text;
			if (animId.StartsWith("attack"))
			{
				text = BestiaryMonsterMove._attackMoveName.GetRawText();
			}
			else if (!(animId == "cast"))
			{
				if (!(animId == "die"))
				{
					if (!(animId == "hurt"))
					{
						if (!(animId == "revive"))
						{
							if (!(animId == "stun"))
							{
								text = animId;
							}
							else
							{
								text = BestiaryMonsterMove._stunMoveName.GetRawText();
							}
						}
						else
						{
							text = BestiaryMonsterMove._reviveMoveName.GetRawText();
						}
					}
					else
					{
						text = BestiaryMonsterMove._hurtMoveName.GetRawText();
					}
				}
				else
				{
					text = BestiaryMonsterMove._dieMoveName.GetRawText();
				}
			}
			else
			{
				text = BestiaryMonsterMove._castMoveName.GetRawText();
			}
			bestiaryMonsterMove.displayName = text;
			bestiaryMonsterMove.sfx = sfx;
			return bestiaryMonsterMove;
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x00228076 File Offset: 0x00226276
		public BestiaryMonsterMove StopOtherSfx()
		{
			this.stopSfxLoops = true;
			return this;
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x00228088 File Offset: 0x00226288
		public static BestiaryMonsterMove FromAnim(LocString moveName, string animId, [Nullable(2)] string sfx)
		{
			return new BestiaryMonsterMove
			{
				animId = animId,
				displayName = moveName.GetRawText(),
				sfx = sfx
			};
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x002280BC File Offset: 0x002262BC
		public static BestiaryMonsterMove FromState(LocString moveName, string stateId)
		{
			return new BestiaryMonsterMove
			{
				displayName = moveName.GetRawText(),
				stateId = stateId
			};
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x002280E8 File Offset: 0x002262E8
		public static BestiaryMonsterMove FromState(string stateId)
		{
			return new BestiaryMonsterMove
			{
				displayName = stateId,
				stateId = stateId
			};
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x00228110 File Offset: 0x00226310
		public static BestiaryMonsterMove FromNonStateMove(LocString moveName, Func<IReadOnlyList<Creature>, Task> nonStateMove)
		{
			return new BestiaryMonsterMove
			{
				displayName = moveName.GetRawText(),
				nonStateMove = nonStateMove
			};
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x0022813C File Offset: 0x0022633C
		public static BestiaryMonsterMove FromAction(LocString moveName, Func<Task> action)
		{
			return new BestiaryMonsterMove
			{
				displayName = moveName.GetRawText(),
				action = action
			};
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x00228167 File Offset: 0x00226367
		public static BestiaryMonsterMove FromStun(Func<Task> action)
		{
			return BestiaryMonsterMove.FromAction(new LocString("monsters", "GENERIC.moves.STUNNED.title"), action);
		}

		// Token: 0x04001CE6 RID: 7398
		private static readonly LocString _attackMoveName = new LocString("bestiary", "ACTION_NAME.attack");

		// Token: 0x04001CE7 RID: 7399
		private static readonly LocString _castMoveName = new LocString("bestiary", "ACTION_NAME.cast");

		// Token: 0x04001CE8 RID: 7400
		private static readonly LocString _dieMoveName = new LocString("bestiary", "ACTION_NAME.die");

		// Token: 0x04001CE9 RID: 7401
		private static readonly LocString _hurtMoveName = new LocString("bestiary", "ACTION_NAME.hurt");

		// Token: 0x04001CEA RID: 7402
		private static readonly LocString _reviveMoveName = new LocString("bestiary", "ACTION_NAME.revive");

		// Token: 0x04001CEB RID: 7403
		private static readonly LocString _stunMoveName = new LocString("bestiary", "ACTION_NAME.stun");

		// Token: 0x04001CEC RID: 7404
		public string displayName;

		// Token: 0x04001CED RID: 7405
		[Nullable(2)]
		public string animId;

		// Token: 0x04001CEE RID: 7406
		[Nullable(2)]
		public string sfx;

		// Token: 0x04001CEF RID: 7407
		[Nullable(2)]
		public string stateId;

		// Token: 0x04001CF0 RID: 7408
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		public Func<IReadOnlyList<Creature>, Task> nonStateMove;

		// Token: 0x04001CF1 RID: 7409
		[Nullable(new byte[] { 2, 1 })]
		public Func<Task> action;

		// Token: 0x04001CF2 RID: 7410
		public bool stopSfxLoops;
	}
}
