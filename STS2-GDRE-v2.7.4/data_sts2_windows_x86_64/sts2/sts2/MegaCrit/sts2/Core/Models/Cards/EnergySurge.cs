using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009A6 RID: 2470
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EnergySurge : CardModel
	{
		// Token: 0x06007112 RID: 28946 RVA: 0x0028A8BB File Offset: 0x00288ABB
		public EnergySurge()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies, true)
		{
		}

		// Token: 0x17001DB9 RID: 7609
		// (get) Token: 0x06007113 RID: 28947 RVA: 0x0028A8C8 File Offset: 0x00288AC8
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001DBA RID: 7610
		// (get) Token: 0x06007114 RID: 28948 RVA: 0x0028A8CB File Offset: 0x00288ACB
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x17001DBB RID: 7611
		// (get) Token: 0x06007115 RID: 28949 RVA: 0x0028A8D8 File Offset: 0x00288AD8
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001DBC RID: 7612
		// (get) Token: 0x06007116 RID: 28950 RVA: 0x0028A8E0 File Offset: 0x00288AE0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x06007117 RID: 28951 RVA: 0x0028A8F0 File Offset: 0x00288AF0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
				where c != null && c.IsAlive && c.IsPlayer
				select c;
			foreach (Creature creature in enumerable)
			{
				await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, creature.Player);
			}
			IEnumerator<Creature> enumerator = null;
		}

		// Token: 0x06007118 RID: 28952 RVA: 0x0028A933 File Offset: 0x00288B33
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
