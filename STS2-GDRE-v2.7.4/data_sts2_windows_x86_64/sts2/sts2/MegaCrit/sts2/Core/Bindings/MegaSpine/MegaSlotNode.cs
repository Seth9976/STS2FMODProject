using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineSlotNode.
	/// </summary>
	// Token: 0x02000D64 RID: 3428
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaSlotNode : MegaSpineBinding
	{
		// Token: 0x1700261D RID: 9757
		// (get) Token: 0x06008A80 RID: 35456 RVA: 0x002E4BF9 File Offset: 0x002E2DF9
		protected override string SpineClassName
		{
			get
			{
				return "SpineSlotNode";
			}
		}

		// Token: 0x1700261E RID: 9758
		// (get) Token: 0x06008A81 RID: 35457 RVA: 0x002E4C00 File Offset: 0x002E2E00
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("get_normal_material");
			}
		}

		// Token: 0x06008A82 RID: 35458 RVA: 0x002E4C0C File Offset: 0x002E2E0C
		public MegaSlotNode(Variant native)
			: base(native)
		{
		}

		// Token: 0x06008A83 RID: 35459 RVA: 0x002E4C18 File Offset: 0x002E2E18
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
	}
}
