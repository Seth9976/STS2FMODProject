using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineBoneData.
	/// </summary>
	// Token: 0x02000D5E RID: 3422
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaBoneData : MegaSpineBinding
	{
		// Token: 0x17002611 RID: 9745
		// (get) Token: 0x06008A60 RID: 35424 RVA: 0x002E4889 File Offset: 0x002E2A89
		protected override string SpineClassName
		{
			get
			{
				return "SpineBoneData";
			}
		}

		// Token: 0x17002612 RID: 9746
		// (get) Token: 0x06008A61 RID: 35425 RVA: 0x002E4890 File Offset: 0x002E2A90
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("set_color");
			}
		}

		// Token: 0x06008A62 RID: 35426 RVA: 0x002E489C File Offset: 0x002E2A9C
		public MegaBoneData(Variant native)
			: base(native)
		{
		}

		// Token: 0x06008A63 RID: 35427 RVA: 0x002E48A5 File Offset: 0x002E2AA5
		public void SetColor(Color color)
		{
			base.Call("set_color", new Variant[] { color });
		}
	}
}
