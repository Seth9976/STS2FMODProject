using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	// Token: 0x0200025E RID: 606
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NEpochPaginateButton.cs")]
	public class NEpochPaginateButton : NGoldArrowButton
	{
		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x000D3F13 File Offset: 0x000D2113
		[Nullable(1)]
		protected override string ClickedSfx
		{
			[NullableContext(1)]
			get
			{
				return "event:/sfx/ui/timeline/ui_timeline_click";
			}
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x000D3F1A File Offset: 0x000D211A
		protected override void OnDisable()
		{
			base.Visible = false;
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x000D3F23 File Offset: 0x000D2123
		protected override void OnEnable()
		{
			base.Visible = true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001FA0 RID: 8096 RVA: 0x000D3F2C File Offset: 0x000D212C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NEpochPaginateButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochPaginateButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001FA1 RID: 8097 RVA: 0x000D3FA0 File Offset: 0x000D21A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochPaginateButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochPaginateButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001FA2 RID: 8098 RVA: 0x000D4000 File Offset: 0x000D2200
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEpochPaginateButton.MethodName.OnDisable || (ref method) == NEpochPaginateButton.MethodName.OnEnable || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001FA3 RID: 8099 RVA: 0x000D4028 File Offset: 0x000D2228
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEpochPaginateButton.PropertyName.ClickedSfx)
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
		// Token: 0x06001FA4 RID: 8100 RVA: 0x000D4060 File Offset: 0x000D2260
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NEpochPaginateButton.PropertyName.ClickedSfx, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001FA5 RID: 8101 RVA: 0x000D4094 File Offset: 0x000D2294
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06001FA6 RID: 8102 RVA: 0x000D409D File Offset: 0x000D229D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012A7 RID: 4775
		public new class MethodName : NGoldArrowButton.MethodName
		{
			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04004892 RID: 18578
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04004893 RID: 18579
			public new static readonly StringName OnEnable = "OnEnable";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012A8 RID: 4776
		public new class PropertyName : NGoldArrowButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ClickedSfx' property.
			/// </summary>
			// Token: 0x04004894 RID: 18580
			public new static readonly StringName ClickedSfx = "ClickedSfx";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012A9 RID: 4777
		public new class SignalName : NGoldArrowButton.SignalName
		{
		}
	}
}
