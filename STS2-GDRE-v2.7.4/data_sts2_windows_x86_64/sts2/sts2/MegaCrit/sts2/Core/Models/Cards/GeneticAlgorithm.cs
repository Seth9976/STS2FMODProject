using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009DD RID: 2525
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GeneticAlgorithm : CardModel
	{
		// Token: 0x06007245 RID: 29253 RVA: 0x0028D05F File Offset: 0x0028B25F
		public GeneticAlgorithm()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001E40 RID: 7744
		// (get) Token: 0x06007246 RID: 29254 RVA: 0x0028D073 File Offset: 0x0028B273
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E41 RID: 7745
		// (get) Token: 0x06007247 RID: 29255 RVA: 0x0028D076 File Offset: 0x0028B276
		// (set) Token: 0x06007248 RID: 29256 RVA: 0x0028D07E File Offset: 0x0028B27E
		[SavedProperty]
		public int CurrentBlock
		{
			get
			{
				return this._currentBlock;
			}
			set
			{
				base.AssertMutable();
				this._currentBlock = value;
				base.DynamicVars.Block.BaseValue = this._currentBlock;
			}
		}

		// Token: 0x17001E42 RID: 7746
		// (get) Token: 0x06007249 RID: 29257 RVA: 0x0028D0A8 File Offset: 0x0028B2A8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(this.CurrentBlock, ValueProp.Move),
					new IntVar("Increase", 3m)
				});
			}
		}

		// Token: 0x17001E43 RID: 7747
		// (get) Token: 0x0600724A RID: 29258 RVA: 0x0028D0DC File Offset: 0x0028B2DC
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001E44 RID: 7748
		// (get) Token: 0x0600724B RID: 29259 RVA: 0x0028D0E4 File Offset: 0x0028B2E4
		// (set) Token: 0x0600724C RID: 29260 RVA: 0x0028D0EC File Offset: 0x0028B2EC
		[SavedProperty]
		public int IncreasedBlock
		{
			get
			{
				return this._increasedBlock;
			}
			set
			{
				base.AssertMutable();
				this._increasedBlock = value;
			}
		}

		// Token: 0x0600724D RID: 29261 RVA: 0x0028D0FC File Offset: 0x0028B2FC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			int intValue = base.DynamicVars["Increase"].IntValue;
			this.BuffFromPlay(intValue);
			GeneticAlgorithm geneticAlgorithm = base.DeckVersion as GeneticAlgorithm;
			if (geneticAlgorithm != null)
			{
				geneticAlgorithm.BuffFromPlay(intValue);
			}
		}

		// Token: 0x0600724E RID: 29262 RVA: 0x0028D147 File Offset: 0x0028B347
		protected override void OnUpgrade()
		{
			base.DynamicVars["Increase"].UpgradeValueBy(1m);
		}

		// Token: 0x0600724F RID: 29263 RVA: 0x0028D163 File Offset: 0x0028B363
		protected override void AfterDowngraded()
		{
			this.UpdateBlock();
		}

		// Token: 0x06007250 RID: 29264 RVA: 0x0028D16B File Offset: 0x0028B36B
		private void BuffFromPlay(int extraBlock)
		{
			this.IncreasedBlock += extraBlock;
			this.UpdateBlock();
		}

		// Token: 0x06007251 RID: 29265 RVA: 0x0028D181 File Offset: 0x0028B381
		private void UpdateBlock()
		{
			this.CurrentBlock = 1 + this.IncreasedBlock;
		}

		// Token: 0x04002874 RID: 10356
		private const string _increaseKey = "Increase";

		// Token: 0x04002875 RID: 10357
		private int _currentBlock = 1;

		// Token: 0x04002876 RID: 10358
		private int _increasedBlock;
	}
}
