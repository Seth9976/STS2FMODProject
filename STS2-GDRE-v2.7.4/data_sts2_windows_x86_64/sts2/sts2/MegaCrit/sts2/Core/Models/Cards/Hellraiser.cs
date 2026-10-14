using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009FD RID: 2557
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Hellraiser : CardModel
	{
		// Token: 0x060072F0 RID: 29424 RVA: 0x0028E5FC File Offset: 0x0028C7FC
		public Hellraiser()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001E88 RID: 7816
		// (get) Token: 0x060072F1 RID: 29425 RVA: 0x0028E609 File Offset: 0x0028C809
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NHellraiserVfx.AssetPaths;
			}
		}

		// Token: 0x060072F2 RID: 29426 RVA: 0x0028E610 File Offset: 0x0028C810
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<HellraiserPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x060072F3 RID: 29427 RVA: 0x0028E65C File Offset: 0x0028C85C
		public override async Task OnEnqueuePlayVfx([Nullable(2)] Creature target)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.CombatVfxContainer.AddChildSafely(NHellraiserVfx.Create(base.Owner.Creature));
			}
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
		}

		// Token: 0x060072F4 RID: 29428 RVA: 0x0028E69F File Offset: 0x0028C89F
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
