using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	// Token: 0x0200021C RID: 540
	[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NTrail2D.cs")]
	public class NTrail2D : Line2D
	{
		// Token: 0x06001BA5 RID: 7077 RVA: 0x000B3CBE File Offset: 0x000B1EBE
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnToggleVisibility)), 0U);
			this.OnToggleVisibility();
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x000B3CF0 File Offset: 0x000B1EF0
		private void OnToggleVisibility()
		{
			this._isActive = base.Visible;
			if (!base.Visible)
			{
				this._pointQueue.Clear();
				base.Points = this._pointQueue.ToArray();
			}
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x000B3D24 File Offset: 0x000B1F24
		public override void _Process(double delta)
		{
			if (!this._isActive)
			{
				return;
			}
			this._pointQueue.Insert(0, this._parent.GlobalPosition);
			if (this._pointQueue.Count >= this._maxSegments)
			{
				this._pointQueue.RemoveAt(this._pointQueue.Count - 1);
			}
			base.Points = this._pointQueue.Select<Vector2, Vector2>((Vector2 point) => base.GetParent<Node2D>().ToLocal(point)).ToArray<Vector2>();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BA8 RID: 7080 RVA: 0x000B3DA0 File Offset: 0x000B1FA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NTrail2D.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTrail2D.MethodName.OnToggleVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTrail2D.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001BA9 RID: 7081 RVA: 0x000B3E68 File Offset: 0x000B2068
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTrail2D.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTrail2D.MethodName.OnToggleVisibility && args.Count == 0)
			{
				this.OnToggleVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTrail2D.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001BAA RID: 7082 RVA: 0x000B3EFB File Offset: 0x000B20FB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTrail2D.MethodName._Ready || (ref method) == NTrail2D.MethodName.OnToggleVisibility || (ref method) == NTrail2D.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001BAB RID: 7083 RVA: 0x000B3F34 File Offset: 0x000B2134
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTrail2D.PropertyName._maxSegments)
			{
				this._maxSegments = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NTrail2D.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NTrail2D.PropertyName._isActive)
			{
				this._isActive = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001BAC RID: 7084 RVA: 0x000B3F9C File Offset: 0x000B219C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTrail2D.PropertyName._maxSegments)
			{
				value = VariantUtils.CreateFrom<int>(ref this._maxSegments);
				return true;
			}
			if ((ref name) == NTrail2D.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			if ((ref name) == NTrail2D.PropertyName._isActive)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isActive);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BAD RID: 7085 RVA: 0x000B4014 File Offset: 0x000B2214
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NTrail2D.PropertyName._maxSegments, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTrail2D.PropertyName._parent, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTrail2D.PropertyName._isActive, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001BAE RID: 7086 RVA: 0x000B408C File Offset: 0x000B228C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTrail2D.PropertyName._maxSegments, Variant.From<int>(ref this._maxSegments));
			info.AddProperty(NTrail2D.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
			info.AddProperty(NTrail2D.PropertyName._isActive, Variant.From<bool>(ref this._isActive));
		}

		/// <inheritdoc />
		// Token: 0x06001BAF RID: 7087 RVA: 0x000B40E4 File Offset: 0x000B22E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTrail2D.PropertyName._maxSegments, ref variant))
			{
				this._maxSegments = variant.As<int>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTrail2D.PropertyName._parent, ref variant2))
			{
				this._parent = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTrail2D.PropertyName._isActive, ref variant3))
			{
				this._isActive = variant3.As<bool>();
			}
		}

		// Token: 0x04000A73 RID: 2675
		private int _maxSegments = 20;

		// Token: 0x04000A74 RID: 2676
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000A75 RID: 2677
		[Nullable(1)]
		private readonly List<Vector2> _pointQueue = new List<Vector2>();

		// Token: 0x04000A76 RID: 2678
		private bool _isActive;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011BD RID: 4541
		public class MethodName : Line2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004514 RID: 17684
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnToggleVisibility' method.
			/// </summary>
			// Token: 0x04004515 RID: 17685
			public static readonly StringName OnToggleVisibility = "OnToggleVisibility";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004516 RID: 17686
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011BE RID: 4542
		public class PropertyName : Line2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_maxSegments' field.
			/// </summary>
			// Token: 0x04004517 RID: 17687
			public static readonly StringName _maxSegments = "_maxSegments";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004518 RID: 17688
			public static readonly StringName _parent = "_parent";

			/// <summary>
			/// Cached name for the '_isActive' field.
			/// </summary>
			// Token: 0x04004519 RID: 17689
			public static readonly StringName _isActive = "_isActive";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011BF RID: 4543
		public class SignalName : Line2D.SignalName
		{
		}
	}
}
