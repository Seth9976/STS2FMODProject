using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;

namespace MegaCrit.Sts2.Core.Nodes
{
	// Token: 0x02000176 RID: 374
	[ScriptPath("res://src/Core/Nodes/NAssetLoader.cs")]
	public class NAssetLoader : Node
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x000660A9 File Offset: 0x000642A9
		[Nullable(1)]
		public static NAssetLoader Instance
		{
			[NullableContext(1)]
			get
			{
				return NAssetLoader._instance ?? new NAssetLoader();
			}
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x000660B9 File Offset: 0x000642B9
		public override void _Ready()
		{
			if (NAssetLoader._instance == null)
			{
				NAssetLoader._instance = this;
			}
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x000660C8 File Offset: 0x000642C8
		[NullableContext(1)]
		public Task<bool> LoadInTheBackground(AssetLoadingSession session)
		{
			this._sessions.Enqueue(session);
			base.SetProcess(true);
			return session.Task;
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x000660E4 File Offset: 0x000642E4
		public override void _Process(double delta)
		{
			if (this._currentSession != null && !this._currentSession.IsCompleted)
			{
				this._currentSession.Process();
				return;
			}
			AssetLoadingSession assetLoadingSession;
			if (this._sessions.TryDequeue(out assetLoadingSession))
			{
				this._currentSession = assetLoadingSession;
				return;
			}
			base.SetProcess(false);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600117B RID: 4475 RVA: 0x00066130 File Offset: 0x00064330
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NAssetLoader.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAssetLoader.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600117C RID: 4476 RVA: 0x000661C8 File Offset: 0x000643C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAssetLoader.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAssetLoader.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600117D RID: 4477 RVA: 0x00066236 File Offset: 0x00064436
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAssetLoader.MethodName._Ready || (ref method) == NAssetLoader.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600117E RID: 4478 RVA: 0x0006625D File Offset: 0x0006445D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x0600117F RID: 4479 RVA: 0x00066266 File Offset: 0x00064466
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x0400063A RID: 1594
		[Nullable(2)]
		private static NAssetLoader _instance;

		// Token: 0x0400063B RID: 1595
		[Nullable(new byte[] { 1, 2 })]
		private readonly ConcurrentQueue<AssetLoadingSession> _sessions = new ConcurrentQueue<AssetLoadingSession>();

		// Token: 0x0400063C RID: 1596
		[Nullable(2)]
		private AssetLoadingSession _currentSession;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F58 RID: 3928
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003D05 RID: 15621
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04003D06 RID: 15622
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F59 RID: 3929
		public class PropertyName : Node.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F5A RID: 3930
		public class SignalName : Node.SignalName
		{
		}
	}
}
