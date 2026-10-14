using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MegaCrit.Sts2.Core.Nodes.Debug
{
	// Token: 0x020003CD RID: 973
	public static class IBootstrapSettingsSubtypes
	{
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06003AE2 RID: 15074 RVA: 0x001C1416 File Offset: 0x001BF616
		public static int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x001C1419 File Offset: 0x001BF619
		public static IReadOnlyList<Type> All
		{
			get
			{
				return IBootstrapSettingsSubtypes._subtypes;
			}
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x001C1420 File Offset: 0x001BF620
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
		[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		public static Type Get(int i)
		{
			return IBootstrapSettingsSubtypes._subtypes[i];
		}

		// Token: 0x040016F5 RID: 5877
		private static readonly Type[] _subtypes = Array.Empty<Type>();
	}
}
