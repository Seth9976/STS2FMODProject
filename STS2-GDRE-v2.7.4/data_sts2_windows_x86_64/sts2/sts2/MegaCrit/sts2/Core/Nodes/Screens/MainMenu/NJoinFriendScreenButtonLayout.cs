using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	/// <summary>
	/// Lays out buttons centered in a vertical column. When that column is full, starts a new column and centers both.
	/// This assumes that all children are the same size.
	/// </summary>
	// Token: 0x02000305 RID: 773
	[Tool]
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NJoinFriendScreenButtonLayout.cs")]
	public class NJoinFriendScreenButtonLayout : Container
	{
		// Token: 0x06002A6F RID: 10863 RVA: 0x00128759 File Offset: 0x00126959
		public override void _Notification(int what)
		{
			if ((long)what == 51L)
			{
				this.LayoutChildren();
			}
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x00128768 File Offset: 0x00126968
		private void LayoutChildren()
		{
			Control[] array = base.GetChildren(false).OfType<Control>().ToArray<Control>();
			if (array.Length == 0)
			{
				return;
			}
			Vector2 size = array[0].Size;
			int num = (int)(base.Size.Y / size.Y);
			int num2 = (int)Math.Ceiling((double)((float)array.Length / (float)num));
			float num3 = (base.Size.X - (float)num2 * size.X) * 0.5f;
			for (int i = 0; i < array.Length; i++)
			{
				int num4 = i / num2;
				int num5 = i - num4 * num2;
				array[i].Position = new Vector2((float)num5, (float)num4) * size + Vector2.Right * num3;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A71 RID: 10865 RVA: 0x00128824 File Offset: 0x00126A24
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NJoinFriendScreenButtonLayout.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NJoinFriendScreenButtonLayout.MethodName.LayoutChildren, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A72 RID: 10866 RVA: 0x001288BC File Offset: 0x00126ABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NJoinFriendScreenButtonLayout.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NJoinFriendScreenButtonLayout.MethodName.LayoutChildren && args.Count == 0)
			{
				this.LayoutChildren();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002A73 RID: 10867 RVA: 0x0012892A File Offset: 0x00126B2A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NJoinFriendScreenButtonLayout.MethodName._Notification || (ref method) == NJoinFriendScreenButtonLayout.MethodName.LayoutChildren || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002A74 RID: 10868 RVA: 0x00128951 File Offset: 0x00126B51
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002A75 RID: 10869 RVA: 0x0012895A File Offset: 0x00126B5A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014FC RID: 5372
		public class MethodName : Container.MethodName
		{
			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x04004FF0 RID: 20464
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'LayoutChildren' method.
			/// </summary>
			// Token: 0x04004FF1 RID: 20465
			public static readonly StringName LayoutChildren = "LayoutChildren";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014FD RID: 5373
		public class PropertyName : Container.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014FE RID: 5374
		public class SignalName : Container.SignalName
		{
		}
	}
}
