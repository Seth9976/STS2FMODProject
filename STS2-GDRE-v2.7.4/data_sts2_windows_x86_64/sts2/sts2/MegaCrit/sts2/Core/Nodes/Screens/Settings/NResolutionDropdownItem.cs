using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002B0 RID: 688
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NResolutionDropdownItem.cs")]
	public class NResolutionDropdownItem : NDropdownItem
	{
		// Token: 0x0600246E RID: 9326 RVA: 0x000F7414 File Offset: 0x000F5614
		public void Init(Vector2I setResolution)
		{
			this.resolution = setResolution;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.resolution.X);
			defaultInterpolatedStringHandler.AppendLiteral(" x ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.resolution.Y);
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			this._label.SetTextAutoSize(text);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600246F RID: 9327 RVA: 0x000F7478 File Offset: 0x000F5678
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NResolutionDropdownItem.MethodName.Init, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(6L, "setResolution", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002470 RID: 9328 RVA: 0x000F74E1 File Offset: 0x000F56E1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NResolutionDropdownItem.MethodName.Init && args.Count == 1)
			{
				this.Init(VariantUtils.ConvertTo<Vector2I>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002471 RID: 9329 RVA: 0x000F751F File Offset: 0x000F571F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NResolutionDropdownItem.MethodName.Init || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002472 RID: 9330 RVA: 0x000F7537 File Offset: 0x000F5737
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NResolutionDropdownItem.PropertyName.resolution)
			{
				this.resolution = VariantUtils.ConvertTo<Vector2I>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002473 RID: 9331 RVA: 0x000F755C File Offset: 0x000F575C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NResolutionDropdownItem.PropertyName.resolution)
			{
				value = VariantUtils.CreateFrom<Vector2I>(ref this.resolution);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002474 RID: 9332 RVA: 0x000F7588 File Offset: 0x000F5788
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(6L, NResolutionDropdownItem.PropertyName.resolution, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002475 RID: 9333 RVA: 0x000F75BC File Offset: 0x000F57BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NResolutionDropdownItem.PropertyName.resolution, Variant.From<Vector2I>(ref this.resolution));
		}

		/// <inheritdoc />
		// Token: 0x06002476 RID: 9334 RVA: 0x000F75DC File Offset: 0x000F57DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NResolutionDropdownItem.PropertyName.resolution, ref variant))
			{
				this.resolution = variant.As<Vector2I>();
			}
		}

		// Token: 0x04000DB8 RID: 3512
		public Vector2I resolution;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013C8 RID: 5064
		public new class MethodName : NDropdownItem.MethodName
		{
			/// <summary>
			/// Cached name for the 'Init' method.
			/// </summary>
			// Token: 0x04004BC3 RID: 19395
			public static readonly StringName Init = "Init";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013C9 RID: 5065
		public new class PropertyName : NDropdownItem.PropertyName
		{
			/// <summary>
			/// Cached name for the 'resolution' field.
			/// </summary>
			// Token: 0x04004BC4 RID: 19396
			public static readonly StringName resolution = "resolution";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013CA RID: 5066
		public new class SignalName : NDropdownItem.SignalName
		{
		}
	}
}
