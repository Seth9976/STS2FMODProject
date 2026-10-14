using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Modding
{
	/// <summary>
	/// Declares a class as the main entry point for the mod.
	/// If this is present, then upon loading the mod, we'll call the method named initializerMethod within the class.
	/// Otherwise, we'll create a harmony instance for the mod and call Harmony.PatchAll.
	/// </summary>
	// Token: 0x02000B95 RID: 2965
	[NullableContext(1)]
	[Nullable(0)]
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class ModInitializerAttribute : Attribute
	{
		// Token: 0x06007BC9 RID: 31689 RVA: 0x002A161D File Offset: 0x0029F81D
		public ModInitializerAttribute(string initializerMethod)
		{
			this.initializerMethod = initializerMethod;
		}

		// Token: 0x04002923 RID: 10531
		public string initializerMethod;
	}
}
