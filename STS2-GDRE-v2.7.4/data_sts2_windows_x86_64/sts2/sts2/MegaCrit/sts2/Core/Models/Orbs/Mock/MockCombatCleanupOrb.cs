using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Orbs.Mock
{
	/// <summary>
	/// This orb is for testing only. When this orb is Evoked, it removes its owner from combat, simulating the player
	/// exiting the run while an orb is evoking.
	/// </summary>
	// Token: 0x02000793 RID: 1939
	public class MockCombatCleanupOrb : OrbModel
	{
		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x06005F26 RID: 24358 RVA: 0x0025A407 File Offset: 0x00258607
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170015F8 RID: 5624
		// (get) Token: 0x06005F27 RID: 24359 RVA: 0x0025A40A File Offset: 0x0025860A
		public override decimal PassiveVal
		{
			get
			{
				return 0m;
			}
		}

		// Token: 0x170015F9 RID: 5625
		// (get) Token: 0x06005F28 RID: 24360 RVA: 0x0025A411 File Offset: 0x00258611
		public override decimal EvokeVal
		{
			get
			{
				return 0m;
			}
		}

		// Token: 0x170015FA RID: 5626
		// (get) Token: 0x06005F29 RID: 24361 RVA: 0x0025A418 File Offset: 0x00258618
		public override Color DarkenedColor
		{
			get
			{
				return new Color("000000");
			}
		}

		// Token: 0x06005F2A RID: 24362 RVA: 0x0025A424 File Offset: 0x00258624
		[NullableContext(1)]
		public override Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
		{
			base.Owner.Creature.CombatState.RemoveCreature(base.Owner.Creature, true);
			return Task.FromResult<IEnumerable<Creature>>(Array.Empty<Creature>());
		}
	}
}
