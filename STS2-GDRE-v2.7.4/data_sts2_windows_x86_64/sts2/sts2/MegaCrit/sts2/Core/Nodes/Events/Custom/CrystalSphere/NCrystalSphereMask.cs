using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;

namespace MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere
{
	/// <summary>
	/// Node that manages the orb reveal shader for the Crystal Sphere event
	/// </summary>
	// Token: 0x020003C3 RID: 963
	[ScriptPath("res://src/Core/Nodes/Events/Custom/CrystalSphere/NCrystalSphereMask.cs")]
	public class NCrystalSphereMask : Control
	{
		// Token: 0x06003A30 RID: 14896 RVA: 0x001BA4E4 File Offset: 0x001B86E4
		public override void _Ready()
		{
			this._material = (ShaderMaterial)base.Material;
			for (int i = 0; i < 121; i++)
			{
				this._values.Add(Vector3.Zero);
			}
			this._values[3] = new Vector3(1f, 0f, 0f);
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x001BA53F File Offset: 0x001B873F
		public override void _Process(double delta)
		{
			this._time += (float)delta;
			this._material.SetShaderParameter(NCrystalSphereMask._timeStr, this._time);
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x001BA56C File Offset: 0x001B876C
		[NullableContext(1)]
		public void UpdateMat(CrystalSphereCell cell)
		{
			int num = cell.Y * 11 + cell.X;
			float num2 = this._values[num].Y;
			if (this._values[num].Z == 0f)
			{
				num2 = 1f;
			}
			this._values[num] = new Vector3(num2, cell.IsHidden > false, this._time);
			this._material.SetShaderParameter(NCrystalSphereMask._gridFadeParams, this._values);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A33 RID: 14899 RVA: 0x001BA5F8 File Offset: 0x001B87F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCrystalSphereMask.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereMask.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A34 RID: 14900 RVA: 0x001BA690 File Offset: 0x001B8890
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCrystalSphereMask.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereMask.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003A35 RID: 14901 RVA: 0x001BA6FE File Offset: 0x001B88FE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCrystalSphereMask.MethodName._Ready || (ref method) == NCrystalSphereMask.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003A36 RID: 14902 RVA: 0x001BA728 File Offset: 0x001B8928
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCrystalSphereMask.PropertyName._material)
			{
				this._material = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereMask.PropertyName._values)
			{
				this._values = VariantUtils.ConvertToArray<Vector3>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereMask.PropertyName._time)
			{
				this._time = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003A37 RID: 14903 RVA: 0x001BA790 File Offset: 0x001B8990
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCrystalSphereMask.PropertyName._material)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._material);
				return true;
			}
			if ((ref name) == NCrystalSphereMask.PropertyName._values)
			{
				value = VariantUtils.CreateFromArray<Vector3>(this._values);
				return true;
			}
			if ((ref name) == NCrystalSphereMask.PropertyName._time)
			{
				value = VariantUtils.CreateFrom<float>(ref this._time);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A38 RID: 14904 RVA: 0x001BA808 File Offset: 0x001B8A08
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCrystalSphereMask.PropertyName._material, 0L, "", 4096L, false),
				new PropertyInfo(28L, NCrystalSphereMask.PropertyName._values, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCrystalSphereMask.PropertyName._time, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A39 RID: 14905 RVA: 0x001BA880 File Offset: 0x001B8A80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCrystalSphereMask.PropertyName._material, Variant.From<ShaderMaterial>(ref this._material));
			info.AddProperty(NCrystalSphereMask.PropertyName._values, Variant.CreateFrom<Vector3>(this._values));
			info.AddProperty(NCrystalSphereMask.PropertyName._time, Variant.From<float>(ref this._time));
		}

		/// <inheritdoc />
		// Token: 0x06003A3A RID: 14906 RVA: 0x001BA8D8 File Offset: 0x001B8AD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCrystalSphereMask.PropertyName._material, ref variant))
			{
				this._material = variant.As<ShaderMaterial>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCrystalSphereMask.PropertyName._values, ref variant2))
			{
				this._values = variant2.AsGodotArray<Vector3>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCrystalSphereMask.PropertyName._time, ref variant3))
			{
				this._time = variant3.As<float>();
			}
		}

		// Token: 0x040016B2 RID: 5810
		[Nullable(1)]
		private static readonly StringName _gridFadeParams = new StringName("gridFadeParams");

		// Token: 0x040016B3 RID: 5811
		[Nullable(1)]
		private static readonly StringName _timeStr = new StringName("time");

		// Token: 0x040016B4 RID: 5812
		[Nullable(1)]
		private ShaderMaterial _material;

		// Token: 0x040016B5 RID: 5813
		[Nullable(1)]
		private Array<Vector3> _values = new Array<Vector3>();

		// Token: 0x040016B6 RID: 5814
		private float _time;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017EA RID: 6122
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005D58 RID: 23896
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005D59 RID: 23897
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017EB RID: 6123
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_material' field.
			/// </summary>
			// Token: 0x04005D5A RID: 23898
			public static readonly StringName _material = "_material";

			/// <summary>
			/// Cached name for the '_values' field.
			/// </summary>
			// Token: 0x04005D5B RID: 23899
			public static readonly StringName _values = "_values";

			/// <summary>
			/// Cached name for the '_time' field.
			/// </summary>
			// Token: 0x04005D5C RID: 23900
			public static readonly StringName _time = "_time";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017EC RID: 6124
		public class SignalName : Control.SignalName
		{
		}
	}
}
