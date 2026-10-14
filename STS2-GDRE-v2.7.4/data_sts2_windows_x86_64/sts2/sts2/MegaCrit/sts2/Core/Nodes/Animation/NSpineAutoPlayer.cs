using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Animation
{
	/// <summary>
	/// A node that automatically plays a single animation on a SpineSprite.
	/// There are two conditions that must be true when using this node:
	///
	/// 1. This node must be the direct child of a SpineSprite node.
	/// 2. The parent SpineSprite must have exactly 1 animation.
	///
	/// The animation is started once the parent SpineSprite's skeleton is ready.
	/// If condition 2 isn't met, this node will throw an exception.
	/// </summary>
	// Token: 0x02000426 RID: 1062
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Animation/NSpineAutoPlayer.cs")]
	public class NSpineAutoPlayer : Node
	{
		// Token: 0x060043D9 RID: 17369 RVA: 0x0020F520 File Offset: 0x0020D720
		public override void _Ready()
		{
			MegaSprite sprite = new MegaSprite(base.GetParent());
			this.RunWhenSpineReady(sprite, delegate(MegaAnimationState animState)
			{
				IReadOnlyList<string> animationNames = sprite.GetSkeleton().GetData().GetAnimationNames();
				if (animationNames.Count != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 2);
					defaultInterpolatedStringHandler.AppendFormatted("NSpineAutoPlayer");
					defaultInterpolatedStringHandler.AppendLiteral("'s parent's skeleton data must have exactly 1 animation. ");
					defaultInterpolatedStringHandler.AppendLiteral("This has ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(animationNames.Count);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				animState.SetAnimation(animationNames[0], true, 0);
			});
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060043DA RID: 17370 RVA: 0x0020F564 File Offset: 0x0020D764
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NSpineAutoPlayer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060043DB RID: 17371 RVA: 0x0020F5A8 File Offset: 0x0020D7A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSpineAutoPlayer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060043DC RID: 17372 RVA: 0x0020F5D8 File Offset: 0x0020D7D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSpineAutoPlayer.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060043DD RID: 17373 RVA: 0x0020F5F0 File Offset: 0x0020D7F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060043DE RID: 17374 RVA: 0x0020F5F9 File Offset: 0x0020D7F9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001988 RID: 6536
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040064E7 RID: 25831
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001989 RID: 6537
		public class PropertyName : Node.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200198A RID: 6538
		public class SignalName : Node.SignalName
		{
		}
	}
}
