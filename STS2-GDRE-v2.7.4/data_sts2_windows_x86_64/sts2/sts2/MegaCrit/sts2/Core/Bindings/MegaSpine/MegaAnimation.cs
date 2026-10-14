using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineAnimation.
	/// </summary>
	// Token: 0x02000D5B RID: 3419
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaAnimation : MegaSpineBinding
	{
		// Token: 0x1700260B RID: 9739
		// (get) Token: 0x06008A46 RID: 35398 RVA: 0x002E43C2 File Offset: 0x002E25C2
		protected override string SpineClassName
		{
			get
			{
				return "SpineAnimation";
			}
		}

		// Token: 0x1700260C RID: 9740
		// (get) Token: 0x06008A47 RID: 35399 RVA: 0x002E43C9 File Offset: 0x002E25C9
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "get_name", "get_duration" });
			}
		}

		// Token: 0x06008A48 RID: 35400 RVA: 0x002E43E6 File Offset: 0x002E25E6
		public MegaAnimation(Variant native)
			: base(native)
		{
		}

		// Token: 0x06008A49 RID: 35401 RVA: 0x002E43F0 File Offset: 0x002E25F0
		public string GetName()
		{
			return base.Call("get_name", Array.Empty<Variant>()).AsString();
		}

		// Token: 0x06008A4A RID: 35402 RVA: 0x002E4418 File Offset: 0x002E2618
		public float GetDuration()
		{
			return base.Call("get_duration", Array.Empty<Variant>()).AsSingle();
		}
	}
}
