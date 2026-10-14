using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens
{
	/// <summary>
	/// Unlock screen which reveals which cards you unlocked after slotting an Epoch.
	/// We want to let players know that these cards show up on future runs and
	/// allow them to inspect each card.
	/// </summary>
	// Token: 0x02000269 RID: 617
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockCardsScreen.cs")]
	public class NUnlockCardsScreen : NUnlockScreen
	{
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x000DC084 File Offset: 0x000DA284
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NUnlockCardsScreen._scenePath);
			}
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000DC090 File Offset: 0x000DA290
		public static NUnlockCardsScreen Create()
		{
			return PreloadManager.Cache.GetScene(NUnlockCardsScreen._scenePath).Instantiate<NUnlockCardsScreen>(0L);
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x000DC0A8 File Offset: 0x000DA2A8
		public override void _Ready()
		{
			base.ConnectSignals();
			this._banner = base.GetNode<NCommonBanner>("%Banner");
			this._banner.label.SetTextAutoSize(new LocString("timeline", "UNLOCK_CARDS_BANNER").GetRawText());
			this._banner.AnimateIn();
			this._cardRow = base.GetNode<Control>("%CardRow");
			LocString locString = new LocString("timeline", "UNLOCK_CARDS");
			base.GetNode<MegaRichTextLabel>("%ExplanationText").Text = "[center]" + locString.GetFormattedText() + "[/center]";
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000DC150 File Offset: 0x000DA350
		public override void Open()
		{
			base.Open();
			SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock", 1f);
			Vector2 vector = Vector2.Left * (float)(this._cards.Count - 1) * 350f * 0.5f;
			this._cardTween = base.CreateTween().SetParallel(true);
			int num = 0;
			foreach (CardModel cardModel in this._cards)
			{
				NCard ncard = NCard.Create(cardModel, ModelVisibility.Visible);
				NGridCardHolder ngridCardHolder = NGridCardHolder.Create(ncard);
				this._cardRow.AddChildSafely(ngridCardHolder);
				ncard.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
				ngridCardHolder.Scale = ngridCardHolder.SmallScale;
				this._cardTween.TweenProperty(ngridCardHolder, "position", ngridCardHolder.Position + vector + Vector2.Right * 350f * (float)num, 0.5).SetEase(1L).SetTrans(5L);
				this._cardTween.TweenProperty(ngridCardHolder, "modulate", Colors.White, 1.0).SetEase(1L).SetTrans(7L)
					.From(Colors.Black);
				ncard.ActivateRewardScreenGlow();
				this._holders.Add(ngridCardHolder);
				num++;
			}
			ActiveScreenContext.Instance.FocusOnDefaultControl();
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000DC2FC File Offset: 0x000DA4FC
		public void SetCards(IReadOnlyList<CardModel> cards)
		{
			this._cards = cards;
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x000DC308 File Offset: 0x000DA508
		protected override void OnScreenPreClose()
		{
			foreach (NGridCardHolder ngridCardHolder in this._holders)
			{
				NCard cardNode = ngridCardHolder.CardNode;
				if (cardNode != null)
				{
					cardNode.KillRarityGlow();
				}
			}
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000DC368 File Offset: 0x000DA568
		protected override void OnScreenClose()
		{
			NTimelineScreen.Instance.EnableInput();
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x000DC374 File Offset: 0x000DA574
		public override void _ExitTree()
		{
			foreach (NGridCardHolder ngridCardHolder in this._cardRow.GetChildren(false).OfType<NGridCardHolder>())
			{
				ngridCardHolder.QueueFreeSafely();
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x000DC3CC File Offset: 0x000DA5CC
		[Nullable(2)]
		public override Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._holders.Count != 0)
				{
					return this._holders[this._holders.Count / 2];
				}
				return null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600207F RID: 8319 RVA: 0x000DC3F8 File Offset: 0x000DA5F8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NUnlockCardsScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NUnlockCardsScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockCardsScreen.MethodName.Open, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockCardsScreen.MethodName.OnScreenPreClose, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockCardsScreen.MethodName.OnScreenClose, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockCardsScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002080 RID: 8320 RVA: 0x000DC534 File Offset: 0x000DA734
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockCardsScreen.MethodName.Create && args.Count == 0)
			{
				NUnlockCardsScreen nunlockCardsScreen = NUnlockCardsScreen.Create();
				ret = VariantUtils.CreateFrom<NUnlockCardsScreen>(ref nunlockCardsScreen);
				return true;
			}
			if ((ref method) == NUnlockCardsScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockCardsScreen.MethodName.Open && args.Count == 0)
			{
				this.Open();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockCardsScreen.MethodName.OnScreenPreClose && args.Count == 0)
			{
				this.OnScreenPreClose();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockCardsScreen.MethodName.OnScreenClose && args.Count == 0)
			{
				this.OnScreenClose();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockCardsScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x000DC630 File Offset: 0x000DA830
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockCardsScreen.MethodName.Create && args.Count == 0)
			{
				NUnlockCardsScreen nunlockCardsScreen = NUnlockCardsScreen.Create();
				ret = VariantUtils.CreateFrom<NUnlockCardsScreen>(ref nunlockCardsScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002082 RID: 8322 RVA: 0x000DC670 File Offset: 0x000DA870
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUnlockCardsScreen.MethodName.Create || (ref method) == NUnlockCardsScreen.MethodName._Ready || (ref method) == NUnlockCardsScreen.MethodName.Open || (ref method) == NUnlockCardsScreen.MethodName.OnScreenPreClose || (ref method) == NUnlockCardsScreen.MethodName.OnScreenClose || (ref method) == NUnlockCardsScreen.MethodName._ExitTree || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002083 RID: 8323 RVA: 0x000DC6E0 File Offset: 0x000DA8E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUnlockCardsScreen.PropertyName._cardRow)
			{
				this._cardRow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NUnlockCardsScreen.PropertyName._banner)
			{
				this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
				return true;
			}
			if ((ref name) == NUnlockCardsScreen.PropertyName._cardTween)
			{
				this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002084 RID: 8324 RVA: 0x000DC748 File Offset: 0x000DA948
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUnlockCardsScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NUnlockCardsScreen.PropertyName._cardRow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardRow);
				return true;
			}
			if ((ref name) == NUnlockCardsScreen.PropertyName._banner)
			{
				value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
				return true;
			}
			if ((ref name) == NUnlockCardsScreen.PropertyName._cardTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002085 RID: 8325 RVA: 0x000DC7E0 File Offset: 0x000DA9E0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUnlockCardsScreen.PropertyName._cardRow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockCardsScreen.PropertyName._banner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockCardsScreen.PropertyName._cardTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockCardsScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002086 RID: 8326 RVA: 0x000DC878 File Offset: 0x000DAA78
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUnlockCardsScreen.PropertyName._cardRow, Variant.From<Control>(ref this._cardRow));
			info.AddProperty(NUnlockCardsScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
			info.AddProperty(NUnlockCardsScreen.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
		}

		/// <inheritdoc />
		// Token: 0x06002087 RID: 8327 RVA: 0x000DC8D0 File Offset: 0x000DAAD0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUnlockCardsScreen.PropertyName._cardRow, ref variant))
			{
				this._cardRow = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NUnlockCardsScreen.PropertyName._banner, ref variant2))
			{
				this._banner = variant2.As<NCommonBanner>();
			}
			Variant variant3;
			if (info.TryGetProperty(NUnlockCardsScreen.PropertyName._cardTween, ref variant3))
			{
				this._cardTween = variant3.As<Tween>();
			}
		}

		// Token: 0x04000CA9 RID: 3241
		private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_cards_screen");

		// Token: 0x04000CAA RID: 3242
		private Control _cardRow;

		// Token: 0x04000CAB RID: 3243
		private NCommonBanner _banner;

		// Token: 0x04000CAC RID: 3244
		private readonly List<NGridCardHolder> _holders = new List<NGridCardHolder>();

		// Token: 0x04000CAD RID: 3245
		private IReadOnlyList<CardModel> _cards;

		// Token: 0x04000CAE RID: 3246
		[Nullable(2)]
		private Tween _cardTween;

		// Token: 0x04000CAF RID: 3247
		private const float _cardXOffset = 350f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012D9 RID: 4825
		[NullableContext(0)]
		public new class MethodName : NUnlockScreen.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004979 RID: 18809
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400497A RID: 18810
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x0400497B RID: 18811
			public new static readonly StringName Open = "Open";

			/// <summary>
			/// Cached name for the 'OnScreenPreClose' method.
			/// </summary>
			// Token: 0x0400497C RID: 18812
			public new static readonly StringName OnScreenPreClose = "OnScreenPreClose";

			/// <summary>
			/// Cached name for the 'OnScreenClose' method.
			/// </summary>
			// Token: 0x0400497D RID: 18813
			public new static readonly StringName OnScreenClose = "OnScreenClose";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400497E RID: 18814
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012DA RID: 4826
		[NullableContext(0)]
		public new class PropertyName : NUnlockScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x0400497F RID: 18815
			public new static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_cardRow' field.
			/// </summary>
			// Token: 0x04004980 RID: 18816
			public static readonly StringName _cardRow = "_cardRow";

			/// <summary>
			/// Cached name for the '_banner' field.
			/// </summary>
			// Token: 0x04004981 RID: 18817
			public static readonly StringName _banner = "_banner";

			/// <summary>
			/// Cached name for the '_cardTween' field.
			/// </summary>
			// Token: 0x04004982 RID: 18818
			public static readonly StringName _cardTween = "_cardTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012DB RID: 4827
		[NullableContext(0)]
		public new class SignalName : NUnlockScreen.SignalName
		{
		}
	}
}
