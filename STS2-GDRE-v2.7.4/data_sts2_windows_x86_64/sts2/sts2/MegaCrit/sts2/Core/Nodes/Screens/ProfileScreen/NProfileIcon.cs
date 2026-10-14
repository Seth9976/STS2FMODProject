using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen
{
	// Token: 0x020002DB RID: 731
	[ScriptPath("res://src/Core/Nodes/Screens/ProfileScreen/NProfileIcon.cs")]
	public class NProfileIcon : Control
	{
		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002703 RID: 9987 RVA: 0x0010A939 File Offset: 0x00108B39
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					ImageHelper.GetImagePath("ui/profile/profile_icon_1.png"),
					ImageHelper.GetImagePath("ui/profile/profile_icon_2.png"),
					ImageHelper.GetImagePath("ui/profile/profile_icon_3.png")
				});
			}
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x0010A96D File Offset: 0x00108B6D
		public override void _Ready()
		{
			this._icon = base.GetNode<TextureRect>("Icon");
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x0010A988 File Offset: 0x00108B88
		public void SetProfileId(int profileId)
		{
			TextureRect icon = this._icon;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
			defaultInterpolatedStringHandler.AppendLiteral("ui/profile/profile_icon_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(profileId);
			defaultInterpolatedStringHandler.AppendLiteral(".png");
			icon.Texture = ResourceLoader.Load<Texture2D>(ImageHelper.GetImagePath(defaultInterpolatedStringHandler.ToStringAndClear()), null, 1L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002706 RID: 9990 RVA: 0x0010A9E0 File Offset: 0x00108BE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NProfileIcon.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProfileIcon.MethodName.SetProfileId, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "profileId", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002707 RID: 9991 RVA: 0x0010AA78 File Offset: 0x00108C78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NProfileIcon.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProfileIcon.MethodName.SetProfileId && args.Count == 1)
			{
				this.SetProfileId(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002708 RID: 9992 RVA: 0x0010AAE6 File Offset: 0x00108CE6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NProfileIcon.MethodName._Ready || (ref method) == NProfileIcon.MethodName.SetProfileId || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002709 RID: 9993 RVA: 0x0010AB0D File Offset: 0x00108D0D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NProfileIcon.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600270A RID: 9994 RVA: 0x0010AB32 File Offset: 0x00108D32
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NProfileIcon.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600270B RID: 9995 RVA: 0x0010AB5C File Offset: 0x00108D5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NProfileIcon.PropertyName._icon, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600270C RID: 9996 RVA: 0x0010AB91 File Offset: 0x00108D91
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NProfileIcon.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
		}

		/// <inheritdoc />
		// Token: 0x0600270D RID: 9997 RVA: 0x0010ABB0 File Offset: 0x00108DB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NProfileIcon.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
		}

		// Token: 0x04000E8B RID: 3723
		[Nullable(1)]
		private TextureRect _icon;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001462 RID: 5218
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004D69 RID: 19817
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetProfileId' method.
			/// </summary>
			// Token: 0x04004D6A RID: 19818
			public static readonly StringName SetProfileId = "SetProfileId";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001463 RID: 5219
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004D6B RID: 19819
			public static readonly StringName _icon = "_icon";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001464 RID: 5220
		public class SignalName : Control.SignalName
		{
		}
	}
}
