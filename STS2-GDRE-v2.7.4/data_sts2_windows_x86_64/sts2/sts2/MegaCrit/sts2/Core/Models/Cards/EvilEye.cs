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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009B1 RID: 2481
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EvilEye : CardModel
	{
		// Token: 0x0600714D RID: 29005 RVA: 0x0028AF34 File Offset: 0x00289134
		public EvilEye()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001DD5 RID: 7637
		// (get) Token: 0x0600714E RID: 29006 RVA: 0x0028AF41 File Offset: 0x00289141
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001DD6 RID: 7638
		// (get) Token: 0x0600714F RID: 29007 RVA: 0x0028AF44 File Offset: 0x00289144
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return this.WasCardExhaustedThisTurn;
			}
		}

		// Token: 0x17001DD7 RID: 7639
		// (get) Token: 0x06007150 RID: 29008 RVA: 0x0028AF4C File Offset: 0x0028914C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(8m, ValueProp.Move));
			}
		}

		// Token: 0x17001DD8 RID: 7640
		// (get) Token: 0x06007151 RID: 29009 RVA: 0x0028AF5F File Offset: 0x0028915F
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06007152 RID: 29010 RVA: 0x0028AF6C File Offset: 0x0028916C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_gaze");
			int blockGains = (this.WasCardExhaustedThisTurn ? 2 : 1);
			for (int i = 0; i < blockGains; i++)
			{
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			}
		}

		// Token: 0x06007153 RID: 29011 RVA: 0x0028AFB7 File Offset: 0x002891B7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}

		// Token: 0x17001DD9 RID: 7641
		// (get) Token: 0x06007154 RID: 29012 RVA: 0x0028AFCF File Offset: 0x002891CF
		private bool WasCardExhaustedThisTurn
		{
			get
			{
				return CombatManager.Instance.History.Entries.OfType<CardExhaustedEntry>().Any<CardExhaustedEntry>((CardExhaustedEntry e) => e.HappenedThisTurn(base.CombatState) && e.Actor == base.Owner.Creature);
			}
		}
	}
}
