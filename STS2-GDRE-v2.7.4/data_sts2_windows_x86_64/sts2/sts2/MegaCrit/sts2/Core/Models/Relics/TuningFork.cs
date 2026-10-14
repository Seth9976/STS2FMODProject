using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000605 RID: 1541
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TuningFork : RelicModel
	{
		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x060055BE RID: 21950 RVA: 0x002492CC File Offset: 0x002474CC
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x060055BF RID: 21951 RVA: 0x002492CF File Offset: 0x002474CF
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x060055C0 RID: 21952 RVA: 0x002492D2 File Offset: 0x002474D2
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.SkillsPlayed;
				}
				return base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x060055C1 RID: 21953 RVA: 0x002492F3 File Offset: 0x002474F3
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x060055C2 RID: 21954 RVA: 0x00249305 File Offset: 0x00247505
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(10),
					new BlockVar(7m, ValueProp.Unpowered)
				});
			}
		}

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x060055C3 RID: 21955 RVA: 0x0024932B File Offset: 0x0024752B
		// (set) Token: 0x060055C4 RID: 21956 RVA: 0x00249333 File Offset: 0x00247533
		private bool IsActivating
		{
			get
			{
				return this._isActivating;
			}
			set
			{
				base.AssertMutable();
				this._isActivating = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x060055C5 RID: 21957 RVA: 0x00249348 File Offset: 0x00247548
		// (set) Token: 0x060055C6 RID: 21958 RVA: 0x00249350 File Offset: 0x00247550
		[SavedProperty]
		public int SkillsPlayed
		{
			get
			{
				return this._skillsPlayed;
			}
			private set
			{
				base.AssertMutable();
				if (this._skillsPlayed != value)
				{
					this._skillsPlayed = value;
					this.UpdateDisplay();
				}
			}
		}

		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x060055C7 RID: 21959 RVA: 0x0024936E File Offset: 0x0024756E
		private int SkillsThreshold
		{
			get
			{
				return base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x060055C8 RID: 21960 RVA: 0x00249380 File Offset: 0x00247580
		private void UpdateDisplay()
		{
			if (this.IsActivating)
			{
				base.Status = RelicStatus.Normal;
			}
			else
			{
				base.Status = ((this.SkillsPlayed == this.SkillsThreshold - 1) ? RelicStatus.Active : RelicStatus.Normal);
			}
			base.InvokeDisplayAmountChanged();
		}

		// Token: 0x060055C9 RID: 21961 RVA: 0x002493B4 File Offset: 0x002475B4
		public void NotifySkillPlayed()
		{
			int skillsPlayed = this.SkillsPlayed;
			this.SkillsPlayed = skillsPlayed + 1;
		}

		// Token: 0x060055CA RID: 21962 RVA: 0x002493D4 File Offset: 0x002475D4
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (cardPlay.Card.Type == CardType.Skill)
				{
					int skillsPlayed = this.SkillsPlayed;
					this.SkillsPlayed = skillsPlayed + 1;
					if (this.SkillsPlayed >= this.SkillsThreshold)
					{
						TaskHelper.RunSafely(this.DoActivateVisuals());
						await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
						this.SkillsPlayed -= this.SkillsThreshold;
					}
				}
			}
		}

		// Token: 0x060055CB RID: 21963 RVA: 0x00249420 File Offset: 0x00247620
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x040024DA RID: 9434
		private bool _isActivating;

		// Token: 0x040024DB RID: 9435
		private int _skillsPlayed;
	}
}
