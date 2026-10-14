using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000902 RID: 2306
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Armaments : CardModel
	{
		// Token: 0x06006DBB RID: 28091 RVA: 0x00283E64 File Offset: 0x00282064
		public Armaments()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001C46 RID: 7238
		// (get) Token: 0x06006DBC RID: 28092 RVA: 0x00283E71 File Offset: 0x00282071
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001C47 RID: 7239
		// (get) Token: 0x06006DBD RID: 28093 RVA: 0x00283E74 File Offset: 0x00282074
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(5m, ValueProp.Move));
			}
		}

		// Token: 0x06006DBE RID: 28094 RVA: 0x00283E88 File Offset: 0x00282088
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			if (base.IsUpgraded)
			{
				using (IEnumerator<CardModel> enumerator = PileType.Hand.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.IsUpgradable).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CardModel cardModel = enumerator.Current;
						CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
					}
					return;
				}
			}
			CardModel cardModel2 = await CardSelectCmd.FromHandForUpgrade(choiceContext, base.Owner, this);
			if (cardModel2 != null)
			{
				CardCmd.Upgrade(cardModel2, CardPreviewStyle.HorizontalLayout);
			}
		}
	}
}
