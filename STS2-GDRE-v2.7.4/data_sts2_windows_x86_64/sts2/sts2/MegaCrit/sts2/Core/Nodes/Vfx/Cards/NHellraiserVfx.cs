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
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	/// <summary>
	/// Vfx for Hellraiser card.
	/// Spawns many swords and tints the screen red.
	/// </summary>
	// Token: 0x0200023A RID: 570
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NHellraiserVfx.cs")]
	public class NHellraiserVfx : Control
	{
		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001CFF RID: 7423 RVA: 0x000BCD88 File Offset: 0x000BAF88
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NHellraiserVfx._scenePath);
			}
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000BCD94 File Offset: 0x000BAF94
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000BCDA8 File Offset: 0x000BAFA8
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NHellraiserVfx Create(Creature target)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NHellraiserVfx nhellraiserVfx = PreloadManager.Cache.GetScene(NHellraiserVfx._scenePath).Instantiate<NHellraiserVfx>(0L);
			nhellraiserVfx._spawnPosition = NCombatRoom.Instance.GetCreatureNode(target).GetBottomOfHitbox() + NHellraiserVfx._vfxOffset;
			return nhellraiserVfx;
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000BCDF8 File Offset: 0x000BAFF8
		public override void _Ready()
		{
			List<float> list = new List<float>();
			for (int i = 0; i < this._swordAmount; i++)
			{
				list.Add(Rng.Chaotic.NextFloat(10f, 50f));
			}
			list.Sort();
			foreach (float num in list)
			{
				NHellraiserSwordVfx nhellraiserSwordVfx = NHellraiserSwordVfx.Create();
				nhellraiserSwordVfx.GlobalPosition = this._spawnPosition;
				nhellraiserSwordVfx.posY = num;
				float num2 = MathHelper.Remap(num, 10f, 50f, 0.8f, 1f);
				nhellraiserSwordVfx.targetColor = new Color(num2, num2, num2, 1f);
				NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(nhellraiserSwordVfx);
			}
			list = new List<float>();
			for (int j = 0; j < this._swordAmount; j++)
			{
				list.Add(Rng.Chaotic.NextFloat(-50f, -10f));
			}
			SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_hellraiser", 1f);
			list.Sort();
			foreach (float num3 in list)
			{
				NHellraiserSwordVfx nhellraiserSwordVfx2 = NHellraiserSwordVfx.Create();
				nhellraiserSwordVfx2.GlobalPosition = this._spawnPosition;
				nhellraiserSwordVfx2.posY = num3;
				float num4 = MathHelper.Remap(num3, -10f, -50f, 0.7f, 0.4f);
				nhellraiserSwordVfx2.targetColor = new Color(num4, num4, num4, 1f);
				NCombatRoom.Instance.BackCombatVfxContainer.AddChildSafely(nhellraiserSwordVfx2);
			}
			NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(NAdditiveOverlayVfx.Create(VfxColor.Red));
			TaskHelper.RunSafely(this.SelfDestruct());
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000BCFDC File Offset: 0x000BB1DC
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
		// Token: 0x06001D04 RID: 7428 RVA: 0x000BD020 File Offset: 0x000BB220
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NHellraiserVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHellraiserVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D05 RID: 7429 RVA: 0x000BD094 File Offset: 0x000BB294
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHellraiserVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHellraiserVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001D06 RID: 7430 RVA: 0x000BD0F4 File Offset: 0x000BB2F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHellraiserVfx.MethodName._ExitTree || (ref method) == NHellraiserVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001D07 RID: 7431 RVA: 0x000BD11C File Offset: 0x000BB31C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHellraiserVfx.PropertyName._duration)
			{
				this._duration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NHellraiserVfx.PropertyName._swordAmount)
			{
				this._swordAmount = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NHellraiserVfx.PropertyName._spawnPosition)
			{
				this._spawnPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001D08 RID: 7432 RVA: 0x000BD184 File Offset: 0x000BB384
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHellraiserVfx.PropertyName._duration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._duration);
				return true;
			}
			if ((ref name) == NHellraiserVfx.PropertyName._swordAmount)
			{
				value = VariantUtils.CreateFrom<int>(ref this._swordAmount);
				return true;
			}
			if ((ref name) == NHellraiserVfx.PropertyName._spawnPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._spawnPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D09 RID: 7433 RVA: 0x000BD1FC File Offset: 0x000BB3FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NHellraiserVfx.PropertyName._duration, 0L, "", 4096L, false),
				new PropertyInfo(2L, NHellraiserVfx.PropertyName._swordAmount, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHellraiserVfx.PropertyName._spawnPosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D0A RID: 7434 RVA: 0x000BD270 File Offset: 0x000BB470
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHellraiserVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
			info.AddProperty(NHellraiserVfx.PropertyName._swordAmount, Variant.From<int>(ref this._swordAmount));
			info.AddProperty(NHellraiserVfx.PropertyName._spawnPosition, Variant.From<Vector2>(ref this._spawnPosition));
		}

		/// <inheritdoc />
		// Token: 0x06001D0B RID: 7435 RVA: 0x000BD2C8 File Offset: 0x000BB4C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHellraiserVfx.PropertyName._duration, ref variant))
			{
				this._duration = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHellraiserVfx.PropertyName._swordAmount, ref variant2))
			{
				this._swordAmount = variant2.As<int>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHellraiserVfx.PropertyName._spawnPosition, ref variant3))
			{
				this._spawnPosition = variant3.As<Vector2>();
			}
		}

		// Token: 0x04000B2B RID: 2859
		private float _duration = 1f;

		// Token: 0x04000B2C RID: 2860
		private int _swordAmount = 10;

		// Token: 0x04000B2D RID: 2861
		private Vector2 _spawnPosition;

		// Token: 0x04000B2E RID: 2862
		private static readonly Vector2 _vfxOffset = new Vector2(-100f, -200f);

		// Token: 0x04000B2F RID: 2863
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/cards/vfx_hellraiser/hellraiser_vfx");

		// Token: 0x04000B30 RID: 2864
		[Nullable(1)]
		private const string _hellraiserSfxPath = "event:/sfx/characters/ironclad/ironclad_hellraiser";

		// Token: 0x04000B31 RID: 2865
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001218 RID: 4632
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004624 RID: 17956
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004625 RID: 17957
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001219 RID: 4633
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_duration' field.
			/// </summary>
			// Token: 0x04004626 RID: 17958
			public static readonly StringName _duration = "_duration";

			/// <summary>
			/// Cached name for the '_swordAmount' field.
			/// </summary>
			// Token: 0x04004627 RID: 17959
			public static readonly StringName _swordAmount = "_swordAmount";

			/// <summary>
			/// Cached name for the '_spawnPosition' field.
			/// </summary>
			// Token: 0x04004628 RID: 17960
			public static readonly StringName _spawnPosition = "_spawnPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200121A RID: 4634
		public class SignalName : Control.SignalName
		{
		}
	}
}
