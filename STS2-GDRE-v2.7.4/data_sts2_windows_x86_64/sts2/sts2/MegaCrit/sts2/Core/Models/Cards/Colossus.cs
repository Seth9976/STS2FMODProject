using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000958 RID: 2392
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Colossus : CardModel
	{
		// Token: 0x06006F70 RID: 28528 RVA: 0x00287627 File Offset: 0x00285827
		public Colossus()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CFC RID: 7420
		// (get) Token: 0x06006F71 RID: 28529 RVA: 0x00287634 File Offset: 0x00285834
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x17001CFD RID: 7421
		// (get) Token: 0x06006F72 RID: 28530 RVA: 0x00287654 File Offset: 0x00285854
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CFE RID: 7422
		// (get) Token: 0x06006F73 RID: 28531 RVA: 0x00287657 File Offset: 0x00285857
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(4m, ValueProp.Move),
					new DynamicVar("Colossus", 1m)
				});
			}
		}

		// Token: 0x06006F74 RID: 28532 RVA: 0x00287688 File Offset: 0x00285888
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<ColossusPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Colossus"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006F75 RID: 28533 RVA: 0x002876DB File Offset: 0x002858DB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}

		// Token: 0x04002855 RID: 10325
		private const string _powerVarName = "Colossus";
	}
}
