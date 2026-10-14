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
	// Token: 0x020001AE RID: 430
	[ScriptPath("res://src/Core/Nodes/Vfx/NFossilStalkerVfx.cs")]
	public class NFossilStalkerVfx : Node
	{
		// Token: 0x0600154C RID: 5452 RVA: 0x00082128 File Offset: 0x00080328
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._debuffParticles = base.GetNode<GpuParticles2D>("../DebuffBone/DebuffParticles");
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._debuffParticles.Emitting = false;
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x000821B4 File Offset: 0x000803B4
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "debuff_spray_start")
			{
				this.StartDebuff();
				return;
			}
			if (!(eventName == "debuff_spray_stop"))
			{
				return;
			}
			this.StopDebuff();
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x00082200 File Offset: 0x00080400
		[NullableContext(1)]
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			if (new MegaAnimationState(animationState).GetCurrentAnimationName(0) != "attack")
			{
				this.StopDebuff();
			}
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x00082225 File Offset: 0x00080425
		private void StartDebuff()
		{
			this._debuffParticles.Emitting = true;
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x00082233 File Offset: 0x00080433
		private void StopDebuff()
		{
			this._debuffParticles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001551 RID: 5457 RVA: 0x00082244 File Offset: 0x00080444
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NFossilStalkerVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFossilStalkerVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NFossilStalkerVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NFossilStalkerVfx.MethodName.StartDebuff, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFossilStalkerVfx.MethodName.StopDebuff, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001552 RID: 5458 RVA: 0x00082480 File Offset: 0x00080680
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFossilStalkerVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFossilStalkerVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFossilStalkerVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFossilStalkerVfx.MethodName.StartDebuff && args.Count == 0)
			{
				this.StartDebuff();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFossilStalkerVfx.MethodName.StopDebuff && args.Count == 0)
			{
				this.StopDebuff();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001553 RID: 5459 RVA: 0x000825AC File Offset: 0x000807AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFossilStalkerVfx.MethodName._Ready || (ref method) == NFossilStalkerVfx.MethodName.OnAnimationEvent || (ref method) == NFossilStalkerVfx.MethodName.OnAnimationStart || (ref method) == NFossilStalkerVfx.MethodName.StartDebuff || (ref method) == NFossilStalkerVfx.MethodName.StopDebuff || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001554 RID: 5460 RVA: 0x0008260B File Offset: 0x0008080B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFossilStalkerVfx.PropertyName._debuffParticles)
			{
				this._debuffParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NFossilStalkerVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001555 RID: 5461 RVA: 0x0008264C File Offset: 0x0008084C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFossilStalkerVfx.PropertyName._debuffParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._debuffParticles);
				return true;
			}
			if ((ref name) == NFossilStalkerVfx.PropertyName._parent)
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
		// Token: 0x06001556 RID: 5462 RVA: 0x000826A4 File Offset: 0x000808A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFossilStalkerVfx.PropertyName._debuffParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFossilStalkerVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001557 RID: 5463 RVA: 0x000826FA File Offset: 0x000808FA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFossilStalkerVfx.PropertyName._debuffParticles, Variant.From<GpuParticles2D>(ref this._debuffParticles));
			info.AddProperty(NFossilStalkerVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001558 RID: 5464 RVA: 0x00082730 File Offset: 0x00080930
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFossilStalkerVfx.PropertyName._debuffParticles, ref variant))
			{
				this._debuffParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFossilStalkerVfx.PropertyName._parent, ref variant2))
			{
				this._parent = variant2.As<Node2D>();
			}
		}

		// Token: 0x04000796 RID: 1942
		[Nullable(1)]
		private GpuParticles2D _debuffParticles;

		// Token: 0x04000797 RID: 1943
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000798 RID: 1944
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001039 RID: 4153
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004021 RID: 16417
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004022 RID: 16418
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x04004023 RID: 16419
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'StartDebuff' method.
			/// </summary>
			// Token: 0x04004024 RID: 16420
			public static readonly StringName StartDebuff = "StartDebuff";

			/// <summary>
			/// Cached name for the 'StopDebuff' method.
			/// </summary>
			// Token: 0x04004025 RID: 16421
			public static readonly StringName StopDebuff = "StopDebuff";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200103A RID: 4154
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_debuffParticles' field.
			/// </summary>
			// Token: 0x04004026 RID: 16422
			public static readonly StringName _debuffParticles = "_debuffParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004027 RID: 16423
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200103B RID: 4155
		public class SignalName : Node.SignalName
		{
		}
	}
}
