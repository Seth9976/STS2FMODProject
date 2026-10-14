using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Modding;

namespace MegaCrit.Sts2.Core.Helpers
{
	/// <summary>
	/// A few utility methods for accessing types via reflection.
	/// In general, you should avoid using this, as it does not work on console platforms (or anywhere that NativeAOT is
	/// required). You can check for support with SubtypesAvailable.
	/// Only use this in scenarios where it is required (e.g. for modding).
	/// </summary>
	// Token: 0x02000C11 RID: 3089
	[NullableContext(1)]
	[Nullable(0)]
	public static class ReflectionHelper
	{
		// Token: 0x17002309 RID: 8969
		// (get) Token: 0x06007FD3 RID: 32723 RVA: 0x002B67C0 File Offset: 0x002B49C0
		public static bool SubtypesAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700230A RID: 8970
		// (get) Token: 0x06007FD4 RID: 32724 RVA: 0x002B67C3 File Offset: 0x002B49C3
		public static Type[] AllTypes
		{
			get
			{
				if (ReflectionHelper._allTypes == null)
				{
					ReflectionHelper._allTypes = Assembly.GetAssembly(typeof(ReflectionHelper)).GetTypes();
				}
				return ReflectionHelper._allTypes;
			}
		}

		// Token: 0x1700230B RID: 8971
		// (get) Token: 0x06007FD5 RID: 32725 RVA: 0x002B67EC File Offset: 0x002B49EC
		public static Type[] ModTypes
		{
			get
			{
				if (ModManager.State == ModManagerState.None)
				{
					throw new InvalidOperationException("ModManager is not finished initializing! ReflectionHelper.ModTypes cannot be called before that.");
				}
				if (ReflectionHelper._modTypes == null)
				{
					ReflectionHelper._modTypes = ModManager.GetLoadedMods().SelectMany<Mod, Assembly>((Mod m) => m.assemblies).SelectMany<Assembly, Type>((Assembly a) => a.GetTypes())
						.ToArray<Type>();
				}
				return ReflectionHelper._modTypes;
			}
		}

		// Token: 0x06007FD6 RID: 32726 RVA: 0x002B686E File Offset: 0x002B4A6E
		public static IEnumerable<Type> GetSubtypes(Type parentType)
		{
			return ReflectionHelper.GetSubtypesFromList(ReflectionHelper.AllTypes, parentType);
		}

		// Token: 0x06007FD7 RID: 32727 RVA: 0x002B687B File Offset: 0x002B4A7B
		public static IEnumerable<Type> GetSubtypesInMods(Type parentType)
		{
			return ReflectionHelper.GetSubtypesFromList(ReflectionHelper.ModTypes, parentType);
		}

		// Token: 0x06007FD8 RID: 32728 RVA: 0x002B6888 File Offset: 0x002B4A88
		public static IEnumerable<Type> GetSubtypesFromAssembly(Assembly assembly, Type parentType)
		{
			return ReflectionHelper.GetSubtypesFromList(assembly.GetTypes(), parentType);
		}

		// Token: 0x06007FD9 RID: 32729 RVA: 0x002B6898 File Offset: 0x002B4A98
		private static IEnumerable<Type> GetSubtypesFromList(IList<Type> list, Type parentType)
		{
			return list.Where<Type>((Type type) => type != null && !type.IsAbstract && !type.IsInterface && ReflectionHelper.InheritsOrImplements(type, parentType));
		}

		// Token: 0x06007FDA RID: 32730 RVA: 0x002B68C4 File Offset: 0x002B4AC4
		public static IEnumerable<Type> GetSubtypes<T>() where T : class
		{
			return ReflectionHelper.GetSubtypes(typeof(T));
		}

		// Token: 0x06007FDB RID: 32731 RVA: 0x002B68D5 File Offset: 0x002B4AD5
		public static IEnumerable<Type> GetSubtypesInMods<T>() where T : class
		{
			return ReflectionHelper.GetSubtypesInMods(typeof(T));
		}

		// Token: 0x06007FDC RID: 32732 RVA: 0x002B68E6 File Offset: 0x002B4AE6
		public static bool InheritsOrImplements([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type derived, Type baseType)
		{
			return derived.IsSubclassOf(baseType) || derived.GetInterfaces().Contains(baseType);
		}

		// Token: 0x04002A90 RID: 10896
		public const BindingFlags allAccessLevels = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x04002A91 RID: 10897
		[Nullable(new byte[] { 2, 1 })]
		private static Type[] _allTypes;

		// Token: 0x04002A92 RID: 10898
		[Nullable(new byte[] { 2, 1 })]
		private static Type[] _modTypes;
	}
}
