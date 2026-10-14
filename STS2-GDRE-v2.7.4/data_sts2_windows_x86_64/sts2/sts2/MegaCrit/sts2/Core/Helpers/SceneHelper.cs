using System;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C12 RID: 3090
	[NullableContext(1)]
	[Nullable(0)]
	public static class SceneHelper
	{
		/// <summary>
		/// Get the path to the scene with the specified "inner" path.
		/// </summary>
		/// <param name="innerPath">The scene's inner path. For example, for "scenes/ui/button.tscn", pass "ui/button".</param>
		/// <returns>The full path to the scene.</returns>
		// Token: 0x06007FDD RID: 32733 RVA: 0x002B6900 File Offset: 0x002B4B00
		public static string GetScenePath(string innerPath)
		{
			if (innerPath.StartsWith('/'))
			{
				string text = innerPath;
				innerPath = text.Substring(1, text.Length - 1);
			}
			return "res://scenes/" + innerPath + ".tscn";
		}

		/// <summary>
		/// Load the packed scene with the specified "inner" path.
		/// </summary>
		/// <param name="innerPath">The scene's inner path. For example, for "scenes/ui/button.tscn", pass "ui/button".</param>
		/// <returns>The PackedScene.</returns>
		// Token: 0x06007FDE RID: 32734 RVA: 0x002B693C File Offset: 0x002B4B3C
		private static PackedScene Load(string innerPath)
		{
			string scenePath = SceneHelper.GetScenePath(innerPath);
			return ResourceLoader.Load<PackedScene>(scenePath, null, 1L);
		}

		/// <summary>
		/// Instantiate the scene with the specified "inner" path.
		/// </summary>
		/// <param name="innerPath">The scene's inner path. For example, for "scenes/ui/button.tscn", pass "ui/button".</param>
		/// <typeparam name="T">The type to cast to. Should be a descendant of <see cref="T:Godot.Node" />.</typeparam>
		/// <returns>The instantiated scene.</returns>
		// Token: 0x06007FDF RID: 32735 RVA: 0x002B6959 File Offset: 0x002B4B59
		public static T Instantiate<[Nullable(0)] T>(string innerPath) where T : Node
		{
			return SceneHelper.Load(innerPath).Instantiate<T>(0L);
		}
	}
}
