using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200085F RID: 2143
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ThisOrThat : EventModel
	{
		// Token: 0x170019F2 RID: 6642
		// (get) Token: 0x0600695F RID: 26975 RVA: 0x0027C167 File Offset: 0x0027A367
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new HpLossVar(6m),
					new GoldVar(0),
					new StringVar("Curse", ModelDb.Card<Clumsy>().Title)
				});
			}
		}

		// Token: 0x06006960 RID: 26976 RVA: 0x0027C1A2 File Offset: 0x0027A3A2
		public override void CalculateVars()
		{
			base.DynamicVars.Gold.BaseValue = base.Rng.NextInt(41, 69);
		}

		// Token: 0x06006961 RID: 26977 RVA: 0x0027C1C8 File Offset: 0x0027A3C8
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Plain), "THIS_OR_THAT.pages.INITIAL.options.PLAIN", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars.HpLoss.IntValue),
				new EventOption(this, new Func<Task>(this.Ornate), "THIS_OR_THAT.pages.INITIAL.options.ORNATE", HoverTipFactory.FromCardWithCardHoverTips<Clumsy>(false))
			});
		}

		// Token: 0x06006962 RID: 26978 RVA: 0x0027C23C File Offset: 0x0027A43C
		private async Task Plain()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.HpLoss.IntValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null, null);
			await PlayerCmd.GainGold(base.DynamicVars.Gold.IntValue, base.Owner, false);
			base.SetEventFinished(base.L10NLookup("THIS_OR_THAT.pages.PLAIN.description"));
		}

		// Token: 0x06006963 RID: 26979 RVA: 0x0027C280 File Offset: 0x0027A480
		private async Task Ornate()
		{
			RelicModel relicModel = RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable();
			await RelicCmd.Obtain(relicModel, base.Owner, -1);
			await CardPileCmd.AddCurseToDeck<Clumsy>(base.Owner);
			base.SetEventFinished(base.L10NLookup("THIS_OR_THAT.pages.ORNATE.description"));
		}
	}
}
