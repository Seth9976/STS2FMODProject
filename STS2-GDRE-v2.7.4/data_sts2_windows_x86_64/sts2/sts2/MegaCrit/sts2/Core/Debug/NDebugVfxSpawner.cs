using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Debug
{
	/// <summary>
	/// A helper script to spawn vfx in whole_screen_vfx_playground
	/// </summary>
	// Token: 0x02000D0F RID: 3343
	[ScriptPath("res://src/Core/Debug/NDebugVfxSpawner.cs")]
	public class NDebugVfxSpawner : Control
	{
		// Token: 0x060086EC RID: 34540 RVA: 0x002D511C File Offset: 0x002D331C
		private void Spawn()
		{
			NRelicFlashVfx nrelicFlashVfx = NRelicFlashVfx.Create(ModelDb.Relic<PaelsEye>());
			if (nrelicFlashVfx != null)
			{
				this.AddChildSafely(nrelicFlashVfx);
				nrelicFlashVfx.Scale = Vector2.One * 2f;
				nrelicFlashVfx.Position = this.Size * 0.5f;
			}
		}

		// Token: 0x060086ED RID: 34541 RVA: 0x002D516B File Offset: 0x002D336B
		public override void _Process(double delta)
		{
			if (Input.IsActionJustReleased(NDebugVfxSpawner._viewDeck, false))
			{
				this.Spawn();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060086EE RID: 34542 RVA: 0x002D5180 File Offset: 0x002D3380
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NDebugVfxSpawner.MethodName.Spawn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDebugVfxSpawner.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060086EF RID: 34543 RVA: 0x002D5218 File Offset: 0x002D3418
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDebugVfxSpawner.MethodName.Spawn && args.Count == 0)
			{
				this.Spawn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugVfxSpawner.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060086F0 RID: 34544 RVA: 0x002D5286 File Offset: 0x002D3486
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDebugVfxSpawner.MethodName.Spawn || (ref method) == NDebugVfxSpawner.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060086F1 RID: 34545 RVA: 0x002D52AD File Offset: 0x002D34AD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060086F2 RID: 34546 RVA: 0x002D52B6 File Offset: 0x002D34B6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04002EC5 RID: 11973
		[Nullable(1)]
		private static readonly StringName _viewDeck = new StringName("view_deck");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200241A RID: 9242
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Spawn' method.
			/// </summary>
			// Token: 0x04009893 RID: 39059
			public static readonly StringName Spawn = "Spawn";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04009894 RID: 39060
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200241B RID: 9243
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200241C RID: 9244
		public class SignalName : Control.SignalName
		{
		}
	}
}
