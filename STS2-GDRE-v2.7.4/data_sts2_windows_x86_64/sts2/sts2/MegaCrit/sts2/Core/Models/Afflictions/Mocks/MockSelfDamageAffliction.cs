using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Afflictions.Mocks
{
	// Token: 0x02000B7E RID: 2942
	public sealed class MockSelfDamageAffliction : AfflictionModel
	{
		// Token: 0x170021FC RID: 8700
		// (get) Token: 0x06007B18 RID: 31512 RVA: 0x002A0340 File Offset: 0x0029E540
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021FD RID: 8701
		// (get) Token: 0x06007B19 RID: 31513 RVA: 0x002A0343 File Offset: 0x0029E543
		public override bool IsStackable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007B1A RID: 31514 RVA: 0x002A0348 File Offset: 0x0029E548
		[NullableContext(1)]
		public override async Task OnPlay(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			await CreatureCmd.Damage(choiceContext, base.Card.Owner.Creature, base.Amount, ValueProp.Unpowered | ValueProp.Move, null, null);
		}
	}
}
