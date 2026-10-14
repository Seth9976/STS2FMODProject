using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Potions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.PotionLab
{
	// Token: 0x020002DD RID: 733
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/PotionLab/NLabPotionHolder.cs")]
	public class NLabPotionHolder : Control
	{
		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002721 RID: 10017 RVA: 0x0010B3B7 File Offset: 0x001095B7
		private LocString UnknownHoverTipTitle
		{
			get
			{
				return new LocString("main_menu_ui", "POTION_LAB_COLLECTION.unknown.title");
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002722 RID: 10018 RVA: 0x0010B3C8 File Offset: 0x001095C8
		private LocString UnknownHoverTipDescription
		{
			get
			{
				return new LocString("main_menu_ui", "POTION_LAB_COLLECTION.unknown.description");
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002723 RID: 10019 RVA: 0x0010B3D9 File Offset: 0x001095D9
		private HoverTip UnknownHoverTip
		{
			get
			{
				return new HoverTip(this.UnknownHoverTipTitle, this.UnknownHoverTipDescription, null);
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002724 RID: 10020 RVA: 0x0010B3ED File Offset: 0x001095ED
		private static LocString LockedHoverTipTitle
		{
			get
			{
				return new LocString("main_menu_ui", "POTION_LAB_COLLECTION.locked.title");
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002725 RID: 10021 RVA: 0x0010B3FE File Offset: 0x001095FE
		private static LocString LockedHoverTipDescription
		{
			get
			{
				return new LocString("main_menu_ui", "POTION_LAB_COLLECTION.locked.description");
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002726 RID: 10022 RVA: 0x0010B40F File Offset: 0x0010960F
		private static HoverTip LockedHoverTip
		{
			get
			{
				return new HoverTip(NLabPotionHolder.LockedHoverTipTitle, NLabPotionHolder.LockedHoverTipDescription, null);
			}
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x0010B424 File Offset: 0x00109624
		public static NLabPotionHolder Create(PotionModel potion, ModelVisibility visibility)
		{
			NLabPotionHolder nlabPotionHolder = PreloadManager.Cache.GetScene(NLabPotionHolder.scenePath).Instantiate<NLabPotionHolder>(0L);
			nlabPotionHolder._model = potion;
			nlabPotionHolder._visibility = visibility;
			return nlabPotionHolder;
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x0010B458 File Offset: 0x00109658
		public override void _Ready()
		{
			this._potionHolder = base.GetNode<Control>("PotionHolder");
			this._potionNode = NPotion.Create(this._model);
			this._potionHolder.AddChildSafely(this._potionNode);
			if (this._visibility == ModelVisibility.Locked)
			{
				this._potionNode.Image.Texture = PreloadManager.Cache.GetTexture2D(NLabPotionHolder.lockedIconPath);
				this._potionNode.Outline.Visible = false;
				this._potionNode.Modulate = StsColors.gray;
			}
			else if (this._visibility == ModelVisibility.NotSeen)
			{
				this._potionNode.Image.SelfModulate = StsColors.ninetyPercentBlack;
				this._potionNode.Outline.Modulate = StsColors.halfTransparentWhite;
			}
			else
			{
				foreach (PotionPoolModel potionPoolModel in ModelDb.AllCharacterPotionPools)
				{
					PotionModel potionModel = potionPoolModel.AllPotions.FirstOrDefault<PotionModel>((PotionModel p) => p.Id == this._model.Id);
					if (potionModel != null)
					{
						CanvasItem outline = this._potionNode.Outline;
						Color labOutlineColor = potionPoolModel.LabOutlineColor;
						labOutlineColor.A = 0.66f;
						outline.Modulate = labOutlineColor;
						break;
					}
				}
			}
			this._potionNode.MouseFilter = 2L;
			this._potionNode.PivotOffset = this._potionNode.Size * 0.5f;
			this._potionNode.Position = Vector2.Zero;
			base.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
			base.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocus)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)), 0U);
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x0010B650 File Offset: 0x00109850
		private void OnFocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			NHoverTipSet.Remove(this);
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._potionNode, "scale", Vector2.One * 1.2f, 0.05);
			ModelVisibility visibility = this._visibility;
			IEnumerable<IHoverTip> enumerable;
			switch (visibility)
			{
			case ModelVisibility.None:
				throw new ArgumentOutOfRangeException();
			case ModelVisibility.Visible:
				enumerable = this._potionNode.Model.HoverTips;
				break;
			case ModelVisibility.NotSeen:
				enumerable = new <>z__ReadOnlySingleElementList<IHoverTip>(this.UnknownHoverTip);
				break;
			case ModelVisibility.Locked:
				enumerable = new <>z__ReadOnlySingleElementList<IHoverTip>(NLabPotionHolder.LockedHoverTip);
				break;
			default:
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(visibility);
				break;
			}
			IEnumerable<IHoverTip> enumerable2 = enumerable;
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, enumerable2, HoverTip.GetHoverTipAlignment(this, 0.75f));
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetFollowOwner();
			}
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetExtraFollowOffset(new Vector2(32f, 0f));
			}
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x0010B758 File Offset: 0x00109958
		private void OnUnfocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._potionNode, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600272B RID: 10027 RVA: 0x0010B7C8 File Offset: 0x001099C8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NLabPotionHolder.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLabPotionHolder.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLabPotionHolder.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600272C RID: 10028 RVA: 0x0010B86C File Offset: 0x00109A6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLabPotionHolder.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLabPotionHolder.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLabPotionHolder.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600272D RID: 10029 RVA: 0x0010B8F1 File Offset: 0x00109AF1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLabPotionHolder.MethodName._Ready || (ref method) == NLabPotionHolder.MethodName.OnFocus || (ref method) == NLabPotionHolder.MethodName.OnUnfocus || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600272E RID: 10030 RVA: 0x0010B928 File Offset: 0x00109B28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLabPotionHolder.PropertyName._potionNode)
			{
				this._potionNode = VariantUtils.ConvertTo<NPotion>(ref value);
				return true;
			}
			if ((ref name) == NLabPotionHolder.PropertyName._potionHolder)
			{
				this._potionHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NLabPotionHolder.PropertyName._visibility)
			{
				this._visibility = VariantUtils.ConvertTo<ModelVisibility>(ref value);
				return true;
			}
			if ((ref name) == NLabPotionHolder.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600272F RID: 10031 RVA: 0x0010B9AC File Offset: 0x00109BAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLabPotionHolder.PropertyName._potionNode)
			{
				value = VariantUtils.CreateFrom<NPotion>(ref this._potionNode);
				return true;
			}
			if ((ref name) == NLabPotionHolder.PropertyName._potionHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._potionHolder);
				return true;
			}
			if ((ref name) == NLabPotionHolder.PropertyName._visibility)
			{
				value = VariantUtils.CreateFrom<ModelVisibility>(ref this._visibility);
				return true;
			}
			if ((ref name) == NLabPotionHolder.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002730 RID: 10032 RVA: 0x0010BA44 File Offset: 0x00109C44
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NLabPotionHolder.PropertyName._potionNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLabPotionHolder.PropertyName._potionHolder, 0L, "", 4096L, false),
				new PropertyInfo(2L, NLabPotionHolder.PropertyName._visibility, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLabPotionHolder.PropertyName._hoverTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002731 RID: 10033 RVA: 0x0010BADC File Offset: 0x00109CDC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLabPotionHolder.PropertyName._potionNode, Variant.From<NPotion>(ref this._potionNode));
			info.AddProperty(NLabPotionHolder.PropertyName._potionHolder, Variant.From<Control>(ref this._potionHolder));
			info.AddProperty(NLabPotionHolder.PropertyName._visibility, Variant.From<ModelVisibility>(ref this._visibility));
			info.AddProperty(NLabPotionHolder.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
		}

		/// <inheritdoc />
		// Token: 0x06002732 RID: 10034 RVA: 0x0010BB48 File Offset: 0x00109D48
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLabPotionHolder.PropertyName._potionNode, ref variant))
			{
				this._potionNode = variant.As<NPotion>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLabPotionHolder.PropertyName._potionHolder, ref variant2))
			{
				this._potionHolder = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NLabPotionHolder.PropertyName._visibility, ref variant3))
			{
				this._visibility = variant3.As<ModelVisibility>();
			}
			Variant variant4;
			if (info.TryGetProperty(NLabPotionHolder.PropertyName._hoverTween, ref variant4))
			{
				this._hoverTween = variant4.As<Tween>();
			}
		}

		// Token: 0x04000E91 RID: 3729
		public static readonly string scenePath = SceneHelper.GetScenePath("screens/potion_lab/lab_potion_holder");

		// Token: 0x04000E92 RID: 3730
		public static readonly string lockedIconPath = ImageHelper.GetImagePath("packed/common_ui/locked_model.png");

		// Token: 0x04000E93 RID: 3731
		private PotionModel _model;

		// Token: 0x04000E94 RID: 3732
		private NPotion _potionNode;

		// Token: 0x04000E95 RID: 3733
		private Control _potionHolder;

		/// <summary>
		/// How the potion should be shown (locked, hidden, or visible).
		/// </summary>
		// Token: 0x04000E96 RID: 3734
		private ModelVisibility _visibility;

		// Token: 0x04000E97 RID: 3735
		[Nullable(2)]
		private Tween _hoverTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001468 RID: 5224
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004D73 RID: 19827
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004D74 RID: 19828
			public static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004D75 RID: 19829
			public static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001469 RID: 5225
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_potionNode' field.
			/// </summary>
			// Token: 0x04004D76 RID: 19830
			public static readonly StringName _potionNode = "_potionNode";

			/// <summary>
			/// Cached name for the '_potionHolder' field.
			/// </summary>
			// Token: 0x04004D77 RID: 19831
			public static readonly StringName _potionHolder = "_potionHolder";

			/// <summary>
			/// Cached name for the '_visibility' field.
			/// </summary>
			// Token: 0x04004D78 RID: 19832
			public static readonly StringName _visibility = "_visibility";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04004D79 RID: 19833
			public static readonly StringName _hoverTween = "_hoverTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200146A RID: 5226
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
