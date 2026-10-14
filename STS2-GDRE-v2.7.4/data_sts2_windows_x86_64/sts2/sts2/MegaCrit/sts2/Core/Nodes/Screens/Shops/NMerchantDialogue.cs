using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops
{
	// Token: 0x0200027D RID: 637
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantDialogue.cs")]
	public class NMerchantDialogue : Node2D
	{
		// Token: 0x060021BF RID: 8639 RVA: 0x000E5D84 File Offset: 0x000E3F84
		public override void _Ready()
		{
			this._label = base.GetNode<MegaRichTextLabel>("%Text");
			this._dialogueBox = base.GetNode<Node2D>("%DialogueBox");
			this._bubble = base.GetNode<Sprite2D>("%Bubble");
			base.Modulate = Colors.Transparent;
			this._hsv = (ShaderMaterial)this._bubble.Material;
			this._hsv.SetShaderParameter(NMerchantDialogue._h, 1f);
			this._hsv.SetShaderParameter(NMerchantDialogue._s, 1.2f);
			this._hsv.SetShaderParameter(NMerchantDialogue._v, 0.4f);
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x000E5E42 File Offset: 0x000E4042
		public void Initialize(MerchantDialogueSet dialogueSet)
		{
			this._dialogueSet = dialogueSet;
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000E5E4B File Offset: 0x000E404B
		public void ShowOnInventoryOpen()
		{
			this.ShowRandom(this._dialogueSet.OpenInventoryLines);
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x000E5E5E File Offset: 0x000E405E
		public void ShowForPurchaseAttempt(PurchaseStatus status)
		{
			this.ShowRandom(this._dialogueSet.GetPurchaseSuccessLines(status));
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x000E5E74 File Offset: 0x000E4074
		private void ShowRandom(IEnumerable<LocString> lines)
		{
			LocString locString = Rng.Chaotic.NextItem<LocString>(lines);
			if (locString == null)
			{
				return;
			}
			this._label.Text = "[fly_in]" + locString.GetFormattedText() + "[/fly_in]";
			base.Modulate = StsColors.transparentWhite;
			base.Position = new Vector2(Rng.Chaotic.NextFloat(NMerchantDialogue._xRange.X, NMerchantDialogue._xRange.Y), base.Position.Y);
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.25);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.25).SetEase(1L).SetTrans(10L);
			this._tween.TweenProperty(this._label, "visible_ratio", 1f, 0.4).From(0f);
			this._tween.TweenProperty(this._bubble, "scale", new Vector2(0.75f, 0.75f), 0.5).From(new Vector2(0.25f, 0.25f)).SetEase(1L)
				.SetTrans(5L);
			this._tween.TweenProperty(this._dialogueBox, "position:y", 0f, 0.5).From(-80f).SetEase(1L)
				.SetTrans(10L);
			this._tween.Chain();
			this._tween.TweenInterval(1.0);
			this._tween.Chain();
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.5).SetEase(1L).SetTrans(1L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060021C4 RID: 8644 RVA: 0x000E60C8 File Offset: 0x000E42C8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMerchantDialogue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantDialogue.MethodName.ShowOnInventoryOpen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMerchantDialogue.MethodName.ShowForPurchaseAttempt, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "status", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060021C5 RID: 8645 RVA: 0x000E6190 File Offset: 0x000E4390
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMerchantDialogue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantDialogue.MethodName.ShowOnInventoryOpen && args.Count == 0)
			{
				this.ShowOnInventoryOpen();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMerchantDialogue.MethodName.ShowForPurchaseAttempt && args.Count == 1)
			{
				this.ShowForPurchaseAttempt(VariantUtils.ConvertTo<PurchaseStatus>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060021C6 RID: 8646 RVA: 0x000E6223 File Offset: 0x000E4423
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMerchantDialogue.MethodName._Ready || (ref method) == NMerchantDialogue.MethodName.ShowOnInventoryOpen || (ref method) == NMerchantDialogue.MethodName.ShowForPurchaseAttempt || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060021C7 RID: 8647 RVA: 0x000E625C File Offset: 0x000E445C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMerchantDialogue.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMerchantDialogue.PropertyName._dialogueBox)
			{
				this._dialogueBox = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NMerchantDialogue.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMerchantDialogue.PropertyName._bubble)
			{
				this._bubble = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NMerchantDialogue.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060021C8 RID: 8648 RVA: 0x000E62F8 File Offset: 0x000E44F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMerchantDialogue.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NMerchantDialogue.PropertyName._dialogueBox)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._dialogueBox);
				return true;
			}
			if ((ref name) == NMerchantDialogue.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NMerchantDialogue.PropertyName._bubble)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._bubble);
				return true;
			}
			if ((ref name) == NMerchantDialogue.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060021C9 RID: 8649 RVA: 0x000E63B0 File Offset: 0x000E45B0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMerchantDialogue.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantDialogue.PropertyName._dialogueBox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantDialogue.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantDialogue.PropertyName._bubble, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMerchantDialogue.PropertyName._hsv, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060021CA RID: 8650 RVA: 0x000E646C File Offset: 0x000E466C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMerchantDialogue.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NMerchantDialogue.PropertyName._dialogueBox, Variant.From<Node2D>(ref this._dialogueBox));
			info.AddProperty(NMerchantDialogue.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NMerchantDialogue.PropertyName._bubble, Variant.From<Sprite2D>(ref this._bubble));
			info.AddProperty(NMerchantDialogue.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
		}

		/// <inheritdoc />
		// Token: 0x060021CB RID: 8651 RVA: 0x000E64F0 File Offset: 0x000E46F0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMerchantDialogue.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMerchantDialogue.PropertyName._dialogueBox, ref variant2))
			{
				this._dialogueBox = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMerchantDialogue.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMerchantDialogue.PropertyName._bubble, ref variant4))
			{
				this._bubble = variant4.As<Sprite2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMerchantDialogue.PropertyName._hsv, ref variant5))
			{
				this._hsv = variant5.As<ShaderMaterial>();
			}
		}

		// Token: 0x04000D27 RID: 3367
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000D28 RID: 3368
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04000D29 RID: 3369
		private static readonly StringName _h = new StringName("h");

		// Token: 0x04000D2A RID: 3370
		private static readonly Vector2 _xRange = new Vector2(450f, 1450f);

		// Token: 0x04000D2B RID: 3371
		private MegaRichTextLabel _label;

		// Token: 0x04000D2C RID: 3372
		private Node2D _dialogueBox;

		// Token: 0x04000D2D RID: 3373
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000D2E RID: 3374
		private Sprite2D _bubble;

		// Token: 0x04000D2F RID: 3375
		private ShaderMaterial _hsv;

		// Token: 0x04000D30 RID: 3376
		private MerchantDialogueSet _dialogueSet;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001321 RID: 4897
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004A50 RID: 19024
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ShowOnInventoryOpen' method.
			/// </summary>
			// Token: 0x04004A51 RID: 19025
			public static readonly StringName ShowOnInventoryOpen = "ShowOnInventoryOpen";

			/// <summary>
			/// Cached name for the 'ShowForPurchaseAttempt' method.
			/// </summary>
			// Token: 0x04004A52 RID: 19026
			public static readonly StringName ShowForPurchaseAttempt = "ShowForPurchaseAttempt";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001322 RID: 4898
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004A53 RID: 19027
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_dialogueBox' field.
			/// </summary>
			// Token: 0x04004A54 RID: 19028
			public static readonly StringName _dialogueBox = "_dialogueBox";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004A55 RID: 19029
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_bubble' field.
			/// </summary>
			// Token: 0x04004A56 RID: 19030
			public static readonly StringName _bubble = "_bubble";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004A57 RID: 19031
			public static readonly StringName _hsv = "_hsv";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001323 RID: 4899
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
