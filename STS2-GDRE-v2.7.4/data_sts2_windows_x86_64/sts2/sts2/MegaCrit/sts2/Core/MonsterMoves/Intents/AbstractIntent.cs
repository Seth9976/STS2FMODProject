using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	/// <summary>
	/// The data backend for intents. See NIntent.cs to update visuals
	/// </summary>
	// Token: 0x020004C3 RID: 1219
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class AbstractIntent
	{
		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x0600490F RID: 18703
		public abstract IntentType IntentType { get; }

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06004910 RID: 18704 RVA: 0x002259CF File Offset: 0x00223BCF
		public virtual bool HasIntentTip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06004911 RID: 18705 RVA: 0x002259D2 File Offset: 0x00223BD2
		[Nullable(2)]
		protected virtual LocString IntentLabelFormat
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06004912 RID: 18706
		protected abstract string IntentPrefix { get; }

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06004913 RID: 18707
		[Nullable(2)]
		protected abstract string SpritePath
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06004914 RID: 18708 RVA: 0x002259D8 File Offset: 0x00223BD8
		public virtual IEnumerable<string> AssetPaths
		{
			get
			{
				if (this.SpritePath != null)
				{
					return new <>z__ReadOnlySingleElementList<string>(ImageHelper.GetImagePath(this.SpritePath));
				}
				return Array.Empty<string>();
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06004915 RID: 18709 RVA: 0x00225A07 File Offset: 0x00223C07
		protected LocString IntentTitle
		{
			get
			{
				return new LocString("intents", this.IntentPrefix + ".title");
			}
		}

		// Token: 0x06004916 RID: 18710 RVA: 0x00225A23 File Offset: 0x00223C23
		public virtual LocString GetIntentLabel(IEnumerable<Creature> targets, Creature owner)
		{
			return this.IntentLabelFormat ?? new LocString("intents", "FORMAT_EMPTY");
		}

		// Token: 0x06004917 RID: 18711 RVA: 0x00225A40 File Offset: 0x00223C40
		[return: Nullable(2)]
		public virtual Texture2D GetTexture(IEnumerable<Creature> targets, Creature owner)
		{
			if (string.IsNullOrEmpty(this.SpritePath))
			{
				return null;
			}
			string imagePath = ImageHelper.GetImagePath(this.SpritePath);
			return PreloadManager.Cache.GetTexture2D(imagePath);
		}

		// Token: 0x06004918 RID: 18712 RVA: 0x00225A74 File Offset: 0x00223C74
		public virtual string GetAnimation(IEnumerable<Creature> targets, Creature owner)
		{
			string text;
			if ((text = this._cachedAnimationName) == null)
			{
				text = (this._cachedAnimationName = this.IntentPrefix.ToLowerInvariant());
			}
			return text;
		}

		// Token: 0x06004919 RID: 18713 RVA: 0x00225AA0 File Offset: 0x00223CA0
		public HoverTip GetHoverTip(IEnumerable<Creature> targets, Creature owner)
		{
			Creature[] array = targets.ToArray<Creature>();
			return new HoverTip(this.IntentTitle, this.GetIntentDescription(array, owner), this.GetTexture(array, owner));
		}

		// Token: 0x0600491A RID: 18714 RVA: 0x00225AD0 File Offset: 0x00223CD0
		protected virtual LocString GetIntentDescription(IEnumerable<Creature> targets, Creature owner)
		{
			LocString locString = new LocString("intents", this.IntentPrefix + ".description");
			LocString locString2 = locString;
			string text = "IsMultiplayer";
			ICombatState combatState = owner.CombatState;
			locString2.Add(text, combatState != null && combatState.RunState.Players.Count > 1);
			return locString;
		}

		// Token: 0x04001CB1 RID: 7345
		protected const string _locTable = "intents";

		// Token: 0x04001CB2 RID: 7346
		[Nullable(2)]
		protected string _cachedAnimationName;
	}
}
