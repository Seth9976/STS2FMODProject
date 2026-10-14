using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary
{
	// Token: 0x02000356 RID: 854
	[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardRarityTickbox.cs")]
	public class NCardRarityTickbox : NTickbox
	{
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x0600315A RID: 12634 RVA: 0x0016EA4F File Offset: 0x0016CC4F
		// (set) Token: 0x0600315B RID: 12635 RVA: 0x0016EA57 File Offset: 0x0016CC57
		[Nullable(1)]
		public LocString Loc
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x0016EA60 File Offset: 0x0016CC60
		public override void _Ready()
		{
			this.ConnectSignals();
			this._label = base.GetNode<MegaLabel>("Label");
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x0016EA80 File Offset: 0x0016CC80
		protected override void OnRelease()
		{
			base.OnRelease();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One * 1.1f, 0.25).SetEase(1L).SetTrans(9L);
			this._labelTween.TweenProperty(this._label, "self_modulate", Colors.White, 0.05);
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x0016EB30 File Offset: 0x0016CD30
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One * 1.1f, 0.05);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, new HoverTip(this.Loc, null), HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(new Vector2(310f, base.GlobalPosition.Y), false);
			}
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x0016EBD8 File Offset: 0x0016CDD8
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x0016EC54 File Offset: 0x0016CE54
		protected override void OnPress()
		{
			base.OnPress();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._labelTween.TweenProperty(this._label, "self_modulate", StsColors.lightGray, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x0016ED04 File Offset: 0x0016CF04
		protected override void OnDisable()
		{
			base.Modulate = StsColors.gray;
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x0016ED11 File Offset: 0x0016CF11
		protected override void OnEnable()
		{
			base.Modulate = Colors.White;
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x0016ED1E File Offset: 0x0016CF1E
		[NullableContext(1)]
		public void SetLabel(string text)
		{
			this._label.SetTextAutoSize(text);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003164 RID: 12644 RVA: 0x0016ED2C File Offset: 0x0016CF2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NCardRarityTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRarityTickbox.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRarityTickbox.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRarityTickbox.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRarityTickbox.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRarityTickbox.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRarityTickbox.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRarityTickbox.MethodName.SetLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003165 RID: 12645 RVA: 0x0016EEE0 File Offset: 0x0016D0E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardRarityTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRarityTickbox.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRarityTickbox.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRarityTickbox.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRarityTickbox.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRarityTickbox.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRarityTickbox.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRarityTickbox.MethodName.SetLabel && args.Count == 1)
			{
				this.SetLabel(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003166 RID: 12646 RVA: 0x0016F02C File Offset: 0x0016D22C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardRarityTickbox.MethodName._Ready || (ref method) == NCardRarityTickbox.MethodName.OnRelease || (ref method) == NCardRarityTickbox.MethodName.OnFocus || (ref method) == NCardRarityTickbox.MethodName.OnUnfocus || (ref method) == NCardRarityTickbox.MethodName.OnPress || (ref method) == NCardRarityTickbox.MethodName.OnDisable || (ref method) == NCardRarityTickbox.MethodName.OnEnable || (ref method) == NCardRarityTickbox.MethodName.SetLabel || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003167 RID: 12647 RVA: 0x0016F0B8 File Offset: 0x0016D2B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardRarityTickbox.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCardRarityTickbox.PropertyName._labelTween)
			{
				this._labelTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003168 RID: 12648 RVA: 0x0016F0F8 File Offset: 0x0016D2F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardRarityTickbox.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NCardRarityTickbox.PropertyName._labelTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._labelTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003169 RID: 12649 RVA: 0x0016F150 File Offset: 0x0016D350
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardRarityTickbox.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardRarityTickbox.PropertyName._labelTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600316A RID: 12650 RVA: 0x0016F1A6 File Offset: 0x0016D3A6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardRarityTickbox.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NCardRarityTickbox.PropertyName._labelTween, Variant.From<Tween>(ref this._labelTween));
		}

		/// <inheritdoc />
		// Token: 0x0600316B RID: 12651 RVA: 0x0016F1DC File Offset: 0x0016D3DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardRarityTickbox.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardRarityTickbox.PropertyName._labelTween, ref variant2))
			{
				this._labelTween = variant2.As<Tween>();
			}
		}

		// Token: 0x04001341 RID: 4929
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x04001342 RID: 4930
		[Nullable(2)]
		private Tween _labelTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200163F RID: 5695
		public new class MethodName : NTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040056A9 RID: 22185
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x040056AA RID: 22186
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040056AB RID: 22187
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040056AC RID: 22188
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x040056AD RID: 22189
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x040056AE RID: 22190
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x040056AF RID: 22191
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'SetLabel' method.
			/// </summary>
			// Token: 0x040056B0 RID: 22192
			public static readonly StringName SetLabel = "SetLabel";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001640 RID: 5696
		public new class PropertyName : NTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x040056B1 RID: 22193
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_labelTween' field.
			/// </summary>
			// Token: 0x040056B2 RID: 22194
			public static readonly StringName _labelTween = "_labelTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001641 RID: 5697
		public new class SignalName : NTickbox.SignalName
		{
		}
	}
}
