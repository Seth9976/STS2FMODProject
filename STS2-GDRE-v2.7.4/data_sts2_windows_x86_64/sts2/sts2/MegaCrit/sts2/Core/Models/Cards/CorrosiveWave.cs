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

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000965 RID: 2405
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CorrosiveWave : CardModel
	{
		// Token: 0x06006FB8 RID: 28600 RVA: 0x00287F87 File Offset: 0x00286187
		public CorrosiveWave()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001D1D RID: 7453
		// (get) Token: 0x06006FB9 RID: 28601 RVA: 0x00287F94 File Offset: 0x00286194
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("CorrosiveWave", 2m));
			}
		}

		// Token: 0x17001D1E RID: 7454
		// (get) Token: 0x06006FBA RID: 28602 RVA: 0x00287FAC File Offset: 0x002861AC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x06006FBB RID: 28603 RVA: 0x00287FCC File Offset: 0x002861CC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<CorrosiveWavePower>(choiceContext, base.Owner.Creature, base.DynamicVars["CorrosiveWave"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006FBC RID: 28604 RVA: 0x00288017 File Offset: 0x00286217
		protected override void OnUpgrade()
		{
			base.DynamicVars["CorrosiveWave"].UpgradeValueBy(1m);
		}

		// Token: 0x04002857 RID: 10327
		private const string _powerKey = "CorrosiveWave";
	}
}
