using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Modding
{
	// Token: 0x02000B8E RID: 2958
	[NullableContext(2)]
	[Nullable(0)]
	public class AssemblyInfo
	{
		/// <summary>
		/// Helper dictionary mapping assemblies to the mods they represent.
		/// </summary>
		// Token: 0x17002254 RID: 8788
		// (get) Token: 0x06007BA8 RID: 31656 RVA: 0x002A1201 File Offset: 0x0029F401
		// (set) Token: 0x06007BA9 RID: 31657 RVA: 0x002A1208 File Offset: 0x0029F408
		[Nullable(new byte[] { 2, 1, 1 })]
		public static Dictionary<Assembly, Mod> ModMap
		{
			[return: Nullable(new byte[] { 2, 1, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 1 })]
			private set;
		}

		/// <summary>
		/// The base game assembly.
		/// </summary>
		// Token: 0x17002255 RID: 8789
		// (get) Token: 0x06007BAA RID: 31658 RVA: 0x002A1210 File Offset: 0x0029F410
		// (set) Token: 0x06007BAB RID: 31659 RVA: 0x002A1217 File Offset: 0x0029F417
		public static Assembly BaseGame { get; private set; }

		// Token: 0x17002256 RID: 8790
		// (get) Token: 0x06007BAC RID: 31660 RVA: 0x002A121F File Offset: 0x0029F41F
		// (set) Token: 0x06007BAD RID: 31661 RVA: 0x002A1226 File Offset: 0x0029F426
		[Nullable(new byte[] { 2, 1, 0, 2 })]
		public static Dictionary<Type, ValueTuple<Mod, bool>> MockTypes
		{
			[return: Nullable(new byte[] { 2, 1, 0, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 0, 2 })]
			set;
		}

		// Token: 0x06007BAE RID: 31662 RVA: 0x002A1230 File Offset: 0x0029F430
		public static void Init()
		{
			AssemblyInfo.BaseGame = Assembly.GetExecutingAssembly();
			AssemblyInfo.ModMap = new Dictionary<Assembly, Mod>();
			foreach (Mod mod in ModManager.Mods)
			{
				if (mod.state == ModLoadState.Loaded)
				{
					foreach (Assembly assembly in mod.assemblies)
					{
						AssemblyInfo.ModMap[assembly] = mod;
					}
				}
			}
		}

		// Token: 0x06007BAF RID: 31663 RVA: 0x002A12DC File Offset: 0x0029F4DC
		[NullableContext(1)]
		[return: Nullable(2)]
		public static Mod ModForType(Type type, out bool isBaseGame)
		{
			ValueTuple<Mod, bool> valueTuple;
			if (AssemblyInfo.MockTypes != null && AssemblyInfo.MockTypes.TryGetValue(type, out valueTuple))
			{
				isBaseGame = valueTuple.Item2;
				return valueTuple.Item1;
			}
			if (AssemblyInfo.ModMap == null)
			{
				throw new InvalidOperationException();
			}
			Assembly assembly = type.Assembly;
			Mod mod;
			AssemblyInfo.ModMap.TryGetValue(assembly, out mod);
			isBaseGame = AssemblyInfo.BaseGame == assembly;
			return mod;
		}

		/// <summary>
		/// This should only be called by tests.
		/// Typically, AssemblyInfo is initialized once at the start of the game and never cleared.
		/// </summary>
		// Token: 0x06007BB0 RID: 31664 RVA: 0x002A133E File Offset: 0x0029F53E
		public static void ClearForTests()
		{
			AssemblyInfo.BaseGame = null;
			AssemblyInfo.ModMap = null;
		}
	}
}
