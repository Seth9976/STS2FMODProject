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
	// Token: 0x020001AC RID: 428
	[ScriptPath("res://src/Core/Nodes/Vfx/NFogmogVfx.cs")]
	public class NFogmogVfx : Node
	{
		// Token: 0x06001538 RID: 5432 RVA: 0x00081A14 File Offset: 0x0007FC14
		public override void _Ready()
		{
			this._thrustParicles = base.GetNode<GpuParticles2D>("../ThrustSlotNode/ThrustParticles");
			this._dustLeftParticles = base.GetNode<GpuParticles2D>("../DustSlotNode/DustLeftParticles");
			this._dustRightParticles = base.GetNode<GpuParticles2D>("../DustSlotNode/DustRightParticles");
			this._thrustParicles.Emitting = false;
			this._dustLeftParticles.Emitting = false;
			this._dustRightParticles.Emitting = false;
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x00081ABC File Offset: 0x0007FCBC
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "thrust_start")
			{
				this.StartThrust();
				return;
			}
			if (!(eventName == "thrust_end"))
			{
				return;
			}
			this.EndThrust();
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x00081B08 File Offset: 0x0007FD08
		private void StartThrust()
		{
			this._thrustParicles.Restart();
			this._dustRightParticles.Restart();
			this._dustLeftParticles.Restart();
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x00081B2B File Offset: 0x0007FD2B
		private void EndThrust()
		{
			this._thrustParicles.Emitting = false;
			this._dustLeftParticles.Emitting = false;
			this._dustRightParticles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600153C RID: 5436 RVA: 0x00081B54 File Offset: 0x0007FD54
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NFogmogVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFogmogVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NFogmogVfx.MethodName.StartThrust, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFogmogVfx.MethodName.EndThrust, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600153D RID: 5437 RVA: 0x00081CDC File Offset: 0x0007FEDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFogmogVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFogmogVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFogmogVfx.MethodName.StartThrust && args.Count == 0)
			{
				this.StartThrust();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFogmogVfx.MethodName.EndThrust && args.Count == 0)
			{
				this.EndThrust();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600153E RID: 5438 RVA: 0x00081DBC File Offset: 0x0007FFBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFogmogVfx.MethodName._Ready || (ref method) == NFogmogVfx.MethodName.OnAnimationEvent || (ref method) == NFogmogVfx.MethodName.StartThrust || (ref method) == NFogmogVfx.MethodName.EndThrust || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600153F RID: 5439 RVA: 0x00081E0C File Offset: 0x0008000C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFogmogVfx.PropertyName._thrustParicles)
			{
				this._thrustParicles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NFogmogVfx.PropertyName._dustLeftParticles)
			{
				this._dustLeftParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NFogmogVfx.PropertyName._dustRightParticles)
			{
				this._dustRightParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001540 RID: 5440 RVA: 0x00081E74 File Offset: 0x00080074
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFogmogVfx.PropertyName._thrustParicles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._thrustParicles);
				return true;
			}
			if ((ref name) == NFogmogVfx.PropertyName._dustLeftParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dustLeftParticles);
				return true;
			}
			if ((ref name) == NFogmogVfx.PropertyName._dustRightParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dustRightParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001541 RID: 5441 RVA: 0x00081EEC File Offset: 0x000800EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFogmogVfx.PropertyName._thrustParicles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFogmogVfx.PropertyName._dustLeftParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFogmogVfx.PropertyName._dustRightParticles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001542 RID: 5442 RVA: 0x00081F64 File Offset: 0x00080164
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFogmogVfx.PropertyName._thrustParicles, Variant.From<GpuParticles2D>(ref this._thrustParicles));
			info.AddProperty(NFogmogVfx.PropertyName._dustLeftParticles, Variant.From<GpuParticles2D>(ref this._dustLeftParticles));
			info.AddProperty(NFogmogVfx.PropertyName._dustRightParticles, Variant.From<GpuParticles2D>(ref this._dustRightParticles));
		}

		/// <inheritdoc />
		// Token: 0x06001543 RID: 5443 RVA: 0x00081FBC File Offset: 0x000801BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFogmogVfx.PropertyName._thrustParicles, ref variant))
			{
				this._thrustParicles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFogmogVfx.PropertyName._dustLeftParticles, ref variant2))
			{
				this._dustLeftParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NFogmogVfx.PropertyName._dustRightParticles, ref variant3))
			{
				this._dustRightParticles = variant3.As<GpuParticles2D>();
			}
		}

		// Token: 0x04000792 RID: 1938
		[Nullable(1)]
		private GpuParticles2D _thrustParicles;

		// Token: 0x04000793 RID: 1939
		[Nullable(1)]
		private GpuParticles2D _dustLeftParticles;

		// Token: 0x04000794 RID: 1940
		[Nullable(1)]
		private GpuParticles2D _dustRightParticles;

		// Token: 0x04000795 RID: 1941
		[Nullable(1)]
		private MegaSprite _megaSprite;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001033 RID: 4147
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004019 RID: 16409
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x0400401A RID: 16410
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartThrust' method.
			/// </summary>
			// Token: 0x0400401B RID: 16411
			public static readonly StringName StartThrust = "StartThrust";

			/// <summary>
			/// Cached name for the 'EndThrust' method.
			/// </summary>
			// Token: 0x0400401C RID: 16412
			public static readonly StringName EndThrust = "EndThrust";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001034 RID: 4148
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_thrustParicles' field.
			/// </summary>
			// Token: 0x0400401D RID: 16413
			public static readonly StringName _thrustParicles = "_thrustParicles";

			/// <summary>
			/// Cached name for the '_dustLeftParticles' field.
			/// </summary>
			// Token: 0x0400401E RID: 16414
			public static readonly StringName _dustLeftParticles = "_dustLeftParticles";

			/// <summary>
			/// Cached name for the '_dustRightParticles' field.
			/// </summary>
			// Token: 0x0400401F RID: 16415
			public static readonly StringName _dustRightParticles = "_dustRightParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001035 RID: 4149
		public class SignalName : Node.SignalName
		{
		}
	}
}
