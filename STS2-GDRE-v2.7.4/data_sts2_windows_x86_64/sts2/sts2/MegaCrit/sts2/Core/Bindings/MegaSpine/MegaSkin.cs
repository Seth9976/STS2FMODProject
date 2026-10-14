using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineSkin.
	/// </summary>
	// Token: 0x02000D63 RID: 3427
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaSkin : MegaSpineBinding
	{
		// Token: 0x1700261B RID: 9755
		// (get) Token: 0x06008A7C RID: 35452 RVA: 0x002E4BB3 File Offset: 0x002E2DB3
		protected override string SpineClassName
		{
			get
			{
				return "SpineSkin";
			}
		}

		// Token: 0x1700261C RID: 9756
		// (get) Token: 0x06008A7D RID: 35453 RVA: 0x002E4BBA File Offset: 0x002E2DBA
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("add_skin");
			}
		}

		// Token: 0x06008A7E RID: 35454 RVA: 0x002E4BC6 File Offset: 0x002E2DC6
		public MegaSkin(Variant native)
			: base(native)
		{
		}

		// Token: 0x06008A7F RID: 35455 RVA: 0x002E4BCF File Offset: 0x002E2DCF
		[NullableContext(2)]
		public void AddSkin(MegaSkin skin)
		{
			if (skin == null)
			{
				return;
			}
			base.Call("add_skin", new Variant[] { skin.BoundObject });
		}
	}
}
