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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000959 RID: 2393
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Comet : CardModel
	{
		// Token: 0x06006F76 RID: 28534 RVA: 0x002876F3 File Offset: 0x002858F3
		public Comet()
			: base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001CFF RID: 7423
		// (get) Token: 0x06006F77 RID: 28535 RVA: 0x00287700 File Offset: 0x00285900
		public override int CanonicalStarCost
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17001D00 RID: 7424
		// (get) Token: 0x06006F78 RID: 28536 RVA: 0x00287703 File Offset: 0x00285903
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(33m, ValueProp.Move),
					new PowerVar<VulnerablePower>(3m),
					new PowerVar<WeakPower>(3m)
				});
			}
		}

		// Token: 0x17001D01 RID: 7425
		// (get) Token: 0x06006F79 RID: 28537 RVA: 0x0028773C File Offset: 0x0028593C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<WeakPower>(null),
					HoverTipFactory.FromPower<VulnerablePower>(null)
				});
			}
		}

		// Token: 0x06006F7A RID: 28538 RVA: 0x00287778 File Offset: 0x00285978
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(cardPlay.Target) : null);
			if (ncreature != null)
			{
				NSmallMagicMissileVfx nsmallMagicMissileVfx = NSmallMagicMissileVfx.Create(ncreature.GetBottomOfHitbox(), new Color("50b598"));
				NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(nsmallMagicMissileVfx);
				await Cmd.Wait(nsmallMagicMissileVfx.WaitTime, false);
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithNoAttackerAnim()
				.Execute(choiceContext);
			await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006F7B RID: 28539 RVA: 0x002877CB File Offset: 0x002859CB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(11m);
		}
	}
}
