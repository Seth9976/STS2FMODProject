using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Collections;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineSkeletonDataResource.
	/// </summary>
	// Token: 0x02000D62 RID: 3426
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaSkeletonDataResource : MegaSpineBinding
	{
		// Token: 0x17002619 RID: 9753
		// (get) Token: 0x06008A75 RID: 35445 RVA: 0x002E4A66 File Offset: 0x002E2C66
		protected override string SpineClassName
		{
			get
			{
				return "SpineSkeletonDataResource";
			}
		}

		// Token: 0x1700261A RID: 9754
		// (get) Token: 0x06008A76 RID: 35446 RVA: 0x002E4A6D File Offset: 0x002E2C6D
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "find_animation", "find_skin", "get_animations", "get_skins" });
			}
		}

		// Token: 0x06008A77 RID: 35447 RVA: 0x002E4A9A File Offset: 0x002E2C9A
		public MegaSkeletonDataResource(Variant native)
			: base(native)
		{
		}

		// Token: 0x06008A78 RID: 35448 RVA: 0x002E4AA4 File Offset: 0x002E2CA4
		[return: Nullable(2)]
		public MegaSkin FindSkin(string skinName)
		{
			Variant variant = base.Call("find_skin", new Variant[] { skinName });
			if (variant.AsGodotObject() == null)
			{
				return null;
			}
			return new MegaSkin(variant);
		}

		/// <summary>
		/// Whether this skeleton defines an animation with the given name. Returns a bool rather than the
		/// animation so no transient <see cref="T:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaAnimation" /> wrapper escapes to callers, which would
		/// invite the GC-unsafe chain this binding is hardened against (PRG-6985).
		/// </summary>
		// Token: 0x06008A79 RID: 35449 RVA: 0x002E4AE4 File Offset: 0x002E2CE4
		public bool HasAnimation(string animName)
		{
			return base.Call("find_animation", new Variant[] { animName }).AsGodotObject() != null;
		}

		/// <summary>
		/// Names of every animation this skeleton defines. Returns the names rather than the wrappers so no
		/// transient <see cref="T:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaAnimation" /> escapes to callers, keeping the GC-unsafe read chain
		/// unrepresentable through this API (PRG-6985).
		/// </summary>
		// Token: 0x06008A7A RID: 35450 RVA: 0x002E4B1C File Offset: 0x002E2D1C
		public IReadOnlyList<string> GetAnimationNames()
		{
			Array<GodotObject> array = (Array<!0>)base.Call("get_animations", Array.Empty<Variant>());
			List<string> list = new List<string>(array.Count);
			foreach (GodotObject godotObject in array)
			{
				MegaAnimation megaAnimation = new MegaAnimation(godotObject);
				list.Add(megaAnimation.GetName());
			}
			return list;
		}

		// Token: 0x06008A7B RID: 35451 RVA: 0x002E4B9C File Offset: 0x002E2D9C
		public Array<GodotObject> GetSkins()
		{
			return (Array<!0>)base.Call("get_skins", Array.Empty<Variant>());
		}
	}
}
