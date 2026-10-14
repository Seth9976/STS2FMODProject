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
	// Token: 0x020001F6 RID: 502
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NSoulFyshVfx.cs")]
	public class NSoulFyshVfx : Node
	{
		// Token: 0x060019AD RID: 6573 RVA: 0x000A216C File Offset: 0x000A036C
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._soundSlotNode = new MegaSlotNode(this._parent.GetNode("Soundwave"));
			this._soundShaderMat = this._soundSlotNode.GetNormalMaterial() as ShaderMaterial;
			ShaderMaterial soundShaderMat = this._soundShaderMat;
			if (soundShaderMat != null)
			{
				soundShaderMat.SetShaderParameter(NSoulFyshVfx._amount, 0.3f);
			}
			this._beckonSlotNode = new MegaSlotNode(this._parent.GetNode("Beckonwave"));
			this._beckonShaderMat = this._beckonSlotNode.GetNormalMaterial() as ShaderMaterial;
			ShaderMaterial beckonShaderMat = this._beckonShaderMat;
			if (beckonShaderMat != null)
			{
				beckonShaderMat.SetShaderParameter(NSoulFyshVfx._amount, 0.3f);
			}
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("attack_debuff", true, 0);
			});
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x000A229C File Offset: 0x000A049C
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "soundwave_start")
			{
				this.StartSoundwave();
				return;
			}
			if (eventName == "soundwave_end")
			{
				this.EndSoundwave();
				return;
			}
			if (eventName == "beckon_start")
			{
				this.StartBeckon();
				return;
			}
			if (!(eventName == "beckon_end"))
			{
				return;
			}
			this.EndBeckon();
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x000A2310 File Offset: 0x000A0510
		private void StartSoundwave()
		{
			Tween tween = base.CreateTween();
			tween.SetEase(1L);
			tween.SetTrans(4L);
			tween.TweenProperty(this._soundShaderMat, "shader_parameter/amount", 1f, 0.44999998807907104);
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x000A2360 File Offset: 0x000A0560
		private void EndSoundwave()
		{
			Tween tween = base.CreateTween();
			tween.SetEase(0L);
			tween.SetTrans(4L);
			tween.TweenProperty(this._soundShaderMat, "shader_parameter/amount", 0.3f, 0.5);
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x000A23B0 File Offset: 0x000A05B0
		private void StartBeckon()
		{
			Tween tween = base.CreateTween();
			tween.SetEase(1L);
			tween.SetTrans(4L);
			tween.TweenProperty(this._beckonShaderMat, "shader_parameter/amount", 1f, 0.25);
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x000A2400 File Offset: 0x000A0600
		private void EndBeckon()
		{
			Tween tween = base.CreateTween();
			tween.SetEase(0L);
			tween.SetTrans(4L);
			tween.TweenProperty(this._beckonShaderMat, "shader_parameter/amount", 0.3f, 0.5);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060019B3 RID: 6579 RVA: 0x000A2450 File Offset: 0x000A0650
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NSoulFyshVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSoulFyshVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NSoulFyshVfx.MethodName.StartSoundwave, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSoulFyshVfx.MethodName.EndSoundwave, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSoulFyshVfx.MethodName.StartBeckon, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSoulFyshVfx.MethodName.EndBeckon, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060019B4 RID: 6580 RVA: 0x000A2634 File Offset: 0x000A0834
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSoulFyshVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSoulFyshVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSoulFyshVfx.MethodName.StartSoundwave && args.Count == 0)
			{
				this.StartSoundwave();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSoulFyshVfx.MethodName.EndSoundwave && args.Count == 0)
			{
				this.EndSoundwave();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSoulFyshVfx.MethodName.StartBeckon && args.Count == 0)
			{
				this.StartBeckon();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSoulFyshVfx.MethodName.EndBeckon && args.Count == 0)
			{
				this.EndBeckon();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060019B5 RID: 6581 RVA: 0x000A2760 File Offset: 0x000A0960
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSoulFyshVfx.MethodName._Ready || (ref method) == NSoulFyshVfx.MethodName.OnAnimationEvent || (ref method) == NSoulFyshVfx.MethodName.StartSoundwave || (ref method) == NSoulFyshVfx.MethodName.EndSoundwave || (ref method) == NSoulFyshVfx.MethodName.StartBeckon || (ref method) == NSoulFyshVfx.MethodName.EndBeckon || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060019B6 RID: 6582 RVA: 0x000A27D0 File Offset: 0x000A09D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSoulFyshVfx.PropertyName._soundShaderMat)
			{
				this._soundShaderMat = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NSoulFyshVfx.PropertyName._beckonShaderMat)
			{
				this._beckonShaderMat = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NSoulFyshVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060019B7 RID: 6583 RVA: 0x000A2838 File Offset: 0x000A0A38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSoulFyshVfx.PropertyName._soundShaderMat)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._soundShaderMat);
				return true;
			}
			if ((ref name) == NSoulFyshVfx.PropertyName._beckonShaderMat)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._beckonShaderMat);
				return true;
			}
			if ((ref name) == NSoulFyshVfx.PropertyName._parent)
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
		// Token: 0x060019B8 RID: 6584 RVA: 0x000A28B0 File Offset: 0x000A0AB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSoulFyshVfx.PropertyName._soundShaderMat, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSoulFyshVfx.PropertyName._beckonShaderMat, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSoulFyshVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060019B9 RID: 6585 RVA: 0x000A2928 File Offset: 0x000A0B28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSoulFyshVfx.PropertyName._soundShaderMat, Variant.From<ShaderMaterial>(ref this._soundShaderMat));
			info.AddProperty(NSoulFyshVfx.PropertyName._beckonShaderMat, Variant.From<ShaderMaterial>(ref this._beckonShaderMat));
			info.AddProperty(NSoulFyshVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x060019BA RID: 6586 RVA: 0x000A2980 File Offset: 0x000A0B80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSoulFyshVfx.PropertyName._soundShaderMat, ref variant))
			{
				this._soundShaderMat = variant.As<ShaderMaterial>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSoulFyshVfx.PropertyName._beckonShaderMat, ref variant2))
			{
				this._beckonShaderMat = variant2.As<ShaderMaterial>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSoulFyshVfx.PropertyName._parent, ref variant3))
			{
				this._parent = variant3.As<Node2D>();
			}
		}

		// Token: 0x04000960 RID: 2400
		[Nullable(1)]
		private static readonly StringName _amount = new StringName("amount");

		// Token: 0x04000961 RID: 2401
		[Nullable(2)]
		private ShaderMaterial _soundShaderMat;

		// Token: 0x04000962 RID: 2402
		[Nullable(1)]
		private MegaSlotNode _soundSlotNode;

		// Token: 0x04000963 RID: 2403
		[Nullable(2)]
		private ShaderMaterial _beckonShaderMat;

		// Token: 0x04000964 RID: 2404
		[Nullable(1)]
		private MegaSlotNode _beckonSlotNode;

		// Token: 0x04000965 RID: 2405
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000966 RID: 2406
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001145 RID: 4421
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400435A RID: 17242
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x0400435B RID: 17243
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartSoundwave' method.
			/// </summary>
			// Token: 0x0400435C RID: 17244
			public static readonly StringName StartSoundwave = "StartSoundwave";

			/// <summary>
			/// Cached name for the 'EndSoundwave' method.
			/// </summary>
			// Token: 0x0400435D RID: 17245
			public static readonly StringName EndSoundwave = "EndSoundwave";

			/// <summary>
			/// Cached name for the 'StartBeckon' method.
			/// </summary>
			// Token: 0x0400435E RID: 17246
			public static readonly StringName StartBeckon = "StartBeckon";

			/// <summary>
			/// Cached name for the 'EndBeckon' method.
			/// </summary>
			// Token: 0x0400435F RID: 17247
			public static readonly StringName EndBeckon = "EndBeckon";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001146 RID: 4422
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_soundShaderMat' field.
			/// </summary>
			// Token: 0x04004360 RID: 17248
			public static readonly StringName _soundShaderMat = "_soundShaderMat";

			/// <summary>
			/// Cached name for the '_beckonShaderMat' field.
			/// </summary>
			// Token: 0x04004361 RID: 17249
			public static readonly StringName _beckonShaderMat = "_beckonShaderMat";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004362 RID: 17250
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001147 RID: 4423
		public class SignalName : Node.SignalName
		{
		}
	}
}
