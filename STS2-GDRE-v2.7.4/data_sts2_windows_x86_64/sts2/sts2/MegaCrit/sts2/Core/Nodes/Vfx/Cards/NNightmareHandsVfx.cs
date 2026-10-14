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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	// Token: 0x0200023B RID: 571
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NNightmareHandsVfx.cs")]
	public class NNightmareHandsVfx : Control
	{
		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x000BD370 File Offset: 0x000BB570
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NNightmareHandsVfx._scenePath);
			}
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000BD37C File Offset: 0x000BB57C
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000BD38E File Offset: 0x000BB58E
		[NullableContext(2)]
		public static NNightmareHandsVfx Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NNightmareHandsVfx._scenePath).Instantiate<NNightmareHandsVfx>(0L);
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000BD3AF File Offset: 0x000BB5AF
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.SelfDestruct());
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000BD3C0 File Offset: 0x000BB5C0
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
		// Token: 0x06001D13 RID: 7443 RVA: 0x000BD404 File Offset: 0x000BB604
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NNightmareHandsVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNightmareHandsVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NNightmareHandsVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D14 RID: 7444 RVA: 0x000BD4B4 File Offset: 0x000BB6B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NNightmareHandsVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNightmareHandsVfx.MethodName.Create && args.Count == 0)
			{
				NNightmareHandsVfx nnightmareHandsVfx = NNightmareHandsVfx.Create();
				ret = VariantUtils.CreateFrom<NNightmareHandsVfx>(ref nnightmareHandsVfx);
				return true;
			}
			if ((ref method) == NNightmareHandsVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x000BD540 File Offset: 0x000BB740
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NNightmareHandsVfx.MethodName.Create && args.Count == 0)
			{
				NNightmareHandsVfx nnightmareHandsVfx = NNightmareHandsVfx.Create();
				ret = VariantUtils.CreateFrom<NNightmareHandsVfx>(ref nnightmareHandsVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001D16 RID: 7446 RVA: 0x000BD580 File Offset: 0x000BB780
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NNightmareHandsVfx.MethodName._ExitTree || (ref method) == NNightmareHandsVfx.MethodName.Create || (ref method) == NNightmareHandsVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001D17 RID: 7447 RVA: 0x000BD5B6 File Offset: 0x000BB7B6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06001D18 RID: 7448 RVA: 0x000BD5BF File Offset: 0x000BB7BF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000B32 RID: 2866
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/cards/nightmare_hands_vfx");

		// Token: 0x04000B33 RID: 2867
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200121C RID: 4636
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400462D RID: 17965
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400462E RID: 17966
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400462F RID: 17967
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200121D RID: 4637
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200121E RID: 4638
		public class SignalName : Control.SignalName
		{
		}
	}
}
