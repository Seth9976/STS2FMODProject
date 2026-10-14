using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Modifiers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models
{
	/// <summary>
	/// Run-lifetime model which alters the run for daily and custom runs.
	/// </summary>
	// Token: 0x020004E6 RID: 1254
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ModifierModel : AbstractModel
	{
		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06004D37 RID: 19767 RVA: 0x0022DE67 File Offset: 0x0022C067
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// If true, the player's deck will be cleared before AfterRunInitialized is called.
		/// </summary>
		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06004D38 RID: 19768 RVA: 0x0022DE6A File Offset: 0x0022C06A
		public virtual bool ClearsPlayerDeck
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06004D39 RID: 19769 RVA: 0x0022DE6D File Offset: 0x0022C06D
		public virtual IEnumerable<IHoverTip> HoverTips
		{
			get
			{
				return Array.Empty<IHoverTip>();
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06004D3A RID: 19770 RVA: 0x0022DE74 File Offset: 0x0022C074
		public virtual LocString Title
		{
			get
			{
				return new LocString("modifiers", base.Id.Entry + ".title");
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06004D3B RID: 19771 RVA: 0x0022DE95 File Offset: 0x0022C095
		public virtual LocString Description
		{
			get
			{
				return new LocString("modifiers", base.Id.Entry + ".description");
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06004D3C RID: 19772 RVA: 0x0022DEB6 File Offset: 0x0022C0B6
		public virtual LocString NeowOptionTitle
		{
			get
			{
				return this.Title;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06004D3D RID: 19773 RVA: 0x0022DEBE File Offset: 0x0022C0BE
		public virtual LocString NeowOptionDescription
		{
			get
			{
				return this.Description;
			}
		}

		/// <summary>
		/// Get the additional text that this modifier should add to the "heal" option at the rest site.
		/// Returns null for modifiers that add no additional text.
		/// </summary>
		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06004D3E RID: 19774 RVA: 0x0022DEC6 File Offset: 0x0022C0C6
		[Nullable(2)]
		protected LocString AdditionalRestSiteHealText
		{
			[NullableContext(2)]
			get
			{
				return LocString.GetIfExists("modifiers", base.Id.Entry + ".additionalRestSiteHealText");
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06004D3F RID: 19775 RVA: 0x0022DEE7 File Offset: 0x0022C0E7
		public Texture2D Icon
		{
			get
			{
				if (ResourceLoader.Exists(this.IconPath, ""))
				{
					return PreloadManager.Cache.GetTexture2D(this.IconPath);
				}
				return PreloadManager.Cache.GetTexture2D(ModifierModel.MissingIconPath);
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06004D40 RID: 19776 RVA: 0x0022DF1B File Offset: 0x0022C11B
		protected virtual string IconPath
		{
			get
			{
				return ImageHelper.GetImagePath("packed/modifiers/" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06004D41 RID: 19777 RVA: 0x0022DF41 File Offset: 0x0022C141
		private static string MissingIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("powers/missing_power.png");
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06004D42 RID: 19778 RVA: 0x0022DF4D File Offset: 0x0022C14D
		protected RunState RunState
		{
			get
			{
				RunState runState = this._runState;
				if (runState == null)
				{
					throw new InvalidOperationException("Modifier was never initialized!");
				}
				return runState;
			}
		}

		/// <summary>
		/// Called after a new run is created with the modifier.
		/// Only called for new runs, not loaded ones.
		/// </summary>
		// Token: 0x06004D43 RID: 19779 RVA: 0x0022DF64 File Offset: 0x0022C164
		public void OnRunCreated(RunState runState)
		{
			base.AssertMutable();
			this._runState = runState;
			if (this.ClearsPlayerDeck)
			{
				foreach (Player player in runState.Players)
				{
					player.Deck.Clear(false);
				}
			}
			this.AfterRunCreated(runState);
		}

		/// <summary>
		/// Called after a run is loaded with the modifier.
		/// </summary>
		// Token: 0x06004D44 RID: 19780 RVA: 0x0022DFD4 File Offset: 0x0022C1D4
		public void OnRunLoaded(RunState runState)
		{
			base.AssertMutable();
			this._runState = runState;
			this.AfterRunLoaded(runState);
		}

		/// <summary>
		/// If this returns a non-null function, the modifier is presented as an option at Neow.
		/// The function is called when the option is chosen. The option will have the modifier's title and description.
		/// </summary>
		/// <param name="eventModel">The event model for which the option should be generated.</param>
		// Token: 0x06004D45 RID: 19781 RVA: 0x0022DFEA File Offset: 0x0022C1EA
		[return: Nullable(new byte[] { 2, 1 })]
		public virtual Func<Task> GenerateNeowOption(EventModel eventModel)
		{
			return null;
		}

		/// <summary>
		/// A special hook for modifiers that is called right after the run is created for the first time.
		/// Used for initializing things like the relic bags.
		/// Note that this is not called when a run is loaded from save.
		/// </summary>
		// Token: 0x06004D46 RID: 19782 RVA: 0x0022DFED File Offset: 0x0022C1ED
		protected virtual void AfterRunCreated(RunState runState)
		{
		}

		/// <summary>
		/// A special hook for modifiers that is called right after the run is loaded.
		/// Used for initializing things that are not serialized like base room odds.
		/// Note that this is not called when a run is created from scratch without loading.
		/// </summary>
		// Token: 0x06004D47 RID: 19783 RVA: 0x0022DFEF File Offset: 0x0022C1EF
		protected virtual void AfterRunLoaded(RunState runState)
		{
		}

		/// <summary>
		/// Returns true if this is equivalent to the passed modifier.
		/// By default, two ModifierModels are considered equivalent if they are of the same type. The CharacterCards modifier
		/// overrides this to ensure that the character property is equal as well.
		/// </summary>
		// Token: 0x06004D48 RID: 19784 RVA: 0x0022DFF1 File Offset: 0x0022C1F1
		public virtual bool IsEquivalent(ModifierModel other)
		{
			return base.IsCanonical == other.IsCanonical && base.GetType() == other.GetType();
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x0022E014 File Offset: 0x0022C214
		public ModifierModel ToMutable()
		{
			base.AssertCanonical();
			return (ModifierModel)base.MutableClone();
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x0022E027 File Offset: 0x0022C227
		public SerializableModifier ToSerializable()
		{
			base.AssertMutable();
			return new SerializableModifier
			{
				Id = base.Id,
				Props = SavedProperties.From(this)
			};
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x0022E04C File Offset: 0x0022C24C
		public static ModifierModel FromSerializable(SerializableModifier serializable)
		{
			ModifierModel modifierModel = SaveUtil.ModifierOrDeprecated(serializable.Id).ToMutable();
			SavedProperties props = serializable.Props;
			if (props != null)
			{
				props.Fill(modifierModel);
			}
			return modifierModel;
		}

		/// <summary>
		/// Pick a set of 2 good modifiers and 1 bad modifier.
		/// </summary>
		/// <param name="rng">RNG to use for picking modifiers.</param>
		/// <param name="excludedCharacters">
		/// Characters to exclude from the <see cref="P:MegaCrit.Sts2.Core.Models.Modifiers.CharacterCards.CharacterModel" /> possibilities.
		/// For example, pass [Defect] if <see cref="P:MegaCrit.Sts2.Core.Models.Modifiers.CharacterCards.CharacterModel" /> should not be set to
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Characters.Defect" />.
		/// Used to prevent setting a modifier that gives a player access to cards they already have access to.
		/// </param>
		// Token: 0x06004D4C RID: 19788 RVA: 0x0022E080 File Offset: 0x0022C280
		public static IReadOnlyCollection<ModifierModel> Pick2Good1Bad(Rng rng, IEnumerable<CharacterModel> excludedCharacters)
		{
			List<ModifierModel> list = new List<ModifierModel>();
			List<ModifierModel> list2 = ModelDb.GoodModifiers.ToList<ModifierModel>();
			List<CharacterModel> list3 = ModelDb.AllCharacters.Except<CharacterModel>(excludedCharacters).ToList<CharacterModel>();
			if (list3.Count <= 0)
			{
				list2.RemoveAll((ModifierModel m) => m is CharacterCards);
			}
			for (int i = 0; i < 2; i++)
			{
				ModifierModel canonicalModifier = rng.NextItem<ModifierModel>(list2);
				if (canonicalModifier == null)
				{
					throw new InvalidOperationException("There were not enough good modifiers to fill the daily!");
				}
				ModifierModel modifierModel = canonicalModifier.ToMutable();
				CharacterCards characterCards = modifierModel as CharacterCards;
				if (characterCards != null)
				{
					characterCards.CharacterModel = rng.NextItem<CharacterModel>(list3).Id;
				}
				list.Add(modifierModel);
				list2.Remove(canonicalModifier);
				IReadOnlySet<ModifierModel> readOnlySet = ModelDb.MutuallyExclusiveModifiers.FirstOrDefault<IReadOnlySet<ModifierModel>>((IReadOnlySet<ModifierModel> s) => s.Contains(canonicalModifier));
				if (readOnlySet != null)
				{
					foreach (ModifierModel modifierModel2 in readOnlySet)
					{
						list2.Remove(modifierModel2);
					}
				}
			}
			list.Add(rng.NextItem<ModifierModel>(ModelDb.BadModifiers).ToMutable());
			return list;
		}

		// Token: 0x04001D5C RID: 7516
		private const string _locTable = "modifiers";

		// Token: 0x04001D5D RID: 7517
		[Nullable(2)]
		private RunState _runState;
	}
}
