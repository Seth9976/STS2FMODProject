using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000191 RID: 401
	[ScriptPath("res://src/Core/Nodes/Vfx/NCardTrail.cs")]
	public class NCardTrail : Line2D
	{
		// Token: 0x0600139B RID: 5019 RVA: 0x0007644B File Offset: 0x0007464B
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnToggleVisibility)), 0U);
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00076478 File Offset: 0x00074678
		public override void _Process(double delta)
		{
			base.GlobalPosition = Vector2.Zero;
			base.GlobalRotation = 0f;
			float num = (float)delta;
			for (int i = 0; i < base.GetPointCount(); i++)
			{
				if (this._pointAge[i] > this._pointDuration)
				{
					base.RemovePoint(0);
					this._pointAge.RemoveAt(0);
				}
				else
				{
					List<float> pointAge = this._pointAge;
					int num2 = i;
					pointAge[num2] += num;
				}
			}
			this.CreatePoint(this._parent.GlobalPosition, delta);
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00076503 File Offset: 0x00074703
		private void OnToggleVisibility()
		{
			base.ProcessMode = (base.Visible ? 0L : 4L);
			base.ClearPoints();
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00076520 File Offset: 0x00074720
		private void CreatePoint(Vector2 pointPos, double delta)
		{
			if (this._lastPointPosition != null)
			{
				float num = pointPos.DistanceTo(this._lastPointPosition.Value);
				if (num < 12f)
				{
					return;
				}
				int pointCount = base.GetPointCount();
				if (pointCount > 2 && num > 48f)
				{
					Vector2 pointPosition = base.GetPointPosition(pointCount - 2);
					Vector2 pointPosition2 = base.GetPointPosition(pointCount - 1);
					Vector2 vector = pointPos;
					for (float num2 = 48f; num2 < num - 12f; num2 += 48f)
					{
						float num3 = 0.5f + num2 / num * 0.5f;
						Vector2 vector2 = pointPosition.Lerp(pointPosition2, num3);
						Vector2 vector3 = pointPosition2.Lerp(vector, num3);
						Vector2 vector4 = vector2.Lerp(vector3, num3);
						this._pointAge.Add((float)delta * num3);
						base.AddPoint(vector4, -1);
					}
				}
			}
			this._pointAge.Add(0f);
			base.AddPoint(pointPos, -1);
			this._lastPointPosition = new Vector2?(pointPos);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600139F RID: 5023 RVA: 0x0007661C File Offset: 0x0007481C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCardTrail.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardTrail.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardTrail.MethodName.OnToggleVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardTrail.MethodName.CreatePoint, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "pointPos", 0L, "", 6L, false),
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013A0 RID: 5024 RVA: 0x00076758 File Offset: 0x00074958
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardTrail.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTrail.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTrail.MethodName.OnToggleVisibility && args.Count == 0)
			{
				this.OnToggleVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTrail.MethodName.CreatePoint && args.Count == 2)
			{
				this.CreatePoint(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<double>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060013A1 RID: 5025 RVA: 0x0007682C File Offset: 0x00074A2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardTrail.MethodName._Ready || (ref method) == NCardTrail.MethodName._Process || (ref method) == NCardTrail.MethodName.OnToggleVisibility || (ref method) == NCardTrail.MethodName.CreatePoint || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060013A2 RID: 5026 RVA: 0x0007687C File Offset: 0x00074A7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardTrail.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCardTrail.PropertyName._pointDuration)
			{
				this._pointDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060013A3 RID: 5027 RVA: 0x000768BC File Offset: 0x00074ABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardTrail.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			if ((ref name) == NCardTrail.PropertyName._pointDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._pointDuration);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060013A4 RID: 5028 RVA: 0x00076914 File Offset: 0x00074B14
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardTrail.PropertyName._parent, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardTrail.PropertyName._pointDuration, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013A5 RID: 5029 RVA: 0x00076969 File Offset: 0x00074B69
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardTrail.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
			info.AddProperty(NCardTrail.PropertyName._pointDuration, Variant.From<float>(ref this._pointDuration));
		}

		/// <inheritdoc />
		// Token: 0x060013A6 RID: 5030 RVA: 0x000769A0 File Offset: 0x00074BA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardTrail.PropertyName._parent, ref variant))
			{
				this._parent = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardTrail.PropertyName._pointDuration, ref variant2))
			{
				this._pointDuration = variant2.As<float>();
			}
		}

		// Token: 0x04000705 RID: 1797
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000706 RID: 1798
		private float _pointDuration = 0.8f;

		// Token: 0x04000707 RID: 1799
		[Nullable(1)]
		private readonly List<float> _pointAge = new List<float>();

		// Token: 0x04000708 RID: 1800
		private const float _minSpawnDist = 12f;

		// Token: 0x04000709 RID: 1801
		private const float _maxSpawnDist = 48f;

		// Token: 0x0400070A RID: 1802
		private Vector2? _lastPointPosition;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FCF RID: 4047
		public class MethodName : Line2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003EF7 RID: 16119
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04003EF8 RID: 16120
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'OnToggleVisibility' method.
			/// </summary>
			// Token: 0x04003EF9 RID: 16121
			public static readonly StringName OnToggleVisibility = "OnToggleVisibility";

			/// <summary>
			/// Cached name for the 'CreatePoint' method.
			/// </summary>
			// Token: 0x04003EFA RID: 16122
			public static readonly StringName CreatePoint = "CreatePoint";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FD0 RID: 4048
		public class PropertyName : Line2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04003EFB RID: 16123
			public static readonly StringName _parent = "_parent";

			/// <summary>
			/// Cached name for the '_pointDuration' field.
			/// </summary>
			// Token: 0x04003EFC RID: 16124
			public static readonly StringName _pointDuration = "_pointDuration";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FD1 RID: 4049
		public class SignalName : Line2D.SignalName
		{
		}
	}
}
