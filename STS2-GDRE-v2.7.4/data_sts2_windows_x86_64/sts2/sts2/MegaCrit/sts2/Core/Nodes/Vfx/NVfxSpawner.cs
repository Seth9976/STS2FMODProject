using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x0200020B RID: 523
	[ScriptPath("res://src/Core/Nodes/Vfx/NVfxSpawner.cs")]
	public class NVfxSpawner : Control
	{
		// Token: 0x06001B15 RID: 6933 RVA: 0x000AEC3C File Offset: 0x000ACE3C
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && inputEventMouseButton.ButtonIndex == 1L && !inputEventMouseButton.Pressed)
			{
				Log.Info("Spawning Vfx!", 2);
				NFailedJoinVfx nfailedJoinVfx = NFailedJoinVfx.Create("<Username> couldn't join. (Reason goes here)");
				if (nfailedJoinVfx != null)
				{
					this.AddChildSafely(nfailedJoinVfx);
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B16 RID: 6934 RVA: 0x000AEC88 File Offset: 0x000ACE88
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NVfxSpawner.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B17 RID: 6935 RVA: 0x000AECFC File Offset: 0x000ACEFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NVfxSpawner.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001B18 RID: 6936 RVA: 0x000AED3A File Offset: 0x000ACF3A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NVfxSpawner.MethodName._Input || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001B19 RID: 6937 RVA: 0x000AED52 File Offset: 0x000ACF52
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06001B1A RID: 6938 RVA: 0x000AED5B File Offset: 0x000ACF5B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001198 RID: 4504
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04004492 RID: 17554
			public static readonly StringName _Input = "_Input";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001199 RID: 4505
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200119A RID: 4506
		public class SignalName : Control.SignalName
		{
		}
	}
}
