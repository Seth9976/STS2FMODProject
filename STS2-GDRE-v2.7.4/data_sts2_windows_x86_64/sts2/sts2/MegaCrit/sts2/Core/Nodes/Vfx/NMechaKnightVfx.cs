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
	// Token: 0x020001D2 RID: 466
	[NullableContext(1)]
	[Nullable(0)]
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NMechaKnightVfx.cs")]
	public class NMechaKnightVfx : Node
	{
		// Token: 0x0600177E RID: 6014 RVA: 0x00091724 File Offset: 0x0008F924
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._flameThrowerParticlesDark = this._parent.GetNode<GpuParticles2D>("FlameParticlesBone/FlameParticlesDark");
			this._flameThrowerParticlesLight = this._parent.GetNode<GpuParticles2D>("FlameParticlesBone/FlameParticlesLight");
			this._cinderParticles = this._parent.GetNode<GpuParticles2D>("FlameParticlesBone/CinderParticles");
			this._glowParticles = this._parent.GetNode<GpuParticles2D>("FlameParticlesBone/GlowParticles");
			this._engineParticles = this._parent.GetNode<GpuParticles2D>("EngineSlot/EngineBone/EngineParticles");
			this._engineParticlesDark = this._parent.GetNode<GpuParticles2D>("EngineSlot/EngineBone/EngineParticlesDark");
			this.TurnOffFlameThrower();
			this.TurnOffEngine();
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x0009183C File Offset: 0x0008FA3C
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "flame_start")
			{
				this.TurnOnFlameThrower();
				return;
			}
			if (eventName == "flame_end")
			{
				this.TurnOffFlameThrower();
				return;
			}
			if (eventName == "engine_start")
			{
				this.TurnOnEngine();
				return;
			}
			if (!(eventName == "engine_stop"))
			{
				return;
			}
			this.TurnOffEngine();
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x000918B0 File Offset: 0x0008FAB0
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			string currentAnimationName = new MegaAnimationState(animationState).GetCurrentAnimationName(0);
			if (currentAnimationName != "attack")
			{
				this.TurnOffFlameThrower();
			}
			bool flag = currentAnimationName == "hurt";
			this._flameThrowerParticlesDark.Visible = !flag;
			this._flameThrowerParticlesLight.Visible = !flag;
			this._cinderParticles.Visible = !flag;
			this._glowParticles.Visible = !flag;
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x0009192A File Offset: 0x0008FB2A
		private void TurnOnFlameThrower()
		{
			this._flameThrowerParticlesDark.Restart();
			this._flameThrowerParticlesLight.Restart();
			this._cinderParticles.Restart();
			this._glowParticles.Restart();
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x00091958 File Offset: 0x0008FB58
		private void TurnOffFlameThrower()
		{
			this._flameThrowerParticlesDark.Emitting = false;
			this._flameThrowerParticlesLight.Emitting = false;
			this._cinderParticles.Emitting = false;
			this._glowParticles.Emitting = false;
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x0009198A File Offset: 0x0008FB8A
		private void TurnOnEngine()
		{
			this._engineParticles.Restart();
			this._engineParticlesDark.Restart();
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x000919A2 File Offset: 0x0008FBA2
		private void TurnOffEngine()
		{
			this._engineParticles.Emitting = false;
			this._engineParticlesDark.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001785 RID: 6021 RVA: 0x000919BC File Offset: 0x0008FBBC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NMechaKnightVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMechaKnightVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NMechaKnightVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NMechaKnightVfx.MethodName.TurnOnFlameThrower, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMechaKnightVfx.MethodName.TurnOffFlameThrower, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMechaKnightVfx.MethodName.TurnOnEngine, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMechaKnightVfx.MethodName.TurnOffEngine, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001786 RID: 6022 RVA: 0x00091C58 File Offset: 0x0008FE58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMechaKnightVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMechaKnightVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMechaKnightVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMechaKnightVfx.MethodName.TurnOnFlameThrower && args.Count == 0)
			{
				this.TurnOnFlameThrower();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMechaKnightVfx.MethodName.TurnOffFlameThrower && args.Count == 0)
			{
				this.TurnOffFlameThrower();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMechaKnightVfx.MethodName.TurnOnEngine && args.Count == 0)
			{
				this.TurnOnEngine();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMechaKnightVfx.MethodName.TurnOffEngine && args.Count == 0)
			{
				this.TurnOffEngine();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001787 RID: 6023 RVA: 0x00091DD0 File Offset: 0x0008FFD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMechaKnightVfx.MethodName._Ready || (ref method) == NMechaKnightVfx.MethodName.OnAnimationEvent || (ref method) == NMechaKnightVfx.MethodName.OnAnimationStart || (ref method) == NMechaKnightVfx.MethodName.TurnOnFlameThrower || (ref method) == NMechaKnightVfx.MethodName.TurnOffFlameThrower || (ref method) == NMechaKnightVfx.MethodName.TurnOnEngine || (ref method) == NMechaKnightVfx.MethodName.TurnOffEngine || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001788 RID: 6024 RVA: 0x00091E50 File Offset: 0x00090050
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMechaKnightVfx.PropertyName._flameThrowerParticlesDark)
			{
				this._flameThrowerParticlesDark = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._flameThrowerParticlesLight)
			{
				this._flameThrowerParticlesLight = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._cinderParticles)
			{
				this._cinderParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._glowParticles)
			{
				this._glowParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._engineParticles)
			{
				this._engineParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._engineParticlesDark)
			{
				this._engineParticlesDark = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001789 RID: 6025 RVA: 0x00091F24 File Offset: 0x00090124
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMechaKnightVfx.PropertyName._flameThrowerParticlesDark)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameThrowerParticlesDark);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._flameThrowerParticlesLight)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameThrowerParticlesLight);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._cinderParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._cinderParticles);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._glowParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._glowParticles);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._engineParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._engineParticles);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._engineParticlesDark)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._engineParticlesDark);
				return true;
			}
			if ((ref name) == NMechaKnightVfx.PropertyName._parent)
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
		// Token: 0x0600178A RID: 6026 RVA: 0x0009201C File Offset: 0x0009021C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMechaKnightVfx.PropertyName._flameThrowerParticlesDark, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMechaKnightVfx.PropertyName._flameThrowerParticlesLight, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMechaKnightVfx.PropertyName._cinderParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMechaKnightVfx.PropertyName._glowParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMechaKnightVfx.PropertyName._engineParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMechaKnightVfx.PropertyName._engineParticlesDark, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMechaKnightVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600178B RID: 6027 RVA: 0x00092118 File Offset: 0x00090318
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMechaKnightVfx.PropertyName._flameThrowerParticlesDark, Variant.From<GpuParticles2D>(ref this._flameThrowerParticlesDark));
			info.AddProperty(NMechaKnightVfx.PropertyName._flameThrowerParticlesLight, Variant.From<GpuParticles2D>(ref this._flameThrowerParticlesLight));
			info.AddProperty(NMechaKnightVfx.PropertyName._cinderParticles, Variant.From<GpuParticles2D>(ref this._cinderParticles));
			info.AddProperty(NMechaKnightVfx.PropertyName._glowParticles, Variant.From<GpuParticles2D>(ref this._glowParticles));
			info.AddProperty(NMechaKnightVfx.PropertyName._engineParticles, Variant.From<GpuParticles2D>(ref this._engineParticles));
			info.AddProperty(NMechaKnightVfx.PropertyName._engineParticlesDark, Variant.From<GpuParticles2D>(ref this._engineParticlesDark));
			info.AddProperty(NMechaKnightVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x0600178C RID: 6028 RVA: 0x000921C8 File Offset: 0x000903C8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMechaKnightVfx.PropertyName._flameThrowerParticlesDark, ref variant))
			{
				this._flameThrowerParticlesDark = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMechaKnightVfx.PropertyName._flameThrowerParticlesLight, ref variant2))
			{
				this._flameThrowerParticlesLight = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMechaKnightVfx.PropertyName._cinderParticles, ref variant3))
			{
				this._cinderParticles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMechaKnightVfx.PropertyName._glowParticles, ref variant4))
			{
				this._glowParticles = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMechaKnightVfx.PropertyName._engineParticles, ref variant5))
			{
				this._engineParticles = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMechaKnightVfx.PropertyName._engineParticlesDark, ref variant6))
			{
				this._engineParticlesDark = variant6.As<GpuParticles2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMechaKnightVfx.PropertyName._parent, ref variant7))
			{
				this._parent = variant7.As<Node2D>();
			}
		}

		// Token: 0x04000864 RID: 2148
		private GpuParticles2D _flameThrowerParticlesDark;

		// Token: 0x04000865 RID: 2149
		private GpuParticles2D _flameThrowerParticlesLight;

		// Token: 0x04000866 RID: 2150
		private GpuParticles2D _cinderParticles;

		// Token: 0x04000867 RID: 2151
		private GpuParticles2D _glowParticles;

		// Token: 0x04000868 RID: 2152
		private GpuParticles2D _engineParticles;

		// Token: 0x04000869 RID: 2153
		private GpuParticles2D _engineParticlesDark;

		// Token: 0x0400086A RID: 2154
		private Node2D _parent;

		// Token: 0x0400086B RID: 2155
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010BF RID: 4287
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040041AD RID: 16813
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040041AE RID: 16814
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x040041AF RID: 16815
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'TurnOnFlameThrower' method.
			/// </summary>
			// Token: 0x040041B0 RID: 16816
			public static readonly StringName TurnOnFlameThrower = "TurnOnFlameThrower";

			/// <summary>
			/// Cached name for the 'TurnOffFlameThrower' method.
			/// </summary>
			// Token: 0x040041B1 RID: 16817
			public static readonly StringName TurnOffFlameThrower = "TurnOffFlameThrower";

			/// <summary>
			/// Cached name for the 'TurnOnEngine' method.
			/// </summary>
			// Token: 0x040041B2 RID: 16818
			public static readonly StringName TurnOnEngine = "TurnOnEngine";

			/// <summary>
			/// Cached name for the 'TurnOffEngine' method.
			/// </summary>
			// Token: 0x040041B3 RID: 16819
			public static readonly StringName TurnOffEngine = "TurnOffEngine";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010C0 RID: 4288
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_flameThrowerParticlesDark' field.
			/// </summary>
			// Token: 0x040041B4 RID: 16820
			public static readonly StringName _flameThrowerParticlesDark = "_flameThrowerParticlesDark";

			/// <summary>
			/// Cached name for the '_flameThrowerParticlesLight' field.
			/// </summary>
			// Token: 0x040041B5 RID: 16821
			public static readonly StringName _flameThrowerParticlesLight = "_flameThrowerParticlesLight";

			/// <summary>
			/// Cached name for the '_cinderParticles' field.
			/// </summary>
			// Token: 0x040041B6 RID: 16822
			public static readonly StringName _cinderParticles = "_cinderParticles";

			/// <summary>
			/// Cached name for the '_glowParticles' field.
			/// </summary>
			// Token: 0x040041B7 RID: 16823
			public static readonly StringName _glowParticles = "_glowParticles";

			/// <summary>
			/// Cached name for the '_engineParticles' field.
			/// </summary>
			// Token: 0x040041B8 RID: 16824
			public static readonly StringName _engineParticles = "_engineParticles";

			/// <summary>
			/// Cached name for the '_engineParticlesDark' field.
			/// </summary>
			// Token: 0x040041B9 RID: 16825
			public static readonly StringName _engineParticlesDark = "_engineParticlesDark";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x040041BA RID: 16826
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010C1 RID: 4289
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
