using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	// Token: 0x0200025A RID: 602
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NChapterPaginateButton.cs")]
	public class NChapterPaginateButton : NButton
	{
		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x000D0564 File Offset: 0x000CE764
		[Nullable(1)]
		protected override string ClickedSfx
		{
			[NullableContext(1)]
			get
			{
				return "event:/sfx/ui/timeline/ui_timeline_click";
			}
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x000D056B File Offset: 0x000CE76B
		public override void _Ready()
		{
			this.ConnectSignals();
			this._icon = base.GetNode<TextureRect>("Arrow");
			base.SetPivotOffset(base.Size * 0.5f);
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x000D059F File Offset: 0x000CE79F
		public void SetDirection(bool isFacingRight)
		{
			this._isFacingRight = isFacingRight;
			this._icon.FlipH = !isFacingRight;
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x000D05B7 File Offset: 0x000CE7B7
		protected override void OnEnable()
		{
			base.OnEnable();
			base.Modulate = Colors.White;
			base.MouseFilter = 0L;
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x000D05D2 File Offset: 0x000CE7D2
		protected override void OnDisable()
		{
			base.OnDisable();
			base.Modulate = StsColors.transparentWhite;
			base.MouseFilter = 2L;
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x000D05F0 File Offset: 0x000CE7F0
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 1.1f, 0.05);
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x000D065C File Offset: 0x000CE85C
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x000D06CC File Offset: 0x000CE8CC
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 0.95f, 0.25).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x000D0744 File Offset: 0x000CE944
		protected override void OnRelease()
		{
			base.OnRelease();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.05);
			base.Disable();
			base.MouseFilter = 2L;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F59 RID: 8025 RVA: 0x000D07B4 File Offset: 0x000CE9B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NChapterPaginateButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChapterPaginateButton.MethodName.SetDirection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isFacingRight", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NChapterPaginateButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChapterPaginateButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChapterPaginateButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChapterPaginateButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChapterPaginateButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChapterPaginateButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F5A RID: 8026 RVA: 0x000D0968 File Offset: 0x000CEB68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NChapterPaginateButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChapterPaginateButton.MethodName.SetDirection && args.Count == 1)
			{
				this.SetDirection(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChapterPaginateButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChapterPaginateButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChapterPaginateButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChapterPaginateButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChapterPaginateButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChapterPaginateButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001F5B RID: 8027 RVA: 0x000D0AB4 File Offset: 0x000CECB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NChapterPaginateButton.MethodName._Ready || (ref method) == NChapterPaginateButton.MethodName.SetDirection || (ref method) == NChapterPaginateButton.MethodName.OnEnable || (ref method) == NChapterPaginateButton.MethodName.OnDisable || (ref method) == NChapterPaginateButton.MethodName.OnFocus || (ref method) == NChapterPaginateButton.MethodName.OnUnfocus || (ref method) == NChapterPaginateButton.MethodName.OnPress || (ref method) == NChapterPaginateButton.MethodName.OnRelease || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001F5C RID: 8028 RVA: 0x000D0B40 File Offset: 0x000CED40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NChapterPaginateButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NChapterPaginateButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NChapterPaginateButton.PropertyName._isFacingRight)
			{
				this._isFacingRight = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06001F5D RID: 8029 RVA: 0x000D0BA8 File Offset: 0x000CEDA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NChapterPaginateButton.PropertyName.ClickedSfx)
			{
				string clickedSfx = this.ClickedSfx;
				value = VariantUtils.CreateFrom<string>(ref clickedSfx);
				return true;
			}
			if ((ref name) == NChapterPaginateButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NChapterPaginateButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NChapterPaginateButton.PropertyName._isFacingRight)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isFacingRight);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F5E RID: 8030 RVA: 0x000D0C40 File Offset: 0x000CEE40
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NChapterPaginateButton.PropertyName.ClickedSfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChapterPaginateButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChapterPaginateButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(1L, NChapterPaginateButton.PropertyName._isFacingRight, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F5F RID: 8031 RVA: 0x000D0CD8 File Offset: 0x000CEED8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NChapterPaginateButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NChapterPaginateButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NChapterPaginateButton.PropertyName._isFacingRight, Variant.From<bool>(ref this._isFacingRight));
		}

		/// <inheritdoc />
		// Token: 0x06001F60 RID: 8032 RVA: 0x000D0D30 File Offset: 0x000CEF30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NChapterPaginateButton.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NChapterPaginateButton.PropertyName._icon, ref variant2))
			{
				this._icon = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NChapterPaginateButton.PropertyName._isFacingRight, ref variant3))
			{
				this._isFacingRight = variant3.As<bool>();
			}
		}

		// Token: 0x04000C14 RID: 3092
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000C15 RID: 3093
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x04000C16 RID: 3094
		private bool _isFacingRight;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001299 RID: 4761
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004836 RID: 18486
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetDirection' method.
			/// </summary>
			// Token: 0x04004837 RID: 18487
			public static readonly StringName SetDirection = "SetDirection";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04004838 RID: 18488
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04004839 RID: 18489
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400483A RID: 18490
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400483B RID: 18491
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400483C RID: 18492
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x0400483D RID: 18493
			public new static readonly StringName OnRelease = "OnRelease";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200129A RID: 4762
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ClickedSfx' property.
			/// </summary>
			// Token: 0x0400483E RID: 18494
			public new static readonly StringName ClickedSfx = "ClickedSfx";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400483F RID: 18495
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004840 RID: 18496
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_isFacingRight' field.
			/// </summary>
			// Token: 0x04004841 RID: 18497
			public static readonly StringName _isFacingRight = "_isFacingRight";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200129B RID: 4763
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
