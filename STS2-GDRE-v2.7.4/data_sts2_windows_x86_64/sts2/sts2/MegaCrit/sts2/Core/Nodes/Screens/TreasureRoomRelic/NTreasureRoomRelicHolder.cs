using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic
{
	// Token: 0x02000256 RID: 598
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/TreasureRoomRelic/NTreasureRoomRelicHolder.cs")]
	public class NTreasureRoomRelicHolder : NButton
	{
		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x000CF23B File Offset: 0x000CD43B
		// (set) Token: 0x06001F28 RID: 7976 RVA: 0x000CF243 File Offset: 0x000CD443
		public int Index { get; set; }

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001F29 RID: 7977 RVA: 0x000CF24C File Offset: 0x000CD44C
		// (set) Token: 0x06001F2A RID: 7978 RVA: 0x000CF254 File Offset: 0x000CD454
		public NMultiplayerVoteContainer VoteContainer { get; private set; }

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x000CF25D File Offset: 0x000CD45D
		// (set) Token: 0x06001F2C RID: 7980 RVA: 0x000CF265 File Offset: 0x000CD465
		public NRelic Relic { get; private set; }

		// Token: 0x06001F2D RID: 7981 RVA: 0x000CF270 File Offset: 0x000CD470
		public override void _Ready()
		{
			this.ConnectSignals();
			this.VoteContainer = base.GetNode<NMultiplayerVoteContainer>("%MultiplayerVoteContainer");
			this.Relic = base.GetNode<NRelic>("%Relic");
			this._uncommonGlow = base.GetNode<GpuParticles2D>("%UncommonGlow");
			this._rareGlow = base.GetNode<GpuParticles2D>("%RareGlow");
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x000CF2DC File Offset: 0x000CD4DC
		public void Initialize(RelicModel relic, IRunState runState)
		{
			this.Relic.Model = relic;
			this.VoteContainer.Initialize(new NMultiplayerVoteContainer.PlayerVotedDelegate(this.PlayerVotedForRelic), runState.Players);
			this.Relic.Modulate = StsColors.transparentBlack;
			Tween initTween = this._initTween;
			if (initTween != null)
			{
				initTween.Kill();
			}
			this._initTween = base.CreateTween().SetParallel(true);
			this._initTween.TweenProperty(this.Relic, "modulate", Colors.White, 0.25);
			if (this.Relic.Model.Rarity == RelicRarity.Uncommon)
			{
				Tween tween = base.CreateTween().SetParallel(true);
				this._uncommonGlow.Visible = true;
				this._uncommonGlow.Modulate = StsColors.transparentWhite;
				this._uncommonGlow.GlobalPosition = this.Relic.GlobalPosition + Vector2.One * 68f;
				tween.TweenProperty(this._uncommonGlow, "modulate", Colors.White, 0.25);
				return;
			}
			if (this.Relic.Model.Rarity == RelicRarity.Rare)
			{
				Tween tween2 = base.CreateTween().SetParallel(true);
				this._rareGlow.Visible = true;
				this._rareGlow.Modulate = StsColors.transparentWhite;
				this._rareGlow.GlobalPosition = this.Relic.GlobalPosition + Vector2.One * 68f;
				tween2.TweenProperty(this._rareGlow, "modulate", Colors.White, 0.25);
			}
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x000CF498 File Offset: 0x000CD698
		private bool PlayerVotedForRelic(Player player)
		{
			TreasureRoomRelicSynchronizer.PlayerVote playerVote = RunManager.Instance.TreasureRoomRelicSynchronizer.GetPlayerVote(player);
			if (playerVote.voteReceived)
			{
				int? index = playerVote.index;
				int index2 = this.Index;
				return (index.GetValueOrDefault() == index2) & (index != null);
			}
			return false;
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x000CF4E0 File Offset: 0x000CD6E0
		public void AnimateAwayVotes()
		{
			base.CreateTween().TweenProperty(this.VoteContainer, "modulate:a", 0f, 0.25);
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x000CF514 File Offset: 0x000CD714
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this.Relic, "scale", Vector2.One * 2.1f, 0.05);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this.Relic.Model.HoverTips, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetAlignmentForRelic(this.Relic);
			}
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x000CF5AC File Offset: 0x000CD7AC
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this.Relic, "scale", Vector2.One * 2f, 0.4).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x000CF630 File Offset: 0x000CD830
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this.Relic, "scale", Vector2.One * 1.9f, 0.4).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x000CF6B0 File Offset: 0x000CD8B0
		protected override void OnRelease()
		{
			base.OnRelease();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this.Relic, "scale", Vector2.One * 2f, 0.05).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F35 RID: 7989 RVA: 0x000CF730 File Offset: 0x000CD930
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NTreasureRoomRelicHolder.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoomRelicHolder.MethodName.AnimateAwayVotes, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoomRelicHolder.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoomRelicHolder.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoomRelicHolder.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTreasureRoomRelicHolder.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F36 RID: 7990 RVA: 0x000CF860 File Offset: 0x000CDA60
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTreasureRoomRelicHolder.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicHolder.MethodName.AnimateAwayVotes && args.Count == 0)
			{
				this.AnimateAwayVotes();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicHolder.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicHolder.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicHolder.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTreasureRoomRelicHolder.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001F37 RID: 7991 RVA: 0x000CF954 File Offset: 0x000CDB54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTreasureRoomRelicHolder.MethodName._Ready || (ref method) == NTreasureRoomRelicHolder.MethodName.AnimateAwayVotes || (ref method) == NTreasureRoomRelicHolder.MethodName.OnFocus || (ref method) == NTreasureRoomRelicHolder.MethodName.OnUnfocus || (ref method) == NTreasureRoomRelicHolder.MethodName.OnPress || (ref method) == NTreasureRoomRelicHolder.MethodName.OnRelease || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001F38 RID: 7992 RVA: 0x000CF9C4 File Offset: 0x000CDBC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName.Index)
			{
				this.Index = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName.VoteContainer)
			{
				this.VoteContainer = VariantUtils.ConvertTo<NMultiplayerVoteContainer>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName.Relic)
			{
				this.Relic = VariantUtils.ConvertTo<NRelic>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName._uncommonGlow)
			{
				this._uncommonGlow = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName._rareGlow)
			{
				this._rareGlow = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName._animatedIn)
			{
				this._animatedIn = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName._initTween)
			{
				this._initTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06001F39 RID: 7993 RVA: 0x000CFAB4 File Offset: 0x000CDCB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName.Index)
			{
				int index = this.Index;
				value = VariantUtils.CreateFrom<int>(ref index);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName.VoteContainer)
			{
				NMultiplayerVoteContainer voteContainer = this.VoteContainer;
				value = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref voteContainer);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName.Relic)
			{
				NRelic relic = this.Relic;
				value = VariantUtils.CreateFrom<NRelic>(ref relic);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName._uncommonGlow)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._uncommonGlow);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName._rareGlow)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._rareGlow);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName._animatedIn)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._animatedIn);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NTreasureRoomRelicHolder.PropertyName._initTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._initTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F3A RID: 7994 RVA: 0x000CFBD4 File Offset: 0x000CDDD4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NTreasureRoomRelicHolder.PropertyName.Index, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicHolder.PropertyName.VoteContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicHolder.PropertyName.Relic, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicHolder.PropertyName._uncommonGlow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicHolder.PropertyName._rareGlow, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTreasureRoomRelicHolder.PropertyName._animatedIn, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicHolder.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTreasureRoomRelicHolder.PropertyName._initTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F3B RID: 7995 RVA: 0x000CFCF0 File Offset: 0x000CDEF0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName index = NTreasureRoomRelicHolder.PropertyName.Index;
			int index2 = this.Index;
			info.AddProperty(index, Variant.From<int>(ref index2));
			StringName voteContainer = NTreasureRoomRelicHolder.PropertyName.VoteContainer;
			NMultiplayerVoteContainer voteContainer2 = this.VoteContainer;
			info.AddProperty(voteContainer, Variant.From<NMultiplayerVoteContainer>(ref voteContainer2));
			StringName relic = NTreasureRoomRelicHolder.PropertyName.Relic;
			NRelic relic2 = this.Relic;
			info.AddProperty(relic, Variant.From<NRelic>(ref relic2));
			info.AddProperty(NTreasureRoomRelicHolder.PropertyName._uncommonGlow, Variant.From<GpuParticles2D>(ref this._uncommonGlow));
			info.AddProperty(NTreasureRoomRelicHolder.PropertyName._rareGlow, Variant.From<GpuParticles2D>(ref this._rareGlow));
			info.AddProperty(NTreasureRoomRelicHolder.PropertyName._animatedIn, Variant.From<bool>(ref this._animatedIn));
			info.AddProperty(NTreasureRoomRelicHolder.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NTreasureRoomRelicHolder.PropertyName._initTween, Variant.From<Tween>(ref this._initTween));
		}

		/// <inheritdoc />
		// Token: 0x06001F3C RID: 7996 RVA: 0x000CFDC0 File Offset: 0x000CDFC0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName.Index, ref variant))
			{
				this.Index = variant.As<int>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName.VoteContainer, ref variant2))
			{
				this.VoteContainer = variant2.As<NMultiplayerVoteContainer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName.Relic, ref variant3))
			{
				this.Relic = variant3.As<NRelic>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName._uncommonGlow, ref variant4))
			{
				this._uncommonGlow = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName._rareGlow, ref variant5))
			{
				this._rareGlow = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName._animatedIn, ref variant6))
			{
				this._animatedIn = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName._tween, ref variant7))
			{
				this._tween = variant7.As<Tween>();
			}
			Variant variant8;
			if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName._initTween, ref variant8))
			{
				this._initTween = variant8.As<Tween>();
			}
		}

		// Token: 0x04000C08 RID: 3080
		private GpuParticles2D _uncommonGlow;

		// Token: 0x04000C09 RID: 3081
		private GpuParticles2D _rareGlow;

		// Token: 0x04000C0A RID: 3082
		private bool _animatedIn;

		// Token: 0x04000C0B RID: 3083
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000C0C RID: 3084
		[Nullable(2)]
		private Tween _initTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001293 RID: 4755
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400481F RID: 18463
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'AnimateAwayVotes' method.
			/// </summary>
			// Token: 0x04004820 RID: 18464
			public static readonly StringName AnimateAwayVotes = "AnimateAwayVotes";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004821 RID: 18465
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004822 RID: 18466
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004823 RID: 18467
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004824 RID: 18468
			public new static readonly StringName OnRelease = "OnRelease";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001294 RID: 4756
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Index' property.
			/// </summary>
			// Token: 0x04004825 RID: 18469
			public static readonly StringName Index = "Index";

			/// <summary>
			/// Cached name for the 'VoteContainer' property.
			/// </summary>
			// Token: 0x04004826 RID: 18470
			public static readonly StringName VoteContainer = "VoteContainer";

			/// <summary>
			/// Cached name for the 'Relic' property.
			/// </summary>
			// Token: 0x04004827 RID: 18471
			public static readonly StringName Relic = "Relic";

			/// <summary>
			/// Cached name for the '_uncommonGlow' field.
			/// </summary>
			// Token: 0x04004828 RID: 18472
			public static readonly StringName _uncommonGlow = "_uncommonGlow";

			/// <summary>
			/// Cached name for the '_rareGlow' field.
			/// </summary>
			// Token: 0x04004829 RID: 18473
			public static readonly StringName _rareGlow = "_rareGlow";

			/// <summary>
			/// Cached name for the '_animatedIn' field.
			/// </summary>
			// Token: 0x0400482A RID: 18474
			public static readonly StringName _animatedIn = "_animatedIn";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400482B RID: 18475
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_initTween' field.
			/// </summary>
			// Token: 0x0400482C RID: 18476
			public static readonly StringName _initTween = "_initTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001295 RID: 4757
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
