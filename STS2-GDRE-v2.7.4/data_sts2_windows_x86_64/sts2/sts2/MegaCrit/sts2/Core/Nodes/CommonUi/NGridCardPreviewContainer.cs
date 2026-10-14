using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	// Token: 0x020003E0 RID: 992
	[ScriptPath("res://src/Core/Nodes/CommonUi/NGridCardPreviewContainer.cs")]
	public class NGridCardPreviewContainer : Control
	{
		// Token: 0x06003C64 RID: 15460 RVA: 0x001CE3A0 File Offset: 0x001CC5A0
		public override void _Ready()
		{
			base.Connect(Node.SignalName.ChildEnteredTree, Callable.From<Node>(new Action<Node>(this.ReformatElements)), 0U);
			base.Connect(Node.SignalName.ChildExitingTree, Callable.From<Node>(new Action<Node>(this.CheckAnyChildrenPresent)), 0U);
		}

		/// <summary>
		/// Enforces a max column count on the grid container.
		/// Call this before adding elements to the container. Once all cards added have disappeared, the count will reset.
		/// </summary>
		// Token: 0x06003C65 RID: 15461 RVA: 0x001CE3DE File Offset: 0x001CC5DE
		public void ForceMaxColumnsUntilEmpty(int maxColumns)
		{
			this._forcedMaxColumns = new int?(maxColumns);
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x001CE3EC File Offset: 0x001CC5EC
		[NullableContext(1)]
		private void ReformatElements(Node _)
		{
			Vector2 vector = base.Size / 2f + Vector2.Down * 50f;
			int childCount = base.GetChildCount(false);
			int num = Mathf.FloorToInt(base.GetViewportRect().Size.X / (NCard.defaultSize.X + 25f));
			int num2 = Mathf.CeilToInt((float)childCount / (float)num);
			if (this._forcedMaxColumns != null)
			{
				num = Math.Min(num, this._forcedMaxColumns.Value);
			}
			for (int i = 0; i < childCount; i++)
			{
				int num3 = i / num;
				int num4 = i % num;
				int num5 = Math.Min(num, childCount - num3 * num);
				float num6 = (float)(-(float)(num2 - 1)) * (NCard.defaultSize.Y + 25f) * 0.5f;
				float num7 = (float)(-(float)(num5 - 1)) * (NCard.defaultSize.X + 25f) * 0.5f;
				Vector2 vector2 = vector;
				Vector2 vector3 = vector2 + new Vector2(num7 + (NCard.defaultSize.X + 25f) * (float)num4, num6 + (NCard.defaultSize.Y + 25f) * (float)num3);
				Node child = base.GetChild(i, false);
				Node2D node2D = child as Node2D;
				if (node2D != null)
				{
					node2D.Position = vector3;
				}
				else
				{
					((Control)child).Position = vector3;
				}
			}
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x001CE559 File Offset: 0x001CC759
		[NullableContext(1)]
		private void CheckAnyChildrenPresent(Node _)
		{
			if (base.GetChildCount(false) == 0)
			{
				this._forcedMaxColumns = null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C68 RID: 15464 RVA: 0x001CE570 File Offset: 0x001CC770
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NGridCardPreviewContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGridCardPreviewContainer.MethodName.ForceMaxColumnsUntilEmpty, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "maxColumns", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGridCardPreviewContainer.MethodName.ReformatElements, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NGridCardPreviewContainer.MethodName.CheckAnyChildrenPresent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Node"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C69 RID: 15465 RVA: 0x001CE6C8 File Offset: 0x001CC8C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGridCardPreviewContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardPreviewContainer.MethodName.ForceMaxColumnsUntilEmpty && args.Count == 1)
			{
				this.ForceMaxColumnsUntilEmpty(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardPreviewContainer.MethodName.ReformatElements && args.Count == 1)
			{
				this.ReformatElements(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardPreviewContainer.MethodName.CheckAnyChildrenPresent && args.Count == 1)
			{
				this.CheckAnyChildrenPresent(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003C6A RID: 15466 RVA: 0x001CE79C File Offset: 0x001CC99C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGridCardPreviewContainer.MethodName._Ready || (ref method) == NGridCardPreviewContainer.MethodName.ForceMaxColumnsUntilEmpty || (ref method) == NGridCardPreviewContainer.MethodName.ReformatElements || (ref method) == NGridCardPreviewContainer.MethodName.CheckAnyChildrenPresent || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003C6B RID: 15467 RVA: 0x001CE7EC File Offset: 0x001CC9EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003C6C RID: 15468 RVA: 0x001CE7F5 File Offset: 0x001CC9F5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04001798 RID: 6040
		private int? _forcedMaxColumns;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001850 RID: 6224
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005F3B RID: 24379
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ForceMaxColumnsUntilEmpty' method.
			/// </summary>
			// Token: 0x04005F3C RID: 24380
			public static readonly StringName ForceMaxColumnsUntilEmpty = "ForceMaxColumnsUntilEmpty";

			/// <summary>
			/// Cached name for the 'ReformatElements' method.
			/// </summary>
			// Token: 0x04005F3D RID: 24381
			public static readonly StringName ReformatElements = "ReformatElements";

			/// <summary>
			/// Cached name for the 'CheckAnyChildrenPresent' method.
			/// </summary>
			// Token: 0x04005F3E RID: 24382
			public static readonly StringName CheckAnyChildrenPresent = "CheckAnyChildrenPresent";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001851 RID: 6225
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001852 RID: 6226
		public class SignalName : Control.SignalName
		{
		}
	}
}
