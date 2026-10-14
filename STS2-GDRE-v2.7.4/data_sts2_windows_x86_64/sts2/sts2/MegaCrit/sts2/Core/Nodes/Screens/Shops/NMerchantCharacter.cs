using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops
{
	// Token: 0x0200027C RID: 636
	[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantCharacter.cs")]
	public class NMerchantCharacter : Node2D
	{
		// Token: 0x060021B6 RID: 8630 RVA: 0x000E5B5C File Offset: 0x000E3D5C
		public override void _Ready()
		{
			this.RunWhenSpineReady(new MegaSprite(base.GetChild(0, false)), delegate(MegaAnimationState _)
			{
				this.PlayAnimation("relaxed_loop", true);
			});
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x000E5B84 File Offset: 0x000E3D84
		[NullableContext(1)]
		public void PlayAnimation(string anim, bool loop = false)
		{
			MegaAnimationState animationState = new MegaSprite(base.GetChild(0, false)).GetAnimationState();
			animationState.SetAnimation(anim, loop, 0);
			if (loop)
			{
				using (MegaTrackEntry current = animationState.GetCurrent(0))
				{
					if (current != null)
					{
						current.SetTrackTime(current.GetAnimationEnd() * Rng.Chaotic.NextFloat(1f));
					}
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060021B8 RID: 8632 RVA: 0x000E5BFC File Offset: 0x000E3DFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NMerchantCharacter.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantCharacter.MethodName.PlayAnimation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "anim", 0L, "", 6L, false),
					new PropertyInfo(1L, "loop", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060021B9 RID: 8633 RVA: 0x000E5CB8 File Offset: 0x000E3EB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantCharacter.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantCharacter.MethodName.PlayAnimation && args.Count == 2)
			{
				this.PlayAnimation(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060021BA RID: 8634 RVA: 0x000E5D33 File Offset: 0x000E3F33
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantCharacter.MethodName._Ready || (ref method) == NMerchantCharacter.MethodName.PlayAnimation || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060021BB RID: 8635 RVA: 0x000E5D5A File Offset: 0x000E3F5A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060021BC RID: 8636 RVA: 0x000E5D63 File Offset: 0x000E3F63
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200131E RID: 4894
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004A4E RID: 19022
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'PlayAnimation' method.
			/// </summary>
			// Token: 0x04004A4F RID: 19023
			public static readonly StringName PlayAnimation = "PlayAnimation";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200131F RID: 4895
		public class PropertyName : Node2D.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001320 RID: 4896
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
