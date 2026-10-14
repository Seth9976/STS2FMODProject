using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Nodes.Debug
{
	// Token: 0x020003CB RID: 971
	public static class BootstrapSettingsUtil
	{
		// Token: 0x06003AD2 RID: 15058 RVA: 0x001C1374 File Offset: 0x001BF574
		[NullableContext(2)]
		[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		public static Type Get()
		{
			if (IBootstrapSettingsSubtypes.Count > 0)
			{
				return IBootstrapSettingsSubtypes.Get(0);
			}
			return null;
		}
	}
}
