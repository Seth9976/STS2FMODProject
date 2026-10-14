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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	// Token: 0x0200023C RID: 572
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NSpikeSplashVfx.cs")]
	public class NSpikeSplashVfx : Node2D
	{
		// Token: 0x06001D1B RID: 7451 RVA: 0x000BD5E1 File Offset: 0x000BB7E1
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x000BD5F4 File Offset: 0x000BB7F4
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NSpikeSplashVfx Create(Creature target, VfxColor vfxColor = VfxColor.Red)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSpikeSplashVfx nspikeSplashVfx = PreloadManager.Cache.GetScene(NSpikeSplashVfx._scenePath).Instantiate<NSpikeSplashVfx>(0L);
			nspikeSplashVfx._spawnPosition = NCombatRoom.Instance.GetCreatureNode(target).GetBottomOfHitbox();
			nspikeSplashVfx._vfxColor = vfxColor;
			return nspikeSplashVfx;
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x000BD640 File Offset: 0x000BB840
		public override void _Ready()
		{
			for (int i = 0; i < this._spikeAmount; i++)
			{
				NFgGroundSpikeVfx nfgGroundSpikeVfx = NFgGroundSpikeVfx.Create(this._spawnPosition, true, this._vfxColor);
				NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(nfgGroundSpikeVfx);
				nfgGroundSpikeVfx = NFgGroundSpikeVfx.Create(this._spawnPosition, false, this._vfxColor);
				NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(nfgGroundSpikeVfx);
			}
			for (int j = 0; j < this._spikeAmount; j++)
			{
				NBgGroundSpikeVfx nbgGroundSpikeVfx = NBgGroundSpikeVfx.Create(this._spawnPosition, true, this._vfxColor);
				NCombatRoom.Instance.BackCombatVfxContainer.AddChildSafely(nbgGroundSpikeVfx);
				nbgGroundSpikeVfx = NBgGroundSpikeVfx.Create(this._spawnPosition, false, this._vfxColor);
				NCombatRoom.Instance.BackCombatVfxContainer.AddChildSafely(nbgGroundSpikeVfx);
			}
			TaskHelper.RunSafely(this.SelfDestruct());
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000BD708 File Offset: 0x000BB908
		[NullableContext(1)]
		private async Task SelfDestruct()
		{
			this._cts = new CancellationTokenSource();
			await Task.Delay(2000, this._cts.Token);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D1F RID: 7455 RVA: 0x000BD74C File Offset: 0x000BB94C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NSpikeSplashVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSpikeSplashVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D20 RID: 7456 RVA: 0x000BD7C0 File Offset: 0x000BB9C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSpikeSplashVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpikeSplashVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001D21 RID: 7457 RVA: 0x000BD820 File Offset: 0x000BBA20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSpikeSplashVfx.MethodName._ExitTree || (ref method) == NSpikeSplashVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001D22 RID: 7458 RVA: 0x000BD848 File Offset: 0x000BBA48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSpikeSplashVfx.PropertyName._duration)
			{
				this._duration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSpikeSplashVfx.PropertyName._spikeAmount)
			{
				this._spikeAmount = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NSpikeSplashVfx.PropertyName._spawnPosition)
			{
				this._spawnPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NSpikeSplashVfx.PropertyName._vfxColor)
			{
				this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001D23 RID: 7459 RVA: 0x000BD8CC File Offset: 0x000BBACC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSpikeSplashVfx.PropertyName._duration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._duration);
				return true;
			}
			if ((ref name) == NSpikeSplashVfx.PropertyName._spikeAmount)
			{
				value = VariantUtils.CreateFrom<int>(ref this._spikeAmount);
				return true;
			}
			if ((ref name) == NSpikeSplashVfx.PropertyName._spawnPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._spawnPosition);
				return true;
			}
			if ((ref name) == NSpikeSplashVfx.PropertyName._vfxColor)
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
		// Token: 0x06001D24 RID: 7460 RVA: 0x000BD964 File Offset: 0x000BBB64
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NSpikeSplashVfx.PropertyName._duration, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSpikeSplashVfx.PropertyName._spikeAmount, 0L, "", 4096L, false),
				new PropertyInfo(5L, NSpikeSplashVfx.PropertyName._spawnPosition, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSpikeSplashVfx.PropertyName._vfxColor, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D25 RID: 7461 RVA: 0x000BD9F8 File Offset: 0x000BBBF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSpikeSplashVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
			info.AddProperty(NSpikeSplashVfx.PropertyName._spikeAmount, Variant.From<int>(ref this._spikeAmount));
			info.AddProperty(NSpikeSplashVfx.PropertyName._spawnPosition, Variant.From<Vector2>(ref this._spawnPosition));
			info.AddProperty(NSpikeSplashVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
		}

		/// <inheritdoc />
		// Token: 0x06001D26 RID: 7462 RVA: 0x000BDA64 File Offset: 0x000BBC64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSpikeSplashVfx.PropertyName._duration, ref variant))
			{
				this._duration = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSpikeSplashVfx.PropertyName._spikeAmount, ref variant2))
			{
				this._spikeAmount = variant2.As<int>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSpikeSplashVfx.PropertyName._spawnPosition, ref variant3))
			{
				this._spawnPosition = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSpikeSplashVfx.PropertyName._vfxColor, ref variant4))
			{
				this._vfxColor = variant4.As<VfxColor>();
			}
		}

		// Token: 0x04000B34 RID: 2868
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/spike_splash_vfx");

		// Token: 0x04000B35 RID: 2869
		private float _duration = 1f;

		// Token: 0x04000B36 RID: 2870
		private int _spikeAmount = 6;

		// Token: 0x04000B37 RID: 2871
		private Vector2 _spawnPosition;

		// Token: 0x04000B38 RID: 2872
		private VfxColor _vfxColor;

		// Token: 0x04000B39 RID: 2873
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001220 RID: 4640
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004634 RID: 17972
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004635 RID: 17973
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001221 RID: 4641
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_duration' field.
			/// </summary>
			// Token: 0x04004636 RID: 17974
			public static readonly StringName _duration = "_duration";

			/// <summary>
			/// Cached name for the '_spikeAmount' field.
			/// </summary>
			// Token: 0x04004637 RID: 17975
			public static readonly StringName _spikeAmount = "_spikeAmount";

			/// <summary>
			/// Cached name for the '_spawnPosition' field.
			/// </summary>
			// Token: 0x04004638 RID: 17976
			public static readonly StringName _spawnPosition = "_spawnPosition";

			/// <summary>
			/// Cached name for the '_vfxColor' field.
			/// </summary>
			// Token: 0x04004639 RID: 17977
			public static readonly StringName _vfxColor = "_vfxColor";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001222 RID: 4642
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
