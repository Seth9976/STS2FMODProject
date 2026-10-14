using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	// Token: 0x0200034F RID: 847
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NSimpleCardSelectScreen.cs")]
	public sealed class NSimpleCardSelectScreen : NCardGridSelectionScreen
	{
		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060030B1 RID: 12465 RVA: 0x00167B8B File Offset: 0x00165D8B
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/card_selection/simple_card_select_screen");
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x00167B97 File Offset: 0x00165D97
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NSimpleCardSelectScreen.ScenePath);
			}
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x00167BA4 File Offset: 0x00165DA4
		public static NSimpleCardSelectScreen Create(IReadOnlyList<CardModel> cards, CardSelectorPrefs prefs)
		{
			NSimpleCardSelectScreen nsimpleCardSelectScreen = PreloadManager.Cache.GetScene(NSimpleCardSelectScreen.ScenePath).Instantiate<NSimpleCardSelectScreen>(0L);
			List<CardModel> list = cards.ToList<CardModel>();
			if (prefs.Comparison != null)
			{
				list.Sort(prefs.Comparison);
			}
			nsimpleCardSelectScreen.Name = "NSimpleCardSelectScreen";
			nsimpleCardSelectScreen._cards = list;
			nsimpleCardSelectScreen._cardResults = null;
			nsimpleCardSelectScreen._prefs = prefs;
			return nsimpleCardSelectScreen;
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x00167C0C File Offset: 0x00165E0C
		public static NSimpleCardSelectScreen Create(IReadOnlyList<CardCreationResult> cards, CardSelectorPrefs prefs)
		{
			NSimpleCardSelectScreen nsimpleCardSelectScreen = PreloadManager.Cache.GetScene(NSimpleCardSelectScreen.ScenePath).Instantiate<NSimpleCardSelectScreen>(0L);
			List<CardCreationResult> list = cards.ToList<CardCreationResult>();
			if (prefs.Comparison != null)
			{
				list.Sort((CardCreationResult c1, CardCreationResult c2) => prefs.Comparison(c1.Card, c2.Card));
			}
			nsimpleCardSelectScreen.Name = "NSimpleCardSelectScreen";
			nsimpleCardSelectScreen._cardResults = list;
			nsimpleCardSelectScreen._cards = nsimpleCardSelectScreen._cardResults.Select<CardCreationResult, CardModel>((CardCreationResult r) => r.Card).ToList<CardModel>();
			nsimpleCardSelectScreen._prefs = prefs;
			return nsimpleCardSelectScreen;
		}

		// Token: 0x060030B5 RID: 12469 RVA: 0x00167CBC File Offset: 0x00165EBC
		public override void _Ready()
		{
			this.ConnectSignalsAndInitGrid();
			this._confirmButton = base.GetNode<NConfirmButton>("%Confirm");
			this._bottomTextContainer = base.GetNode<Control>("%BottomText");
			this._infoLabel = this._bottomTextContainer.GetNode<MegaRichTextLabel>("%BottomLabel");
			this._infoLabel.Text = this._prefs.Prompt.GetFormattedText();
			if (this._prefs.MinSelect == 0)
			{
				this._confirmButton.Enable();
			}
			else
			{
				this._confirmButton.Disable();
			}
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.CompleteSelection();
			}), 0U);
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x00167D79 File Offset: 0x00165F79
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x00167D86 File Offset: 0x00165F86
		public override void _ExitTree()
		{
			base._ExitTree();
			this._cts.Cancel();
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x00167D9C File Offset: 0x00165F9C
		protected override void ConnectSignalsAndInitGrid()
		{
			base.ConnectSignalsAndInitGrid();
			this._combatPiles = base.GetNode<NCombatPilesContainer>("%CombatPiles");
			if (CombatManager.Instance.IsInProgress)
			{
				this._combatPiles.Initialize(this._cards.First<CardModel>().Owner);
			}
			this._combatPiles.Disable();
			this._combatPiles.SetVisible(false);
			this._peekButton.Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>(delegate(NPeekButton _)
			{
				if (this._peekButton.IsPeeking)
				{
					this._combatPiles.Enable();
					this._combatPiles.SetVisible(true);
					return;
				}
				this._combatPiles.Disable();
				this._combatPiles.SetVisible(false);
			}), 0U);
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060030B9 RID: 12473 RVA: 0x00167E26 File Offset: 0x00166026
		protected override IEnumerable<Control> PeekButtonTargets
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<Control>(this._bottomTextContainer);
			}
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x00167E33 File Offset: 0x00166033
		public override void AfterOverlayOpened()
		{
			base.AfterOverlayOpened();
			TaskHelper.RunSafely(this.FlashRelicsOnModifiedCards());
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x00167E48 File Offset: 0x00166048
		private async Task FlashRelicsOnModifiedCards()
		{
			if (this._cardResults != null)
			{
				await this.AwaitProcessFrame(this._cts.Token);
				using (List<CardCreationResult>.Enumerator enumerator = this._cardResults.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CardCreationResult result = enumerator.Current;
						NGridCardHolder ngridCardHolder = this._grid.CurrentlyDisplayedCardHolders.FirstOrDefault<NGridCardHolder>((NGridCardHolder h) => h.CardModel == result.Card);
						if (ngridCardHolder != null && result.HasBeenModified)
						{
							foreach (RelicModel relicModel in result.ModifyingRelics)
							{
								relicModel.Flash();
								NCard cardNode = ngridCardHolder.CardNode;
								if (cardNode != null)
								{
									cardNode.FlashRelicOnCard(relicModel);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x00167E8C File Offset: 0x0016608C
		protected override void OnCardClicked(CardModel card)
		{
			if (this._selectedCards.Contains(card))
			{
				this._grid.UnhighlightCard(card);
				this._selectedCards.Remove(card);
			}
			else
			{
				if (this._selectedCards.Count < this._prefs.MaxSelect)
				{
					this._grid.HighlightCard(card);
					this._selectedCards.Add(card);
				}
				if (!this._prefs.RequireManualConfirmation)
				{
					this.CheckIfSelectionComplete();
				}
			}
			if (this._selectedCards.Count >= this._prefs.MinSelect && this._prefs.RequireManualConfirmation)
			{
				this._confirmButton.Enable();
				return;
			}
			this._confirmButton.Disable();
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x00167F42 File Offset: 0x00166142
		private void CheckIfSelectionComplete()
		{
			if (this._selectedCards.Count >= this._prefs.MaxSelect)
			{
				this.CompleteSelection();
			}
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x00167F62 File Offset: 0x00166162
		private void CompleteSelection()
		{
			this._completionSource.SetResult(this._selectedCards);
			NOverlayStack.Instance.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060030BF RID: 12479 RVA: 0x00167F80 File Offset: 0x00166180
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NSimpleCardSelectScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSimpleCardSelectScreen.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSimpleCardSelectScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSimpleCardSelectScreen.MethodName.ConnectSignalsAndInitGrid, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSimpleCardSelectScreen.MethodName.AfterOverlayOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSimpleCardSelectScreen.MethodName.CheckIfSelectionComplete, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSimpleCardSelectScreen.MethodName.CompleteSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060030C0 RID: 12480 RVA: 0x001680E0 File Offset: 0x001662E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSimpleCardSelectScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSimpleCardSelectScreen.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSimpleCardSelectScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSimpleCardSelectScreen.MethodName.ConnectSignalsAndInitGrid && args.Count == 0)
			{
				this.ConnectSignalsAndInitGrid();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSimpleCardSelectScreen.MethodName.AfterOverlayOpened && args.Count == 0)
			{
				this.AfterOverlayOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSimpleCardSelectScreen.MethodName.CheckIfSelectionComplete && args.Count == 0)
			{
				this.CheckIfSelectionComplete();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSimpleCardSelectScreen.MethodName.CompleteSelection && args.Count == 0)
			{
				this.CompleteSelection();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060030C1 RID: 12481 RVA: 0x001681FC File Offset: 0x001663FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSimpleCardSelectScreen.MethodName._Ready || (ref method) == NSimpleCardSelectScreen.MethodName._EnterTree || (ref method) == NSimpleCardSelectScreen.MethodName._ExitTree || (ref method) == NSimpleCardSelectScreen.MethodName.ConnectSignalsAndInitGrid || (ref method) == NSimpleCardSelectScreen.MethodName.AfterOverlayOpened || (ref method) == NSimpleCardSelectScreen.MethodName.CheckIfSelectionComplete || (ref method) == NSimpleCardSelectScreen.MethodName.CompleteSelection || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060030C2 RID: 12482 RVA: 0x0016827C File Offset: 0x0016647C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSimpleCardSelectScreen.PropertyName._bottomTextContainer)
			{
				this._bottomTextContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSimpleCardSelectScreen.PropertyName._infoLabel)
			{
				this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NSimpleCardSelectScreen.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NSimpleCardSelectScreen.PropertyName._combatPiles)
			{
				this._combatPiles = VariantUtils.ConvertTo<NCombatPilesContainer>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060030C3 RID: 12483 RVA: 0x00168300 File Offset: 0x00166500
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSimpleCardSelectScreen.PropertyName._bottomTextContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bottomTextContainer);
				return true;
			}
			if ((ref name) == NSimpleCardSelectScreen.PropertyName._infoLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
				return true;
			}
			if ((ref name) == NSimpleCardSelectScreen.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NSimpleCardSelectScreen.PropertyName._combatPiles)
			{
				value = VariantUtils.CreateFrom<NCombatPilesContainer>(ref this._combatPiles);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060030C4 RID: 12484 RVA: 0x00168398 File Offset: 0x00166598
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSimpleCardSelectScreen.PropertyName._bottomTextContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSimpleCardSelectScreen.PropertyName._infoLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSimpleCardSelectScreen.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSimpleCardSelectScreen.PropertyName._combatPiles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060030C5 RID: 12485 RVA: 0x00168430 File Offset: 0x00166630
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSimpleCardSelectScreen.PropertyName._bottomTextContainer, Variant.From<Control>(ref this._bottomTextContainer));
			info.AddProperty(NSimpleCardSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
			info.AddProperty(NSimpleCardSelectScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
			info.AddProperty(NSimpleCardSelectScreen.PropertyName._combatPiles, Variant.From<NCombatPilesContainer>(ref this._combatPiles));
		}

		/// <inheritdoc />
		// Token: 0x060030C6 RID: 12486 RVA: 0x0016849C File Offset: 0x0016669C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSimpleCardSelectScreen.PropertyName._bottomTextContainer, ref variant))
			{
				this._bottomTextContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSimpleCardSelectScreen.PropertyName._infoLabel, ref variant2))
			{
				this._infoLabel = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSimpleCardSelectScreen.PropertyName._confirmButton, ref variant3))
			{
				this._confirmButton = variant3.As<NConfirmButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSimpleCardSelectScreen.PropertyName._combatPiles, ref variant4))
			{
				this._combatPiles = variant4.As<NCombatPilesContainer>();
			}
		}

		// Token: 0x040012DE RID: 4830
		private Control _bottomTextContainer;

		// Token: 0x040012DF RID: 4831
		private MegaRichTextLabel _infoLabel;

		// Token: 0x040012E0 RID: 4832
		private NConfirmButton _confirmButton;

		// Token: 0x040012E1 RID: 4833
		private NCombatPilesContainer _combatPiles;

		// Token: 0x040012E2 RID: 4834
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x040012E3 RID: 4835
		private readonly HashSet<CardModel> _selectedCards = new HashSet<CardModel>();

		// Token: 0x040012E4 RID: 4836
		private CardSelectorPrefs _prefs;

		// Token: 0x040012E5 RID: 4837
		[Nullable(new byte[] { 2, 1 })]
		private List<CardCreationResult> _cardResults;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200161F RID: 5663
		[NullableContext(0)]
		public new class MethodName : NCardGridSelectionScreen.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005600 RID: 22016
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005601 RID: 22017
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005602 RID: 22018
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ConnectSignalsAndInitGrid' method.
			/// </summary>
			// Token: 0x04005603 RID: 22019
			public new static readonly StringName ConnectSignalsAndInitGrid = "ConnectSignalsAndInitGrid";

			/// <summary>
			/// Cached name for the 'AfterOverlayOpened' method.
			/// </summary>
			// Token: 0x04005604 RID: 22020
			public new static readonly StringName AfterOverlayOpened = "AfterOverlayOpened";

			/// <summary>
			/// Cached name for the 'CheckIfSelectionComplete' method.
			/// </summary>
			// Token: 0x04005605 RID: 22021
			public static readonly StringName CheckIfSelectionComplete = "CheckIfSelectionComplete";

			/// <summary>
			/// Cached name for the 'CompleteSelection' method.
			/// </summary>
			// Token: 0x04005606 RID: 22022
			public static readonly StringName CompleteSelection = "CompleteSelection";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001620 RID: 5664
		[NullableContext(0)]
		public new class PropertyName : NCardGridSelectionScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the '_bottomTextContainer' field.
			/// </summary>
			// Token: 0x04005607 RID: 22023
			public static readonly StringName _bottomTextContainer = "_bottomTextContainer";

			/// <summary>
			/// Cached name for the '_infoLabel' field.
			/// </summary>
			// Token: 0x04005608 RID: 22024
			public static readonly StringName _infoLabel = "_infoLabel";

			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04005609 RID: 22025
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_combatPiles' field.
			/// </summary>
			// Token: 0x0400560A RID: 22026
			public static readonly StringName _combatPiles = "_combatPiles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001621 RID: 5665
		[NullableContext(0)]
		public new class SignalName : NCardGridSelectionScreen.SignalName
		{
		}
	}
}
