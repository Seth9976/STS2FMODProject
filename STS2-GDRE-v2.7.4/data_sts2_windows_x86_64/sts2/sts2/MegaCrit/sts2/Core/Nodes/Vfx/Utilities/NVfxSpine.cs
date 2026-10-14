using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	// Token: 0x0200021E RID: 542
	[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NVfxSpine.cs")]
	public class NVfxSpine : Node2D
	{
		// Token: 0x06001BBE RID: 7102 RVA: 0x000B44C8 File Offset: 0x000B26C8
		public override void _Ready()
		{
			MegaSprite megaSprite = new MegaSprite(base.GetNode("SpineSprite"));
			megaSprite.ConnectAnimationCompleted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.AnimationEnded)));
			this.RunWhenSpineReady(megaSprite, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation(this._animation, false, 0);
			});
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000B451B File Offset: 0x000B271B
		[NullableContext(1)]
		private void AnimationEnded(GodotObject _, GodotObject __, GodotObject ___)
		{
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BC0 RID: 7104 RVA: 0x000B4524 File Offset: 0x000B2724
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NVfxSpine.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVfxSpine.MethodName.AnimationEnded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001BC1 RID: 7105 RVA: 0x000B4620 File Offset: 0x000B2820
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NVfxSpine.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVfxSpine.MethodName.AnimationEnded && args.Count == 3)
			{
				this.AnimationEnded(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001BC2 RID: 7106 RVA: 0x000B46A8 File Offset: 0x000B28A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NVfxSpine.MethodName._Ready || (ref method) == NVfxSpine.MethodName.AnimationEnded || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001BC3 RID: 7107 RVA: 0x000B46CF File Offset: 0x000B28CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NVfxSpine.PropertyName._animation)
			{
				this._animation = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001BC4 RID: 7108 RVA: 0x000B46F4 File Offset: 0x000B28F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NVfxSpine.PropertyName._animation)
			{
				value = VariantUtils.CreateFrom<string>(ref this._animation);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BC5 RID: 7109 RVA: 0x000B4720 File Offset: 0x000B2920
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NVfxSpine.PropertyName._animation, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001BC6 RID: 7110 RVA: 0x000B4754 File Offset: 0x000B2954
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NVfxSpine.PropertyName._animation, Variant.From<string>(ref this._animation));
		}

		/// <inheritdoc />
		// Token: 0x06001BC7 RID: 7111 RVA: 0x000B4774 File Offset: 0x000B2974
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NVfxSpine.PropertyName._animation, ref variant))
			{
				this._animation = variant.As<string>();
			}
		}

		// Token: 0x04000A78 RID: 2680
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private string _animation;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011C3 RID: 4547
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400451E RID: 17694
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'AnimationEnded' method.
			/// </summary>
			// Token: 0x0400451F RID: 17695
			public static readonly StringName AnimationEnded = "AnimationEnded";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011C4 RID: 4548
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_animation' field.
			/// </summary>
			// Token: 0x04004520 RID: 17696
			public static readonly StringName _animation = "_animation";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011C5 RID: 4549
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
