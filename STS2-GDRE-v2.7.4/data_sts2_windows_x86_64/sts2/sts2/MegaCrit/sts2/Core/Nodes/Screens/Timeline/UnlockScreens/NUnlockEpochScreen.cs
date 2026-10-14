using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens
{
	/// <summary>
	/// Unlock screen which appears after you place the first Epoch and you unlock the three Epochs:
	/// Neow, Silent, and Regent Epochs. Used during Timeline Expansion One (slotting Ironclad Enters) and after you slot your fourth Epoch.
	/// </summary>
	// Token: 0x0200026B RID: 619
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockEpochScreen.cs")]
	public class NUnlockEpochScreen : NUnlockScreen
	{
		// Token: 0x0600209A RID: 8346 RVA: 0x000DD254 File Offset: 0x000DB454
		public override void _Ready()
		{
			base.ConnectSignals();
			this._infoLabel = base.GetNode<RichTextLabel>("%InfoLabel");
			LocString locString = new LocString("timeline", "UNLOCK_EPOCHS");
			this._infoLabel.Text = "[center]" + locString.GetFormattedText() + "[/center]";
			this._infoLabel.Modulate = StsColors.transparentWhite;
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x000DD2C0 File Offset: 0x000DB4C0
		public override void Open()
		{
			base.Open();
			this._cardFlyTween = base.CreateTween().SetParallel(true);
			double num = 0.3;
			Vector2 position = base.GetNode<Control>("%Center").Position;
			PackedScene scene = PreloadManager.Cache.GetScene("res://scenes/timeline_screen/epoch.tscn");
			if (this._unlockedEpochs.Count == 3)
			{
				for (int i = 0; i < this._unlockedEpochs.Count; i++)
				{
					NEpochCard nepochCard = scene.Instantiate<NEpochCard>(0L);
					nepochCard.Init(this._unlockedEpochs[i]);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Slot");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					Control node = base.GetNode<Control>(defaultInterpolatedStringHandler.ToStringAndClear());
					node.AddChildSafely(nepochCard);
					nepochCard.SetToWigglyUnlockPreviewMode();
					this._cardFlyTween.TweenProperty(node, "modulate", Colors.White, 1.0).SetDelay(num - 0.3);
					this._cardFlyTween.TweenProperty(node, "position", node.Position, 1.0).SetEase(1L).SetTrans(10L)
						.SetDelay(num);
					node.Modulate = StsColors.transparentBlack;
					node.Position = position;
					num += 0.25;
				}
			}
			else if (this._unlockedEpochs.Count == 2)
			{
				for (int j = 0; j < this._unlockedEpochs.Count; j++)
				{
					NEpochCard nepochCard2 = scene.Instantiate<NEpochCard>(0L);
					nepochCard2.Init(this._unlockedEpochs[j]);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Slot");
					defaultInterpolatedStringHandler.AppendFormatted<int>(3 + j);
					Control node2 = base.GetNode<Control>(defaultInterpolatedStringHandler.ToStringAndClear());
					node2.AddChildSafely(nepochCard2);
					nepochCard2.SetToWigglyUnlockPreviewMode();
					this._cardFlyTween.TweenProperty(node2, "modulate", Colors.White, 1.0).SetDelay(num - 0.3);
					this._cardFlyTween.TweenProperty(node2, "position", node2.Position, 1.0).SetEase(1L).SetTrans(10L)
						.SetDelay(num);
					node2.Modulate = StsColors.transparentBlack;
					node2.Position = position;
					num += 0.33;
				}
			}
			else
			{
				Log.Error("Unlocking exactly 1 OR more than 3 Epochs are not supported.", 2);
			}
			this._cardFlyTween.TweenProperty(this._infoLabel, "modulate", Colors.White, 1.0).SetDelay(0.25);
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x000DD5BC File Offset: 0x000DB7BC
		[NullableContext(1)]
		public void SetUnlocks(IReadOnlyList<EpochModel> epochs)
		{
			this._unlockedEpochs = epochs;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600209D RID: 8349 RVA: 0x000DD5C8 File Offset: 0x000DB7C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NUnlockEpochScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockEpochScreen.MethodName.Open, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600209E RID: 8350 RVA: 0x000DD63C File Offset: 0x000DB83C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockEpochScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockEpochScreen.MethodName.Open && args.Count == 0)
			{
				this.Open();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600209F RID: 8351 RVA: 0x000DD69C File Offset: 0x000DB89C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUnlockEpochScreen.MethodName._Ready || (ref method) == NUnlockEpochScreen.MethodName.Open || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060020A0 RID: 8352 RVA: 0x000DD6C3 File Offset: 0x000DB8C3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUnlockEpochScreen.PropertyName._cardFlyTween)
			{
				this._cardFlyTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NUnlockEpochScreen.PropertyName._infoLabel)
			{
				this._infoLabel = VariantUtils.ConvertTo<RichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060020A1 RID: 8353 RVA: 0x000DD704 File Offset: 0x000DB904
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUnlockEpochScreen.PropertyName._cardFlyTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._cardFlyTween);
				return true;
			}
			if ((ref name) == NUnlockEpochScreen.PropertyName._infoLabel)
			{
				value = VariantUtils.CreateFrom<RichTextLabel>(ref this._infoLabel);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060020A2 RID: 8354 RVA: 0x000DD75C File Offset: 0x000DB95C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUnlockEpochScreen.PropertyName._cardFlyTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockEpochScreen.PropertyName._infoLabel, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060020A3 RID: 8355 RVA: 0x000DD7B2 File Offset: 0x000DB9B2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUnlockEpochScreen.PropertyName._cardFlyTween, Variant.From<Tween>(ref this._cardFlyTween));
			info.AddProperty(NUnlockEpochScreen.PropertyName._infoLabel, Variant.From<RichTextLabel>(ref this._infoLabel));
		}

		/// <inheritdoc />
		// Token: 0x060020A4 RID: 8356 RVA: 0x000DD7E8 File Offset: 0x000DB9E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUnlockEpochScreen.PropertyName._cardFlyTween, ref variant))
			{
				this._cardFlyTween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NUnlockEpochScreen.PropertyName._infoLabel, ref variant2))
			{
				this._infoLabel = variant2.As<RichTextLabel>();
			}
		}

		// Token: 0x04000CB9 RID: 3257
		[Nullable(1)]
		private IReadOnlyList<EpochModel> _unlockedEpochs;

		// Token: 0x04000CBA RID: 3258
		[Nullable(2)]
		private Tween _cardFlyTween;

		// Token: 0x04000CBB RID: 3259
		private const double _initDelay = 0.3;

		// Token: 0x04000CBC RID: 3260
		[Nullable(1)]
		private RichTextLabel _infoLabel;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012DF RID: 4831
		public new class MethodName : NUnlockScreen.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400498D RID: 18829
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x0400498E RID: 18830
			public new static readonly StringName Open = "Open";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012E0 RID: 4832
		public new class PropertyName : NUnlockScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the '_cardFlyTween' field.
			/// </summary>
			// Token: 0x0400498F RID: 18831
			public static readonly StringName _cardFlyTween = "_cardFlyTween";

			/// <summary>
			/// Cached name for the '_infoLabel' field.
			/// </summary>
			// Token: 0x04004990 RID: 18832
			public static readonly StringName _infoLabel = "_infoLabel";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012E1 RID: 4833
		public new class SignalName : NUnlockScreen.SignalName
		{
		}
	}
}
