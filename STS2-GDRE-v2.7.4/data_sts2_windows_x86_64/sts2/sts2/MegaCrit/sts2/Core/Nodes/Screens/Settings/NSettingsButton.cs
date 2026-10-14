using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002B5 RID: 693
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsButton.cs")]
	public class NSettingsButton : NButton
	{
		// Token: 0x060024A4 RID: 9380 RVA: 0x000F833E File Offset: 0x000F653E
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			base.PivotOffset = base.Size * 0.5f;
			this._selectionReticle = base.GetNode<NSelectionReticle>("SelectionReticle");
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000F8374 File Offset: 0x000F6574
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 1.05f, 0.05);
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000F83F8 File Offset: 0x000F65F8
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
			this._selectionReticle.OnDeselect();
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x000F8474 File Offset: 0x000F6674
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

		// Token: 0x060024A8 RID: 9384 RVA: 0x000F84EC File Offset: 0x000F66EC
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
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060024A9 RID: 9385 RVA: 0x000F854C File Offset: 0x000F674C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NSettingsButton.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060024AA RID: 9386 RVA: 0x000F864C File Offset: 0x000F684C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSettingsButton.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060024AB RID: 9387 RVA: 0x000F871C File Offset: 0x000F691C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSettingsButton.MethodName.ConnectSignals || (ref method) == NSettingsButton.MethodName.OnFocus || (ref method) == NSettingsButton.MethodName.OnUnfocus || (ref method) == NSettingsButton.MethodName.OnPress || (ref method) == NSettingsButton.MethodName.OnRelease || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060024AC RID: 9388 RVA: 0x000F877C File Offset: 0x000F697C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSettingsButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NSettingsButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NSettingsButton.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NSettingsButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060024AD RID: 9389 RVA: 0x000F8800 File Offset: 0x000F6A00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSettingsButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NSettingsButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NSettingsButton.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NSettingsButton.PropertyName._tween)
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
		// Token: 0x060024AE RID: 9390 RVA: 0x000F8898 File Offset: 0x000F6A98
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSettingsButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsButton.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsButton.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060024AF RID: 9391 RVA: 0x000F8930 File Offset: 0x000F6B30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSettingsButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NSettingsButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NSettingsButton.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NSettingsButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x060024B0 RID: 9392 RVA: 0x000F899C File Offset: 0x000F6B9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSettingsButton.PropertyName._image, ref variant))
			{
				this._image = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSettingsButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSettingsButton.PropertyName._selectionReticle, ref variant3))
			{
				this._selectionReticle = variant3.As<NSelectionReticle>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSettingsButton.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
		}

		// Token: 0x04000DBC RID: 3516
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04000DBD RID: 3517
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x04000DBE RID: 3518
		[Nullable(1)]
		private NSelectionReticle _selectionReticle;

		// Token: 0x04000DBF RID: 3519
		[Nullable(2)]
		protected Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013D7 RID: 5079
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04004BD5 RID: 19413
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004BD6 RID: 19414
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004BD7 RID: 19415
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004BD8 RID: 19416
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004BD9 RID: 19417
			public new static readonly StringName OnRelease = "OnRelease";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013D8 RID: 5080
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004BDA RID: 19418
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004BDB RID: 19419
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04004BDC RID: 19420
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004BDD RID: 19421
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013D9 RID: 5081
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
