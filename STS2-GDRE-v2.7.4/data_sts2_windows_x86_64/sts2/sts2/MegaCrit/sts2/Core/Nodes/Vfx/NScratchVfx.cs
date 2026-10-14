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
	// Token: 0x020001EA RID: 490
	[ScriptPath("res://src/Core/Nodes/Vfx/NScratchVfx.cs")]
	public class NScratchVfx : Node2D
	{
		// Token: 0x060018EB RID: 6379 RVA: 0x0009C50C File Offset: 0x0009A70C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NScratchVfx Create(Creature creature, bool goingRight)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NScratchVfx.Create(ncreature.VfxSpawnPosition, goingRight);
			}
			return null;
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x0009C548 File Offset: 0x0009A748
		[NullableContext(2)]
		public static NScratchVfx Create(Vector2 targetCenterPosition, bool goingRight)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NScratchVfx nscratchVfx = PreloadManager.Cache.GetScene(NScratchVfx.scenePath).Instantiate<NScratchVfx>(0L);
			nscratchVfx.GlobalPosition = targetCenterPosition;
			Vector2 one = Vector2.One;
			one.X *= (goingRight ? (-1f) : 1f);
			nscratchVfx.Scale = one;
			return nscratchVfx;
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x0009C5A4 File Offset: 0x0009A7A4
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x0009C5B2 File Offset: 0x0009A7B2
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x0009C5C4 File Offset: 0x0009A7C4
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			for (int i = 0; i < this._anticipationParticles.Count; i++)
			{
				this._anticipationParticles[i].Restart();
			}
			await Cmd.Wait(0.1f, this._cts.Token, false);
			for (int j = 0; j < this._impactParticles.Count; j++)
			{
				this._impactParticles[j].Restart();
			}
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.VeryWeak, ShakeDuration.Short, -1f);
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060018F0 RID: 6384 RVA: 0x0009C608 File Offset: 0x0009A808
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NScratchVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false),
					new PropertyInfo(1L, "goingRight", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScratchVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScratchVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060018F1 RID: 6385 RVA: 0x0009C6FC File Offset: 0x0009A8FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScratchVfx.MethodName.Create && args.Count == 2)
			{
				NScratchVfx nscratchVfx = NScratchVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = VariantUtils.CreateFrom<NScratchVfx>(ref nscratchVfx);
				return true;
			}
			if ((ref method) == NScratchVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScratchVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x0009C7A4 File Offset: 0x0009A9A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScratchVfx.MethodName.Create && args.Count == 2)
			{
				NScratchVfx nscratchVfx = NScratchVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = VariantUtils.CreateFrom<NScratchVfx>(ref nscratchVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060018F3 RID: 6387 RVA: 0x0009C7FF File Offset: 0x0009A9FF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NScratchVfx.MethodName.Create || (ref method) == NScratchVfx.MethodName._Ready || (ref method) == NScratchVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060018F4 RID: 6388 RVA: 0x0009C835 File Offset: 0x0009AA35
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NScratchVfx.PropertyName._anticipationParticles)
			{
				this._anticipationParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NScratchVfx.PropertyName._impactParticles)
			{
				this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060018F5 RID: 6389 RVA: 0x0009C878 File Offset: 0x0009AA78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NScratchVfx.PropertyName._anticipationParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._anticipationParticles);
				return true;
			}
			if ((ref name) == NScratchVfx.PropertyName._impactParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060018F6 RID: 6390 RVA: 0x0009C8D0 File Offset: 0x0009AAD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NScratchVfx.PropertyName._anticipationParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NScratchVfx.PropertyName._impactParticles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x060018F7 RID: 6391 RVA: 0x0009C928 File Offset: 0x0009AB28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NScratchVfx.PropertyName._anticipationParticles, Variant.CreateFrom<GpuParticles2D>(this._anticipationParticles));
			info.AddProperty(NScratchVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
		}

		/// <inheritdoc />
		// Token: 0x060018F8 RID: 6392 RVA: 0x0009C960 File Offset: 0x0009AB60
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NScratchVfx.PropertyName._anticipationParticles, ref variant))
			{
				this._anticipationParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NScratchVfx.PropertyName._impactParticles, ref variant2))
			{
				this._impactParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x04000913 RID: 2323
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_scratch_impact");

		// Token: 0x04000914 RID: 2324
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _anticipationParticles = new Array<GpuParticles2D>();

		// Token: 0x04000915 RID: 2325
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();

		// Token: 0x04000916 RID: 2326
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001118 RID: 4376
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040042C6 RID: 17094
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040042C7 RID: 17095
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040042C8 RID: 17096
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001119 RID: 4377
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_anticipationParticles' field.
			/// </summary>
			// Token: 0x040042C9 RID: 17097
			public static readonly StringName _anticipationParticles = "_anticipationParticles";

			/// <summary>
			/// Cached name for the '_impactParticles' field.
			/// </summary>
			// Token: 0x040042CA RID: 17098
			public static readonly StringName _impactParticles = "_impactParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200111A RID: 4378
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
