using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001AF RID: 431
	[ScriptPath("res://src/Core/Nodes/Vfx/NFullscreenTextVfx.cs")]
	public class NFullscreenTextVfx : Control
	{
		// Token: 0x0600155A RID: 5466 RVA: 0x00082784 File Offset: 0x00080984
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x00082798 File Offset: 0x00080998
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NFullscreenTextVfx Create(string text)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			Log.Info(text, 2);
			NFullscreenTextVfx nfullscreenTextVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/vfx_fullscreen_text.tscn").Instantiate<NFullscreenTextVfx>(0L);
			nfullscreenTextVfx.GetNode<MegaLabel>("Label").SetTextAutoSize(text);
			TaskHelper.RunSafely(nfullscreenTextVfx.SelfDestruct());
			return nfullscreenTextVfx;
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x000827F0 File Offset: 0x000809F0
		[NullableContext(1)]
		private async Task SelfDestruct()
		{
			this._cts = new CancellationTokenSource();
			await Task.Delay(500, this._cts.Token);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600155D RID: 5469 RVA: 0x00082834 File Offset: 0x00080A34
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NFullscreenTextVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFullscreenTextVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600155E RID: 5470 RVA: 0x000828D8 File Offset: 0x00080AD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFullscreenTextVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFullscreenTextVfx.MethodName.Create && args.Count == 1)
			{
				NFullscreenTextVfx nfullscreenTextVfx = NFullscreenTextVfx.Create(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<NFullscreenTextVfx>(ref nfullscreenTextVfx);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0008294C File Offset: 0x00080B4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFullscreenTextVfx.MethodName.Create && args.Count == 1)
			{
				NFullscreenTextVfx nfullscreenTextVfx = NFullscreenTextVfx.Create(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<NFullscreenTextVfx>(ref nfullscreenTextVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001560 RID: 5472 RVA: 0x0008299A File Offset: 0x00080B9A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFullscreenTextVfx.MethodName._ExitTree || (ref method) == NFullscreenTextVfx.MethodName.Create || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001561 RID: 5473 RVA: 0x000829C1 File Offset: 0x00080BC1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06001562 RID: 5474 RVA: 0x000829CA File Offset: 0x00080BCA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000799 RID: 1945
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200103C RID: 4156
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004028 RID: 16424
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004029 RID: 16425
			public static readonly StringName Create = "Create";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200103D RID: 4157
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200103E RID: 4158
		public class SignalName : Control.SignalName
		{
		}
	}
}
