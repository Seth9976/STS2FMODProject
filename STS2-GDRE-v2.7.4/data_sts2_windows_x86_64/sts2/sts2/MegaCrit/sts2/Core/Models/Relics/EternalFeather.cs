using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200053D RID: 1341
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EternalFeather : RelicModel
	{
		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x0600507E RID: 20606 RVA: 0x0023F3C3 File Offset: 0x0023D5C3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x0600507F RID: 20607 RVA: 0x0023F3C6 File Offset: 0x0023D5C6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(5),
					new HealVar(3m)
				});
			}
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x0023F3EC File Offset: 0x0023D5EC
		public override async Task AfterRoomEntered(AbstractRoom room)
		{
			if (room is RestSiteRoom)
			{
				base.Flash();
				int num = PileType.Deck.GetPile(base.Owner).Cards.Count / base.DynamicVars.Cards.IntValue;
				decimal healAmount = base.DynamicVars.Heal.BaseValue * num;
				await CreatureCmd.Heal(base.Owner.Creature, healAmount, true);
				if (LocalContext.IsMe(base.Owner))
				{
					PlayerFullscreenHealVfx.Play(base.Owner, healAmount, NRestSiteRoom.Instance);
				}
			}
		}
	}
}
