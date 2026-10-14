using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Makes the word Blocked appear over the enemy if attack damage is fully Blocked.
	/// </summary>
	// Token: 0x02000199 RID: 409
	[ScriptPath("res://src/Core/Nodes/Vfx/NDamageBlockedVfx.cs")]
	public class NDamageBlockedVfx : Node2D
	{
		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x000795EA File Offset: 0x000777EA
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_blocked_text");
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x000795F6 File Offset: 0x000777F6
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NDamageBlockedVfx.ScenePath);
			}
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00079604 File Offset: 0x00077804
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NDamageBlockedVfx Create(Creature target)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
			if (!creatureNode.IsInteractable)
			{
				return null;
			}
			NDamageBlockedVfx ndamageBlockedVfx = PreloadManager.Cache.GetScene(NDamageBlockedVfx.ScenePath).Instantiate<NDamageBlockedVfx>(0L);
			ndamageBlockedVfx.GlobalPosition = creatureNode.VfxSpawnPosition + new Vector2(Rng.Chaotic.NextFloat(-20f, 20f), Rng.Chaotic.NextFloat(-60f, -40f));
			ndamageBlockedVfx.RotationDegrees = Rng.Chaotic.NextFloat(-2f, 2f);
			return ndamageBlockedVfx;
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x000796A0 File Offset: 0x000778A0
		public override void _Ready()
		{
			this._label = base.GetNode<MegaLabel>("Label");
			this._label.SetTextAutoSize(NDamageBlockedVfx._blockedLoc.GetRawText());
			TaskHelper.RunSafely(this.BlockAnim());
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x000796D9 File Offset: 0x000778D9
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x000796EC File Offset: 0x000778EC
		[NullableContext(1)]
		private async Task BlockAnim()
		{
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 0.6f, 2.0).From(Vector2.One).SetEase(1L)
				.SetTrans(4L);
			this._tween.TweenProperty(this, "position:y", base.Position.Y - 250f, 2.0).SetEase(1L).SetTrans(4L);
			this._tween.TweenProperty(this._label, "modulate", Colors.White, 2.0).From(new Color("21C0FF")).SetEase(1L)
				.SetTrans(7L);
			this._tween.TweenProperty(this, "modulate:a", 0f, 1.5).SetEase(0L).SetTrans(1L)
				.SetDelay(0.5);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600141C RID: 5148 RVA: 0x00079730 File Offset: 0x00077930
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NDamageBlockedVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDamageBlockedVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600141D RID: 5149 RVA: 0x000797A4 File Offset: 0x000779A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDamageBlockedVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDamageBlockedVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600141E RID: 5150 RVA: 0x00079804 File Offset: 0x00077A04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDamageBlockedVfx.MethodName._Ready || (ref method) == NDamageBlockedVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600141F RID: 5151 RVA: 0x0007982B File Offset: 0x00077A2B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDamageBlockedVfx.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDamageBlockedVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001420 RID: 5152 RVA: 0x0007986C File Offset: 0x00077A6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDamageBlockedVfx.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NDamageBlockedVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001421 RID: 5153 RVA: 0x000798C4 File Offset: 0x00077AC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDamageBlockedVfx.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDamageBlockedVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001422 RID: 5154 RVA: 0x0007991A File Offset: 0x00077B1A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDamageBlockedVfx.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NDamageBlockedVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001423 RID: 5155 RVA: 0x00079950 File Offset: 0x00077B50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDamageBlockedVfx.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDamageBlockedVfx.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
		}

		// Token: 0x0400072D RID: 1837
		[Nullable(1)]
		private static readonly LocString _blockedLoc = new LocString("vfx", "BLOCKED");

		// Token: 0x0400072E RID: 1838
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x0400072F RID: 1839
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FEE RID: 4078
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F51 RID: 16209
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003F52 RID: 16210
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FEF RID: 4079
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04003F53 RID: 16211
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003F54 RID: 16212
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FF0 RID: 4080
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
