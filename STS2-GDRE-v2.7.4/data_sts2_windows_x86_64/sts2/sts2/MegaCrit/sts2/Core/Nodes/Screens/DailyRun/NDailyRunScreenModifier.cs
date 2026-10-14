using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	// Token: 0x02000332 RID: 818
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunScreenModifier.cs")]
	public class NDailyRunScreenModifier : Control
	{
		// Token: 0x06002DD9 RID: 11737 RVA: 0x00149CBC File Offset: 0x00147EBC
		public override void _Ready()
		{
			this._icon = base.GetNode<TextureRect>("Icon");
			this._description = base.GetNode<MegaRichTextLabel>("Description");
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x00149CEC File Offset: 0x00147EEC
		[NullableContext(1)]
		public void Fill(ModifierModel modifier)
		{
			NDailyRunScreenModifier._modifierLoc.Add("title", modifier.Title.GetFormattedText());
			NDailyRunScreenModifier._modifierLoc.Add("description", modifier.Description.GetFormattedText());
			this._icon.Texture = modifier.Icon;
			this._description.Text = NDailyRunScreenModifier._modifierLoc.GetFormattedText();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002DDB RID: 11739 RVA: 0x00149D54 File Offset: 0x00147F54
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NDailyRunScreenModifier.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002DDC RID: 11740 RVA: 0x00149D98 File Offset: 0x00147F98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunScreenModifier.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002DDD RID: 11741 RVA: 0x00149DC8 File Offset: 0x00147FC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDailyRunScreenModifier.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002DDE RID: 11742 RVA: 0x00149DE0 File Offset: 0x00147FE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDailyRunScreenModifier.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunScreenModifier.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002DDF RID: 11743 RVA: 0x00149E20 File Offset: 0x00148020
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDailyRunScreenModifier.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NDailyRunScreenModifier.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002DE0 RID: 11744 RVA: 0x00149E78 File Offset: 0x00148078
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDailyRunScreenModifier.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunScreenModifier.PropertyName._description, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002DE1 RID: 11745 RVA: 0x00149ECE File Offset: 0x001480CE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDailyRunScreenModifier.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NDailyRunScreenModifier.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
		}

		/// <inheritdoc />
		// Token: 0x06002DE2 RID: 11746 RVA: 0x00149F04 File Offset: 0x00148104
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDailyRunScreenModifier.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDailyRunScreenModifier.PropertyName._description, ref variant2))
			{
				this._description = variant2.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x04001190 RID: 4496
		[Nullable(1)]
		private static readonly LocString _modifierLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.MODIFIER");

		// Token: 0x04001191 RID: 4497
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x04001192 RID: 4498
		[Nullable(1)]
		private MegaRichTextLabel _description;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015AE RID: 5550
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400534E RID: 21326
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015AF RID: 5551
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x0400534F RID: 21327
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005350 RID: 21328
			public static readonly StringName _description = "_description";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015B0 RID: 5552
		public class SignalName : Control.SignalName
		{
		}
	}
}
