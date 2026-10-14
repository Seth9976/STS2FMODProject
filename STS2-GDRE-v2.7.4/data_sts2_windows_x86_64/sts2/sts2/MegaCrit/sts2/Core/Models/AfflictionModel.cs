using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004D8 RID: 1240
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class AfflictionModel : AbstractModel
	{
		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06004A8E RID: 19086 RVA: 0x0022768C File Offset: 0x0022588C
		// (remove) Token: 0x06004A8F RID: 19087 RVA: 0x002276C4 File Offset: 0x002258C4
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<int, int> AmountChanged;

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06004A90 RID: 19088 RVA: 0x002276F9 File Offset: 0x002258F9
		public LocString Title
		{
			get
			{
				return new LocString("afflictions", base.Id.Entry + ".title");
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06004A91 RID: 19089 RVA: 0x0022771A File Offset: 0x0022591A
		private LocString Description
		{
			get
			{
				return new LocString("afflictions", base.Id.Entry + ".description");
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06004A92 RID: 19090 RVA: 0x0022773B File Offset: 0x0022593B
		private LocString ExtraCardText
		{
			get
			{
				return new LocString("afflictions", base.Id.Entry + ".extraCardText");
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06004A93 RID: 19091 RVA: 0x0022775C File Offset: 0x0022595C
		public virtual bool HasExtraCardText
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06004A94 RID: 19092 RVA: 0x00227760 File Offset: 0x00225960
		public LocString DynamicDescription
		{
			get
			{
				LocString description = this.Description;
				description.Add("Amount", this.Amount);
				return description;
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06004A95 RID: 19093 RVA: 0x0022778C File Offset: 0x0022598C
		[Nullable(2)]
		public LocString DynamicExtraCardText
		{
			[NullableContext(2)]
			get
			{
				if (!this.HasExtraCardText)
				{
					return null;
				}
				LocString extraCardText = this.ExtraCardText;
				extraCardText.Add("Amount", this.Amount);
				return extraCardText;
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x06004A96 RID: 19094 RVA: 0x002277C1 File Offset: 0x002259C1
		public string OverlayPath
		{
			get
			{
				return SceneHelper.GetScenePath("cards/overlays/afflictions/" + base.Id.Entry.ToLowerInvariant());
			}
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x002277E2 File Offset: 0x002259E2
		public Control CreateOverlay()
		{
			return PreloadManager.Cache.GetScene(this.OverlayPath).Instantiate<Control>(0L);
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x06004A98 RID: 19096 RVA: 0x002277FB File Offset: 0x002259FB
		public bool HasOverlay
		{
			get
			{
				return ResourceLoader.Exists(this.OverlayPath, "");
			}
		}

		/// <summary>
		/// Get the card that this is afflicting.
		/// This is almost never null, so we leave it as non-nullable to make it easier to use. If you really need to check
		/// for null, use <see cref="P:MegaCrit.Sts2.Core.Models.AfflictionModel.HasCard" />.
		/// </summary>
		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06004A99 RID: 19097 RVA: 0x0022780D File Offset: 0x00225A0D
		// (set) Token: 0x06004A9A RID: 19098 RVA: 0x0022781B File Offset: 0x00225A1B
		public CardModel Card
		{
			get
			{
				base.AssertMutable();
				return this._card;
			}
			set
			{
				base.AssertMutable();
				value.AssertMutable();
				if (this._card != null)
				{
					throw new InvalidOperationException("Afflictions cannot be moved from one card to another.");
				}
				this._card = value;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06004A9B RID: 19099 RVA: 0x00227843 File Offset: 0x00225A43
		public bool HasCard
		{
			get
			{
				return this._card != null;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06004A9C RID: 19100 RVA: 0x0022784E File Offset: 0x00225A4E
		// (set) Token: 0x06004A9D RID: 19101 RVA: 0x00227858 File Offset: 0x00225A58
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				base.AssertMutable();
				if (this._amount == value)
				{
					return;
				}
				int amount = this._amount;
				this._amount = value;
				if (this._card != null)
				{
					this._card.Owner.PlayerCombatState.RecalculateCardValues();
				}
				Action<int, int> amountChanged = this.AmountChanged;
				if (amountChanged == null)
				{
					return;
				}
				amountChanged(amount, this._amount);
			}
		}

		/// <summary>
		/// Get the CombatState of the card that this is afflicting.
		/// Will never be null, since Afflictions are combat-only.
		/// </summary>
		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06004A9E RID: 19102 RVA: 0x002278B7 File Offset: 0x00225AB7
		public ICombatState CombatState
		{
			get
			{
				return this.Card.CombatState;
			}
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x002278C4 File Offset: 0x00225AC4
		public virtual bool CanAfflictCardType(CardType cardType)
		{
			return true;
		}

		/// <summary>
		/// Can this afflict Unplayable cards?
		/// Usually true, because Unplayable cards sometimes have Sly and we might want the affliction's effect to still
		/// trigger. Some exceptions for when it doesn't make sense (like one that makes the card cost more energy).
		/// </summary>
		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06004AA0 RID: 19104 RVA: 0x002278C7 File Offset: 0x00225AC7
		public virtual bool CanAfflictUnplayableCards
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06004AA1 RID: 19105 RVA: 0x002278CA File Offset: 0x00225ACA
		public virtual bool IsStackable
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Checks whether the specified card can be afflicted with this affliction. For example, Ringing can only
		/// afflict attacks, so this will return true if an Attack is passed, but false if a Skill is passed.
		///
		/// Note: Do not override this method to REMOVE restrictions, just to ADD them. When you override it, make sure to
		/// call `base.CanAfflict`, and then add your own restrictions afterwards. You can also override some other methods
		/// to add specific types of restrictions (check AfflictionModel.cs for details).
		/// </summary>
		/// <param name="card">Card to check validity of.</param>
		/// <returns>Whether or not the specified card is valid to afflict with this.</returns>
		// Token: 0x06004AA2 RID: 19106 RVA: 0x002278D0 File Offset: 0x00225AD0
		public virtual bool CanAfflict(CardModel card)
		{
			return this.CanAfflictCardType(card.Type) && (!card.Keywords.Contains(CardKeyword.Unplayable) || this.CanAfflictUnplayableCards) && (card.Affliction == null || (this.IsStackable && !(card.Affliction.GetType() != base.GetType())));
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x00227930 File Offset: 0x00225B30
		public virtual void AfterApplied()
		{
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x00227932 File Offset: 0x00225B32
		public virtual void BeforeRemoved()
		{
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x00227934 File Offset: 0x00225B34
		public virtual Task OnPlay(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			return Task.CompletedTask;
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06004AA6 RID: 19110 RVA: 0x0022793B File Offset: 0x00225B3B
		// (set) Token: 0x06004AA7 RID: 19111 RVA: 0x0022794D File Offset: 0x00225B4D
		public AfflictionModel CanonicalInstance
		{
			get
			{
				if (!base.IsMutable)
				{
					return this;
				}
				return this._canonicalInstance;
			}
			private set
			{
				base.AssertMutable();
				this._canonicalInstance = value;
			}
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x0022795C File Offset: 0x00225B5C
		public AfflictionModel ToMutable()
		{
			base.AssertCanonical();
			AfflictionModel afflictionModel = (AfflictionModel)base.MutableClone();
			afflictionModel.CanonicalInstance = this;
			return afflictionModel;
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x00227983 File Offset: 0x00225B83
		protected override void AfterCloned()
		{
			base.AfterCloned();
			this.AmountChanged = null;
			this._card = null;
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004AAA RID: 19114 RVA: 0x00227999 File Offset: 0x00225B99
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06004AAB RID: 19115 RVA: 0x0022799C File Offset: 0x00225B9C
		protected virtual IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return Array.Empty<IHoverTip>();
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06004AAC RID: 19116 RVA: 0x002279A3 File Offset: 0x00225BA3
		public HoverTip HoverTip
		{
			get
			{
				return new HoverTip(this, this.DynamicDescription);
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06004AAD RID: 19117 RVA: 0x002279B4 File Offset: 0x00225BB4
		public unsafe IEnumerable<IHoverTip> HoverTips
		{
			get
			{
				int num = 1;
				List<IHoverTip> list = new List<IHoverTip>(num);
				CollectionsMarshal.SetCount<IHoverTip>(list, num);
				Span<IHoverTip> span = CollectionsMarshal.AsSpan<IHoverTip>(list);
				int num2 = 0;
				*span[num2] = this.HoverTip;
				List<IHoverTip> list2 = list;
				list2.AddRange(this.ExtraHoverTips);
				return list2;
			}
		}

		/// <summary>
		/// Chooses random targets out of a set of cards to afflict.
		/// </summary>
		/// <param name="rngSet">The RNG set to use. Does not use Card.Owner because we may be the canonical version.</param>
		/// <param name="cards">The possible cards to afflict.</param>
		/// <param name="count">Number of cards to afflict.</param>
		/// <returns>The cards to afflict.</returns>
		// Token: 0x06004AAE RID: 19118 RVA: 0x002279FC File Offset: 0x00225BFC
		public IReadOnlyList<CardModel> PickRandomTargets(RunRngSet rngSet, IEnumerable<CardModel> cards, int count)
		{
			List<CardModel> list = cards.Where<CardModel>(new Func<CardModel, bool>(this.CanAfflict)).ToList<CardModel>().UnstableShuffle<CardModel>(rngSet.CombatCardGeneration);
			list.RemoveRange(Math.Clamp(list.Count - 1, 0, count), Math.Max(0, list.Count - count));
			return list;
		}

		/// <summary>
		/// Remove this affliction from its card.
		/// Should only be called by <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.ClearAfflictionInternal" /> and in tests.
		/// </summary>
		// Token: 0x06004AAF RID: 19119 RVA: 0x00227A51 File Offset: 0x00225C51
		public void ClearInternal()
		{
			this.BeforeRemoved();
			this._card = null;
		}

		// Token: 0x04001CDA RID: 7386
		public const string locTable = "afflictions";

		// Token: 0x04001CDC RID: 7388
		[Nullable(2)]
		private CardModel _card;

		// Token: 0x04001CDD RID: 7389
		private int _amount;

		// Token: 0x04001CDE RID: 7390
		private AfflictionModel _canonicalInstance;
	}
}
