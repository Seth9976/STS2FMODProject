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
	// Token: 0x020001F1 RID: 497
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NSlimedBerserkerVfx.cs")]
	public class NSlimedBerserkerVfx : Node
	{
		// Token: 0x0600195B RID: 6491 RVA: 0x0009F6C8 File Offset: 0x0009D8C8
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._gooParticlesR = this._parent.GetNode<GpuParticles2D>("ParticleSlotNodeR/GooParticles");
			this._gooParticlesL = this._parent.GetNode<GpuParticles2D>("ParticleSlotNodeL/GooParticles");
			this._gooParticlesVomit = this._parent.GetNode<GpuParticles2D>("ParticleSlotNodeVomit/GooParticles");
			this.StopGooParticles();
			this.StopVomitParticles();
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x0009F774 File Offset: 0x0009D974
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "goo_start")
			{
				this.StartGooParticles();
				return;
			}
			if (eventName == "goo_stop")
			{
				this.StopGooParticles();
				return;
			}
			if (eventName == "vomit_start")
			{
				this.StartVomitParticles();
				return;
			}
			if (!(eventName == "vomit_stop"))
			{
				return;
			}
			this.StopVomitParticles();
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0009F7E8 File Offset: 0x0009D9E8
		private void StartGooParticles()
		{
			this._gooParticlesR.Emitting = true;
			this._gooParticlesL.Emitting = true;
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0009F802 File Offset: 0x0009DA02
		private void StopGooParticles()
		{
			this._gooParticlesR.Emitting = false;
			this._gooParticlesL.Emitting = false;
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0009F81C File Offset: 0x0009DA1C
		private void StopVomitParticles()
		{
			this._gooParticlesVomit.Emitting = false;
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0009F82A File Offset: 0x0009DA2A
		private void StartVomitParticles()
		{
			this._gooParticlesVomit.Emitting = true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001961 RID: 6497 RVA: 0x0009F838 File Offset: 0x0009DA38
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NSlimedBerserkerVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSlimedBerserkerVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NSlimedBerserkerVfx.MethodName.StartGooParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSlimedBerserkerVfx.MethodName.StopGooParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSlimedBerserkerVfx.MethodName.StopVomitParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSlimedBerserkerVfx.MethodName.StartVomitParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001962 RID: 6498 RVA: 0x0009FA1C File Offset: 0x0009DC1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSlimedBerserkerVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlimedBerserkerVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlimedBerserkerVfx.MethodName.StartGooParticles && args.Count == 0)
			{
				this.StartGooParticles();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlimedBerserkerVfx.MethodName.StopGooParticles && args.Count == 0)
			{
				this.StopGooParticles();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlimedBerserkerVfx.MethodName.StopVomitParticles && args.Count == 0)
			{
				this.StopVomitParticles();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlimedBerserkerVfx.MethodName.StartVomitParticles && args.Count == 0)
			{
				this.StartVomitParticles();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001963 RID: 6499 RVA: 0x0009FB48 File Offset: 0x0009DD48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSlimedBerserkerVfx.MethodName._Ready || (ref method) == NSlimedBerserkerVfx.MethodName.OnAnimationEvent || (ref method) == NSlimedBerserkerVfx.MethodName.StartGooParticles || (ref method) == NSlimedBerserkerVfx.MethodName.StopGooParticles || (ref method) == NSlimedBerserkerVfx.MethodName.StopVomitParticles || (ref method) == NSlimedBerserkerVfx.MethodName.StartVomitParticles || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001964 RID: 6500 RVA: 0x0009FBB8 File Offset: 0x0009DDB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSlimedBerserkerVfx.PropertyName._gooParticlesR)
			{
				this._gooParticlesR = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSlimedBerserkerVfx.PropertyName._gooParticlesL)
			{
				this._gooParticlesL = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSlimedBerserkerVfx.PropertyName._gooParticlesVomit)
			{
				this._gooParticlesVomit = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSlimedBerserkerVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001965 RID: 6501 RVA: 0x0009FC3C File Offset: 0x0009DE3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSlimedBerserkerVfx.PropertyName._gooParticlesR)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._gooParticlesR);
				return true;
			}
			if ((ref name) == NSlimedBerserkerVfx.PropertyName._gooParticlesL)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._gooParticlesL);
				return true;
			}
			if ((ref name) == NSlimedBerserkerVfx.PropertyName._gooParticlesVomit)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._gooParticlesVomit);
				return true;
			}
			if ((ref name) == NSlimedBerserkerVfx.PropertyName._parent)
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
		// Token: 0x06001966 RID: 6502 RVA: 0x0009FCD4 File Offset: 0x0009DED4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSlimedBerserkerVfx.PropertyName._gooParticlesR, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSlimedBerserkerVfx.PropertyName._gooParticlesL, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSlimedBerserkerVfx.PropertyName._gooParticlesVomit, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSlimedBerserkerVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001967 RID: 6503 RVA: 0x0009FD6C File Offset: 0x0009DF6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesR, Variant.From<GpuParticles2D>(ref this._gooParticlesR));
			info.AddProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesL, Variant.From<GpuParticles2D>(ref this._gooParticlesL));
			info.AddProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesVomit, Variant.From<GpuParticles2D>(ref this._gooParticlesVomit));
			info.AddProperty(NSlimedBerserkerVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001968 RID: 6504 RVA: 0x0009FDD8 File Offset: 0x0009DFD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesR, ref variant))
			{
				this._gooParticlesR = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesL, ref variant2))
			{
				this._gooParticlesL = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesVomit, ref variant3))
			{
				this._gooParticlesVomit = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSlimedBerserkerVfx.PropertyName._parent, ref variant4))
			{
				this._parent = variant4.As<Node2D>();
			}
		}

		// Token: 0x0400093B RID: 2363
		[Nullable(1)]
		private GpuParticles2D _gooParticlesR;

		// Token: 0x0400093C RID: 2364
		[Nullable(1)]
		private GpuParticles2D _gooParticlesL;

		// Token: 0x0400093D RID: 2365
		[Nullable(1)]
		private GpuParticles2D _gooParticlesVomit;

		// Token: 0x0400093E RID: 2366
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x0400093F RID: 2367
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001132 RID: 4402
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004313 RID: 17171
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004314 RID: 17172
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartGooParticles' method.
			/// </summary>
			// Token: 0x04004315 RID: 17173
			public static readonly StringName StartGooParticles = "StartGooParticles";

			/// <summary>
			/// Cached name for the 'StopGooParticles' method.
			/// </summary>
			// Token: 0x04004316 RID: 17174
			public static readonly StringName StopGooParticles = "StopGooParticles";

			/// <summary>
			/// Cached name for the 'StopVomitParticles' method.
			/// </summary>
			// Token: 0x04004317 RID: 17175
			public static readonly StringName StopVomitParticles = "StopVomitParticles";

			/// <summary>
			/// Cached name for the 'StartVomitParticles' method.
			/// </summary>
			// Token: 0x04004318 RID: 17176
			public static readonly StringName StartVomitParticles = "StartVomitParticles";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001133 RID: 4403
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_gooParticlesR' field.
			/// </summary>
			// Token: 0x04004319 RID: 17177
			public static readonly StringName _gooParticlesR = "_gooParticlesR";

			/// <summary>
			/// Cached name for the '_gooParticlesL' field.
			/// </summary>
			// Token: 0x0400431A RID: 17178
			public static readonly StringName _gooParticlesL = "_gooParticlesL";

			/// <summary>
			/// Cached name for the '_gooParticlesVomit' field.
			/// </summary>
			// Token: 0x0400431B RID: 17179
			public static readonly StringName _gooParticlesVomit = "_gooParticlesVomit";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x0400431C RID: 17180
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001134 RID: 4404
		public class SignalName : Node.SignalName
		{
		}
	}
}
