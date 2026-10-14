using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Events
{
	// Token: 0x02000C4D RID: 3149
	[NullableContext(1)]
	[Nullable(0)]
	public class EventOption
	{
		// Token: 0x1700235D RID: 9053
		// (get) Token: 0x0600819B RID: 33179 RVA: 0x002BF2A2 File Offset: 0x002BD4A2
		// (set) Token: 0x0600819C RID: 33180 RVA: 0x002BF2AA File Offset: 0x002BD4AA
		public string TextKey { get; private set; }

		// Token: 0x1700235E RID: 9054
		// (get) Token: 0x0600819D RID: 33181 RVA: 0x002BF2B3 File Offset: 0x002BD4B3
		// (set) Token: 0x0600819E RID: 33182 RVA: 0x002BF2BB File Offset: 0x002BD4BB
		public LocString Title { get; private set; }

		// Token: 0x1700235F RID: 9055
		// (get) Token: 0x0600819F RID: 33183 RVA: 0x002BF2C4 File Offset: 0x002BD4C4
		// (set) Token: 0x060081A0 RID: 33184 RVA: 0x002BF2CC File Offset: 0x002BD4CC
		public LocString Description { get; private set; }

		// Token: 0x17002360 RID: 9056
		// (get) Token: 0x060081A1 RID: 33185 RVA: 0x002BF2D5 File Offset: 0x002BD4D5
		[Nullable(new byte[] { 2, 1 })]
		private Func<Task> OnChosen
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
		}

		// Token: 0x17002361 RID: 9057
		// (get) Token: 0x060081A2 RID: 33186 RVA: 0x002BF2DD File Offset: 0x002BD4DD
		// (set) Token: 0x060081A3 RID: 33187 RVA: 0x002BF2E5 File Offset: 0x002BD4E5
		public IEnumerable<IHoverTip> HoverTips { get; set; }

		// Token: 0x17002362 RID: 9058
		// (get) Token: 0x060081A4 RID: 33188 RVA: 0x002BF2EE File Offset: 0x002BD4EE
		public bool IsLocked { get; }

		// Token: 0x17002363 RID: 9059
		// (get) Token: 0x060081A5 RID: 33189 RVA: 0x002BF2F6 File Offset: 0x002BD4F6
		// (set) Token: 0x060081A6 RID: 33190 RVA: 0x002BF2FE File Offset: 0x002BD4FE
		public bool IsProceed { get; private set; }

		// Token: 0x17002364 RID: 9060
		// (get) Token: 0x060081A7 RID: 33191 RVA: 0x002BF307 File Offset: 0x002BD507
		// (set) Token: 0x060081A8 RID: 33192 RVA: 0x002BF30F File Offset: 0x002BD50F
		public bool WasChosen { get; private set; }

		// Token: 0x17002365 RID: 9061
		// (get) Token: 0x060081A9 RID: 33193 RVA: 0x002BF318 File Offset: 0x002BD518
		// (set) Token: 0x060081AA RID: 33194 RVA: 0x002BF320 File Offset: 0x002BD520
		[Nullable(2)]
		public RelicModel Relic
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x17002366 RID: 9062
		// (get) Token: 0x060081AB RID: 33195 RVA: 0x002BF329 File Offset: 0x002BD529
		private bool DisableOnChosen { get; }

		// Token: 0x17002367 RID: 9063
		// (get) Token: 0x060081AC RID: 33196 RVA: 0x002BF331 File Offset: 0x002BD531
		// (set) Token: 0x060081AD RID: 33197 RVA: 0x002BF339 File Offset: 0x002BD539
		[Nullable(new byte[] { 2, 1 })]
		public Func<Player, bool> WillKillPlayer
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			private set;
		}

		// Token: 0x17002368 RID: 9064
		// (get) Token: 0x060081AE RID: 33198 RVA: 0x002BF342 File Offset: 0x002BD542
		// (set) Token: 0x060081AF RID: 33199 RVA: 0x002BF34A File Offset: 0x002BD54A
		public bool ShouldSaveChoiceToHistory { get; private set; } = true;

		// Token: 0x17002369 RID: 9065
		// (get) Token: 0x060081B0 RID: 33200 RVA: 0x002BF353 File Offset: 0x002BD553
		// (set) Token: 0x060081B1 RID: 33201 RVA: 0x002BF35B File Offset: 0x002BD55B
		public LocString HistoryName { get; private set; }

		// Token: 0x1700236A RID: 9066
		// (get) Token: 0x060081B2 RID: 33202 RVA: 0x002BF364 File Offset: 0x002BD564
		// (set) Token: 0x060081B3 RID: 33203 RVA: 0x002BF36C File Offset: 0x002BD56C
		public bool ShouldSaveVariablesToHistory { get; private set; } = true;

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x060081B4 RID: 33204 RVA: 0x002BF378 File Offset: 0x002BD578
		// (remove) Token: 0x060081B5 RID: 33205 RVA: 0x002BF3B0 File Offset: 0x002BD5B0
		[Nullable(new byte[] { 2, 1, 1 })]
		[field: Nullable(new byte[] { 2, 1, 1 })]
		public event Func<EventOption, Task> BeforeChosen;

		/// <summary>
		/// Create a new event option.
		/// </summary>
		/// <param name="eventModel">The event that this option belongs to.</param>
		/// <param name="onChosen">The function that will run when the option is chosen.</param>
		/// <param name="title">Option title.</param>
		/// <param name="description">Option description.</param>
		/// <param name="textKey">
		/// Localization key prefix for this option. Does not include ".title" or ".description".
		/// </param>
		/// <param name="hoverTips">HoverTips that should be shown when hovering this option.</param>
		// Token: 0x060081B6 RID: 33206 RVA: 0x002BF3E8 File Offset: 0x002BD5E8
		public EventOption(EventModel eventModel, [Nullable(new byte[] { 2, 1 })] Func<Task> onChosen, LocString title, LocString description, string textKey, IEnumerable<IHoverTip> hoverTips)
		{
			this.TextKey = textKey;
			this.OnChosen = onChosen;
			this.Title = title;
			this.Description = description;
			this.HoverTips = hoverTips;
			this.IsLocked = this.OnChosen == null;
			this.DisableOnChosen = true;
			this.HistoryName = title;
			this.AddLocVars(eventModel);
		}

		/// <summary>
		/// Create a new event option.
		/// </summary>
		/// <param name="eventModel">The event that this option belongs to.</param>
		/// <param name="onChosen">The function that will run when the option is chosen.</param>
		/// <param name="textKey">
		/// Localization key prefix for this option. Does not include ".title" or ".description".
		/// </param>
		/// <param name="hoverTips">HoverTips that should be shown when hovering this option.</param>
		// Token: 0x060081B7 RID: 33207 RVA: 0x002BF454 File Offset: 0x002BD654
		public EventOption(EventModel eventModel, [Nullable(new byte[] { 2, 1 })] Func<Task> onChosen, string textKey, IEnumerable<IHoverTip> hoverTips)
		{
			this.TextKey = textKey;
			this.OnChosen = onChosen;
			this.Title = eventModel.GetOptionTitle(textKey);
			this.Description = eventModel.GetOptionDescription(textKey);
			this.HoverTips = hoverTips;
			this.IsLocked = this.OnChosen == null;
			this.DisableOnChosen = true;
			this.HistoryName = this.Title;
			this.AddLocVars(eventModel);
		}

		/// <summary>
		/// Create a new event option.
		/// </summary>
		/// <param name="eventModel">The event that this option belongs to.</param>
		/// <param name="onChosen">The function that will run when the option is chosen.</param>
		/// <param name="textKey">
		/// Localization key prefix for this option. Does not include ".title" or ".description".
		/// </param>
		/// <param name="hoverTips">HoverTips that should be shown when hovering this option.</param>
		// Token: 0x060081B8 RID: 33208 RVA: 0x002BF4CE File Offset: 0x002BD6CE
		public EventOption(EventModel eventModel, [Nullable(new byte[] { 2, 1 })] Func<Task> onChosen, string textKey, params IHoverTip[] hoverTips)
			: this(eventModel, onChosen, textKey, hoverTips.ToList<IHoverTip>())
		{
		}

		/// <summary>
		/// Create a new event option.
		/// </summary>
		/// <param name="eventModel">The event that this option belongs to.</param>
		/// <param name="onChosen">The function that will run when the option is chosen.</param>
		/// <param name="textKey">
		/// Localization key prefix for this option. Does not include ".title" or ".description".
		/// </param>
		/// <param name="disableOnChosen">
		/// Whether this option should be disabled after clicking it.
		/// Usually true, but false for options that have a confirmation dialog that can be canceled,
		/// so the option can be re-clicked after.
		/// </param>
		/// <param name="isProceed">Whether this option is a "proceed" button.</param>
		/// <param name="hoverTips">HoverTips that should be shown when hovering this option.</param>
		// Token: 0x060081B9 RID: 33209 RVA: 0x002BF4E0 File Offset: 0x002BD6E0
		public EventOption(EventModel eventModel, [Nullable(new byte[] { 2, 1 })] Func<Task> onChosen, string textKey, bool disableOnChosen = true, bool isProceed = false, params IHoverTip[] hoverTips)
			: this(eventModel, onChosen, textKey, hoverTips.ToList<IHoverTip>())
		{
			this.IsProceed = isProceed;
			this.DisableOnChosen = disableOnChosen;
		}

		// Token: 0x060081BA RID: 33210 RVA: 0x002BF504 File Offset: 0x002BD704
		public EventOption(EventOption eventOption)
		{
			this.TextKey = eventOption.TextKey;
			this.OnChosen = eventOption.OnChosen;
			this.Title = eventOption.Title;
			this.Description = eventOption.Description;
			this.HoverTips = eventOption.HoverTips;
			this.IsLocked = eventOption.IsLocked;
			this.DisableOnChosen = eventOption.DisableOnChosen;
			this.HistoryName = eventOption.HistoryName;
			this.IsProceed = eventOption.IsProceed;
			this.DisableOnChosen = eventOption.DisableOnChosen;
		}

		// Token: 0x060081BB RID: 33211 RVA: 0x002BF5A0 File Offset: 0x002BD7A0
		public static EventOption FromRelic(RelicModel relic, EventModel eventModel, [Nullable(new byte[] { 2, 1 })] Func<Task> onChosen, string textKey)
		{
			LocString locString = eventModel.GetOptionTitle(textKey) ?? relic.Title;
			LocString locString2 = eventModel.GetOptionDescription(textKey) ?? relic.DynamicEventDescription;
			return new EventOption(eventModel, onChosen, locString, locString2, textKey, relic.HoverTipsExcludingRelic).WithRelic(relic);
		}

		/// <summary>
		/// Associates a relic with this event option.
		/// Should be called on custom ancient event options that give relics so that they show up in the correct category
		/// in the relic library.
		/// </summary>
		// Token: 0x060081BC RID: 33212 RVA: 0x002BF5EC File Offset: 0x002BD7EC
		public EventOption WithRelic<[Nullable(0)] T>([Nullable(2)] Player owner) where T : RelicModel
		{
			RelicModel relicModel = ModelDb.Relic<T>().ToMutable();
			if (owner != null)
			{
				relicModel.Owner = owner;
			}
			return this.WithRelic(relicModel);
		}

		/// <summary>
		/// Associates a relic with this event option.
		/// Should be called on custom ancient event options that give relics so that they show up in the correct category
		/// in the relic library.
		/// </summary>
		// Token: 0x060081BD RID: 33213 RVA: 0x002BF61A File Offset: 0x002BD81A
		public EventOption WithRelic(RelicModel relic)
		{
			relic.AssertMutable();
			this.Relic = relic;
			return this;
		}

		// Token: 0x060081BE RID: 33214 RVA: 0x002BF62C File Offset: 0x002BD82C
		public async Task Chosen()
		{
			if (this.OnChosen != null)
			{
				if (!this.DisableOnChosen || !this.WasChosen)
				{
					this.WasChosen = true;
					if (this.BeforeChosen != null)
					{
						await this.BeforeChosen(this);
					}
					await this.OnChosen();
				}
			}
		}

		/// <summary>
		/// Typically, the title of the event is used for the name shown in the run history and uploaded to metrics. This
		/// method overrides the name used.
		/// </summary>
		// Token: 0x060081BF RID: 33215 RVA: 0x002BF66F File Offset: 0x002BD86F
		public EventOption WithOverridenHistoryName(LocString historyName)
		{
			this.HistoryName = historyName;
			return this;
		}

		/// <summary>
		/// Causes the event option to flash red if the player's HP is below the damage value passed.
		/// </summary>
		// Token: 0x060081C0 RID: 33216 RVA: 0x002BF67C File Offset: 0x002BD87C
		public EventOption ThatDoesDamage(decimal damage)
		{
			return this.ThatWillKillPlayerIf((Player p) => p.Creature.CurrentHp <= damage);
		}

		/// <summary>
		/// Causes the event option to flash red if the player's Max HP is below the value passed.
		/// </summary>
		// Token: 0x060081C1 RID: 33217 RVA: 0x002BF6A8 File Offset: 0x002BD8A8
		public EventOption ThatDecreasesMaxHp(decimal value)
		{
			return this.ThatWillKillPlayerIf((Player p) => p.Creature.MaxHp <= value);
		}

		/// <summary>
		/// Causes the event option to flash red when the passed function returns true.
		/// </summary>
		// Token: 0x060081C2 RID: 33218 RVA: 0x002BF6D4 File Offset: 0x002BD8D4
		public EventOption ThatWillKillPlayerIf(Func<Player, bool> willKillPlayer)
		{
			this.WillKillPlayer = willKillPlayer;
			return this;
		}

		/// <summary>
		/// Certain event options have dynamic titles that vary based on the DynamicVars passed to the loc string. By
		/// default, LocString DynamicVars are not saved to the save file. If this is called, those variables will be saved
		/// to the run history so that the title can be correctly displayed. This should only be used with specific
		/// events, as saving variables often saves lots of unnecessary data.
		/// </summary>
		// Token: 0x060081C3 RID: 33219 RVA: 0x002BF6DE File Offset: 0x002BD8DE
		public EventOption ThatHasDynamicTitle()
		{
			this.ShouldSaveVariablesToHistory = true;
			return this;
		}

		// Token: 0x060081C4 RID: 33220 RVA: 0x002BF6E8 File Offset: 0x002BD8E8
		public EventOption ThatWontSaveToChoiceHistory()
		{
			this.ShouldSaveChoiceToHistory = false;
			return this;
		}

		// Token: 0x060081C5 RID: 33221 RVA: 0x002BF6F4 File Offset: 0x002BD8F4
		private void AddLocVars(EventModel eventModel)
		{
			Player owner = eventModel.Owner;
			if (owner != null)
			{
				owner.Character.AddDetailsTo(this.Description);
			}
			LocString description = this.Description;
			string text = "IsMultiplayer";
			Player owner2 = eventModel.Owner;
			description.Add(text, owner2 != null && owner2.RunState.Players.Count > 1);
		}

		// Token: 0x060081C6 RID: 33222 RVA: 0x002BF74C File Offset: 0x002BD94C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 4);
			defaultInterpolatedStringHandler.AppendFormatted("EventOption");
			defaultInterpolatedStringHandler.AppendLiteral(" title: ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Title.GetRawText());
			defaultInterpolatedStringHandler.AppendLiteral(" description: ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Description.GetRawText());
			defaultInterpolatedStringHandler.AppendLiteral(" textKey: ");
			defaultInterpolatedStringHandler.AppendFormatted(this.TextKey);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
