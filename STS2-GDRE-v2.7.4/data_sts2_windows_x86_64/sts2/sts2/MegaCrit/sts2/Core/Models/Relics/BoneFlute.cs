using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000508 RID: 1288
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BoneFlute : RelicModel
	{
		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06004F4E RID: 20302 RVA: 0x0023D12B File Offset: 0x0023B32B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06004F4F RID: 20303 RVA: 0x0023D12E File Offset: 0x0023B32E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(2m, ValueProp.Unpowered));
			}
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x0023D144 File Offset: 0x0023B344
		public override Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
		{
			Creature attacker = command.Attacker;
			if (!(((attacker != null) ? attacker.Monster : null) is Osty))
			{
				return Task.CompletedTask;
			}
			Player petOwner = command.Attacker.PetOwner;
			if (((petOwner != null) ? petOwner.Creature : null) != base.Owner.Creature)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			return CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
		}
	}
}
