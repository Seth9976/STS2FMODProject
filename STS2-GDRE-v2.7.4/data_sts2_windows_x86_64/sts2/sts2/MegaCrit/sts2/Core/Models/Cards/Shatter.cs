using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AC2 RID: 2754
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Shatter : CardModel
	{
		// Token: 0x06007711 RID: 30481 RVA: 0x00296EEB File Offset: 0x002950EB
		public Shatter()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x1700203E RID: 8254
		// (get) Token: 0x06007712 RID: 30482 RVA: 0x00296EF8 File Offset: 0x002950F8
		public override OrbEvokeType OrbEvokeType
		{
			get
			{
				return OrbEvokeType.All;
			}
		}

		// Token: 0x1700203F RID: 8255
		// (get) Token: 0x06007713 RID: 30483 RVA: 0x00296EFB File Offset: 0x002950FB
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17002040 RID: 8256
		// (get) Token: 0x06007714 RID: 30484 RVA: 0x00296F03 File Offset: 0x00295103
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Evoke, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17002041 RID: 8257
		// (get) Token: 0x06007715 RID: 30485 RVA: 0x00296F15 File Offset: 0x00295115
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(7m, ValueProp.Move));
			}
		}

		// Token: 0x06007716 RID: 30486 RVA: 0x00296F28 File Offset: 0x00295128
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			int orbCount = base.Owner.PlayerCombatState.OrbQueue.Orbs.Count;
			for (int i = 0; i < orbCount; i++)
			{
				await OrbCmd.EvokeNext(choiceContext, base.Owner, false);
				await OrbCmd.EvokeNext(choiceContext, base.Owner, true);
			}
		}

		// Token: 0x06007717 RID: 30487 RVA: 0x00296F7B File Offset: 0x0029517B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
