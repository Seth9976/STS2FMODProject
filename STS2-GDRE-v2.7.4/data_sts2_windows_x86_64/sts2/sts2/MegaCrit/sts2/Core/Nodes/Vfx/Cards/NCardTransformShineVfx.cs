using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	// Token: 0x02000235 RID: 565
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NCardTransformShineVfx.cs")]
	public class NCardTransformShineVfx : Control
	{
		// Token: 0x06001CB6 RID: 7350 RVA: 0x000BA638 File Offset: 0x000B8838
		[return: Nullable(2)]
		public static NCardTransformShineVfx Create(NCard cardNode, CardModel endCard, [Nullable(new byte[] { 2, 1 })] IEnumerable<RelicModel> relicsToFlash)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCardTransformShineVfx ncardTransformShineVfx = PreloadManager.Cache.GetScene(NCardTransformShineVfx.scenePath).Instantiate<NCardTransformShineVfx>(0L);
			ncardTransformShineVfx._cardNode = cardNode;
			ncardTransformShineVfx._endCard = endCard;
			ncardTransformShineVfx._relicsToFlash = relicsToFlash;
			cardNode.CardVfxContainer.AddChildSafely(ncardTransformShineVfx);
			return ncardTransformShineVfx;
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x000BA687 File Offset: 0x000B8887
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x000BA69C File Offset: 0x000B889C
		private async Task<bool> WaitAndInterruptIfNecessary(float seconds, NCard cardNode)
		{
			float num2;
			float num3;
			for (float num = 0f; num <= seconds; num = num2 + num3)
			{
				if (!cardNode.IsInsideTree() || this._endCard.Pile == null)
				{
					return false;
				}
				num2 = num;
				num3 = await this.AwaitProcessFrame(default(CancellationToken));
			}
			return true;
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x000BA6F0 File Offset: 0x000B88F0
		private static void UpdateCard(NCard cardNode, CardModel endCard)
		{
			if (endCard.Pile == null)
			{
				return;
			}
			NPlayerHand instance = NPlayerHand.Instance;
			if (instance != null)
			{
				instance.TryCancelCardPlay(cardNode.Model);
			}
			cardNode.Model = endCard;
			cardNode.UpdateVisuals(endCard.Pile.Type, CardPreviewMode.Normal);
			NCombatRoom instance2 = NCombatRoom.Instance;
			NHandCardHolder nhandCardHolder = ((instance2 != null) ? instance2.Ui.Hand.GetCardHolder(endCard) : null) as NHandCardHolder;
			if (nhandCardHolder != null)
			{
				nhandCardHolder.UpdateCard();
			}
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000BA760 File Offset: 0x000B8960
		public async Task PlayUntilCardUpdate(bool shortVersion = false)
		{
			this._overlay.SelfModulate = this._whiteClear;
			this._borderGlow.SelfModulate = this._whiteClear;
			float num = (shortVersion ? this._overlayShowShortDuration : this._overlayShowDuration);
			float num2 = (shortVersion ? this._overlayIdleShortDuration : this._overlayIdleDuration);
			TaskHelper.RunSafely(this.AnimatingCardScale(this._anticipationScaleCurve, num + num2));
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this._overlay, "self_modulate", this._whiteOpaque, (double)num).SetEase(2L).SetTrans(3L);
			TaskAwaiter<bool> taskAwaiter = this.WaitAndInterruptIfNecessary(num + num2, this._cardNode).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				this._cardNode.Scale = NCardTransformShineVfx._originalCardScale;
				this.QueueFreeSafely();
			}
			else
			{
				NCardTransformShineVfx.UpdateCard(this._cardNode, this._endCard);
			}
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x000BA7AC File Offset: 0x000B89AC
		public async Task PlayShineAndReveal()
		{
			TaskHelper.RunSafely(this.AnimatingCardScale(this._revealScaleCurve, this._glowFadeDuration));
			this._borderGlow.SelfModulate = this._whiteOpaque;
			this._borderGlow.Scale = Vector2.One;
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._overlay, "self_modulate", this._whiteClear, (double)this._overlayHideDuration).SetEase(1L).SetTrans(3L);
			this._tween.TweenProperty(this._borderGlow, "scale", Vector2.One * this._glowTopScale, (double)this._glowFadeDuration).SetEase(1L).SetTrans(2L);
			this._tween.TweenProperty(this._borderGlow, "self_modulate", this._whiteClear, (double)this._glowFadeDuration).SetEase(1L).SetTrans(2L);
			if (this._relicsToFlash != null)
			{
				foreach (RelicModel relicModel in this._relicsToFlash)
				{
					relicModel.Flash();
					this._cardNode.FlashRelicOnCard(relicModel);
				}
			}
			this._revealParticles.Restart();
			TaskAwaiter<bool> taskAwaiter = this.WaitAndInterruptIfNecessary(this._shineDelay, this._cardNode).GetAwaiter();
			TaskAwaiter<bool> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				this._cardNode.Scale = NCardTransformShineVfx._originalCardScale;
				this.QueueFreeSafely();
			}
			else
			{
				this._shineParticles.Restart();
				TaskAwaiter<bool> taskAwaiter3 = this.WaitAndInterruptIfNecessary(this._endParticlesDelay, this._cardNode).GetAwaiter();
				if (!taskAwaiter3.IsCompleted)
				{
					await taskAwaiter3;
					taskAwaiter3 = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter3.GetResult())
				{
					this._cardNode.Scale = NCardTransformShineVfx._originalCardScale;
					this.QueueFreeSafely();
				}
				else
				{
					this._endParticles.Restart();
					TaskHelper.RunSafely(this.DelayedFree());
				}
			}
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x000BA7F0 File Offset: 0x000B89F0
		public async Task PlayAnimation(bool shortVersion = false)
		{
			await this.PlayUntilCardUpdate(shortVersion);
			await this.PlayShineAndReveal();
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x000BA83C File Offset: 0x000B8A3C
		public async Task PlayAnimationWithoutWaitingForEnd(bool shortVersion = false)
		{
			await this.PlayUntilCardUpdate(shortVersion);
			TaskHelper.RunSafely(this.PlayShineAndReveal());
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000BA888 File Offset: 0x000B8A88
		private async Task DelayedFree()
		{
			await Cmd.Wait(2f, false);
			this.QueueFreeSafely();
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x000BA8CC File Offset: 0x000B8ACC
		private async Task AnimatingCardScale(CurveXyzTexture curve, float duration)
		{
			float num = 0f;
			Vector2 one = Vector2.One;
			while (num < duration)
			{
				float num2 = num / duration;
				float num3 = curve.CurveX.Sample(num2);
				float num4 = curve.CurveY.Sample(num2);
				one.X = num3;
				one.Y = num4;
				this._cardNode.Scale = NCardTransformShineVfx._originalCardScale * one;
				float num5 = num;
				float num6 = await this.AwaitProcessFrame(default(CancellationToken));
				num = num5 + num6;
			}
			one.X = curve.CurveX.Sample(1f);
			one.Y = curve.CurveY.Sample(1f);
			this._cardNode.Scale = NCardTransformShineVfx._originalCardScale * one;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001CC0 RID: 7360 RVA: 0x000BA920 File Offset: 0x000B8B20
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NCardTransformShineVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CC1 RID: 7361 RVA: 0x000BA964 File Offset: 0x000B8B64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardTransformShineVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001CC2 RID: 7362 RVA: 0x000BA994 File Offset: 0x000B8B94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardTransformShineVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001CC3 RID: 7363 RVA: 0x000BA9AC File Offset: 0x000B8BAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlay)
			{
				this._overlay = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._borderGlow)
			{
				this._borderGlow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._revealParticles)
			{
				this._revealParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._shineParticles)
			{
				this._shineParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._endParticles)
			{
				this._endParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._anticipationScaleCurve)
			{
				this._anticipationScaleCurve = VariantUtils.ConvertTo<CurveXyzTexture>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._revealScaleCurve)
			{
				this._revealScaleCurve = VariantUtils.ConvertTo<CurveXyzTexture>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlayShowDuration)
			{
				this._overlayShowDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlayShowShortDuration)
			{
				this._overlayShowShortDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlayIdleDuration)
			{
				this._overlayIdleDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlayIdleShortDuration)
			{
				this._overlayIdleShortDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlayHideDuration)
			{
				this._overlayHideDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._glowFadeDuration)
			{
				this._glowFadeDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._glowTopScale)
			{
				this._glowTopScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._shineDelay)
			{
				this._shineDelay = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._endParticlesDelay)
			{
				this._endParticlesDelay = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._cardNode)
			{
				this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._whiteOpaque)
			{
				this._whiteOpaque = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._whiteClear)
			{
				this._whiteClear = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001CC4 RID: 7364 RVA: 0x000BABE0 File Offset: 0x000B8DE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlay)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._overlay);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._borderGlow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._borderGlow);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._revealParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._revealParticles);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._shineParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._shineParticles);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._endParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._endParticles);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._anticipationScaleCurve)
			{
				value = VariantUtils.CreateFrom<CurveXyzTexture>(ref this._anticipationScaleCurve);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._revealScaleCurve)
			{
				value = VariantUtils.CreateFrom<CurveXyzTexture>(ref this._revealScaleCurve);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlayShowDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._overlayShowDuration);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlayShowShortDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._overlayShowShortDuration);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlayIdleDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._overlayIdleDuration);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlayIdleShortDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._overlayIdleShortDuration);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._overlayHideDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._overlayHideDuration);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._glowFadeDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._glowFadeDuration);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._glowTopScale)
			{
				value = VariantUtils.CreateFrom<float>(ref this._glowTopScale);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._shineDelay)
			{
				value = VariantUtils.CreateFrom<float>(ref this._shineDelay);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._endParticlesDelay)
			{
				value = VariantUtils.CreateFrom<float>(ref this._endParticlesDelay);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._cardNode)
			{
				value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._whiteOpaque)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._whiteOpaque);
				return true;
			}
			if ((ref name) == NCardTransformShineVfx.PropertyName._whiteClear)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._whiteClear);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001CC5 RID: 7365 RVA: 0x000BAE78 File Offset: 0x000B9078
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardTransformShineVfx.PropertyName._overlay, 34L, "Control", 4102L, true),
				new PropertyInfo(24L, NCardTransformShineVfx.PropertyName._borderGlow, 34L, "Control", 4102L, true),
				new PropertyInfo(24L, NCardTransformShineVfx.PropertyName._revealParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCardTransformShineVfx.PropertyName._shineParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCardTransformShineVfx.PropertyName._endParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCardTransformShineVfx.PropertyName._anticipationScaleCurve, 17L, "CurveXYZTexture", 4102L, true),
				new PropertyInfo(24L, NCardTransformShineVfx.PropertyName._revealScaleCurve, 17L, "CurveXYZTexture", 4102L, true),
				new PropertyInfo(3L, NCardTransformShineVfx.PropertyName._overlayShowDuration, 0L, "", 4102L, true),
				new PropertyInfo(3L, NCardTransformShineVfx.PropertyName._overlayShowShortDuration, 0L, "", 4102L, true),
				new PropertyInfo(3L, NCardTransformShineVfx.PropertyName._overlayIdleDuration, 0L, "", 4102L, true),
				new PropertyInfo(3L, NCardTransformShineVfx.PropertyName._overlayIdleShortDuration, 0L, "", 4102L, true),
				new PropertyInfo(3L, NCardTransformShineVfx.PropertyName._overlayHideDuration, 0L, "", 4102L, true),
				new PropertyInfo(3L, NCardTransformShineVfx.PropertyName._glowFadeDuration, 0L, "", 4102L, true),
				new PropertyInfo(3L, NCardTransformShineVfx.PropertyName._glowTopScale, 0L, "", 4102L, true),
				new PropertyInfo(3L, NCardTransformShineVfx.PropertyName._shineDelay, 0L, "", 4102L, true),
				new PropertyInfo(3L, NCardTransformShineVfx.PropertyName._endParticlesDelay, 0L, "", 4102L, true),
				new PropertyInfo(24L, NCardTransformShineVfx.PropertyName._cardNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardTransformShineVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(20L, NCardTransformShineVfx.PropertyName._whiteOpaque, 0L, "", 4096L, false),
				new PropertyInfo(20L, NCardTransformShineVfx.PropertyName._whiteClear, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CC6 RID: 7366 RVA: 0x000BB120 File Offset: 0x000B9320
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardTransformShineVfx.PropertyName._overlay, Variant.From<Control>(ref this._overlay));
			info.AddProperty(NCardTransformShineVfx.PropertyName._borderGlow, Variant.From<Control>(ref this._borderGlow));
			info.AddProperty(NCardTransformShineVfx.PropertyName._revealParticles, Variant.From<NParticlesContainer>(ref this._revealParticles));
			info.AddProperty(NCardTransformShineVfx.PropertyName._shineParticles, Variant.From<NParticlesContainer>(ref this._shineParticles));
			info.AddProperty(NCardTransformShineVfx.PropertyName._endParticles, Variant.From<NParticlesContainer>(ref this._endParticles));
			info.AddProperty(NCardTransformShineVfx.PropertyName._anticipationScaleCurve, Variant.From<CurveXyzTexture>(ref this._anticipationScaleCurve));
			info.AddProperty(NCardTransformShineVfx.PropertyName._revealScaleCurve, Variant.From<CurveXyzTexture>(ref this._revealScaleCurve));
			info.AddProperty(NCardTransformShineVfx.PropertyName._overlayShowDuration, Variant.From<float>(ref this._overlayShowDuration));
			info.AddProperty(NCardTransformShineVfx.PropertyName._overlayShowShortDuration, Variant.From<float>(ref this._overlayShowShortDuration));
			info.AddProperty(NCardTransformShineVfx.PropertyName._overlayIdleDuration, Variant.From<float>(ref this._overlayIdleDuration));
			info.AddProperty(NCardTransformShineVfx.PropertyName._overlayIdleShortDuration, Variant.From<float>(ref this._overlayIdleShortDuration));
			info.AddProperty(NCardTransformShineVfx.PropertyName._overlayHideDuration, Variant.From<float>(ref this._overlayHideDuration));
			info.AddProperty(NCardTransformShineVfx.PropertyName._glowFadeDuration, Variant.From<float>(ref this._glowFadeDuration));
			info.AddProperty(NCardTransformShineVfx.PropertyName._glowTopScale, Variant.From<float>(ref this._glowTopScale));
			info.AddProperty(NCardTransformShineVfx.PropertyName._shineDelay, Variant.From<float>(ref this._shineDelay));
			info.AddProperty(NCardTransformShineVfx.PropertyName._endParticlesDelay, Variant.From<float>(ref this._endParticlesDelay));
			info.AddProperty(NCardTransformShineVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
			info.AddProperty(NCardTransformShineVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NCardTransformShineVfx.PropertyName._whiteOpaque, Variant.From<Color>(ref this._whiteOpaque));
			info.AddProperty(NCardTransformShineVfx.PropertyName._whiteClear, Variant.From<Color>(ref this._whiteClear));
		}

		/// <inheritdoc />
		// Token: 0x06001CC7 RID: 7367 RVA: 0x000BB2EC File Offset: 0x000B94EC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlay, ref variant))
			{
				this._overlay = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._borderGlow, ref variant2))
			{
				this._borderGlow = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._revealParticles, ref variant3))
			{
				this._revealParticles = variant3.As<NParticlesContainer>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._shineParticles, ref variant4))
			{
				this._shineParticles = variant4.As<NParticlesContainer>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._endParticles, ref variant5))
			{
				this._endParticles = variant5.As<NParticlesContainer>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._anticipationScaleCurve, ref variant6))
			{
				this._anticipationScaleCurve = variant6.As<CurveXyzTexture>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._revealScaleCurve, ref variant7))
			{
				this._revealScaleCurve = variant7.As<CurveXyzTexture>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlayShowDuration, ref variant8))
			{
				this._overlayShowDuration = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlayShowShortDuration, ref variant9))
			{
				this._overlayShowShortDuration = variant9.As<float>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlayIdleDuration, ref variant10))
			{
				this._overlayIdleDuration = variant10.As<float>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlayIdleShortDuration, ref variant11))
			{
				this._overlayIdleShortDuration = variant11.As<float>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._overlayHideDuration, ref variant12))
			{
				this._overlayHideDuration = variant12.As<float>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._glowFadeDuration, ref variant13))
			{
				this._glowFadeDuration = variant13.As<float>();
			}
			Variant variant14;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._glowTopScale, ref variant14))
			{
				this._glowTopScale = variant14.As<float>();
			}
			Variant variant15;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._shineDelay, ref variant15))
			{
				this._shineDelay = variant15.As<float>();
			}
			Variant variant16;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._endParticlesDelay, ref variant16))
			{
				this._endParticlesDelay = variant16.As<float>();
			}
			Variant variant17;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._cardNode, ref variant17))
			{
				this._cardNode = variant17.As<NCard>();
			}
			Variant variant18;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._tween, ref variant18))
			{
				this._tween = variant18.As<Tween>();
			}
			Variant variant19;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._whiteOpaque, ref variant19))
			{
				this._whiteOpaque = variant19.As<Color>();
			}
			Variant variant20;
			if (info.TryGetProperty(NCardTransformShineVfx.PropertyName._whiteClear, ref variant20))
			{
				this._whiteClear = variant20.As<Color>();
			}
		}

		// Token: 0x04000AFC RID: 2812
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/ui/card/vfx_card_transform");

		// Token: 0x04000AFD RID: 2813
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Control _overlay;

		// Token: 0x04000AFE RID: 2814
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Control _borderGlow;

		// Token: 0x04000AFF RID: 2815
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _revealParticles;

		// Token: 0x04000B00 RID: 2816
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _shineParticles;

		// Token: 0x04000B01 RID: 2817
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _endParticles;

		// Token: 0x04000B02 RID: 2818
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private CurveXyzTexture _anticipationScaleCurve;

		// Token: 0x04000B03 RID: 2819
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private CurveXyzTexture _revealScaleCurve;

		// Token: 0x04000B04 RID: 2820
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _overlayShowDuration = 0.75f;

		// Token: 0x04000B05 RID: 2821
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _overlayShowShortDuration = 0.75f;

		// Token: 0x04000B06 RID: 2822
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _overlayIdleDuration = 0.125f;

		// Token: 0x04000B07 RID: 2823
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _overlayIdleShortDuration = 0.75f;

		// Token: 0x04000B08 RID: 2824
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _overlayHideDuration = 0.25f;

		// Token: 0x04000B09 RID: 2825
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _glowFadeDuration = 0.25f;

		// Token: 0x04000B0A RID: 2826
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _glowTopScale = 1.5f;

		// Token: 0x04000B0B RID: 2827
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _shineDelay = 0.125f;

		// Token: 0x04000B0C RID: 2828
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _endParticlesDelay = 0.4f;

		// Token: 0x04000B0D RID: 2829
		private NCard _cardNode;

		// Token: 0x04000B0E RID: 2830
		private CardModel _endCard;

		// Token: 0x04000B0F RID: 2831
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<RelicModel> _relicsToFlash;

		// Token: 0x04000B10 RID: 2832
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000B11 RID: 2833
		private Color _whiteOpaque = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04000B12 RID: 2834
		private Color _whiteClear = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04000B13 RID: 2835
		private static Vector2 _originalCardScale = new Vector2(1f, 1f);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001201 RID: 4609
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040045CB RID: 17867
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001202 RID: 4610
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_overlay' field.
			/// </summary>
			// Token: 0x040045CC RID: 17868
			public static readonly StringName _overlay = "_overlay";

			/// <summary>
			/// Cached name for the '_borderGlow' field.
			/// </summary>
			// Token: 0x040045CD RID: 17869
			public static readonly StringName _borderGlow = "_borderGlow";

			/// <summary>
			/// Cached name for the '_revealParticles' field.
			/// </summary>
			// Token: 0x040045CE RID: 17870
			public static readonly StringName _revealParticles = "_revealParticles";

			/// <summary>
			/// Cached name for the '_shineParticles' field.
			/// </summary>
			// Token: 0x040045CF RID: 17871
			public static readonly StringName _shineParticles = "_shineParticles";

			/// <summary>
			/// Cached name for the '_endParticles' field.
			/// </summary>
			// Token: 0x040045D0 RID: 17872
			public static readonly StringName _endParticles = "_endParticles";

			/// <summary>
			/// Cached name for the '_anticipationScaleCurve' field.
			/// </summary>
			// Token: 0x040045D1 RID: 17873
			public static readonly StringName _anticipationScaleCurve = "_anticipationScaleCurve";

			/// <summary>
			/// Cached name for the '_revealScaleCurve' field.
			/// </summary>
			// Token: 0x040045D2 RID: 17874
			public static readonly StringName _revealScaleCurve = "_revealScaleCurve";

			/// <summary>
			/// Cached name for the '_overlayShowDuration' field.
			/// </summary>
			// Token: 0x040045D3 RID: 17875
			public static readonly StringName _overlayShowDuration = "_overlayShowDuration";

			/// <summary>
			/// Cached name for the '_overlayShowShortDuration' field.
			/// </summary>
			// Token: 0x040045D4 RID: 17876
			public static readonly StringName _overlayShowShortDuration = "_overlayShowShortDuration";

			/// <summary>
			/// Cached name for the '_overlayIdleDuration' field.
			/// </summary>
			// Token: 0x040045D5 RID: 17877
			public static readonly StringName _overlayIdleDuration = "_overlayIdleDuration";

			/// <summary>
			/// Cached name for the '_overlayIdleShortDuration' field.
			/// </summary>
			// Token: 0x040045D6 RID: 17878
			public static readonly StringName _overlayIdleShortDuration = "_overlayIdleShortDuration";

			/// <summary>
			/// Cached name for the '_overlayHideDuration' field.
			/// </summary>
			// Token: 0x040045D7 RID: 17879
			public static readonly StringName _overlayHideDuration = "_overlayHideDuration";

			/// <summary>
			/// Cached name for the '_glowFadeDuration' field.
			/// </summary>
			// Token: 0x040045D8 RID: 17880
			public static readonly StringName _glowFadeDuration = "_glowFadeDuration";

			/// <summary>
			/// Cached name for the '_glowTopScale' field.
			/// </summary>
			// Token: 0x040045D9 RID: 17881
			public static readonly StringName _glowTopScale = "_glowTopScale";

			/// <summary>
			/// Cached name for the '_shineDelay' field.
			/// </summary>
			// Token: 0x040045DA RID: 17882
			public static readonly StringName _shineDelay = "_shineDelay";

			/// <summary>
			/// Cached name for the '_endParticlesDelay' field.
			/// </summary>
			// Token: 0x040045DB RID: 17883
			public static readonly StringName _endParticlesDelay = "_endParticlesDelay";

			/// <summary>
			/// Cached name for the '_cardNode' field.
			/// </summary>
			// Token: 0x040045DC RID: 17884
			public static readonly StringName _cardNode = "_cardNode";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040045DD RID: 17885
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_whiteOpaque' field.
			/// </summary>
			// Token: 0x040045DE RID: 17886
			public static readonly StringName _whiteOpaque = "_whiteOpaque";

			/// <summary>
			/// Cached name for the '_whiteClear' field.
			/// </summary>
			// Token: 0x040045DF RID: 17887
			public static readonly StringName _whiteClear = "_whiteClear";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001203 RID: 4611
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
