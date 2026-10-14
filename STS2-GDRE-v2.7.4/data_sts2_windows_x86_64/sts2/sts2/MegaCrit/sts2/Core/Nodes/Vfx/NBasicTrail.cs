using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000183 RID: 387
	[ScriptPath("res://src/Core/Nodes/Vfx/NBasicTrail.cs")]
	public class NBasicTrail : Line2D
	{
		// Token: 0x060012B2 RID: 4786 RVA: 0x0006FF03 File Offset: 0x0006E103
		public override void _Ready()
		{
			this._target = base.GetParent<Node2D>();
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0006FF14 File Offset: 0x0006E114
		public override void _Process(double delta)
		{
			base.GlobalPosition = Vector2.Zero;
			base.GlobalRotation = 0f;
			base.GlobalScale = Vector2.One;
			base.AddPoint(this._target.GlobalPosition, -1);
			if (base.Points.Length > this._maxSegments)
			{
				base.RemovePoint(0);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060012B4 RID: 4788 RVA: 0x0006FF6C File Offset: 0x0006E16C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NBasicTrail.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBasicTrail.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012B5 RID: 4789 RVA: 0x00070004 File Offset: 0x0006E204
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBasicTrail.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBasicTrail.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060012B6 RID: 4790 RVA: 0x00070072 File Offset: 0x0006E272
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBasicTrail.MethodName._Ready || (ref method) == NBasicTrail.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060012B7 RID: 4791 RVA: 0x00070099 File Offset: 0x0006E299
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBasicTrail.PropertyName._target)
			{
				this._target = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NBasicTrail.PropertyName._maxSegments)
			{
				this._maxSegments = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060012B8 RID: 4792 RVA: 0x000700DC File Offset: 0x0006E2DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBasicTrail.PropertyName._target)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._target);
				return true;
			}
			if ((ref name) == NBasicTrail.PropertyName._maxSegments)
			{
				value = VariantUtils.CreateFrom<int>(ref this._maxSegments);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060012B9 RID: 4793 RVA: 0x00070134 File Offset: 0x0006E334
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NBasicTrail.PropertyName._target, 0L, "", 4096L, false),
				new PropertyInfo(2L, NBasicTrail.PropertyName._maxSegments, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012BA RID: 4794 RVA: 0x00070189 File Offset: 0x0006E389
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBasicTrail.PropertyName._target, Variant.From<Node2D>(ref this._target));
			info.AddProperty(NBasicTrail.PropertyName._maxSegments, Variant.From<int>(ref this._maxSegments));
		}

		/// <inheritdoc />
		// Token: 0x060012BB RID: 4795 RVA: 0x000701C0 File Offset: 0x0006E3C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBasicTrail.PropertyName._target, ref variant))
			{
				this._target = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBasicTrail.PropertyName._maxSegments, ref variant2))
			{
				this._maxSegments = variant2.As<int>();
			}
		}

		// Token: 0x040006A5 RID: 1701
		[Nullable(1)]
		private Node2D _target;

		// Token: 0x040006A6 RID: 1702
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private int _maxSegments = 10;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F99 RID: 3993
		public class MethodName : Line2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E4A RID: 15946
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04003E4B RID: 15947
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F9A RID: 3994
		public class PropertyName : Line2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_target' field.
			/// </summary>
			// Token: 0x04003E4C RID: 15948
			public static readonly StringName _target = "_target";

			/// <summary>
			/// Cached name for the '_maxSegments' field.
			/// </summary>
			// Token: 0x04003E4D RID: 15949
			public static readonly StringName _maxSegments = "_maxSegments";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F9B RID: 3995
		public class SignalName : Line2D.SignalName
		{
		}
	}
}
