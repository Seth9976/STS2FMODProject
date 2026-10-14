using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Cards.Holders
{
	// Token: 0x0200041C RID: 1052
	[ScriptPath("res://src/Core/Nodes/Cards/Holders/NCardHolderHitbox.cs")]
	public class NCardHolderHitbox : NButton
	{
		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x060042FE RID: 17150 RVA: 0x00208D27 File Offset: 0x00206F27
		[Nullable(2)]
		protected override string ClickedSfx
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x00208D2A File Offset: 0x00206F2A
		public override void _Ready()
		{
			this.ConnectSignals();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004300 RID: 17152 RVA: 0x00208D34 File Offset: 0x00206F34
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NCardHolderHitbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004301 RID: 17153 RVA: 0x00208D78 File Offset: 0x00206F78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardHolderHitbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06004302 RID: 17154 RVA: 0x00208DA8 File Offset: 0x00206FA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardHolderHitbox.MethodName._Ready || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06004303 RID: 17155 RVA: 0x00208DC0 File Offset: 0x00206FC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardHolderHitbox.PropertyName.ClickedSfx)
			{
				string clickedSfx = this.ClickedSfx;
				value = VariantUtils.CreateFrom<string>(ref clickedSfx);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004304 RID: 17156 RVA: 0x00208DF8 File Offset: 0x00206FF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NCardHolderHitbox.PropertyName.ClickedSfx, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004305 RID: 17157 RVA: 0x00208E2C File Offset: 0x0020702C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06004306 RID: 17158 RVA: 0x00208E35 File Offset: 0x00207035
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001960 RID: 6496
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006441 RID: 25665
			public new static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001961 RID: 6497
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ClickedSfx' property.
			/// </summary>
			// Token: 0x04006442 RID: 25666
			public new static readonly StringName ClickedSfx = "ClickedSfx";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001962 RID: 6498
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
