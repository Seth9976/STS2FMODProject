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
	// Token: 0x020001BD RID: 445
	[ScriptPath("res://src/Core/Nodes/Vfx/NHunterKillerVfx.cs")]
	public class NHunterKillerVfx : Node
	{
		// Token: 0x0600162F RID: 5679 RVA: 0x00087894 File Offset: 0x00085A94
		public override void _Ready()
		{
			this._particles = base.GetNode<GpuParticles2D>("../MouthBone/Particles");
			this._particles.Emitting = false;
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._megaSprite.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x00087914 File Offset: 0x00085B14
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "spit_start")
			{
				this.OnParticlesStart();
				return;
			}
			if (!(eventName == "spit_end"))
			{
				return;
			}
			this.OnParticlesEnd();
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x00087960 File Offset: 0x00085B60
		[NullableContext(1)]
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			if (new MegaAnimationState(animationState).GetCurrentAnimationName(0) != "cast")
			{
				this.OnParticlesEnd();
			}
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00087985 File Offset: 0x00085B85
		private void OnParticlesStart()
		{
			this._particles.Emitting = true;
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00087993 File Offset: 0x00085B93
		private void OnParticlesEnd()
		{
			this._particles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001634 RID: 5684 RVA: 0x000879A4 File Offset: 0x00085BA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NHunterKillerVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHunterKillerVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NHunterKillerVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NHunterKillerVfx.MethodName.OnParticlesStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHunterKillerVfx.MethodName.OnParticlesEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001635 RID: 5685 RVA: 0x00087BE0 File Offset: 0x00085DE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHunterKillerVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHunterKillerVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHunterKillerVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHunterKillerVfx.MethodName.OnParticlesStart && args.Count == 0)
			{
				this.OnParticlesStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHunterKillerVfx.MethodName.OnParticlesEnd && args.Count == 0)
			{
				this.OnParticlesEnd();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001636 RID: 5686 RVA: 0x00087D0C File Offset: 0x00085F0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHunterKillerVfx.MethodName._Ready || (ref method) == NHunterKillerVfx.MethodName.OnAnimationEvent || (ref method) == NHunterKillerVfx.MethodName.OnAnimationStart || (ref method) == NHunterKillerVfx.MethodName.OnParticlesStart || (ref method) == NHunterKillerVfx.MethodName.OnParticlesEnd || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001637 RID: 5687 RVA: 0x00087D6B File Offset: 0x00085F6B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHunterKillerVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001638 RID: 5688 RVA: 0x00087D90 File Offset: 0x00085F90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHunterKillerVfx.PropertyName._particles)
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
		// Token: 0x06001639 RID: 5689 RVA: 0x00087DBC File Offset: 0x00085FBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NHunterKillerVfx.PropertyName._particles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600163A RID: 5690 RVA: 0x00087DF1 File Offset: 0x00085FF1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHunterKillerVfx.PropertyName._particles, Variant.From<GpuParticles2D>(ref this._particles));
		}

		/// <inheritdoc />
		// Token: 0x0600163B RID: 5691 RVA: 0x00087E10 File Offset: 0x00086010
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHunterKillerVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.As<GpuParticles2D>();
			}
		}

		// Token: 0x040007E1 RID: 2017
		[Nullable(1)]
		private MegaSprite _megaSprite;

		// Token: 0x040007E2 RID: 2018
		[Nullable(1)]
		private GpuParticles2D _particles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001072 RID: 4210
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040040B8 RID: 16568
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040040B9 RID: 16569
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x040040BA RID: 16570
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'OnParticlesStart' method.
			/// </summary>
			// Token: 0x040040BB RID: 16571
			public static readonly StringName OnParticlesStart = "OnParticlesStart";

			/// <summary>
			/// Cached name for the 'OnParticlesEnd' method.
			/// </summary>
			// Token: 0x040040BC RID: 16572
			public static readonly StringName OnParticlesEnd = "OnParticlesEnd";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001073 RID: 4211
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x040040BD RID: 16573
			public static readonly StringName _particles = "_particles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001074 RID: 4212
		public class SignalName : Node.SignalName
		{
		}
	}
}
