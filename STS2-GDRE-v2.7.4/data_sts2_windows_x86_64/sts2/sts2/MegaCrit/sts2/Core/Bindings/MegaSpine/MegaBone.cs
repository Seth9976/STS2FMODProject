using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineBone.
	/// </summary>
	// Token: 0x02000D5D RID: 3421
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaBone : MegaSpineBinding
	{
		// Token: 0x1700260F RID: 9743
		// (get) Token: 0x06008A58 RID: 35416 RVA: 0x002E47BA File Offset: 0x002E29BA
		protected override string SpineClassName
		{
			get
			{
				return "SpineBone";
			}
		}

		// Token: 0x17002610 RID: 9744
		// (get) Token: 0x06008A59 RID: 35417 RVA: 0x002E47C1 File Offset: 0x002E29C1
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "get_data", "set_rotation", "set_scale_x", "set_scale_y" });
			}
		}

		// Token: 0x06008A5A RID: 35418 RVA: 0x002E47EE File Offset: 0x002E29EE
		public MegaBone(Variant native)
			: base(native)
		{
		}

		// Token: 0x06008A5B RID: 35419 RVA: 0x002E47F7 File Offset: 0x002E29F7
		public MegaBoneData GetData()
		{
			return new MegaBoneData(base.Call("get_data", Array.Empty<Variant>()));
		}

		// Token: 0x06008A5C RID: 35420 RVA: 0x002E480E File Offset: 0x002E2A0E
		public void SetRotation(float rotation)
		{
			base.Call("set_rotation", new Variant[] { rotation });
		}

		// Token: 0x06008A5D RID: 35421 RVA: 0x002E482F File Offset: 0x002E2A2F
		public void SetScaleX(float scaleX)
		{
			base.Call("set_scale_x", new Variant[] { scaleX });
		}

		// Token: 0x06008A5E RID: 35422 RVA: 0x002E4850 File Offset: 0x002E2A50
		public void SetScaleY(float scaleY)
		{
			base.Call("set_scale_y", new Variant[] { scaleY });
		}

		// Token: 0x06008A5F RID: 35423 RVA: 0x002E4871 File Offset: 0x002E2A71
		public void Hide()
		{
			this.SetScaleX(0f);
			this.SetScaleY(0f);
		}
	}
}
