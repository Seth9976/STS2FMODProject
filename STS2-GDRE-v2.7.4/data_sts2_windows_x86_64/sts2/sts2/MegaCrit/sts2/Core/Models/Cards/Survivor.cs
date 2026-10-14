using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AFD RID: 2813
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Survivor : CardModel
	{
		// Token: 0x0600785A RID: 30810 RVA: 0x00299A65 File Offset: 0x00297C65
		public Survivor()
			: base(1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
		{
		}

		// Token: 0x170020C1 RID: 8385
		// (get) Token: 0x0600785B RID: 30811 RVA: 0x00299A72 File Offset: 0x00297C72
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170020C2 RID: 8386
		// (get) Token: 0x0600785C RID: 30812 RVA: 0x00299A75 File Offset: 0x00297C75
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(8m, ValueProp.Move));
			}
		}

		// Token: 0x0600785D RID: 30813 RVA: 0x00299A88 File Offset: 0x00297C88
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHandForDiscard(choiceContext, base.Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 1), null, this);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				await CardCmd.Discard(choiceContext, cardModel);
			}
		}

		// Token: 0x0600785E RID: 30814 RVA: 0x00299ADB File Offset: 0x00297CDB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
