using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// A popup ui/modal which has a Yes and No button.
	/// Used for important popups like Abandon Run confirmation and the "Enable Tutorials?" popup.
	/// </summary>
	// Token: 0x020003F0 RID: 1008
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NVerticalPopup.cs")]
	public class NVerticalPopup : Control
	{
		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06003DB9 RID: 15801 RVA: 0x001D92F0 File Offset: 0x001D74F0
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NVerticalPopup._scenePath);
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x001D92FC File Offset: 0x001D74FC
		// (set) Token: 0x06003DBB RID: 15803 RVA: 0x001D9304 File Offset: 0x001D7504
		private MegaLabel TitleLabel { get; set; }

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06003DBC RID: 15804 RVA: 0x001D930D File Offset: 0x001D750D
		// (set) Token: 0x06003DBD RID: 15805 RVA: 0x001D9315 File Offset: 0x001D7515
		private MegaRichTextLabel BodyLabel { get; set; }

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06003DBE RID: 15806 RVA: 0x001D931E File Offset: 0x001D751E
		// (set) Token: 0x06003DBF RID: 15807 RVA: 0x001D9326 File Offset: 0x001D7526
		public NPopupYesNoButton YesButton { get; private set; }

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x001D932F File Offset: 0x001D752F
		// (set) Token: 0x06003DC1 RID: 15809 RVA: 0x001D9337 File Offset: 0x001D7537
		public NPopupYesNoButton NoButton { get; private set; }

		// Token: 0x06003DC2 RID: 15810 RVA: 0x001D9340 File Offset: 0x001D7540
		public override void _Ready()
		{
			this.EnsureNodesAreSet();
		}

		/// <summary>
		/// This is to ensure that the node parameters are set. We have to do this because
		/// AddChildSafely may defer the call to add child (and thus defer the _Ready), and
		/// can lead to timing issues in NGenericPopup where we try to SetText before the label
		/// parameters have been set. While this is a vulnerability that can techinically happen
		/// for any node added via AddChildSafely, I think we are seeing a particularly high
		/// number of errors here because the vertical popup is bieng created at the start of the game
		/// for a startup errors.
		/// </summary>
		// Token: 0x06003DC3 RID: 15811 RVA: 0x001D9348 File Offset: 0x001D7548
		private void EnsureNodesAreSet()
		{
			if (this._nodesAreSet)
			{
				return;
			}
			this.TitleLabel = base.GetNode<MegaLabel>("Header");
			this.BodyLabel = base.GetNode<MegaRichTextLabel>("Description");
			this.YesButton = base.GetNode<NPopupYesNoButton>("YesButton");
			this.NoButton = base.GetNode<NPopupYesNoButton>("NoButton");
			this._nodesAreSet = true;
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x001D93BD File Offset: 0x001D75BD
		public void SetText(LocString title, LocString body)
		{
			this.EnsureNodesAreSet();
			this.TitleLabel.SetTextAutoSize(title.GetFormattedText());
			this.BodyLabel.SetTextAutoSize(body.GetFormattedText());
		}

		/// <summary>
		/// Sets the popup text using raw strings instead of localization.
		/// Use this when localization may be broken (e.g., showing localization errors).
		/// </summary>
		// Token: 0x06003DC5 RID: 15813 RVA: 0x001D93E7 File Offset: 0x001D75E7
		public void SetText(string title, string body)
		{
			this.EnsureNodesAreSet();
			this.TitleLabel.SetTextAutoSize(title);
			this.BodyLabel.SetTextAutoSize(body);
		}

		/// <summary>
		/// Initializes the yes button.
		/// If this is not called, then the yes button is hidden.
		/// </summary>
		// Token: 0x06003DC6 RID: 15814 RVA: 0x001D9408 File Offset: 0x001D7608
		public void InitYesButton(LocString yesButton, Action<NButton> onPressed)
		{
			this.EnsureNodesAreSet();
			this._yesCallable = new Callable?(Callable.From<NButton>(onPressed));
			this.YesButton.IsYes = true;
			this.YesButton.SetText(yesButton.GetFormattedText());
			this.YesButton.Connect(NClickableControl.SignalName.Released, this._yesCallable.Value, 0U);
			this.YesButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.Close)), 0U);
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x001D948C File Offset: 0x001D768C
		public void InitNoButton(LocString noButton, Action<NButton> onPressed)
		{
			this.EnsureNodesAreSet();
			this._noCallable = new Callable?(Callable.From<NButton>(onPressed));
			this.NoButton.Visible = true;
			this.NoButton.IsYes = false;
			this.NoButton.SetText(noButton.GetFormattedText());
			this.NoButton.Connect(NClickableControl.SignalName.Released, this._noCallable.Value, 0U);
			this.NoButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.Close)), 0U);
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x001D9519 File Offset: 0x001D7719
		private void Close(NButton _)
		{
			NModalContainer.Instance.Clear();
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x001D9525 File Offset: 0x001D7725
		public void HideNoButton()
		{
			this.NoButton.Visible = false;
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x001D9534 File Offset: 0x001D7734
		public void DisconnectSignals()
		{
			if (this._yesCallable != null)
			{
				this.YesButton.Disconnect(NClickableControl.SignalName.Released, this._yesCallable.Value);
				this.YesButton.Disconnect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.Close)));
			}
			if (this._noCallable != null)
			{
				this.NoButton.Disconnect(NClickableControl.SignalName.Released, this._noCallable.Value);
				this.NoButton.Disconnect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.Close)));
			}
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x001D95D3 File Offset: 0x001D77D3
		public void DisconnectHotkeys()
		{
			if (this._yesCallable != null)
			{
				this.YesButton.DisconnectHotkeys();
			}
			if (this._noCallable != null)
			{
				this.NoButton.DisconnectHotkeys();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003DCC RID: 15820 RVA: 0x001D9608 File Offset: 0x001D7808
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NVerticalPopup.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVerticalPopup.MethodName.EnsureNodesAreSet, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVerticalPopup.MethodName.SetText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "title", 0L, "", 6L, false),
					new PropertyInfo(4L, "body", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NVerticalPopup.MethodName.Close, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NVerticalPopup.MethodName.HideNoButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVerticalPopup.MethodName.DisconnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVerticalPopup.MethodName.DisconnectHotkeys, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003DCD RID: 15821 RVA: 0x001D97DC File Offset: 0x001D79DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NVerticalPopup.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVerticalPopup.MethodName.EnsureNodesAreSet && args.Count == 0)
			{
				this.EnsureNodesAreSet();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVerticalPopup.MethodName.SetText && args.Count == 2)
			{
				this.SetText(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVerticalPopup.MethodName.Close && args.Count == 1)
			{
				this.Close(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVerticalPopup.MethodName.HideNoButton && args.Count == 0)
			{
				this.HideNoButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVerticalPopup.MethodName.DisconnectSignals && args.Count == 0)
			{
				this.DisconnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVerticalPopup.MethodName.DisconnectHotkeys && args.Count == 0)
			{
				this.DisconnectHotkeys();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003DCE RID: 15822 RVA: 0x001D9920 File Offset: 0x001D7B20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NVerticalPopup.MethodName._Ready || (ref method) == NVerticalPopup.MethodName.EnsureNodesAreSet || (ref method) == NVerticalPopup.MethodName.SetText || (ref method) == NVerticalPopup.MethodName.Close || (ref method) == NVerticalPopup.MethodName.HideNoButton || (ref method) == NVerticalPopup.MethodName.DisconnectSignals || (ref method) == NVerticalPopup.MethodName.DisconnectHotkeys || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003DCF RID: 15823 RVA: 0x001D99A0 File Offset: 0x001D7BA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NVerticalPopup.PropertyName.TitleLabel)
			{
				this.TitleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NVerticalPopup.PropertyName.BodyLabel)
			{
				this.BodyLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NVerticalPopup.PropertyName.YesButton)
			{
				this.YesButton = VariantUtils.ConvertTo<NPopupYesNoButton>(ref value);
				return true;
			}
			if ((ref name) == NVerticalPopup.PropertyName.NoButton)
			{
				this.NoButton = VariantUtils.ConvertTo<NPopupYesNoButton>(ref value);
				return true;
			}
			if ((ref name) == NVerticalPopup.PropertyName._nodesAreSet)
			{
				this._nodesAreSet = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003DD0 RID: 15824 RVA: 0x001D9A3C File Offset: 0x001D7C3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NVerticalPopup.PropertyName.TitleLabel)
			{
				MegaLabel titleLabel = this.TitleLabel;
				value = VariantUtils.CreateFrom<MegaLabel>(ref titleLabel);
				return true;
			}
			if ((ref name) == NVerticalPopup.PropertyName.BodyLabel)
			{
				MegaRichTextLabel bodyLabel = this.BodyLabel;
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref bodyLabel);
				return true;
			}
			if ((ref name) == NVerticalPopup.PropertyName.YesButton)
			{
				NPopupYesNoButton npopupYesNoButton = this.YesButton;
				value = VariantUtils.CreateFrom<NPopupYesNoButton>(ref npopupYesNoButton);
				return true;
			}
			if ((ref name) == NVerticalPopup.PropertyName.NoButton)
			{
				NPopupYesNoButton npopupYesNoButton = this.NoButton;
				value = VariantUtils.CreateFrom<NPopupYesNoButton>(ref npopupYesNoButton);
				return true;
			}
			if ((ref name) == NVerticalPopup.PropertyName._nodesAreSet)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._nodesAreSet);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003DD1 RID: 15825 RVA: 0x001D9B00 File Offset: 0x001D7D00
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NVerticalPopup.PropertyName.TitleLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVerticalPopup.PropertyName.BodyLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVerticalPopup.PropertyName.YesButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NVerticalPopup.PropertyName.NoButton, 0L, "", 4096L, false),
				new PropertyInfo(1L, NVerticalPopup.PropertyName._nodesAreSet, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003DD2 RID: 15826 RVA: 0x001D9BB8 File Offset: 0x001D7DB8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName titleLabel = NVerticalPopup.PropertyName.TitleLabel;
			MegaLabel titleLabel2 = this.TitleLabel;
			info.AddProperty(titleLabel, Variant.From<MegaLabel>(ref titleLabel2));
			StringName bodyLabel = NVerticalPopup.PropertyName.BodyLabel;
			MegaRichTextLabel bodyLabel2 = this.BodyLabel;
			info.AddProperty(bodyLabel, Variant.From<MegaRichTextLabel>(ref bodyLabel2));
			StringName yesButton = NVerticalPopup.PropertyName.YesButton;
			NPopupYesNoButton npopupYesNoButton = this.YesButton;
			info.AddProperty(yesButton, Variant.From<NPopupYesNoButton>(ref npopupYesNoButton));
			StringName noButton = NVerticalPopup.PropertyName.NoButton;
			npopupYesNoButton = this.NoButton;
			info.AddProperty(noButton, Variant.From<NPopupYesNoButton>(ref npopupYesNoButton));
			info.AddProperty(NVerticalPopup.PropertyName._nodesAreSet, Variant.From<bool>(ref this._nodesAreSet));
		}

		/// <inheritdoc />
		// Token: 0x06003DD3 RID: 15827 RVA: 0x001D9C48 File Offset: 0x001D7E48
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NVerticalPopup.PropertyName.TitleLabel, ref variant))
			{
				this.TitleLabel = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NVerticalPopup.PropertyName.BodyLabel, ref variant2))
			{
				this.BodyLabel = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NVerticalPopup.PropertyName.YesButton, ref variant3))
			{
				this.YesButton = variant3.As<NPopupYesNoButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NVerticalPopup.PropertyName.NoButton, ref variant4))
			{
				this.NoButton = variant4.As<NPopupYesNoButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NVerticalPopup.PropertyName._nodesAreSet, ref variant5))
			{
				this._nodesAreSet = variant5.As<bool>();
			}
		}

		// Token: 0x04001813 RID: 6163
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/vertical_popup");

		// Token: 0x04001818 RID: 6168
		private bool _nodesAreSet;

		// Token: 0x04001819 RID: 6169
		private Callable? _yesCallable;

		// Token: 0x0400181A RID: 6170
		private Callable? _noCallable;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001895 RID: 6293
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006023 RID: 24611
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'EnsureNodesAreSet' method.
			/// </summary>
			// Token: 0x04006024 RID: 24612
			public static readonly StringName EnsureNodesAreSet = "EnsureNodesAreSet";

			/// <summary>
			/// Cached name for the 'SetText' method.
			/// </summary>
			// Token: 0x04006025 RID: 24613
			public static readonly StringName SetText = "SetText";

			/// <summary>
			/// Cached name for the 'Close' method.
			/// </summary>
			// Token: 0x04006026 RID: 24614
			public static readonly StringName Close = "Close";

			/// <summary>
			/// Cached name for the 'HideNoButton' method.
			/// </summary>
			// Token: 0x04006027 RID: 24615
			public static readonly StringName HideNoButton = "HideNoButton";

			/// <summary>
			/// Cached name for the 'DisconnectSignals' method.
			/// </summary>
			// Token: 0x04006028 RID: 24616
			public static readonly StringName DisconnectSignals = "DisconnectSignals";

			/// <summary>
			/// Cached name for the 'DisconnectHotkeys' method.
			/// </summary>
			// Token: 0x04006029 RID: 24617
			public static readonly StringName DisconnectHotkeys = "DisconnectHotkeys";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001896 RID: 6294
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'TitleLabel' property.
			/// </summary>
			// Token: 0x0400602A RID: 24618
			public static readonly StringName TitleLabel = "TitleLabel";

			/// <summary>
			/// Cached name for the 'BodyLabel' property.
			/// </summary>
			// Token: 0x0400602B RID: 24619
			public static readonly StringName BodyLabel = "BodyLabel";

			/// <summary>
			/// Cached name for the 'YesButton' property.
			/// </summary>
			// Token: 0x0400602C RID: 24620
			public static readonly StringName YesButton = "YesButton";

			/// <summary>
			/// Cached name for the 'NoButton' property.
			/// </summary>
			// Token: 0x0400602D RID: 24621
			public static readonly StringName NoButton = "NoButton";

			/// <summary>
			/// Cached name for the '_nodesAreSet' field.
			/// </summary>
			// Token: 0x0400602E RID: 24622
			public static readonly StringName _nodesAreSet = "_nodesAreSet";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001897 RID: 6295
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
