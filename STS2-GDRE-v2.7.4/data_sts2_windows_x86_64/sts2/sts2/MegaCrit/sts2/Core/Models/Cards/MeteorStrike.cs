using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A3D RID: 2621
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeteorStrike : CardModel
	{
		// Token: 0x06007453 RID: 29779 RVA: 0x0029152B File Offset: 0x0028F72B
		public MeteorStrike()
			: base(5, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F18 RID: 7960
		// (get) Token: 0x06007454 RID: 29780 RVA: 0x00291538 File Offset: 0x0028F738
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x17001F19 RID: 7961
		// (get) Token: 0x06007455 RID: 29781 RVA: 0x00291547 File Offset: 0x0028F747
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(24m, ValueProp.Move));
			}
		}

		// Token: 0x17001F1A RID: 7962
		// (get) Token: 0x06007456 RID: 29782 RVA: 0x0029155B File Offset: 0x0028F75B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<PlasmaOrb>()
				});
			}
		}

		// Token: 0x06007457 RID: 29783 RVA: 0x00291580 File Offset: 0x0028F780
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(cardPlay.Target) : null);
			if (ncreature != null)
			{
				NLargeMagicMissileVfx nlargeMagicMissileVfx = NLargeMagicMissileVfx.Create(ncreature.GetBottomOfHitbox(), new Color("50b598"));
				NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(nlargeMagicMissileVfx);
				await Cmd.Wait(nlargeMagicMissileVfx.WaitTime, false);
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx(null, null, "blunt_attack.mp3")
				.Execute(choiceContext);
			for (int i = 0; i < 3; i++)
			{
				await OrbCmd.Channel<PlasmaOrb>(choiceContext, base.Owner);
			}
		}

		// Token: 0x06007458 RID: 29784 RVA: 0x002915D3 File Offset: 0x0028F7D3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(6m);
		}
	}
}
