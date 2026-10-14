using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen
{
	// Token: 0x02000326 RID: 806
	[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NSendFeedbackEmojiButton.cs")]
	public class NSendFeedbackEmojiButton : NButton
	{
		// Token: 0x06002CF0 RID: 11504 RVA: 0x001408D7 File Offset: 0x0013EAD7
		public override void _Ready()
		{
			this.ConnectSignals();
			this._selectionReticle = base.GetNode<NSelectionReticle>("SelectionReticle");
			base.PivotOffset = base.Size * 0.5f;
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x0014090B File Offset: 0x0013EB0B
		protected override void OnFocus()
		{
			base.OnFocus();
			base.Scale = Vector2.One * 1.2f;
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x0014093F File Offset: 0x0013EB3F
		protected override void OnUnfocus()
		{
			base.OnFocus();
			if (!this._isSelected)
			{
				this._selectionReticle.OnDeselect();
				base.Scale = Vector2.One;
			}
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x00140968 File Offset: 0x0013EB68
		public void SetSelected(bool isSelected)
		{
			if (isSelected)
			{
				base.Scale = Vector2.One * 1.2f;
				base.SelfModulate = StsColors.blueGlow;
			}
			else
			{
				base.Scale = Vector2.One;
				base.SelfModulate = NSendFeedbackEmojiButton._defaultColor;
			}
			this._isSelected = isSelected;
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x001409B8 File Offset: 0x0013EBB8
		public void FlashError()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "self_modulate", StsColors.redGlow, 0.10000000149011612);
			this._tween.TweenProperty(this, "self_modulate", NSendFeedbackEmojiButton._defaultColor, 0.10000000149011612);
			this._tween.TweenProperty(this, "self_modulate", StsColors.redGlow, 0.10000000149011612);
			this._tween.TweenProperty(this, "self_modulate", NSendFeedbackEmojiButton._defaultColor, 0.10000000149011612);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CF5 RID: 11509 RVA: 0x00140A8C File Offset: 0x0013EC8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NSendFeedbackEmojiButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackEmojiButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackEmojiButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackEmojiButton.MethodName.SetSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isSelected", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSendFeedbackEmojiButton.MethodName.FlashError, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CF6 RID: 11510 RVA: 0x00140BB4 File Offset: 0x0013EDB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSendFeedbackEmojiButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackEmojiButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackEmojiButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackEmojiButton.MethodName.SetSelected && args.Count == 1)
			{
				this.SetSelected(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackEmojiButton.MethodName.FlashError && args.Count == 0)
			{
				this.FlashError();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002CF7 RID: 11511 RVA: 0x00140C94 File Offset: 0x0013EE94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSendFeedbackEmojiButton.MethodName._Ready || (ref method) == NSendFeedbackEmojiButton.MethodName.OnFocus || (ref method) == NSendFeedbackEmojiButton.MethodName.OnUnfocus || (ref method) == NSendFeedbackEmojiButton.MethodName.SetSelected || (ref method) == NSendFeedbackEmojiButton.MethodName.FlashError || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002CF8 RID: 11512 RVA: 0x00140CF4 File Offset: 0x0013EEF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSendFeedbackEmojiButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackEmojiButton.PropertyName._isSelected)
			{
				this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackEmojiButton.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002CF9 RID: 11513 RVA: 0x00140D5C File Offset: 0x0013EF5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSendFeedbackEmojiButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NSendFeedbackEmojiButton.PropertyName._isSelected)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
				return true;
			}
			if ((ref name) == NSendFeedbackEmojiButton.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CFA RID: 11514 RVA: 0x00140DD4 File Offset: 0x0013EFD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSendFeedbackEmojiButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSendFeedbackEmojiButton.PropertyName._isSelected, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackEmojiButton.PropertyName._selectionReticle, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CFB RID: 11515 RVA: 0x00140E4C File Offset: 0x0013F04C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSendFeedbackEmojiButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NSendFeedbackEmojiButton.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
			info.AddProperty(NSendFeedbackEmojiButton.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
		}

		/// <inheritdoc />
		// Token: 0x06002CFC RID: 11516 RVA: 0x00140EA4 File Offset: 0x0013F0A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSendFeedbackEmojiButton.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSendFeedbackEmojiButton.PropertyName._isSelected, ref variant2))
			{
				this._isSelected = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSendFeedbackEmojiButton.PropertyName._selectionReticle, ref variant3))
			{
				this._selectionReticle = variant3.As<NSelectionReticle>();
			}
		}

		// Token: 0x0400110C RID: 4364
		private static readonly Color _defaultColor = new Color(0.1f, 0.1f, 0.1f, 1f);

		// Token: 0x0400110D RID: 4365
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x0400110E RID: 4366
		private bool _isSelected;

		// Token: 0x0400110F RID: 4367
		[Nullable(1)]
		private NSelectionReticle _selectionReticle;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200157B RID: 5499
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400523C RID: 21052
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400523D RID: 21053
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400523E RID: 21054
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'SetSelected' method.
			/// </summary>
			// Token: 0x0400523F RID: 21055
			public static readonly StringName SetSelected = "SetSelected";

			/// <summary>
			/// Cached name for the 'FlashError' method.
			/// </summary>
			// Token: 0x04005240 RID: 21056
			public static readonly StringName FlashError = "FlashError";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200157C RID: 5500
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005241 RID: 21057
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_isSelected' field.
			/// </summary>
			// Token: 0x04005242 RID: 21058
			public static readonly StringName _isSelected = "_isSelected";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04005243 RID: 21059
			public static readonly StringName _selectionReticle = "_selectionReticle";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200157D RID: 5501
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
