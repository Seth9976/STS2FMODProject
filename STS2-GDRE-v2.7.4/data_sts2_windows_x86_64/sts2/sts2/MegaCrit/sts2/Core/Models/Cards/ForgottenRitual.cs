using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009D2 RID: 2514
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ForgottenRitual : CardModel
	{
		// Token: 0x06007204 RID: 29188 RVA: 0x0028C80A File Offset: 0x0028AA0A
		public ForgottenRitual()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E23 RID: 7715
		// (get) Token: 0x06007205 RID: 29189 RVA: 0x0028C817 File Offset: 0x0028AA17
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return this.WasCardExhaustedThisTurn;
			}
		}

		// Token: 0x17001E24 RID: 7716
		// (get) Token: 0x06007206 RID: 29190 RVA: 0x0028C81F File Offset: 0x0028AA1F
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001E25 RID: 7717
		// (get) Token: 0x06007207 RID: 29191 RVA: 0x0028C827 File Offset: 0x0028AA27
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(3));
			}
		}

		// Token: 0x17001E26 RID: 7718
		// (get) Token: 0x06007208 RID: 29192 RVA: 0x0028C834 File Offset: 0x0028AA34
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					base.EnergyHoverTip,
					HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
				});
			}
		}

		// Token: 0x17001E27 RID: 7719
		// (get) Token: 0x06007209 RID: 29193 RVA: 0x0028C853 File Offset: 0x0028AA53
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NGroundFireVfx.AssetPaths;
			}
		}

		// Token: 0x0600720A RID: 29194 RVA: 0x0028C85C File Offset: 0x0028AA5C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (this.WasCardExhaustedThisTurn)
			{
				NGroundFireVfx ngroundFireVfx = NGroundFireVfx.Create(base.Owner.Creature, VfxColor.Purple);
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.CombatVfxContainer.AddChildSafely(ngroundFireVfx);
				}
				await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
				await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
			}
		}

		// Token: 0x0600720B RID: 29195 RVA: 0x0028C89F File Offset: 0x0028AA9F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}

		// Token: 0x17001E28 RID: 7720
		// (get) Token: 0x0600720C RID: 29196 RVA: 0x0028C8B6 File Offset: 0x0028AAB6
		private bool WasCardExhaustedThisTurn
		{
			get
			{
				return CombatManager.Instance.History.Entries.OfType<CardExhaustedEntry>().Any<CardExhaustedEntry>((CardExhaustedEntry e) => e.HappenedThisTurn(base.CombatState) && e.Actor == base.Owner.Creature);
			}
		}
	}
}
