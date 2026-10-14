using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000517 RID: 1303
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CallingBell : RelicModel
	{
		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06004FBD RID: 20413 RVA: 0x0023DEBC File Offset: 0x0023C0BC
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06004FBE RID: 20414 RVA: 0x0023DEBF File Offset: 0x0023C0BF
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06004FBF RID: 20415 RVA: 0x0023DEC2 File Offset: 0x0023C0C2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Relics", 3m));
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06004FC0 RID: 20416 RVA: 0x0023DED9 File Offset: 0x0023C0D9
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromCardWithCardHoverTips<CurseOfTheBell>(false);
			}
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x0023DEE4 File Offset: 0x0023C0E4
		public override async Task AfterObtained()
		{
			await CardPileCmd.AddCurseToDeck<CurseOfTheBell>(base.Owner);
			await Cmd.Wait(0.75f, false);
			await RewardsCmd.OfferCustom(base.Owner, this.GenerateRewards());
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x0023DF28 File Offset: 0x0023C128
		private unsafe List<Reward> GenerateRewards()
		{
			int num;
			Span<Reward> span;
			int num2;
			if (TestMode.IsOn)
			{
				num = 3;
				List<Reward> list = new List<Reward>(num);
				CollectionsMarshal.SetCount<Reward>(list, num);
				span = CollectionsMarshal.AsSpan<Reward>(list);
				num2 = 0;
				*span[num2] = new RelicReward(ModelDb.Relic<Anchor>().ToMutable(), base.Owner);
				num2++;
				*span[num2] = new RelicReward(ModelDb.Relic<GremlinHorn>().ToMutable(), base.Owner);
				num2++;
				*span[num2] = new RelicReward(ModelDb.Relic<MummifiedHand>().ToMutable(), base.Owner);
				return list;
			}
			num2 = 3;
			List<Reward> list2 = new List<Reward>(num2);
			CollectionsMarshal.SetCount<Reward>(list2, num2);
			span = CollectionsMarshal.AsSpan<Reward>(list2);
			num = 0;
			*span[num] = new RelicReward(RelicRarity.Common, base.Owner);
			num++;
			*span[num] = new RelicReward(RelicRarity.Uncommon, base.Owner);
			num++;
			*span[num] = new RelicReward(RelicRarity.Rare, base.Owner);
			return list2;
		}

		// Token: 0x04002433 RID: 9267
		private const string _relicsKey = "Relics";
	}
}
