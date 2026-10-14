using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001A8 RID: 424
	[ScriptPath("res://src/Core/Nodes/Vfx/NFireBurningVfx.cs")]
	public class NFireBurningVfx : Node2D
	{
		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x060014F7 RID: 5367 RVA: 0x00080161 File Offset: 0x0007E361
		private static Color DefaultColor
		{
			get
			{
				return Color.FromHtml("#ff8b57");
			}
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00080174 File Offset: 0x0007E374
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NFireBurningVfx Create(Creature creature, float scaleFactor, bool goingRight)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NFireBurningVfx.Create(ncreature.GetBottomOfHitbox(), scaleFactor, goingRight);
			}
			return null;
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x000801AF File Offset: 0x0007E3AF
		[NullableContext(2)]
		public static NFireBurningVfx Create(Vector2 targetFloorPosition, float scaleFactor, bool goingRight)
		{
			return NFireBurningVfx.Create(targetFloorPosition, scaleFactor, goingRight, NFireBurningVfx.DefaultColor);
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x000801C0 File Offset: 0x0007E3C0
		[NullableContext(2)]
		public static NFireBurningVfx Create(Vector2 targetFloorPosition, float scaleFactor, bool goingRight, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NFireBurningVfx nfireBurningVfx = PreloadManager.Cache.GetScene(NFireBurningVfx.scenePath).Instantiate<NFireBurningVfx>(0L);
			nfireBurningVfx.GlobalPosition = targetFloorPosition;
			nfireBurningVfx.Modulate = tint;
			Vector2 vector = Vector2.One * scaleFactor;
			vector.X *= (goingRight ? 1f : (-1f));
			nfireBurningVfx.Scale = vector;
			return nfireBurningVfx;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00080229 File Offset: 0x0007E429
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00080237 File Offset: 0x0007E437
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0008024C File Offset: 0x0007E44C
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			for (int i = 0; i < this._startParticles.Count; i++)
			{
				this._startParticles[i].Restart();
			}
			await Cmd.Wait(0.3f, this._cts.Token, false);
			for (int j = 0; j < this._endParticles.Count; j++)
			{
				this._endParticles[j].Restart();
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014FE RID: 5374 RVA: 0x00080290 File Offset: 0x0007E490
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NFireBurningVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetFloorPosition", 0L, "", 6L, false),
					new PropertyInfo(3L, "scaleFactor", 0L, "", 6L, false),
					new PropertyInfo(1L, "goingRight", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NFireBurningVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetFloorPosition", 0L, "", 6L, false),
					new PropertyInfo(3L, "scaleFactor", 0L, "", 6L, false),
					new PropertyInfo(1L, "goingRight", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NFireBurningVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFireBurningVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014FF RID: 5375 RVA: 0x0008046C File Offset: 0x0007E66C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFireBurningVfx.MethodName.Create && args.Count == 3)
			{
				NFireBurningVfx nfireBurningVfx = NFireBurningVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<float>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NFireBurningVfx>(ref nfireBurningVfx);
				return true;
			}
			if ((ref method) == NFireBurningVfx.MethodName.Create && args.Count == 4)
			{
				NFireBurningVfx nfireBurningVfx2 = NFireBurningVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<float>(args[1]), VariantUtils.ConvertTo<bool>(args[2]), VariantUtils.ConvertTo<Color>(args[3]));
				ret = VariantUtils.CreateFrom<NFireBurningVfx>(ref nfireBurningVfx2);
				return true;
			}
			if ((ref method) == NFireBurningVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFireBurningVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x00080580 File Offset: 0x0007E780
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFireBurningVfx.MethodName.Create && args.Count == 3)
			{
				NFireBurningVfx nfireBurningVfx = NFireBurningVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<float>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NFireBurningVfx>(ref nfireBurningVfx);
				return true;
			}
			if ((ref method) == NFireBurningVfx.MethodName.Create && args.Count == 4)
			{
				NFireBurningVfx nfireBurningVfx2 = NFireBurningVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<float>(args[1]), VariantUtils.ConvertTo<bool>(args[2]), VariantUtils.ConvertTo<Color>(args[3]));
				ret = VariantUtils.CreateFrom<NFireBurningVfx>(ref nfireBurningVfx2);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001501 RID: 5377 RVA: 0x00080648 File Offset: 0x0007E848
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFireBurningVfx.MethodName.Create || (ref method) == NFireBurningVfx.MethodName._Ready || (ref method) == NFireBurningVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001502 RID: 5378 RVA: 0x0008067E File Offset: 0x0007E87E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFireBurningVfx.PropertyName._startParticles)
			{
				this._startParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NFireBurningVfx.PropertyName._endParticles)
			{
				this._endParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001503 RID: 5379 RVA: 0x000806C0 File Offset: 0x0007E8C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFireBurningVfx.PropertyName._startParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._startParticles);
				return true;
			}
			if ((ref name) == NFireBurningVfx.PropertyName._endParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._endParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001504 RID: 5380 RVA: 0x00080718 File Offset: 0x0007E918
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NFireBurningVfx.PropertyName._startParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NFireBurningVfx.PropertyName._endParticles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001505 RID: 5381 RVA: 0x00080770 File Offset: 0x0007E970
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFireBurningVfx.PropertyName._startParticles, Variant.CreateFrom<GpuParticles2D>(this._startParticles));
			info.AddProperty(NFireBurningVfx.PropertyName._endParticles, Variant.CreateFrom<GpuParticles2D>(this._endParticles));
		}

		/// <inheritdoc />
		// Token: 0x06001506 RID: 5382 RVA: 0x000807A8 File Offset: 0x0007E9A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFireBurningVfx.PropertyName._startParticles, ref variant))
			{
				this._startParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFireBurningVfx.PropertyName._endParticles, ref variant2))
			{
				this._endParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x04000783 RID: 1923
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_fire_burning");

		// Token: 0x04000784 RID: 1924
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _startParticles = new Array<GpuParticles2D>();

		// Token: 0x04000785 RID: 1925
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _endParticles = new Array<GpuParticles2D>();

		// Token: 0x04000786 RID: 1926
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001024 RID: 4132
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003FF7 RID: 16375
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003FF8 RID: 16376
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003FF9 RID: 16377
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001025 RID: 4133
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_startParticles' field.
			/// </summary>
			// Token: 0x04003FFA RID: 16378
			public static readonly StringName _startParticles = "_startParticles";

			/// <summary>
			/// Cached name for the '_endParticles' field.
			/// </summary>
			// Token: 0x04003FFB RID: 16379
			public static readonly StringName _endParticles = "_endParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001026 RID: 4134
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
