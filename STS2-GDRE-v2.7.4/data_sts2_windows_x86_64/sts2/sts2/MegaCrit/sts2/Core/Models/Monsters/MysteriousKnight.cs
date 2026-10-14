using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007D4 RID: 2004
	public class MysteriousKnight : FlailKnight
	{
		// Token: 0x06006309 RID: 25353 RVA: 0x00265C28 File Offset: 0x00263E28
		[NullableContext(1)]
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, 6m, base.Creature, null, false);
			await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), base.Creature, 6m, base.Creature, null, false);
		}
	}
}
