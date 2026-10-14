using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineEvent.
	/// </summary>
	// Token: 0x02000D5F RID: 3423
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaEvent : MegaSpineBinding
	{
		// Token: 0x17002613 RID: 9747
		// (get) Token: 0x06008A64 RID: 35428 RVA: 0x002E48C6 File Offset: 0x002E2AC6
		protected override string SpineClassName
		{
			get
			{
				return "SpineEvent";
			}
		}

		// Token: 0x17002614 RID: 9748
		// (get) Token: 0x06008A65 RID: 35429 RVA: 0x002E48CD File Offset: 0x002E2ACD
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("get_data");
			}
		}

		// Token: 0x06008A66 RID: 35430 RVA: 0x002E48D9 File Offset: 0x002E2AD9
		public MegaEvent(Variant native)
			: base(native)
		{
		}

		// Token: 0x06008A67 RID: 35431 RVA: 0x002E48E2 File Offset: 0x002E2AE2
		public MegaEventData GetData()
		{
			return new MegaEventData(base.Call("get_data", Array.Empty<Variant>()));
		}
	}
}
