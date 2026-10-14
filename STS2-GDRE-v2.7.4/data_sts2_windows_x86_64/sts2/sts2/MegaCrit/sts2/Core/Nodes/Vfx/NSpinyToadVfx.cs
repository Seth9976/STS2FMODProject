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
	// Token: 0x020001FA RID: 506
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NSpinyToadVfx.cs")]
	public class NSpinyToadVfx : Node
	{
		// Token: 0x06001A05 RID: 6661 RVA: 0x000A6FA8 File Offset: 0x000A51A8
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._spineParticles = this._parent.GetNode<GpuParticles2D>("BurstParticles");
			this._spineSubParticles = this._parent.GetNode<GpuParticles2D>("BurstParticlesSub");
			this._spineParticles.Emitting = false;
			this._spineParticles.OneShot = true;
			this._spineSubParticles.Emitting = false;
			this._spineSubParticles.OneShot = true;
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("explode", true, 0);
			});
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x000A7088 File Offset: 0x000A5288
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "explode")
			{
				this.StartExplosion();
			}
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x000A70BF File Offset: 0x000A52BF
		private void StartExplosion()
		{
			this._spineParticles.Restart();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A08 RID: 6664 RVA: 0x000A70CC File Offset: 0x000A52CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSpinyToadVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSpinyToadVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NSpinyToadVfx.MethodName.StartExplosion, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A09 RID: 6665 RVA: 0x000A7224 File Offset: 0x000A5424
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSpinyToadVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpinyToadVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpinyToadVfx.MethodName.StartExplosion && args.Count == 0)
			{
				this.StartExplosion();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001A0A RID: 6666 RVA: 0x000A72DE File Offset: 0x000A54DE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSpinyToadVfx.MethodName._Ready || (ref method) == NSpinyToadVfx.MethodName.OnAnimationEvent || (ref method) == NSpinyToadVfx.MethodName.StartExplosion || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001A0B RID: 6667 RVA: 0x000A7314 File Offset: 0x000A5514
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSpinyToadVfx.PropertyName._spineParticles)
			{
				this._spineParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSpinyToadVfx.PropertyName._spineSubParticles)
			{
				this._spineSubParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSpinyToadVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001A0C RID: 6668 RVA: 0x000A737C File Offset: 0x000A557C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSpinyToadVfx.PropertyName._spineParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spineParticles);
				return true;
			}
			if ((ref name) == NSpinyToadVfx.PropertyName._spineSubParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spineSubParticles);
				return true;
			}
			if ((ref name) == NSpinyToadVfx.PropertyName._parent)
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
		// Token: 0x06001A0D RID: 6669 RVA: 0x000A73F4 File Offset: 0x000A55F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSpinyToadVfx.PropertyName._spineParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSpinyToadVfx.PropertyName._spineSubParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSpinyToadVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A0E RID: 6670 RVA: 0x000A746C File Offset: 0x000A566C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSpinyToadVfx.PropertyName._spineParticles, Variant.From<GpuParticles2D>(ref this._spineParticles));
			info.AddProperty(NSpinyToadVfx.PropertyName._spineSubParticles, Variant.From<GpuParticles2D>(ref this._spineSubParticles));
			info.AddProperty(NSpinyToadVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001A0F RID: 6671 RVA: 0x000A74C4 File Offset: 0x000A56C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSpinyToadVfx.PropertyName._spineParticles, ref variant))
			{
				this._spineParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSpinyToadVfx.PropertyName._spineSubParticles, ref variant2))
			{
				this._spineSubParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSpinyToadVfx.PropertyName._parent, ref variant3))
			{
				this._parent = variant3.As<Node2D>();
			}
		}

		// Token: 0x040009AB RID: 2475
		[Nullable(1)]
		private GpuParticles2D _spineParticles;

		// Token: 0x040009AC RID: 2476
		[Nullable(1)]
		private GpuParticles2D _spineSubParticles;

		// Token: 0x040009AD RID: 2477
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x040009AE RID: 2478
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001156 RID: 4438
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040043C2 RID: 17346
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040043C3 RID: 17347
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartExplosion' method.
			/// </summary>
			// Token: 0x040043C4 RID: 17348
			public static readonly StringName StartExplosion = "StartExplosion";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001157 RID: 4439
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_spineParticles' field.
			/// </summary>
			// Token: 0x040043C5 RID: 17349
			public static readonly StringName _spineParticles = "_spineParticles";

			/// <summary>
			/// Cached name for the '_spineSubParticles' field.
			/// </summary>
			// Token: 0x040043C6 RID: 17350
			public static readonly StringName _spineSubParticles = "_spineSubParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x040043C7 RID: 17351
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001158 RID: 4440
		public class SignalName : Node.SignalName
		{
		}
	}
}
