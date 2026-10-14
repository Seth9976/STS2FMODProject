using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Nodes.Rewards
{
	// Token: 0x02000371 RID: 881
	[ScriptPath("res://src/Core/Nodes/Rewards/NLinkedRewardSet.cs")]
	public class NLinkedRewardSet : Control
	{
		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060033D1 RID: 13265 RVA: 0x00182550 File Offset: 0x00180750
		// (set) Token: 0x060033D2 RID: 13266 RVA: 0x00182558 File Offset: 0x00180758
		[Nullable(1)]
		public LinkedRewardSet LinkedRewardSet
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			private set;
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x060033D3 RID: 13267 RVA: 0x00182561 File Offset: 0x00180761
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("/rewards/linked_reward_set");
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060033D4 RID: 13268 RVA: 0x0018256D File Offset: 0x0018076D
		[Nullable(1)]
		private static string ChainImagePath
		{
			[NullableContext(1)]
			get
			{
				return ImageHelper.GetImagePath("/ui/reward_screen/reward_chain.png");
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x060033D5 RID: 13269 RVA: 0x00182579 File Offset: 0x00180779
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new string[]
				{
					NLinkedRewardSet.ScenePath,
					NLinkedRewardSet.ChainImagePath
				};
			}
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x00182591 File Offset: 0x00180791
		public override void _Ready()
		{
			this._rewardContainer = base.GetNode<Control>("%RewardContainer");
			this._chainsContainer = base.GetNode<Control>("%ChainContainer");
			this.Reload();
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x001825C8 File Offset: 0x001807C8
		[NullableContext(1)]
		public static NLinkedRewardSet Create(LinkedRewardSet linkedReward, NRewardsScreen screen)
		{
			NLinkedRewardSet nlinkedRewardSet = PreloadManager.Cache.GetScene(NLinkedRewardSet.ScenePath).Instantiate<NLinkedRewardSet>(0L);
			nlinkedRewardSet._rewardsScreen = screen;
			nlinkedRewardSet.SetReward(linkedReward);
			return nlinkedRewardSet;
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x001825FB File Offset: 0x001807FB
		[NullableContext(1)]
		private void SetReward(LinkedRewardSet linkedReward)
		{
			this.LinkedRewardSet = linkedReward;
			if (base.IsNodeReady())
			{
				this.Reload();
			}
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x00182614 File Offset: 0x00180814
		private void Reload()
		{
			if (!base.IsNodeReady())
			{
				return;
			}
			for (int i = 0; i < this.LinkedRewardSet.Rewards.Count; i++)
			{
				Reward reward = this.LinkedRewardSet.Rewards[i];
				NRewardButton nrewardButton = NRewardButton.Create(reward, this._rewardsScreen);
				nrewardButton.CustomMinimumSize -= Vector2.Right * 20f;
				this._rewardContainer.AddChildSafely(nrewardButton);
				nrewardButton.Connect(NRewardButton.SignalName.RewardClaimed, Callable.From(new Action(this.GetReward)), 0U);
				if (i < this.LinkedRewardSet.Rewards.Count - 1)
				{
					TextureRect textureRect = new TextureRect();
					textureRect.MouseFilter = 2L;
					textureRect.Texture = PreloadManager.Cache.GetCompressedTexture2D(NLinkedRewardSet.ChainImagePath);
					textureRect.Size = Vector2.One * 50f;
					this._chainsContainer.AddChildSafely(textureRect);
					textureRect.GlobalPosition = this._chainsContainer.GlobalPosition + Vector2.Down * (float)i * (3f + nrewardButton.CustomMinimumSize.Y);
				}
			}
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x00182745 File Offset: 0x00180945
		private void GetReward()
		{
			this._rewardsScreen.RewardCollectedFrom(this);
			this.LinkedRewardSet.OnSkipped();
			base.EmitSignal(NLinkedRewardSet.SignalName.RewardClaimed, Array.Empty<Variant>());
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060033DB RID: 13275 RVA: 0x00182778 File Offset: 0x00180978
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NLinkedRewardSet.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLinkedRewardSet.MethodName.Reload, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLinkedRewardSet.MethodName.GetReward, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060033DC RID: 13276 RVA: 0x0018281C File Offset: 0x00180A1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLinkedRewardSet.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLinkedRewardSet.MethodName.Reload && args.Count == 0)
			{
				this.Reload();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLinkedRewardSet.MethodName.GetReward && args.Count == 0)
			{
				this.GetReward();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060033DD RID: 13277 RVA: 0x001828A1 File Offset: 0x00180AA1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLinkedRewardSet.MethodName._Ready || (ref method) == NLinkedRewardSet.MethodName.Reload || (ref method) == NLinkedRewardSet.MethodName.GetReward || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060033DE RID: 13278 RVA: 0x001828D8 File Offset: 0x00180AD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLinkedRewardSet.PropertyName._rewardsScreen)
			{
				this._rewardsScreen = VariantUtils.ConvertTo<NRewardsScreen>(ref value);
				return true;
			}
			if ((ref name) == NLinkedRewardSet.PropertyName._rewardContainer)
			{
				this._rewardContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NLinkedRewardSet.PropertyName._chainsContainer)
			{
				this._chainsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060033DF RID: 13279 RVA: 0x00182940 File Offset: 0x00180B40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLinkedRewardSet.PropertyName._rewardsScreen)
			{
				value = VariantUtils.CreateFrom<NRewardsScreen>(ref this._rewardsScreen);
				return true;
			}
			if ((ref name) == NLinkedRewardSet.PropertyName._rewardContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rewardContainer);
				return true;
			}
			if ((ref name) == NLinkedRewardSet.PropertyName._chainsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._chainsContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060033E0 RID: 13280 RVA: 0x001829B8 File Offset: 0x00180BB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NLinkedRewardSet.PropertyName._rewardsScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLinkedRewardSet.PropertyName._rewardContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLinkedRewardSet.PropertyName._chainsContainer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060033E1 RID: 13281 RVA: 0x00182A30 File Offset: 0x00180C30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLinkedRewardSet.PropertyName._rewardsScreen, Variant.From<NRewardsScreen>(ref this._rewardsScreen));
			info.AddProperty(NLinkedRewardSet.PropertyName._rewardContainer, Variant.From<Control>(ref this._rewardContainer));
			info.AddProperty(NLinkedRewardSet.PropertyName._chainsContainer, Variant.From<Control>(ref this._chainsContainer));
			info.AddSignalEventDelegate(NLinkedRewardSet.SignalName.RewardClaimed, this.backing_RewardClaimed);
		}

		/// <inheritdoc />
		// Token: 0x060033E2 RID: 13282 RVA: 0x00182A98 File Offset: 0x00180C98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLinkedRewardSet.PropertyName._rewardsScreen, ref variant))
			{
				this._rewardsScreen = variant.As<NRewardsScreen>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLinkedRewardSet.PropertyName._rewardContainer, ref variant2))
			{
				this._rewardContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NLinkedRewardSet.PropertyName._chainsContainer, ref variant3))
			{
				this._chainsContainer = variant3.As<Control>();
			}
			NLinkedRewardSet.RewardClaimedEventHandler rewardClaimedEventHandler;
			if (info.TryGetSignalEventDelegate<NLinkedRewardSet.RewardClaimedEventHandler>(NLinkedRewardSet.SignalName.RewardClaimed, ref rewardClaimedEventHandler))
			{
				this.backing_RewardClaimed = rewardClaimedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060033E3 RID: 13283 RVA: 0x00182B18 File Offset: 0x00180D18
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NLinkedRewardSet.SignalName.RewardClaimed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "linkedRewardSet", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Rewards.NLinkedRewardSet.RewardClaimedEventHandler" />
		// Token: 0x1400002F RID: 47
		// (add) Token: 0x060033E4 RID: 13284 RVA: 0x00182B8C File Offset: 0x00180D8C
		// (remove) Token: 0x060033E5 RID: 13285 RVA: 0x00182BA5 File Offset: 0x00180DA5
		public event NLinkedRewardSet.RewardClaimedEventHandler RewardClaimed
		{
			add
			{
				this.backing_RewardClaimed = (NLinkedRewardSet.RewardClaimedEventHandler)Delegate.Combine(this.backing_RewardClaimed, value);
			}
			remove
			{
				this.backing_RewardClaimed = (NLinkedRewardSet.RewardClaimedEventHandler)Delegate.Remove(this.backing_RewardClaimed, value);
			}
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x00182BBE File Offset: 0x00180DBE
		protected void EmitSignalRewardClaimed(NLinkedRewardSet linkedRewardSet)
		{
			base.EmitSignal(NLinkedRewardSet.SignalName.RewardClaimed, new Variant[] { linkedRewardSet });
		}

		/// <inheritdoc />
		// Token: 0x060033E7 RID: 13287 RVA: 0x00182BDF File Offset: 0x00180DDF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NLinkedRewardSet.SignalName.RewardClaimed) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NLinkedRewardSet.RewardClaimedEventHandler rewardClaimedEventHandler = this.backing_RewardClaimed;
			if (rewardClaimedEventHandler == null)
			{
				return;
			}
			rewardClaimedEventHandler(VariantUtils.ConvertTo<NLinkedRewardSet>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x060033E8 RID: 13288 RVA: 0x00182C1E File Offset: 0x00180E1E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NLinkedRewardSet.SignalName.RewardClaimed || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04001419 RID: 5145
		[Nullable(1)]
		private NRewardsScreen _rewardsScreen;

		// Token: 0x0400141B RID: 5147
		[Nullable(1)]
		private Control _rewardContainer;

		// Token: 0x0400141C RID: 5148
		[Nullable(1)]
		private Control _chainsContainer;

		// Token: 0x0400141D RID: 5149
		private NLinkedRewardSet.RewardClaimedEventHandler backing_RewardClaimed;

		// Token: 0x020016AB RID: 5803
		// (Invoke) Token: 0x0600A533 RID: 42291
		[Signal]
		public delegate void RewardClaimedEventHandler(NLinkedRewardSet linkedRewardSet);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016AC RID: 5804
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005863 RID: 22627
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Reload' method.
			/// </summary>
			// Token: 0x04005864 RID: 22628
			public static readonly StringName Reload = "Reload";

			/// <summary>
			/// Cached name for the 'GetReward' method.
			/// </summary>
			// Token: 0x04005865 RID: 22629
			public static readonly StringName GetReward = "GetReward";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016AD RID: 5805
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_rewardsScreen' field.
			/// </summary>
			// Token: 0x04005866 RID: 22630
			public static readonly StringName _rewardsScreen = "_rewardsScreen";

			/// <summary>
			/// Cached name for the '_rewardContainer' field.
			/// </summary>
			// Token: 0x04005867 RID: 22631
			public static readonly StringName _rewardContainer = "_rewardContainer";

			/// <summary>
			/// Cached name for the '_chainsContainer' field.
			/// </summary>
			// Token: 0x04005868 RID: 22632
			public static readonly StringName _chainsContainer = "_chainsContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016AE RID: 5806
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'RewardClaimed' signal.
			/// </summary>
			// Token: 0x04005869 RID: 22633
			public static readonly StringName RewardClaimed = "RewardClaimed";
		}
	}
}
