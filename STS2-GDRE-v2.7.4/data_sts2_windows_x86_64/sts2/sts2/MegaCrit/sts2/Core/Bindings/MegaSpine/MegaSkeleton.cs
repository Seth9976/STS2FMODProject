using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineSkeleton.
	/// </summary>
	// Token: 0x02000D61 RID: 3425
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaSkeleton : MegaSpineBinding
	{
		// Token: 0x17002617 RID: 9751
		// (get) Token: 0x06008A6C RID: 35436 RVA: 0x002E493D File Offset: 0x002E2B3D
		protected override string SpineClassName
		{
			get
			{
				return "SpineSkeleton";
			}
		}

		// Token: 0x17002618 RID: 9752
		// (get) Token: 0x06008A6D RID: 35437 RVA: 0x002E4944 File Offset: 0x002E2B44
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "find_bone", "get_bounds", "get_data", "set_skin", "set_skin_by_name", "set_slots_to_setup_pose" });
			}
		}

		// Token: 0x06008A6E RID: 35438 RVA: 0x002E4981 File Offset: 0x002E2B81
		public MegaSkeleton(Variant native)
			: base(native)
		{
		}

		// Token: 0x06008A6F RID: 35439 RVA: 0x002E498C File Offset: 0x002E2B8C
		[return: Nullable(2)]
		public MegaBone FindBone(string boneName)
		{
			Variant variant = base.Call("find_bone", new Variant[] { boneName });
			if (variant.AsGodotObject() == null)
			{
				return null;
			}
			return new MegaBone(variant);
		}

		// Token: 0x06008A70 RID: 35440 RVA: 0x002E49CC File Offset: 0x002E2BCC
		public Rect2 GetBounds()
		{
			return base.Call("get_bounds", Array.Empty<Variant>()).As<Rect2>();
		}

		// Token: 0x06008A71 RID: 35441 RVA: 0x002E49F1 File Offset: 0x002E2BF1
		public MegaSkeletonDataResource GetData()
		{
			return new MegaSkeletonDataResource(base.Call("get_data", Array.Empty<Variant>()));
		}

		// Token: 0x06008A72 RID: 35442 RVA: 0x002E4A08 File Offset: 0x002E2C08
		[NullableContext(2)]
		public void SetSkin(MegaSkin skin)
		{
			if (skin == null)
			{
				return;
			}
			base.Call("set_skin", new Variant[] { skin.BoundObject });
		}

		// Token: 0x06008A73 RID: 35443 RVA: 0x002E4A32 File Offset: 0x002E2C32
		public void SetSkinByName(string skinName)
		{
			base.Call("set_skin_by_name", new Variant[] { skinName });
		}

		// Token: 0x06008A74 RID: 35444 RVA: 0x002E4A53 File Offset: 0x002E2C53
		public void SetSlotsToSetupPose()
		{
			base.Call("set_slots_to_setup_pose", Array.Empty<Variant>());
		}
	}
}
