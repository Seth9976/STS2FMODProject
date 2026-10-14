using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// A custom scrollbar component made especially for NDropdownContainer
	/// </summary>
	// Token: 0x020003DC RID: 988
	[ScriptPath("res://src/Core/Nodes/CommonUi/NDropdownScrollbar.cs")]
	public class NDropdownScrollbar : NButton
	{
		// Token: 0x06003C02 RID: 15362 RVA: 0x001CB578 File Offset: 0x001C9778
		public override void _Ready()
		{
			this.ConnectSignals();
			this._dropdownContainer = base.GetParent<NDropdownContainer>();
			this._train = base.GetNode<Control>("Train");
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnShow)), 0U);
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x001CB5CB File Offset: 0x001C97CB
		public void RefreshTrainBounds()
		{
			this._scrollLimitTop = 600f - this._train.Size.Y - 9f;
			this._scrollLimitBottom = 9f;
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x001CB5FA File Offset: 0x001C97FA
		protected override void OnFocus()
		{
			this._train.Modulate = StsColors.gold;
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x001CB60C File Offset: 0x001C980C
		protected override void OnUnfocus()
		{
			if (this.hasControl)
			{
				return;
			}
			this._train.Modulate = StsColors.quarterTransparentWhite;
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x001CB627 File Offset: 0x001C9827
		private void OnShow()
		{
			this._train.Modulate = StsColors.quarterTransparentWhite;
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x001CB639 File Offset: 0x001C9839
		protected override void OnPress()
		{
			this.hasControl = true;
			this._train.Modulate = StsColors.gold;
			Input.MouseMode = 1L;
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x001CB65C File Offset: 0x001C985C
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			base._Input(inputEvent);
			if (this.hasControl)
			{
				InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
				if (inputEventMouseButton != null && inputEventMouseButton.ButtonIndex == 1L && inputEventMouseButton.IsReleased())
				{
					this.hasControl = false;
					Input.MouseMode = 0L;
					this._train.Modulate = StsColors.quarterTransparentWhite;
				}
			}
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x001CB6B4 File Offset: 0x001C98B4
		[NullableContext(1)]
		public override void _GuiInput(InputEvent inputEvent)
		{
			base._GuiInput(inputEvent);
			if (this.hasControl)
			{
				InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
				if (inputEventMouseMotion != null)
				{
					this._train.Position += new Vector2(0f, inputEventMouseMotion.Relative.Y);
					this.ClampTrain();
					this._dropdownContainer.UpdatePositionBasedOnTrain(1f - (this._train.Position.Y - this._scrollLimitBottom) / (this._scrollLimitTop - this._scrollLimitBottom));
				}
			}
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x001CB744 File Offset: 0x001C9944
		private void ClampTrain()
		{
			this._train.Position = new Vector2(this._train.Position.X, Mathf.Clamp(this._train.Position.Y, this._scrollLimitBottom, this._scrollLimitTop));
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x001CB792 File Offset: 0x001C9992
		public void SetTrainPositionFromPercentage(float percentage)
		{
			this._train.Position = new Vector2(this._train.Position.X, this._scrollLimitBottom + percentage * (this._scrollLimitTop - this._scrollLimitBottom));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C0C RID: 15372 RVA: 0x001CB7CC File Offset: 0x001C99CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NDropdownScrollbar.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownScrollbar.MethodName.RefreshTrainBounds, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownScrollbar.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownScrollbar.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownScrollbar.MethodName.OnShow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownScrollbar.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownScrollbar.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NDropdownScrollbar.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NDropdownScrollbar.MethodName.ClampTrain, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDropdownScrollbar.MethodName.SetTrainPositionFromPercentage, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "percentage", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C0D RID: 15373 RVA: 0x001CBA40 File Offset: 0x001C9C40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDropdownScrollbar.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownScrollbar.MethodName.RefreshTrainBounds && args.Count == 0)
			{
				this.RefreshTrainBounds();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownScrollbar.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownScrollbar.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownScrollbar.MethodName.OnShow && args.Count == 0)
			{
				this.OnShow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownScrollbar.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownScrollbar.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownScrollbar.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownScrollbar.MethodName.ClampTrain && args.Count == 0)
			{
				this.ClampTrain();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDropdownScrollbar.MethodName.SetTrainPositionFromPercentage && args.Count == 1)
			{
				this.SetTrainPositionFromPercentage(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003C0E RID: 15374 RVA: 0x001CBBF4 File Offset: 0x001C9DF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDropdownScrollbar.MethodName._Ready || (ref method) == NDropdownScrollbar.MethodName.RefreshTrainBounds || (ref method) == NDropdownScrollbar.MethodName.OnFocus || (ref method) == NDropdownScrollbar.MethodName.OnUnfocus || (ref method) == NDropdownScrollbar.MethodName.OnShow || (ref method) == NDropdownScrollbar.MethodName.OnPress || (ref method) == NDropdownScrollbar.MethodName._Input || (ref method) == NDropdownScrollbar.MethodName._GuiInput || (ref method) == NDropdownScrollbar.MethodName.ClampTrain || (ref method) == NDropdownScrollbar.MethodName.SetTrainPositionFromPercentage || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003C0F RID: 15375 RVA: 0x001CBCA0 File Offset: 0x001C9EA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDropdownScrollbar.PropertyName._dropdownContainer)
			{
				this._dropdownContainer = VariantUtils.ConvertTo<NDropdownContainer>(ref value);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName._train)
			{
				this._train = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName.hasControl)
			{
				this.hasControl = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName._startDragPos)
			{
				this._startDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName._targetDragPos)
			{
				this._targetDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName._scrollLimitTop)
			{
				this._scrollLimitTop = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName._scrollLimitBottom)
			{
				this._scrollLimitBottom = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003C10 RID: 15376 RVA: 0x001CBD74 File Offset: 0x001C9F74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDropdownScrollbar.PropertyName._dropdownContainer)
			{
				value = VariantUtils.CreateFrom<NDropdownContainer>(ref this._dropdownContainer);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName._train)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._train);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName.hasControl)
			{
				value = VariantUtils.CreateFrom<bool>(ref this.hasControl);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName._startDragPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._startDragPos);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName._targetDragPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetDragPos);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName._scrollLimitTop)
			{
				value = VariantUtils.CreateFrom<float>(ref this._scrollLimitTop);
				return true;
			}
			if ((ref name) == NDropdownScrollbar.PropertyName._scrollLimitBottom)
			{
				value = VariantUtils.CreateFrom<float>(ref this._scrollLimitBottom);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C11 RID: 15377 RVA: 0x001CBE6C File Offset: 0x001CA06C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDropdownScrollbar.PropertyName._dropdownContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDropdownScrollbar.PropertyName._train, 0L, "", 4096L, false),
				new PropertyInfo(1L, NDropdownScrollbar.PropertyName.hasControl, 0L, "", 4096L, false),
				new PropertyInfo(5L, NDropdownScrollbar.PropertyName._startDragPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NDropdownScrollbar.PropertyName._targetDragPos, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDropdownScrollbar.PropertyName._scrollLimitTop, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDropdownScrollbar.PropertyName._scrollLimitBottom, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C12 RID: 15378 RVA: 0x001CBF64 File Offset: 0x001CA164
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDropdownScrollbar.PropertyName._dropdownContainer, Variant.From<NDropdownContainer>(ref this._dropdownContainer));
			info.AddProperty(NDropdownScrollbar.PropertyName._train, Variant.From<Control>(ref this._train));
			info.AddProperty(NDropdownScrollbar.PropertyName.hasControl, Variant.From<bool>(ref this.hasControl));
			info.AddProperty(NDropdownScrollbar.PropertyName._startDragPos, Variant.From<Vector2>(ref this._startDragPos));
			info.AddProperty(NDropdownScrollbar.PropertyName._targetDragPos, Variant.From<Vector2>(ref this._targetDragPos));
			info.AddProperty(NDropdownScrollbar.PropertyName._scrollLimitTop, Variant.From<float>(ref this._scrollLimitTop));
			info.AddProperty(NDropdownScrollbar.PropertyName._scrollLimitBottom, Variant.From<float>(ref this._scrollLimitBottom));
		}

		/// <inheritdoc />
		// Token: 0x06003C13 RID: 15379 RVA: 0x001CC014 File Offset: 0x001CA214
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDropdownScrollbar.PropertyName._dropdownContainer, ref variant))
			{
				this._dropdownContainer = variant.As<NDropdownContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDropdownScrollbar.PropertyName._train, ref variant2))
			{
				this._train = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDropdownScrollbar.PropertyName.hasControl, ref variant3))
			{
				this.hasControl = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDropdownScrollbar.PropertyName._startDragPos, ref variant4))
			{
				this._startDragPos = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDropdownScrollbar.PropertyName._targetDragPos, ref variant5))
			{
				this._targetDragPos = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDropdownScrollbar.PropertyName._scrollLimitTop, ref variant6))
			{
				this._scrollLimitTop = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDropdownScrollbar.PropertyName._scrollLimitBottom, ref variant7))
			{
				this._scrollLimitBottom = variant7.As<float>();
			}
		}

		// Token: 0x04001773 RID: 6003
		[Nullable(1)]
		private NDropdownContainer _dropdownContainer;

		// Token: 0x04001774 RID: 6004
		[Nullable(1)]
		private Control _train;

		// Token: 0x04001775 RID: 6005
		public bool hasControl;

		// Token: 0x04001776 RID: 6006
		private Vector2 _startDragPos;

		// Token: 0x04001777 RID: 6007
		private Vector2 _targetDragPos;

		// Token: 0x04001778 RID: 6008
		private float _scrollLimitTop;

		// Token: 0x04001779 RID: 6009
		private float _scrollLimitBottom;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001843 RID: 6211
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005EFC RID: 24316
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshTrainBounds' method.
			/// </summary>
			// Token: 0x04005EFD RID: 24317
			public static readonly StringName RefreshTrainBounds = "RefreshTrainBounds";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005EFE RID: 24318
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005EFF RID: 24319
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnShow' method.
			/// </summary>
			// Token: 0x04005F00 RID: 24320
			public static readonly StringName OnShow = "OnShow";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005F01 RID: 24321
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005F02 RID: 24322
			public new static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04005F03 RID: 24323
			public new static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'ClampTrain' method.
			/// </summary>
			// Token: 0x04005F04 RID: 24324
			public static readonly StringName ClampTrain = "ClampTrain";

			/// <summary>
			/// Cached name for the 'SetTrainPositionFromPercentage' method.
			/// </summary>
			// Token: 0x04005F05 RID: 24325
			public static readonly StringName SetTrainPositionFromPercentage = "SetTrainPositionFromPercentage";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001844 RID: 6212
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_dropdownContainer' field.
			/// </summary>
			// Token: 0x04005F06 RID: 24326
			public static readonly StringName _dropdownContainer = "_dropdownContainer";

			/// <summary>
			/// Cached name for the '_train' field.
			/// </summary>
			// Token: 0x04005F07 RID: 24327
			public static readonly StringName _train = "_train";

			/// <summary>
			/// Cached name for the 'hasControl' field.
			/// </summary>
			// Token: 0x04005F08 RID: 24328
			public static readonly StringName hasControl = "hasControl";

			/// <summary>
			/// Cached name for the '_startDragPos' field.
			/// </summary>
			// Token: 0x04005F09 RID: 24329
			public static readonly StringName _startDragPos = "_startDragPos";

			/// <summary>
			/// Cached name for the '_targetDragPos' field.
			/// </summary>
			// Token: 0x04005F0A RID: 24330
			public static readonly StringName _targetDragPos = "_targetDragPos";

			/// <summary>
			/// Cached name for the '_scrollLimitTop' field.
			/// </summary>
			// Token: 0x04005F0B RID: 24331
			public static readonly StringName _scrollLimitTop = "_scrollLimitTop";

			/// <summary>
			/// Cached name for the '_scrollLimitBottom' field.
			/// </summary>
			// Token: 0x04005F0C RID: 24332
			public static readonly StringName _scrollLimitBottom = "_scrollLimitBottom";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001845 RID: 6213
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
