using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Entities.CardRewardAlternatives
{
	/// <summary>
	/// Manages the extra, non card, options in a card reward screen.
	/// i.e. Skip (default option), Sacrifice (for Pael  Wing), Heal +2 (for Dream Catcher)
	/// </summary>
	// Token: 0x02000CCD RID: 3277
	[NullableContext(1)]
	[Nullable(0)]
	public class CardRewardAlternative
	{
		// Token: 0x1700245A RID: 9306
		// (get) Token: 0x06008508 RID: 34056 RVA: 0x002CB728 File Offset: 0x002C9928
		public string OptionId { get; }

		// Token: 0x1700245B RID: 9307
		// (get) Token: 0x06008509 RID: 34057 RVA: 0x002CB730 File Offset: 0x002C9930
		public LocString Title
		{
			get
			{
				return new LocString("card_reward_ui", "OPTION_" + this.OptionId.ToUpperInvariant() + ".name");
			}
		}

		// Token: 0x1700245C RID: 9308
		// (get) Token: 0x0600850A RID: 34058 RVA: 0x002CB756 File Offset: 0x002C9956
		public string Hotkey { get; }

		/// <summary>
		/// Action to perform when the alternative option is selected.
		/// </summary>
		// Token: 0x1700245D RID: 9309
		// (get) Token: 0x0600850B RID: 34059 RVA: 0x002CB75E File Offset: 0x002C995E
		// (set) Token: 0x0600850C RID: 34060 RVA: 0x002CB766 File Offset: 0x002C9966
		public Func<Task> OnSelect { get; private set; }

		/// <summary>
		/// The action to take after the alternate reward is selected. See the values for more information.
		/// </summary>
		// Token: 0x1700245E RID: 9310
		// (get) Token: 0x0600850D RID: 34061 RVA: 0x002CB76F File Offset: 0x002C996F
		// (set) Token: 0x0600850E RID: 34062 RVA: 0x002CB777 File Offset: 0x002C9977
		public PostAlternateCardRewardAction AfterSelected { get; private set; }

		// Token: 0x0600850F RID: 34063 RVA: 0x002CB780 File Offset: 0x002C9980
		public CardRewardAlternative(string optionId, PostAlternateCardRewardAction afterSelected)
			: this(optionId, () => Task.CompletedTask, afterSelected)
		{
		}

		// Token: 0x06008510 RID: 34064 RVA: 0x002CB7A9 File Offset: 0x002C99A9
		public CardRewardAlternative(string optionId, Func<Task> onSelect, PostAlternateCardRewardAction afterSelected)
		{
			this.OptionId = optionId;
			this.OnSelect = onSelect;
			this.AfterSelected = afterSelected;
			this.Hotkey = ((afterSelected == PostAlternateCardRewardAction.EndSelectionAndDoNotCompleteReward) ? MegaInput.cancel : MegaInput.viewExhaustPileAndTabRight);
		}

		/// <summary>
		/// Generates a list of extra options to use for the next card reward screen.
		/// This list will include extra options added by models like the Pael Wing relic if the player has them.
		/// </summary>
		/// <param name="cardReward">The reward for which alternatives are being generated.</param>
		/// <returns>List of rest extra options options.</returns>
		// Token: 0x06008511 RID: 34065 RVA: 0x002CB7E4 File Offset: 0x002C99E4
		public static IReadOnlyList<CardRewardAlternative> Generate(CardReward cardReward)
		{
			List<CardRewardAlternative> list = new List<CardRewardAlternative>();
			if (cardReward.CanSkip)
			{
				list.Add(new CardRewardAlternative("Skip", PostAlternateCardRewardAction.EndSelectionAndDoNotCompleteReward));
			}
			if (cardReward.CanReroll)
			{
				list.Add(new CardRewardAlternative("REROLL", delegate
				{
					cardReward.Reroll();
					return Task.CompletedTask;
				}, PostAlternateCardRewardAction.DoNothing));
			}
			Hook.ModifyCardRewardAlternatives(cardReward.Player.RunState, cardReward.Player, cardReward, list);
			if (list.Count > 2)
			{
				throw new InvalidOperationException("More than 2 card reward alternatives are not supported.");
			}
			return list;
		}
	}
}
