using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000207 RID: 519
	[ScriptPath("res://src/Core/Nodes/Vfx/NThinSliceVfx.cs")]
	public class NThinSliceVfx : Node2D
	{
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x000AC728 File Offset: 0x000AA928
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/vfx/thin_slice_vfx.tscn");
			}
		}

		/// <summary>
		/// Thin Slice vfx will intersect the given position. Pass in the creature vfx center position!
		/// </summary>
		/// <param name="target"></param>
		/// <param name="vfxColor"></param>
		/// <returns></returns>
		// Token: 0x06001ACE RID: 6862 RVA: 0x000AC734 File Offset: 0x000AA934
		[NullableContext(2)]
		public static NThinSliceVfx Create(Creature target, VfxColor vfxColor = VfxColor.Cyan)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(target) : null);
			if (ncreature == null)
			{
				return null;
			}
			Vector2 vfxSpawnPosition = ncreature.VfxSpawnPosition;
			NThinSliceVfx nthinSliceVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/thin_slice_vfx.tscn").Instantiate<NThinSliceVfx>(0L);
			nthinSliceVfx._vfxColor = vfxColor;
			Vector2 vector;
			vector..ctor(Rng.Chaotic.NextFloat(-50f, 50f), Rng.Chaotic.NextFloat(-50f, 50f));
			nthinSliceVfx._creatureCenter = vfxSpawnPosition + vector;
			return nthinSliceVfx;
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x000AC7C4 File Offset: 0x000AA9C4
		public override void _Ready()
		{
			this._slash = base.GetNode<GpuParticles2D>("Slash");
			this._slash.GlobalPosition = this.GenerateSpawnPosition();
			this._slash.Rotation = this.GetAngle();
			this._slash.Emitting = true;
			this._sparkle = this._slash.GetNode<GpuParticles2D>("Sparkle");
			this._sparkle.GlobalPosition = this._creatureCenter;
			this._sparkle.Emitting = true;
			this.SetColor();
			TaskHelper.RunSafely(this.SelfDestruct());
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x000AC85F File Offset: 0x000AAA5F
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x000AC874 File Offset: 0x000AAA74
		private void SetColor()
		{
			ParticleProcessMaterial particleProcessMaterial = (ParticleProcessMaterial)this._slash.GetProcessMaterial();
			switch (this._vfxColor)
			{
			case VfxColor.Red:
				particleProcessMaterial.Color = new Color("FF9900");
				return;
			case VfxColor.Green:
			case VfxColor.Blue:
			case VfxColor.Purple:
			case VfxColor.Black:
				return;
			case VfxColor.White:
				particleProcessMaterial.Color = Colors.White;
				return;
			case VfxColor.Cyan:
				particleProcessMaterial.Color = new Color("C4FFE6");
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Spawns our slash particle in a random position around the given centerPoint
		/// </summary>
		// Token: 0x06001AD2 RID: 6866 RVA: 0x000AC8F4 File Offset: 0x000AAAF4
		private Vector2 GenerateSpawnPosition()
		{
			float num = Rng.Chaotic.NextFloat(0f, 6.2831855f);
			float num2 = Rng.Chaotic.NextFloat(400f, 500f);
			return new Vector2(this._creatureCenter.X + num2 * Mathf.Cos(num), this._creatureCenter.Y + num2 * Mathf.Sin(num));
		}

		/// <summary>
		/// Returns the angle from one vector to another in radians.
		/// Written because Vector2.AngleTo() was confusing
		/// </summary>
		/// <returns></returns>
		// Token: 0x06001AD3 RID: 6867 RVA: 0x000AC958 File Offset: 0x000AAB58
		private float GetAngle()
		{
			Vector2 vector = this._creatureCenter - this._slash.GlobalPosition;
			return Mathf.Atan2(vector.Y, vector.X);
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x000AC990 File Offset: 0x000AAB90
		[NullableContext(1)]
		private async Task SelfDestruct()
		{
			CancellationTokenSource cts = this._cts;
			if (cts != null)
			{
				cts.Cancel();
			}
			this._cts = new CancellationTokenSource();
			await Task.Delay(1000, this._cts.Token);
			if (!this._cts.IsCancellationRequested)
			{
				this.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001AD5 RID: 6869 RVA: 0x000AC9D4 File Offset: 0x000AABD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NThinSliceVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NThinSliceVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NThinSliceVfx.MethodName.SetColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NThinSliceVfx.MethodName.GenerateSpawnPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NThinSliceVfx.MethodName.GetAngle, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001AD6 RID: 6870 RVA: 0x000ACAD4 File Offset: 0x000AACD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NThinSliceVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NThinSliceVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NThinSliceVfx.MethodName.SetColor && args.Count == 0)
			{
				this.SetColor();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NThinSliceVfx.MethodName.GenerateSpawnPosition && args.Count == 0)
			{
				Vector2 vector = this.GenerateSpawnPosition();
				ret = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref method) == NThinSliceVfx.MethodName.GetAngle && args.Count == 0)
			{
				float angle = this.GetAngle();
				ret = VariantUtils.CreateFrom<float>(ref angle);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001AD7 RID: 6871 RVA: 0x000ACBB4 File Offset: 0x000AADB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NThinSliceVfx.MethodName._Ready || (ref method) == NThinSliceVfx.MethodName._ExitTree || (ref method) == NThinSliceVfx.MethodName.SetColor || (ref method) == NThinSliceVfx.MethodName.GenerateSpawnPosition || (ref method) == NThinSliceVfx.MethodName.GetAngle || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001AD8 RID: 6872 RVA: 0x000ACC14 File Offset: 0x000AAE14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NThinSliceVfx.PropertyName._slash)
			{
				this._slash = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NThinSliceVfx.PropertyName._sparkle)
			{
				this._sparkle = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NThinSliceVfx.PropertyName._creatureCenter)
			{
				this._creatureCenter = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NThinSliceVfx.PropertyName._vfxColor)
			{
				this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001AD9 RID: 6873 RVA: 0x000ACC98 File Offset: 0x000AAE98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NThinSliceVfx.PropertyName._slash)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._slash);
				return true;
			}
			if ((ref name) == NThinSliceVfx.PropertyName._sparkle)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sparkle);
				return true;
			}
			if ((ref name) == NThinSliceVfx.PropertyName._creatureCenter)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._creatureCenter);
				return true;
			}
			if ((ref name) == NThinSliceVfx.PropertyName._vfxColor)
			{
				value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001ADA RID: 6874 RVA: 0x000ACD30 File Offset: 0x000AAF30
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NThinSliceVfx.PropertyName._slash, 0L, "", 4096L, false),
				new PropertyInfo(24L, NThinSliceVfx.PropertyName._sparkle, 0L, "", 4096L, false),
				new PropertyInfo(5L, NThinSliceVfx.PropertyName._creatureCenter, 0L, "", 4096L, false),
				new PropertyInfo(2L, NThinSliceVfx.PropertyName._vfxColor, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001ADB RID: 6875 RVA: 0x000ACDC8 File Offset: 0x000AAFC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NThinSliceVfx.PropertyName._slash, Variant.From<GpuParticles2D>(ref this._slash));
			info.AddProperty(NThinSliceVfx.PropertyName._sparkle, Variant.From<GpuParticles2D>(ref this._sparkle));
			info.AddProperty(NThinSliceVfx.PropertyName._creatureCenter, Variant.From<Vector2>(ref this._creatureCenter));
			info.AddProperty(NThinSliceVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
		}

		/// <inheritdoc />
		// Token: 0x06001ADC RID: 6876 RVA: 0x000ACE34 File Offset: 0x000AB034
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NThinSliceVfx.PropertyName._slash, ref variant))
			{
				this._slash = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NThinSliceVfx.PropertyName._sparkle, ref variant2))
			{
				this._sparkle = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NThinSliceVfx.PropertyName._creatureCenter, ref variant3))
			{
				this._creatureCenter = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NThinSliceVfx.PropertyName._vfxColor, ref variant4))
			{
				this._vfxColor = variant4.As<VfxColor>();
			}
		}

		// Token: 0x040009F1 RID: 2545
		[Nullable(1)]
		private const string _scenePath = "res://scenes/vfx/thin_slice_vfx.tscn";

		// Token: 0x040009F2 RID: 2546
		[Nullable(2)]
		private CancellationTokenSource _cts;

		// Token: 0x040009F3 RID: 2547
		[Nullable(1)]
		private GpuParticles2D _slash;

		// Token: 0x040009F4 RID: 2548
		[Nullable(1)]
		private GpuParticles2D _sparkle;

		// Token: 0x040009F5 RID: 2549
		private Vector2 _creatureCenter;

		// Token: 0x040009F6 RID: 2550
		private VfxColor _vfxColor;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001187 RID: 4487
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004454 RID: 17492
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004455 RID: 17493
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SetColor' method.
			/// </summary>
			// Token: 0x04004456 RID: 17494
			public static readonly StringName SetColor = "SetColor";

			/// <summary>
			/// Cached name for the 'GenerateSpawnPosition' method.
			/// </summary>
			// Token: 0x04004457 RID: 17495
			public static readonly StringName GenerateSpawnPosition = "GenerateSpawnPosition";

			/// <summary>
			/// Cached name for the 'GetAngle' method.
			/// </summary>
			// Token: 0x04004458 RID: 17496
			public static readonly StringName GetAngle = "GetAngle";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001188 RID: 4488
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_slash' field.
			/// </summary>
			// Token: 0x04004459 RID: 17497
			public static readonly StringName _slash = "_slash";

			/// <summary>
			/// Cached name for the '_sparkle' field.
			/// </summary>
			// Token: 0x0400445A RID: 17498
			public static readonly StringName _sparkle = "_sparkle";

			/// <summary>
			/// Cached name for the '_creatureCenter' field.
			/// </summary>
			// Token: 0x0400445B RID: 17499
			public static readonly StringName _creatureCenter = "_creatureCenter";

			/// <summary>
			/// Cached name for the '_vfxColor' field.
			/// </summary>
			// Token: 0x0400445C RID: 17500
			public static readonly StringName _vfxColor = "_vfxColor";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001189 RID: 4489
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
