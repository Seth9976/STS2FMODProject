using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001A7 RID: 423
	[ScriptPath("res://src/Core/Nodes/Vfx/NFgGroundSpikeVfx.cs")]
	public class NFgGroundSpikeVfx : NBgGroundSpikeVfx
	{
		// Token: 0x060014EE RID: 5358 RVA: 0x0007FE90 File Offset: 0x0007E090
		[NullableContext(2)]
		public new static NFgGroundSpikeVfx Create(Vector2 position, bool movingRight = true, VfxColor vfxColor = VfxColor.Red)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NFgGroundSpikeVfx nfgGroundSpikeVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/fg_ground_spike_vfx.tscn").Instantiate<NFgGroundSpikeVfx>(0L);
			nfgGroundSpikeVfx._startPosition = position;
			nfgGroundSpikeVfx._movingRight = movingRight;
			nfgGroundSpikeVfx._vfxColor = vfxColor;
			return nfgGroundSpikeVfx;
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0007FED4 File Offset: 0x0007E0D4
		protected override void AdjustStartPosition()
		{
			this._startPosition += new Vector2(this._movingRight ? Rng.Chaotic.NextFloat(40f, 160f) : Rng.Chaotic.NextFloat(-160f, -40f), Rng.Chaotic.NextFloat(10f, 32f));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014F0 RID: 5360 RVA: 0x0007FF40 File Offset: 0x0007E140
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NFgGroundSpikeVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Sprite2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false),
					new PropertyInfo(1L, "movingRight", 0L, "", 6L, false),
					new PropertyInfo(2L, "vfxColor", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NFgGroundSpikeVfx.MethodName.AdjustStartPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014F1 RID: 5361 RVA: 0x00080028 File Offset: 0x0007E228
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFgGroundSpikeVfx.MethodName.Create && args.Count == 3)
			{
				NFgGroundSpikeVfx nfgGroundSpikeVfx = NFgGroundSpikeVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<VfxColor>(args[2]));
				ret = VariantUtils.CreateFrom<NFgGroundSpikeVfx>(ref nfgGroundSpikeVfx);
				return true;
			}
			if ((ref method) == NFgGroundSpikeVfx.MethodName.AdjustStartPosition && args.Count == 0)
			{
				this.AdjustStartPosition();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x000800B8 File Offset: 0x0007E2B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFgGroundSpikeVfx.MethodName.Create && args.Count == 3)
			{
				NFgGroundSpikeVfx nfgGroundSpikeVfx = NFgGroundSpikeVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<VfxColor>(args[2]));
				ret = VariantUtils.CreateFrom<NFgGroundSpikeVfx>(ref nfgGroundSpikeVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060014F3 RID: 5363 RVA: 0x00080120 File Offset: 0x0007E320
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFgGroundSpikeVfx.MethodName.Create || (ref method) == NFgGroundSpikeVfx.MethodName.AdjustStartPosition || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060014F4 RID: 5364 RVA: 0x00080147 File Offset: 0x0007E347
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060014F5 RID: 5365 RVA: 0x00080150 File Offset: 0x0007E350
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000782 RID: 1922
		[Nullable(1)]
		private const string _scenePath = "res://scenes/vfx/fg_ground_spike_vfx.tscn";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001021 RID: 4129
		public new class MethodName : NBgGroundSpikeVfx.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003FF5 RID: 16373
			public new static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'AdjustStartPosition' method.
			/// </summary>
			// Token: 0x04003FF6 RID: 16374
			public new static readonly StringName AdjustStartPosition = "AdjustStartPosition";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001022 RID: 4130
		public new class PropertyName : NBgGroundSpikeVfx.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001023 RID: 4131
		public new class SignalName : NBgGroundSpikeVfx.SignalName
		{
		}
	}
}
