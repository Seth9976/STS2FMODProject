using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	/// <summary>
	/// Handles the visuals for the Settings tabs in the settings screen.
	/// Click and input events are handled by the SettingsTabManager class.
	/// </summary>
	// Token: 0x020002BB RID: 699
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsTab.cs")]
	public class NSettingsTab : NButton
	{
		// Token: 0x0600250C RID: 9484 RVA: 0x000FB30C File Offset: 0x000F950C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._outline = base.GetNode<TextureRect>("Outline");
			this._image = base.GetNode<TextureRect>("TabImage");
			this._label = base.GetNode<MegaLabel>("Label");
			this._hsv = (ShaderMaterial)this._image.Material;
			this._label.Modulate = StsColors.halfTransparentCream;
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x000FB387 File Offset: 0x000F9587
		public void ForceTabPressed()
		{
			base.EmitSignalReleased(this);
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x000FB390 File Offset: 0x000F9590
		public void Select()
		{
			if (this._isSelected)
			{
				return;
			}
			this._isSelected = true;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._label.Modulate = StsColors.cream;
			this._image.Modulate = Colors.White;
			this._outline.Visible = true;
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x000FB3EA File Offset: 0x000F95EA
		public void Deselect()
		{
			if (!this._isSelected)
			{
				return;
			}
			this._isSelected = false;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._outline.Visible = false;
			this._label.Modulate = StsColors.halfTransparentCream;
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x000FB42C File Offset: 0x000F962C
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._label.Modulate = StsColors.gold;
			this._hsv.SetShaderParameter(NSettingsTab._v, 1.2f);
			base.Scale = Vector2.One * 1.05f;
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x000FB490 File Offset: 0x000F9690
		protected override void OnUnfocus()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			Color color = (this._isSelected ? StsColors.cream : StsColors.halfTransparentCream);
			this._tween.TweenProperty(this._label, "modulate", color, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), 1.2f, 0.9f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x000FB58E File Offset: 0x000F978E
		protected override void OnEnable()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			base.Modulate = Colors.White;
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x000FB5AC File Offset: 0x000F97AC
		protected override void OnDisable()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			base.Modulate = Colors.DimGray;
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x000FB5CA File Offset: 0x000F97CA
		private void UpdateShaderParam(float newV)
		{
			this._hsv.SetShaderParameter(NSettingsTab._v, newV);
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x000FB5E2 File Offset: 0x000F97E2
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
		// Token: 0x06002516 RID: 9494 RVA: 0x000FB5F0 File Offset: 0x000F97F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NSettingsTab.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTab.MethodName.ForceTabPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTab.MethodName.Select, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTab.MethodName.Deselect, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTab.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTab.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTab.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTab.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTab.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "newV", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSettingsTab.MethodName.SetLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002517 RID: 9495 RVA: 0x000FB828 File Offset: 0x000F9A28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSettingsTab.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTab.MethodName.ForceTabPressed && args.Count == 0)
			{
				this.ForceTabPressed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTab.MethodName.Select && args.Count == 0)
			{
				this.Select();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTab.MethodName.Deselect && args.Count == 0)
			{
				this.Deselect();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTab.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTab.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTab.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTab.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTab.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTab.MethodName.SetLabel && args.Count == 1)
			{
				this.SetLabel(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002518 RID: 9496 RVA: 0x000FB9CC File Offset: 0x000F9BCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSettingsTab.MethodName._Ready || (ref method) == NSettingsTab.MethodName.ForceTabPressed || (ref method) == NSettingsTab.MethodName.Select || (ref method) == NSettingsTab.MethodName.Deselect || (ref method) == NSettingsTab.MethodName.OnFocus || (ref method) == NSettingsTab.MethodName.OnUnfocus || (ref method) == NSettingsTab.MethodName.OnEnable || (ref method) == NSettingsTab.MethodName.OnDisable || (ref method) == NSettingsTab.MethodName.UpdateShaderParam || (ref method) == NSettingsTab.MethodName.SetLabel || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002519 RID: 9497 RVA: 0x000FBA78 File Offset: 0x000F9C78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSettingsTab.PropertyName._isSelected)
			{
				this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSettingsTab.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NSettingsTab.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NSettingsTab.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NSettingsTab.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NSettingsTab.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600251A RID: 9498 RVA: 0x000FBB30 File Offset: 0x000F9D30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSettingsTab.PropertyName._isSelected)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
				return true;
			}
			if ((ref name) == NSettingsTab.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
				return true;
			}
			if ((ref name) == NSettingsTab.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NSettingsTab.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NSettingsTab.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NSettingsTab.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600251B RID: 9499 RVA: 0x000FBC08 File Offset: 0x000F9E08
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NSettingsTab.PropertyName._isSelected, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTab.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTab.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTab.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTab.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTab.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600251C RID: 9500 RVA: 0x000FBCE4 File Offset: 0x000F9EE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSettingsTab.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
			info.AddProperty(NSettingsTab.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
			info.AddProperty(NSettingsTab.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NSettingsTab.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NSettingsTab.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NSettingsTab.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x0600251D RID: 9501 RVA: 0x000FBD7C File Offset: 0x000F9F7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSettingsTab.PropertyName._isSelected, ref variant))
			{
				this._isSelected = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSettingsTab.PropertyName._outline, ref variant2))
			{
				this._outline = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSettingsTab.PropertyName._image, ref variant3))
			{
				this._image = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSettingsTab.PropertyName._label, ref variant4))
			{
				this._label = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSettingsTab.PropertyName._hsv, ref variant5))
			{
				this._hsv = variant5.As<ShaderMaterial>();
			}
			Variant variant6;
			if (info.TryGetProperty(NSettingsTab.PropertyName._tween, ref variant6))
			{
				this._tween = variant6.As<Tween>();
			}
		}

		// Token: 0x04000DD5 RID: 3541
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		/// <summary>
		/// A special flag as there can be a "currently selected" tab.
		/// Kind of like radio buttons I guess.
		/// </summary>
		// Token: 0x04000DD6 RID: 3542
		private bool _isSelected;

		// Token: 0x04000DD7 RID: 3543
		[Nullable(1)]
		private TextureRect _outline;

		// Token: 0x04000DD8 RID: 3544
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04000DD9 RID: 3545
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x04000DDA RID: 3546
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04000DDB RID: 3547
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000DDC RID: 3548
		private const float _defaultV = 0.9f;

		// Token: 0x04000DDD RID: 3549
		private const float _hoverV = 1.2f;

		// Token: 0x04000DDE RID: 3550
		private const float _unhoverDuration = 0.5f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013EC RID: 5100
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C0F RID: 19471
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ForceTabPressed' method.
			/// </summary>
			// Token: 0x04004C10 RID: 19472
			public static readonly StringName ForceTabPressed = "ForceTabPressed";

			/// <summary>
			/// Cached name for the 'Select' method.
			/// </summary>
			// Token: 0x04004C11 RID: 19473
			public static readonly StringName Select = "Select";

			/// <summary>
			/// Cached name for the 'Deselect' method.
			/// </summary>
			// Token: 0x04004C12 RID: 19474
			public static readonly StringName Deselect = "Deselect";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004C13 RID: 19475
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004C14 RID: 19476
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04004C15 RID: 19477
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04004C16 RID: 19478
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x04004C17 RID: 19479
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";

			/// <summary>
			/// Cached name for the 'SetLabel' method.
			/// </summary>
			// Token: 0x04004C18 RID: 19480
			public static readonly StringName SetLabel = "SetLabel";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013ED RID: 5101
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_isSelected' field.
			/// </summary>
			// Token: 0x04004C19 RID: 19481
			public static readonly StringName _isSelected = "_isSelected";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04004C1A RID: 19482
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004C1B RID: 19483
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004C1C RID: 19484
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004C1D RID: 19485
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004C1E RID: 19486
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013EE RID: 5102
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
