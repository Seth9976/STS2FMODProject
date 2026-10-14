using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000185 RID: 389
	[ScriptPath("res://src/Core/Nodes/Vfx/NBezierTrail.cs")]
	public class NBezierTrail : Line2D
	{
		// Token: 0x060012C9 RID: 4809 RVA: 0x000705F8 File Offset: 0x0006E7F8
		public override void _Ready()
		{
			this._target = base.GetParent<Node2D>();
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnToggleVisibility)), 0U);
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00070624 File Offset: 0x0006E824
		public override void _Process(double delta)
		{
			base.GlobalPosition = Vector2.Zero;
			base.GlobalRotation = 0f;
			this.CreatePoint(this._target.GlobalPosition, delta);
			float num = (float)delta;
			for (int i = 0; i < base.Points.Length; i++)
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
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x000706B1 File Offset: 0x0006E8B1
		private void OnToggleVisibility()
		{
			base.ProcessMode = (base.Visible ? 0L : 4L);
			base.ClearPoints();
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x000706D0 File Offset: 0x0006E8D0
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
					for (float num2 = 48f; num2 < num - 12f; num2 += 48f)
					{
						float num3 = 0.5f + num2 / num * 0.5f;
						Vector2 vector = pointPosition.Lerp(pointPosition2, num3);
						Vector2 vector2 = pointPosition2.Lerp(pointPos, num3);
						Vector2 vector3 = vector.Lerp(vector2, num3);
						this._pointAge.Add((float)delta * num3);
						base.AddPoint(vector3, -1);
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
		// Token: 0x060012CD RID: 4813 RVA: 0x000707C8 File Offset: 0x0006E9C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NBezierTrail.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBezierTrail.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBezierTrail.MethodName.OnToggleVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBezierTrail.MethodName.CreatePoint, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "pointPos", 0L, "", 6L, false),
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012CE RID: 4814 RVA: 0x00070904 File Offset: 0x0006EB04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBezierTrail.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBezierTrail.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBezierTrail.MethodName.OnToggleVisibility && args.Count == 0)
			{
				this.OnToggleVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBezierTrail.MethodName.CreatePoint && args.Count == 2)
			{
				this.CreatePoint(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<double>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060012CF RID: 4815 RVA: 0x000709D8 File Offset: 0x0006EBD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBezierTrail.MethodName._Ready || (ref method) == NBezierTrail.MethodName._Process || (ref method) == NBezierTrail.MethodName.OnToggleVisibility || (ref method) == NBezierTrail.MethodName.CreatePoint || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060012D0 RID: 4816 RVA: 0x00070A28 File Offset: 0x0006EC28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBezierTrail.PropertyName._pointDuration)
			{
				this._pointDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NBezierTrail.PropertyName._target)
			{
				this._target = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060012D1 RID: 4817 RVA: 0x00070A68 File Offset: 0x0006EC68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBezierTrail.PropertyName._pointDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._pointDuration);
				return true;
			}
			if ((ref name) == NBezierTrail.PropertyName._target)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._target);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060012D2 RID: 4818 RVA: 0x00070AC0 File Offset: 0x0006ECC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NBezierTrail.PropertyName._pointDuration, 0L, "", 4102L, true),
				new PropertyInfo(24L, NBezierTrail.PropertyName._target, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012D3 RID: 4819 RVA: 0x00070B15 File Offset: 0x0006ED15
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBezierTrail.PropertyName._pointDuration, Variant.From<float>(ref this._pointDuration));
			info.AddProperty(NBezierTrail.PropertyName._target, Variant.From<Node2D>(ref this._target));
		}

		/// <inheritdoc />
		// Token: 0x060012D4 RID: 4820 RVA: 0x00070B4C File Offset: 0x0006ED4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBezierTrail.PropertyName._pointDuration, ref variant))
			{
				this._pointDuration = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBezierTrail.PropertyName._target, ref variant2))
			{
				this._target = variant2.As<Node2D>();
			}
		}

		// Token: 0x040006A9 RID: 1705
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _pointDuration = 0.5f;

		// Token: 0x040006AA RID: 1706
		[Nullable(1)]
		private Node2D _target;

		// Token: 0x040006AB RID: 1707
		[Nullable(1)]
		private readonly List<float> _pointAge = new List<float>();

		// Token: 0x040006AC RID: 1708
		private const float _minSpawnDist = 12f;

		// Token: 0x040006AD RID: 1709
		private const float _maxSpawnDist = 48f;

		// Token: 0x040006AE RID: 1710
		private Vector2? _lastPointPosition;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F9F RID: 3999
		public class MethodName : Line2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E52 RID: 15954
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04003E53 RID: 15955
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'OnToggleVisibility' method.
			/// </summary>
			// Token: 0x04003E54 RID: 15956
			public static readonly StringName OnToggleVisibility = "OnToggleVisibility";

			/// <summary>
			/// Cached name for the 'CreatePoint' method.
			/// </summary>
			// Token: 0x04003E55 RID: 15957
			public static readonly StringName CreatePoint = "CreatePoint";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FA0 RID: 4000
		public class PropertyName : Line2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_pointDuration' field.
			/// </summary>
			// Token: 0x04003E56 RID: 15958
			public static readonly StringName _pointDuration = "_pointDuration";

			/// <summary>
			/// Cached name for the '_target' field.
			/// </summary>
			// Token: 0x04003E57 RID: 15959
			public static readonly StringName _target = "_target";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FA1 RID: 4001
		public class SignalName : Line2D.SignalName
		{
		}
	}
}
