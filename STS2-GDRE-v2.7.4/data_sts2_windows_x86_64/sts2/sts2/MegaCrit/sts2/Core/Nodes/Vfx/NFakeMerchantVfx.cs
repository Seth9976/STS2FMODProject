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
	// Token: 0x020001A6 RID: 422
	[ScriptPath("res://src/Core/Nodes/Vfx/NFakeMerchantVfx.cs")]
	public class NFakeMerchantVfx : Node
	{
		// Token: 0x060014E1 RID: 5345 RVA: 0x0007FA30 File Offset: 0x0007DC30
		public override void _Ready()
		{
			this._particles = base.GetNode<GpuParticles2D>("../ParticlesSlot/Particles");
			this._particles.Emitting = false;
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0007FA94 File Offset: 0x0007DC94
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "particles_start")
			{
				this.OnParticlesStart();
				return;
			}
			if (!(eventName == "particles_end"))
			{
				return;
			}
			this.OnParticlesEnd();
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0007FAE0 File Offset: 0x0007DCE0
		private void OnParticlesStart()
		{
			this._particles.Emitting = true;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0007FAEE File Offset: 0x0007DCEE
		private void OnParticlesEnd()
		{
			this._particles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014E5 RID: 5349 RVA: 0x0007FAFC File Offset: 0x0007DCFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NFakeMerchantVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFakeMerchantVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NFakeMerchantVfx.MethodName.OnParticlesStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFakeMerchantVfx.MethodName.OnParticlesEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014E6 RID: 5350 RVA: 0x0007FC84 File Offset: 0x0007DE84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFakeMerchantVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchantVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchantVfx.MethodName.OnParticlesStart && args.Count == 0)
			{
				this.OnParticlesStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFakeMerchantVfx.MethodName.OnParticlesEnd && args.Count == 0)
			{
				this.OnParticlesEnd();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060014E7 RID: 5351 RVA: 0x0007FD64 File Offset: 0x0007DF64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFakeMerchantVfx.MethodName._Ready || (ref method) == NFakeMerchantVfx.MethodName.OnAnimationEvent || (ref method) == NFakeMerchantVfx.MethodName.OnParticlesStart || (ref method) == NFakeMerchantVfx.MethodName.OnParticlesEnd || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060014E8 RID: 5352 RVA: 0x0007FDB4 File Offset: 0x0007DFB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFakeMerchantVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060014E9 RID: 5353 RVA: 0x0007FDD9 File Offset: 0x0007DFD9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFakeMerchantVfx.PropertyName._particles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._particles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014EA RID: 5354 RVA: 0x0007FE04 File Offset: 0x0007E004
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFakeMerchantVfx.PropertyName._particles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014EB RID: 5355 RVA: 0x0007FE39 File Offset: 0x0007E039
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFakeMerchantVfx.PropertyName._particles, Variant.From<GpuParticles2D>(ref this._particles));
		}

		/// <inheritdoc />
		// Token: 0x060014EC RID: 5356 RVA: 0x0007FE58 File Offset: 0x0007E058
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFakeMerchantVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.As<GpuParticles2D>();
			}
		}

		// Token: 0x04000780 RID: 1920
		[Nullable(1)]
		private MegaSprite _megaSprite;

		// Token: 0x04000781 RID: 1921
		[Nullable(1)]
		private GpuParticles2D _particles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200101E RID: 4126
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003FF0 RID: 16368
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04003FF1 RID: 16369
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnParticlesStart' method.
			/// </summary>
			// Token: 0x04003FF2 RID: 16370
			public static readonly StringName OnParticlesStart = "OnParticlesStart";

			/// <summary>
			/// Cached name for the 'OnParticlesEnd' method.
			/// </summary>
			// Token: 0x04003FF3 RID: 16371
			public static readonly StringName OnParticlesEnd = "OnParticlesEnd";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200101F RID: 4127
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x04003FF4 RID: 16372
			public static readonly StringName _particles = "_particles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001020 RID: 4128
		public class SignalName : Node.SignalName
		{
		}
	}
}
