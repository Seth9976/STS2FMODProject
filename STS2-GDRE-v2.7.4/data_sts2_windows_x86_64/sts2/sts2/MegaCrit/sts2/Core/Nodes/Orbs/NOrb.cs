using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Orbs
{
	// Token: 0x02000388 RID: 904
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Orbs/NOrb.cs")]
	public class NOrb : NClickableControl
	{
		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x060035A9 RID: 13737 RVA: 0x00190208 File Offset: 0x0018E408
		// (set) Token: 0x060035AA RID: 13738 RVA: 0x00190210 File Offset: 0x0018E410
		[Nullable(2)]
		public OrbModel Model
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x060035AB RID: 13739 RVA: 0x00190219 File Offset: 0x0018E419
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("/orbs/orb");
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x060035AC RID: 13740 RVA: 0x00190225 File Offset: 0x0018E425
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NOrb.ScenePath);
			}
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x00190234 File Offset: 0x0018E434
		public static NOrb Create(bool isLocal)
		{
			NOrb norb = PreloadManager.Cache.GetScene(NOrb.ScenePath).Instantiate<NOrb>(0L);
			norb._isLocal = isLocal;
			return norb;
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x00190260 File Offset: 0x0018E460
		public static NOrb Create(bool isLocal, [Nullable(2)] OrbModel model)
		{
			NOrb norb = NOrb.Create(isLocal);
			norb.Model = model;
			return norb;
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x0019027C File Offset: 0x0018E47C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._outline = base.GetNode<TextureRect>("%Outline");
			this._visualContainer = base.GetNode<Control>("%VisualContainer");
			this._passiveLabel = base.GetNode<MegaLabel>("%PassiveAmount");
			this._evokeLabel = base.GetNode<MegaLabel>("%EvokeAmount");
			this._bounds = base.GetNode<Control>("Bounds");
			this._labelContainer = base.GetNode<Control>("%LabelContainer");
			this._selectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			if (this.Model != null)
			{
				base.CreateTween().TweenProperty(this._outline, "scale", Vector2.One, 0.25).From(Vector2.Zero);
			}
			if (this._isLocal)
			{
				base.Scale *= 0.85f;
			}
			this.UpdateVisuals(false);
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x00190394 File Offset: 0x0018E594
		public void ReplaceOrb(OrbModel model)
		{
			Node2D sprite = this._sprite;
			if (sprite != null)
			{
				sprite.QueueFreeSafely();
			}
			this._sprite = null;
			this.Model = model;
			this.UpdateVisuals(false);
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x001903BC File Offset: 0x0018E5BC
		public void UpdateVisuals(bool isEvoking)
		{
			if (!base.IsNodeReady())
			{
				return;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			if (this.Model == null)
			{
				Node2D sprite = this._sprite;
				if (sprite != null)
				{
					sprite.QueueFreeSafely();
				}
				this._passiveLabel.Visible = false;
				this._evokeLabel.Visible = false;
				this._outline.Visible = this._isLocal;
				return;
			}
			if (this._sprite == null)
			{
				this._sprite = this.Model.CreateSprite();
				this._visualContainer.AddChildSafely(this._sprite);
				this._sprite.Position = Vector2.Zero;
				this.RunWhenSpineReady(new MegaSprite(this._sprite.GetNode("SpineSkeleton")), delegate(MegaAnimationState animState)
				{
					animState.SetAnimation("idle_loop", true, 0);
				});
				this._orbVfx = this._sprite as NOrbVfx;
				if (this._orbVfx != null)
				{
					this._orbVfx.Initialize(this.Model);
				}
				Tween curTween = this._curTween;
				if (curTween != null)
				{
					curTween.Kill();
				}
				this._curTween = base.CreateTween();
				this._curTween.TweenProperty(this._sprite, "scale", Vector2.One, 0.5).From(Vector2.Zero).SetTrans(10L)
					.SetEase(1L);
			}
			this._outline.Visible = false;
			this._labelContainer.Visible = this._isLocal;
			if (!this._isLocal)
			{
				base.Modulate = this.Model.DarkenedColor;
			}
			OrbModel model = this.Model;
			if (model is PlasmaOrb)
			{
				this._passiveLabel.Visible = false;
				this._evokeLabel.Visible = false;
				return;
			}
			if (model is DarkOrb)
			{
				this._passiveLabel.Visible = true;
				this._evokeLabel.Visible = true;
				this._passiveLabel.SetTextAutoSize(this.Model.PassiveVal.ToString("0"));
				this._evokeLabel.SetTextAutoSize(this.Model.EvokeVal.ToString("0"));
				return;
			}
			if (!(model is GlassOrb))
			{
				this._passiveLabel.Visible = !isEvoking;
				this._evokeLabel.Visible = isEvoking;
				this._passiveLabel.SetTextAutoSize(this.Model.PassiveVal.ToString("0"));
				this._evokeLabel.SetTextAutoSize(this.Model.EvokeVal.ToString("0"));
				return;
			}
			this._passiveLabel.Visible = !isEvoking;
			this._evokeLabel.Visible = isEvoking;
			this._sprite.Modulate = ((this.Model.PassiveVal == 0m) ? this.Model.DarkenedColor : Colors.White);
			this._passiveLabel.SetTextAutoSize(this.Model.PassiveVal.ToString("0"));
			this._evokeLabel.SetTextAutoSize(this.Model.EvokeVal.ToString("0"));
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x00190700 File Offset: 0x0018E900
		protected override void OnFocus()
		{
			if (this.Model == null && !this._isLocal)
			{
				return;
			}
			IEnumerable<IHoverTip> enumerable;
			if (this.Model != null)
			{
				enumerable = this.Model.HoverTips;
			}
			else
			{
				IEnumerable<IHoverTip> enumerable2 = new List<IHoverTip> { OrbModel.EmptySlotHoverTipHoverTip };
				enumerable = enumerable2;
			}
			IEnumerable<IHoverTip> enumerable3 = enumerable;
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this._bounds, enumerable3, HoverTip.GetHoverTipAlignment(this._bounds, 0.75f));
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetFollowOwner();
			}
			this._labelContainer.Visible = true;
			base.Modulate = Colors.White;
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x001907A0 File Offset: 0x0018E9A0
		protected override void OnUnfocus()
		{
			this._labelContainer.Visible = this._isLocal;
			if (this.Model != null)
			{
				base.Modulate = (this._isLocal ? Colors.White : this.Model.DarkenedColor);
			}
			NHoverTipSet.Remove(this._bounds);
			this._selectionReticle.OnDeselect();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060035B4 RID: 13748 RVA: 0x001907FC File Offset: 0x0018E9FC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NOrb.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isLocal", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NOrb.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOrb.MethodName.UpdateVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isEvoking", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NOrb.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOrb.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060035B5 RID: 13749 RVA: 0x00190954 File Offset: 0x0018EB54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NOrb.MethodName.Create && args.Count == 1)
			{
				NOrb norb = NOrb.Create(VariantUtils.ConvertTo<bool>(args[0]));
				ret = VariantUtils.CreateFrom<NOrb>(ref norb);
				return true;
			}
			if ((ref method) == NOrb.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrb.MethodName.UpdateVisuals && args.Count == 1)
			{
				this.UpdateVisuals(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrb.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOrb.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x00190A48 File Offset: 0x0018EC48
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NOrb.MethodName.Create && args.Count == 1)
			{
				NOrb norb = NOrb.Create(VariantUtils.ConvertTo<bool>(args[0]));
				ret = VariantUtils.CreateFrom<NOrb>(ref norb);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060035B7 RID: 13751 RVA: 0x00190A98 File Offset: 0x0018EC98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NOrb.MethodName.Create || (ref method) == NOrb.MethodName._Ready || (ref method) == NOrb.MethodName.UpdateVisuals || (ref method) == NOrb.MethodName.OnFocus || (ref method) == NOrb.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060035B8 RID: 13752 RVA: 0x00190AF8 File Offset: 0x0018ECF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NOrb.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._visualContainer)
			{
				this._visualContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._labelContainer)
			{
				this._labelContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._passiveLabel)
			{
				this._passiveLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._evokeLabel)
			{
				this._evokeLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._bounds)
			{
				this._bounds = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._orbVfx)
			{
				this._orbVfx = VariantUtils.ConvertTo<NOrbVfx>(ref value);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._isLocal)
			{
				this._isLocal = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._sprite)
			{
				this._sprite = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._curTween)
			{
				this._curTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060035B9 RID: 13753 RVA: 0x00190C38 File Offset: 0x0018EE38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NOrb.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._visualContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._visualContainer);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._labelContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._labelContainer);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._passiveLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._passiveLabel);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._evokeLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._evokeLabel);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._bounds)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bounds);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._orbVfx)
			{
				value = VariantUtils.CreateFrom<NOrbVfx>(ref this._orbVfx);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._isLocal)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isLocal);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._sprite)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._sprite);
				return true;
			}
			if ((ref name) == NOrb.PropertyName._curTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._curTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060035BA RID: 13754 RVA: 0x00190DB0 File Offset: 0x0018EFB0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NOrb.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrb.PropertyName._visualContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrb.PropertyName._labelContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrb.PropertyName._passiveLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrb.PropertyName._evokeLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrb.PropertyName._bounds, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrb.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrb.PropertyName._orbVfx, 0L, "", 4096L, false),
				new PropertyInfo(1L, NOrb.PropertyName._isLocal, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrb.PropertyName._sprite, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOrb.PropertyName._curTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060035BB RID: 13755 RVA: 0x00190F30 File Offset: 0x0018F130
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NOrb.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
			info.AddProperty(NOrb.PropertyName._visualContainer, Variant.From<Control>(ref this._visualContainer));
			info.AddProperty(NOrb.PropertyName._labelContainer, Variant.From<Control>(ref this._labelContainer));
			info.AddProperty(NOrb.PropertyName._passiveLabel, Variant.From<MegaLabel>(ref this._passiveLabel));
			info.AddProperty(NOrb.PropertyName._evokeLabel, Variant.From<MegaLabel>(ref this._evokeLabel));
			info.AddProperty(NOrb.PropertyName._bounds, Variant.From<Control>(ref this._bounds));
			info.AddProperty(NOrb.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NOrb.PropertyName._orbVfx, Variant.From<NOrbVfx>(ref this._orbVfx));
			info.AddProperty(NOrb.PropertyName._isLocal, Variant.From<bool>(ref this._isLocal));
			info.AddProperty(NOrb.PropertyName._sprite, Variant.From<Node2D>(ref this._sprite));
			info.AddProperty(NOrb.PropertyName._curTween, Variant.From<Tween>(ref this._curTween));
		}

		/// <inheritdoc />
		// Token: 0x060035BC RID: 13756 RVA: 0x00191038 File Offset: 0x0018F238
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NOrb.PropertyName._outline, ref variant))
			{
				this._outline = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NOrb.PropertyName._visualContainer, ref variant2))
			{
				this._visualContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NOrb.PropertyName._labelContainer, ref variant3))
			{
				this._labelContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NOrb.PropertyName._passiveLabel, ref variant4))
			{
				this._passiveLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NOrb.PropertyName._evokeLabel, ref variant5))
			{
				this._evokeLabel = variant5.As<MegaLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NOrb.PropertyName._bounds, ref variant6))
			{
				this._bounds = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NOrb.PropertyName._selectionReticle, ref variant7))
			{
				this._selectionReticle = variant7.As<NSelectionReticle>();
			}
			Variant variant8;
			if (info.TryGetProperty(NOrb.PropertyName._orbVfx, ref variant8))
			{
				this._orbVfx = variant8.As<NOrbVfx>();
			}
			Variant variant9;
			if (info.TryGetProperty(NOrb.PropertyName._isLocal, ref variant9))
			{
				this._isLocal = variant9.As<bool>();
			}
			Variant variant10;
			if (info.TryGetProperty(NOrb.PropertyName._sprite, ref variant10))
			{
				this._sprite = variant10.As<Node2D>();
			}
			Variant variant11;
			if (info.TryGetProperty(NOrb.PropertyName._curTween, ref variant11))
			{
				this._curTween = variant11.As<Tween>();
			}
		}

		// Token: 0x040014C4 RID: 5316
		private TextureRect _outline;

		// Token: 0x040014C5 RID: 5317
		private Control _visualContainer;

		// Token: 0x040014C6 RID: 5318
		private Control _labelContainer;

		// Token: 0x040014C7 RID: 5319
		private MegaLabel _passiveLabel;

		// Token: 0x040014C8 RID: 5320
		private MegaLabel _evokeLabel;

		// Token: 0x040014C9 RID: 5321
		private Control _bounds;

		// Token: 0x040014CA RID: 5322
		private NSelectionReticle _selectionReticle;

		// Token: 0x040014CB RID: 5323
		[Nullable(2)]
		private NOrbVfx _orbVfx;

		// Token: 0x040014CC RID: 5324
		private bool _isLocal;

		// Token: 0x040014CD RID: 5325
		[Nullable(2)]
		private Node2D _sprite;

		// Token: 0x040014CE RID: 5326
		[Nullable(2)]
		private Tween _curTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001706 RID: 5894
		[NullableContext(0)]
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040059BE RID: 22974
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040059BF RID: 22975
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'UpdateVisuals' method.
			/// </summary>
			// Token: 0x040059C0 RID: 22976
			public static readonly StringName UpdateVisuals = "UpdateVisuals";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040059C1 RID: 22977
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040059C2 RID: 22978
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001707 RID: 5895
		[NullableContext(0)]
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x040059C3 RID: 22979
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_visualContainer' field.
			/// </summary>
			// Token: 0x040059C4 RID: 22980
			public static readonly StringName _visualContainer = "_visualContainer";

			/// <summary>
			/// Cached name for the '_labelContainer' field.
			/// </summary>
			// Token: 0x040059C5 RID: 22981
			public static readonly StringName _labelContainer = "_labelContainer";

			/// <summary>
			/// Cached name for the '_passiveLabel' field.
			/// </summary>
			// Token: 0x040059C6 RID: 22982
			public static readonly StringName _passiveLabel = "_passiveLabel";

			/// <summary>
			/// Cached name for the '_evokeLabel' field.
			/// </summary>
			// Token: 0x040059C7 RID: 22983
			public static readonly StringName _evokeLabel = "_evokeLabel";

			/// <summary>
			/// Cached name for the '_bounds' field.
			/// </summary>
			// Token: 0x040059C8 RID: 22984
			public static readonly StringName _bounds = "_bounds";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x040059C9 RID: 22985
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_orbVfx' field.
			/// </summary>
			// Token: 0x040059CA RID: 22986
			public static readonly StringName _orbVfx = "_orbVfx";

			/// <summary>
			/// Cached name for the '_isLocal' field.
			/// </summary>
			// Token: 0x040059CB RID: 22987
			public static readonly StringName _isLocal = "_isLocal";

			/// <summary>
			/// Cached name for the '_sprite' field.
			/// </summary>
			// Token: 0x040059CC RID: 22988
			public static readonly StringName _sprite = "_sprite";

			/// <summary>
			/// Cached name for the '_curTween' field.
			/// </summary>
			// Token: 0x040059CD RID: 22989
			public static readonly StringName _curTween = "_curTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001708 RID: 5896
		[NullableContext(0)]
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
