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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	/// <summary>
	/// Ftue which appears when a player is presented with an option to receive a Power card in a Card Rewards screen,
	/// Choose Card Selection screen, or a Shop screen. NOTE: I lied, these last two cases aren't implemented yet.
	/// </summary>
	// Token: 0x020003B2 RID: 946
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Ftue/NPowerCardFtue.cs")]
	public class NPowerCardFtue : NFtue
	{
		// Token: 0x060038EE RID: 14574 RVA: 0x001AE864 File Offset: 0x001ACA64
		public override void _Ready()
		{
			this._header = base.GetNode<MegaLabel>("FtuePopup/Header");
			this._header.SetTextAutoSize(new LocString("ftues", "POWER_FTUE_TITLE").GetFormattedText());
			this._description = base.GetNode<MegaRichTextLabel>("FtuePopup/DescriptionContainer/Description");
			this._description.Text = new LocString("ftues", "POWER_FTUE_DESCRIPTION").GetFormattedText();
			this._confirmButton = base.GetNode<NButton>("FtuePopup/FtueConfirmButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
			this._ftueHolder = base.GetNode<Control>("FtuePopup");
			this._defaultZIndex = this._card.ZIndex;
			Control card = this._card;
			int zindex = card.ZIndex;
			card.ZIndex = zindex + 1;
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x001AE950 File Offset: 0x001ACB50
		[return: Nullable(2)]
		public static NPowerCardFtue Create(Control card)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NPowerCardFtue npowerCardFtue = PreloadManager.Cache.GetScene(NPowerCardFtue._scenePath).Instantiate<NPowerCardFtue>(0L);
			npowerCardFtue._card = card;
			return npowerCardFtue;
		}

		// Token: 0x060038F0 RID: 14576 RVA: 0x001AE985 File Offset: 0x001ACB85
		private void CloseFtue(NButton _)
		{
			this._card.ZIndex = this._defaultZIndex;
			base.CloseFtue();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060038F1 RID: 14577 RVA: 0x001AE9A0 File Offset: 0x001ACBA0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NPowerCardFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPowerCardFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NPowerCardFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038F2 RID: 14578 RVA: 0x001AEAB0 File Offset: 0x001ACCB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPowerCardFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPowerCardFtue.MethodName.Create && args.Count == 1)
			{
				NPowerCardFtue npowerCardFtue = NPowerCardFtue.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NPowerCardFtue>(ref npowerCardFtue);
				return true;
			}
			if ((ref method) == NPowerCardFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060038F3 RID: 14579 RVA: 0x001AEB58 File Offset: 0x001ACD58
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPowerCardFtue.MethodName.Create && args.Count == 1)
			{
				NPowerCardFtue npowerCardFtue = NPowerCardFtue.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NPowerCardFtue>(ref npowerCardFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060038F4 RID: 14580 RVA: 0x001AEBA6 File Offset: 0x001ACDA6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPowerCardFtue.MethodName._Ready || (ref method) == NPowerCardFtue.MethodName.Create || (ref method) == NPowerCardFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060038F5 RID: 14581 RVA: 0x001AEBDC File Offset: 0x001ACDDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPowerCardFtue.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NPowerCardFtue.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPowerCardFtue.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NPowerCardFtue.PropertyName._card)
			{
				this._card = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPowerCardFtue.PropertyName._ftueHolder)
			{
				this._ftueHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPowerCardFtue.PropertyName._defaultZIndex)
			{
				this._defaultZIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060038F6 RID: 14582 RVA: 0x001AEC94 File Offset: 0x001ACE94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPowerCardFtue.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NPowerCardFtue.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NPowerCardFtue.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NPowerCardFtue.PropertyName._card)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._card);
				return true;
			}
			if ((ref name) == NPowerCardFtue.PropertyName._ftueHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._ftueHolder);
				return true;
			}
			if ((ref name) == NPowerCardFtue.PropertyName._defaultZIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._defaultZIndex);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060038F7 RID: 14583 RVA: 0x001AED6C File Offset: 0x001ACF6C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPowerCardFtue.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerCardFtue.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerCardFtue.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerCardFtue.PropertyName._card, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPowerCardFtue.PropertyName._ftueHolder, 0L, "", 4096L, false),
				new PropertyInfo(2L, NPowerCardFtue.PropertyName._defaultZIndex, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038F8 RID: 14584 RVA: 0x001AEE48 File Offset: 0x001AD048
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPowerCardFtue.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
			info.AddProperty(NPowerCardFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NPowerCardFtue.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NPowerCardFtue.PropertyName._card, Variant.From<Control>(ref this._card));
			info.AddProperty(NPowerCardFtue.PropertyName._ftueHolder, Variant.From<Control>(ref this._ftueHolder));
			info.AddProperty(NPowerCardFtue.PropertyName._defaultZIndex, Variant.From<int>(ref this._defaultZIndex));
		}

		/// <inheritdoc />
		// Token: 0x060038F9 RID: 14585 RVA: 0x001AEEE0 File Offset: 0x001AD0E0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPowerCardFtue.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPowerCardFtue.PropertyName._header, ref variant2))
			{
				this._header = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPowerCardFtue.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPowerCardFtue.PropertyName._card, ref variant4))
			{
				this._card = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPowerCardFtue.PropertyName._ftueHolder, ref variant5))
			{
				this._ftueHolder = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPowerCardFtue.PropertyName._defaultZIndex, ref variant6))
			{
				this._defaultZIndex = variant6.As<int>();
			}
		}

		// Token: 0x04001617 RID: 5655
		public const string id = "power_card_ftue";

		// Token: 0x04001618 RID: 5656
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/power_card_ftue");

		// Token: 0x04001619 RID: 5657
		private NButton _confirmButton;

		// Token: 0x0400161A RID: 5658
		private MegaLabel _header;

		// Token: 0x0400161B RID: 5659
		private MegaRichTextLabel _description;

		// Token: 0x0400161C RID: 5660
		private Control _card;

		// Token: 0x0400161D RID: 5661
		private Control _ftueHolder;

		// Token: 0x0400161E RID: 5662
		private int _defaultZIndex;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017B2 RID: 6066
		[NullableContext(0)]
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C4F RID: 23631
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005C50 RID: 23632
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005C51 RID: 23633
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017B3 RID: 6067
		[NullableContext(0)]
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04005C52 RID: 23634
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005C53 RID: 23635
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005C54 RID: 23636
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_card' field.
			/// </summary>
			// Token: 0x04005C55 RID: 23637
			public static readonly StringName _card = "_card";

			/// <summary>
			/// Cached name for the '_ftueHolder' field.
			/// </summary>
			// Token: 0x04005C56 RID: 23638
			public static readonly StringName _ftueHolder = "_ftueHolder";

			/// <summary>
			/// Cached name for the '_defaultZIndex' field.
			/// </summary>
			// Token: 0x04005C57 RID: 23639
			public static readonly StringName _defaultZIndex = "_defaultZIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017B4 RID: 6068
		[NullableContext(0)]
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
