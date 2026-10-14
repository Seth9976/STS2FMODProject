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
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000211 RID: 529
	[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NVfxProjectileHandler.cs")]
	public class NVfxProjectileHandler : Node2D
	{
		// Token: 0x06001B58 RID: 7000 RVA: 0x000B1A6C File Offset: 0x000AFC6C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NVfxProjectileHandler Create(string handlerScenePath, string projectileScenePath, Vector2 sourceGlobalPosition, Vector2 destinationGlobalPosition, Callable endAction)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NVfxProjectileHandler nvfxProjectileHandler = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath(handlerScenePath)).Instantiate<NVfxProjectileHandler>(0L);
			nvfxProjectileHandler._sourceGlobalPosition = sourceGlobalPosition;
			nvfxProjectileHandler._destinationGlobalPosition = destinationGlobalPosition;
			nvfxProjectileHandler._projectileScenePath = projectileScenePath;
			nvfxProjectileHandler._endAction = endAction;
			return nvfxProjectileHandler;
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000B1AB8 File Offset: 0x000AFCB8
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x000B1AC6 File Offset: 0x000AFCC6
		public override void _ExitTree()
		{
			if (this._loadedProjectile != null)
			{
				this._loadedProjectile.QueueFreeSafely();
			}
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x000B1ADC File Offset: 0x000AFCDC
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._loadedProjectile = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath(this._projectileScenePath)).Instantiate<NVfxProjectile>(0L);
			if (this._loadedProjectile != null)
			{
				this.AddChildSafely(this._loadedProjectile);
				this._loadedProjectile.GlobalPosition = this._sourceGlobalPosition;
				this._loadedProjectile.SetEmitting(true);
				Vector2 vector = (this._destinationGlobalPosition - this._sourceGlobalPosition).Normalized();
				Vector2 normal = new Vector2(vector.Y, -vector.X);
				if ((this._sourceGlobalPosition + normal).Y < this._sourceGlobalPosition.Y)
				{
					normal *= -1f;
				}
				float num = 0f;
				float projectileDuration = (float)GD.RandRange((double)this._travelTimeRange.X, (double)this._travelTimeRange.Y);
				float projectileHeightOffset = (float)GD.RandRange((double)this._heightOffsetRange.X, (double)this._heightOffsetRange.Y);
				Curve chosenMovementCurve = this._movementCurves[Mathf.RoundToInt(GD.RandRange(0.0, (double)(this._movementCurves.Length - 1)))];
				Curve chosenHeightCurve = this._pathHeightOffsets[Mathf.RoundToInt(GD.RandRange(0.0, (double)(this._pathHeightOffsets.Length - 1)))];
				while (num < projectileDuration)
				{
					float num2 = num / projectileDuration;
					float num3 = chosenMovementCurve.Sample(num2);
					float num4 = chosenHeightCurve.Sample(num2);
					Vector2 vector2 = this._sourceGlobalPosition.Lerp(this._destinationGlobalPosition, num3);
					Vector2 vector3 = normal * projectileHeightOffset * num4;
					Vector2 vector4 = vector2 + vector3;
					if (this._loadedProjectile.AlignToVelocity)
					{
						Vector2 vector5 = vector4 - this._loadedProjectile.GlobalPosition;
						float num5 = Mathf.Atan2(vector5.Y, vector5.X);
						this._loadedProjectile.GlobalRotation = num5;
					}
					this._loadedProjectile.GlobalPosition = vector4;
					float num6 = num;
					float num7 = await this.AwaitProcessFrame(default(CancellationToken));
					num = num6 + num7;
				}
				this._loadedProjectile.GlobalPosition = this._destinationGlobalPosition;
				this._loadedProjectile.SetEmitting(false);
				this.SpawnImpactVfx(this._destinationGlobalPosition);
				if (this._endAction.Delegate != null)
				{
					this._endAction.Call(Array.Empty<Variant>());
				}
				TaskHelper.RunSafely(this.DelayedFree());
			}
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x000B1B20 File Offset: 0x000AFD20
		private void SpawnImpactVfx(Vector2 spawnPosition)
		{
			if (string.IsNullOrEmpty(this._impactParticlesScenePath))
			{
				return;
			}
			Control control = base.GetParent<Control>();
			if (NCombatRoom.Instance != null)
			{
				control = NCombatRoom.Instance.CombatVfxContainer;
			}
			VfxCmd.PlayVfx(spawnPosition, this._impactParticlesScenePath, control);
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x000B1B64 File Offset: 0x000AFD64
		[NullableContext(1)]
		private async Task DelayedFree()
		{
			await Cmd.Wait(2f, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B5E RID: 7006 RVA: 0x000B1BA8 File Offset: 0x000AFDA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NVfxProjectileHandler.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "handlerScenePath", 0L, "", 6L, false),
					new PropertyInfo(4L, "projectileScenePath", 0L, "", 6L, false),
					new PropertyInfo(5L, "sourceGlobalPosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "destinationGlobalPosition", 0L, "", 6L, false),
					new PropertyInfo(25L, "endAction", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NVfxProjectileHandler.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVfxProjectileHandler.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVfxProjectileHandler.MethodName.SpawnImpactVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "spawnPosition", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B5F RID: 7007 RVA: 0x000B1D54 File Offset: 0x000AFF54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NVfxProjectileHandler.MethodName.Create && args.Count == 5)
			{
				NVfxProjectileHandler nvfxProjectileHandler = NVfxProjectileHandler.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]), VariantUtils.ConvertTo<Vector2>(args[2]), VariantUtils.ConvertTo<Vector2>(args[3]), VariantUtils.ConvertTo<Callable>(args[4]));
				ret = VariantUtils.CreateFrom<NVfxProjectileHandler>(ref nvfxProjectileHandler);
				return true;
			}
			if ((ref method) == NVfxProjectileHandler.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVfxProjectileHandler.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVfxProjectileHandler.MethodName.SpawnImpactVfx && args.Count == 1)
			{
				this.SpawnImpactVfx(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x000B1E54 File Offset: 0x000B0054
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NVfxProjectileHandler.MethodName.Create && args.Count == 5)
			{
				NVfxProjectileHandler nvfxProjectileHandler = NVfxProjectileHandler.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]), VariantUtils.ConvertTo<Vector2>(args[2]), VariantUtils.ConvertTo<Vector2>(args[3]), VariantUtils.ConvertTo<Callable>(args[4]));
				ret = VariantUtils.CreateFrom<NVfxProjectileHandler>(ref nvfxProjectileHandler);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001B61 RID: 7009 RVA: 0x000B1ED8 File Offset: 0x000B00D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NVfxProjectileHandler.MethodName.Create || (ref method) == NVfxProjectileHandler.MethodName._Ready || (ref method) == NVfxProjectileHandler.MethodName._ExitTree || (ref method) == NVfxProjectileHandler.MethodName.SpawnImpactVfx || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001B62 RID: 7010 RVA: 0x000B1F28 File Offset: 0x000B0128
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NVfxProjectileHandler.PropertyName._pathHeightOffsets)
			{
				this._pathHeightOffsets = VariantUtils.ConvertToSystemArrayOfGodotObject<Curve>(ref value);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._heightOffsetRange)
			{
				this._heightOffsetRange = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._movementCurves)
			{
				this._movementCurves = VariantUtils.ConvertToSystemArrayOfGodotObject<Curve>(ref value);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._travelTimeRange)
			{
				this._travelTimeRange = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._impactParticlesScenePath)
			{
				this._impactParticlesScenePath = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._sourceGlobalPosition)
			{
				this._sourceGlobalPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._destinationGlobalPosition)
			{
				this._destinationGlobalPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._projectileScenePath)
			{
				this._projectileScenePath = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._endAction)
			{
				this._endAction = VariantUtils.ConvertTo<Callable>(ref value);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._loadedProjectile)
			{
				this._loadedProjectile = VariantUtils.ConvertTo<NVfxProjectile>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001B63 RID: 7011 RVA: 0x000B204C File Offset: 0x000B024C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NVfxProjectileHandler.PropertyName._pathHeightOffsets)
			{
				GodotObject[] array = this._pathHeightOffsets;
				value = VariantUtils.CreateFromSystemArrayOfGodotObject(array);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._heightOffsetRange)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._heightOffsetRange);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._movementCurves)
			{
				GodotObject[] array = this._movementCurves;
				value = VariantUtils.CreateFromSystemArrayOfGodotObject(array);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._travelTimeRange)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._travelTimeRange);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._impactParticlesScenePath)
			{
				value = VariantUtils.CreateFrom<string>(ref this._impactParticlesScenePath);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._sourceGlobalPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._sourceGlobalPosition);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._destinationGlobalPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._destinationGlobalPosition);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._projectileScenePath)
			{
				value = VariantUtils.CreateFrom<string>(ref this._projectileScenePath);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._endAction)
			{
				value = VariantUtils.CreateFrom<Callable>(ref this._endAction);
				return true;
			}
			if ((ref name) == NVfxProjectileHandler.PropertyName._loadedProjectile)
			{
				value = VariantUtils.CreateFrom<NVfxProjectile>(ref this._loadedProjectile);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B64 RID: 7012 RVA: 0x000B21A8 File Offset: 0x000B03A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NVfxProjectileHandler.PropertyName._pathHeightOffsets, 23L, "24/17:Curve", 4102L, true),
				new PropertyInfo(5L, NVfxProjectileHandler.PropertyName._heightOffsetRange, 0L, "", 4102L, true),
				new PropertyInfo(28L, NVfxProjectileHandler.PropertyName._movementCurves, 23L, "24/17:Curve", 4102L, true),
				new PropertyInfo(5L, NVfxProjectileHandler.PropertyName._travelTimeRange, 0L, "", 4102L, true),
				new PropertyInfo(4L, NVfxProjectileHandler.PropertyName._impactParticlesScenePath, 0L, "", 4102L, true),
				new PropertyInfo(5L, NVfxProjectileHandler.PropertyName._sourceGlobalPosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NVfxProjectileHandler.PropertyName._destinationGlobalPosition, 0L, "", 4096L, false),
				new PropertyInfo(4L, NVfxProjectileHandler.PropertyName._projectileScenePath, 0L, "", 4096L, false),
				new PropertyInfo(25L, NVfxProjectileHandler.PropertyName._endAction, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVfxProjectileHandler.PropertyName._loadedProjectile, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B65 RID: 7013 RVA: 0x000B2304 File Offset: 0x000B0504
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName pathHeightOffsets = NVfxProjectileHandler.PropertyName._pathHeightOffsets;
			GodotObject[] array = this._pathHeightOffsets;
			info.AddProperty(pathHeightOffsets, Variant.CreateFrom(array));
			info.AddProperty(NVfxProjectileHandler.PropertyName._heightOffsetRange, Variant.From<Vector2>(ref this._heightOffsetRange));
			StringName movementCurves = NVfxProjectileHandler.PropertyName._movementCurves;
			array = this._movementCurves;
			info.AddProperty(movementCurves, Variant.CreateFrom(array));
			info.AddProperty(NVfxProjectileHandler.PropertyName._travelTimeRange, Variant.From<Vector2>(ref this._travelTimeRange));
			info.AddProperty(NVfxProjectileHandler.PropertyName._impactParticlesScenePath, Variant.From<string>(ref this._impactParticlesScenePath));
			info.AddProperty(NVfxProjectileHandler.PropertyName._sourceGlobalPosition, Variant.From<Vector2>(ref this._sourceGlobalPosition));
			info.AddProperty(NVfxProjectileHandler.PropertyName._destinationGlobalPosition, Variant.From<Vector2>(ref this._destinationGlobalPosition));
			info.AddProperty(NVfxProjectileHandler.PropertyName._projectileScenePath, Variant.From<string>(ref this._projectileScenePath));
			info.AddProperty(NVfxProjectileHandler.PropertyName._endAction, Variant.From<Callable>(ref this._endAction));
			info.AddProperty(NVfxProjectileHandler.PropertyName._loadedProjectile, Variant.From<NVfxProjectile>(ref this._loadedProjectile));
		}

		/// <inheritdoc />
		// Token: 0x06001B66 RID: 7014 RVA: 0x000B23F8 File Offset: 0x000B05F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._pathHeightOffsets, ref variant))
			{
				this._pathHeightOffsets = variant.AsGodotObjectArray<Curve>();
			}
			Variant variant2;
			if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._heightOffsetRange, ref variant2))
			{
				this._heightOffsetRange = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._movementCurves, ref variant3))
			{
				this._movementCurves = variant3.AsGodotObjectArray<Curve>();
			}
			Variant variant4;
			if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._travelTimeRange, ref variant4))
			{
				this._travelTimeRange = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._impactParticlesScenePath, ref variant5))
			{
				this._impactParticlesScenePath = variant5.As<string>();
			}
			Variant variant6;
			if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._sourceGlobalPosition, ref variant6))
			{
				this._sourceGlobalPosition = variant6.As<Vector2>();
			}
			Variant variant7;
			if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._destinationGlobalPosition, ref variant7))
			{
				this._destinationGlobalPosition = variant7.As<Vector2>();
			}
			Variant variant8;
			if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._projectileScenePath, ref variant8))
			{
				this._projectileScenePath = variant8.As<string>();
			}
			Variant variant9;
			if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._endAction, ref variant9))
			{
				this._endAction = variant9.As<Callable>();
			}
			Variant variant10;
			if (info.TryGetProperty(NVfxProjectileHandler.PropertyName._loadedProjectile, ref variant10))
			{
				this._loadedProjectile = variant10.As<NVfxProjectile>();
			}
		}

		// Token: 0x04000A35 RID: 2613
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve[] _pathHeightOffsets = Array.Empty<Curve>();

		// Token: 0x04000A36 RID: 2614
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Vector2 _heightOffsetRange;

		// Token: 0x04000A37 RID: 2615
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve[] _movementCurves = Array.Empty<Curve>();

		// Token: 0x04000A38 RID: 2616
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Vector2 _travelTimeRange;

		// Token: 0x04000A39 RID: 2617
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private string _impactParticlesScenePath = "";

		// Token: 0x04000A3A RID: 2618
		private Vector2 _sourceGlobalPosition;

		// Token: 0x04000A3B RID: 2619
		private Vector2 _destinationGlobalPosition;

		// Token: 0x04000A3C RID: 2620
		[Nullable(1)]
		private string _projectileScenePath;

		// Token: 0x04000A3D RID: 2621
		private Callable _endAction;

		// Token: 0x04000A3E RID: 2622
		[Nullable(2)]
		private NVfxProjectile _loadedProjectile;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011A7 RID: 4519
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040044CF RID: 17615
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040044D0 RID: 17616
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040044D1 RID: 17617
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SpawnImpactVfx' method.
			/// </summary>
			// Token: 0x040044D2 RID: 17618
			public static readonly StringName SpawnImpactVfx = "SpawnImpactVfx";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011A8 RID: 4520
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_pathHeightOffsets' field.
			/// </summary>
			// Token: 0x040044D3 RID: 17619
			public static readonly StringName _pathHeightOffsets = "_pathHeightOffsets";

			/// <summary>
			/// Cached name for the '_heightOffsetRange' field.
			/// </summary>
			// Token: 0x040044D4 RID: 17620
			public static readonly StringName _heightOffsetRange = "_heightOffsetRange";

			/// <summary>
			/// Cached name for the '_movementCurves' field.
			/// </summary>
			// Token: 0x040044D5 RID: 17621
			public static readonly StringName _movementCurves = "_movementCurves";

			/// <summary>
			/// Cached name for the '_travelTimeRange' field.
			/// </summary>
			// Token: 0x040044D6 RID: 17622
			public static readonly StringName _travelTimeRange = "_travelTimeRange";

			/// <summary>
			/// Cached name for the '_impactParticlesScenePath' field.
			/// </summary>
			// Token: 0x040044D7 RID: 17623
			public static readonly StringName _impactParticlesScenePath = "_impactParticlesScenePath";

			/// <summary>
			/// Cached name for the '_sourceGlobalPosition' field.
			/// </summary>
			// Token: 0x040044D8 RID: 17624
			public static readonly StringName _sourceGlobalPosition = "_sourceGlobalPosition";

			/// <summary>
			/// Cached name for the '_destinationGlobalPosition' field.
			/// </summary>
			// Token: 0x040044D9 RID: 17625
			public static readonly StringName _destinationGlobalPosition = "_destinationGlobalPosition";

			/// <summary>
			/// Cached name for the '_projectileScenePath' field.
			/// </summary>
			// Token: 0x040044DA RID: 17626
			public static readonly StringName _projectileScenePath = "_projectileScenePath";

			/// <summary>
			/// Cached name for the '_endAction' field.
			/// </summary>
			// Token: 0x040044DB RID: 17627
			public static readonly StringName _endAction = "_endAction";

			/// <summary>
			/// Cached name for the '_loadedProjectile' field.
			/// </summary>
			// Token: 0x040044DC RID: 17628
			public static readonly StringName _loadedProjectile = "_loadedProjectile";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011A9 RID: 4521
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
