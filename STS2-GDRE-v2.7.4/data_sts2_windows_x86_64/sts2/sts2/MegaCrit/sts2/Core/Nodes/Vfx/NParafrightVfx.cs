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
	// Token: 0x020001D9 RID: 473
	[ScriptPath("res://src/Core/Nodes/Vfx/NParafrightVfx.cs")]
	public class NParafrightVfx : Node
	{
		// Token: 0x060017E9 RID: 6121 RVA: 0x0009591C File Offset: 0x00093B1C
		public override void _Ready()
		{
			this._particles = base.GetNode<GpuParticles2D>("../ParticlesSlot/Particles");
			this._particles.Emitting = false;
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00095980 File Offset: 0x00093B80
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

		// Token: 0x060017EB RID: 6123 RVA: 0x000959CC File Offset: 0x00093BCC
		private void OnParticlesStart()
		{
			this._particles.Emitting = true;
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x000959DA File Offset: 0x00093BDA
		private void OnParticlesEnd()
		{
			this._particles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060017ED RID: 6125 RVA: 0x000959E8 File Offset: 0x00093BE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NParafrightVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NParafrightVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NParafrightVfx.MethodName.OnParticlesStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NParafrightVfx.MethodName.OnParticlesEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060017EE RID: 6126 RVA: 0x00095B70 File Offset: 0x00093D70
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NParafrightVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NParafrightVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NParafrightVfx.MethodName.OnParticlesStart && args.Count == 0)
			{
				this.OnParticlesStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NParafrightVfx.MethodName.OnParticlesEnd && args.Count == 0)
			{
				this.OnParticlesEnd();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060017EF RID: 6127 RVA: 0x00095C50 File Offset: 0x00093E50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NParafrightVfx.MethodName._Ready || (ref method) == NParafrightVfx.MethodName.OnAnimationEvent || (ref method) == NParafrightVfx.MethodName.OnParticlesStart || (ref method) == NParafrightVfx.MethodName.OnParticlesEnd || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060017F0 RID: 6128 RVA: 0x00095CA0 File Offset: 0x00093EA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NParafrightVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060017F1 RID: 6129 RVA: 0x00095CC5 File Offset: 0x00093EC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NParafrightVfx.PropertyName._particles)
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
		// Token: 0x060017F2 RID: 6130 RVA: 0x00095CF0 File Offset: 0x00093EF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NParafrightVfx.PropertyName._particles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060017F3 RID: 6131 RVA: 0x00095D25 File Offset: 0x00093F25
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NParafrightVfx.PropertyName._particles, Variant.From<GpuParticles2D>(ref this._particles));
		}

		/// <inheritdoc />
		// Token: 0x060017F4 RID: 6132 RVA: 0x00095D44 File Offset: 0x00093F44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NParafrightVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.As<GpuParticles2D>();
			}
		}

		// Token: 0x040008A6 RID: 2214
		[Nullable(1)]
		private MegaSprite _megaSprite;

		// Token: 0x040008A7 RID: 2215
		[Nullable(1)]
		private GpuParticles2D _particles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010D9 RID: 4313
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004214 RID: 16916
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004215 RID: 16917
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnParticlesStart' method.
			/// </summary>
			// Token: 0x04004216 RID: 16918
			public static readonly StringName OnParticlesStart = "OnParticlesStart";

			/// <summary>
			/// Cached name for the 'OnParticlesEnd' method.
			/// </summary>
			// Token: 0x04004217 RID: 16919
			public static readonly StringName OnParticlesEnd = "OnParticlesEnd";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010DA RID: 4314
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x04004218 RID: 16920
			public static readonly StringName _particles = "_particles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010DB RID: 4315
		public class SignalName : Node.SignalName
		{
		}
	}
}
