using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x0200020C RID: 524
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NVineShamblerVinesVfx.cs")]
	public class NVineShamblerVinesVfx : Node2D
	{
		// Token: 0x06001B1C RID: 6940 RVA: 0x000AED6C File Offset: 0x000ACF6C
		public override void _Ready()
		{
			this._frontVinesNode = base.GetNode<Node2D>("VinesFront");
			this._frontVinesAnimController = new MegaSprite(this._frontVinesNode);
			this._frontVinesAnimController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnFrontEvent)));
			this._frontVinesAnimController.ConnectAnimationCompleted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.AnimationEnded)));
			this._backVinesNode = base.GetNode<Node2D>("VinesBackScene/VinesBack");
			this._backVinesAnimController = new MegaSprite(this._backVinesNode);
			this._dirtBlast1 = base.GetNode<GpuParticles2D>("DirtBlast1");
			this._dirtBlast3 = base.GetNode<GpuParticles2D>("DirtBlast3");
			this._dirtBlast2 = base.GetNode<GpuParticles2D>("VinesBackScene/DirtBlast2");
			this._dirtBlast4 = base.GetNode<GpuParticles2D>("VinesBackScene/DirtBlast4");
			this._dirtBlast1.Emitting = false;
			this._dirtBlast1.OneShot = true;
			this._dirtBlast2.Emitting = false;
			this._dirtBlast2.OneShot = true;
			this._dirtBlast3.Emitting = false;
			this._dirtBlast3.OneShot = true;
			this._dirtBlast4.Emitting = false;
			this._dirtBlast4.OneShot = true;
			Vector2 backVineOffset = this._backVinesNode.GlobalPosition - this._frontVinesNode.GlobalPosition;
			this._backVinesNode.Reparent(NCombatRoom.Instance.BackCombatVfxContainer, true);
			Callable.From(delegate
			{
				this._backVinesNode.GlobalPosition = this._frontVinesNode.GlobalPosition + backVineOffset;
			}).CallDeferred(Array.Empty<Variant>());
			this.RunWhenSpineReady(this._frontVinesAnimController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("animation", true, 0);
			});
			this.RunWhenSpineReady(this._backVinesAnimController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("animation", true, 0);
			});
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x000AEF7C File Offset: 0x000AD17C
		private void OnFrontEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "dirt_1")
			{
				this._dirtBlast1.Restart();
				return;
			}
			if (eventName == "dirt_2")
			{
				this._dirtBlast2.Restart();
				return;
			}
			if (eventName == "dirt_3")
			{
				this._dirtBlast3.Restart();
				return;
			}
			if (!(eventName == "dirt_4"))
			{
				return;
			}
			this._dirtBlast4.Restart();
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x000AF004 File Offset: 0x000AD204
		private void AnimationEnded(GodotObject _, GodotObject __, GodotObject ___)
		{
			this.QueueFreeSafely();
			this._backVinesNode.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B1F RID: 6943 RVA: 0x000AF018 File Offset: 0x000AD218
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NVineShamblerVinesVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVineShamblerVinesVfx.MethodName.OnFrontEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NVineShamblerVinesVfx.MethodName.AnimationEnded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B20 RID: 6944 RVA: 0x000AF1F8 File Offset: 0x000AD3F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NVineShamblerVinesVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVineShamblerVinesVfx.MethodName.OnFrontEvent && args.Count == 4)
			{
				this.OnFrontEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVineShamblerVinesVfx.MethodName.AnimationEnded && args.Count == 3)
			{
				this.AnimationEnded(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001B21 RID: 6945 RVA: 0x000AF2DA File Offset: 0x000AD4DA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NVineShamblerVinesVfx.MethodName._Ready || (ref method) == NVineShamblerVinesVfx.MethodName.OnFrontEvent || (ref method) == NVineShamblerVinesVfx.MethodName.AnimationEnded || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001B22 RID: 6946 RVA: 0x000AF310 File Offset: 0x000AD510
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._frontVinesNode)
			{
				this._frontVinesNode = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._backVinesNode)
			{
				this._backVinesNode = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._dirtBlast1)
			{
				this._dirtBlast1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._dirtBlast2)
			{
				this._dirtBlast2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._dirtBlast3)
			{
				this._dirtBlast3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._dirtBlast4)
			{
				this._dirtBlast4 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001B23 RID: 6947 RVA: 0x000AF3C8 File Offset: 0x000AD5C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._frontVinesNode)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._frontVinesNode);
				return true;
			}
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._backVinesNode)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._backVinesNode);
				return true;
			}
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._dirtBlast1)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dirtBlast1);
				return true;
			}
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._dirtBlast2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dirtBlast2);
				return true;
			}
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._dirtBlast3)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dirtBlast3);
				return true;
			}
			if ((ref name) == NVineShamblerVinesVfx.PropertyName._dirtBlast4)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dirtBlast4);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B24 RID: 6948 RVA: 0x000AF4A0 File Offset: 0x000AD6A0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NVineShamblerVinesVfx.PropertyName._frontVinesNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVineShamblerVinesVfx.PropertyName._backVinesNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVineShamblerVinesVfx.PropertyName._dirtBlast1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVineShamblerVinesVfx.PropertyName._dirtBlast2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVineShamblerVinesVfx.PropertyName._dirtBlast3, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVineShamblerVinesVfx.PropertyName._dirtBlast4, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B25 RID: 6949 RVA: 0x000AF57C File Offset: 0x000AD77C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NVineShamblerVinesVfx.PropertyName._frontVinesNode, Variant.From<Node2D>(ref this._frontVinesNode));
			info.AddProperty(NVineShamblerVinesVfx.PropertyName._backVinesNode, Variant.From<Node2D>(ref this._backVinesNode));
			info.AddProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast1, Variant.From<GpuParticles2D>(ref this._dirtBlast1));
			info.AddProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast2, Variant.From<GpuParticles2D>(ref this._dirtBlast2));
			info.AddProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast3, Variant.From<GpuParticles2D>(ref this._dirtBlast3));
			info.AddProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast4, Variant.From<GpuParticles2D>(ref this._dirtBlast4));
		}

		/// <inheritdoc />
		// Token: 0x06001B26 RID: 6950 RVA: 0x000AF614 File Offset: 0x000AD814
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._frontVinesNode, ref variant))
			{
				this._frontVinesNode = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._backVinesNode, ref variant2))
			{
				this._backVinesNode = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast1, ref variant3))
			{
				this._dirtBlast1 = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast2, ref variant4))
			{
				this._dirtBlast2 = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast3, ref variant5))
			{
				this._dirtBlast3 = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NVineShamblerVinesVfx.PropertyName._dirtBlast4, ref variant6))
			{
				this._dirtBlast4 = variant6.As<GpuParticles2D>();
			}
		}

		// Token: 0x04000A14 RID: 2580
		private Node2D _frontVinesNode;

		// Token: 0x04000A15 RID: 2581
		private MegaSprite _frontVinesAnimController;

		// Token: 0x04000A16 RID: 2582
		private Node2D _backVinesNode;

		// Token: 0x04000A17 RID: 2583
		private MegaSprite _backVinesAnimController;

		// Token: 0x04000A18 RID: 2584
		private GpuParticles2D _dirtBlast1;

		// Token: 0x04000A19 RID: 2585
		private GpuParticles2D _dirtBlast2;

		// Token: 0x04000A1A RID: 2586
		private GpuParticles2D _dirtBlast3;

		// Token: 0x04000A1B RID: 2587
		private GpuParticles2D _dirtBlast4;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200119B RID: 4507
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004493 RID: 17555
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFrontEvent' method.
			/// </summary>
			// Token: 0x04004494 RID: 17556
			public static readonly StringName OnFrontEvent = "OnFrontEvent";

			/// <summary>
			/// Cached name for the 'AnimationEnded' method.
			/// </summary>
			// Token: 0x04004495 RID: 17557
			public static readonly StringName AnimationEnded = "AnimationEnded";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200119C RID: 4508
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_frontVinesNode' field.
			/// </summary>
			// Token: 0x04004496 RID: 17558
			public static readonly StringName _frontVinesNode = "_frontVinesNode";

			/// <summary>
			/// Cached name for the '_backVinesNode' field.
			/// </summary>
			// Token: 0x04004497 RID: 17559
			public static readonly StringName _backVinesNode = "_backVinesNode";

			/// <summary>
			/// Cached name for the '_dirtBlast1' field.
			/// </summary>
			// Token: 0x04004498 RID: 17560
			public static readonly StringName _dirtBlast1 = "_dirtBlast1";

			/// <summary>
			/// Cached name for the '_dirtBlast2' field.
			/// </summary>
			// Token: 0x04004499 RID: 17561
			public static readonly StringName _dirtBlast2 = "_dirtBlast2";

			/// <summary>
			/// Cached name for the '_dirtBlast3' field.
			/// </summary>
			// Token: 0x0400449A RID: 17562
			public static readonly StringName _dirtBlast3 = "_dirtBlast3";

			/// <summary>
			/// Cached name for the '_dirtBlast4' field.
			/// </summary>
			// Token: 0x0400449B RID: 17563
			public static readonly StringName _dirtBlast4 = "_dirtBlast4";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200119D RID: 4509
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
