using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineSprite.
	/// </summary>
	// Token: 0x02000D66 RID: 3430
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaSprite : MegaSpineBinding
	{
		// Token: 0x17002623 RID: 9763
		// (get) Token: 0x06008A90 RID: 35472 RVA: 0x002E4F54 File Offset: 0x002E3154
		protected override string SpineClassName
		{
			get
			{
				return "SpineSprite";
			}
		}

		// Token: 0x17002624 RID: 9764
		// (get) Token: 0x06008A91 RID: 35473 RVA: 0x002E4F5C File Offset: 0x002E315C
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "get_animation_state", "get_additive_material", "get_normal_material", "get_skeleton", "new_skin", "set_normal_material", "set_skeleton_data_res" });
			}
		}

		// Token: 0x17002625 RID: 9765
		// (get) Token: 0x06008A92 RID: 35474 RVA: 0x002E4FAC File Offset: 0x002E31AC
		protected override IEnumerable<string> SpineSignals
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "animation_started", "animation_interrupted", "animation_ended", "animation_completed", "animation_disposed", "animation_event", "before_animation_state_update", "before_animation_state_apply", "before_world_transforms_change", "world_transforms_changed" });
			}
		}

		// Token: 0x06008A93 RID: 35475 RVA: 0x002E5016 File Offset: 0x002E3216
		public MegaSprite(Variant native)
			: base(native)
		{
		}

		// Token: 0x06008A94 RID: 35476 RVA: 0x002E501F File Offset: 0x002E321F
		public Error ConnectAnimationStarted(Callable callable)
		{
			return base.Connect("animation_started", callable);
		}

		// Token: 0x06008A95 RID: 35477 RVA: 0x002E502D File Offset: 0x002E322D
		public Error ConnectAnimationInterrupted(Callable callable)
		{
			return base.Connect("animation_interrupted", callable);
		}

		// Token: 0x06008A96 RID: 35478 RVA: 0x002E503B File Offset: 0x002E323B
		public Error ConnectAnimationEnded(Callable callable)
		{
			return base.Connect("animation_ended", callable);
		}

		// Token: 0x06008A97 RID: 35479 RVA: 0x002E5049 File Offset: 0x002E3249
		public Error ConnectAnimationCompleted(Callable callable)
		{
			return base.Connect("animation_completed", callable);
		}

		// Token: 0x06008A98 RID: 35480 RVA: 0x002E5057 File Offset: 0x002E3257
		public Error ConnectAnimationDisposed(Callable callable)
		{
			return base.Connect("animation_disposed", callable);
		}

		// Token: 0x06008A99 RID: 35481 RVA: 0x002E5065 File Offset: 0x002E3265
		public Error ConnectAnimationEvent(Callable callable)
		{
			return base.Connect("animation_event", callable);
		}

		// Token: 0x06008A9A RID: 35482 RVA: 0x002E5073 File Offset: 0x002E3273
		public Error ConnectBeforeAnimationStateUpdate(Callable callable)
		{
			return base.Connect("before_animation_state_update", callable);
		}

		// Token: 0x06008A9B RID: 35483 RVA: 0x002E5081 File Offset: 0x002E3281
		public Error ConnectBeforeAnimationStateApply(Callable callable)
		{
			return base.Connect("before_animation_state_apply", callable);
		}

		// Token: 0x06008A9C RID: 35484 RVA: 0x002E508F File Offset: 0x002E328F
		public Error ConnectBeforeWorldTransformsChange(Callable callable)
		{
			return base.Connect("before_world_transforms_change", callable);
		}

		// Token: 0x06008A9D RID: 35485 RVA: 0x002E509D File Offset: 0x002E329D
		public Error ConnectWorldTransformsChanged(Callable callable)
		{
			return base.Connect("world_transforms_changed", callable);
		}

		// Token: 0x06008A9E RID: 35486 RVA: 0x002E50AB File Offset: 0x002E32AB
		public void DisconnectAnimationStarted(Callable callable)
		{
			base.Disconnect("animation_started", callable);
		}

		// Token: 0x06008A9F RID: 35487 RVA: 0x002E50B9 File Offset: 0x002E32B9
		public void DisconnectAnimationInterrupted(Callable callable)
		{
			base.Disconnect("animation_interrupted", callable);
		}

		// Token: 0x06008AA0 RID: 35488 RVA: 0x002E50C7 File Offset: 0x002E32C7
		public void DisconnectAnimationEnded(Callable callable)
		{
			base.Disconnect("animation_ended", callable);
		}

		// Token: 0x06008AA1 RID: 35489 RVA: 0x002E50D5 File Offset: 0x002E32D5
		public void DisconnectAnimationCompleted(Callable callable)
		{
			base.Disconnect("animation_completed", callable);
		}

		// Token: 0x06008AA2 RID: 35490 RVA: 0x002E50E3 File Offset: 0x002E32E3
		public void DisconnectAnimationDisposed(Callable callable)
		{
			base.Disconnect("animation_disposed", callable);
		}

		// Token: 0x06008AA3 RID: 35491 RVA: 0x002E50F1 File Offset: 0x002E32F1
		public void DisconnectAnimationEvent(Callable callable)
		{
			base.Disconnect("animation_event", callable);
		}

		// Token: 0x06008AA4 RID: 35492 RVA: 0x002E50FF File Offset: 0x002E32FF
		public void DisconnectBeforeAnimationStateUpdate(Callable callable)
		{
			base.Disconnect("before_animation_state_update", callable);
		}

		// Token: 0x06008AA5 RID: 35493 RVA: 0x002E510D File Offset: 0x002E330D
		public void DisconnectBeforeAnimationStateApply(Callable callable)
		{
			base.Disconnect("before_animation_state_apply", callable);
		}

		// Token: 0x06008AA6 RID: 35494 RVA: 0x002E511B File Offset: 0x002E331B
		public void DisconnectBeforeWorldTransformsChange(Callable callable)
		{
			base.Disconnect("before_world_transforms_change", callable);
		}

		// Token: 0x06008AA7 RID: 35495 RVA: 0x002E5129 File Offset: 0x002E3329
		public void DisconnectWorldTransformsChanged(Callable callable)
		{
			base.Disconnect("world_transforms_changed", callable);
		}

		/// <summary>
		/// Helper method for the lazy
		/// </summary>
		// Token: 0x06008AA8 RID: 35496 RVA: 0x002E5137 File Offset: 0x002E3337
		public bool HasAnimation(string animId)
		{
			MegaSkeleton skeleton = this.GetSkeleton();
			return skeleton != null && skeleton.GetData().HasAnimation(animId);
		}

		/// <summary>
		/// Returns the animation state, throwing a descriptive error if it does not exist yet. Prefer
		/// <see cref="M:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaSprite.TryGetAnimationState" /> / <see cref="M:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaSprite.IsAnimationStateReady" /> in code that can run
		/// before the SpineSprite is ready (e.g. a child node's _Ready, which Godot runs before the parent
		/// SpineSprite has built its skeleton). Failing fast here turns the old opaque NullReferenceException
		/// (from driving a null-wrapped state) into a clear, actionable message.
		/// </summary>
		// Token: 0x06008AA9 RID: 35497 RVA: 0x002E5150 File Offset: 0x002E3350
		public MegaAnimationState GetAnimationState()
		{
			MegaAnimationState megaAnimationState = this.TryGetAnimationState();
			if (megaAnimationState == null)
			{
				throw new InvalidOperationException("GetAnimationState() was called before the SpineSprite's skeleton finished initializing. Godot runs _Ready() bottom-up and the skeleton loads asynchronously; drive animations from a spine-ready callback (Node.RunWhenSpineReady) or gate on IsAnimationStateReady()/TryGetAnimationState().");
			}
			return megaAnimationState;
		}

		/// <summary>
		/// Returns the animation state, or null if it has not been created yet. The skeleton (and with it
		/// the animation state) may not exist when this is called before the SpineSprite has finished
		/// initializing, which happens when a child node's _Ready() runs first: Godot invokes _Ready()
		/// bottom-up (children before parents). Prefer this over <see cref="M:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaSprite.GetAnimationState" /> in code
		/// that can run during that window, since GetAnimationState() would wrap a null object and throw
		/// on first use.
		/// </summary>
		// Token: 0x06008AAA RID: 35498 RVA: 0x002E5168 File Offset: 0x002E3368
		[NullableContext(2)]
		public MegaAnimationState TryGetAnimationState()
		{
			Variant variant = base.Call("get_animation_state", Array.Empty<Variant>());
			if (variant.VariantType != 24L || variant.AsGodotObject() == null)
			{
				return null;
			}
			return new MegaAnimationState(variant);
		}

		/// <summary>
		/// True once the SpineSprite has finished initializing and it is safe to drive its animations.
		/// This gates on the skeleton existing (get_skeleton returns null until the skeleton data has
		/// loaded, the same null-skeleton condition NCreatureVisuals checks to detect a failed load) and
		/// additionally confirms the animation state object we drive exists. It returns false during the
		/// window where a child node's _Ready() runs before the parent SpineSprite is ready, because Godot
		/// invokes _Ready() bottom-up and the skeleton loads asynchronously.
		/// </summary>
		// Token: 0x06008AAB RID: 35499 RVA: 0x002E51A3 File Offset: 0x002E33A3
		public bool IsAnimationStateReady()
		{
			return this.GetSkeleton() != null && this.TryGetAnimationState() != null;
		}

		// Token: 0x06008AAC RID: 35500 RVA: 0x002E51B8 File Offset: 0x002E33B8
		[NullableContext(2)]
		public MegaSkeleton GetSkeleton()
		{
			Variant? variant = base.CallNullable("get_skeleton", Array.Empty<Variant>());
			if (variant == null)
			{
				return null;
			}
			return new MegaSkeleton(variant.Value);
		}

		// Token: 0x06008AAD RID: 35501 RVA: 0x002E51F0 File Offset: 0x002E33F0
		[NullableContext(2)]
		public Material GetAdditiveMaterial()
		{
			if (base.CallNullable("get_additive_material", Array.Empty<Variant>()) == null)
			{
				return null;
			}
			Variant? variant;
			return variant.GetValueOrDefault().As<Material>();
		}

		// Token: 0x06008AAE RID: 35502 RVA: 0x002E5228 File Offset: 0x002E3428
		[NullableContext(2)]
		public Material GetNormalMaterial()
		{
			if (base.CallNullable("get_normal_material", Array.Empty<Variant>()) == null)
			{
				return null;
			}
			Variant? variant;
			return variant.GetValueOrDefault().As<Material>();
		}

		// Token: 0x06008AAF RID: 35503 RVA: 0x002E5260 File Offset: 0x002E3460
		public MegaSkin NewSkin(string name)
		{
			return new MegaSkin(base.Call("new_skin", new Variant[] { name }));
		}

		// Token: 0x06008AB0 RID: 35504 RVA: 0x002E5285 File Offset: 0x002E3485
		public void SetNormalMaterial(Material material)
		{
			base.Call("set_normal_material", new Variant[] { material });
		}

		// Token: 0x06008AB1 RID: 35505 RVA: 0x002E52A6 File Offset: 0x002E34A6
		public void SetSkeletonDataRes(MegaSkeletonDataResource skeletonData)
		{
			base.Call("set_skeleton_data_res", new Variant[] { skeletonData.BoundObject });
		}

		// Token: 0x0400301C RID: 12316
		public const string spineClassName = "SpineSprite";
	}
}
