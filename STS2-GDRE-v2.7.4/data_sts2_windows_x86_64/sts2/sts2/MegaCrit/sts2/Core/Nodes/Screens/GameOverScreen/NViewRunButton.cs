using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen
{
	// Token: 0x0200031F RID: 799
	[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NViewRunButton.cs")]
	public class NViewRunButton : NButton
	{
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002C9D RID: 11421 RVA: 0x0013EA15 File Offset: 0x0013CC15
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.accept };
			}
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x0013EA2C File Offset: 0x0013CC2C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._hsv = (ShaderMaterial)base.GetNode<TextureRect>("Image").Material;
			this._showPosition = base.GetPosition();
			base.Position = this._showPosition + new Vector2(140f, 0f);
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x0013EA8C File Offset: 0x0013CC8C
		protected override void OnPress()
		{
			base.OnPress();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One, 1.0).SetTrans(5L).SetEase(1L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NViewRunButton._s), 1f, 1.0).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NViewRunButton._v), 1f, 1.0).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Call when we want this button to animate in.
		/// Called when we transition from Death intermission -&gt; summary and when returning from Run History/Settings.
		/// </summary>
		// Token: 0x06002CA0 RID: 11424 RVA: 0x0013EB98 File Offset: 0x0013CD98
		protected override void OnEnable()
		{
			base.OnEnable();
			base.Visible = false;
			this._isEnabled = true;
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "position", this._showPosition, 0.5).SetEase(1L).SetTrans(3L);
			this._tween.TweenProperty(this, "modulate", Colors.White, 0.5).From(StsColors.transparentBlack);
		}

		/// <summary>
		/// Called to hide this button (Open Settings/Run History or something during Death summary)
		/// </summary>
		// Token: 0x06002CA1 RID: 11425 RVA: 0x0013EC40 File Offset: 0x0013CE40
		protected override void OnDisable()
		{
			base.OnDisable();
			base.Visible = true;
			this._isEnabled = false;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "position:y", this._showPosition.Y + 190f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x0013ECC8 File Offset: 0x0013CEC8
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One * 1.1f, 0.05);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NViewRunButton._s), 1.2f, 0.05).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NViewRunButton._v), 1.4f, 0.05).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x0013EDD0 File Offset: 0x0013CFD0
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One, 1.0).SetTrans(5L).SetEase(1L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NViewRunButton._s), 1f, 1.0).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NViewRunButton._v), 1f, 1.0).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x0013EEDA File Offset: 0x0013D0DA
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NViewRunButton._s, value);
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x0013EEF2 File Offset: 0x0013D0F2
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NViewRunButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CA6 RID: 11430 RVA: 0x0013EF0C File Offset: 0x0013D10C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NViewRunButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NViewRunButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NViewRunButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NViewRunButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NViewRunButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NViewRunButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NViewRunButton.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NViewRunButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CA7 RID: 11431 RVA: 0x0013F0E4 File Offset: 0x0013D2E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NViewRunButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NViewRunButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NViewRunButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NViewRunButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NViewRunButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NViewRunButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NViewRunButton.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NViewRunButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002CA8 RID: 11432 RVA: 0x0013F240 File Offset: 0x0013D440
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NViewRunButton.MethodName._Ready || (ref method) == NViewRunButton.MethodName.OnPress || (ref method) == NViewRunButton.MethodName.OnEnable || (ref method) == NViewRunButton.MethodName.OnDisable || (ref method) == NViewRunButton.MethodName.OnFocus || (ref method) == NViewRunButton.MethodName.OnUnfocus || (ref method) == NViewRunButton.MethodName.UpdateShaderS || (ref method) == NViewRunButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002CA9 RID: 11433 RVA: 0x0013F2CC File Offset: 0x0013D4CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NViewRunButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NViewRunButton.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NViewRunButton.PropertyName._showPosition)
			{
				this._showPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NViewRunButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002CAA RID: 11434 RVA: 0x0013F350 File Offset: 0x0013D550
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NViewRunButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NViewRunButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NViewRunButton.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NViewRunButton.PropertyName._showPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._showPosition);
				return true;
			}
			if ((ref name) == NViewRunButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CAB RID: 11435 RVA: 0x0013F408 File Offset: 0x0013D608
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NViewRunButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(24L, NViewRunButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NViewRunButton.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NViewRunButton.PropertyName._showPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NViewRunButton.PropertyName._hsv, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CAC RID: 11436 RVA: 0x0013F4C0 File Offset: 0x0013D6C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NViewRunButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NViewRunButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NViewRunButton.PropertyName._showPosition, Variant.From<Vector2>(ref this._showPosition));
			info.AddProperty(NViewRunButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
		}

		/// <inheritdoc />
		// Token: 0x06002CAD RID: 11437 RVA: 0x0013F52C File Offset: 0x0013D72C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NViewRunButton.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NViewRunButton.PropertyName._hoverTween, ref variant2))
			{
				this._hoverTween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NViewRunButton.PropertyName._showPosition, ref variant3))
			{
				this._showPosition = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NViewRunButton.PropertyName._hsv, ref variant4))
			{
				this._hsv = variant4.As<ShaderMaterial>();
			}
		}

		// Token: 0x040010E8 RID: 4328
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040010E9 RID: 4329
		[Nullable(1)]
		private static readonly StringName _s = new StringName("s");

		// Token: 0x040010EA RID: 4330
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040010EB RID: 4331
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x040010EC RID: 4332
		private Vector2 _showPosition;

		// Token: 0x040010ED RID: 4333
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x040010EE RID: 4334
		private const float _hoverS = 1.2f;

		// Token: 0x040010EF RID: 4335
		private const float _hoverV = 1.4f;

		// Token: 0x040010F0 RID: 4336
		private const float _unhoverS = 1f;

		// Token: 0x040010F1 RID: 4337
		private const float _unhoverV = 1f;

		// Token: 0x040010F2 RID: 4338
		private const float _pressDownS = 1f;

		// Token: 0x040010F3 RID: 4339
		private const float _pressDownV = 1f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001568 RID: 5480
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005213 RID: 21011
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005214 RID: 21012
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04005215 RID: 21013
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04005216 RID: 21014
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005217 RID: 21015
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005218 RID: 21016
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x04005219 RID: 21017
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x0400521A RID: 21018
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001569 RID: 5481
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x0400521B RID: 21019
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400521C RID: 21020
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x0400521D RID: 21021
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_showPosition' field.
			/// </summary>
			// Token: 0x0400521E RID: 21022
			public static readonly StringName _showPosition = "_showPosition";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x0400521F RID: 21023
			public static readonly StringName _hsv = "_hsv";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200156A RID: 5482
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
