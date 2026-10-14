using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	// Token: 0x020003D3 RID: 979
	[ScriptPath("res://src/Core/Nodes/CommonUi/NCardPreviewContainer.cs")]
	public class NCardPreviewContainer : Control
	{
		// Token: 0x06003B41 RID: 15169 RVA: 0x001C4BA4 File Offset: 0x001C2DA4
		public override void _Ready()
		{
			base.Connect(Node.SignalName.ChildEnteredTree, Callable.From<Node>(new Action<Node>(this.ReformatElements)), 0U);
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x001C4BC4 File Offset: 0x001C2DC4
		[NullableContext(1)]
		private void ReformatElements(Node _)
		{
			Vector2 vector = base.Size / 2f + Vector2.Down * 50f;
			int childCount = base.GetChildCount(false);
			for (int i = 0; i < childCount; i++)
			{
				float num = (float)(-(float)(childCount - 1) * 325) * 0.5f;
				Vector2 vector2 = vector;
				Vector2 vector3 = vector2 + Vector2.Right * (num + (float)(325 * i));
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

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B43 RID: 15171 RVA: 0x001C4C6C File Offset: 0x001C2E6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCardPreviewContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPreviewContainer.MethodName.ReformatElements, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Node"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B44 RID: 15172 RVA: 0x001C4D10 File Offset: 0x001C2F10
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardPreviewContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPreviewContainer.MethodName.ReformatElements && args.Count == 1)
			{
				this.ReformatElements(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003B45 RID: 15173 RVA: 0x001C4D7E File Offset: 0x001C2F7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardPreviewContainer.MethodName._Ready || (ref method) == NCardPreviewContainer.MethodName.ReformatElements || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003B46 RID: 15174 RVA: 0x001C4DA5 File Offset: 0x001C2FA5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003B47 RID: 15175 RVA: 0x001C4DAE File Offset: 0x001C2FAE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200181F RID: 6175
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005E5A RID: 24154
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ReformatElements' method.
			/// </summary>
			// Token: 0x04005E5B RID: 24155
			public static readonly StringName ReformatElements = "ReformatElements";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001820 RID: 6176
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001821 RID: 6177
		public class SignalName : Control.SignalName
		{
		}
	}
}
