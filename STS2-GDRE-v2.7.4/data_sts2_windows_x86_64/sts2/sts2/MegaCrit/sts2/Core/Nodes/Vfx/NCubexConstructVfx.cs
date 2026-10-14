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
	// Token: 0x02000196 RID: 406
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NCubexConstructVfx.cs")]
	public class NCubexConstructVfx : Node
	{
		// Token: 0x060013E6 RID: 5094 RVA: 0x00078214 File Offset: 0x00076414
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._laser = this._parent.GetNode<NLaserVfx>("LaserBone/Laser");
			this._rings = this._parent.GetNode<Node2D>("LaserBone/Rings");
			this._rings.Visible = false;
			this._laser.Scale = Vector2.One / this._parent.Scale;
			this._laser.Visible = false;
			this._laser.ResetLaser();
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				MegaSkeleton skeleton = this._animController.GetSkeleton();
				this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
				MegaSkin megaSkin = this._animController.NewSkin("newSkin");
				megaSkin.AddSkin(skeleton.GetData().FindSkin("moss3"));
				skeleton.SetSkin(megaSkin);
				animState.SetAnimation("idle_loop", true, 0);
			});
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x000782D4 File Offset: 0x000764D4
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "laser_start")
			{
				this.StartLaser();
				return;
			}
			if (!(eventName == "laser_end"))
			{
				return;
			}
			this.EndLaser();
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00078320 File Offset: 0x00076520
		private void StartLaser()
		{
			this._laser.Visible = true;
			this._laser.ExtendLaser(new Vector2(this._laser.Position.X - 3000f, this._parent.Position.Y));
			Vector2 scale = this._rings.Scale;
			this._rings.Scale = Vector2.Zero;
			this._rings.Visible = true;
			Tween tween = base.CreateTween();
			tween.TweenProperty(this._rings, "scale", scale, 0.4000000059604645).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x000783D2 File Offset: 0x000765D2
		private void EndLaser()
		{
			this._laser.RetractLaser();
			this._rings.Visible = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060013EA RID: 5098 RVA: 0x000783EC File Offset: 0x000765EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCubexConstructVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCubexConstructVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NCubexConstructVfx.MethodName.StartLaser, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCubexConstructVfx.MethodName.EndLaser, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013EB RID: 5099 RVA: 0x00078574 File Offset: 0x00076774
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCubexConstructVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCubexConstructVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCubexConstructVfx.MethodName.StartLaser && args.Count == 0)
			{
				this.StartLaser();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCubexConstructVfx.MethodName.EndLaser && args.Count == 0)
			{
				this.EndLaser();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060013EC RID: 5100 RVA: 0x00078654 File Offset: 0x00076854
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCubexConstructVfx.MethodName._Ready || (ref method) == NCubexConstructVfx.MethodName.OnAnimationEvent || (ref method) == NCubexConstructVfx.MethodName.StartLaser || (ref method) == NCubexConstructVfx.MethodName.EndLaser || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060013ED RID: 5101 RVA: 0x000786A4 File Offset: 0x000768A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCubexConstructVfx.PropertyName._laser)
			{
				this._laser = VariantUtils.ConvertTo<NLaserVfx>(ref value);
				return true;
			}
			if ((ref name) == NCubexConstructVfx.PropertyName._rings)
			{
				this._rings = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCubexConstructVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060013EE RID: 5102 RVA: 0x0007870C File Offset: 0x0007690C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCubexConstructVfx.PropertyName._laser)
			{
				value = VariantUtils.CreateFrom<NLaserVfx>(ref this._laser);
				return true;
			}
			if ((ref name) == NCubexConstructVfx.PropertyName._rings)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._rings);
				return true;
			}
			if ((ref name) == NCubexConstructVfx.PropertyName._parent)
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
		// Token: 0x060013EF RID: 5103 RVA: 0x00078784 File Offset: 0x00076984
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCubexConstructVfx.PropertyName._laser, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCubexConstructVfx.PropertyName._rings, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCubexConstructVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013F0 RID: 5104 RVA: 0x000787FC File Offset: 0x000769FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCubexConstructVfx.PropertyName._laser, Variant.From<NLaserVfx>(ref this._laser));
			info.AddProperty(NCubexConstructVfx.PropertyName._rings, Variant.From<Node2D>(ref this._rings));
			info.AddProperty(NCubexConstructVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x060013F1 RID: 5105 RVA: 0x00078854 File Offset: 0x00076A54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCubexConstructVfx.PropertyName._laser, ref variant))
			{
				this._laser = variant.As<NLaserVfx>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCubexConstructVfx.PropertyName._rings, ref variant2))
			{
				this._rings = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCubexConstructVfx.PropertyName._parent, ref variant3))
			{
				this._parent = variant3.As<Node2D>();
			}
		}

		// Token: 0x0400071F RID: 1823
		[Nullable(1)]
		private NLaserVfx _laser;

		// Token: 0x04000720 RID: 1824
		[Nullable(1)]
		private Node2D _rings;

		// Token: 0x04000721 RID: 1825
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000722 RID: 1826
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FE3 RID: 4067
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F35 RID: 16181
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04003F36 RID: 16182
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartLaser' method.
			/// </summary>
			// Token: 0x04003F37 RID: 16183
			public static readonly StringName StartLaser = "StartLaser";

			/// <summary>
			/// Cached name for the 'EndLaser' method.
			/// </summary>
			// Token: 0x04003F38 RID: 16184
			public static readonly StringName EndLaser = "EndLaser";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FE4 RID: 4068
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_laser' field.
			/// </summary>
			// Token: 0x04003F39 RID: 16185
			public static readonly StringName _laser = "_laser";

			/// <summary>
			/// Cached name for the '_rings' field.
			/// </summary>
			// Token: 0x04003F3A RID: 16186
			public static readonly StringName _rings = "_rings";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04003F3B RID: 16187
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FE5 RID: 4069
		public class SignalName : Node.SignalName
		{
		}
	}
}
