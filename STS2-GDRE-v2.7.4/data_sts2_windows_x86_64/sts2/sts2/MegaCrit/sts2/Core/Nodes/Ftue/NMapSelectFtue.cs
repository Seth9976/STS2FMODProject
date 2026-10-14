using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	/// <summary>
	/// Ftue which appears when a player opens the map for the first time.
	/// Automatically appears after the start of act animation.
	/// </summary>
	// Token: 0x020003AF RID: 943
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Ftue/NMapSelectFtue.cs")]
	public class NMapSelectFtue : NFtue
	{
		// Token: 0x060038C2 RID: 14530 RVA: 0x001AD274 File Offset: 0x001AB474
		public override void _Ready()
		{
			this._header = base.GetNode<MegaLabel>("%Header");
			this._header.SetTextAutoSize(new LocString("ftues", "MAP_SELECT_TITLE").GetFormattedText());
			this._description = base.GetNode<MegaRichTextLabel>("%Description");
			this._description.Text = new LocString("ftues", "MAP_SELECT_DESCRIPTION").GetFormattedText();
			this._confirmButton = base.GetNode<NButton>("%FtueConfirmButton");
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
			base.GetNode<Control>("%Positioner").GlobalPosition = this._ftuePosition;
			this._defaultZIndex = this._firstNode.ZIndex;
			Control firstNode = this._firstNode;
			int zindex = firstNode.ZIndex;
			firstNode.ZIndex = zindex + 1;
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x001AD368 File Offset: 0x001AB568
		public static NMapSelectFtue Create(Control roomNode)
		{
			NMapSelectFtue nmapSelectFtue = PreloadManager.Cache.GetScene(NMapSelectFtue._scenePath).Instantiate<NMapSelectFtue>(0L);
			nmapSelectFtue._ftuePosition = roomNode.GlobalPosition + roomNode.Size * 0.5f;
			nmapSelectFtue._firstNode = roomNode;
			return nmapSelectFtue;
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x001AD3B5 File Offset: 0x001AB5B5
		private void CloseFtue(NButton _)
		{
			this._firstNode.ZIndex = this._defaultZIndex;
			base.CloseFtue();
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x001AD3D0 File Offset: 0x001AB5D0
		public async Task WaitForPlayerToConfirm()
		{
			await this._confirmButton.AwaitSignal<NClickableControl>(NClickableControl.SignalName.Released, this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060038C6 RID: 14534 RVA: 0x001AD414 File Offset: 0x001AB614
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMapSelectFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapSelectFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "roomNode", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMapSelectFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038C7 RID: 14535 RVA: 0x001AD524 File Offset: 0x001AB724
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapSelectFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapSelectFtue.MethodName.Create && args.Count == 1)
			{
				NMapSelectFtue nmapSelectFtue = NMapSelectFtue.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NMapSelectFtue>(ref nmapSelectFtue);
				return true;
			}
			if ((ref method) == NMapSelectFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x001AD5CC File Offset: 0x001AB7CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapSelectFtue.MethodName.Create && args.Count == 1)
			{
				NMapSelectFtue nmapSelectFtue = NMapSelectFtue.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NMapSelectFtue>(ref nmapSelectFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060038C9 RID: 14537 RVA: 0x001AD61A File Offset: 0x001AB81A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapSelectFtue.MethodName._Ready || (ref method) == NMapSelectFtue.MethodName.Create || (ref method) == NMapSelectFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060038CA RID: 14538 RVA: 0x001AD650 File Offset: 0x001AB850
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapSelectFtue.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NMapSelectFtue.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMapSelectFtue.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMapSelectFtue.PropertyName._ftuePosition)
			{
				this._ftuePosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMapSelectFtue.PropertyName._firstNode)
			{
				this._firstNode = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapSelectFtue.PropertyName._defaultZIndex)
			{
				this._defaultZIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060038CB RID: 14539 RVA: 0x001AD708 File Offset: 0x001AB908
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapSelectFtue.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NMapSelectFtue.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NMapSelectFtue.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NMapSelectFtue.PropertyName._ftuePosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._ftuePosition);
				return true;
			}
			if ((ref name) == NMapSelectFtue.PropertyName._firstNode)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._firstNode);
				return true;
			}
			if ((ref name) == NMapSelectFtue.PropertyName._defaultZIndex)
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
		// Token: 0x060038CC RID: 14540 RVA: 0x001AD7E0 File Offset: 0x001AB9E0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapSelectFtue.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapSelectFtue.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapSelectFtue.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMapSelectFtue.PropertyName._ftuePosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapSelectFtue.PropertyName._firstNode, 0L, "", 4096L, false),
				new PropertyInfo(2L, NMapSelectFtue.PropertyName._defaultZIndex, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038CD RID: 14541 RVA: 0x001AD8B8 File Offset: 0x001ABAB8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapSelectFtue.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
			info.AddProperty(NMapSelectFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NMapSelectFtue.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NMapSelectFtue.PropertyName._ftuePosition, Variant.From<Vector2>(ref this._ftuePosition));
			info.AddProperty(NMapSelectFtue.PropertyName._firstNode, Variant.From<Control>(ref this._firstNode));
			info.AddProperty(NMapSelectFtue.PropertyName._defaultZIndex, Variant.From<int>(ref this._defaultZIndex));
		}

		/// <inheritdoc />
		// Token: 0x060038CE RID: 14542 RVA: 0x001AD950 File Offset: 0x001ABB50
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapSelectFtue.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapSelectFtue.PropertyName._header, ref variant2))
			{
				this._header = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapSelectFtue.PropertyName._description, ref variant3))
			{
				this._description = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMapSelectFtue.PropertyName._ftuePosition, ref variant4))
			{
				this._ftuePosition = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMapSelectFtue.PropertyName._firstNode, ref variant5))
			{
				this._firstNode = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMapSelectFtue.PropertyName._defaultZIndex, ref variant6))
			{
				this._defaultZIndex = variant6.As<int>();
			}
		}

		// Token: 0x04001601 RID: 5633
		public const string id = "map_select_ftue";

		// Token: 0x04001602 RID: 5634
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/map_select_ftue");

		// Token: 0x04001603 RID: 5635
		private NButton _confirmButton;

		// Token: 0x04001604 RID: 5636
		private MegaLabel _header;

		// Token: 0x04001605 RID: 5637
		private MegaRichTextLabel _description;

		// Token: 0x04001606 RID: 5638
		private Vector2 _ftuePosition;

		// Token: 0x04001607 RID: 5639
		private Control _firstNode;

		// Token: 0x04001608 RID: 5640
		private int _defaultZIndex;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017A8 RID: 6056
		[NullableContext(0)]
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C31 RID: 23601
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005C32 RID: 23602
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005C33 RID: 23603
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017A9 RID: 6057
		[NullableContext(0)]
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x04005C34 RID: 23604
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005C35 RID: 23605
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005C36 RID: 23606
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_ftuePosition' field.
			/// </summary>
			// Token: 0x04005C37 RID: 23607
			public static readonly StringName _ftuePosition = "_ftuePosition";

			/// <summary>
			/// Cached name for the '_firstNode' field.
			/// </summary>
			// Token: 0x04005C38 RID: 23608
			public static readonly StringName _firstNode = "_firstNode";

			/// <summary>
			/// Cached name for the '_defaultZIndex' field.
			/// </summary>
			// Token: 0x04005C39 RID: 23609
			public static readonly StringName _defaultZIndex = "_defaultZIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017AA RID: 6058
		[NullableContext(0)]
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
