using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009E7 RID: 2535
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GrandFinale : CardModel
	{
		// Token: 0x06007280 RID: 29312 RVA: 0x0028D7E0 File Offset: 0x0028B9E0
		public GrandFinale()
			: base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001E58 RID: 7768
		// (get) Token: 0x06007281 RID: 29313 RVA: 0x0028D7ED File Offset: 0x0028B9ED
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return this.IsPlayable;
			}
		}

		// Token: 0x17001E59 RID: 7769
		// (get) Token: 0x06007282 RID: 29314 RVA: 0x0028D7F5 File Offset: 0x0028B9F5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(60m, ValueProp.Move));
			}
		}

		// Token: 0x06007283 RID: 29315 RVA: 0x0028D80C File Offset: 0x0028BA0C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			NGrandFinaleVfx ngrandFinaleVfx = NGrandFinaleVfx.Create(base.Owner.Creature);
			if (ngrandFinaleVfx != null)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.CombatVfxContainer.AddChildSafely(ngrandFinaleVfx);
				}
				await Cmd.Wait(NGrandFinaleVfx.totalAnticipationDuration, false);
			}
			AttackCommand attackCommand = DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState);
			Func<Creature, Node2D> func;
			if ((func = GrandFinale.<>O.<0>__Create) == null)
			{
				func = (GrandFinale.<>O.<0>__Create = new Func<Creature, Node2D>(NGrandFinaleImpactVfx.Create));
			}
			await attackCommand.WithHitVfxNode(func).WithHitFx(null, null, "blunt_attack.mp3").Execute(choiceContext);
		}

		// Token: 0x17001E5A RID: 7770
		// (get) Token: 0x06007284 RID: 29316 RVA: 0x0028D85F File Offset: 0x0028BA5F
		protected override bool IsPlayable
		{
			get
			{
				return PileType.Draw.GetPile(base.Owner).Cards.Count == 0;
			}
		}

		// Token: 0x06007285 RID: 29317 RVA: 0x0028D87A File Offset: 0x0028BA7A
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(15m);
		}

		// Token: 0x020020E1 RID: 8417
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04008839 RID: 34873
			[Nullable(new byte[] { 0, 1, 2 })]
			public static Func<Creature, Node2D> <0>__Create;
		}
	}
}
