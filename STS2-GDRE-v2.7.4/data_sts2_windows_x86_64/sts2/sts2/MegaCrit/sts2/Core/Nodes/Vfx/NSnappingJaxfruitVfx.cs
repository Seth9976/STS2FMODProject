using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001F5 RID: 501
	[NullableContext(1)]
	[Nullable(0)]
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NSnappingJaxfruitVfx.cs")]
	public class NSnappingJaxfruitVfx : Node
	{
		// Token: 0x0600199E RID: 6558 RVA: 0x000A17C0 File Offset: 0x0009F9C0
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._projectileBone = this._parent.GetNode<Node2D>("ProjectileAttachBone");
			this._targetBone = this._parent.GetNode<Node2D>("TargetBone");
			this._glowParticles = this._parent.GetNode<GpuParticles2D>("ProjectileAttachBone/GlowParticles");
			this._blobParticles = this._parent.GetNode<GpuParticles2D>("ProjectileAttachBone/BlobParticles");
			this._trail = this._parent.GetNode<NBasicTrail>("ProjectileAttachBone/Trail");
			this.ResetCast();
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x000A18B6 File Offset: 0x0009FAB6
		[NullableContext(2)]
		public void SetTarget(Creature target)
		{
			this._target = target;
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x000A18C0 File Offset: 0x0009FAC0
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "cast_start")
			{
				this.StartCast();
				return;
			}
			if (!(eventName == "cast_end"))
			{
				return;
			}
			this.ResetCast();
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x000A190C File Offset: 0x0009FB0C
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			if (new MegaAnimationState(animationState).GetCurrentAnimationName(0) != "attack")
			{
				this.ResetCast();
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x000A1934 File Offset: 0x0009FB34
		private void StartCast()
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(this._target) : null);
			if (ncreature != null)
			{
				this._targetBone.GlobalPosition = new Vector2(ncreature.GlobalPosition.X, ncreature.GlobalPosition.Y - 130f);
			}
			this._projectileBone.Visible = true;
			this._trail.ClearPoints();
			this._blobParticles.Restart();
			this._glowParticles.Restart();
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x000A19B5 File Offset: 0x0009FBB5
		private void ResetCast()
		{
			this._blobParticles.Emitting = false;
			this._glowParticles.Emitting = false;
			this._projectileBone.Visible = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060019A4 RID: 6564 RVA: 0x000A19DC File Offset: 0x0009FBDC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NSnappingJaxfruitVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSnappingJaxfruitVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NSnappingJaxfruitVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NSnappingJaxfruitVfx.MethodName.StartCast, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSnappingJaxfruitVfx.MethodName.ResetCast, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060019A5 RID: 6565 RVA: 0x000A1C18 File Offset: 0x0009FE18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSnappingJaxfruitVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSnappingJaxfruitVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSnappingJaxfruitVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSnappingJaxfruitVfx.MethodName.StartCast && args.Count == 0)
			{
				this.StartCast();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSnappingJaxfruitVfx.MethodName.ResetCast && args.Count == 0)
			{
				this.ResetCast();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060019A6 RID: 6566 RVA: 0x000A1D44 File Offset: 0x0009FF44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSnappingJaxfruitVfx.MethodName._Ready || (ref method) == NSnappingJaxfruitVfx.MethodName.OnAnimationEvent || (ref method) == NSnappingJaxfruitVfx.MethodName.OnAnimationStart || (ref method) == NSnappingJaxfruitVfx.MethodName.StartCast || (ref method) == NSnappingJaxfruitVfx.MethodName.ResetCast || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060019A7 RID: 6567 RVA: 0x000A1DA4 File Offset: 0x0009FFA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._projectileBone)
			{
				this._projectileBone = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._targetBone)
			{
				this._targetBone = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._glowParticles)
			{
				this._glowParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._blobParticles)
			{
				this._blobParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._trail)
			{
				this._trail = VariantUtils.ConvertTo<NBasicTrail>(ref value);
				return true;
			}
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060019A8 RID: 6568 RVA: 0x000A1E5C File Offset: 0x000A005C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._projectileBone)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._projectileBone);
				return true;
			}
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._targetBone)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._targetBone);
				return true;
			}
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._glowParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._glowParticles);
				return true;
			}
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._blobParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._blobParticles);
				return true;
			}
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._trail)
			{
				value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail);
				return true;
			}
			if ((ref name) == NSnappingJaxfruitVfx.PropertyName._parent)
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
		// Token: 0x060019A9 RID: 6569 RVA: 0x000A1F34 File Offset: 0x000A0134
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSnappingJaxfruitVfx.PropertyName._projectileBone, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSnappingJaxfruitVfx.PropertyName._targetBone, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSnappingJaxfruitVfx.PropertyName._glowParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSnappingJaxfruitVfx.PropertyName._blobParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSnappingJaxfruitVfx.PropertyName._trail, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSnappingJaxfruitVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060019AA RID: 6570 RVA: 0x000A2010 File Offset: 0x000A0210
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSnappingJaxfruitVfx.PropertyName._projectileBone, Variant.From<Node2D>(ref this._projectileBone));
			info.AddProperty(NSnappingJaxfruitVfx.PropertyName._targetBone, Variant.From<Node2D>(ref this._targetBone));
			info.AddProperty(NSnappingJaxfruitVfx.PropertyName._glowParticles, Variant.From<GpuParticles2D>(ref this._glowParticles));
			info.AddProperty(NSnappingJaxfruitVfx.PropertyName._blobParticles, Variant.From<GpuParticles2D>(ref this._blobParticles));
			info.AddProperty(NSnappingJaxfruitVfx.PropertyName._trail, Variant.From<NBasicTrail>(ref this._trail));
			info.AddProperty(NSnappingJaxfruitVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x060019AB RID: 6571 RVA: 0x000A20A8 File Offset: 0x000A02A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._projectileBone, ref variant))
			{
				this._projectileBone = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._targetBone, ref variant2))
			{
				this._targetBone = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._glowParticles, ref variant3))
			{
				this._glowParticles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._blobParticles, ref variant4))
			{
				this._blobParticles = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._trail, ref variant5))
			{
				this._trail = variant5.As<NBasicTrail>();
			}
			Variant variant6;
			if (info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._parent, ref variant6))
			{
				this._parent = variant6.As<Node2D>();
			}
		}

		// Token: 0x04000957 RID: 2391
		private const float _attackHeight = 130f;

		// Token: 0x04000958 RID: 2392
		[Nullable(2)]
		private Creature _target;

		// Token: 0x04000959 RID: 2393
		private Node2D _projectileBone;

		// Token: 0x0400095A RID: 2394
		private Node2D _targetBone;

		// Token: 0x0400095B RID: 2395
		private GpuParticles2D _glowParticles;

		// Token: 0x0400095C RID: 2396
		private GpuParticles2D _blobParticles;

		// Token: 0x0400095D RID: 2397
		private NBasicTrail _trail;

		// Token: 0x0400095E RID: 2398
		private Node2D _parent;

		// Token: 0x0400095F RID: 2399
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001142 RID: 4418
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400434F RID: 17231
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004350 RID: 17232
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x04004351 RID: 17233
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'StartCast' method.
			/// </summary>
			// Token: 0x04004352 RID: 17234
			public static readonly StringName StartCast = "StartCast";

			/// <summary>
			/// Cached name for the 'ResetCast' method.
			/// </summary>
			// Token: 0x04004353 RID: 17235
			public static readonly StringName ResetCast = "ResetCast";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001143 RID: 4419
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_projectileBone' field.
			/// </summary>
			// Token: 0x04004354 RID: 17236
			public static readonly StringName _projectileBone = "_projectileBone";

			/// <summary>
			/// Cached name for the '_targetBone' field.
			/// </summary>
			// Token: 0x04004355 RID: 17237
			public static readonly StringName _targetBone = "_targetBone";

			/// <summary>
			/// Cached name for the '_glowParticles' field.
			/// </summary>
			// Token: 0x04004356 RID: 17238
			public static readonly StringName _glowParticles = "_glowParticles";

			/// <summary>
			/// Cached name for the '_blobParticles' field.
			/// </summary>
			// Token: 0x04004357 RID: 17239
			public static readonly StringName _blobParticles = "_blobParticles";

			/// <summary>
			/// Cached name for the '_trail' field.
			/// </summary>
			// Token: 0x04004358 RID: 17240
			public static readonly StringName _trail = "_trail";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004359 RID: 17241
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001144 RID: 4420
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
