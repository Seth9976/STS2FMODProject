using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	// Token: 0x02000267 RID: 615
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NUnlockConfirmButton.cs")]
	public class NUnlockConfirmButton : NButton
	{
		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002059 RID: 8281 RVA: 0x000DB462 File Offset: 0x000D9662
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[]
				{
					MegaInput.select,
					MegaInput.pauseAndBack
				};
			}
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x000DB484 File Offset: 0x000D9684
		public override void _Ready()
		{
			this.ConnectSignals();
			base.GetNode<MegaLabel>("Label").SetTextAutoSize(new LocString("timeline", "UNLOCK_CONFIRM").GetFormattedText());
			this._tween = base.CreateTween();
			float y = base.Position.Y;
			base.Position = new Vector2(base.Position.X, y + 180f);
			this._tween.TweenProperty(this, "position:y", y, 0.3).SetEase(1L).SetTrans(10L)
				.SetDelay(0.1);
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x000DB538 File Offset: 0x000D9738
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x000DB5A0 File Offset: 0x000D97A0
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this, "scale", Vector2.One * 1.1f, 0.05);
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000DB604 File Offset: 0x000D9804
		public override void _Process(double delta)
		{
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x000DB608 File Offset: 0x000D9808
		protected override void OnPress()
		{
			base.OnPress();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One * 0.95f, 0.25).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000DB680 File Offset: 0x000D9880
		protected override void OnRelease()
		{
			base.Disable();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", new Vector2(3f, 0.1f), 0.3).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this, "modulate:a", 0f, 0.25).SetEase(1L).SetTrans(7L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002060 RID: 8288 RVA: 0x000DB730 File Offset: 0x000D9930
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NUnlockConfirmButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockConfirmButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockConfirmButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockConfirmButton.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NUnlockConfirmButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockConfirmButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002061 RID: 8289 RVA: 0x000DB884 File Offset: 0x000D9A84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockConfirmButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockConfirmButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockConfirmButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockConfirmButton.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockConfirmButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockConfirmButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002062 RID: 8290 RVA: 0x000DB988 File Offset: 0x000D9B88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUnlockConfirmButton.MethodName._Ready || (ref method) == NUnlockConfirmButton.MethodName.OnUnfocus || (ref method) == NUnlockConfirmButton.MethodName.OnFocus || (ref method) == NUnlockConfirmButton.MethodName._Process || (ref method) == NUnlockConfirmButton.MethodName.OnPress || (ref method) == NUnlockConfirmButton.MethodName.OnRelease || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002063 RID: 8291 RVA: 0x000DB9F6 File Offset: 0x000D9BF6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUnlockConfirmButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NUnlockConfirmButton.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002064 RID: 8292 RVA: 0x000DBA38 File Offset: 0x000D9C38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUnlockConfirmButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NUnlockConfirmButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NUnlockConfirmButton.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002065 RID: 8293 RVA: 0x000DBAB0 File Offset: 0x000D9CB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUnlockConfirmButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockConfirmButton.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(34L, NUnlockConfirmButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002066 RID: 8294 RVA: 0x000DBB27 File Offset: 0x000D9D27
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUnlockConfirmButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NUnlockConfirmButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
		}

		/// <inheritdoc />
		// Token: 0x06002067 RID: 8295 RVA: 0x000DBB5C File Offset: 0x000D9D5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUnlockConfirmButton.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NUnlockConfirmButton.PropertyName._hoverTween, ref variant2))
			{
				this._hoverTween = variant2.As<Tween>();
			}
		}

		// Token: 0x04000CA4 RID: 3236
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000CA5 RID: 3237
		[Nullable(2)]
		private Tween _hoverTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012D2 RID: 4818
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004965 RID: 18789
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004966 RID: 18790
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004967 RID: 18791
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004968 RID: 18792
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004969 RID: 18793
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x0400496A RID: 18794
			public new static readonly StringName OnRelease = "OnRelease";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012D3 RID: 4819
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x0400496B RID: 18795
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400496C RID: 18796
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x0400496D RID: 18797
			public static readonly StringName _hoverTween = "_hoverTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012D4 RID: 4820
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
