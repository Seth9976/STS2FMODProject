using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen
{
	// Token: 0x020002CF RID: 719
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NDeckHistoryEntry.cs")]
	public class NDeckHistoryEntry : NButton
	{
		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x060025F4 RID: 9716 RVA: 0x00100487 File Offset: 0x000FE687
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/run_history_screen/deck_history_entry");
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x060025F5 RID: 9717 RVA: 0x00100493 File Offset: 0x000FE693
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NDeckHistoryEntry.ScenePath);
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x060025F6 RID: 9718 RVA: 0x0010049F File Offset: 0x000FE69F
		// (set) Token: 0x060025F7 RID: 9719 RVA: 0x001004A7 File Offset: 0x000FE6A7
		public IEnumerable<int> FloorsAddedToDeck { get; private set; }

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x060025F8 RID: 9720 RVA: 0x001004B0 File Offset: 0x000FE6B0
		// (set) Token: 0x060025F9 RID: 9721 RVA: 0x001004B8 File Offset: 0x000FE6B8
		public CardModel Card { get; private set; }

		// Token: 0x060025FA RID: 9722 RVA: 0x001004C4 File Offset: 0x000FE6C4
		public override void _Ready()
		{
			this.ConnectSignals();
			this._titleLabel = base.GetNode<MegaLabel>("%Label");
			this._labelContainer = this._titleLabel.GetParent<MarginContainer>();
			this._cardImage = base.GetNode<NTinyCard>("%Card");
			this._enchantmentImage = base.GetNode<TextureRect>("%Enchantment");
			this._cardImage.PivotOffset = this._cardImage.Size * 0.5f;
			this.Reload();
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x00100550 File Offset: 0x000FE750
		public static NDeckHistoryEntry Create(CardModel card, int amount)
		{
			return NDeckHistoryEntry.Create(card, amount, Array.Empty<int>());
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x00100560 File Offset: 0x000FE760
		public static NDeckHistoryEntry Create(CardModel card, int amount, IEnumerable<int> floorsAdded)
		{
			NDeckHistoryEntry ndeckHistoryEntry = PreloadManager.Cache.GetScene(NDeckHistoryEntry.ScenePath).Instantiate<NDeckHistoryEntry>(0L);
			ndeckHistoryEntry.Card = card;
			ndeckHistoryEntry._amount = amount;
			ndeckHistoryEntry.FloorsAddedToDeck = floorsAdded;
			return ndeckHistoryEntry;
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x0010059C File Offset: 0x000FE79C
		private void Reload()
		{
			this._titleLabel.SetTextAutoSize(this.Card.Title);
			bool flag = this.Card.CurrentUpgradeLevel >= 1;
			bool flag2 = this.Card.Enchantment != null;
			string text = this.Card.Title;
			if (this._amount > 1)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._amount);
				defaultInterpolatedStringHandler.AppendLiteral("x ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			this._titleLabel.SetTextAutoSize(text);
			if (flag2)
			{
				this._titleLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, StsColors.purple);
			}
			else if (flag)
			{
				this._titleLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, StsColors.green);
			}
			this._cardImage.SetCard(this.Card);
			if (this.Card.Enchantment != null)
			{
				this._enchantmentImage.Texture = this.Card.Enchantment.Icon;
			}
			this._enchantmentImage.Visible = this.Card.Enchantment != null;
			base.Size = new Vector2(this._cardImage.Size.X + this._titleLabel.Size.X + 10f, base.Size.Y);
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x001006F4 File Offset: 0x000FE8F4
		protected override void OnFocus()
		{
			Tween scaleTween = this._scaleTween;
			if (scaleTween != null)
			{
				scaleTween.FastForwardToCompletion();
			}
			this._scaleTween = base.CreateTween().SetParallel(true);
			this._scaleTween.TweenProperty(this._cardImage, "scale", Vector2.One * 1.5f, 0.05);
			this._scaleTween.TweenProperty(this._labelContainer, "position:x", this._labelContainer.Position.X + 8f, 0.05);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x001007A0 File Offset: 0x000FE9A0
		protected override void OnUnfocus()
		{
			Tween scaleTween = this._scaleTween;
			if (scaleTween != null)
			{
				scaleTween.FastForwardToCompletion();
			}
			this._scaleTween = base.CreateTween().SetParallel(true);
			this._scaleTween.TweenProperty(this._cardImage, "scale", Vector2.One, 0.5).SetTrans(5L).SetEase(1L);
			this._scaleTween.TweenProperty(this._labelContainer, "position:x", this._labelContainer.Position.X - 8f, 0.5).SetTrans(5L).SetEase(1L);
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x0010085B File Offset: 0x000FEA5B
		protected override void OnRelease()
		{
			base.EmitSignal(NDeckHistoryEntry.SignalName.Clicked, new Variant[] { this });
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002601 RID: 9729 RVA: 0x0010087C File Offset: 0x000FEA7C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NDeckHistoryEntry.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckHistoryEntry.MethodName.Reload, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckHistoryEntry.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckHistoryEntry.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckHistoryEntry.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002602 RID: 9730 RVA: 0x0010097C File Offset: 0x000FEB7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDeckHistoryEntry.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckHistoryEntry.MethodName.Reload && args.Count == 0)
			{
				this.Reload();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckHistoryEntry.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckHistoryEntry.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckHistoryEntry.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002603 RID: 9731 RVA: 0x00100A4C File Offset: 0x000FEC4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDeckHistoryEntry.MethodName._Ready || (ref method) == NDeckHistoryEntry.MethodName.Reload || (ref method) == NDeckHistoryEntry.MethodName.OnFocus || (ref method) == NDeckHistoryEntry.MethodName.OnUnfocus || (ref method) == NDeckHistoryEntry.MethodName.OnRelease || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002604 RID: 9732 RVA: 0x00100AAC File Offset: 0x000FECAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDeckHistoryEntry.PropertyName._titleLabel)
			{
				this._titleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDeckHistoryEntry.PropertyName._cardImage)
			{
				this._cardImage = VariantUtils.ConvertTo<NTinyCard>(ref value);
				return true;
			}
			if ((ref name) == NDeckHistoryEntry.PropertyName._enchantmentImage)
			{
				this._enchantmentImage = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NDeckHistoryEntry.PropertyName._labelContainer)
			{
				this._labelContainer = VariantUtils.ConvertTo<MarginContainer>(ref value);
				return true;
			}
			if ((ref name) == NDeckHistoryEntry.PropertyName._scaleTween)
			{
				this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NDeckHistoryEntry.PropertyName._amount)
			{
				this._amount = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002605 RID: 9733 RVA: 0x00100B64 File Offset: 0x000FED64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDeckHistoryEntry.PropertyName._titleLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._titleLabel);
				return true;
			}
			if ((ref name) == NDeckHistoryEntry.PropertyName._cardImage)
			{
				value = VariantUtils.CreateFrom<NTinyCard>(ref this._cardImage);
				return true;
			}
			if ((ref name) == NDeckHistoryEntry.PropertyName._enchantmentImage)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._enchantmentImage);
				return true;
			}
			if ((ref name) == NDeckHistoryEntry.PropertyName._labelContainer)
			{
				value = VariantUtils.CreateFrom<MarginContainer>(ref this._labelContainer);
				return true;
			}
			if ((ref name) == NDeckHistoryEntry.PropertyName._scaleTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
				return true;
			}
			if ((ref name) == NDeckHistoryEntry.PropertyName._amount)
			{
				value = VariantUtils.CreateFrom<int>(ref this._amount);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002606 RID: 9734 RVA: 0x00100C3C File Offset: 0x000FEE3C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDeckHistoryEntry.PropertyName._titleLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckHistoryEntry.PropertyName._cardImage, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckHistoryEntry.PropertyName._enchantmentImage, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckHistoryEntry.PropertyName._labelContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckHistoryEntry.PropertyName._scaleTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NDeckHistoryEntry.PropertyName._amount, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002607 RID: 9735 RVA: 0x00100D18 File Offset: 0x000FEF18
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDeckHistoryEntry.PropertyName._titleLabel, Variant.From<MegaLabel>(ref this._titleLabel));
			info.AddProperty(NDeckHistoryEntry.PropertyName._cardImage, Variant.From<NTinyCard>(ref this._cardImage));
			info.AddProperty(NDeckHistoryEntry.PropertyName._enchantmentImage, Variant.From<TextureRect>(ref this._enchantmentImage));
			info.AddProperty(NDeckHistoryEntry.PropertyName._labelContainer, Variant.From<MarginContainer>(ref this._labelContainer));
			info.AddProperty(NDeckHistoryEntry.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
			info.AddProperty(NDeckHistoryEntry.PropertyName._amount, Variant.From<int>(ref this._amount));
			info.AddSignalEventDelegate(NDeckHistoryEntry.SignalName.Clicked, this.backing_Clicked);
		}

		/// <inheritdoc />
		// Token: 0x06002608 RID: 9736 RVA: 0x00100DC4 File Offset: 0x000FEFC4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._titleLabel, ref variant))
			{
				this._titleLabel = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._cardImage, ref variant2))
			{
				this._cardImage = variant2.As<NTinyCard>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._enchantmentImage, ref variant3))
			{
				this._enchantmentImage = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._labelContainer, ref variant4))
			{
				this._labelContainer = variant4.As<MarginContainer>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._scaleTween, ref variant5))
			{
				this._scaleTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._amount, ref variant6))
			{
				this._amount = variant6.As<int>();
			}
			NDeckHistoryEntry.ClickedEventHandler clickedEventHandler;
			if (info.TryGetSignalEventDelegate<NDeckHistoryEntry.ClickedEventHandler>(NDeckHistoryEntry.SignalName.Clicked, ref clickedEventHandler))
			{
				this.backing_Clicked = clickedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002609 RID: 9737 RVA: 0x00100E98 File Offset: 0x000FF098
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NDeckHistoryEntry.SignalName.Clicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "entry", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NDeckHistoryEntry.ClickedEventHandler" />
		// Token: 0x1400001D RID: 29
		// (add) Token: 0x0600260A RID: 9738 RVA: 0x00100F0C File Offset: 0x000FF10C
		// (remove) Token: 0x0600260B RID: 9739 RVA: 0x00100F25 File Offset: 0x000FF125
		[Nullable(0)]
		public event NDeckHistoryEntry.ClickedEventHandler Clicked
		{
			[NullableContext(0)]
			add
			{
				this.backing_Clicked = (NDeckHistoryEntry.ClickedEventHandler)Delegate.Combine(this.backing_Clicked, value);
			}
			[NullableContext(0)]
			remove
			{
				this.backing_Clicked = (NDeckHistoryEntry.ClickedEventHandler)Delegate.Remove(this.backing_Clicked, value);
			}
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x00100F3E File Offset: 0x000FF13E
		[NullableContext(0)]
		protected void EmitSignalClicked(NDeckHistoryEntry entry)
		{
			base.EmitSignal(NDeckHistoryEntry.SignalName.Clicked, new Variant[] { entry });
		}

		/// <inheritdoc />
		// Token: 0x0600260D RID: 9741 RVA: 0x00100F5F File Offset: 0x000FF15F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NDeckHistoryEntry.SignalName.Clicked) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(in signal, args);
				return;
			}
			NDeckHistoryEntry.ClickedEventHandler clickedEventHandler = this.backing_Clicked;
			if (clickedEventHandler == null)
			{
				return;
			}
			clickedEventHandler(VariantUtils.ConvertTo<NDeckHistoryEntry>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x0600260E RID: 9742 RVA: 0x00100F9E File Offset: 0x000FF19E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NDeckHistoryEntry.SignalName.Clicked || base.HasGodotClassSignal(in signal);
		}

		// Token: 0x04000E0D RID: 3597
		private MegaLabel _titleLabel;

		// Token: 0x04000E0E RID: 3598
		private NTinyCard _cardImage;

		// Token: 0x04000E0F RID: 3599
		private TextureRect _enchantmentImage;

		// Token: 0x04000E10 RID: 3600
		private MarginContainer _labelContainer;

		// Token: 0x04000E11 RID: 3601
		[Nullable(2)]
		private Tween _scaleTween;

		// Token: 0x04000E12 RID: 3602
		private int _amount;

		// Token: 0x04000E15 RID: 3605
		[Nullable(0)]
		private NDeckHistoryEntry.ClickedEventHandler backing_Clicked;

		// Token: 0x02001423 RID: 5155
		// (Invoke) Token: 0x0600A02A RID: 41002
		[NullableContext(0)]
		[Signal]
		public delegate void ClickedEventHandler(NDeckHistoryEntry entry);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001424 RID: 5156
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C76 RID: 19574
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Reload' method.
			/// </summary>
			// Token: 0x04004C77 RID: 19575
			public static readonly StringName Reload = "Reload";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004C78 RID: 19576
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004C79 RID: 19577
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004C7A RID: 19578
			public new static readonly StringName OnRelease = "OnRelease";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001425 RID: 5157
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_titleLabel' field.
			/// </summary>
			// Token: 0x04004C7B RID: 19579
			public static readonly StringName _titleLabel = "_titleLabel";

			/// <summary>
			/// Cached name for the '_cardImage' field.
			/// </summary>
			// Token: 0x04004C7C RID: 19580
			public static readonly StringName _cardImage = "_cardImage";

			/// <summary>
			/// Cached name for the '_enchantmentImage' field.
			/// </summary>
			// Token: 0x04004C7D RID: 19581
			public static readonly StringName _enchantmentImage = "_enchantmentImage";

			/// <summary>
			/// Cached name for the '_labelContainer' field.
			/// </summary>
			// Token: 0x04004C7E RID: 19582
			public static readonly StringName _labelContainer = "_labelContainer";

			/// <summary>
			/// Cached name for the '_scaleTween' field.
			/// </summary>
			// Token: 0x04004C7F RID: 19583
			public static readonly StringName _scaleTween = "_scaleTween";

			/// <summary>
			/// Cached name for the '_amount' field.
			/// </summary>
			// Token: 0x04004C80 RID: 19584
			public static readonly StringName _amount = "_amount";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001426 RID: 5158
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
			/// <summary>
			/// Cached name for the 'Clicked' signal.
			/// </summary>
			// Token: 0x04004C81 RID: 19585
			public static readonly StringName Clicked = "Clicked";
		}
	}
}
