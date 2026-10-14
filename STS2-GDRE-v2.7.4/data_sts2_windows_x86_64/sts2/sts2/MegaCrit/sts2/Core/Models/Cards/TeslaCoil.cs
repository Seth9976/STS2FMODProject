using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x02000B0B RID: 2827
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TeslaCoil : CardModel
	{
		// Token: 0x060078A1 RID: 30881 RVA: 0x0029A424 File Offset: 0x00298624
		public TeslaCoil()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020DE RID: 8414
		// (get) Token: 0x060078A2 RID: 30882 RVA: 0x0029A431 File Offset: 0x00298631
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromOrb<LightningOrb>());
			}
		}

		// Token: 0x170020DF RID: 8415
		// (get) Token: 0x060078A3 RID: 30883 RVA: 0x0029A43D File Offset: 0x0029863D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(3m, ValueProp.Move));
			}
		}

		// Token: 0x060078A4 RID: 30884 RVA: 0x0029A450 File Offset: 0x00298650
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			List<LightningOrb> list = base.Owner.PlayerCombatState.OrbQueue.Orbs.OfType<LightningOrb>().ToList<LightningOrb>();
			foreach (LightningOrb lightningOrb in list)
			{
				await OrbCmd.Passive(choiceContext, lightningOrb, cardPlay.Target, false);
				if (base.IsUpgraded)
				{
					await OrbCmd.Passive(choiceContext, lightningOrb, cardPlay.Target, false);
				}
				lightningOrb = null;
			}
			List<LightningOrb>.Enumerator enumerator = default(List<LightningOrb>.Enumerator);
		}

		// Token: 0x060078A5 RID: 30885 RVA: 0x0029A4A3 File Offset: 0x002986A3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
		}
	}
}
