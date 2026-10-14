using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens
{
	// Token: 0x02000317 RID: 791
	[ScriptPath("res://src/Core/Nodes/Screens/InspectScreens/NUpgradePreviewTickbox.cs")]
	public class NUpgradePreviewTickbox : NTickbox
	{
		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002BF8 RID: 11256 RVA: 0x0013812C File Offset: 0x0013632C
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.accept };
			}
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x00138141 File Offset: 0x00136341
		public override void _Ready()
		{
			this.ConnectSignals();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002BFA RID: 11258 RVA: 0x0013814C File Offset: 0x0013634C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NUpgradePreviewTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002BFB RID: 11259 RVA: 0x00138190 File Offset: 0x00136390
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUpgradePreviewTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002BFC RID: 11260 RVA: 0x001381C0 File Offset: 0x001363C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUpgradePreviewTickbox.MethodName._Ready || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002BFD RID: 11261 RVA: 0x001381D8 File Offset: 0x001363D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUpgradePreviewTickbox.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002BFE RID: 11262 RVA: 0x00138210 File Offset: 0x00136410
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NUpgradePreviewTickbox.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002BFF RID: 11263 RVA: 0x00138245 File Offset: 0x00136445
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002C00 RID: 11264 RVA: 0x0013824E File Offset: 0x0013644E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001540 RID: 5440
		public new class MethodName : NTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005151 RID: 20817
			public new static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001541 RID: 5441
		public new class PropertyName : NTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005152 RID: 20818
			public new static readonly StringName Hotkeys = "Hotkeys";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001542 RID: 5442
		public new class SignalName : NTickbox.SignalName
		{
		}
	}
}
