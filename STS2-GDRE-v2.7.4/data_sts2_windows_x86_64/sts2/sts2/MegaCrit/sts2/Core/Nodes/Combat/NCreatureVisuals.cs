using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x020003FB RID: 1019
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NCreatureVisuals.cs")]
	public class NCreatureVisuals : Node2D
	{
		// Token: 0x06003F43 RID: 16195 RVA: 0x001E57D8 File Offset: 0x001E39D8
		public Node2D GetCurrentBody()
		{
			Node2D phobiaModeBody = this._phobiaModeBody;
			if (phobiaModeBody == null || !phobiaModeBody.Visible)
			{
				return this._body;
			}
			return this._phobiaModeBody;
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003F44 RID: 16196 RVA: 0x001E5804 File Offset: 0x001E3A04
		// (set) Token: 0x06003F45 RID: 16197 RVA: 0x001E580C File Offset: 0x001E3A0C
		public Control Bounds { get; private set; }

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003F46 RID: 16198 RVA: 0x001E5815 File Offset: 0x001E3A15
		// (set) Token: 0x06003F47 RID: 16199 RVA: 0x001E581D File Offset: 0x001E3A1D
		public Marker2D IntentPosition { get; private set; }

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003F48 RID: 16200 RVA: 0x001E5826 File Offset: 0x001E3A26
		// (set) Token: 0x06003F49 RID: 16201 RVA: 0x001E582E File Offset: 0x001E3A2E
		public Marker2D OrbPosition { get; private set; }

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06003F4A RID: 16202 RVA: 0x001E5837 File Offset: 0x001E3A37
		// (set) Token: 0x06003F4B RID: 16203 RVA: 0x001E583F File Offset: 0x001E3A3F
		[Nullable(2)]
		public Marker2D TalkPosition
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06003F4C RID: 16204 RVA: 0x001E5848 File Offset: 0x001E3A48
		private bool IsSpineNode
		{
			get
			{
				return GodotObject.IsInstanceValid(this._body) && this._body.GetClass() == "SpineSprite";
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06003F4D RID: 16205 RVA: 0x001E586E File Offset: 0x001E3A6E
		public bool HasSpineAnimation
		{
			get
			{
				return this.SpineBody != null;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06003F4E RID: 16206 RVA: 0x001E5879 File Offset: 0x001E3A79
		public bool IsUsingPhobiaModeBody
		{
			get
			{
				return this._phobiaModeBody == this.GetCurrentBody();
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06003F4F RID: 16207 RVA: 0x001E5889 File Offset: 0x001E3A89
		// (set) Token: 0x06003F50 RID: 16208 RVA: 0x001E5891 File Offset: 0x001E3A91
		[Nullable(2)]
		public MegaSprite SpineBody
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06003F51 RID: 16209 RVA: 0x001E589A File Offset: 0x001E3A9A
		public SpineAnimationAccess SpineAnimation
		{
			get
			{
				return new SpineAnimationAccess(this.SpineBody);
			}
		}

		/// <summary>
		/// Position we spawn things like hit vfx. Sometimes the center of the creatures isn't always the center of the visual.
		/// </summary>
		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06003F52 RID: 16210 RVA: 0x001E58A7 File Offset: 0x001E3AA7
		// (set) Token: 0x06003F53 RID: 16211 RVA: 0x001E58AF File Offset: 0x001E3AAF
		public Marker2D VfxSpawnPosition { get; private set; }

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003F54 RID: 16212 RVA: 0x001E58B8 File Offset: 0x001E3AB8
		// (set) Token: 0x06003F55 RID: 16213 RVA: 0x001E58C0 File Offset: 0x001E3AC0
		public float DefaultScale { get; set; } = 1f;

		// Token: 0x06003F56 RID: 16214 RVA: 0x001E58CC File Offset: 0x001E3ACC
		public override void _Ready()
		{
			this._body = base.GetNode<Node2D>("%Visuals");
			this._phobiaModeBody = base.GetNodeOrNull<Node2D>("%PhobiaModeVisuals");
			this.Bounds = base.GetNode<Control>("%Bounds");
			this.IntentPosition = base.GetNode<Marker2D>("%IntentPos");
			this.VfxSpawnPosition = base.GetNode<Marker2D>("%CenterPos");
			this.OrbPosition = (base.HasNode("%OrbPos") ? base.GetNode<Marker2D>("%OrbPos") : this.IntentPosition);
			this.TalkPosition = (base.HasNode("%TalkPos") ? base.GetNode<Marker2D>("%TalkPos") : null);
			if (this.IsSpineNode)
			{
				this.SpineBody = new MegaSprite(this._body);
				MegaSkeleton skeleton = this.SpineBody.GetSkeleton();
				if (((skeleton != null) ? skeleton.GetData() : null) == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Spine skeleton data failed to load for ");
					defaultInterpolatedStringHandler.AppendFormatted<StringName>(base.Name);
					defaultInterpolatedStringHandler.AppendLiteral(", disabling spine animation.");
					GD.PushWarning(defaultInterpolatedStringHandler.ToStringAndClear());
					this.SpineBody = null;
				}
			}
			this._savedNormalMaterial = null;
			this._currentLiquidOverlayMaterial = null;
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x001E5A29 File Offset: 0x001E3C29
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x001E5A36 File Offset: 0x001E3C36
		public override void _ExitTree()
		{
			this._cts.Cancel();
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x001E5A44 File Offset: 0x001E3C44
		[NullableContext(2)]
		public void UpdatePhobiaMode(MonsterModel model)
		{
			if (this._phobiaModeBody != null)
			{
				this._phobiaModeBody.Visible = SaveManager.Instance.PrefsSave.PhobiaMode;
				this._body.Visible = !this._phobiaModeBody.Visible;
			}
			if (this.SpineBody != null)
			{
				MegaSkeleton skeleton = this.SpineBody.GetSkeleton();
				if (skeleton != null && model != null)
				{
					model.OnPhobiaModeToggled(SaveManager.Instance.PrefsSave.PhobiaMode, this.SpineBody, skeleton);
				}
			}
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x001E5AC4 File Offset: 0x001E3CC4
		public void SetUpSkin(MonsterModel model)
		{
			if (this.SpineBody == null)
			{
				return;
			}
			MegaSkeleton skeleton = this.SpineBody.GetSkeleton();
			if (skeleton != null)
			{
				model.SetupSkins(this.SpineBody, skeleton);
			}
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x001E5AF8 File Offset: 0x001E3CF8
		public void SetScaleAndHue(float scale, float hue)
		{
			this.DefaultScale = scale;
			base.Scale = Vector2.One * scale;
			this._hue = hue;
			if (!Mathf.IsEqualApprox(hue, 0f) && this.SpineBody != null)
			{
				Material normalMaterial = this.SpineBody.GetNormalMaterial();
				ShaderMaterial shaderMaterial;
				if (normalMaterial == null)
				{
					Material material = (ShaderMaterial)PreloadManager.Cache.GetMaterial("res://materials/vfx/hsv.tres");
					shaderMaterial = (ShaderMaterial)material.Duplicate(false);
					this.SpineBody.SetNormalMaterial(shaderMaterial);
				}
				else
				{
					shaderMaterial = (ShaderMaterial)normalMaterial;
				}
				shaderMaterial.SetShaderParameter(NCreatureVisuals._h, hue);
			}
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x001E5B90 File Offset: 0x001E3D90
		public bool IsPlayingHurtAnimation()
		{
			return this.SpineAnimation.GetCurrentAnimationName(0) == "hurt";
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x001E5BB6 File Offset: 0x001E3DB6
		public void TryApplyLiquidOverlay(Color tint)
		{
			if (this._currentLiquidOverlayMaterial != null)
			{
				this._currentLiquidOverlayMaterial.SetShaderParameter(NCreatureVisuals._tint, tint);
				this._liquidOverlayTimer = 1.0;
				return;
			}
			TaskHelper.RunSafely(this.ApplyLiquidOverlayInternal(tint));
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x001E5BF4 File Offset: 0x001E3DF4
		private async Task ApplyLiquidOverlayInternal(Color tint)
		{
			if (this.SpineBody != null)
			{
				this._savedNormalMaterial = this.SpineBody.GetNormalMaterial();
				Material material = (ShaderMaterial)PreloadManager.Cache.GetMaterial("res://materials/vfx/potion/potion_liquid_overlay.tres");
				this._currentLiquidOverlayMaterial = (ShaderMaterial)material.Duplicate(false);
				this._currentLiquidOverlayMaterial.SetShaderParameter(NCreatureVisuals._tint, tint);
				this._currentLiquidOverlayMaterial.SetShaderParameter(NCreatureVisuals._h, this._hue);
				this._currentLiquidOverlayMaterial.SetShaderParameter(NCreatureVisuals._overlayInfluence, 1f);
				this.SpineBody.SetNormalMaterial(this._currentLiquidOverlayMaterial);
				this._liquidOverlayTimer = 1.0;
				while (this._liquidOverlayTimer > 0.0)
				{
					double num = (1.0 - this._liquidOverlayTimer) / 1.0;
					this._currentLiquidOverlayMaterial.SetShaderParameter(NCreatureVisuals._overlayInfluence, 1.0 - num);
					this._liquidOverlayTimer -= base.GetProcessDeltaTime();
					await this.AwaitProcessFrame(this._cts.Token);
				}
				this.SpineBody.SetNormalMaterial(this._savedNormalMaterial);
				this._currentLiquidOverlayMaterial = null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003F5F RID: 16223 RVA: 0x001E5C40 File Offset: 0x001E3E40
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NCreatureVisuals.MethodName.GetCurrentBody, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 1L, null, null),
				new MethodInfo(NCreatureVisuals.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureVisuals.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureVisuals.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureVisuals.MethodName.SetScaleAndHue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "scale", 0L, "", 6L, false),
					new PropertyInfo(3L, "hue", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCreatureVisuals.MethodName.IsPlayingHurtAnimation, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCreatureVisuals.MethodName.TryApplyLiquidOverlay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003F60 RID: 16224 RVA: 0x001E5E18 File Offset: 0x001E4018
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCreatureVisuals.MethodName.GetCurrentBody && args.Count == 0)
			{
				Node2D currentBody = this.GetCurrentBody();
				ret = VariantUtils.CreateFrom<Node2D>(ref currentBody);
				return true;
			}
			if ((ref method) == NCreatureVisuals.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureVisuals.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureVisuals.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureVisuals.MethodName.SetScaleAndHue && args.Count == 2)
			{
				this.SetScaleAndHue(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCreatureVisuals.MethodName.IsPlayingHurtAnimation && args.Count == 0)
			{
				bool flag = this.IsPlayingHurtAnimation();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NCreatureVisuals.MethodName.TryApplyLiquidOverlay && args.Count == 1)
			{
				this.TryApplyLiquidOverlay(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003F61 RID: 16225 RVA: 0x001E5F68 File Offset: 0x001E4168
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCreatureVisuals.MethodName.GetCurrentBody || (ref method) == NCreatureVisuals.MethodName._Ready || (ref method) == NCreatureVisuals.MethodName._EnterTree || (ref method) == NCreatureVisuals.MethodName._ExitTree || (ref method) == NCreatureVisuals.MethodName.SetScaleAndHue || (ref method) == NCreatureVisuals.MethodName.IsPlayingHurtAnimation || (ref method) == NCreatureVisuals.MethodName.TryApplyLiquidOverlay || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003F62 RID: 16226 RVA: 0x001E5FE8 File Offset: 0x001E41E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCreatureVisuals.PropertyName.Bounds)
			{
				this.Bounds = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.IntentPosition)
			{
				this.IntentPosition = VariantUtils.ConvertTo<Marker2D>(ref value);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.OrbPosition)
			{
				this.OrbPosition = VariantUtils.ConvertTo<Marker2D>(ref value);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.TalkPosition)
			{
				this.TalkPosition = VariantUtils.ConvertTo<Marker2D>(ref value);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.VfxSpawnPosition)
			{
				this.VfxSpawnPosition = VariantUtils.ConvertTo<Marker2D>(ref value);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.DefaultScale)
			{
				this.DefaultScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._body)
			{
				this._body = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._phobiaModeBody)
			{
				this._phobiaModeBody = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._hue)
			{
				this._hue = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._liquidOverlayTimer)
			{
				this._liquidOverlayTimer = VariantUtils.ConvertTo<double>(ref value);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._savedNormalMaterial)
			{
				this._savedNormalMaterial = VariantUtils.ConvertTo<Material>(ref value);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._currentLiquidOverlayMaterial)
			{
				this._currentLiquidOverlayMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003F63 RID: 16227 RVA: 0x001E6144 File Offset: 0x001E4344
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCreatureVisuals.PropertyName.Bounds)
			{
				Control bounds = this.Bounds;
				value = VariantUtils.CreateFrom<Control>(ref bounds);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.IntentPosition)
			{
				Marker2D marker2D = this.IntentPosition;
				value = VariantUtils.CreateFrom<Marker2D>(ref marker2D);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.OrbPosition)
			{
				Marker2D marker2D = this.OrbPosition;
				value = VariantUtils.CreateFrom<Marker2D>(ref marker2D);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.TalkPosition)
			{
				Marker2D marker2D = this.TalkPosition;
				value = VariantUtils.CreateFrom<Marker2D>(ref marker2D);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.IsSpineNode)
			{
				bool flag = this.IsSpineNode;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.HasSpineAnimation)
			{
				bool flag = this.HasSpineAnimation;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.IsUsingPhobiaModeBody)
			{
				bool flag = this.IsUsingPhobiaModeBody;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.VfxSpawnPosition)
			{
				Marker2D marker2D = this.VfxSpawnPosition;
				value = VariantUtils.CreateFrom<Marker2D>(ref marker2D);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName.DefaultScale)
			{
				float defaultScale = this.DefaultScale;
				value = VariantUtils.CreateFrom<float>(ref defaultScale);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._body)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._body);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._phobiaModeBody)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._phobiaModeBody);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._hue)
			{
				value = VariantUtils.CreateFrom<float>(ref this._hue);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._liquidOverlayTimer)
			{
				value = VariantUtils.CreateFrom<double>(ref this._liquidOverlayTimer);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._savedNormalMaterial)
			{
				value = VariantUtils.CreateFrom<Material>(ref this._savedNormalMaterial);
				return true;
			}
			if ((ref name) == NCreatureVisuals.PropertyName._currentLiquidOverlayMaterial)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._currentLiquidOverlayMaterial);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003F64 RID: 16228 RVA: 0x001E6354 File Offset: 0x001E4554
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCreatureVisuals.PropertyName._body, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureVisuals.PropertyName._phobiaModeBody, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureVisuals.PropertyName.Bounds, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureVisuals.PropertyName.IntentPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureVisuals.PropertyName.OrbPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureVisuals.PropertyName.TalkPosition, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCreatureVisuals.PropertyName.IsSpineNode, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCreatureVisuals.PropertyName.HasSpineAnimation, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCreatureVisuals.PropertyName.IsUsingPhobiaModeBody, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureVisuals.PropertyName.VfxSpawnPosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCreatureVisuals.PropertyName.DefaultScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCreatureVisuals.PropertyName._hue, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCreatureVisuals.PropertyName._liquidOverlayTimer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureVisuals.PropertyName._savedNormalMaterial, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCreatureVisuals.PropertyName._currentLiquidOverlayMaterial, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003F65 RID: 16229 RVA: 0x001E6554 File Offset: 0x001E4754
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName bounds = NCreatureVisuals.PropertyName.Bounds;
			Control bounds2 = this.Bounds;
			info.AddProperty(bounds, Variant.From<Control>(ref bounds2));
			StringName intentPosition = NCreatureVisuals.PropertyName.IntentPosition;
			Marker2D marker2D = this.IntentPosition;
			info.AddProperty(intentPosition, Variant.From<Marker2D>(ref marker2D));
			StringName orbPosition = NCreatureVisuals.PropertyName.OrbPosition;
			marker2D = this.OrbPosition;
			info.AddProperty(orbPosition, Variant.From<Marker2D>(ref marker2D));
			StringName talkPosition = NCreatureVisuals.PropertyName.TalkPosition;
			marker2D = this.TalkPosition;
			info.AddProperty(talkPosition, Variant.From<Marker2D>(ref marker2D));
			StringName vfxSpawnPosition = NCreatureVisuals.PropertyName.VfxSpawnPosition;
			marker2D = this.VfxSpawnPosition;
			info.AddProperty(vfxSpawnPosition, Variant.From<Marker2D>(ref marker2D));
			StringName defaultScale = NCreatureVisuals.PropertyName.DefaultScale;
			float defaultScale2 = this.DefaultScale;
			info.AddProperty(defaultScale, Variant.From<float>(ref defaultScale2));
			info.AddProperty(NCreatureVisuals.PropertyName._body, Variant.From<Node2D>(ref this._body));
			info.AddProperty(NCreatureVisuals.PropertyName._phobiaModeBody, Variant.From<Node2D>(ref this._phobiaModeBody));
			info.AddProperty(NCreatureVisuals.PropertyName._hue, Variant.From<float>(ref this._hue));
			info.AddProperty(NCreatureVisuals.PropertyName._liquidOverlayTimer, Variant.From<double>(ref this._liquidOverlayTimer));
			info.AddProperty(NCreatureVisuals.PropertyName._savedNormalMaterial, Variant.From<Material>(ref this._savedNormalMaterial));
			info.AddProperty(NCreatureVisuals.PropertyName._currentLiquidOverlayMaterial, Variant.From<ShaderMaterial>(ref this._currentLiquidOverlayMaterial));
		}

		/// <inheritdoc />
		// Token: 0x06003F66 RID: 16230 RVA: 0x001E6684 File Offset: 0x001E4884
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName.Bounds, ref variant))
			{
				this.Bounds = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName.IntentPosition, ref variant2))
			{
				this.IntentPosition = variant2.As<Marker2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName.OrbPosition, ref variant3))
			{
				this.OrbPosition = variant3.As<Marker2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName.TalkPosition, ref variant4))
			{
				this.TalkPosition = variant4.As<Marker2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName.VfxSpawnPosition, ref variant5))
			{
				this.VfxSpawnPosition = variant5.As<Marker2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName.DefaultScale, ref variant6))
			{
				this.DefaultScale = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName._body, ref variant7))
			{
				this._body = variant7.As<Node2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName._phobiaModeBody, ref variant8))
			{
				this._phobiaModeBody = variant8.As<Node2D>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName._hue, ref variant9))
			{
				this._hue = variant9.As<float>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName._liquidOverlayTimer, ref variant10))
			{
				this._liquidOverlayTimer = variant10.As<double>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName._savedNormalMaterial, ref variant11))
			{
				this._savedNormalMaterial = variant11.As<Material>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCreatureVisuals.PropertyName._currentLiquidOverlayMaterial, ref variant12))
			{
				this._currentLiquidOverlayMaterial = variant12.As<ShaderMaterial>();
			}
		}

		// Token: 0x04001881 RID: 6273
		private static readonly StringName _overlayInfluence = new StringName("overlay_influence");

		// Token: 0x04001882 RID: 6274
		private static readonly StringName _h = new StringName("h");

		// Token: 0x04001883 RID: 6275
		private static readonly StringName _tint = new StringName("tint");

		// Token: 0x04001884 RID: 6276
		private const double _baseLiquidOverlayDuration = 1.0;

		// Token: 0x04001885 RID: 6277
		private Node2D _body;

		// Token: 0x04001886 RID: 6278
		[Nullable(2)]
		private Node2D _phobiaModeBody;

		// Token: 0x0400188E RID: 6286
		private float _hue = 1f;

		// Token: 0x0400188F RID: 6287
		private double _liquidOverlayTimer;

		// Token: 0x04001890 RID: 6288
		[Nullable(2)]
		private Material _savedNormalMaterial;

		// Token: 0x04001891 RID: 6289
		[Nullable(2)]
		private ShaderMaterial _currentLiquidOverlayMaterial;

		// Token: 0x04001892 RID: 6290
		private CancellationTokenSource _cts = new CancellationTokenSource();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018D2 RID: 6354
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'GetCurrentBody' method.
			/// </summary>
			// Token: 0x04006135 RID: 24885
			public static readonly StringName GetCurrentBody = "GetCurrentBody";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006136 RID: 24886
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04006137 RID: 24887
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006138 RID: 24888
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SetScaleAndHue' method.
			/// </summary>
			// Token: 0x04006139 RID: 24889
			public static readonly StringName SetScaleAndHue = "SetScaleAndHue";

			/// <summary>
			/// Cached name for the 'IsPlayingHurtAnimation' method.
			/// </summary>
			// Token: 0x0400613A RID: 24890
			public static readonly StringName IsPlayingHurtAnimation = "IsPlayingHurtAnimation";

			/// <summary>
			/// Cached name for the 'TryApplyLiquidOverlay' method.
			/// </summary>
			// Token: 0x0400613B RID: 24891
			public static readonly StringName TryApplyLiquidOverlay = "TryApplyLiquidOverlay";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018D3 RID: 6355
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Bounds' property.
			/// </summary>
			// Token: 0x0400613C RID: 24892
			public static readonly StringName Bounds = "Bounds";

			/// <summary>
			/// Cached name for the 'IntentPosition' property.
			/// </summary>
			// Token: 0x0400613D RID: 24893
			public static readonly StringName IntentPosition = "IntentPosition";

			/// <summary>
			/// Cached name for the 'OrbPosition' property.
			/// </summary>
			// Token: 0x0400613E RID: 24894
			public static readonly StringName OrbPosition = "OrbPosition";

			/// <summary>
			/// Cached name for the 'TalkPosition' property.
			/// </summary>
			// Token: 0x0400613F RID: 24895
			public static readonly StringName TalkPosition = "TalkPosition";

			/// <summary>
			/// Cached name for the 'IsSpineNode' property.
			/// </summary>
			// Token: 0x04006140 RID: 24896
			public static readonly StringName IsSpineNode = "IsSpineNode";

			/// <summary>
			/// Cached name for the 'HasSpineAnimation' property.
			/// </summary>
			// Token: 0x04006141 RID: 24897
			public static readonly StringName HasSpineAnimation = "HasSpineAnimation";

			/// <summary>
			/// Cached name for the 'IsUsingPhobiaModeBody' property.
			/// </summary>
			// Token: 0x04006142 RID: 24898
			public static readonly StringName IsUsingPhobiaModeBody = "IsUsingPhobiaModeBody";

			/// <summary>
			/// Cached name for the 'VfxSpawnPosition' property.
			/// </summary>
			// Token: 0x04006143 RID: 24899
			public static readonly StringName VfxSpawnPosition = "VfxSpawnPosition";

			/// <summary>
			/// Cached name for the 'DefaultScale' property.
			/// </summary>
			// Token: 0x04006144 RID: 24900
			public static readonly StringName DefaultScale = "DefaultScale";

			/// <summary>
			/// Cached name for the '_body' field.
			/// </summary>
			// Token: 0x04006145 RID: 24901
			public static readonly StringName _body = "_body";

			/// <summary>
			/// Cached name for the '_phobiaModeBody' field.
			/// </summary>
			// Token: 0x04006146 RID: 24902
			public static readonly StringName _phobiaModeBody = "_phobiaModeBody";

			/// <summary>
			/// Cached name for the '_hue' field.
			/// </summary>
			// Token: 0x04006147 RID: 24903
			public static readonly StringName _hue = "_hue";

			/// <summary>
			/// Cached name for the '_liquidOverlayTimer' field.
			/// </summary>
			// Token: 0x04006148 RID: 24904
			public static readonly StringName _liquidOverlayTimer = "_liquidOverlayTimer";

			/// <summary>
			/// Cached name for the '_savedNormalMaterial' field.
			/// </summary>
			// Token: 0x04006149 RID: 24905
			public static readonly StringName _savedNormalMaterial = "_savedNormalMaterial";

			/// <summary>
			/// Cached name for the '_currentLiquidOverlayMaterial' field.
			/// </summary>
			// Token: 0x0400614A RID: 24906
			public static readonly StringName _currentLiquidOverlayMaterial = "_currentLiquidOverlayMaterial";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018D4 RID: 6356
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
