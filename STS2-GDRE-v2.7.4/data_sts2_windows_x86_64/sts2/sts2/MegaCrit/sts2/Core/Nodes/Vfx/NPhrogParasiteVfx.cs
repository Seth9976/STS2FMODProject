using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001DB RID: 475
	[NullableContext(1)]
	[Nullable(0)]
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NPhrogParasiteVfx.cs")]
	public class NPhrogParasiteVfx : Node
	{
		// Token: 0x06001803 RID: 6147 RVA: 0x000961DC File Offset: 0x000943DC
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._bubbleParticlesA = this._parent.GetNode<GpuParticles2D>("BubbleABoneNode/WormParticlesA");
			this._bubbleParticlesB = this._parent.GetNode<GpuParticles2D>("BubbleBSlotNode/WormParticlesB");
			this._bubbleParticlesC = this._parent.GetNode<GpuParticles2D>("BubbleCBoneNode/WormParticlesC");
			this._gooParticlesDeath = this._parent.GetNode<GpuParticles2D>("DeathParticles");
			this._wormParticlesDeath = this._parent.GetNode<GpuParticles2D>("DeathWormParticles");
			this._bubbleParticlesA.Emitting = false;
			this._bubbleParticlesB.Emitting = false;
			this._bubbleParticlesC.Emitting = false;
			this._gooParticlesDeath.Emitting = false;
			this._wormParticlesDeath.Emitting = false;
			this._gooParticlesDeath.OneShot = true;
			this._wormParticlesDeath.OneShot = true;
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("die", true, 0);
			});
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x0009634C File Offset: 0x0009454C
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "infect")
			{
				this.TurnOnInfect();
				return;
			}
			if (eventName == "stop_infect")
			{
				this.TurnOffInfect();
				return;
			}
			if (!(eventName == "explode"))
			{
				return;
			}
			this.StartExplode();
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x000963AC File Offset: 0x000945AC
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			this.TurnOffInfect();
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x000963B4 File Offset: 0x000945B4
		private void TurnOnInfect()
		{
			this._bubbleParticlesA.Emitting = true;
			this._bubbleParticlesB.Emitting = true;
			this._bubbleParticlesC.Emitting = true;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x000963DA File Offset: 0x000945DA
		private void TurnOffInfect()
		{
			this._bubbleParticlesA.Emitting = false;
			this._bubbleParticlesB.Emitting = false;
			this._bubbleParticlesC.Emitting = false;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00096400 File Offset: 0x00094600
		private void StartExplode()
		{
			this._gooParticlesDeath.Restart();
			this._wormParticlesDeath.Restart();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001809 RID: 6153 RVA: 0x00096418 File Offset: 0x00094618
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NPhrogParasiteVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPhrogParasiteVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NPhrogParasiteVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NPhrogParasiteVfx.MethodName.TurnOnInfect, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPhrogParasiteVfx.MethodName.TurnOffInfect, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPhrogParasiteVfx.MethodName.StartExplode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600180A RID: 6154 RVA: 0x00096684 File Offset: 0x00094884
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPhrogParasiteVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPhrogParasiteVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPhrogParasiteVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPhrogParasiteVfx.MethodName.TurnOnInfect && args.Count == 0)
			{
				this.TurnOnInfect();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPhrogParasiteVfx.MethodName.TurnOffInfect && args.Count == 0)
			{
				this.TurnOffInfect();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPhrogParasiteVfx.MethodName.StartExplode && args.Count == 0)
			{
				this.StartExplode();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600180B RID: 6155 RVA: 0x000967D8 File Offset: 0x000949D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPhrogParasiteVfx.MethodName._Ready || (ref method) == NPhrogParasiteVfx.MethodName.OnAnimationEvent || (ref method) == NPhrogParasiteVfx.MethodName.OnAnimationStart || (ref method) == NPhrogParasiteVfx.MethodName.TurnOnInfect || (ref method) == NPhrogParasiteVfx.MethodName.TurnOffInfect || (ref method) == NPhrogParasiteVfx.MethodName.StartExplode || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600180C RID: 6156 RVA: 0x00096848 File Offset: 0x00094A48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPhrogParasiteVfx.PropertyName._bubbleParticlesA)
			{
				this._bubbleParticlesA = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NPhrogParasiteVfx.PropertyName._bubbleParticlesB)
			{
				this._bubbleParticlesB = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NPhrogParasiteVfx.PropertyName._bubbleParticlesC)
			{
				this._bubbleParticlesC = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NPhrogParasiteVfx.PropertyName._gooParticlesDeath)
			{
				this._gooParticlesDeath = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NPhrogParasiteVfx.PropertyName._wormParticlesDeath)
			{
				this._wormParticlesDeath = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NPhrogParasiteVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600180D RID: 6157 RVA: 0x00096900 File Offset: 0x00094B00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPhrogParasiteVfx.PropertyName._bubbleParticlesA)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._bubbleParticlesA);
				return true;
			}
			if ((ref name) == NPhrogParasiteVfx.PropertyName._bubbleParticlesB)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._bubbleParticlesB);
				return true;
			}
			if ((ref name) == NPhrogParasiteVfx.PropertyName._bubbleParticlesC)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._bubbleParticlesC);
				return true;
			}
			if ((ref name) == NPhrogParasiteVfx.PropertyName._gooParticlesDeath)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._gooParticlesDeath);
				return true;
			}
			if ((ref name) == NPhrogParasiteVfx.PropertyName._wormParticlesDeath)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._wormParticlesDeath);
				return true;
			}
			if ((ref name) == NPhrogParasiteVfx.PropertyName._parent)
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
		// Token: 0x0600180E RID: 6158 RVA: 0x000969D8 File Offset: 0x00094BD8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPhrogParasiteVfx.PropertyName._bubbleParticlesA, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPhrogParasiteVfx.PropertyName._bubbleParticlesB, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPhrogParasiteVfx.PropertyName._bubbleParticlesC, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPhrogParasiteVfx.PropertyName._gooParticlesDeath, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPhrogParasiteVfx.PropertyName._wormParticlesDeath, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPhrogParasiteVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600180F RID: 6159 RVA: 0x00096AB4 File Offset: 0x00094CB4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesA, Variant.From<GpuParticles2D>(ref this._bubbleParticlesA));
			info.AddProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesB, Variant.From<GpuParticles2D>(ref this._bubbleParticlesB));
			info.AddProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesC, Variant.From<GpuParticles2D>(ref this._bubbleParticlesC));
			info.AddProperty(NPhrogParasiteVfx.PropertyName._gooParticlesDeath, Variant.From<GpuParticles2D>(ref this._gooParticlesDeath));
			info.AddProperty(NPhrogParasiteVfx.PropertyName._wormParticlesDeath, Variant.From<GpuParticles2D>(ref this._wormParticlesDeath));
			info.AddProperty(NPhrogParasiteVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001810 RID: 6160 RVA: 0x00096B4C File Offset: 0x00094D4C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesA, ref variant))
			{
				this._bubbleParticlesA = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesB, ref variant2))
			{
				this._bubbleParticlesB = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPhrogParasiteVfx.PropertyName._bubbleParticlesC, ref variant3))
			{
				this._bubbleParticlesC = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPhrogParasiteVfx.PropertyName._gooParticlesDeath, ref variant4))
			{
				this._gooParticlesDeath = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPhrogParasiteVfx.PropertyName._wormParticlesDeath, ref variant5))
			{
				this._wormParticlesDeath = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPhrogParasiteVfx.PropertyName._parent, ref variant6))
			{
				this._parent = variant6.As<Node2D>();
			}
		}

		// Token: 0x040008AA RID: 2218
		private GpuParticles2D _bubbleParticlesA;

		// Token: 0x040008AB RID: 2219
		private GpuParticles2D _bubbleParticlesB;

		// Token: 0x040008AC RID: 2220
		private GpuParticles2D _bubbleParticlesC;

		// Token: 0x040008AD RID: 2221
		private GpuParticles2D _gooParticlesDeath;

		// Token: 0x040008AE RID: 2222
		private GpuParticles2D _wormParticlesDeath;

		// Token: 0x040008AF RID: 2223
		private Node2D _parent;

		// Token: 0x040008B0 RID: 2224
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010DF RID: 4319
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400421E RID: 16926
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x0400421F RID: 16927
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x04004220 RID: 16928
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'TurnOnInfect' method.
			/// </summary>
			// Token: 0x04004221 RID: 16929
			public static readonly StringName TurnOnInfect = "TurnOnInfect";

			/// <summary>
			/// Cached name for the 'TurnOffInfect' method.
			/// </summary>
			// Token: 0x04004222 RID: 16930
			public static readonly StringName TurnOffInfect = "TurnOffInfect";

			/// <summary>
			/// Cached name for the 'StartExplode' method.
			/// </summary>
			// Token: 0x04004223 RID: 16931
			public static readonly StringName StartExplode = "StartExplode";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010E0 RID: 4320
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_bubbleParticlesA' field.
			/// </summary>
			// Token: 0x04004224 RID: 16932
			public static readonly StringName _bubbleParticlesA = "_bubbleParticlesA";

			/// <summary>
			/// Cached name for the '_bubbleParticlesB' field.
			/// </summary>
			// Token: 0x04004225 RID: 16933
			public static readonly StringName _bubbleParticlesB = "_bubbleParticlesB";

			/// <summary>
			/// Cached name for the '_bubbleParticlesC' field.
			/// </summary>
			// Token: 0x04004226 RID: 16934
			public static readonly StringName _bubbleParticlesC = "_bubbleParticlesC";

			/// <summary>
			/// Cached name for the '_gooParticlesDeath' field.
			/// </summary>
			// Token: 0x04004227 RID: 16935
			public static readonly StringName _gooParticlesDeath = "_gooParticlesDeath";

			/// <summary>
			/// Cached name for the '_wormParticlesDeath' field.
			/// </summary>
			// Token: 0x04004228 RID: 16936
			public static readonly StringName _wormParticlesDeath = "_wormParticlesDeath";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004229 RID: 16937
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010E1 RID: 4321
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
