using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace GodotPlugins.Game
{
	// Token: 0x02000016 RID: 22
	internal static class Main
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00006704 File Offset: 0x00004904
		[UnmanagedCallersOnly(EntryPoint = "godotsharp_game_main_init")]
		private static godot_bool InitializeFromGameProject(IntPtr godotDllHandle, IntPtr outManagedCallbacks, IntPtr unmanagedCallbacks, int unmanagedCallbacksSize)
		{
			godot_bool godot_bool;
			try
			{
				DllImportResolver dllImportResolver = new DllImportResolver(new GodotDllImportResolver(godotDllHandle).OnResolveDllImport);
				Assembly assembly = typeof(GodotObject).Assembly;
				NativeLibrary.SetDllImportResolver(assembly, dllImportResolver);
				NativeFuncs.Initialize(unmanagedCallbacks, unmanagedCallbacksSize);
				ManagedCallbacks.Create(outManagedCallbacks);
				ScriptManagerBridge.LookupScriptsInAssembly(typeof(Main).Assembly);
				godot_bool = 1;
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex);
				godot_bool = GodotBoolExtensions.ToGodotBool(false);
			}
			return godot_bool;
		}
	}
}
