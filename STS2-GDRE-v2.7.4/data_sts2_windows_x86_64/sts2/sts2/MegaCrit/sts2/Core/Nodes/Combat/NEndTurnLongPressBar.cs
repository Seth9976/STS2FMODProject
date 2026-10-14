using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// This bar represents the player's progress when clicking or pushing down the End Turn input.
	/// When the bar is filled, the turn is ended. This is an option players can enable so they don't
	/// accidentally press the End Turn button (useful for touch-devices and people who are simply
	/// playing super fast all the time).
	/// </summary>
	// Token: 0x020003FF RID: 1023
	[ScriptPath("res://src/Core/Nodes/Combat/NEndTurnLongPressBar.cs")]
	public class NEndTurnLongPressBar : ColorRect
	{
		// Token: 0x06003FAF RID: 16303 RVA: 0x001E933E File Offset: 0x001E753E
		public override void _Ready()
		{
			this._outline = base.GetNode<Control>("%BarOutline");
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x001E9356 File Offset: 0x001E7556
		[NullableContext(1)]
		public void Init(NEndTurnButton endTurnButton)
		{
			this._endTurnButton = endTurnButton;
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x001E935F File Offset: 0x001E755F
		public void StartPress()
		{
			this._isPressed = true;
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x001E9368 File Offset: 0x001E7568
		public void CancelPress()
		{
			this._isPressed = false;
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x001E9374 File Offset: 0x001E7574
		public override void _Process(double delta)
		{
			if (!this._enabled)
			{
				return;
			}
			if (!this._isPressed)
			{
				if (this._pressTimer > 0.0)
				{
					this._pressTimer -= delta;
					if (this._pressTimer < 0.0)
					{
						this._pressTimer = 0.0;
						Color modulate = base.Modulate;
						modulate.A = 0f;
						base.Modulate = modulate;
						return;
					}
					this.RecalculateBar();
				}
				return;
			}
			this._pressTimer += delta;
			if (this._pressTimer > 0.5)
			{
				this._enabled = false;
				base.Size = new Vector2(204f, 6f);
				this._pressTimer = 0.0;
				this._endTurnButton.CallReleaseLogic();
				TaskHelper.RunSafely(this.PlayAnim());
				return;
			}
			this.RecalculateBar();
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x001E945C File Offset: 0x001E765C
		private void RecalculateBar()
		{
			float num = (float)(this._pressTimer / 0.5);
			base.Size = new Vector2(num * 204f, 6f);
			base.Color = new Color(num * 2.5f, 0.6f + num, 0.6f, 1f);
			Color modulate = base.Modulate;
			modulate.A = Ease.CubicOut(num * 0.75f);
			base.Modulate = modulate;
		}

		/// <summary>
		/// Called when the bar is completely filled.
		/// </summary>
		// Token: 0x06003FB5 RID: 16309 RVA: 0x001E94D8 File Offset: 0x001E76D8
		[NullableContext(1)]
		private async Task PlayAnim()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.25);
			TaskAwaiter<bool> taskAwaiter = this._tween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				base.Color = new Color(1f, 0.85f, 0.36f, 1f);
				this._isPressed = false;
				this._enabled = true;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003FB6 RID: 16310 RVA: 0x001E951C File Offset: 0x001E771C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NEndTurnLongPressBar.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnLongPressBar.MethodName.Init, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "endTurnButton", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NEndTurnLongPressBar.MethodName.StartPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnLongPressBar.MethodName.CancelPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEndTurnLongPressBar.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEndTurnLongPressBar.MethodName.RecalculateBar, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003FB7 RID: 16311 RVA: 0x001E96A0 File Offset: 0x001E78A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEndTurnLongPressBar.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnLongPressBar.MethodName.Init && args.Count == 1)
			{
				this.Init(VariantUtils.ConvertTo<NEndTurnButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnLongPressBar.MethodName.StartPress && args.Count == 0)
			{
				this.StartPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnLongPressBar.MethodName.CancelPress && args.Count == 0)
			{
				this.CancelPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnLongPressBar.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEndTurnLongPressBar.MethodName.RecalculateBar && args.Count == 0)
			{
				this.RecalculateBar();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003FB8 RID: 16312 RVA: 0x001E97B0 File Offset: 0x001E79B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEndTurnLongPressBar.MethodName._Ready || (ref method) == NEndTurnLongPressBar.MethodName.Init || (ref method) == NEndTurnLongPressBar.MethodName.StartPress || (ref method) == NEndTurnLongPressBar.MethodName.CancelPress || (ref method) == NEndTurnLongPressBar.MethodName._Process || (ref method) == NEndTurnLongPressBar.MethodName.RecalculateBar || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003FB9 RID: 16313 RVA: 0x001E9820 File Offset: 0x001E7A20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEndTurnLongPressBar.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnLongPressBar.PropertyName._pressTimer)
			{
				this._pressTimer = VariantUtils.ConvertTo<double>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnLongPressBar.PropertyName._isPressed)
			{
				this._isPressed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnLongPressBar.PropertyName._endTurnButton)
			{
				this._endTurnButton = VariantUtils.ConvertTo<NEndTurnButton>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnLongPressBar.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEndTurnLongPressBar.PropertyName._enabled)
			{
				this._enabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003FBA RID: 16314 RVA: 0x001E98D8 File Offset: 0x001E7AD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEndTurnLongPressBar.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outline);
				return true;
			}
			if ((ref name) == NEndTurnLongPressBar.PropertyName._pressTimer)
			{
				value = VariantUtils.CreateFrom<double>(ref this._pressTimer);
				return true;
			}
			if ((ref name) == NEndTurnLongPressBar.PropertyName._isPressed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isPressed);
				return true;
			}
			if ((ref name) == NEndTurnLongPressBar.PropertyName._endTurnButton)
			{
				value = VariantUtils.CreateFrom<NEndTurnButton>(ref this._endTurnButton);
				return true;
			}
			if ((ref name) == NEndTurnLongPressBar.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NEndTurnLongPressBar.PropertyName._enabled)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._enabled);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003FBB RID: 16315 RVA: 0x001E99B0 File Offset: 0x001E7BB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEndTurnLongPressBar.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(3L, NEndTurnLongPressBar.PropertyName._pressTimer, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEndTurnLongPressBar.PropertyName._isPressed, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnLongPressBar.PropertyName._endTurnButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEndTurnLongPressBar.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEndTurnLongPressBar.PropertyName._enabled, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003FBC RID: 16316 RVA: 0x001E9A88 File Offset: 0x001E7C88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEndTurnLongPressBar.PropertyName._outline, Variant.From<Control>(ref this._outline));
			info.AddProperty(NEndTurnLongPressBar.PropertyName._pressTimer, Variant.From<double>(ref this._pressTimer));
			info.AddProperty(NEndTurnLongPressBar.PropertyName._isPressed, Variant.From<bool>(ref this._isPressed));
			info.AddProperty(NEndTurnLongPressBar.PropertyName._endTurnButton, Variant.From<NEndTurnButton>(ref this._endTurnButton));
			info.AddProperty(NEndTurnLongPressBar.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NEndTurnLongPressBar.PropertyName._enabled, Variant.From<bool>(ref this._enabled));
		}

		/// <inheritdoc />
		// Token: 0x06003FBD RID: 16317 RVA: 0x001E9B20 File Offset: 0x001E7D20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEndTurnLongPressBar.PropertyName._outline, ref variant))
			{
				this._outline = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEndTurnLongPressBar.PropertyName._pressTimer, ref variant2))
			{
				this._pressTimer = variant2.As<double>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEndTurnLongPressBar.PropertyName._isPressed, ref variant3))
			{
				this._isPressed = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEndTurnLongPressBar.PropertyName._endTurnButton, ref variant4))
			{
				this._endTurnButton = variant4.As<NEndTurnButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NEndTurnLongPressBar.PropertyName._tween, ref variant5))
			{
				this._tween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NEndTurnLongPressBar.PropertyName._enabled, ref variant6))
			{
				this._enabled = variant6.As<bool>();
			}
		}

		// Token: 0x040018B0 RID: 6320
		[Nullable(1)]
		private Control _outline;

		// Token: 0x040018B1 RID: 6321
		private double _pressTimer;

		// Token: 0x040018B2 RID: 6322
		private const double _longPressDuration = 0.5;

		// Token: 0x040018B3 RID: 6323
		private bool _isPressed;

		// Token: 0x040018B4 RID: 6324
		private const float _targetWidth = 204f;

		// Token: 0x040018B5 RID: 6325
		[Nullable(1)]
		private NEndTurnButton _endTurnButton;

		// Token: 0x040018B6 RID: 6326
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040018B7 RID: 6327
		private bool _enabled = true;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018E0 RID: 6368
		public class MethodName : ColorRect.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006189 RID: 24969
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Init' method.
			/// </summary>
			// Token: 0x0400618A RID: 24970
			public static readonly StringName Init = "Init";

			/// <summary>
			/// Cached name for the 'StartPress' method.
			/// </summary>
			// Token: 0x0400618B RID: 24971
			public static readonly StringName StartPress = "StartPress";

			/// <summary>
			/// Cached name for the 'CancelPress' method.
			/// </summary>
			// Token: 0x0400618C RID: 24972
			public static readonly StringName CancelPress = "CancelPress";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x0400618D RID: 24973
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'RecalculateBar' method.
			/// </summary>
			// Token: 0x0400618E RID: 24974
			public static readonly StringName RecalculateBar = "RecalculateBar";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018E1 RID: 6369
		public class PropertyName : ColorRect.PropertyName
		{
			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x0400618F RID: 24975
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_pressTimer' field.
			/// </summary>
			// Token: 0x04006190 RID: 24976
			public static readonly StringName _pressTimer = "_pressTimer";

			/// <summary>
			/// Cached name for the '_isPressed' field.
			/// </summary>
			// Token: 0x04006191 RID: 24977
			public static readonly StringName _isPressed = "_isPressed";

			/// <summary>
			/// Cached name for the '_endTurnButton' field.
			/// </summary>
			// Token: 0x04006192 RID: 24978
			public static readonly StringName _endTurnButton = "_endTurnButton";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04006193 RID: 24979
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_enabled' field.
			/// </summary>
			// Token: 0x04006194 RID: 24980
			public static readonly StringName _enabled = "_enabled";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018E2 RID: 6370
		public class SignalName : ColorRect.SignalName
		{
		}
	}
}
