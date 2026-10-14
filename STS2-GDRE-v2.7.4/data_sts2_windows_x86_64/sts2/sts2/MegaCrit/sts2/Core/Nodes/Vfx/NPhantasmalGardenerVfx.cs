using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001DA RID: 474
	[ScriptPath("res://src/Core/Nodes/Vfx/NPhantasmalGardenerVfx.cs")]
	public class NPhantasmalGardenerVfx : Node
	{
		// Token: 0x060017F6 RID: 6134 RVA: 0x00095D7C File Offset: 0x00093F7C
		public override void _Ready()
		{
			this._spewParticles = base.GetNode<GpuParticles2D>("../SpewSlotNode/SpewParticles");
			this._spewParticles.Emitting = false;
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00095DE0 File Offset: 0x00093FE0
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "spew_start")
			{
				this.OnSpewStart();
				return;
			}
			if (!(eventName == "spew_end"))
			{
				return;
			}
			this.OnSpewEnd();
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x00095E2C File Offset: 0x0009402C
		private void OnSpewStart()
		{
			this._spewParticles.Emitting = true;
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00095E3A File Offset: 0x0009403A
		private void OnSpewEnd()
		{
			this._spewParticles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060017FA RID: 6138 RVA: 0x00095E48 File Offset: 0x00094048
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NPhantasmalGardenerVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPhantasmalGardenerVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NPhantasmalGardenerVfx.MethodName.OnSpewStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPhantasmalGardenerVfx.MethodName.OnSpewEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060017FB RID: 6139 RVA: 0x00095FD0 File Offset: 0x000941D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPhantasmalGardenerVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPhantasmalGardenerVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPhantasmalGardenerVfx.MethodName.OnSpewStart && args.Count == 0)
			{
				this.OnSpewStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPhantasmalGardenerVfx.MethodName.OnSpewEnd && args.Count == 0)
			{
				this.OnSpewEnd();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060017FC RID: 6140 RVA: 0x000960B0 File Offset: 0x000942B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPhantasmalGardenerVfx.MethodName._Ready || (ref method) == NPhantasmalGardenerVfx.MethodName.OnAnimationEvent || (ref method) == NPhantasmalGardenerVfx.MethodName.OnSpewStart || (ref method) == NPhantasmalGardenerVfx.MethodName.OnSpewEnd || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060017FD RID: 6141 RVA: 0x00096100 File Offset: 0x00094300
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPhantasmalGardenerVfx.PropertyName._spewParticles)
			{
				this._spewParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060017FE RID: 6142 RVA: 0x00096125 File Offset: 0x00094325
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPhantasmalGardenerVfx.PropertyName._spewParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spewParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060017FF RID: 6143 RVA: 0x00096150 File Offset: 0x00094350
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPhantasmalGardenerVfx.PropertyName._spewParticles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001800 RID: 6144 RVA: 0x00096185 File Offset: 0x00094385
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPhantasmalGardenerVfx.PropertyName._spewParticles, Variant.From<GpuParticles2D>(ref this._spewParticles));
		}

		/// <inheritdoc />
		// Token: 0x06001801 RID: 6145 RVA: 0x000961A4 File Offset: 0x000943A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPhantasmalGardenerVfx.PropertyName._spewParticles, ref variant))
			{
				this._spewParticles = variant.As<GpuParticles2D>();
			}
		}

		// Token: 0x040008A8 RID: 2216
		[Nullable(1)]
		private MegaSprite _megaSprite;

		// Token: 0x040008A9 RID: 2217
		[Nullable(1)]
		private GpuParticles2D _spewParticles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010DC RID: 4316
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004219 RID: 16921
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x0400421A RID: 16922
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnSpewStart' method.
			/// </summary>
			// Token: 0x0400421B RID: 16923
			public static readonly StringName OnSpewStart = "OnSpewStart";

			/// <summary>
			/// Cached name for the 'OnSpewEnd' method.
			/// </summary>
			// Token: 0x0400421C RID: 16924
			public static readonly StringName OnSpewEnd = "OnSpewEnd";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010DD RID: 4317
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_spewParticles' field.
			/// </summary>
			// Token: 0x0400421D RID: 16925
			public static readonly StringName _spewParticles = "_spewParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010DE RID: 4318
		public class SignalName : Node.SignalName
		{
		}
	}
}
