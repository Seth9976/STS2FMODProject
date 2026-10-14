using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200084B RID: 2123
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RelicTrader : EventModel
	{
		// Token: 0x06006893 RID: 26771 RVA: 0x0027844C File Offset: 0x0027664C
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			List<EventOption> list = new List<EventOption>();
			if (this.OwnedRelics.Count >= 1)
			{
				list.Add(new EventOption(this, new Func<Task>(this.Top), "RELIC_TRADER.pages.INITIAL.options.TOP", this.GetRelicHoverTips(0)));
			}
			if (this.OwnedRelics.Count >= 2)
			{
				list.Add(new EventOption(this, new Func<Task>(this.Middle), "RELIC_TRADER.pages.INITIAL.options.MIDDLE", this.GetRelicHoverTips(1)));
			}
			if (this.OwnedRelics.Count >= 3)
			{
				list.Add(new EventOption(this, new Func<Task>(this.Bottom), "RELIC_TRADER.pages.INITIAL.options.BOTTOM", this.GetRelicHoverTips(2)));
			}
			if (list.Count == 0)
			{
				list.Add(new EventOption(this, new Func<Task>(this.Done), "PROCEED", Array.Empty<IHoverTip>()));
			}
			return list;
		}

		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x06006894 RID: 26772 RVA: 0x00278520 File Offset: 0x00276720
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("TopRelicOwned", ""),
					new StringVar("TopRelicNew", ""),
					new StringVar("MiddleRelicOwned", ""),
					new StringVar("MiddleRelicNew", ""),
					new StringVar("BottomRelicOwned", ""),
					new StringVar("BottomRelicNew", "")
				});
			}
		}

		// Token: 0x06006895 RID: 26773 RVA: 0x002785A4 File Offset: 0x002767A4
		public override bool IsAllowed(IRunState runState)
		{
			return runState.CurrentActIndex != 0 && runState.Players.All<Player>((Player p) => this.GetValidRelics(p).Count<RelicModel>() >= 5);
		}

		// Token: 0x06006896 RID: 26774 RVA: 0x002785C7 File Offset: 0x002767C7
		private IEnumerable<RelicModel> GetValidRelics(Player player)
		{
			return player.Relics.Where<RelicModel>((RelicModel r) => r.IsTradable);
		}

		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x06006897 RID: 26775 RVA: 0x002785F4 File Offset: 0x002767F4
		private IReadOnlyList<RelicModel> OwnedRelics
		{
			get
			{
				base.AssertMutable();
				if (this._ownedRelics == null)
				{
					this._ownedRelics = this.GetValidRelics(base.Owner).ToList<RelicModel>().StableShuffle<RelicModel>(base.Rng)
						.Take<RelicModel>(3)
						.ToList<RelicModel>();
				}
				return this._ownedRelics;
			}
		}

		// Token: 0x170019BD RID: 6589
		// (get) Token: 0x06006898 RID: 26776 RVA: 0x00278644 File Offset: 0x00276844
		private IReadOnlyList<RelicModel> NewRelics
		{
			get
			{
				base.AssertMutable();
				if (this._newRelics == null)
				{
					RelicModel[] array = new RelicModel[3];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = RelicFactory.PullNextRelicFromFront(base.Owner);
					}
					this._newRelics = array;
				}
				return this._newRelics;
			}
		}

		// Token: 0x06006899 RID: 26777 RVA: 0x00278690 File Offset: 0x00276890
		public override void CalculateVars()
		{
			if (this.OwnedRelics.Count > 0 && this.NewRelics.Count > 0)
			{
				((StringVar)base.DynamicVars["TopRelicOwned"]).StringValue = this.OwnedRelics[0].Title.GetFormattedText();
				((StringVar)base.DynamicVars["TopRelicNew"]).StringValue = this.NewRelics[0].Title.GetFormattedText();
			}
			if (this.OwnedRelics.Count > 1 && this.NewRelics.Count > 1)
			{
				((StringVar)base.DynamicVars["MiddleRelicOwned"]).StringValue = this.OwnedRelics[1].Title.GetFormattedText();
				((StringVar)base.DynamicVars["MiddleRelicNew"]).StringValue = this.NewRelics[1].Title.GetFormattedText();
			}
			if (this.OwnedRelics.Count > 2 && this.NewRelics.Count > 2)
			{
				((StringVar)base.DynamicVars["BottomRelicOwned"]).StringValue = this.OwnedRelics[2].Title.GetFormattedText();
				((StringVar)base.DynamicVars["BottomRelicNew"]).StringValue = this.NewRelics[2].Title.GetFormattedText();
			}
		}

		// Token: 0x0600689A RID: 26778 RVA: 0x00278814 File Offset: 0x00276A14
		private async Task Top()
		{
			await this.Trade(0);
		}

		// Token: 0x0600689B RID: 26779 RVA: 0x00278858 File Offset: 0x00276A58
		private async Task Middle()
		{
			await this.Trade(1);
		}

		// Token: 0x0600689C RID: 26780 RVA: 0x0027889C File Offset: 0x00276A9C
		private async Task Bottom()
		{
			await this.Trade(2);
		}

		// Token: 0x0600689D RID: 26781 RVA: 0x002788E0 File Offset: 0x00276AE0
		private async Task Trade(int index)
		{
			await RelicCmd.Remove(this.OwnedRelics[index]);
			await RelicCmd.Obtain(this.NewRelics[index].ToMutable(), base.Owner, -1);
			await this.Done();
		}

		// Token: 0x0600689E RID: 26782 RVA: 0x0027892B File Offset: 0x00276B2B
		private Task Done()
		{
			base.SetEventFinished(base.L10NLookup("RELIC_TRADER.pages.DONE.description"));
			return Task.CompletedTask;
		}

		// Token: 0x0600689F RID: 26783 RVA: 0x00278944 File Offset: 0x00276B44
		private IEnumerable<IHoverTip> GetRelicHoverTips(int index)
		{
			if (this.OwnedRelics.Count <= index || this.NewRelics.Count <= index)
			{
				return Array.Empty<IHoverTip>();
			}
			List<IHoverTip> list = new List<IHoverTip>();
			list.AddRange(this.OwnedRelics.ElementAt<RelicModel>(index).HoverTips);
			list.AddRange(this.NewRelics.ElementAt<RelicModel>(index).HoverTips);
			return new <>z__ReadOnlyList<IHoverTip>(list);
		}

		// Token: 0x040027A1 RID: 10145
		private const string _topRelicOwnedKey = "TopRelicOwned";

		// Token: 0x040027A2 RID: 10146
		private const string _topRelicNewKey = "TopRelicNew";

		// Token: 0x040027A3 RID: 10147
		private const string _middleRelicOwnedKey = "MiddleRelicOwned";

		// Token: 0x040027A4 RID: 10148
		private const string _middleRelicNewKey = "MiddleRelicNew";

		// Token: 0x040027A5 RID: 10149
		private const string _bottomRelicOwnedKey = "BottomRelicOwned";

		// Token: 0x040027A6 RID: 10150
		private const string _bottomRelicNewKey = "BottomRelicNew";

		// Token: 0x040027A7 RID: 10151
		[Nullable(new byte[] { 2, 1 })]
		private IReadOnlyList<RelicModel> _ownedRelics;

		// Token: 0x040027A8 RID: 10152
		[Nullable(new byte[] { 2, 1 })]
		private IReadOnlyList<RelicModel> _newRelics;
	}
}
