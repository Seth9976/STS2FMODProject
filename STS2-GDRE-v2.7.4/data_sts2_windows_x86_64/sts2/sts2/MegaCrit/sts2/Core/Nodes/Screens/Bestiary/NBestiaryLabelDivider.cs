using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary
{
	// Token: 0x02000360 RID: 864
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryLabelDivider.cs")]
	public class NBestiaryLabelDivider : NButton
	{
		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06003240 RID: 12864 RVA: 0x001774E4 File Offset: 0x001756E4
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/bestiary/bestiary_label_divider");
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x001774F0 File Offset: 0x001756F0
		// (set) Token: 0x06003242 RID: 12866 RVA: 0x001774F8 File Offset: 0x001756F8
		private LocString LocString { get; set; }

		// Token: 0x06003243 RID: 12867 RVA: 0x00177504 File Offset: 0x00175704
		public static NBestiaryLabelDivider Create(ActModel act)
		{
			NBestiaryLabelDivider nbestiaryLabelDivider = PreloadManager.Cache.GetScene(NBestiaryLabelDivider.ScenePath).Instantiate<NBestiaryLabelDivider>(0L);
			nbestiaryLabelDivider.LocString = act.Title;
			return nbestiaryLabelDivider;
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x00177538 File Offset: 0x00175738
		public static NBestiaryLabelDivider Create(LocString locString)
		{
			NBestiaryLabelDivider nbestiaryLabelDivider = PreloadManager.Cache.GetScene(NBestiaryLabelDivider.ScenePath).Instantiate<NBestiaryLabelDivider>(0L);
			nbestiaryLabelDivider.LocString = locString;
			return nbestiaryLabelDivider;
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x00177564 File Offset: 0x00175764
		public override void _Ready()
		{
			this.ConnectSignals();
			this._nameLabel = base.GetNode<MegaRichTextLabel>("Label");
			this._nameLabel.Text = this.LocString.GetFormattedText();
			this._nameLabel.Modulate = StsColors.blue;
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x001775B3 File Offset: 0x001757B3
		protected override void OnFocus()
		{
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x001775B5 File Offset: 0x001757B5
		protected override void OnUnfocus()
		{
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003248 RID: 12872 RVA: 0x001775B8 File Offset: 0x001757B8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NBestiaryLabelDivider.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryLabelDivider.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryLabelDivider.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003249 RID: 12873 RVA: 0x0017765C File Offset: 0x0017585C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryLabelDivider.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryLabelDivider.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryLabelDivider.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600324A RID: 12874 RVA: 0x001776E1 File Offset: 0x001758E1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBestiaryLabelDivider.MethodName._Ready || (ref method) == NBestiaryLabelDivider.MethodName.OnFocus || (ref method) == NBestiaryLabelDivider.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600324B RID: 12875 RVA: 0x00177717 File Offset: 0x00175917
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBestiaryLabelDivider.PropertyName._nameLabel)
			{
				this._nameLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600324C RID: 12876 RVA: 0x0017773C File Offset: 0x0017593C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBestiaryLabelDivider.PropertyName._nameLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._nameLabel);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600324D RID: 12877 RVA: 0x00177768 File Offset: 0x00175968
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NBestiaryLabelDivider.PropertyName._nameLabel, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600324E RID: 12878 RVA: 0x0017779D File Offset: 0x0017599D
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBestiaryLabelDivider.PropertyName._nameLabel, Variant.From<MegaRichTextLabel>(ref this._nameLabel));
		}

		/// <inheritdoc />
		// Token: 0x0600324F RID: 12879 RVA: 0x001777BC File Offset: 0x001759BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBestiaryLabelDivider.PropertyName._nameLabel, ref variant))
			{
				this._nameLabel = variant.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x040013A2 RID: 5026
		private MegaRichTextLabel _nameLabel;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200165F RID: 5727
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005755 RID: 22357
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005756 RID: 22358
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005757 RID: 22359
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001660 RID: 5728
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_nameLabel' field.
			/// </summary>
			// Token: 0x04005758 RID: 22360
			public static readonly StringName _nameLabel = "_nameLabel";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001661 RID: 5729
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
