using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.AutoSlay.Helpers
{
	/// <summary>
	/// UI interaction utilities for AutoSlay.
	/// </summary>
	// Token: 0x02000D6F RID: 3439
	[NullableContext(1)]
	[Nullable(0)]
	public static class UiHelper
	{
		/// <summary>
		/// Clicks a clickable control by directly emitting the Released signal.
		/// This bypasses hover/focus/pause checks that can fail in headless/automated testing.
		/// </summary>
		// Token: 0x06008AFC RID: 35580 RVA: 0x002E692C File Offset: 0x002E4B2C
		public static async Task Click(NClickableControl button, int delayMs = 100)
		{
			button.ForceClick();
			await Task.Delay(delayMs);
		}

		/// <summary>
		/// Finds all nodes of a specific type under a parent node recursively.
		/// </summary>
		// Token: 0x06008AFD RID: 35581 RVA: 0x002E6978 File Offset: 0x002E4B78
		public static List<T> FindAll<[Nullable(0)] T>(Node start) where T : Node
		{
			List<T> list = new List<T>();
			if (GodotObject.IsInstanceValid(start))
			{
				UiHelper.FindAllRecursive<T>(start, list);
			}
			return list;
		}

		// Token: 0x06008AFE RID: 35582 RVA: 0x002E699C File Offset: 0x002E4B9C
		private static void FindAllRecursive<[Nullable(0)] T>(Node node, List<T> found) where T : Node
		{
			if (!GodotObject.IsInstanceValid(node))
			{
				return;
			}
			T t = node as T;
			if (t != null)
			{
				found.Add(t);
			}
			foreach (Node node2 in node.GetChildren(false))
			{
				UiHelper.FindAllRecursive<T>(node2, found);
			}
		}

		/// <summary>
		/// Finds the first node of a specific type under a parent node.
		/// </summary>
		// Token: 0x06008AFF RID: 35583 RVA: 0x002E6A10 File Offset: 0x002E4C10
		[return: Nullable(2)]
		public static T FindFirst<[Nullable(0)] T>(Node start) where T : Node
		{
			if (!GodotObject.IsInstanceValid(start))
			{
				return default(T);
			}
			T t = start as T;
			if (t != null)
			{
				return t;
			}
			foreach (Node node in start.GetChildren(false))
			{
				T t2 = UiHelper.FindFirst<T>(node);
				if (t2 != null)
				{
					return t2;
				}
			}
			return default(T);
		}
	}
}
