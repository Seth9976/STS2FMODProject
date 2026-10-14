using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Potions
{
	// Token: 0x02000382 RID: 898
	[ScriptPath("res://src/Core/Nodes/Potions/NPotionPopupButton.cs")]
	public class NPotionPopupButton : NButton
	{
		// Token: 0x0600357A RID: 13690 RVA: 0x0018F444 File Offset: 0x0018D644
		public override void _Ready()
		{
			this.ConnectSignals();
			this._background = base.GetNode<TextureRect>("Background");
			this._label = base.GetNode<MegaLabel>("Label");
			this._background.Modulate = Colors.Transparent;
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x0018F493 File Offset: 0x0018D693
		[NullableContext(1)]
		public void SetLocKey(string locEntryKey)
		{
			this._label.SetTextAutoSize(new LocString("gameplay_ui", locEntryKey).GetFormattedText());
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x0018F4B0 File Offset: 0x0018D6B0
		protected override void OnFocus()
		{
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween();
			this._currentTween.TweenProperty(this._background, "modulate:a", 0.25f, 0.15000000596046448);
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x0018F50C File Offset: 0x0018D70C
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this);
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween();
			this._currentTween.TweenProperty(this._background, "modulate:a", 0f, 0.15000000596046448);
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x0018F56B File Offset: 0x0018D76B
		protected override void OnDisable()
		{
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._background.Modulate = new Color(0.1f, 0.1f, 0.1f, 0.75f);
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x0018F5A2 File Offset: 0x0018D7A2
		protected override void OnEnable()
		{
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._background.Modulate = new Color(1f, 1f, 1f, 0f);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003580 RID: 13696 RVA: 0x0018F5DC File Offset: 0x0018D7DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NPotionPopupButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionPopupButton.MethodName.SetLocKey, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "locEntryKey", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPotionPopupButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionPopupButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionPopupButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionPopupButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003581 RID: 13697 RVA: 0x0018F730 File Offset: 0x0018D930
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionPopupButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopupButton.MethodName.SetLocKey && args.Count == 1)
			{
				this.SetLocKey(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopupButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopupButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopupButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionPopupButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003582 RID: 13698 RVA: 0x0018F834 File Offset: 0x0018DA34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPotionPopupButton.MethodName._Ready || (ref method) == NPotionPopupButton.MethodName.SetLocKey || (ref method) == NPotionPopupButton.MethodName.OnFocus || (ref method) == NPotionPopupButton.MethodName.OnUnfocus || (ref method) == NPotionPopupButton.MethodName.OnDisable || (ref method) == NPotionPopupButton.MethodName.OnEnable || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003583 RID: 13699 RVA: 0x0018F8A4 File Offset: 0x0018DAA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPotionPopupButton.PropertyName._background)
			{
				this._background = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPotionPopupButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPotionPopupButton.PropertyName._currentTween)
			{
				this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003584 RID: 13700 RVA: 0x0018F90C File Offset: 0x0018DB0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPotionPopupButton.PropertyName._background)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._background);
				return true;
			}
			if ((ref name) == NPotionPopupButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NPotionPopupButton.PropertyName._currentTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003585 RID: 13701 RVA: 0x0018F984 File Offset: 0x0018DB84
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPotionPopupButton.PropertyName._background, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionPopupButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionPopupButton.PropertyName._currentTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003586 RID: 13702 RVA: 0x0018F9FC File Offset: 0x0018DBFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPotionPopupButton.PropertyName._background, Variant.From<TextureRect>(ref this._background));
			info.AddProperty(NPotionPopupButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NPotionPopupButton.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
		}

		/// <inheritdoc />
		// Token: 0x06003587 RID: 13703 RVA: 0x0018FA54 File Offset: 0x0018DC54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPotionPopupButton.PropertyName._background, ref variant))
			{
				this._background = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPotionPopupButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPotionPopupButton.PropertyName._currentTween, ref variant3))
			{
				this._currentTween = variant3.As<Tween>();
			}
		}

		// Token: 0x040014BA RID: 5306
		[Nullable(1)]
		private TextureRect _background;

		// Token: 0x040014BB RID: 5307
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x040014BC RID: 5308
		[Nullable(2)]
		private Tween _currentTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001700 RID: 5888
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040059B3 RID: 22963
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetLocKey' method.
			/// </summary>
			// Token: 0x040059B4 RID: 22964
			public static readonly StringName SetLocKey = "SetLocKey";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040059B5 RID: 22965
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040059B6 RID: 22966
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x040059B7 RID: 22967
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x040059B8 RID: 22968
			public new static readonly StringName OnEnable = "OnEnable";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001701 RID: 5889
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_background' field.
			/// </summary>
			// Token: 0x040059B9 RID: 22969
			public static readonly StringName _background = "_background";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x040059BA RID: 22970
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_currentTween' field.
			/// </summary>
			// Token: 0x040059BB RID: 22971
			public static readonly StringName _currentTween = "_currentTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001702 RID: 5890
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
