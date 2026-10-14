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
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// A nice puff of smoke used by cards and potions.
	/// </summary>
	// Token: 0x020001F3 RID: 499
	[ScriptPath("res://src/Core/Nodes/Vfx/NSmokePuffVfx.cs")]
	public class NSmokePuffVfx : Node2D
	{
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600197F RID: 6527 RVA: 0x000A0C2C File Offset: 0x0009EE2C
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NSmokePuffVfx.ScenePath);
			}
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x000A0C38 File Offset: 0x0009EE38
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001981 RID: 6529 RVA: 0x000A0C4A File Offset: 0x0009EE4A
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_smoke_puff");
			}
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x000A0C58 File Offset: 0x0009EE58
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NSmokePuffVfx Create(Creature target, NSmokePuffVfx.SmokePuffColor puffColor)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
			if (creatureNode == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to spawn ");
				defaultInterpolatedStringHandler.AppendFormatted("NSmokePuffVfx");
				defaultInterpolatedStringHandler.AppendLiteral(" on creature ");
				defaultInterpolatedStringHandler.AppendFormatted<Creature>(target);
				defaultInterpolatedStringHandler.AppendLiteral(" without node!");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return null;
			}
			NSmokePuffVfx nsmokePuffVfx = PreloadManager.Cache.GetScene(NSmokePuffVfx.ScenePath).Instantiate<NSmokePuffVfx>(0L);
			nsmokePuffVfx._color = puffColor;
			nsmokePuffVfx.GlobalPosition = creatureNode.VfxSpawnPosition;
			return nsmokePuffVfx;
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x000A0CFC File Offset: 0x0009EEFC
		public override void _Ready()
		{
			this._ember = base.GetNode<GpuParticles2D>("Ember");
			this._clouds = base.GetNode<GpuParticles2D>("Clouds");
			this._ember.Emitting = true;
			this._clouds.Emitting = true;
			if (this._color == NSmokePuffVfx.SmokePuffColor.Purple)
			{
				ParticleProcessMaterial particleProcessMaterial = (ParticleProcessMaterial)this._clouds.ProcessMaterial;
				particleProcessMaterial.HueVariationMin = -0.02f;
				particleProcessMaterial.HueVariationMax = 0.02f;
				particleProcessMaterial.Color = new Color("F6B1FF");
			}
			TaskHelper.RunSafely(this.DeleteAfterComplete());
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x000A0D9C File Offset: 0x0009EF9C
		[NullableContext(1)]
		private async Task DeleteAfterComplete()
		{
			this._cts = new CancellationTokenSource();
			await Task.Delay(2500, this._cts.Token);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001985 RID: 6533 RVA: 0x000A0DE0 File Offset: 0x0009EFE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NSmokePuffVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSmokePuffVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001986 RID: 6534 RVA: 0x000A0E54 File Offset: 0x0009F054
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSmokePuffVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSmokePuffVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001987 RID: 6535 RVA: 0x000A0EB4 File Offset: 0x0009F0B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSmokePuffVfx.MethodName._ExitTree || (ref method) == NSmokePuffVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001988 RID: 6536 RVA: 0x000A0EDC File Offset: 0x0009F0DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSmokePuffVfx.PropertyName._clouds)
			{
				this._clouds = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSmokePuffVfx.PropertyName._ember)
			{
				this._ember = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSmokePuffVfx.PropertyName._color)
			{
				this._color = VariantUtils.ConvertTo<NSmokePuffVfx.SmokePuffColor>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001989 RID: 6537 RVA: 0x000A0F44 File Offset: 0x0009F144
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSmokePuffVfx.PropertyName._clouds)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._clouds);
				return true;
			}
			if ((ref name) == NSmokePuffVfx.PropertyName._ember)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._ember);
				return true;
			}
			if ((ref name) == NSmokePuffVfx.PropertyName._color)
			{
				value = VariantUtils.CreateFrom<NSmokePuffVfx.SmokePuffColor>(ref this._color);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600198A RID: 6538 RVA: 0x000A0FBC File Offset: 0x0009F1BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSmokePuffVfx.PropertyName._clouds, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSmokePuffVfx.PropertyName._ember, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSmokePuffVfx.PropertyName._color, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600198B RID: 6539 RVA: 0x000A1034 File Offset: 0x0009F234
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSmokePuffVfx.PropertyName._clouds, Variant.From<GpuParticles2D>(ref this._clouds));
			info.AddProperty(NSmokePuffVfx.PropertyName._ember, Variant.From<GpuParticles2D>(ref this._ember));
			info.AddProperty(NSmokePuffVfx.PropertyName._color, Variant.From<NSmokePuffVfx.SmokePuffColor>(ref this._color));
		}

		/// <inheritdoc />
		// Token: 0x0600198C RID: 6540 RVA: 0x000A108C File Offset: 0x0009F28C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSmokePuffVfx.PropertyName._clouds, ref variant))
			{
				this._clouds = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSmokePuffVfx.PropertyName._ember, ref variant2))
			{
				this._ember = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSmokePuffVfx.PropertyName._color, ref variant3))
			{
				this._color = variant3.As<NSmokePuffVfx.SmokePuffColor>();
			}
		}

		// Token: 0x0400094E RID: 2382
		[Nullable(1)]
		private GpuParticles2D _clouds;

		// Token: 0x0400094F RID: 2383
		[Nullable(1)]
		private GpuParticles2D _ember;

		// Token: 0x04000950 RID: 2384
		private NSmokePuffVfx.SmokePuffColor _color;

		// Token: 0x04000951 RID: 2385
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Special vfx-only enum as passing in any color can be problematic.
		/// Color treatments require bespoke tuning (i.e. the color of the associated embers)
		/// </summary>
		// Token: 0x02001139 RID: 4409
		public enum SmokePuffColor
		{
			// Token: 0x04004337 RID: 17207
			Green,
			// Token: 0x04004338 RID: 17208
			Purple
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200113A RID: 4410
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004339 RID: 17209
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400433A RID: 17210
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200113B RID: 4411
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_clouds' field.
			/// </summary>
			// Token: 0x0400433B RID: 17211
			public static readonly StringName _clouds = "_clouds";

			/// <summary>
			/// Cached name for the '_ember' field.
			/// </summary>
			// Token: 0x0400433C RID: 17212
			public static readonly StringName _ember = "_ember";

			/// <summary>
			/// Cached name for the '_color' field.
			/// </summary>
			// Token: 0x0400433D RID: 17213
			public static readonly StringName _color = "_color";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200113C RID: 4412
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
