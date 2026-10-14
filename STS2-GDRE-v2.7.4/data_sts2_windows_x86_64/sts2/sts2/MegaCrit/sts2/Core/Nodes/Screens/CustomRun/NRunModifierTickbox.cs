using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CustomRun
{
	// Token: 0x02000339 RID: 825
	[ScriptPath("res://src/Core/Nodes/Screens/CustomRun/NRunModifierTickbox.cs")]
	public class NRunModifierTickbox : NTickbox
	{
		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002E83 RID: 11907 RVA: 0x001502D5 File Offset: 0x0014E4D5
		// (set) Token: 0x06002E84 RID: 11908 RVA: 0x001502DD File Offset: 0x0014E4DD
		[Nullable(2)]
		public ModifierModel Modifier
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x001502E8 File Offset: 0x0014E4E8
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NRunModifierTickbox Create(ModifierModel model)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NRunModifierTickbox nrunModifierTickbox = PreloadManager.Cache.GetScene("res://scenes/screens/custom_run/modifier_tickbox.tscn").Instantiate<NRunModifierTickbox>(0L);
			nrunModifierTickbox.Modifier = model;
			return nrunModifierTickbox;
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x0015031D File Offset: 0x0014E51D
		protected override void OnEnable()
		{
			base.OnEnable();
			base.Modulate = Colors.White;
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x00150330 File Offset: 0x0014E530
		protected override void OnDisable()
		{
			base.OnDisable();
			base.Modulate = Colors.Gray;
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x00150344 File Offset: 0x0014E544
		public override void _Ready()
		{
			this.ConnectSignals();
			this._label = base.GetNode<MegaRichTextLabel>("%Description");
			this._highlight = base.GetNode<Control>("Highlight");
			if (this.Modifier != null)
			{
				string text = ((ModelDb.GoodModifiers.FirstOrDefault<ModifierModel>((ModifierModel m) => m.GetType() == this.Modifier.GetType()) != null) ? "green" : ((ModelDb.BadModifiers.FirstOrDefault<ModifierModel>((ModifierModel m) => m.GetType() == this.Modifier.GetType()) != null) ? "red" : "blue"));
				NRunModifierTickbox._descriptionLoc.Add("color", text);
				NRunModifierTickbox._descriptionLoc.Add("modifier_title", this.Modifier.Title.GetFormattedText());
				NRunModifierTickbox._descriptionLoc.Add("modifier_description", this.Modifier.Description.GetFormattedText());
				this._label.Text = NRunModifierTickbox._descriptionLoc.GetFormattedText();
			}
			base.IsTicked = false;
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x0015043C File Offset: 0x0014E63C
		protected override void OnFocus()
		{
			base.OnFocus();
			if (NControllerManager.Instance.IsUsingController)
			{
				this._highlight.Visible = true;
			}
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x0015045C File Offset: 0x0014E65C
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this._highlight.Visible = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E8B RID: 11915 RVA: 0x00150470 File Offset: 0x0014E670
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NRunModifierTickbox.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunModifierTickbox.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunModifierTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunModifierTickbox.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunModifierTickbox.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E8C RID: 11916 RVA: 0x00150570 File Offset: 0x0014E770
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunModifierTickbox.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunModifierTickbox.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunModifierTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunModifierTickbox.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunModifierTickbox.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002E8D RID: 11917 RVA: 0x00150640 File Offset: 0x0014E840
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRunModifierTickbox.MethodName.OnEnable || (ref method) == NRunModifierTickbox.MethodName.OnDisable || (ref method) == NRunModifierTickbox.MethodName._Ready || (ref method) == NRunModifierTickbox.MethodName.OnFocus || (ref method) == NRunModifierTickbox.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002E8E RID: 11918 RVA: 0x0015069F File Offset: 0x0014E89F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRunModifierTickbox.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunModifierTickbox.PropertyName._highlight)
			{
				this._highlight = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002E8F RID: 11919 RVA: 0x001506E0 File Offset: 0x0014E8E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRunModifierTickbox.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NRunModifierTickbox.PropertyName._highlight)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._highlight);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E90 RID: 11920 RVA: 0x00150738 File Offset: 0x0014E938
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRunModifierTickbox.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunModifierTickbox.PropertyName._highlight, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E91 RID: 11921 RVA: 0x0015078E File Offset: 0x0014E98E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRunModifierTickbox.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NRunModifierTickbox.PropertyName._highlight, Variant.From<Control>(ref this._highlight));
		}

		/// <inheritdoc />
		// Token: 0x06002E92 RID: 11922 RVA: 0x001507C4 File Offset: 0x0014E9C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRunModifierTickbox.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRunModifierTickbox.PropertyName._highlight, ref variant2))
			{
				this._highlight = variant2.As<Control>();
			}
		}

		// Token: 0x040011C6 RID: 4550
		[Nullable(1)]
		public const string scenePath = "res://scenes/screens/custom_run/modifier_tickbox.tscn";

		// Token: 0x040011C7 RID: 4551
		[Nullable(1)]
		private static readonly LocString _descriptionLoc = new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.MODIFIER_LABEL");

		// Token: 0x040011C8 RID: 4552
		[Nullable(1)]
		private MegaRichTextLabel _label;

		// Token: 0x040011C9 RID: 4553
		[Nullable(1)]
		private Control _highlight;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015C9 RID: 5577
		public new class MethodName : NTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x040053E9 RID: 21481
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x040053EA RID: 21482
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040053EB RID: 21483
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040053EC RID: 21484
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040053ED RID: 21485
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015CA RID: 5578
		public new class PropertyName : NTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x040053EE RID: 21486
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_highlight' field.
			/// </summary>
			// Token: 0x040053EF RID: 21487
			public static readonly StringName _highlight = "_highlight";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015CB RID: 5579
		public new class SignalName : NTickbox.SignalName
		{
		}
	}
}
