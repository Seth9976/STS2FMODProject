using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Rewards
{
	// Token: 0x02000372 RID: 882
	[ScriptPath("res://src/Core/Nodes/Rewards/NRewardButton.cs")]
	public class NRewardButton : NButton
	{
		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x060033EA RID: 13290 RVA: 0x00182C3E File Offset: 0x00180E3E
		// (set) Token: 0x060033EB RID: 13291 RVA: 0x00182C46 File Offset: 0x00180E46
		[Nullable(2)]
		public Reward Reward
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060033EC RID: 13292 RVA: 0x00182C4F File Offset: 0x00180E4F
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return "res://scenes/rewards/reward_button.tscn";
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x00182C56 File Offset: 0x00180E56
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NRewardButton.ScenePath);
			}
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x00182C64 File Offset: 0x00180E64
		[NullableContext(1)]
		public static NRewardButton Create(Reward reward, NRewardsScreen screen)
		{
			NRewardButton nrewardButton = PreloadManager.Cache.GetScene(NRewardButton.ScenePath).Instantiate<NRewardButton>(0L);
			nrewardButton.SetReward(reward);
			return nrewardButton;
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x00182C90 File Offset: 0x00180E90
		public override void _Ready()
		{
			this.ConnectSignals();
			this._background = base.GetNode<TextureRect>("%Background");
			this._iconContainer = base.GetNode<Control>("%Icon");
			this._label = base.GetNode<MegaRichTextLabel>("%Label");
			this._reticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this._hsv = (ShaderMaterial)this._background.Material;
			this.Reload();
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x00182D17 File Offset: 0x00180F17
		[NullableContext(1)]
		private void SetReward(Reward reward)
		{
			if (reward is LinkedRewardSet)
			{
				throw new ArgumentException("You aren't allowed to apply a RewardChainSet to a NRewardButton");
			}
			this.Reward = reward;
			if (base.IsNodeReady())
			{
				this.Reload();
			}
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x00182D44 File Offset: 0x00180F44
		private void Reload()
		{
			if (!base.IsNodeReady() || this.Reward == null)
			{
				return;
			}
			Control control = this.Reward.CreateIcon();
			this._iconContainer.AddChildSafely(control);
			control.Position = this.Reward.IconPosition;
			if (this.Reward is PotionReward)
			{
				control.Scale = 0.8f * Vector2.One;
			}
			this._label.Text = this.Reward.Description.GetFormattedText();
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x00182DC8 File Offset: 0x00180FC8
		[NullableContext(1)]
		private async Task GetReward()
		{
			base.Disable();
			bool flag = await RunManager.Instance.RewardsSetSynchronizer.SelectLocalReward(this.Reward);
			bool flag2 = flag;
			if (flag2)
			{
				if (TestMode.IsOff)
				{
					NGlobalUi globalUi = NRun.Instance.GlobalUi;
					Reward reward = this.Reward;
					RelicReward relicReward = reward as RelicReward;
					if (relicReward != null)
					{
						if (relicReward.ClaimedRelic != null)
						{
							globalUi.RelicInventory.AnimateRelic(relicReward.ClaimedRelic, new Vector2?(this._iconContainer.GlobalPosition), null);
						}
					}
					else
					{
						PotionReward potionReward = reward as PotionReward;
						if (potionReward != null)
						{
							globalUi.TopBar.PotionContainer.AnimatePotion(potionReward.ClaimedPotion, new Vector2?(this._iconContainer.GlobalPosition));
						}
					}
				}
				this._isEnabled = false;
				base.EmitSignal(NRewardButton.SignalName.RewardClaimed, new Variant[] { this });
			}
			else
			{
				base.Enable();
				this.TryGrabFocus();
				base.EmitSignal(NRewardButton.SignalName.RewardSkipped, new Variant[] { this });
			}
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x00182E0B File Offset: 0x0018100B
		protected override void OnRelease()
		{
			base.OnRelease();
			this.OnUnfocus();
			TaskHelper.RunSafely(this.GetReward());
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x00182E28 File Offset: 0x00181028
		protected override void OnPress()
		{
			base.OnPress();
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween().SetParallel(true);
			this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsvHover, this._hsvDown, 0.5).SetEase(1L).SetTrans(5L);
			this._currentTween.TweenProperty(this._label, "modulate:a", 1f, 0.5).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x00182EE4 File Offset: 0x001810E4
		protected override void OnFocus()
		{
			base.OnFocus();
			this._label.Set(NRewardButton._defaultColor, StsColors.gold);
			this._label.Set(NRewardButton._fontOutlineColor, StsColors.rewardLabelGoldOutline);
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._hsv.SetShaderParameter(NRewardButton._v, this._hsvHover);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this.Reward.HoverTips, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + Vector2.Left * 45f, false);
			}
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetAlignment(this, HoverTipAlignment.Left);
			}
			this._reticle.OnSelect();
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x00182FA0 File Offset: 0x001811A0
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this._label.Set(NRewardButton._defaultColor, StsColors.cream);
			this._label.Set(NRewardButton._fontOutlineColor, StsColors.rewardLabelOutline);
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween();
			this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsv.GetShaderParameter(NRewardButton._v), this._hsvDefault, 0.5).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
			this._reticle.OnDeselect();
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x0018305F File Offset: 0x0018125F
		private void UpdateShaderParam(float value)
		{
			this._hsv.SetShaderParameter(NRewardButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060033F8 RID: 13304 RVA: 0x00183078 File Offset: 0x00181278
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NRewardButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardButton.MethodName.Reload, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardButton.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060033F9 RID: 13305 RVA: 0x001831FC File Offset: 0x001813FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRewardButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardButton.MethodName.Reload && args.Count == 0)
			{
				this.Reload();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardButton.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060033FA RID: 13306 RVA: 0x00183324 File Offset: 0x00181524
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRewardButton.MethodName._Ready || (ref method) == NRewardButton.MethodName.Reload || (ref method) == NRewardButton.MethodName.OnRelease || (ref method) == NRewardButton.MethodName.OnPress || (ref method) == NRewardButton.MethodName.OnFocus || (ref method) == NRewardButton.MethodName.OnUnfocus || (ref method) == NRewardButton.MethodName.UpdateShaderParam || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060033FB RID: 13307 RVA: 0x001833A4 File Offset: 0x001815A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRewardButton.PropertyName._background)
			{
				this._background = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._iconContainer)
			{
				this._iconContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._reticle)
			{
				this._reticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._currentTween)
			{
				this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._hsvDefault)
			{
				this._hsvDefault = VariantUtils.ConvertTo<Variant>(ref value);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._hsvHover)
			{
				this._hsvHover = VariantUtils.ConvertTo<Variant>(ref value);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._hsvDown)
			{
				this._hsvDown = VariantUtils.ConvertTo<Variant>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060033FC RID: 13308 RVA: 0x001834AC File Offset: 0x001816AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRewardButton.PropertyName._background)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._background);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._iconContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._iconContainer);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._reticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._reticle);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._currentTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._hsvDefault)
			{
				value = VariantUtils.CreateFrom<Variant>(ref this._hsvDefault);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._hsvHover)
			{
				value = VariantUtils.CreateFrom<Variant>(ref this._hsvHover);
				return true;
			}
			if ((ref name) == NRewardButton.PropertyName._hsvDown)
			{
				value = VariantUtils.CreateFrom<Variant>(ref this._hsvDown);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060033FD RID: 13309 RVA: 0x001835E4 File Offset: 0x001817E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRewardButton.PropertyName._background, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardButton.PropertyName._iconContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardButton.PropertyName._reticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardButton.PropertyName._currentTween, 0L, "", 4096L, false),
				new PropertyInfo(0L, NRewardButton.PropertyName._hsvDefault, 0L, "", 4096L, false),
				new PropertyInfo(0L, NRewardButton.PropertyName._hsvHover, 0L, "", 4096L, false),
				new PropertyInfo(0L, NRewardButton.PropertyName._hsvDown, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060033FE RID: 13310 RVA: 0x00183720 File Offset: 0x00181920
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRewardButton.PropertyName._background, Variant.From<TextureRect>(ref this._background));
			info.AddProperty(NRewardButton.PropertyName._iconContainer, Variant.From<Control>(ref this._iconContainer));
			info.AddProperty(NRewardButton.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NRewardButton.PropertyName._reticle, Variant.From<NSelectionReticle>(ref this._reticle));
			info.AddProperty(NRewardButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NRewardButton.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
			info.AddProperty(NRewardButton.PropertyName._hsvDefault, Variant.From<Variant>(ref this._hsvDefault));
			info.AddProperty(NRewardButton.PropertyName._hsvHover, Variant.From<Variant>(ref this._hsvHover));
			info.AddProperty(NRewardButton.PropertyName._hsvDown, Variant.From<Variant>(ref this._hsvDown));
			info.AddSignalEventDelegate(NRewardButton.SignalName.RewardClaimed, this.backing_RewardClaimed);
			info.AddSignalEventDelegate(NRewardButton.SignalName.RewardSkipped, this.backing_RewardSkipped);
		}

		/// <inheritdoc />
		// Token: 0x060033FF RID: 13311 RVA: 0x0018381C File Offset: 0x00181A1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRewardButton.PropertyName._background, ref variant))
			{
				this._background = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRewardButton.PropertyName._iconContainer, ref variant2))
			{
				this._iconContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRewardButton.PropertyName._label, ref variant3))
			{
				this._label = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRewardButton.PropertyName._reticle, ref variant4))
			{
				this._reticle = variant4.As<NSelectionReticle>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRewardButton.PropertyName._hsv, ref variant5))
			{
				this._hsv = variant5.As<ShaderMaterial>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRewardButton.PropertyName._currentTween, ref variant6))
			{
				this._currentTween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRewardButton.PropertyName._hsvDefault, ref variant7))
			{
				this._hsvDefault = variant7.As<Variant>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRewardButton.PropertyName._hsvHover, ref variant8))
			{
				this._hsvHover = variant8.As<Variant>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRewardButton.PropertyName._hsvDown, ref variant9))
			{
				this._hsvDown = variant9.As<Variant>();
			}
			NRewardButton.RewardClaimedEventHandler rewardClaimedEventHandler;
			if (info.TryGetSignalEventDelegate<NRewardButton.RewardClaimedEventHandler>(NRewardButton.SignalName.RewardClaimed, ref rewardClaimedEventHandler))
			{
				this.backing_RewardClaimed = rewardClaimedEventHandler;
			}
			NRewardButton.RewardSkippedEventHandler rewardSkippedEventHandler;
			if (info.TryGetSignalEventDelegate<NRewardButton.RewardSkippedEventHandler>(NRewardButton.SignalName.RewardSkipped, ref rewardSkippedEventHandler))
			{
				this.backing_RewardSkipped = rewardSkippedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003400 RID: 13312 RVA: 0x0018395C File Offset: 0x00181B5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NRewardButton.SignalName.RewardClaimed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRewardButton.SignalName.RewardSkipped, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Rewards.NRewardButton.RewardClaimedEventHandler" />
		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06003401 RID: 13313 RVA: 0x00183A2F File Offset: 0x00181C2F
		// (remove) Token: 0x06003402 RID: 13314 RVA: 0x00183A48 File Offset: 0x00181C48
		public event NRewardButton.RewardClaimedEventHandler RewardClaimed
		{
			add
			{
				this.backing_RewardClaimed = (NRewardButton.RewardClaimedEventHandler)Delegate.Combine(this.backing_RewardClaimed, value);
			}
			remove
			{
				this.backing_RewardClaimed = (NRewardButton.RewardClaimedEventHandler)Delegate.Remove(this.backing_RewardClaimed, value);
			}
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x00183A61 File Offset: 0x00181C61
		protected void EmitSignalRewardClaimed(NRewardButton button)
		{
			base.EmitSignal(NRewardButton.SignalName.RewardClaimed, new Variant[] { button });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Rewards.NRewardButton.RewardSkippedEventHandler" />
		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06003404 RID: 13316 RVA: 0x00183A82 File Offset: 0x00181C82
		// (remove) Token: 0x06003405 RID: 13317 RVA: 0x00183A9B File Offset: 0x00181C9B
		public event NRewardButton.RewardSkippedEventHandler RewardSkipped
		{
			add
			{
				this.backing_RewardSkipped = (NRewardButton.RewardSkippedEventHandler)Delegate.Combine(this.backing_RewardSkipped, value);
			}
			remove
			{
				this.backing_RewardSkipped = (NRewardButton.RewardSkippedEventHandler)Delegate.Remove(this.backing_RewardSkipped, value);
			}
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x00183AB4 File Offset: 0x00181CB4
		protected void EmitSignalRewardSkipped(NRewardButton button)
		{
			base.EmitSignal(NRewardButton.SignalName.RewardSkipped, new Variant[] { button });
		}

		/// <inheritdoc />
		// Token: 0x06003407 RID: 13319 RVA: 0x00183AD8 File Offset: 0x00181CD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NRewardButton.SignalName.RewardClaimed && args.Count == 1)
			{
				NRewardButton.RewardClaimedEventHandler rewardClaimedEventHandler = this.backing_RewardClaimed;
				if (rewardClaimedEventHandler == null)
				{
					return;
				}
				rewardClaimedEventHandler(VariantUtils.ConvertTo<NRewardButton>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NRewardButton.SignalName.RewardSkipped) || args.Count != 1)
				{
					base.RaiseGodotClassSignalCallbacks(in signal, args);
					return;
				}
				NRewardButton.RewardSkippedEventHandler rewardSkippedEventHandler = this.backing_RewardSkipped;
				if (rewardSkippedEventHandler == null)
				{
					return;
				}
				rewardSkippedEventHandler(VariantUtils.ConvertTo<NRewardButton>(args[0]));
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x06003408 RID: 13320 RVA: 0x00183B57 File Offset: 0x00181D57
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NRewardButton.SignalName.RewardClaimed || (ref signal) == NRewardButton.SignalName.RewardSkipped || base.HasGodotClassSignal(in signal);
		}

		// Token: 0x0400141E RID: 5150
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x0400141F RID: 5151
		[Nullable(1)]
		private static readonly StringName _fontOutlineColor = new StringName("theme_override_colors/font_outline_color");

		// Token: 0x04001420 RID: 5152
		[Nullable(1)]
		private static readonly StringName _defaultColor = new StringName("theme_override_colors/default_color");

		// Token: 0x04001421 RID: 5153
		[Nullable(1)]
		private TextureRect _background;

		// Token: 0x04001422 RID: 5154
		[Nullable(1)]
		private Control _iconContainer;

		// Token: 0x04001423 RID: 5155
		[Nullable(1)]
		private MegaRichTextLabel _label;

		// Token: 0x04001424 RID: 5156
		[Nullable(1)]
		private NSelectionReticle _reticle;

		// Token: 0x04001425 RID: 5157
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04001426 RID: 5158
		[Nullable(2)]
		private Tween _currentTween;

		// Token: 0x04001427 RID: 5159
		private Variant _hsvDefault = 0.9;

		// Token: 0x04001428 RID: 5160
		private Variant _hsvHover = 1.1;

		// Token: 0x04001429 RID: 5161
		private Variant _hsvDown = 0.7;

		// Token: 0x0400142B RID: 5163
		private NRewardButton.RewardClaimedEventHandler backing_RewardClaimed;

		// Token: 0x0400142C RID: 5164
		private NRewardButton.RewardSkippedEventHandler backing_RewardSkipped;

		// Token: 0x020016AF RID: 5807
		// (Invoke) Token: 0x0600A53D RID: 42301
		[Signal]
		public delegate void RewardClaimedEventHandler(NRewardButton button);

		// Token: 0x020016B0 RID: 5808
		// (Invoke) Token: 0x0600A541 RID: 42305
		[Signal]
		public delegate void RewardSkippedEventHandler(NRewardButton button);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016B1 RID: 5809
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400586A RID: 22634
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Reload' method.
			/// </summary>
			// Token: 0x0400586B RID: 22635
			public static readonly StringName Reload = "Reload";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x0400586C RID: 22636
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400586D RID: 22637
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400586E RID: 22638
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400586F RID: 22639
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x04005870 RID: 22640
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016B2 RID: 5810
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_background' field.
			/// </summary>
			// Token: 0x04005871 RID: 22641
			public static readonly StringName _background = "_background";

			/// <summary>
			/// Cached name for the '_iconContainer' field.
			/// </summary>
			// Token: 0x04005872 RID: 22642
			public static readonly StringName _iconContainer = "_iconContainer";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005873 RID: 22643
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_reticle' field.
			/// </summary>
			// Token: 0x04005874 RID: 22644
			public static readonly StringName _reticle = "_reticle";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005875 RID: 22645
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_currentTween' field.
			/// </summary>
			// Token: 0x04005876 RID: 22646
			public static readonly StringName _currentTween = "_currentTween";

			/// <summary>
			/// Cached name for the '_hsvDefault' field.
			/// </summary>
			// Token: 0x04005877 RID: 22647
			public static readonly StringName _hsvDefault = "_hsvDefault";

			/// <summary>
			/// Cached name for the '_hsvHover' field.
			/// </summary>
			// Token: 0x04005878 RID: 22648
			public static readonly StringName _hsvHover = "_hsvHover";

			/// <summary>
			/// Cached name for the '_hsvDown' field.
			/// </summary>
			// Token: 0x04005879 RID: 22649
			public static readonly StringName _hsvDown = "_hsvDown";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016B3 RID: 5811
		public new class SignalName : NButton.SignalName
		{
			/// <summary>
			/// Cached name for the 'RewardClaimed' signal.
			/// </summary>
			// Token: 0x0400587A RID: 22650
			public static readonly StringName RewardClaimed = "RewardClaimed";

			/// <summary>
			/// Cached name for the 'RewardSkipped' signal.
			/// </summary>
			// Token: 0x0400587B RID: 22651
			public static readonly StringName RewardSkipped = "RewardSkipped";
		}
	}
}
