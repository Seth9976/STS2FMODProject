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
	// Token: 0x020001F8 RID: 504
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NSpectralKnightVfx.cs")]
	public class NSpectralKnightVfx : Node
	{
		// Token: 0x060019DE RID: 6622 RVA: 0x000A5330 File Offset: 0x000A3530
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._flameParticlesAdd = this._parent.GetNode<GpuParticles2D>("FlameParticlesAdd");
			this._flameParticlesFlat = this._parent.GetNode<GpuParticles2D>("FlameParticlesFlat");
			this._cinderParticles = this._parent.GetNode<GpuParticles2D>("CinderParticles");
			this.TurnOffFire();
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x000A53F0 File Offset: 0x000A35F0
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "flame_start")
			{
				this.TurnOnFire();
				return;
			}
			if (!(eventName == "flame_end"))
			{
				return;
			}
			this.TurnOffFire();
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x000A543C File Offset: 0x000A363C
		[NullableContext(1)]
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			if (new MegaAnimationState(animationState).GetCurrentAnimationName(0) != "attack")
			{
				this.TurnOffFire();
			}
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x000A5461 File Offset: 0x000A3661
		private void TurnOnFire()
		{
			this._flameParticlesAdd.Restart();
			this._flameParticlesFlat.Restart();
			this._cinderParticles.Restart();
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x000A5484 File Offset: 0x000A3684
		private void TurnOffFire()
		{
			this._flameParticlesAdd.Emitting = false;
			this._flameParticlesFlat.Emitting = false;
			this._cinderParticles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060019E3 RID: 6627 RVA: 0x000A54AC File Offset: 0x000A36AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NSpectralKnightVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSpectralKnightVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NSpectralKnightVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NSpectralKnightVfx.MethodName.TurnOnFire, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSpectralKnightVfx.MethodName.TurnOffFire, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060019E4 RID: 6628 RVA: 0x000A56E8 File Offset: 0x000A38E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSpectralKnightVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpectralKnightVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpectralKnightVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpectralKnightVfx.MethodName.TurnOnFire && args.Count == 0)
			{
				this.TurnOnFire();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpectralKnightVfx.MethodName.TurnOffFire && args.Count == 0)
			{
				this.TurnOffFire();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060019E5 RID: 6629 RVA: 0x000A5814 File Offset: 0x000A3A14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSpectralKnightVfx.MethodName._Ready || (ref method) == NSpectralKnightVfx.MethodName.OnAnimationEvent || (ref method) == NSpectralKnightVfx.MethodName.OnAnimationStart || (ref method) == NSpectralKnightVfx.MethodName.TurnOnFire || (ref method) == NSpectralKnightVfx.MethodName.TurnOffFire || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060019E6 RID: 6630 RVA: 0x000A5874 File Offset: 0x000A3A74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSpectralKnightVfx.PropertyName._flameParticlesFlat)
			{
				this._flameParticlesFlat = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSpectralKnightVfx.PropertyName._flameParticlesAdd)
			{
				this._flameParticlesAdd = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSpectralKnightVfx.PropertyName._cinderParticles)
			{
				this._cinderParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSpectralKnightVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060019E7 RID: 6631 RVA: 0x000A58F8 File Offset: 0x000A3AF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSpectralKnightVfx.PropertyName._flameParticlesFlat)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameParticlesFlat);
				return true;
			}
			if ((ref name) == NSpectralKnightVfx.PropertyName._flameParticlesAdd)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameParticlesAdd);
				return true;
			}
			if ((ref name) == NSpectralKnightVfx.PropertyName._cinderParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._cinderParticles);
				return true;
			}
			if ((ref name) == NSpectralKnightVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060019E8 RID: 6632 RVA: 0x000A5990 File Offset: 0x000A3B90
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSpectralKnightVfx.PropertyName._flameParticlesFlat, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSpectralKnightVfx.PropertyName._flameParticlesAdd, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSpectralKnightVfx.PropertyName._cinderParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSpectralKnightVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060019E9 RID: 6633 RVA: 0x000A5A28 File Offset: 0x000A3C28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSpectralKnightVfx.PropertyName._flameParticlesFlat, Variant.From<GpuParticles2D>(ref this._flameParticlesFlat));
			info.AddProperty(NSpectralKnightVfx.PropertyName._flameParticlesAdd, Variant.From<GpuParticles2D>(ref this._flameParticlesAdd));
			info.AddProperty(NSpectralKnightVfx.PropertyName._cinderParticles, Variant.From<GpuParticles2D>(ref this._cinderParticles));
			info.AddProperty(NSpectralKnightVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x060019EA RID: 6634 RVA: 0x000A5A94 File Offset: 0x000A3C94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSpectralKnightVfx.PropertyName._flameParticlesFlat, ref variant))
			{
				this._flameParticlesFlat = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSpectralKnightVfx.PropertyName._flameParticlesAdd, ref variant2))
			{
				this._flameParticlesAdd = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSpectralKnightVfx.PropertyName._cinderParticles, ref variant3))
			{
				this._cinderParticles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSpectralKnightVfx.PropertyName._parent, ref variant4))
			{
				this._parent = variant4.As<Node2D>();
			}
		}

		// Token: 0x04000990 RID: 2448
		[Nullable(1)]
		private GpuParticles2D _flameParticlesFlat;

		// Token: 0x04000991 RID: 2449
		[Nullable(1)]
		private GpuParticles2D _flameParticlesAdd;

		// Token: 0x04000992 RID: 2450
		[Nullable(1)]
		private GpuParticles2D _cinderParticles;

		// Token: 0x04000993 RID: 2451
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000994 RID: 2452
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200114D RID: 4429
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004399 RID: 17305
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x0400439A RID: 17306
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x0400439B RID: 17307
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'TurnOnFire' method.
			/// </summary>
			// Token: 0x0400439C RID: 17308
			public static readonly StringName TurnOnFire = "TurnOnFire";

			/// <summary>
			/// Cached name for the 'TurnOffFire' method.
			/// </summary>
			// Token: 0x0400439D RID: 17309
			public static readonly StringName TurnOffFire = "TurnOffFire";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200114E RID: 4430
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_flameParticlesFlat' field.
			/// </summary>
			// Token: 0x0400439E RID: 17310
			public static readonly StringName _flameParticlesFlat = "_flameParticlesFlat";

			/// <summary>
			/// Cached name for the '_flameParticlesAdd' field.
			/// </summary>
			// Token: 0x0400439F RID: 17311
			public static readonly StringName _flameParticlesAdd = "_flameParticlesAdd";

			/// <summary>
			/// Cached name for the '_cinderParticles' field.
			/// </summary>
			// Token: 0x040043A0 RID: 17312
			public static readonly StringName _cinderParticles = "_cinderParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x040043A1 RID: 17313
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200114F RID: 4431
		public class SignalName : Node.SignalName
		{
		}
	}
}
