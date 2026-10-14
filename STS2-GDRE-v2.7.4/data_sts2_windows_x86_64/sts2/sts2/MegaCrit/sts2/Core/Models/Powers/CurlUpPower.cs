using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000657 RID: 1623
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CurlUpPower : PowerModel
	{
		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x0600579C RID: 22428 RVA: 0x0024D264 File Offset: 0x0024B464
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x0600579D RID: 22429 RVA: 0x0024D267 File Offset: 0x0024B467
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x0600579E RID: 22430 RVA: 0x0024D26A File Offset: 0x0024B46A
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x0600579F RID: 22431 RVA: 0x0024D26D File Offset: 0x0024B46D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060057A0 RID: 22432 RVA: 0x0024D27F File Offset: 0x0024B47F
		protected override object InitInternalData()
		{
			return new CurlUpPower.Data();
		}

		// Token: 0x060057A1 RID: 22433 RVA: 0x0024D288 File Offset: 0x0024B488
		public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult _, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (!props.IsPoweredAttack())
			{
				return Task.CompletedTask;
			}
			if (cardSource == null)
			{
				return Task.CompletedTask;
			}
			if (base.GetInternalData<CurlUpPower.Data>().playedCard != null && cardSource != base.GetInternalData<CurlUpPower.Data>().playedCard)
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<CurlUpPower.Data>().playedCard = cardSource;
			return Task.CompletedTask;
		}

		// Token: 0x060057A2 RID: 22434 RVA: 0x0024D2F4 File Offset: 0x0024B4F4
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card == base.GetInternalData<CurlUpPower.Data>().playedCard)
			{
				base.GetInternalData<CurlUpPower.Data>().playedCard = null;
				SfxCmd.Play("event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_curl", 1f);
				await CreatureCmd.TriggerAnim(base.Owner, "Curl", 0.25f);
				await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
				LouseProgenitor louseProgenitor = base.Owner.Monster as LouseProgenitor;
				if (louseProgenitor != null)
				{
					louseProgenitor.Curled = true;
				}
				await PowerCmd.Remove(this);
			}
		}

		// Token: 0x02001BA8 RID: 7080
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006E14 RID: 28180
			[Nullable(2)]
			public CardModel playedCard;
		}
	}
}
