using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Relics;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection
{
	/// <summary>
	/// Handles logic/visuals for a single relic button in the Relic Collection screen.
	/// </summary>
	// Token: 0x020002D8 RID: 728
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/RelicCollection/NRelicCollectionEntry.cs")]
	public class NRelicCollectionEntry : NButton
	{
		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060026C8 RID: 9928 RVA: 0x0010870E File Offset: 0x0010690E
		private static LocString UnknownHoverTipTitle
		{
			get
			{
				return new LocString("main_menu_ui", "COMPENDIUM_RELIC_COLLECTION.unknown.title");
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060026C9 RID: 9929 RVA: 0x0010871F File Offset: 0x0010691F
		private static LocString UnknownHoverTipDescription
		{
			get
			{
				return new LocString("main_menu_ui", "COMPENDIUM_RELIC_COLLECTION.unknown.description");
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060026CA RID: 9930 RVA: 0x00108730 File Offset: 0x00106930
		private static HoverTip UnknownHoverTip
		{
			get
			{
				return new HoverTip(NRelicCollectionEntry.UnknownHoverTipTitle, NRelicCollectionEntry.UnknownHoverTipDescription, null);
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060026CB RID: 9931 RVA: 0x00108742 File Offset: 0x00106942
		private static LocString LockedHoverTipTitle
		{
			get
			{
				return new LocString("main_menu_ui", "COMPENDIUM_RELIC_COLLECTION.locked.title");
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060026CC RID: 9932 RVA: 0x00108753 File Offset: 0x00106953
		private static LocString LockedHoverTipDescription
		{
			get
			{
				return new LocString("main_menu_ui", "COMPENDIUM_RELIC_COLLECTION.locked.description");
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060026CD RID: 9933 RVA: 0x00108764 File Offset: 0x00106964
		private static HoverTip LockedHoverTip
		{
			get
			{
				return new HoverTip(NRelicCollectionEntry.LockedHoverTipTitle, NRelicCollectionEntry.LockedHoverTipDescription, null);
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x00108776 File Offset: 0x00106976
		// (set) Token: 0x060026CF RID: 9935 RVA: 0x0010877E File Offset: 0x0010697E
		public ModelVisibility ModelVisibility { get; set; }

		// Token: 0x060026D0 RID: 9936 RVA: 0x00108788 File Offset: 0x00106988
		public static NRelicCollectionEntry Create(RelicModel relic, ModelVisibility visibility)
		{
			NRelicCollectionEntry nrelicCollectionEntry = PreloadManager.Cache.GetScene(NRelicCollectionEntry.scenePath).Instantiate<NRelicCollectionEntry>(0L);
			nrelicCollectionEntry.relic = relic;
			nrelicCollectionEntry.ModelVisibility = visibility;
			return nrelicCollectionEntry;
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x001087BC File Offset: 0x001069BC
		public override void _Ready()
		{
			this.ConnectSignals();
			this._relicHolder = base.GetNode<Control>("RelicHolder");
			if (this.ModelVisibility == ModelVisibility.Locked)
			{
				TextureRect textureRect = new TextureRect();
				textureRect.ExpandMode = 1L;
				textureRect.StretchMode = 4L;
				textureRect.Texture = PreloadManager.Cache.GetTexture2D(NRelicCollectionEntry.lockedIconPath);
				textureRect.Size = Vector2.One * 68f;
				textureRect.PivotOffset = textureRect.Size * 0.5f;
				textureRect.Modulate = StsColors.gray;
				this._relicHolder.AddChildSafely(textureRect);
				this._relicNode = textureRect;
			}
			else
			{
				NRelic nrelic = NRelic.Create(this.relic.ToMutable(), NRelic.IconSize.Small);
				this._relicHolder.AddChildSafely(nrelic);
				if (this.ModelVisibility == ModelVisibility.NotSeen)
				{
					nrelic.Icon.SelfModulate = StsColors.ninetyPercentBlack;
					nrelic.Outline.SelfModulate = StsColors.halfTransparentWhite;
				}
				else
				{
					foreach (RelicPoolModel relicPoolModel in ModelDb.AllCharacterRelicPools)
					{
						if (relicPoolModel.AllRelicIds.Contains(this.relic.Id))
						{
							CanvasItem outline = nrelic.Outline;
							Color labOutlineColor = relicPoolModel.LabOutlineColor;
							labOutlineColor.A = 0.66f;
							outline.SelfModulate = labOutlineColor;
							break;
						}
					}
				}
				this._relicNode = nrelic;
			}
			this._relicNode.MouseFilter = 2L;
			this._relicNode.FocusMode = 0L;
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x00108944 File Offset: 0x00106B44
		protected override void OnRelease()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._relicNode, "scale", Vector2.One, 1.0).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x001089B4 File Offset: 0x00106BB4
		protected override void OnFocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._relicNode.Scale = Vector2.One * 1.25f;
			ModelVisibility modelVisibility = this.ModelVisibility;
			IEnumerable<IHoverTip> enumerable;
			switch (modelVisibility)
			{
			case ModelVisibility.None:
				throw new ArgumentOutOfRangeException();
			case ModelVisibility.Visible:
				enumerable = this.relic.HoverTips;
				break;
			case ModelVisibility.NotSeen:
				enumerable = new <>z__ReadOnlySingleElementList<IHoverTip>(NRelicCollectionEntry.UnknownHoverTip);
				break;
			case ModelVisibility.Locked:
				enumerable = new <>z__ReadOnlySingleElementList<IHoverTip>(NRelicCollectionEntry.LockedHoverTip);
				break;
			default:
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(modelVisibility);
				break;
			}
			IEnumerable<IHoverTip> enumerable2 = enumerable;
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, enumerable2, HoverTip.GetHoverTipAlignment(this, 0.75f));
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetFollowOwner();
			}
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x00108A6C File Offset: 0x00106C6C
		protected override void OnUnfocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._relicNode, "scale", Vector2.One, 1.0).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x00108ADC File Offset: 0x00106CDC
		protected override void OnPress()
		{
			base.OnPress();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._relicNode, "scale", Vector2.One, 1.0).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060026D6 RID: 9942 RVA: 0x00108B4C File Offset: 0x00106D4C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NRelicCollectionEntry.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollectionEntry.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollectionEntry.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollectionEntry.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollectionEntry.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060026D7 RID: 9943 RVA: 0x00108C4C File Offset: 0x00106E4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicCollectionEntry.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollectionEntry.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollectionEntry.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollectionEntry.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollectionEntry.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060026D8 RID: 9944 RVA: 0x00108D1C File Offset: 0x00106F1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRelicCollectionEntry.MethodName._Ready || (ref method) == NRelicCollectionEntry.MethodName.OnRelease || (ref method) == NRelicCollectionEntry.MethodName.OnFocus || (ref method) == NRelicCollectionEntry.MethodName.OnUnfocus || (ref method) == NRelicCollectionEntry.MethodName.OnPress || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060026D9 RID: 9945 RVA: 0x00108D7C File Offset: 0x00106F7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRelicCollectionEntry.PropertyName.ModelVisibility)
			{
				this.ModelVisibility = VariantUtils.ConvertTo<ModelVisibility>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollectionEntry.PropertyName._relicHolder)
			{
				this._relicHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollectionEntry.PropertyName._relicNode)
			{
				this._relicNode = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollectionEntry.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060026DA RID: 9946 RVA: 0x00108E00 File Offset: 0x00107000
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRelicCollectionEntry.PropertyName.ModelVisibility)
			{
				ModelVisibility modelVisibility = this.ModelVisibility;
				value = VariantUtils.CreateFrom<ModelVisibility>(ref modelVisibility);
				return true;
			}
			if ((ref name) == NRelicCollectionEntry.PropertyName._relicHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._relicHolder);
				return true;
			}
			if ((ref name) == NRelicCollectionEntry.PropertyName._relicNode)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._relicNode);
				return true;
			}
			if ((ref name) == NRelicCollectionEntry.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060026DB RID: 9947 RVA: 0x00108E98 File Offset: 0x00107098
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NRelicCollectionEntry.PropertyName.ModelVisibility, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollectionEntry.PropertyName._relicHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollectionEntry.PropertyName._relicNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollectionEntry.PropertyName._hoverTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060026DC RID: 9948 RVA: 0x00108F30 File Offset: 0x00107130
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName modelVisibility = NRelicCollectionEntry.PropertyName.ModelVisibility;
			ModelVisibility modelVisibility2 = this.ModelVisibility;
			info.AddProperty(modelVisibility, Variant.From<ModelVisibility>(ref modelVisibility2));
			info.AddProperty(NRelicCollectionEntry.PropertyName._relicHolder, Variant.From<Control>(ref this._relicHolder));
			info.AddProperty(NRelicCollectionEntry.PropertyName._relicNode, Variant.From<Control>(ref this._relicNode));
			info.AddProperty(NRelicCollectionEntry.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
		}

		/// <inheritdoc />
		// Token: 0x060026DD RID: 9949 RVA: 0x00108FA0 File Offset: 0x001071A0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRelicCollectionEntry.PropertyName.ModelVisibility, ref variant))
			{
				this.ModelVisibility = variant.As<ModelVisibility>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRelicCollectionEntry.PropertyName._relicHolder, ref variant2))
			{
				this._relicHolder = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRelicCollectionEntry.PropertyName._relicNode, ref variant3))
			{
				this._relicNode = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRelicCollectionEntry.PropertyName._hoverTween, ref variant4))
			{
				this._hoverTween = variant4.As<Tween>();
			}
		}

		// Token: 0x04000E6F RID: 3695
		public static readonly string scenePath = SceneHelper.GetScenePath("screens/relic_collection/relic_collection_entry");

		// Token: 0x04000E70 RID: 3696
		public static readonly string lockedIconPath = ImageHelper.GetImagePath("packed/common_ui/locked_model.png");

		// Token: 0x04000E72 RID: 3698
		public RelicModel relic;

		// Token: 0x04000E73 RID: 3699
		private Control _relicHolder;

		// Token: 0x04000E74 RID: 3700
		private Control _relicNode;

		// Token: 0x04000E75 RID: 3701
		[Nullable(2)]
		private Tween _hoverTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001457 RID: 5207
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004D3C RID: 19772
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004D3D RID: 19773
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004D3E RID: 19774
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004D3F RID: 19775
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004D40 RID: 19776
			public new static readonly StringName OnPress = "OnPress";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001458 RID: 5208
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ModelVisibility' property.
			/// </summary>
			// Token: 0x04004D41 RID: 19777
			public static readonly StringName ModelVisibility = "ModelVisibility";

			/// <summary>
			/// Cached name for the '_relicHolder' field.
			/// </summary>
			// Token: 0x04004D42 RID: 19778
			public static readonly StringName _relicHolder = "_relicHolder";

			/// <summary>
			/// Cached name for the '_relicNode' field.
			/// </summary>
			// Token: 0x04004D43 RID: 19779
			public static readonly StringName _relicNode = "_relicNode";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04004D44 RID: 19780
			public static readonly StringName _hoverTween = "_hoverTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001459 RID: 5209
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
