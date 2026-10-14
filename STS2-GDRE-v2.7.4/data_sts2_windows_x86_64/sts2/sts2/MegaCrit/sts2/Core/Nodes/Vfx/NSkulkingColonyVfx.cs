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
	// Token: 0x020001EF RID: 495
	[ScriptPath("res://src/Core/Nodes/Vfx/NSkulkingColonyVfx.cs")]
	public class NSkulkingColonyVfx : Node2D
	{
		// Token: 0x0600193B RID: 6459 RVA: 0x0009E7F0 File Offset: 0x0009C9F0
		public override void _Ready()
		{
			this._particles1 = base.GetNode<GpuParticles2D>("../ParticleSlot1/Particles");
			this._wideParticles = base.GetNode<GpuParticles2D>("../ParticleSlot2/ParticlesWide");
			this._poofParticles = base.GetNode<GpuParticles2D>("../ParticleSlot3/Particles");
			this._particles1.Emitting = false;
			this._poofParticles.Emitting = false;
			this._wideParticles.Emitting = false;
			this._particles1.OneShot = true;
			this._poofParticles.OneShot = true;
			this._wideParticles.OneShot = true;
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x0009E8BC File Offset: 0x0009CABC
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "take_damage")
			{
				this.DamageHandler();
				return;
			}
			if (eventName == "take_fatal_damage")
			{
				this.DeathHandler();
				return;
			}
			if (!(eventName == "final_poof"))
			{
				return;
			}
			this.PoofHandler();
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x0009E91C File Offset: 0x0009CB1C
		private void DamageHandler()
		{
			this._particles1.Restart();
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0009E929 File Offset: 0x0009CB29
		private void DeathHandler()
		{
			this._wideParticles.Restart();
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0009E936 File Offset: 0x0009CB36
		private void PoofHandler()
		{
			this._poofParticles.Restart();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001940 RID: 6464 RVA: 0x0009E944 File Offset: 0x0009CB44
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NSkulkingColonyVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSkulkingColonyVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NSkulkingColonyVfx.MethodName.DamageHandler, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSkulkingColonyVfx.MethodName.DeathHandler, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSkulkingColonyVfx.MethodName.PoofHandler, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001941 RID: 6465 RVA: 0x0009EAF8 File Offset: 0x0009CCF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSkulkingColonyVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSkulkingColonyVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSkulkingColonyVfx.MethodName.DamageHandler && args.Count == 0)
			{
				this.DamageHandler();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSkulkingColonyVfx.MethodName.DeathHandler && args.Count == 0)
			{
				this.DeathHandler();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSkulkingColonyVfx.MethodName.PoofHandler && args.Count == 0)
			{
				this.PoofHandler();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001942 RID: 6466 RVA: 0x0009EBFC File Offset: 0x0009CDFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSkulkingColonyVfx.MethodName._Ready || (ref method) == NSkulkingColonyVfx.MethodName.OnAnimationEvent || (ref method) == NSkulkingColonyVfx.MethodName.DamageHandler || (ref method) == NSkulkingColonyVfx.MethodName.DeathHandler || (ref method) == NSkulkingColonyVfx.MethodName.PoofHandler || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001943 RID: 6467 RVA: 0x0009EC5C File Offset: 0x0009CE5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSkulkingColonyVfx.PropertyName._particles1)
			{
				this._particles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSkulkingColonyVfx.PropertyName._wideParticles)
			{
				this._wideParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSkulkingColonyVfx.PropertyName._poofParticles)
			{
				this._poofParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001944 RID: 6468 RVA: 0x0009ECC4 File Offset: 0x0009CEC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSkulkingColonyVfx.PropertyName._particles1)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._particles1);
				return true;
			}
			if ((ref name) == NSkulkingColonyVfx.PropertyName._wideParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._wideParticles);
				return true;
			}
			if ((ref name) == NSkulkingColonyVfx.PropertyName._poofParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._poofParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001945 RID: 6469 RVA: 0x0009ED3C File Offset: 0x0009CF3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSkulkingColonyVfx.PropertyName._particles1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSkulkingColonyVfx.PropertyName._wideParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSkulkingColonyVfx.PropertyName._poofParticles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001946 RID: 6470 RVA: 0x0009EDB4 File Offset: 0x0009CFB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSkulkingColonyVfx.PropertyName._particles1, Variant.From<GpuParticles2D>(ref this._particles1));
			info.AddProperty(NSkulkingColonyVfx.PropertyName._wideParticles, Variant.From<GpuParticles2D>(ref this._wideParticles));
			info.AddProperty(NSkulkingColonyVfx.PropertyName._poofParticles, Variant.From<GpuParticles2D>(ref this._poofParticles));
		}

		/// <inheritdoc />
		// Token: 0x06001947 RID: 6471 RVA: 0x0009EE0C File Offset: 0x0009D00C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSkulkingColonyVfx.PropertyName._particles1, ref variant))
			{
				this._particles1 = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSkulkingColonyVfx.PropertyName._wideParticles, ref variant2))
			{
				this._wideParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSkulkingColonyVfx.PropertyName._poofParticles, ref variant3))
			{
				this._poofParticles = variant3.As<GpuParticles2D>();
			}
		}

		// Token: 0x04000930 RID: 2352
		[Nullable(1)]
		private MegaSprite _megaSprite;

		// Token: 0x04000931 RID: 2353
		[Nullable(1)]
		private GpuParticles2D _particles1;

		// Token: 0x04000932 RID: 2354
		[Nullable(1)]
		private GpuParticles2D _wideParticles;

		// Token: 0x04000933 RID: 2355
		[Nullable(1)]
		private GpuParticles2D _poofParticles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200112B RID: 4395
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040042FD RID: 17149
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040042FE RID: 17150
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'DamageHandler' method.
			/// </summary>
			// Token: 0x040042FF RID: 17151
			public static readonly StringName DamageHandler = "DamageHandler";

			/// <summary>
			/// Cached name for the 'DeathHandler' method.
			/// </summary>
			// Token: 0x04004300 RID: 17152
			public static readonly StringName DeathHandler = "DeathHandler";

			/// <summary>
			/// Cached name for the 'PoofHandler' method.
			/// </summary>
			// Token: 0x04004301 RID: 17153
			public static readonly StringName PoofHandler = "PoofHandler";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200112C RID: 4396
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles1' field.
			/// </summary>
			// Token: 0x04004302 RID: 17154
			public static readonly StringName _particles1 = "_particles1";

			/// <summary>
			/// Cached name for the '_wideParticles' field.
			/// </summary>
			// Token: 0x04004303 RID: 17155
			public static readonly StringName _wideParticles = "_wideParticles";

			/// <summary>
			/// Cached name for the '_poofParticles' field.
			/// </summary>
			// Token: 0x04004304 RID: 17156
			public static readonly StringName _poofParticles = "_poofParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200112D RID: 4397
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
