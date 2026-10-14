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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	/// <summary>
	/// Ftue which appears when a player presses Proceed without interacting with the Merchant.
	/// Auto-disables if the player interacts with the Merchant.
	/// </summary>
	// Token: 0x020003B0 RID: 944
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Ftue/NMerchantFtue.cs")]
	public class NMerchantFtue : NFtue
	{
		// Token: 0x060038D1 RID: 14545 RVA: 0x001ADA28 File Offset: 0x001ABC28
		public override void _Ready()
		{
			this._header = base.GetNode<MegaLabel>("FtuePopup/Header");
			this._header.SetTextAutoSize(new LocString("ftues", "MERCHANT_FTUE_TITLE").GetFormattedText());
			this._description = base.GetNode<MegaRichTextLabel>("FtuePopup/DescriptionContainer/Description");
			this._description.Text = new LocString("ftues", "MERCHANT_FTUE_DESCRIPTION").GetFormattedText();
			this._confirmButton = base.GetNode<NButton>("FtuePopup/FtueConfirmButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
			this._sneakyHitbox = base.GetNode<Control>("SneakyHitbox");
			this._sneakyHitbox.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtueAndOpenRug)), 0U);
			this._defaultZIndex = this._merchantRoom.MerchantButton.ZIndex;
			NMerchantButton merchantButton = this._merchantRoom.MerchantButton;
			int zindex = merchantButton.ZIndex;
			merchantButton.ZIndex = zindex + 1;
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x001ADB44 File Offset: 0x001ABD44
		[return: Nullable(2)]
		public static NMerchantFtue Create(NMerchantRoom merchantRoom)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NMerchantFtue nmerchantFtue = PreloadManager.Cache.GetScene(NMerchantFtue._scenePath).Instantiate<NMerchantFtue>(0L);
			nmerchantFtue._merchantRoom = merchantRoom;
			return nmerchantFtue;
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x001ADB79 File Offset: 0x001ABD79
		private void CloseFtueAndOpenRug(NButton _)
		{
			this._merchantRoom.OpenInventory();
			this.CloseFtue(_);
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x001ADB8D File Offset: 0x001ABD8D
		private void CloseFtue(NButton _)
		{
			this._merchantRoom.MerchantButton.ZIndex = this._defaultZIndex;
			base.CloseFtue();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060038D5 RID: 14549 RVA: 0x001ADBAC File Offset: 0x001ABDAC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NMerchantFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "merchantRoom", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMerchantFtue.MethodName.CloseFtueAndOpenRug, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMerchantFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038D6 RID: 14550 RVA: 0x001ADD1C File Offset: 0x001ABF1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantFtue.MethodName.Create && args.Count == 1)
			{
				NMerchantFtue nmerchantFtue = NMerchantFtue.Create(VariantUtils.ConvertTo<NMerchantRoom>(args[0]));
				ret = VariantUtils.CreateFrom<NMerchantFtue>(ref nmerchantFtue);
				return true;
			}
			if ((ref method) == NMerchantFtue.MethodName.CloseFtueAndOpenRug && args.Count == 1)
			{
				this.CloseFtueAndOpenRug(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x001ADDF8 File Offset: 0x001ABFF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantFtue.MethodName.Create && args.Count == 1)
			{
				NMerchantFtue nmerchantFtue = NMerchantFtue.Create(VariantUtils.ConvertTo<NMerchantRoom>(args[0]));
				ret = VariantUtils.CreateFrom<NMerchantFtue>(ref nmerchantFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060038D8 RID: 14552 RVA: 0x001ADE48 File Offset: 0x001AC048
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantFtue.MethodName._Ready || (ref method) == NMerchantFtue.MethodName.Create || (ref method) == NMerchantFtue.MethodName.CloseFtueAndOpenRug || (ref method) == NMerchantFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060038D9 RID: 14553 RVA: 0x001ADE98 File Offset: 0x001AC098
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMerchantFtue.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NMerchantFtue.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMerchantFtue.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMerchantFtue.PropertyName._sneakyHitbox)
			{
				this._sneakyHitbox = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMerchantFtue.PropertyName._merchantRoom)
			{
				this._merchantRoom = VariantUtils.ConvertTo<NMerchantRoom>(ref value);
				return true;
			}
			if ((ref name) == NMerchantFtue.PropertyName._defaultZIndex)
			{
				this._defaultZIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060038DA RID: 14554 RVA: 0x001ADF50 File Offset: 0x001AC150
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMerchantFtue.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NMerchantFtue.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NMerchantFtue.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NMerchantFtue.PropertyName._sneakyHitbox)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._sneakyHitbox);
				return true;
			}
			if ((ref name) == NMerchantFtue.PropertyName._merchantRoom)
			{
				value = VariantUtils.CreateFrom<NMerchantRoom>(ref this._merchantRoom);
				return true;
			}
			if ((ref name) == NMerchantFtue.PropertyName._defaultZIndex)
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
		// Token: 0x060038DB RID: 14555 RVA: 0x001AE028 File Offset: 0x001AC228
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMerchantFtue.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantFtue.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantFtue.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantFtue.PropertyName._sneakyHitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantFtue.PropertyName._merchantRoom, 0L, "", 4096L, false),
				new PropertyInfo(2L, NMerchantFtue.PropertyName._defaultZIndex, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038DC RID: 14556 RVA: 0x001AE104 File Offset: 0x001AC304
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMerchantFtue.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
			info.AddProperty(NMerchantFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NMerchantFtue.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NMerchantFtue.PropertyName._sneakyHitbox, Variant.From<Control>(ref this._sneakyHitbox));
			info.AddProperty(NMerchantFtue.PropertyName._merchantRoom, Variant.From<NMerchantRoom>(ref this._merchantRoom));
			info.AddProperty(NMerchantFtue.PropertyName._defaultZIndex, Variant.From<int>(ref this._defaultZIndex));
		}

		/// <inheritdoc />
		// Token: 0x060038DD RID: 14557 RVA: 0x001AE19C File Offset: 0x001AC39C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMerchantFtue.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMerchantFtue.PropertyName._header, ref variant2))
			{
				this._header = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMerchantFtue.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMerchantFtue.PropertyName._sneakyHitbox, ref variant4))
			{
				this._sneakyHitbox = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMerchantFtue.PropertyName._merchantRoom, ref variant5))
			{
				this._merchantRoom = variant5.As<NMerchantRoom>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMerchantFtue.PropertyName._defaultZIndex, ref variant6))
			{
				this._defaultZIndex = variant6.As<int>();
			}
		}

		// Token: 0x04001609 RID: 5641
		public const string id = "merchant_ftue";

		// Token: 0x0400160A RID: 5642
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/merchant_ftue");

		// Token: 0x0400160B RID: 5643
		private NButton _confirmButton;

		// Token: 0x0400160C RID: 5644
		private MegaLabel _header;

		// Token: 0x0400160D RID: 5645
		private MegaRichTextLabel _description;

		// Token: 0x0400160E RID: 5646
		private Control _sneakyHitbox;

		// Token: 0x0400160F RID: 5647
		private NMerchantRoom _merchantRoom;

		// Token: 0x04001610 RID: 5648
		private int _defaultZIndex;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017AC RID: 6060
		[NullableContext(0)]
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C3E RID: 23614
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005C3F RID: 23615
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtueAndOpenRug' method.
			/// </summary>
			// Token: 0x04005C40 RID: 23616
			public static readonly StringName CloseFtueAndOpenRug = "CloseFtueAndOpenRug";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005C41 RID: 23617
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017AD RID: 6061
		[NullableContext(0)]
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04005C42 RID: 23618
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005C43 RID: 23619
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005C44 RID: 23620
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_sneakyHitbox' field.
			/// </summary>
			// Token: 0x04005C45 RID: 23621
			public static readonly StringName _sneakyHitbox = "_sneakyHitbox";

			/// <summary>
			/// Cached name for the '_merchantRoom' field.
			/// </summary>
			// Token: 0x04005C46 RID: 23622
			public static readonly StringName _merchantRoom = "_merchantRoom";

			/// <summary>
			/// Cached name for the '_defaultZIndex' field.
			/// </summary>
			// Token: 0x04005C47 RID: 23623
			public static readonly StringName _defaultZIndex = "_defaultZIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017AE RID: 6062
		[NullableContext(0)]
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
