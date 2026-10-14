using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000566 RID: 1382
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InfusedCore : RelicModel
	{
		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06005186 RID: 20870 RVA: 0x00241171 File Offset: 0x0023F371
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Starter;
			}
		}

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06005187 RID: 20871 RVA: 0x00241174 File Offset: 0x0023F374
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("Lightning", 3m),
					new DynamicVar("ExtraDamage", 1m)
				});
			}
		}

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06005188 RID: 20872 RVA: 0x002411A6 File Offset: 0x0023F3A6
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<LightningOrb>()
				});
			}
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x002411CC File Offset: 0x0023F3CC
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					int i = 0;
					while (i < base.DynamicVars["Lightning"].BaseValue)
					{
						await OrbCmd.Channel<LightningOrb>(new BlockingPlayerChoiceContext(), base.Owner);
						i++;
					}
				}
			}
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x00241217 File Offset: 0x0023F417
		public override decimal ModifyOrbValue(OrbModel orb, decimal value)
		{
			if (orb.Owner != base.Owner)
			{
				return value;
			}
			if (!(orb is LightningOrb))
			{
				return value;
			}
			return value + base.DynamicVars["ExtraDamage"].BaseValue;
		}

		// Token: 0x0400245C RID: 9308
		private const string _lightningKey = "Lightning";

		// Token: 0x0400245D RID: 9309
		private const string _extraDamageKey = "ExtraDamage";
	}
}
