using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A0A RID: 2570
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IceLance : CardModel
	{
		// Token: 0x06007339 RID: 29497 RVA: 0x0028EF73 File Offset: 0x0028D173
		public IceLance()
			: base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001EA7 RID: 7847
		// (get) Token: 0x0600733A RID: 29498 RVA: 0x0028EF80 File Offset: 0x0028D180
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<FrostOrb>()
				});
			}
		}

		// Token: 0x17001EA8 RID: 7848
		// (get) Token: 0x0600733B RID: 29499 RVA: 0x0028EFA3 File Offset: 0x0028D1A3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(19m, ValueProp.Move),
					new RepeatVar(3)
				});
			}
		}

		// Token: 0x0600733C RID: 29500 RVA: 0x0028EFCC File Offset: 0x0028D1CC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			for (int i = 0; i < base.DynamicVars.Repeat.IntValue; i++)
			{
				await OrbCmd.Channel<FrostOrb>(choiceContext, base.Owner);
			}
		}

		// Token: 0x0600733D RID: 29501 RVA: 0x0028F01F File Offset: 0x0028D21F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(5m);
		}
	}
}
