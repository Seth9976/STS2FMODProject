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
	// Token: 0x020001C0 RID: 448
	[ScriptPath("res://src/Core/Nodes/Vfx/NIroncladVfx.cs")]
	public class NIroncladVfx : Node
	{
		// Token: 0x06001660 RID: 5728 RVA: 0x00088D28 File Offset: 0x00086F28
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._slashShaderMat = new MegaSlotNode(this._parent.GetNode("SlashVfxSlot")).GetNormalMaterial() as ShaderMaterial;
			this._slashStepBase = (Vector2)this._slashShaderMat.GetShaderParameter(NIroncladVfx._step);
			this._megaSprite = new MegaSprite(this._parent);
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00088DC0 File Offset: 0x00086FC0
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "heavy_slash_start")
			{
				this.OnHeavySlash();
				return;
			}
			if (!(eventName == "attack_slash_start"))
			{
				return;
			}
			this.OnAttackSlash();
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00088E0C File Offset: 0x0008700C
		private void OnHeavySlash()
		{
			ShaderMaterial slashShaderMat = this._slashShaderMat;
			if (slashShaderMat != null)
			{
				slashShaderMat.SetShaderParameter(NIroncladVfx._step, this._slashStepBase);
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetEase(0L).SetTrans(7L);
			Vector2 vector;
			vector..ctor(1f, 1.02f);
			this._tween.TweenProperty(this._slashShaderMat, "shader_parameter/step", vector, 0.3499999940395355);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00088EA4 File Offset: 0x000870A4
		private void OnAttackSlash()
		{
			ShaderMaterial slashShaderMat = this._slashShaderMat;
			if (slashShaderMat != null)
			{
				slashShaderMat.SetShaderParameter(NIroncladVfx._step, this._slashStepBase);
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetEase(0L).SetTrans(4L);
			Vector2 vector;
			vector..ctor(1f, 1.02f);
			this._tween.TweenInterval(0.15000000596046448);
			this._tween.TweenProperty(this._slashShaderMat, "shader_parameter/step", vector, 0.20000000298023224);
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00088F4E File Offset: 0x0008714E
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001665 RID: 5733 RVA: 0x00088F60 File Offset: 0x00087160
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NIroncladVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIroncladVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NIroncladVfx.MethodName.OnHeavySlash, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIroncladVfx.MethodName.OnAttackSlash, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIroncladVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001666 RID: 5734 RVA: 0x00089114 File Offset: 0x00087314
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NIroncladVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIroncladVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIroncladVfx.MethodName.OnHeavySlash && args.Count == 0)
			{
				this.OnHeavySlash();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIroncladVfx.MethodName.OnAttackSlash && args.Count == 0)
			{
				this.OnAttackSlash();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIroncladVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001667 RID: 5735 RVA: 0x00089218 File Offset: 0x00087418
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NIroncladVfx.MethodName._Ready || (ref method) == NIroncladVfx.MethodName.OnAnimationEvent || (ref method) == NIroncladVfx.MethodName.OnHeavySlash || (ref method) == NIroncladVfx.MethodName.OnAttackSlash || (ref method) == NIroncladVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001668 RID: 5736 RVA: 0x00089278 File Offset: 0x00087478
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NIroncladVfx.PropertyName._slashStepBase)
			{
				this._slashStepBase = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NIroncladVfx.PropertyName._slashShaderMat)
			{
				this._slashShaderMat = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NIroncladVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NIroncladVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001669 RID: 5737 RVA: 0x000892FC File Offset: 0x000874FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NIroncladVfx.PropertyName._slashStepBase)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._slashStepBase);
				return true;
			}
			if ((ref name) == NIroncladVfx.PropertyName._slashShaderMat)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._slashShaderMat);
				return true;
			}
			if ((ref name) == NIroncladVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NIroncladVfx.PropertyName._parent)
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
		// Token: 0x0600166A RID: 5738 RVA: 0x00089394 File Offset: 0x00087594
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NIroncladVfx.PropertyName._slashStepBase, 0L, "", 4096L, false),
				new PropertyInfo(24L, NIroncladVfx.PropertyName._slashShaderMat, 0L, "", 4096L, false),
				new PropertyInfo(24L, NIroncladVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NIroncladVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600166B RID: 5739 RVA: 0x0008942C File Offset: 0x0008762C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NIroncladVfx.PropertyName._slashStepBase, Variant.From<Vector2>(ref this._slashStepBase));
			info.AddProperty(NIroncladVfx.PropertyName._slashShaderMat, Variant.From<ShaderMaterial>(ref this._slashShaderMat));
			info.AddProperty(NIroncladVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NIroncladVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x0600166C RID: 5740 RVA: 0x00089498 File Offset: 0x00087698
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NIroncladVfx.PropertyName._slashStepBase, ref variant))
			{
				this._slashStepBase = variant.As<Vector2>();
			}
			Variant variant2;
			if (info.TryGetProperty(NIroncladVfx.PropertyName._slashShaderMat, ref variant2))
			{
				this._slashShaderMat = variant2.As<ShaderMaterial>();
			}
			Variant variant3;
			if (info.TryGetProperty(NIroncladVfx.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NIroncladVfx.PropertyName._parent, ref variant4))
			{
				this._parent = variant4.As<Node2D>();
			}
		}

		// Token: 0x040007F1 RID: 2033
		[Nullable(1)]
		private static readonly StringName _step = new StringName("step");

		// Token: 0x040007F2 RID: 2034
		private Vector2 _slashStepBase;

		// Token: 0x040007F3 RID: 2035
		[Nullable(2)]
		private ShaderMaterial _slashShaderMat;

		// Token: 0x040007F4 RID: 2036
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040007F5 RID: 2037
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x040007F6 RID: 2038
		[Nullable(1)]
		private MegaSprite _megaSprite;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200107D RID: 4221
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040040D6 RID: 16598
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040040D7 RID: 16599
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnHeavySlash' method.
			/// </summary>
			// Token: 0x040040D8 RID: 16600
			public static readonly StringName OnHeavySlash = "OnHeavySlash";

			/// <summary>
			/// Cached name for the 'OnAttackSlash' method.
			/// </summary>
			// Token: 0x040040D9 RID: 16601
			public static readonly StringName OnAttackSlash = "OnAttackSlash";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040040DA RID: 16602
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200107E RID: 4222
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_slashStepBase' field.
			/// </summary>
			// Token: 0x040040DB RID: 16603
			public static readonly StringName _slashStepBase = "_slashStepBase";

			/// <summary>
			/// Cached name for the '_slashShaderMat' field.
			/// </summary>
			// Token: 0x040040DC RID: 16604
			public static readonly StringName _slashShaderMat = "_slashShaderMat";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040040DD RID: 16605
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x040040DE RID: 16606
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200107F RID: 4223
		public class SignalName : Node.SignalName
		{
		}
	}
}
