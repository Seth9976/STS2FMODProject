using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	/// <summary>
	/// Many of this VFX are spawned via NHellraiserVfx.
	/// This represents a single sword particle and maybe useful for re-use if we want these to spawn when the
	/// Hellraiser power procs.
	/// </summary>
	// Token: 0x02000238 RID: 568
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NHellraiserAttackVfx.cs")]
	public class NHellraiserAttackVfx : Node2D
	{
		// Token: 0x06001CE9 RID: 7401 RVA: 0x000BC4D8 File Offset: 0x000BA6D8
		public override void _Ready()
		{
			TextureRect node = base.GetNode<TextureRect>("%Sword");
			node.FlipH = Rng.Chaotic.NextBool();
			Tween tween = base.CreateTween().SetParallel(true);
			tween.TweenProperty(node, "position:y", 90, 0.5).SetEase(1L).SetTrans(5L);
			tween.TweenProperty(this, "modulate", Colors.White, 0.30000001192092896).From(Colors.Red);
			tween.Chain().TweenInterval(0.10000000149011612);
			tween.Chain();
			tween.TweenProperty(node, "position:y", 300, 0.5).SetEase(0L).SetTrans(5L);
			tween.TweenProperty(this, "modulate:a", 0f, 0.30000001192092896);
			tween.Chain().TweenCallback(Callable.From(new Action(this.OnTweenFinished)));
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x000BC608 File Offset: 0x000BA808
		private void OnTweenFinished()
		{
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001CEB RID: 7403 RVA: 0x000BC610 File Offset: 0x000BA810
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NHellraiserAttackVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHellraiserAttackVfx.MethodName.OnTweenFinished, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CEC RID: 7404 RVA: 0x000BC684 File Offset: 0x000BA884
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHellraiserAttackVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHellraiserAttackVfx.MethodName.OnTweenFinished && args.Count == 0)
			{
				this.OnTweenFinished();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001CED RID: 7405 RVA: 0x000BC6E4 File Offset: 0x000BA8E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHellraiserAttackVfx.MethodName._Ready || (ref method) == NHellraiserAttackVfx.MethodName.OnTweenFinished || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001CEE RID: 7406 RVA: 0x000BC70B File Offset: 0x000BA90B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06001CEF RID: 7407 RVA: 0x000BC714 File Offset: 0x000BA914
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001212 RID: 4626
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400461C RID: 17948
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnTweenFinished' method.
			/// </summary>
			// Token: 0x0400461D RID: 17949
			public static readonly StringName OnTweenFinished = "OnTweenFinished";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001213 RID: 4627
		public class PropertyName : Node2D.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001214 RID: 4628
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
