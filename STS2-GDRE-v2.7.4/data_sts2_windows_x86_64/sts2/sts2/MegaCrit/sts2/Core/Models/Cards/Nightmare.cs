using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A54 RID: 2644
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Nightmare : CardModel
	{
		// Token: 0x060074D1 RID: 29905 RVA: 0x00292508 File Offset: 0x00290708
		public Nightmare()
			: base(3, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F50 RID: 8016
		// (get) Token: 0x060074D2 RID: 29906 RVA: 0x00292515 File Offset: 0x00290715
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001F51 RID: 8017
		// (get) Token: 0x060074D3 RID: 29907 RVA: 0x00292518 File Offset: 0x00290718
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001F52 RID: 8018
		// (get) Token: 0x060074D4 RID: 29908 RVA: 0x00292520 File Offset: 0x00290720
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NNightmareHandsVfx.AssetPaths;
			}
		}

		// Token: 0x060074D5 RID: 29909 RVA: 0x00292528 File Offset: 0x00290728
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, cardSelectorPrefs, null, this);
			IEnumerable<CardModel> cards = enumerable;
			if (TestMode.IsOff && LocalContext.IsMe(base.Owner))
			{
				NSmokyVignetteVfx nsmokyVignetteVfx = NSmokyVignetteVfx.Create(new Color(0.8f, 0.3f, 0.8f, 0.66f), new Color(0f, 0f, 4f, 0.33f));
				NGame.Instance.CurrentRunNode.GlobalUi.AddChildSafely(nsmokyVignetteVfx);
				NGame.Instance.CurrentRunNode.GlobalUi.AddChildSafely(NNightmareHandsVfx.Create());
				await Cmd.CustomScaledWait(0.1f, 0.25f, false, default(CancellationToken));
			}
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			CardModel selectedCard = cards.FirstOrDefault<CardModel>();
			if (selectedCard != null)
			{
				NightmarePower nightmarePower = await PowerCmd.Apply<NightmarePower>(choiceContext, base.Owner.Creature, 3m, base.Owner.Creature, this, false);
				nightmarePower.SetSelectedCard(selectedCard);
			}
		}

		// Token: 0x060074D6 RID: 29910 RVA: 0x00292573 File Offset: 0x00290773
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
