using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	// Token: 0x020003E7 RID: 999
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NMultiplayerVoteContainer.cs")]
	public class NMultiplayerVoteContainer : Control
	{
		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06003CDA RID: 15578 RVA: 0x001D17F8 File Offset: 0x001CF9F8
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(SceneHelper.GetScenePath("ui/multiplayer_vote_icon"));
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06003CDB RID: 15579 RVA: 0x001D1809 File Offset: 0x001CFA09
		public IEnumerable<Player> Players
		{
			get
			{
				return this._votes.Select<NMultiplayerVoteContainer.VoteIcon, Player>((NMultiplayerVoteContainer.VoteIcon v) => v.player);
			}
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x001D1835 File Offset: 0x001CFA35
		public void Initialize(NMultiplayerVoteContainer.PlayerVotedDelegate del, IReadOnlyList<Player> players)
		{
			this._playerVotedDelegate = del;
			this._allPlayers.AddRange(players);
		}

		/// <summary>
		/// Updates the player votes, animating them in or out based on their presence as reported by _playerVotedDelegate.
		/// </summary>
		/// <param name="animate">Pass false to this to immediately update the container, instead of animating. This is
		/// useful when, for example, the turn begins and we don't want characters popping in or out.</param>
		// Token: 0x06003CDD RID: 15581 RVA: 0x001D184C File Offset: 0x001CFA4C
		public void RefreshPlayerVotes(bool animate = true)
		{
			if (this._allPlayers.Count == 1)
			{
				return;
			}
			for (int i = 0; i < this._votes.Count; i++)
			{
				NMultiplayerVoteContainer.VoteIcon voteIcon = this._votes[i];
				if (!this._playerVotedDelegate(voteIcon.player))
				{
					this.AnimVoteOut(voteIcon, animate);
					this._votes.RemoveAt(i);
					i--;
				}
			}
			using (List<Player>.Enumerator enumerator = this._allPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Player player = enumerator.Current;
					if (this._playerVotedDelegate(player))
					{
						int num = this._votes.FindIndex((NMultiplayerVoteContainer.VoteIcon p) => p.player == player);
						if (num < 0)
						{
							NMultiplayerVoteContainer.VoteIcon voteIcon2 = new NMultiplayerVoteContainer.VoteIcon
							{
								player = player,
								node = SceneHelper.Instantiate<TextureRect>("ui/multiplayer_vote_icon")
							};
							voteIcon2.node.Texture = player.Character.IconTexture;
							voteIcon2.node.GetNode<TextureRect>("Outline").Texture = player.Character.IconOutlineTexture;
							this._votes.Add(voteIcon2);
							this.AddChildSafely(voteIcon2.node);
							voteIcon2.node.PivotOffset = voteIcon2.node.Size * 0.5f;
							this.AnimVoteIn(voteIcon2, animate);
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns the index at which player is shown in this map vote container.
		/// </summary>
		// Token: 0x06003CDE RID: 15582 RVA: 0x001D19F8 File Offset: 0x001CFBF8
		public int GetVoteIndex(Player player)
		{
			return this._votes.FindIndex((NMultiplayerVoteContainer.VoteIcon v) => v.player == player);
		}

		/// <summary>
		/// Highlights the player within the container.
		/// Used during the map vote randomization animation to show which player is currently being picked.
		/// </summary>
		// Token: 0x06003CDF RID: 15583 RVA: 0x001D1A2C File Offset: 0x001CFC2C
		public void SetPlayerHighlighted(Player player, bool isHighlighted)
		{
			NMultiplayerVoteContainer.VoteIcon voteIcon = this._votes.FirstOrDefault<NMultiplayerVoteContainer.VoteIcon>((NMultiplayerVoteContainer.VoteIcon v) => v.player == player);
			if (voteIcon == null)
			{
				throw new InvalidOperationException();
			}
			Tween tween = voteIcon.tween;
			if (tween != null)
			{
				tween.FastForwardToCompletion();
			}
			if (isHighlighted)
			{
				voteIcon.node.Scale = Vector2.One * 1.25f;
				return;
			}
			voteIcon.node.Scale = Vector2.One;
		}

		/// <summary>
		/// Bounces all players within the container.
		/// Used during the map vote randomization animation to show which map coordinate has been chosen.
		/// </summary>
		// Token: 0x06003CE0 RID: 15584 RVA: 0x001D1AAC File Offset: 0x001CFCAC
		public void BouncePlayers()
		{
			foreach (NMultiplayerVoteContainer.VoteIcon voteIcon in this._votes)
			{
				Tween tween = voteIcon.tween;
				if (tween != null)
				{
					tween.FastForwardToCompletion();
				}
				voteIcon.tween = base.CreateTween().SetParallel(true);
				voteIcon.tween.TweenProperty(voteIcon.node, "scale", Vector2.One * 1.3f, 0.15).SetEase(1L).SetTrans(4L);
				voteIcon.tween.TweenProperty(voteIcon.node, "position:y", -10f, 0.15).SetEase(1L).SetTrans(4L);
				voteIcon.tween.Chain().TweenProperty(voteIcon.node, "scale", Vector2.One, 0.2).SetEase(0L)
					.SetTrans(4L);
				voteIcon.tween.TweenProperty(voteIcon.node, "position:y", 0f, 0.3).SetEase(0L).SetTrans(4L);
			}
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x001D1C30 File Offset: 0x001CFE30
		private void AnimVoteIn(NMultiplayerVoteContainer.VoteIcon vote, bool animate)
		{
			int num = this._iconsAnimatingOut.FindIndex((NMultiplayerVoteContainer.VoteIcon i) => i.player == vote.player);
			if (num > 0)
			{
				Tween tween = this._iconsAnimatingOut[num].tween;
				if (tween != null)
				{
					tween.Kill();
				}
				this._iconsAnimatingOut[num].node.QueueFreeSafely();
				this._iconsAnimatingOut.RemoveAt(num);
			}
			if (animate)
			{
				Tween tween2 = vote.tween;
				if (tween2 != null)
				{
					tween2.Kill();
				}
				vote.tween = base.CreateTween().SetParallel(true);
				vote.tween.TweenProperty(vote.node, "modulate:a", 1f, 0.2).From(0f);
				vote.tween.TweenProperty(vote.node, "position:y", 0f, 0.3).From(20f).SetTrans(10L)
					.SetEase(1L);
			}
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x001D1D78 File Offset: 0x001CFF78
		private void AnimVoteOut(NMultiplayerVoteContainer.VoteIcon vote, bool animate)
		{
			this._iconsAnimatingOut.Add(vote);
			if (animate)
			{
				Tween tween = vote.tween;
				if (tween != null)
				{
					tween.Kill();
				}
				vote.tween = base.CreateTween().SetParallel(true);
				vote.tween.TweenProperty(vote.node, "modulate:a", 0f, 0.1).SetDelay(0.15000000596046448);
				vote.tween.TweenProperty(vote.node, "position:y", 20f, 0.25).SetTrans(5L).SetEase(0L);
				vote.tween.Chain().TweenCallback(Callable.From(delegate
				{
					this.RemoveVoteAfterAnimation(vote);
				}));
				return;
			}
			this.RemoveVoteAfterAnimation(vote);
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x001D1E9F File Offset: 0x001D009F
		private void RemoveVoteAfterAnimation(NMultiplayerVoteContainer.VoteIcon vote)
		{
			this._iconsAnimatingOut.Remove(vote);
			vote.node.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003CE4 RID: 15588 RVA: 0x001D1EBC File Offset: 0x001D00BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NMultiplayerVoteContainer.MethodName.RefreshPlayerVotes, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "animate", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMultiplayerVoteContainer.MethodName.BouncePlayers, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003CE5 RID: 15589 RVA: 0x001D1F54 File Offset: 0x001D0154
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerVoteContainer.MethodName.RefreshPlayerVotes && args.Count == 1)
			{
				this.RefreshPlayerVotes(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerVoteContainer.MethodName.BouncePlayers && args.Count == 0)
			{
				this.BouncePlayers();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003CE6 RID: 15590 RVA: 0x001D1FC2 File Offset: 0x001D01C2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerVoteContainer.MethodName.RefreshPlayerVotes || (ref method) == NMultiplayerVoteContainer.MethodName.BouncePlayers || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003CE7 RID: 15591 RVA: 0x001D1FE9 File Offset: 0x001D01E9
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003CE8 RID: 15592 RVA: 0x001D1FF2 File Offset: 0x001D01F2
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x040017B8 RID: 6072
		private const string _voteIconPath = "ui/multiplayer_vote_icon";

		// Token: 0x040017B9 RID: 6073
		private readonly List<NMultiplayerVoteContainer.VoteIcon> _votes = new List<NMultiplayerVoteContainer.VoteIcon>();

		// Token: 0x040017BA RID: 6074
		private readonly List<NMultiplayerVoteContainer.VoteIcon> _iconsAnimatingOut = new List<NMultiplayerVoteContainer.VoteIcon>();

		// Token: 0x040017BB RID: 6075
		private NMultiplayerVoteContainer.PlayerVotedDelegate _playerVotedDelegate;

		// Token: 0x040017BC RID: 6076
		private readonly List<Player> _allPlayers = new List<Player>();

		// Token: 0x0200186D RID: 6253
		// (Invoke) Token: 0x0600A8A7 RID: 43175
		[NullableContext(0)]
		public delegate bool PlayerVotedDelegate(Player player);

		// Token: 0x0200186E RID: 6254
		[Nullable(0)]
		[RequiredMember]
		private class VoteIcon : IEquatable<NMultiplayerVoteContainer.VoteIcon>
		{
			// Token: 0x170026BB RID: 9915
			// (get) Token: 0x0600A8AA RID: 43178 RVA: 0x00355948 File Offset: 0x00353B48
			[CompilerGenerated]
			protected virtual Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(NMultiplayerVoteContainer.VoteIcon);
				}
			}

			// Token: 0x0600A8AB RID: 43179 RVA: 0x00355954 File Offset: 0x00353B54
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("VoteIcon");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600A8AC RID: 43180 RVA: 0x003559A0 File Offset: 0x00353BA0
			[CompilerGenerated]
			protected virtual bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("player = ");
				builder.Append(this.player);
				builder.Append(", node = ");
				builder.Append(this.node);
				builder.Append(", tween = ");
				builder.Append(this.tween);
				return true;
			}

			// Token: 0x0600A8AD RID: 43181 RVA: 0x003559FE File Offset: 0x00353BFE
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(NMultiplayerVoteContainer.VoteIcon left, NMultiplayerVoteContainer.VoteIcon right)
			{
				return !(left == right);
			}

			// Token: 0x0600A8AE RID: 43182 RVA: 0x00355A0A File Offset: 0x00353C0A
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(NMultiplayerVoteContainer.VoteIcon left, NMultiplayerVoteContainer.VoteIcon right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600A8AF RID: 43183 RVA: 0x00355A20 File Offset: 0x00353C20
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<Player>.Default.GetHashCode(this.player)) * -1521134295 + EqualityComparer<TextureRect>.Default.GetHashCode(this.node)) * -1521134295 + EqualityComparer<Tween>.Default.GetHashCode(this.tween);
			}

			// Token: 0x0600A8B0 RID: 43184 RVA: 0x00355A82 File Offset: 0x00353C82
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as NMultiplayerVoteContainer.VoteIcon);
			}

			// Token: 0x0600A8B1 RID: 43185 RVA: 0x00355A90 File Offset: 0x00353C90
			[NullableContext(2)]
			[CompilerGenerated]
			public virtual bool Equals(NMultiplayerVoteContainer.VoteIcon other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<Player>.Default.Equals(this.player, other.player) && EqualityComparer<TextureRect>.Default.Equals(this.node, other.node) && EqualityComparer<Tween>.Default.Equals(this.tween, other.tween));
			}

			// Token: 0x0600A8B3 RID: 43187 RVA: 0x00355B09 File Offset: 0x00353D09
			[CompilerGenerated]
			[SetsRequiredMembers]
			protected VoteIcon(NMultiplayerVoteContainer.VoteIcon original)
			{
				this.player = original.player;
				this.node = original.node;
				this.tween = original.tween;
			}

			// Token: 0x0600A8B4 RID: 43188 RVA: 0x00355B35 File Offset: 0x00353D35
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public VoteIcon()
			{
			}

			// Token: 0x04005F88 RID: 24456
			[RequiredMember]
			public Player player;

			// Token: 0x04005F89 RID: 24457
			[RequiredMember]
			public TextureRect node;

			// Token: 0x04005F8A RID: 24458
			[Nullable(2)]
			public Tween tween;
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200186F RID: 6255
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'RefreshPlayerVotes' method.
			/// </summary>
			// Token: 0x04005F8B RID: 24459
			public static readonly StringName RefreshPlayerVotes = "RefreshPlayerVotes";

			/// <summary>
			/// Cached name for the 'BouncePlayers' method.
			/// </summary>
			// Token: 0x04005F8C RID: 24460
			public static readonly StringName BouncePlayers = "BouncePlayers";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001870 RID: 6256
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001871 RID: 6257
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
