using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A5E RID: 2654
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OneForAll : CardModel
	{
		// Token: 0x06007507 RID: 29959 RVA: 0x00292C2F File Offset: 0x00290E2F
		public OneForAll()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.AllAllies, true)
		{
		}

		// Token: 0x17001F68 RID: 8040
		// (get) Token: 0x06007508 RID: 29960 RVA: 0x00292C3C File Offset: 0x00290E3C
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001F69 RID: 8041
		// (get) Token: 0x06007509 RID: 29961 RVA: 0x00292C3F File Offset: 0x00290E3F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<OneForAllPower>(3m));
			}
		}

		// Token: 0x0600750A RID: 29962 RVA: 0x00292C54 File Offset: 0x00290E54
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ICombatState combatState = base.CombatState;
			foreach (Player player in (((combatState != null) ? combatState.Players : null) ?? Array.Empty<Player>()))
			{
				await CreatureCmd.TriggerAnim(player.Creature, "PowerUp", player.Character.PowerUpAnimDelay);
				await PowerCmd.Apply<OneForAllPower>(choiceContext, player.Creature, base.DynamicVars["OneForAllPower"].BaseValue, base.Owner.Creature, this, false);
				player = null;
			}
			IEnumerator<Player> enumerator = null;
		}

		// Token: 0x0600750B RID: 29963 RVA: 0x00292C9F File Offset: 0x00290E9F
		protected override void OnUpgrade()
		{
			base.DynamicVars["OneForAllPower"].UpgradeValueBy(1m);
		}
	}
}
