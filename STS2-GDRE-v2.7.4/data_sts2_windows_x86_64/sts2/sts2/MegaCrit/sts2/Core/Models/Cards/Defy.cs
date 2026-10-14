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
	// Token: 0x02000989 RID: 2441
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Defy : CardModel
	{
		// Token: 0x06007072 RID: 28786 RVA: 0x0028963F File Offset: 0x0028783F
		public Defy()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D6F RID: 7535
		// (get) Token: 0x06007073 RID: 28787 RVA: 0x0028964C File Offset: 0x0028784C
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D70 RID: 7536
		// (get) Token: 0x06007074 RID: 28788 RVA: 0x0028964F File Offset: 0x0028784F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(6m, ValueProp.Move),
					new PowerVar<WeakPower>(1m)
				});
			}
		}

		// Token: 0x17001D71 RID: 7537
		// (get) Token: 0x06007075 RID: 28789 RVA: 0x00289678 File Offset: 0x00287878
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
			}
		}

		// Token: 0x17001D72 RID: 7538
		// (get) Token: 0x06007076 RID: 28790 RVA: 0x00289680 File Offset: 0x00287880
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>(null));
			}
		}

		// Token: 0x06007077 RID: 28791 RVA: 0x002896A0 File Offset: 0x002878A0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007078 RID: 28792 RVA: 0x002896F3 File Offset: 0x002878F3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
