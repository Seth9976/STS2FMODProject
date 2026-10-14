using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	/// <summary>
	/// Base class for Paginator controls for Max FPS and Combat Speed in the settings screen.
	/// Use a dropdown if there are a lot of settings.
	/// TODO: Does this need to override NButton for controller support?
	/// </summary>
	// Token: 0x020002A8 RID: 680
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NPaginator.cs")]
	public class NPaginator : Control
	{
		// Token: 0x06002407 RID: 9223 RVA: 0x000F4C19 File Offset: 0x000F2E19
		public override void _Ready()
		{
			if (base.GetType() != typeof(NPaginator))
			{
				throw new InvalidOperationException("Don't call base._Ready(). Use ConnectSignals() instead");
			}
			this.ConnectSignals();
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000F4C44 File Offset: 0x000F2E44
		protected void ConnectSignals()
		{
			this._label = base.GetNode<MegaLabel>("%Label");
			this._vfxLabel = base.GetNode<MegaLabel>("%VfxLabel");
			this._selectionReticle = base.GetNode<NSelectionReticle>("SelectionReticle");
			base.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
			base.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000F4CCF File Offset: 0x000F2ECF
		[NullableContext(1)]
		public override void _GuiInput(InputEvent input)
		{
			base._GuiInput(input);
			if (input.IsActionPressed(MegaInput.left, false, false))
			{
				this.PageLeft();
			}
			if (input.IsActionPressed(MegaInput.right, false, false))
			{
				this.PageRight();
			}
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000F4D02 File Offset: 0x000F2F02
		protected virtual void OnIndexChanged(int index)
		{
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x000F4D04 File Offset: 0x000F2F04
		public void SetIndex(int index)
		{
			if (this._currentIndex != index)
			{
				this._currentIndex = Mathf.Clamp(index, 0, this._options.Count - 1);
				this.OnIndexChanged(this._currentIndex);
			}
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x000F4D35 File Offset: 0x000F2F35
		public void PageLeft()
		{
			this._currentIndex--;
			if (this._currentIndex < 0)
			{
				this._currentIndex = this._options.Count - 1;
			}
			this.IndexChangeHelper(true);
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x000F4D68 File Offset: 0x000F2F68
		public void PageRight()
		{
			this._currentIndex++;
			if (this._currentIndex > this._options.Count - 1)
			{
				this._currentIndex = 0;
			}
			this.IndexChangeHelper(false);
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x000F4D9C File Offset: 0x000F2F9C
		private void IndexChangeHelper(bool pagedLeft)
		{
			this._vfxLabel.SetTextAutoSize(this._label.Text);
			this._vfxLabel.Modulate = this._label.Modulate;
			this.OnIndexChanged(this._currentIndex);
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._label, "position:x", 0f, 0.25).SetEase(1L).SetTrans(7L)
				.From(pagedLeft ? (-90f) : 90f);
			this._tween.TweenProperty(this._label, "modulate:a", 1f, 0.25).From(0.75f);
			this._tween.TweenProperty(this._vfxLabel, "position:x", pagedLeft ? 90f : (-90f), 0.25).SetEase(1L).SetTrans(7L)
				.From(0f);
			this._tween.TweenProperty(this._vfxLabel, "modulate", StsColors.transparentBlack, 0.25);
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x000F4F1D File Offset: 0x000F311D
		private void OnFocus()
		{
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x000F4F36 File Offset: 0x000F3136
		private void OnUnfocus()
		{
			this._selectionReticle.OnDeselect();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002411 RID: 9233 RVA: 0x000F4F44 File Offset: 0x000F3144
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NPaginator.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPaginator.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPaginator.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "input", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NPaginator.MethodName.OnIndexChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPaginator.MethodName.SetIndex, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPaginator.MethodName.PageLeft, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPaginator.MethodName.PageRight, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPaginator.MethodName.IndexChangeHelper, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "pagedLeft", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPaginator.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPaginator.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002412 RID: 9234 RVA: 0x000F51D0 File Offset: 0x000F33D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPaginator.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginator.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginator.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginator.MethodName.OnIndexChanged && args.Count == 1)
			{
				this.OnIndexChanged(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginator.MethodName.SetIndex && args.Count == 1)
			{
				this.SetIndex(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginator.MethodName.PageLeft && args.Count == 0)
			{
				this.PageLeft();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginator.MethodName.PageRight && args.Count == 0)
			{
				this.PageRight();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginator.MethodName.IndexChangeHelper && args.Count == 1)
			{
				this.IndexChangeHelper(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginator.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPaginator.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002413 RID: 9235 RVA: 0x000F5390 File Offset: 0x000F3590
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPaginator.MethodName._Ready || (ref method) == NPaginator.MethodName.ConnectSignals || (ref method) == NPaginator.MethodName._GuiInput || (ref method) == NPaginator.MethodName.OnIndexChanged || (ref method) == NPaginator.MethodName.SetIndex || (ref method) == NPaginator.MethodName.PageLeft || (ref method) == NPaginator.MethodName.PageRight || (ref method) == NPaginator.MethodName.IndexChangeHelper || (ref method) == NPaginator.MethodName.OnFocus || (ref method) == NPaginator.MethodName.OnUnfocus || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002414 RID: 9236 RVA: 0x000F543C File Offset: 0x000F363C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPaginator.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPaginator.PropertyName._vfxLabel)
			{
				this._vfxLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPaginator.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NPaginator.PropertyName._currentIndex)
			{
				this._currentIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NPaginator.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002415 RID: 9237 RVA: 0x000F54D8 File Offset: 0x000F36D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPaginator.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NPaginator.PropertyName._vfxLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._vfxLabel);
				return true;
			}
			if ((ref name) == NPaginator.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NPaginator.PropertyName._currentIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentIndex);
				return true;
			}
			if ((ref name) == NPaginator.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002416 RID: 9238 RVA: 0x000F5590 File Offset: 0x000F3790
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPaginator.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPaginator.PropertyName._vfxLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPaginator.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(2L, NPaginator.PropertyName._currentIndex, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPaginator.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002417 RID: 9239 RVA: 0x000F5648 File Offset: 0x000F3848
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPaginator.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NPaginator.PropertyName._vfxLabel, Variant.From<MegaLabel>(ref this._vfxLabel));
			info.AddProperty(NPaginator.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NPaginator.PropertyName._currentIndex, Variant.From<int>(ref this._currentIndex));
			info.AddProperty(NPaginator.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06002418 RID: 9240 RVA: 0x000F56CC File Offset: 0x000F38CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPaginator.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPaginator.PropertyName._vfxLabel, ref variant2))
			{
				this._vfxLabel = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPaginator.PropertyName._selectionReticle, ref variant3))
			{
				this._selectionReticle = variant3.As<NSelectionReticle>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPaginator.PropertyName._currentIndex, ref variant4))
			{
				this._currentIndex = variant4.As<int>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPaginator.PropertyName._tween, ref variant5))
			{
				this._tween = variant5.As<Tween>();
			}
		}

		// Token: 0x04000DA4 RID: 3492
		[Nullable(1)]
		protected MegaLabel _label;

		// Token: 0x04000DA5 RID: 3493
		[Nullable(1)]
		private MegaLabel _vfxLabel;

		// Token: 0x04000DA6 RID: 3494
		[Nullable(1)]
		private NSelectionReticle _selectionReticle;

		// Token: 0x04000DA7 RID: 3495
		[Nullable(1)]
		protected readonly List<string> _options = new List<string>();

		// Token: 0x04000DA8 RID: 3496
		protected int _currentIndex;

		// Token: 0x04000DA9 RID: 3497
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000DAA RID: 3498
		private const double _animDuration = 0.25;

		// Token: 0x04000DAB RID: 3499
		private const float _animDistance = 90f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013AE RID: 5038
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B8D RID: 19341
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04004B8E RID: 19342
			public static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04004B8F RID: 19343
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'OnIndexChanged' method.
			/// </summary>
			// Token: 0x04004B90 RID: 19344
			public static readonly StringName OnIndexChanged = "OnIndexChanged";

			/// <summary>
			/// Cached name for the 'SetIndex' method.
			/// </summary>
			// Token: 0x04004B91 RID: 19345
			public static readonly StringName SetIndex = "SetIndex";

			/// <summary>
			/// Cached name for the 'PageLeft' method.
			/// </summary>
			// Token: 0x04004B92 RID: 19346
			public static readonly StringName PageLeft = "PageLeft";

			/// <summary>
			/// Cached name for the 'PageRight' method.
			/// </summary>
			// Token: 0x04004B93 RID: 19347
			public static readonly StringName PageRight = "PageRight";

			/// <summary>
			/// Cached name for the 'IndexChangeHelper' method.
			/// </summary>
			// Token: 0x04004B94 RID: 19348
			public static readonly StringName IndexChangeHelper = "IndexChangeHelper";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004B95 RID: 19349
			public static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004B96 RID: 19350
			public static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013AF RID: 5039
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004B97 RID: 19351
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_vfxLabel' field.
			/// </summary>
			// Token: 0x04004B98 RID: 19352
			public static readonly StringName _vfxLabel = "_vfxLabel";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04004B99 RID: 19353
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_currentIndex' field.
			/// </summary>
			// Token: 0x04004B9A RID: 19354
			public static readonly StringName _currentIndex = "_currentIndex";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004B9B RID: 19355
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013B0 RID: 5040
		public class SignalName : Control.SignalName
		{
		}
	}
}
