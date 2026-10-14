using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Examples
{
	// Token: 0x0200022D RID: 557
	[ScriptPath("res://src/Core/Nodes/Vfx/Examples/NNoiseScroller.cs")]
	public class NNoiseScroller : TextureRect
	{
		// Token: 0x06001C48 RID: 7240 RVA: 0x000B799C File Offset: 0x000B5B9C
		public override void _Ready()
		{
			this._noise = (FastNoiseLite)((NoiseTexture2D)base.Texture).Noise;
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000B79B9 File Offset: 0x000B5BB9
		public override void _Process(double delta)
		{
			this._noise.Offset += this._offsetDelta * (float)delta;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C4A RID: 7242 RVA: 0x000B79E0 File Offset: 0x000B5BE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NNoiseScroller.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNoiseScroller.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C4B RID: 7243 RVA: 0x000B7A78 File Offset: 0x000B5C78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NNoiseScroller.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNoiseScroller.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001C4C RID: 7244 RVA: 0x000B7AE6 File Offset: 0x000B5CE6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NNoiseScroller.MethodName._Ready || (ref method) == NNoiseScroller.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001C4D RID: 7245 RVA: 0x000B7B0D File Offset: 0x000B5D0D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NNoiseScroller.PropertyName._offsetDelta)
			{
				this._offsetDelta = VariantUtils.ConvertTo<Vector3>(ref value);
				return true;
			}
			if ((ref name) == NNoiseScroller.PropertyName._noise)
			{
				this._noise = VariantUtils.ConvertTo<FastNoiseLite>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001C4E RID: 7246 RVA: 0x000B7B50 File Offset: 0x000B5D50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NNoiseScroller.PropertyName._offsetDelta)
			{
				value = VariantUtils.CreateFrom<Vector3>(ref this._offsetDelta);
				return true;
			}
			if ((ref name) == NNoiseScroller.PropertyName._noise)
			{
				value = VariantUtils.CreateFrom<FastNoiseLite>(ref this._noise);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C4F RID: 7247 RVA: 0x000B7BA8 File Offset: 0x000B5DA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(9L, NNoiseScroller.PropertyName._offsetDelta, 0L, "", 4102L, true),
				new PropertyInfo(24L, NNoiseScroller.PropertyName._noise, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C50 RID: 7248 RVA: 0x000B7BFE File Offset: 0x000B5DFE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NNoiseScroller.PropertyName._offsetDelta, Variant.From<Vector3>(ref this._offsetDelta));
			info.AddProperty(NNoiseScroller.PropertyName._noise, Variant.From<FastNoiseLite>(ref this._noise));
		}

		/// <inheritdoc />
		// Token: 0x06001C51 RID: 7249 RVA: 0x000B7C34 File Offset: 0x000B5E34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NNoiseScroller.PropertyName._offsetDelta, ref variant))
			{
				this._offsetDelta = variant.As<Vector3>();
			}
			Variant variant2;
			if (info.TryGetProperty(NNoiseScroller.PropertyName._noise, ref variant2))
			{
				this._noise = variant2.As<FastNoiseLite>();
			}
		}

		// Token: 0x04000AD2 RID: 2770
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Vector3 _offsetDelta;

		// Token: 0x04000AD3 RID: 2771
		[Nullable(1)]
		private FastNoiseLite _noise;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011E2 RID: 4578
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004577 RID: 17783
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004578 RID: 17784
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011E3 RID: 4579
		public class PropertyName : TextureRect.PropertyName
		{
			/// <summary>
			/// Cached name for the '_offsetDelta' field.
			/// </summary>
			// Token: 0x04004579 RID: 17785
			public static readonly StringName _offsetDelta = "_offsetDelta";

			/// <summary>
			/// Cached name for the '_noise' field.
			/// </summary>
			// Token: 0x0400457A RID: 17786
			public static readonly StringName _noise = "_noise";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011E4 RID: 4580
		public class SignalName : TextureRect.SignalName
		{
		}
	}
}
