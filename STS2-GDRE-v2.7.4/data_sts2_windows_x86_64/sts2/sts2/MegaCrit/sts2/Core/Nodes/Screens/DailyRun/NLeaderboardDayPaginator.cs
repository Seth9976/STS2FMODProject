using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	/// <summary>
	/// This is pretty close to NPaginator, but NPaginator only supports a static list of options whereas this needs to
	/// - Set its text based on a DateTimeOffset that is (effectively) infinite
	/// - Dynamically enable/disable arrows based on whether the query says there is a leaderboard
	/// It uses the same scene as NPaginator but with the script replaced.
	/// </summary>
	// Token: 0x02000334 RID: 820
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NLeaderboardDayPaginator.cs")]
	public class NLeaderboardDayPaginator : Control
	{
		// Token: 0x06002DF5 RID: 11765 RVA: 0x0014A67C File Offset: 0x0014887C
		public override void _Ready()
		{
			this._label = base.GetNode<MegaLabel>("LabelContainer/Mask/Label");
			this._vfxLabel = base.GetNode<MegaLabel>("LabelContainer/Mask/VfxLabel");
			this._selectionReticle = base.GetNode<NSelectionReticle>("SelectionReticle");
			this._leftArrow = base.GetNode<NLeaderboardPageArrow>("LeftArrow");
			this._rightArrow = base.GetNode<NLeaderboardPageArrow>("RightArrow");
			base.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
			base.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
			this._leftArrow.Connect(new Action(this.PageLeft));
			this._rightArrow.Connect(new Action(this.PageRight));
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x0014A761 File Offset: 0x00148961
		[NullableContext(1)]
		public void Initialize(NDailyRunLeaderboard leaderboard, DateTimeOffset dateTime, bool showArrows)
		{
			this._currentDay = dateTime;
			this._leaderboard = leaderboard;
			this.OnDayChanged(false);
			this._leftArrow.Visible = showArrows;
			this._rightArrow.Visible = showArrows;
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x0014A790 File Offset: 0x00148990
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

		// Token: 0x06002DF8 RID: 11768 RVA: 0x0014A7C4 File Offset: 0x001489C4
		private void PageLeft()
		{
			DateTimeOffset? dateTimeOffset = DailyRunUtility.AddLeaderboardDays(this._currentDay, -1);
			if (dateTimeOffset != null)
			{
				DateTimeOffset valueOrDefault = dateTimeOffset.GetValueOrDefault();
				this._currentDay = valueOrDefault;
				this.DayChangeHelper(true);
				return;
			}
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x0014A800 File Offset: 0x00148A00
		private void PageRight()
		{
			DateTimeOffset? dateTimeOffset = DailyRunUtility.AddLeaderboardDays(this._currentDay, 1);
			if (dateTimeOffset != null)
			{
				DateTimeOffset valueOrDefault = dateTimeOffset.GetValueOrDefault();
				this._currentDay = valueOrDefault;
				this.DayChangeHelper(false);
				return;
			}
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x0014A83C File Offset: 0x00148A3C
		private void DayChangeHelper(bool pagedLeft)
		{
			this._vfxLabel.SetTextAutoSize(this._label.Text);
			this._vfxLabel.Modulate = this._label.Modulate;
			this.OnDayChanged(true);
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

		// Token: 0x06002DFB RID: 11771 RVA: 0x0014A9B8 File Offset: 0x00148BB8
		private void OnDayChanged(bool changeLeaderboardDay)
		{
			this._label.SetTextAutoSize(this._currentDay.ToString(NDailyRunScreen.dateFormat));
			if (changeLeaderboardDay)
			{
				this._leaderboard.SetDay(this._currentDay);
			}
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x0014A9E9 File Offset: 0x00148BE9
		public void Disable()
		{
			this._leftArrow.Disable();
			this._rightArrow.Disable();
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x0014AA01 File Offset: 0x00148C01
		public void Enable(bool leftArrowEnabled, bool rightArrowEnabled)
		{
			if (leftArrowEnabled)
			{
				this._leftArrow.Enable();
			}
			if (rightArrowEnabled)
			{
				this._rightArrow.Enable();
			}
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x0014AA1F File Offset: 0x00148C1F
		private void OnFocus()
		{
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x0014AA38 File Offset: 0x00148C38
		private void OnUnfocus()
		{
			this._selectionReticle.OnDeselect();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E00 RID: 11776 RVA: 0x0014AA48 File Offset: 0x00148C48
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NLeaderboardDayPaginator.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardDayPaginator.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "input", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NLeaderboardDayPaginator.MethodName.PageLeft, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardDayPaginator.MethodName.PageRight, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardDayPaginator.MethodName.DayChangeHelper, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "pagedLeft", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NLeaderboardDayPaginator.MethodName.OnDayChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "changeLeaderboardDay", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NLeaderboardDayPaginator.MethodName.Disable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardDayPaginator.MethodName.Enable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "leftArrowEnabled", 0L, "", 6L, false),
					new PropertyInfo(1L, "rightArrowEnabled", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NLeaderboardDayPaginator.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardDayPaginator.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E01 RID: 11777 RVA: 0x0014ACF4 File Offset: 0x00148EF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLeaderboardDayPaginator.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardDayPaginator.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardDayPaginator.MethodName.PageLeft && args.Count == 0)
			{
				this.PageLeft();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardDayPaginator.MethodName.PageRight && args.Count == 0)
			{
				this.PageRight();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardDayPaginator.MethodName.DayChangeHelper && args.Count == 1)
			{
				this.DayChangeHelper(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardDayPaginator.MethodName.OnDayChanged && args.Count == 1)
			{
				this.OnDayChanged(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardDayPaginator.MethodName.Disable && args.Count == 0)
			{
				this.Disable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardDayPaginator.MethodName.Enable && args.Count == 2)
			{
				this.Enable(VariantUtils.ConvertTo<bool>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardDayPaginator.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardDayPaginator.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002E02 RID: 11778 RVA: 0x0014AEC4 File Offset: 0x001490C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLeaderboardDayPaginator.MethodName._Ready || (ref method) == NLeaderboardDayPaginator.MethodName._GuiInput || (ref method) == NLeaderboardDayPaginator.MethodName.PageLeft || (ref method) == NLeaderboardDayPaginator.MethodName.PageRight || (ref method) == NLeaderboardDayPaginator.MethodName.DayChangeHelper || (ref method) == NLeaderboardDayPaginator.MethodName.OnDayChanged || (ref method) == NLeaderboardDayPaginator.MethodName.Disable || (ref method) == NLeaderboardDayPaginator.MethodName.Enable || (ref method) == NLeaderboardDayPaginator.MethodName.OnFocus || (ref method) == NLeaderboardDayPaginator.MethodName.OnUnfocus || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002E03 RID: 11779 RVA: 0x0014AF70 File Offset: 0x00149170
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._vfxLabel)
			{
				this._vfxLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._leftArrow)
			{
				this._leftArrow = VariantUtils.ConvertTo<NLeaderboardPageArrow>(ref value);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._rightArrow)
			{
				this._rightArrow = VariantUtils.ConvertTo<NLeaderboardPageArrow>(ref value);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._leaderboard)
			{
				this._leaderboard = VariantUtils.ConvertTo<NDailyRunLeaderboard>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002E04 RID: 11780 RVA: 0x0014B044 File Offset: 0x00149244
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._vfxLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._vfxLabel);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._leftArrow)
			{
				value = VariantUtils.CreateFrom<NLeaderboardPageArrow>(ref this._leftArrow);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._rightArrow)
			{
				value = VariantUtils.CreateFrom<NLeaderboardPageArrow>(ref this._rightArrow);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NLeaderboardDayPaginator.PropertyName._leaderboard)
			{
				value = VariantUtils.CreateFrom<NDailyRunLeaderboard>(ref this._leaderboard);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E05 RID: 11781 RVA: 0x0014B13C File Offset: 0x0014933C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NLeaderboardDayPaginator.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLeaderboardDayPaginator.PropertyName._vfxLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLeaderboardDayPaginator.PropertyName._leftArrow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLeaderboardDayPaginator.PropertyName._rightArrow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLeaderboardDayPaginator.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLeaderboardDayPaginator.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLeaderboardDayPaginator.PropertyName._leaderboard, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E06 RID: 11782 RVA: 0x0014B238 File Offset: 0x00149438
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLeaderboardDayPaginator.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NLeaderboardDayPaginator.PropertyName._vfxLabel, Variant.From<MegaLabel>(ref this._vfxLabel));
			info.AddProperty(NLeaderboardDayPaginator.PropertyName._leftArrow, Variant.From<NLeaderboardPageArrow>(ref this._leftArrow));
			info.AddProperty(NLeaderboardDayPaginator.PropertyName._rightArrow, Variant.From<NLeaderboardPageArrow>(ref this._rightArrow));
			info.AddProperty(NLeaderboardDayPaginator.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NLeaderboardDayPaginator.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NLeaderboardDayPaginator.PropertyName._leaderboard, Variant.From<NDailyRunLeaderboard>(ref this._leaderboard));
		}

		/// <inheritdoc />
		// Token: 0x06002E07 RID: 11783 RVA: 0x0014B2E8 File Offset: 0x001494E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._vfxLabel, ref variant2))
			{
				this._vfxLabel = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._leftArrow, ref variant3))
			{
				this._leftArrow = variant3.As<NLeaderboardPageArrow>();
			}
			Variant variant4;
			if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._rightArrow, ref variant4))
			{
				this._rightArrow = variant4.As<NLeaderboardPageArrow>();
			}
			Variant variant5;
			if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._selectionReticle, ref variant5))
			{
				this._selectionReticle = variant5.As<NSelectionReticle>();
			}
			Variant variant6;
			if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._tween, ref variant6))
			{
				this._tween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NLeaderboardDayPaginator.PropertyName._leaderboard, ref variant7))
			{
				this._leaderboard = variant7.As<NDailyRunLeaderboard>();
			}
		}

		// Token: 0x04001195 RID: 4501
		[Nullable(1)]
		protected MegaLabel _label;

		// Token: 0x04001196 RID: 4502
		[Nullable(1)]
		private MegaLabel _vfxLabel;

		// Token: 0x04001197 RID: 4503
		[Nullable(1)]
		private NLeaderboardPageArrow _leftArrow;

		// Token: 0x04001198 RID: 4504
		[Nullable(1)]
		private NLeaderboardPageArrow _rightArrow;

		// Token: 0x04001199 RID: 4505
		[Nullable(1)]
		private NSelectionReticle _selectionReticle;

		// Token: 0x0400119A RID: 4506
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x0400119B RID: 4507
		private const double _animDuration = 0.25;

		// Token: 0x0400119C RID: 4508
		private const float _animDistance = 90f;

		// Token: 0x0400119D RID: 4509
		private DateTimeOffset _currentDay;

		// Token: 0x0400119E RID: 4510
		[Nullable(2)]
		private NDailyRunLeaderboard _leaderboard;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015B4 RID: 5556
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400535A RID: 21338
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x0400535B RID: 21339
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'PageLeft' method.
			/// </summary>
			// Token: 0x0400535C RID: 21340
			public static readonly StringName PageLeft = "PageLeft";

			/// <summary>
			/// Cached name for the 'PageRight' method.
			/// </summary>
			// Token: 0x0400535D RID: 21341
			public static readonly StringName PageRight = "PageRight";

			/// <summary>
			/// Cached name for the 'DayChangeHelper' method.
			/// </summary>
			// Token: 0x0400535E RID: 21342
			public static readonly StringName DayChangeHelper = "DayChangeHelper";

			/// <summary>
			/// Cached name for the 'OnDayChanged' method.
			/// </summary>
			// Token: 0x0400535F RID: 21343
			public static readonly StringName OnDayChanged = "OnDayChanged";

			/// <summary>
			/// Cached name for the 'Disable' method.
			/// </summary>
			// Token: 0x04005360 RID: 21344
			public static readonly StringName Disable = "Disable";

			/// <summary>
			/// Cached name for the 'Enable' method.
			/// </summary>
			// Token: 0x04005361 RID: 21345
			public static readonly StringName Enable = "Enable";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005362 RID: 21346
			public static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005363 RID: 21347
			public static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015B5 RID: 5557
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005364 RID: 21348
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_vfxLabel' field.
			/// </summary>
			// Token: 0x04005365 RID: 21349
			public static readonly StringName _vfxLabel = "_vfxLabel";

			/// <summary>
			/// Cached name for the '_leftArrow' field.
			/// </summary>
			// Token: 0x04005366 RID: 21350
			public static readonly StringName _leftArrow = "_leftArrow";

			/// <summary>
			/// Cached name for the '_rightArrow' field.
			/// </summary>
			// Token: 0x04005367 RID: 21351
			public static readonly StringName _rightArrow = "_rightArrow";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04005368 RID: 21352
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005369 RID: 21353
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_leaderboard' field.
			/// </summary>
			// Token: 0x0400536A RID: 21354
			public static readonly StringName _leaderboard = "_leaderboard";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015B6 RID: 5558
		public class SignalName : Control.SignalName
		{
		}
	}
}
