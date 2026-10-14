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
	// Token: 0x020001EC RID: 492
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NSeapunkVfx.cs")]
	public class NSeapunkVfx : Node
	{
		// Token: 0x0600190A RID: 6410 RVA: 0x0009CEC0 File Offset: 0x0009B0C0
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._bubbleParticles = this._parent.GetNode<GpuParticles2D>("BubbleParticles");
			this._weedParticles = this._parent.GetNode<GpuParticles2D>("WeedParticles");
			this._bubbleParticles.OneShot = true;
			this._bubbleParticles.Emitting = false;
			this._weedParticles.OneShot = true;
			this._weedParticles.Emitting = false;
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("cast", true, 0);
			});
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0009CFA0 File Offset: 0x0009B1A0
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "bubbles_start")
			{
				this.StartBubbles();
				return;
			}
			if (!(eventName == "weeds_start"))
			{
				return;
			}
			this.StartWeeds();
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x0009CFEC File Offset: 0x0009B1EC
		private void StartBubbles()
		{
			this._bubbleParticles.Restart();
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x0009CFF9 File Offset: 0x0009B1F9
		private void StartWeeds()
		{
			this._weedParticles.Restart();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600190E RID: 6414 RVA: 0x0009D008 File Offset: 0x0009B208
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NSeapunkVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSeapunkVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NSeapunkVfx.MethodName.StartBubbles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSeapunkVfx.MethodName.StartWeeds, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600190F RID: 6415 RVA: 0x0009D190 File Offset: 0x0009B390
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSeapunkVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSeapunkVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSeapunkVfx.MethodName.StartBubbles && args.Count == 0)
			{
				this.StartBubbles();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSeapunkVfx.MethodName.StartWeeds && args.Count == 0)
			{
				this.StartWeeds();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001910 RID: 6416 RVA: 0x0009D270 File Offset: 0x0009B470
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSeapunkVfx.MethodName._Ready || (ref method) == NSeapunkVfx.MethodName.OnAnimationEvent || (ref method) == NSeapunkVfx.MethodName.StartBubbles || (ref method) == NSeapunkVfx.MethodName.StartWeeds || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001911 RID: 6417 RVA: 0x0009D2C0 File Offset: 0x0009B4C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSeapunkVfx.PropertyName._bubbleParticles)
			{
				this._bubbleParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSeapunkVfx.PropertyName._weedParticles)
			{
				this._weedParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSeapunkVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001912 RID: 6418 RVA: 0x0009D328 File Offset: 0x0009B528
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSeapunkVfx.PropertyName._bubbleParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._bubbleParticles);
				return true;
			}
			if ((ref name) == NSeapunkVfx.PropertyName._weedParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._weedParticles);
				return true;
			}
			if ((ref name) == NSeapunkVfx.PropertyName._parent)
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
		// Token: 0x06001913 RID: 6419 RVA: 0x0009D3A0 File Offset: 0x0009B5A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSeapunkVfx.PropertyName._bubbleParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSeapunkVfx.PropertyName._weedParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSeapunkVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001914 RID: 6420 RVA: 0x0009D418 File Offset: 0x0009B618
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSeapunkVfx.PropertyName._bubbleParticles, Variant.From<GpuParticles2D>(ref this._bubbleParticles));
			info.AddProperty(NSeapunkVfx.PropertyName._weedParticles, Variant.From<GpuParticles2D>(ref this._weedParticles));
			info.AddProperty(NSeapunkVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001915 RID: 6421 RVA: 0x0009D470 File Offset: 0x0009B670
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSeapunkVfx.PropertyName._bubbleParticles, ref variant))
			{
				this._bubbleParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSeapunkVfx.PropertyName._weedParticles, ref variant2))
			{
				this._weedParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSeapunkVfx.PropertyName._parent, ref variant3))
			{
				this._parent = variant3.As<Node2D>();
			}
		}

		// Token: 0x0400091C RID: 2332
		[Nullable(1)]
		private GpuParticles2D _bubbleParticles;

		// Token: 0x0400091D RID: 2333
		[Nullable(1)]
		private GpuParticles2D _weedParticles;

		// Token: 0x0400091E RID: 2334
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x0400091F RID: 2335
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001120 RID: 4384
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040042D9 RID: 17113
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040042DA RID: 17114
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartBubbles' method.
			/// </summary>
			// Token: 0x040042DB RID: 17115
			public static readonly StringName StartBubbles = "StartBubbles";

			/// <summary>
			/// Cached name for the 'StartWeeds' method.
			/// </summary>
			// Token: 0x040042DC RID: 17116
			public static readonly StringName StartWeeds = "StartWeeds";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001121 RID: 4385
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_bubbleParticles' field.
			/// </summary>
			// Token: 0x040042DD RID: 17117
			public static readonly StringName _bubbleParticles = "_bubbleParticles";

			/// <summary>
			/// Cached name for the '_weedParticles' field.
			/// </summary>
			// Token: 0x040042DE RID: 17118
			public static readonly StringName _weedParticles = "_weedParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x040042DF RID: 17119
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001122 RID: 4386
		public class SignalName : Node.SignalName
		{
		}
	}
}
