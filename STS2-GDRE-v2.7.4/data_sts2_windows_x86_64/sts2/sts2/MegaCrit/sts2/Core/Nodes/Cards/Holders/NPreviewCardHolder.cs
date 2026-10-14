using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Cards.Holders
{
	// Token: 0x0200041F RID: 1055
	[ScriptPath("res://src/Core/Nodes/Cards/Holders/NPreviewCardHolder.cs")]
	public class NPreviewCardHolder : NCardHolder
	{
		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06004360 RID: 17248 RVA: 0x0020B90A File Offset: 0x00209B0A
		protected override Vector2 HoverScale
		{
			get
			{
				return this._originalScale * 1.1f;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06004361 RID: 17249 RVA: 0x0020B91C File Offset: 0x00209B1C
		public override Vector2 SmallScale
		{
			get
			{
				return this._originalScale;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06004362 RID: 17250 RVA: 0x0020B924 File Offset: 0x00209B24
		public override bool IsShowingUpgradedCard
		{
			get
			{
				return base.IsShowingUpgradedCard || (this.CardModel != null && this.CardModel.UpgradePreviewType.IsPreview());
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06004363 RID: 17251 RVA: 0x0020B94A File Offset: 0x00209B4A
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("cards/holders/preview_card_holder");
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06004364 RID: 17252 RVA: 0x0020B956 File Offset: 0x00209B56
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NPreviewCardHolder.ScenePath);
			}
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x0020B964 File Offset: 0x00209B64
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NPreviewCardHolder Create(NCard card, bool showHoverTips, bool scaleOnHover)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NPreviewCardHolder npreviewCardHolder = PreloadManager.Cache.GetScene(NPreviewCardHolder.ScenePath).Instantiate<NPreviewCardHolder>(0L);
			npreviewCardHolder.Initialize(card, showHoverTips, scaleOnHover);
			return npreviewCardHolder;
		}

		// Token: 0x06004366 RID: 17254 RVA: 0x0020B99B File Offset: 0x00209B9B
		public override void _Ready()
		{
			base.ConnectSignals();
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x0020B9A4 File Offset: 0x00209BA4
		[NullableContext(1)]
		private void Initialize(NCard card, bool showHoverTips, bool scaleOnHover)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
			defaultInterpolatedStringHandler.AppendLiteral("UpgradePreviewCardHolder-");
			CardModel model = card.Model;
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>((model != null) ? model.Id : null);
			base.Name = defaultInterpolatedStringHandler.ToStringAndClear();
			this.SetCard(card);
			base.Scale = this.SmallScale;
			this._showHoverTips = showHoverTips;
			this._scaleOnHover = scaleOnHover;
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x0020BA13 File Offset: 0x00209C13
		protected override void OnFocus()
		{
			this._isHovered = true;
			if (this._scaleOnHover)
			{
				Tween hoverTween = this._hoverTween;
				if (hoverTween != null)
				{
					hoverTween.Kill();
				}
				base.Scale = this.HoverScale;
			}
			if (this._showHoverTips)
			{
				this.CreateHoverTips();
			}
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x0020BA50 File Offset: 0x00209C50
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			if (this._scaleOnHover)
			{
				Tween hoverTween = this._hoverTween;
				if (hoverTween != null)
				{
					hoverTween.Kill();
				}
				this._hoverTween = base.CreateTween();
				this._hoverTween.TweenProperty(this, "scale", this.SmallScale, 0.5).SetEase(1L).SetTrans(5L);
			}
			if (this._showHoverTips)
			{
				base.ClearHoverTips();
			}
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x0020BACF File Offset: 0x00209CCF
		public void SetCardScale(Vector2 scale)
		{
			this._originalScale = scale;
			base.Scale = this._originalScale;
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x0020BAE4 File Offset: 0x00209CE4
		protected override void CreateHoverTips()
		{
			if (base.CardNode == null)
			{
				return;
			}
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, base.CardNode.Model.HoverTips, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetAlignmentForCardHolder(this);
			}
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x0020BB1C File Offset: 0x00209D1C
		public override void _ExitTree()
		{
			if (base.CardNode != null)
			{
				if (base.IsAncestorOf(base.CardNode))
				{
					base.CardNode.QueueFreeSafely();
				}
				base.CardNode = null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600436D RID: 17261 RVA: 0x0020BB48 File Offset: 0x00209D48
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NPreviewCardHolder.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(1L, "showHoverTips", 0L, "", 6L, false),
					new PropertyInfo(1L, "scaleOnHover", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPreviewCardHolder.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPreviewCardHolder.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(1L, "showHoverTips", 0L, "", 6L, false),
					new PropertyInfo(1L, "scaleOnHover", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPreviewCardHolder.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPreviewCardHolder.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPreviewCardHolder.MethodName.SetCardScale, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "scale", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPreviewCardHolder.MethodName.CreateHoverTips, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPreviewCardHolder.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600436E RID: 17262 RVA: 0x0020BDEC File Offset: 0x00209FEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPreviewCardHolder.MethodName.Create && args.Count == 3)
			{
				NPreviewCardHolder npreviewCardHolder = NPreviewCardHolder.Create(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NPreviewCardHolder>(ref npreviewCardHolder);
				return true;
			}
			if ((ref method) == NPreviewCardHolder.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPreviewCardHolder.MethodName.Initialize && args.Count == 3)
			{
				this.Initialize(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPreviewCardHolder.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPreviewCardHolder.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPreviewCardHolder.MethodName.SetCardScale && args.Count == 1)
			{
				this.SetCardScale(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPreviewCardHolder.MethodName.CreateHoverTips && args.Count == 0)
			{
				this.CreateHoverTips();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPreviewCardHolder.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x0020BF90 File Offset: 0x0020A190
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPreviewCardHolder.MethodName.Create && args.Count == 3)
			{
				NPreviewCardHolder npreviewCardHolder = NPreviewCardHolder.Create(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NPreviewCardHolder>(ref npreviewCardHolder);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06004370 RID: 17264 RVA: 0x0020BFF8 File Offset: 0x0020A1F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPreviewCardHolder.MethodName.Create || (ref method) == NPreviewCardHolder.MethodName._Ready || (ref method) == NPreviewCardHolder.MethodName.Initialize || (ref method) == NPreviewCardHolder.MethodName.OnFocus || (ref method) == NPreviewCardHolder.MethodName.OnUnfocus || (ref method) == NPreviewCardHolder.MethodName.SetCardScale || (ref method) == NPreviewCardHolder.MethodName.CreateHoverTips || (ref method) == NPreviewCardHolder.MethodName._ExitTree || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06004371 RID: 17265 RVA: 0x0020C084 File Offset: 0x0020A284
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPreviewCardHolder.PropertyName._showHoverTips)
			{
				this._showHoverTips = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPreviewCardHolder.PropertyName._scaleOnHover)
			{
				this._scaleOnHover = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPreviewCardHolder.PropertyName._originalScale)
			{
				this._originalScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06004372 RID: 17266 RVA: 0x0020C0EC File Offset: 0x0020A2EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPreviewCardHolder.PropertyName.HoverScale)
			{
				Vector2 vector = this.HoverScale;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NPreviewCardHolder.PropertyName.SmallScale)
			{
				Vector2 vector = this.SmallScale;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NPreviewCardHolder.PropertyName.IsShowingUpgradedCard)
			{
				bool isShowingUpgradedCard = this.IsShowingUpgradedCard;
				value = VariantUtils.CreateFrom<bool>(ref isShowingUpgradedCard);
				return true;
			}
			if ((ref name) == NPreviewCardHolder.PropertyName._showHoverTips)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._showHoverTips);
				return true;
			}
			if ((ref name) == NPreviewCardHolder.PropertyName._scaleOnHover)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._scaleOnHover);
				return true;
			}
			if ((ref name) == NPreviewCardHolder.PropertyName._originalScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._originalScale);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004373 RID: 17267 RVA: 0x0020C1CC File Offset: 0x0020A3CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NPreviewCardHolder.PropertyName.HoverScale, 0L, "", 4096L, false),
				new PropertyInfo(5L, NPreviewCardHolder.PropertyName.SmallScale, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPreviewCardHolder.PropertyName._showHoverTips, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPreviewCardHolder.PropertyName._scaleOnHover, 0L, "", 4096L, false),
				new PropertyInfo(5L, NPreviewCardHolder.PropertyName._originalScale, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPreviewCardHolder.PropertyName.IsShowingUpgradedCard, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004374 RID: 17268 RVA: 0x0020C2A0 File Offset: 0x0020A4A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPreviewCardHolder.PropertyName._showHoverTips, Variant.From<bool>(ref this._showHoverTips));
			info.AddProperty(NPreviewCardHolder.PropertyName._scaleOnHover, Variant.From<bool>(ref this._scaleOnHover));
			info.AddProperty(NPreviewCardHolder.PropertyName._originalScale, Variant.From<Vector2>(ref this._originalScale));
		}

		/// <inheritdoc />
		// Token: 0x06004375 RID: 17269 RVA: 0x0020C2F8 File Offset: 0x0020A4F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPreviewCardHolder.PropertyName._showHoverTips, ref variant))
			{
				this._showHoverTips = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPreviewCardHolder.PropertyName._scaleOnHover, ref variant2))
			{
				this._scaleOnHover = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPreviewCardHolder.PropertyName._originalScale, ref variant3))
			{
				this._originalScale = variant3.As<Vector2>();
			}
		}

		// Token: 0x04001A30 RID: 6704
		private bool _showHoverTips;

		// Token: 0x04001A31 RID: 6705
		private bool _scaleOnHover;

		// Token: 0x04001A32 RID: 6706
		private Vector2 _originalScale = Vector2.One;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200196F RID: 6511
		public new class MethodName : NCardHolder.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04006488 RID: 25736
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006489 RID: 25737
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x0400648A RID: 25738
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400648B RID: 25739
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400648C RID: 25740
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'SetCardScale' method.
			/// </summary>
			// Token: 0x0400648D RID: 25741
			public static readonly StringName SetCardScale = "SetCardScale";

			/// <summary>
			/// Cached name for the 'CreateHoverTips' method.
			/// </summary>
			// Token: 0x0400648E RID: 25742
			public new static readonly StringName CreateHoverTips = "CreateHoverTips";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400648F RID: 25743
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001970 RID: 6512
		public new class PropertyName : NCardHolder.PropertyName
		{
			/// <summary>
			/// Cached name for the 'HoverScale' property.
			/// </summary>
			// Token: 0x04006490 RID: 25744
			public new static readonly StringName HoverScale = "HoverScale";

			/// <summary>
			/// Cached name for the 'SmallScale' property.
			/// </summary>
			// Token: 0x04006491 RID: 25745
			public new static readonly StringName SmallScale = "SmallScale";

			/// <summary>
			/// Cached name for the 'IsShowingUpgradedCard' property.
			/// </summary>
			// Token: 0x04006492 RID: 25746
			public new static readonly StringName IsShowingUpgradedCard = "IsShowingUpgradedCard";

			/// <summary>
			/// Cached name for the '_showHoverTips' field.
			/// </summary>
			// Token: 0x04006493 RID: 25747
			public static readonly StringName _showHoverTips = "_showHoverTips";

			/// <summary>
			/// Cached name for the '_scaleOnHover' field.
			/// </summary>
			// Token: 0x04006494 RID: 25748
			public static readonly StringName _scaleOnHover = "_scaleOnHover";

			/// <summary>
			/// Cached name for the '_originalScale' field.
			/// </summary>
			// Token: 0x04006495 RID: 25749
			public static readonly StringName _originalScale = "_originalScale";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001971 RID: 6513
		public new class SignalName : NCardHolder.SignalName
		{
		}
	}
}
