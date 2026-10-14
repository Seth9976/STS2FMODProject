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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen
{
	// Token: 0x02000276 RID: 630
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NStatEntry.cs")]
	public class NStatEntry : NClickableControl
	{
		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002150 RID: 8528 RVA: 0x000E2900 File Offset: 0x000E0B00
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/stats_screen/stats_screen_section");
			}
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000E290C File Offset: 0x000E0B0C
		public static NStatEntry Create(string imgUrl)
		{
			NStatEntry nstatEntry = PreloadManager.Cache.GetScene(NStatEntry.ScenePath).Instantiate<NStatEntry>(0L);
			nstatEntry._imgUrl = imgUrl;
			return nstatEntry;
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000E2938 File Offset: 0x000E0B38
		public void SetTopText(string text)
		{
			this._topLabel.Visible = true;
			this._topLabel.SetTextAutoSize(text);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000E2952 File Offset: 0x000E0B52
		public void SetBottomText(string text)
		{
			this._bottomLabel.Visible = true;
			this._bottomLabel.SetTextAutoSize(text);
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x000E296C File Offset: 0x000E0B6C
		public override void _Ready()
		{
			base.SetPivotOffset(new Vector2(50f, base.Size.Y * 0.5f));
			this._icon = base.GetNode<TextureRect>("%Icon");
			if (this._imgUrl != null)
			{
				this._icon.Texture = PreloadManager.Cache.GetTexture2D(this._imgUrl);
			}
			this._topLabel = base.GetNode<MegaRichTextLabel>("%TopLabel");
			this._bottomLabel = base.GetNode<MegaRichTextLabel>("%BottomLabel");
			this._controllerFocusReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this.ConnectSignals();
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x000E2A1B File Offset: 0x000E0C1B
		public void SetHoverTip(HoverTip hoverTip)
		{
			this._hoverTip = new HoverTip?(hoverTip);
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x000E2A2C File Offset: 0x000E0C2C
		protected override void OnFocus()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 1.05f, 0.05);
			if (this._hoverTip != null)
			{
				if (base.GlobalPosition.X < base.GetViewport().GetVisibleRect().Size.X * 0.4f)
				{
					NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
					if (nhoverTipSet != null)
					{
						nhoverTipSet.SetGlobalPosition(new Vector2(base.GlobalPosition.X - 392f, base.GlobalPosition.Y), false);
					}
				}
				else
				{
					NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
					if (nhoverTipSet != null)
					{
						nhoverTipSet.SetGlobalPosition(new Vector2(base.GlobalPosition.X + 532f, base.GlobalPosition.Y), false);
					}
				}
			}
			if (NControllerManager.Instance.IsUsingController)
			{
				this._controllerFocusReticle.OnSelect();
			}
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x000E2B60 File Offset: 0x000E0D60
		protected override void OnUnfocus()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			if (this._hoverTip != null)
			{
				NHoverTipSet.Remove(this);
			}
			this._controllerFocusReticle.OnDeselect();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002158 RID: 8536 RVA: 0x000E2BE8 File Offset: 0x000E0DE8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NStatEntry.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "imgUrl", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NStatEntry.MethodName.SetTopText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NStatEntry.MethodName.SetBottomText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NStatEntry.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStatEntry.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStatEntry.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002159 RID: 8537 RVA: 0x000E2D94 File Offset: 0x000E0F94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NStatEntry.MethodName.Create && args.Count == 1)
			{
				NStatEntry nstatEntry = NStatEntry.Create(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<NStatEntry>(ref nstatEntry);
				return true;
			}
			if ((ref method) == NStatEntry.MethodName.SetTopText && args.Count == 1)
			{
				this.SetTopText(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStatEntry.MethodName.SetBottomText && args.Count == 1)
			{
				this.SetBottomText(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStatEntry.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStatEntry.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStatEntry.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000E2EB8 File Offset: 0x000E10B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NStatEntry.MethodName.Create && args.Count == 1)
			{
				NStatEntry nstatEntry = NStatEntry.Create(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<NStatEntry>(ref nstatEntry);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600215B RID: 8539 RVA: 0x000E2F08 File Offset: 0x000E1108
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NStatEntry.MethodName.Create || (ref method) == NStatEntry.MethodName.SetTopText || (ref method) == NStatEntry.MethodName.SetBottomText || (ref method) == NStatEntry.MethodName._Ready || (ref method) == NStatEntry.MethodName.OnFocus || (ref method) == NStatEntry.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600215C RID: 8540 RVA: 0x000E2F78 File Offset: 0x000E1178
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NStatEntry.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NStatEntry.PropertyName._imgUrl)
			{
				this._imgUrl = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NStatEntry.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NStatEntry.PropertyName._topLabel)
			{
				this._topLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NStatEntry.PropertyName._bottomLabel)
			{
				this._bottomLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NStatEntry.PropertyName._controllerFocusReticle)
			{
				this._controllerFocusReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600215D RID: 8541 RVA: 0x000E3030 File Offset: 0x000E1230
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NStatEntry.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NStatEntry.PropertyName._imgUrl)
			{
				value = VariantUtils.CreateFrom<string>(ref this._imgUrl);
				return true;
			}
			if ((ref name) == NStatEntry.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NStatEntry.PropertyName._topLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._topLabel);
				return true;
			}
			if ((ref name) == NStatEntry.PropertyName._bottomLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._bottomLabel);
				return true;
			}
			if ((ref name) == NStatEntry.PropertyName._controllerFocusReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._controllerFocusReticle);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600215E RID: 8542 RVA: 0x000E3108 File Offset: 0x000E1308
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NStatEntry.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(4L, NStatEntry.PropertyName._imgUrl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatEntry.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatEntry.PropertyName._topLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatEntry.PropertyName._bottomLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NStatEntry.PropertyName._controllerFocusReticle, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600215F RID: 8543 RVA: 0x000E31E4 File Offset: 0x000E13E4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NStatEntry.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NStatEntry.PropertyName._imgUrl, Variant.From<string>(ref this._imgUrl));
			info.AddProperty(NStatEntry.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NStatEntry.PropertyName._topLabel, Variant.From<MegaRichTextLabel>(ref this._topLabel));
			info.AddProperty(NStatEntry.PropertyName._bottomLabel, Variant.From<MegaRichTextLabel>(ref this._bottomLabel));
			info.AddProperty(NStatEntry.PropertyName._controllerFocusReticle, Variant.From<NSelectionReticle>(ref this._controllerFocusReticle));
		}

		/// <inheritdoc />
		// Token: 0x06002160 RID: 8544 RVA: 0x000E327C File Offset: 0x000E147C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NStatEntry.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NStatEntry.PropertyName._imgUrl, ref variant2))
			{
				this._imgUrl = variant2.As<string>();
			}
			Variant variant3;
			if (info.TryGetProperty(NStatEntry.PropertyName._icon, ref variant3))
			{
				this._icon = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NStatEntry.PropertyName._topLabel, ref variant4))
			{
				this._topLabel = variant4.As<MegaRichTextLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NStatEntry.PropertyName._bottomLabel, ref variant5))
			{
				this._bottomLabel = variant5.As<MegaRichTextLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NStatEntry.PropertyName._controllerFocusReticle, ref variant6))
			{
				this._controllerFocusReticle = variant6.As<NSelectionReticle>();
			}
		}

		// Token: 0x04000D08 RID: 3336
		private HoverTip? _hoverTip;

		// Token: 0x04000D09 RID: 3337
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000D0A RID: 3338
		[Nullable(2)]
		private string _imgUrl;

		// Token: 0x04000D0B RID: 3339
		private TextureRect _icon;

		// Token: 0x04000D0C RID: 3340
		private MegaRichTextLabel _topLabel;

		// Token: 0x04000D0D RID: 3341
		private MegaRichTextLabel _bottomLabel;

		// Token: 0x04000D0E RID: 3342
		private NSelectionReticle _controllerFocusReticle;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001307 RID: 4871
		[NullableContext(0)]
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004A02 RID: 18946
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'SetTopText' method.
			/// </summary>
			// Token: 0x04004A03 RID: 18947
			public static readonly StringName SetTopText = "SetTopText";

			/// <summary>
			/// Cached name for the 'SetBottomText' method.
			/// </summary>
			// Token: 0x04004A04 RID: 18948
			public static readonly StringName SetBottomText = "SetBottomText";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004A05 RID: 18949
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004A06 RID: 18950
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004A07 RID: 18951
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001308 RID: 4872
		[NullableContext(0)]
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004A08 RID: 18952
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_imgUrl' field.
			/// </summary>
			// Token: 0x04004A09 RID: 18953
			public static readonly StringName _imgUrl = "_imgUrl";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004A0A RID: 18954
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_topLabel' field.
			/// </summary>
			// Token: 0x04004A0B RID: 18955
			public static readonly StringName _topLabel = "_topLabel";

			/// <summary>
			/// Cached name for the '_bottomLabel' field.
			/// </summary>
			// Token: 0x04004A0C RID: 18956
			public static readonly StringName _bottomLabel = "_bottomLabel";

			/// <summary>
			/// Cached name for the '_controllerFocusReticle' field.
			/// </summary>
			// Token: 0x04004A0D RID: 18957
			public static readonly StringName _controllerFocusReticle = "_controllerFocusReticle";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001309 RID: 4873
		[NullableContext(0)]
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
