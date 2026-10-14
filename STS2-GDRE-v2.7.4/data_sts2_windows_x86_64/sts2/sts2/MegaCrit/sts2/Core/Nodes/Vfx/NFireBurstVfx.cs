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
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001A9 RID: 425
	[ScriptPath("res://src/Core/Nodes/Vfx/NFireBurstVfx.cs")]
	public class NFireBurstVfx : Node2D
	{
		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x00080823 File Offset: 0x0007EA23
		private static Color DefaultColor
		{
			get
			{
				return Color.FromHtml("#ff8b57");
			}
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00080834 File Offset: 0x0007EA34
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NFireBurstVfx Create(Creature creature, float scaleFactor)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NFireBurstVfx.Create(ncreature.GetBottomOfHitbox(), scaleFactor, NFireBurstVfx.DefaultColor);
			}
			return null;
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x00080873 File Offset: 0x0007EA73
		[NullableContext(2)]
		public static NFireBurstVfx Create(Vector2 targetFloorPosition, float scaleFactor)
		{
			return NFireBurstVfx.Create(targetFloorPosition, scaleFactor, NFireBurstVfx.DefaultColor);
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00080884 File Offset: 0x0007EA84
		[NullableContext(2)]
		public static NFireBurstVfx Create(Vector2 targetFloorPosition, float scaleFactor, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NFireBurstVfx nfireBurstVfx = PreloadManager.Cache.GetScene(NFireBurstVfx.scenePath).Instantiate<NFireBurstVfx>(0L);
			nfireBurstVfx.GlobalPosition = targetFloorPosition;
			nfireBurstVfx.ApplyTint(tint);
			Vector2 vector = Vector2.One * scaleFactor;
			vector.X *= (((double)GD.Randf() > 0.5) ? 1f : (-1f));
			nfireBurstVfx.Scale = vector;
			return nfireBurstVfx;
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x000808FB File Offset: 0x0007EAFB
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x00080909 File Offset: 0x0007EB09
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0008091C File Offset: 0x0007EB1C
		public void ApplyTint(Color tint)
		{
			for (int i = 0; i < this._modulateParticles.Count; i++)
			{
				this._modulateParticles[i].SelfModulate = tint;
			}
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00080954 File Offset: 0x0007EB54
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			for (int i = 0; i < this._particles.Count; i++)
			{
				this._particles[i].Restart();
			}
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Weak, ShakeDuration.Normal, -1f);
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001511 RID: 5393 RVA: 0x00080998 File Offset: 0x0007EB98
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NFireBurstVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetFloorPosition", 0L, "", 6L, false),
					new PropertyInfo(3L, "scaleFactor", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NFireBurstVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetFloorPosition", 0L, "", 6L, false),
					new PropertyInfo(3L, "scaleFactor", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NFireBurstVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFireBurstVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFireBurstVfx.MethodName.ApplyTint, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001512 RID: 5394 RVA: 0x00080B84 File Offset: 0x0007ED84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFireBurstVfx.MethodName.Create && args.Count == 2)
			{
				NFireBurstVfx nfireBurstVfx = NFireBurstVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = VariantUtils.CreateFrom<NFireBurstVfx>(ref nfireBurstVfx);
				return true;
			}
			if ((ref method) == NFireBurstVfx.MethodName.Create && args.Count == 3)
			{
				NFireBurstVfx nfireBurstVfx2 = NFireBurstVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<float>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<NFireBurstVfx>(ref nfireBurstVfx2);
				return true;
			}
			if ((ref method) == NFireBurstVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFireBurstVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFireBurstVfx.MethodName.ApplyTint && args.Count == 1)
			{
				this.ApplyTint(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00080CB0 File Offset: 0x0007EEB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFireBurstVfx.MethodName.Create && args.Count == 2)
			{
				NFireBurstVfx nfireBurstVfx = NFireBurstVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = VariantUtils.CreateFrom<NFireBurstVfx>(ref nfireBurstVfx);
				return true;
			}
			if ((ref method) == NFireBurstVfx.MethodName.Create && args.Count == 3)
			{
				NFireBurstVfx nfireBurstVfx2 = NFireBurstVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<float>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<NFireBurstVfx>(ref nfireBurstVfx2);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001514 RID: 5396 RVA: 0x00080D60 File Offset: 0x0007EF60
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFireBurstVfx.MethodName.Create || (ref method) == NFireBurstVfx.MethodName._Ready || (ref method) == NFireBurstVfx.MethodName._ExitTree || (ref method) == NFireBurstVfx.MethodName.ApplyTint || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001515 RID: 5397 RVA: 0x00080DB0 File Offset: 0x0007EFB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFireBurstVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NFireBurstVfx.PropertyName._modulateParticles)
			{
				this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001516 RID: 5398 RVA: 0x00080DF0 File Offset: 0x0007EFF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFireBurstVfx.PropertyName._particles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
				return true;
			}
			if ((ref name) == NFireBurstVfx.PropertyName._modulateParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001517 RID: 5399 RVA: 0x00080E48 File Offset: 0x0007F048
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NFireBurstVfx.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NFireBurstVfx.PropertyName._modulateParticles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001518 RID: 5400 RVA: 0x00080EA0 File Offset: 0x0007F0A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFireBurstVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
			info.AddProperty(NFireBurstVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
		}

		/// <inheritdoc />
		// Token: 0x06001519 RID: 5401 RVA: 0x00080ED8 File Offset: 0x0007F0D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFireBurstVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFireBurstVfx.PropertyName._modulateParticles, ref variant2))
			{
				this._modulateParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x04000787 RID: 1927
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_fire_burst");

		// Token: 0x04000788 RID: 1928
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();

		// Token: 0x04000789 RID: 1929
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();

		// Token: 0x0400078A RID: 1930
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001028 RID: 4136
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004000 RID: 16384
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004001 RID: 16385
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004002 RID: 16386
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ApplyTint' method.
			/// </summary>
			// Token: 0x04004003 RID: 16387
			public static readonly StringName ApplyTint = "ApplyTint";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001029 RID: 4137
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x04004004 RID: 16388
			public static readonly StringName _particles = "_particles";

			/// <summary>
			/// Cached name for the '_modulateParticles' field.
			/// </summary>
			// Token: 0x04004005 RID: 16389
			public static readonly StringName _modulateParticles = "_modulateParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200102A RID: 4138
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
