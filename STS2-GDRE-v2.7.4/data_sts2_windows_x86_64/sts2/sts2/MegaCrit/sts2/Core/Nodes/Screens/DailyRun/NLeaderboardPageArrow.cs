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

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	// Token: 0x02000335 RID: 821
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NLeaderboardPageArrow.cs")]
	public class NLeaderboardPageArrow : NButton
	{
		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002E09 RID: 11785 RVA: 0x0014B3C8 File Offset: 0x001495C8
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return this._hotkeys;
			}
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x0014B3D0 File Offset: 0x001495D0
		public override void _Ready()
		{
			string[] array;
			if (!this._isLeftArrow)
			{
				(array = new string[1])[0] = MegaInput.viewDiscardPile;
			}
			else
			{
				(array = new string[1])[0] = MegaInput.viewDrawPile;
			}
			this._hotkeys = array;
			this.ConnectSignals();
			this._image = base.GetNode<TextureRect>("Image");
			this._baseScale = this._image.Scale;
			this._hsv = (ShaderMaterial)this._image.Material;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x0014B456 File Offset: 0x00149656
		protected override void GetControllerIconNode()
		{
			this._controllerHotkeyIcon = base.GetNodeOrNull<TextureRect>("ControllerIcon");
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x0014B46E File Offset: 0x0014966E
		[NullableContext(1)]
		public void Connect(Action onRelease)
		{
			this._onRelease = onRelease;
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x0014B477 File Offset: 0x00149677
		protected override void OnDisable()
		{
			base.Modulate = StsColors.exhaustGray;
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x0014B484 File Offset: 0x00149684
		protected override void OnEnable()
		{
			base.Modulate = Colors.White;
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x0014B494 File Offset: 0x00149694
		protected override void OnRelease()
		{
			Action onRelease = this._onRelease;
			if (onRelease != null)
			{
				onRelease();
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale * 1.1f, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NLeaderboardPageArrow._v), 1.5f, 0.05);
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x0014B550 File Offset: 0x00149750
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale * 1.1f, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NLeaderboardPageArrow._v), 1.5f, 0.05);
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x0014B600 File Offset: 0x00149800
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NLeaderboardPageArrow._v), 0.9f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x0014B6C4 File Offset: 0x001498C4
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale * 0.9f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NLeaderboardPageArrow._v), 0.8f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x0014B790 File Offset: 0x00149990
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NLeaderboardPageArrow._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E14 RID: 11796 RVA: 0x0014B7A8 File Offset: 0x001499A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NLeaderboardPageArrow.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardPageArrow.MethodName.GetControllerIconNode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardPageArrow.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardPageArrow.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardPageArrow.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardPageArrow.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardPageArrow.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardPageArrow.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLeaderboardPageArrow.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E15 RID: 11797 RVA: 0x0014B98C File Offset: 0x00149B8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLeaderboardPageArrow.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardPageArrow.MethodName.GetControllerIconNode && args.Count == 0)
			{
				this.GetControllerIconNode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardPageArrow.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardPageArrow.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardPageArrow.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardPageArrow.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardPageArrow.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardPageArrow.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLeaderboardPageArrow.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002E16 RID: 11798 RVA: 0x0014BB00 File Offset: 0x00149D00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLeaderboardPageArrow.MethodName._Ready || (ref method) == NLeaderboardPageArrow.MethodName.GetControllerIconNode || (ref method) == NLeaderboardPageArrow.MethodName.OnDisable || (ref method) == NLeaderboardPageArrow.MethodName.OnEnable || (ref method) == NLeaderboardPageArrow.MethodName.OnRelease || (ref method) == NLeaderboardPageArrow.MethodName.OnFocus || (ref method) == NLeaderboardPageArrow.MethodName.OnUnfocus || (ref method) == NLeaderboardPageArrow.MethodName.OnPress || (ref method) == NLeaderboardPageArrow.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002E17 RID: 11799 RVA: 0x0014BB9C File Offset: 0x00149D9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLeaderboardPageArrow.PropertyName._isLeftArrow)
			{
				this._isLeftArrow = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NLeaderboardPageArrow.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NLeaderboardPageArrow.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NLeaderboardPageArrow.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NLeaderboardPageArrow.PropertyName._baseScale)
			{
				this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NLeaderboardPageArrow.PropertyName._hotkeys)
			{
				this._hotkeys = VariantUtils.ConvertTo<string[]>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002E18 RID: 11800 RVA: 0x0014BC54 File Offset: 0x00149E54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLeaderboardPageArrow.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NLeaderboardPageArrow.PropertyName._isLeftArrow)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isLeftArrow);
				return true;
			}
			if ((ref name) == NLeaderboardPageArrow.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NLeaderboardPageArrow.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NLeaderboardPageArrow.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NLeaderboardPageArrow.PropertyName._baseScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
				return true;
			}
			if ((ref name) == NLeaderboardPageArrow.PropertyName._hotkeys)
			{
				value = VariantUtils.CreateFrom<string[]>(ref this._hotkeys);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E19 RID: 11801 RVA: 0x0014BD4C File Offset: 0x00149F4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NLeaderboardPageArrow.PropertyName._isLeftArrow, 0L, "", 4102L, true),
				new PropertyInfo(24L, NLeaderboardPageArrow.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLeaderboardPageArrow.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLeaderboardPageArrow.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NLeaderboardPageArrow.PropertyName._baseScale, 0L, "", 4096L, false),
				new PropertyInfo(34L, NLeaderboardPageArrow.PropertyName._hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(34L, NLeaderboardPageArrow.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E1A RID: 11802 RVA: 0x0014BE48 File Offset: 0x0014A048
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLeaderboardPageArrow.PropertyName._isLeftArrow, Variant.From<bool>(ref this._isLeftArrow));
			info.AddProperty(NLeaderboardPageArrow.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NLeaderboardPageArrow.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NLeaderboardPageArrow.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NLeaderboardPageArrow.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
			info.AddProperty(NLeaderboardPageArrow.PropertyName._hotkeys, Variant.From<string[]>(ref this._hotkeys));
		}

		/// <inheritdoc />
		// Token: 0x06002E1B RID: 11803 RVA: 0x0014BEE0 File Offset: 0x0014A0E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLeaderboardPageArrow.PropertyName._isLeftArrow, ref variant))
			{
				this._isLeftArrow = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLeaderboardPageArrow.PropertyName._image, ref variant2))
			{
				this._image = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NLeaderboardPageArrow.PropertyName._hsv, ref variant3))
			{
				this._hsv = variant3.As<ShaderMaterial>();
			}
			Variant variant4;
			if (info.TryGetProperty(NLeaderboardPageArrow.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NLeaderboardPageArrow.PropertyName._baseScale, ref variant5))
			{
				this._baseScale = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NLeaderboardPageArrow.PropertyName._hotkeys, ref variant6))
			{
				this._hotkeys = variant6.As<string[]>();
			}
		}

		// Token: 0x0400119F RID: 4511
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private bool _isLeftArrow = true;

		// Token: 0x040011A0 RID: 4512
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040011A1 RID: 4513
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x040011A2 RID: 4514
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x040011A3 RID: 4515
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040011A4 RID: 4516
		private Vector2 _baseScale;

		// Token: 0x040011A5 RID: 4517
		[Nullable(2)]
		private Action _onRelease;

		// Token: 0x040011A6 RID: 4518
		[Nullable(1)]
		private string[] _hotkeys = Array.Empty<string>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015B7 RID: 5559
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400536B RID: 21355
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'GetControllerIconNode' method.
			/// </summary>
			// Token: 0x0400536C RID: 21356
			public new static readonly StringName GetControllerIconNode = "GetControllerIconNode";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x0400536D RID: 21357
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x0400536E RID: 21358
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x0400536F RID: 21359
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005370 RID: 21360
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005371 RID: 21361
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005372 RID: 21362
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04005373 RID: 21363
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015B8 RID: 5560
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005374 RID: 21364
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_isLeftArrow' field.
			/// </summary>
			// Token: 0x04005375 RID: 21365
			public static readonly StringName _isLeftArrow = "_isLeftArrow";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04005376 RID: 21366
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005377 RID: 21367
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005378 RID: 21368
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_baseScale' field.
			/// </summary>
			// Token: 0x04005379 RID: 21369
			public static readonly StringName _baseScale = "_baseScale";

			/// <summary>
			/// Cached name for the '_hotkeys' field.
			/// </summary>
			// Token: 0x0400537A RID: 21370
			public static readonly StringName _hotkeys = "_hotkeys";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015B9 RID: 5561
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
