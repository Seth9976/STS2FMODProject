using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineEventData.
	/// </summary>
	// Token: 0x02000D60 RID: 3424
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaEventData : MegaSpineBinding
	{
		// Token: 0x17002615 RID: 9749
		// (get) Token: 0x06008A68 RID: 35432 RVA: 0x002E48F9 File Offset: 0x002E2AF9
		protected override string SpineClassName
		{
			get
			{
				return "SpineEventData";
			}
		}

		// Token: 0x17002616 RID: 9750
		// (get) Token: 0x06008A69 RID: 35433 RVA: 0x002E4900 File Offset: 0x002E2B00
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("get_event_name");
			}
		}

		// Token: 0x06008A6A RID: 35434 RVA: 0x002E490C File Offset: 0x002E2B0C
		public MegaEventData(Variant native)
			: base(native)
		{
		}

		// Token: 0x06008A6B RID: 35435 RVA: 0x002E4918 File Offset: 0x002E2B18
		public string GetEventName()
		{
			return base.Call("get_event_name", Array.Empty<Variant>()).AsString();
		}
	}
}
