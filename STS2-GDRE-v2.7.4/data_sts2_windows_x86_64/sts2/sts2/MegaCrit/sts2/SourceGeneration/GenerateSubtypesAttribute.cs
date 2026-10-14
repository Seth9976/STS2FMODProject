using System;
using System.Diagnostics.CodeAnalysis;

namespace MegaCrit.Sts2.SourceGeneration
{
	// Token: 0x02000021 RID: 33
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	internal sealed class GenerateSubtypesAttribute : Attribute
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000097BC File Offset: 0x000079BC
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000097C4 File Offset: 0x000079C4
		public DynamicallyAccessedMemberTypes DynamicallyAccessedMemberTypes { get; set; }
	}
}
