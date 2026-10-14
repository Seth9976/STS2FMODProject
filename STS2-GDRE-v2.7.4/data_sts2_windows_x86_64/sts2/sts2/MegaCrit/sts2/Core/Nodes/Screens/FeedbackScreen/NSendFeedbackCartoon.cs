using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen
{
	// Token: 0x02000325 RID: 805
	[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NSendFeedbackCartoon.cs")]
	public class NSendFeedbackCartoon : TextureRect
	{
		// Token: 0x06002CE4 RID: 11492 RVA: 0x00140610 File Offset: 0x0013E810
		public void SetRotation1()
		{
			base.Rotation = (this.opposite ? NSendFeedbackCartoon._rot1 : NSendFeedbackCartoon._rot2);
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x0014062C File Offset: 0x0013E82C
		public void SetRotation2()
		{
			base.Rotation = (this.opposite ? NSendFeedbackCartoon._rot2 : NSendFeedbackCartoon._rot1);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CE6 RID: 11494 RVA: 0x00140648 File Offset: 0x0013E848
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NSendFeedbackCartoon.MethodName.SetRotation1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackCartoon.MethodName.SetRotation2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CE7 RID: 11495 RVA: 0x001406BC File Offset: 0x0013E8BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSendFeedbackCartoon.MethodName.SetRotation1 && args.Count == 0)
			{
				this.SetRotation1();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackCartoon.MethodName.SetRotation2 && args.Count == 0)
			{
				this.SetRotation2();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002CE8 RID: 11496 RVA: 0x0014071C File Offset: 0x0013E91C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSendFeedbackCartoon.MethodName.SetRotation1 || (ref method) == NSendFeedbackCartoon.MethodName.SetRotation2 || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002CE9 RID: 11497 RVA: 0x00140743 File Offset: 0x0013E943
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSendFeedbackCartoon.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackCartoon.PropertyName.opposite)
			{
				this.opposite = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002CEA RID: 11498 RVA: 0x00140784 File Offset: 0x0013E984
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSendFeedbackCartoon.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NSendFeedbackCartoon.PropertyName.opposite)
			{
				value = VariantUtils.CreateFrom<bool>(ref this.opposite);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CEB RID: 11499 RVA: 0x001407DC File Offset: 0x0013E9DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSendFeedbackCartoon.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSendFeedbackCartoon.PropertyName.opposite, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CEC RID: 11500 RVA: 0x00140831 File Offset: 0x0013EA31
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSendFeedbackCartoon.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NSendFeedbackCartoon.PropertyName.opposite, Variant.From<bool>(ref this.opposite));
		}

		/// <inheritdoc />
		// Token: 0x06002CED RID: 11501 RVA: 0x00140868 File Offset: 0x0013EA68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSendFeedbackCartoon.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSendFeedbackCartoon.PropertyName.opposite, ref variant2))
			{
				this.opposite = variant2.As<bool>();
			}
		}

		// Token: 0x04001108 RID: 4360
		private static readonly float _rot1 = Mathf.DegToRad(5f);

		// Token: 0x04001109 RID: 4361
		private static readonly float _rot2 = 0f;

		// Token: 0x0400110A RID: 4362
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x0400110B RID: 4363
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public bool opposite;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001578 RID: 5496
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the 'SetRotation1' method.
			/// </summary>
			// Token: 0x04005238 RID: 21048
			public static readonly StringName SetRotation1 = "SetRotation1";

			/// <summary>
			/// Cached name for the 'SetRotation2' method.
			/// </summary>
			// Token: 0x04005239 RID: 21049
			public static readonly StringName SetRotation2 = "SetRotation2";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001579 RID: 5497
		public class PropertyName : TextureRect.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400523A RID: 21050
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the 'opposite' field.
			/// </summary>
			// Token: 0x0400523B RID: 21051
			public static readonly StringName opposite = "opposite";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200157A RID: 5498
		public class SignalName : TextureRect.SignalName
		{
		}
	}
}
