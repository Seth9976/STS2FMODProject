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
	// Token: 0x02000206 RID: 518
	[ScriptPath("res://src/Core/Nodes/Vfx/NTheObscuraVfx.cs")]
	public class NTheObscuraVfx : Node
	{
		// Token: 0x06001AC0 RID: 6848 RVA: 0x000AC2C8 File Offset: 0x000AA4C8
		public override void _Ready()
		{
			this._particles = base.GetNode<GpuParticles2D>("../ParticlesSlot/Particles");
			this._particles.Emitting = false;
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x000AC32C File Offset: 0x000AA52C
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

		// Token: 0x06001AC2 RID: 6850 RVA: 0x000AC378 File Offset: 0x000AA578
		private void OnParticlesStart()
		{
			this._particles.Emitting = true;
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x000AC386 File Offset: 0x000AA586
		private void OnParticlesEnd()
		{
			this._particles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001AC4 RID: 6852 RVA: 0x000AC394 File Offset: 0x000AA594
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NTheObscuraVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTheObscuraVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NTheObscuraVfx.MethodName.OnParticlesStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTheObscuraVfx.MethodName.OnParticlesEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001AC5 RID: 6853 RVA: 0x000AC51C File Offset: 0x000AA71C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTheObscuraVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheObscuraVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheObscuraVfx.MethodName.OnParticlesStart && args.Count == 0)
			{
				this.OnParticlesStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheObscuraVfx.MethodName.OnParticlesEnd && args.Count == 0)
			{
				this.OnParticlesEnd();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001AC6 RID: 6854 RVA: 0x000AC5FC File Offset: 0x000AA7FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTheObscuraVfx.MethodName._Ready || (ref method) == NTheObscuraVfx.MethodName.OnAnimationEvent || (ref method) == NTheObscuraVfx.MethodName.OnParticlesStart || (ref method) == NTheObscuraVfx.MethodName.OnParticlesEnd || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001AC7 RID: 6855 RVA: 0x000AC64C File Offset: 0x000AA84C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTheObscuraVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001AC8 RID: 6856 RVA: 0x000AC671 File Offset: 0x000AA871
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTheObscuraVfx.PropertyName._particles)
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
		// Token: 0x06001AC9 RID: 6857 RVA: 0x000AC69C File Offset: 0x000AA89C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTheObscuraVfx.PropertyName._particles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001ACA RID: 6858 RVA: 0x000AC6D1 File Offset: 0x000AA8D1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTheObscuraVfx.PropertyName._particles, Variant.From<GpuParticles2D>(ref this._particles));
		}

		/// <inheritdoc />
		// Token: 0x06001ACB RID: 6859 RVA: 0x000AC6F0 File Offset: 0x000AA8F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTheObscuraVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.As<GpuParticles2D>();
			}
		}

		// Token: 0x040009EF RID: 2543
		[Nullable(1)]
		private MegaSprite _megaSprite;

		// Token: 0x040009F0 RID: 2544
		[Nullable(1)]
		private GpuParticles2D _particles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001184 RID: 4484
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400444F RID: 17487
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004450 RID: 17488
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnParticlesStart' method.
			/// </summary>
			// Token: 0x04004451 RID: 17489
			public static readonly StringName OnParticlesStart = "OnParticlesStart";

			/// <summary>
			/// Cached name for the 'OnParticlesEnd' method.
			/// </summary>
			// Token: 0x04004452 RID: 17490
			public static readonly StringName OnParticlesEnd = "OnParticlesEnd";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001185 RID: 4485
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x04004453 RID: 17491
			public static readonly StringName _particles = "_particles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001186 RID: 4486
		public class SignalName : Node.SignalName
		{
		}
	}
}
