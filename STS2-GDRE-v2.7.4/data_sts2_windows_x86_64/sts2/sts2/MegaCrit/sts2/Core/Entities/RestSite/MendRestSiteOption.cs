using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Entities.RestSite
{
	// Token: 0x02000C6C RID: 3180
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MendRestSiteOption : RestSiteOption
	{
		// Token: 0x170023A0 RID: 9120
		// (get) Token: 0x06008278 RID: 33400 RVA: 0x002C1743 File Offset: 0x002BF943
		public override string OptionId
		{
			get
			{
				return "MEND";
			}
		}

		// Token: 0x170023A1 RID: 9121
		// (get) Token: 0x06008279 RID: 33401 RVA: 0x002C174C File Offset: 0x002BF94C
		public override LocString Description
		{
			get
			{
				if (this._description == null)
				{
					this._description = base.Description;
					this._description.Add("HasTarget", false);
					this._description.Add("Name", "");
					this._description.Add(this._healVar);
				}
				return this._description;
			}
		}

		// Token: 0x0600827A RID: 33402 RVA: 0x002C17AA File Offset: 0x002BF9AA
		public static decimal GetHealAmount(Player player)
		{
			return Hook.ModifyRestSiteHealAmount(player.RunState, player.Creature, HealRestSiteOption.GetBaseHealAmount(player.Creature));
		}

		// Token: 0x0600827B RID: 33403 RVA: 0x002C17C8 File Offset: 0x002BF9C8
		public MendRestSiteOption(Player owner)
			: base(owner)
		{
		}

		// Token: 0x0600827C RID: 33404 RVA: 0x002C17E4 File Offset: 0x002BF9E4
		public override async Task<bool> OnSelect()
		{
			uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(base.Owner);
			Player target = null;
			if (LocalContext.IsMe(base.Owner))
			{
				NRestSiteRoom.Instance.AnimateDescriptionDown();
				NRestSiteButton buttonForOption = NRestSiteRoom.Instance.GetButtonForOption(this);
				Vector2 vector = buttonForOption.GlobalPosition + buttonForOption.Size / 2f;
				bool usingController = NControllerManager.Instance.IsUsingController;
				NTargetManager targetManager = NTargetManager.Instance;
				targetManager.StartTargeting(TargetType.AnyPlayer, vector, usingController ? TargetMode.Controller : TargetMode.ClickMouseToTarget, new Func<bool>(this.ShouldCancelTargeting), new Func<Node, bool>(this.AllowHoveringNode));
				if (usingController)
				{
					List<NRestSiteCharacter> list = NRestSiteRoom.Instance.characterAnims.Where<NRestSiteCharacter>((NRestSiteCharacter c) => c.Player != base.Owner).ToList<NRestSiteCharacter>();
					for (int i = 0; i < list.Count; i++)
					{
						list[i].Hitbox.SetFocusMode(2L);
						list[i].Hitbox.FocusNeighborTop = list[i].Hitbox.GetPath();
						list[i].Hitbox.FocusNeighborBottom = list[i].Hitbox.GetPath();
						Control hitbox = list[i].Hitbox;
						NodePath nodePath;
						if (i <= 0)
						{
							List<NRestSiteCharacter> list2 = list;
							nodePath = list2[list2.Count - 1].Hitbox.GetPath();
						}
						else
						{
							nodePath = list[i - 1].Hitbox.GetPath();
						}
						hitbox.FocusNeighborLeft = nodePath;
						list[i].Hitbox.FocusNeighborRight = ((i < list.Count - 1) ? list[i + 1].Hitbox.GetPath() : list[0].Hitbox.GetPath());
					}
					NRestSiteCharacter nrestSiteCharacter = list.FirstOrDefault<NRestSiteCharacter>();
					if (nrestSiteCharacter != null)
					{
						nrestSiteCharacter.Hitbox.TryGrabFocus();
					}
				}
				targetManager.Connect(NTargetManager.SignalName.NodeHovered, Callable.From<Node>(new Action<Node>(this.OnNodeHovered)), 0U);
				targetManager.Connect(NTargetManager.SignalName.NodeUnhovered, Callable.From<Node>(new Action<Node>(this.OnNodeUnhovered)), 0U);
				try
				{
					Node node = await targetManager.SelectionFinished();
					Node node2 = node;
					target = this.NodeToPlayer(node2);
					PlayerChoiceSynchronizer playerChoiceSynchronizer = RunManager.Instance.PlayerChoiceSynchronizer;
					Player owner = base.Owner;
					uint num = choiceId;
					Player player = target;
					playerChoiceSynchronizer.SyncLocalChoice(owner, num, PlayerChoiceResult.FromPlayerId((player != null) ? new ulong?(player.NetId) : null));
				}
				finally
				{
					targetManager.Disconnect(NTargetManager.SignalName.NodeHovered, Callable.From<Node>(new Action<Node>(this.OnNodeHovered)));
					targetManager.Disconnect(NTargetManager.SignalName.NodeUnhovered, Callable.From<Node>(new Action<Node>(this.OnNodeUnhovered)));
					if (usingController)
					{
						List<NRestSiteCharacter>.Enumerator enumerator = NRestSiteRoom.Instance.characterAnims.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								NRestSiteCharacter nrestSiteCharacter2 = enumerator.Current;
								nrestSiteCharacter2.Hitbox.SetFocusMode(0L);
							}
						}
						finally
						{
							int num2;
							if (num2 < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
					}
				}
				targetManager = null;
			}
			else
			{
				ulong? num3 = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(base.Owner, choiceId)).AsPlayerId();
				if (num3 != null)
				{
					target = base.Owner.RunState.GetPlayer(num3.Value);
				}
			}
			NRestSiteRoom instance = NRestSiteRoom.Instance;
			if (instance != null)
			{
				instance.AnimateDescriptionUp();
			}
			this.Description.Add("HasTarget", false);
			NRestSiteRoom instance2 = NRestSiteRoom.Instance;
			if (instance2 != null)
			{
				NRestSiteButton buttonForOption2 = instance2.GetButtonForOption(this);
				if (buttonForOption2 != null)
				{
					buttonForOption2.RefreshTextState();
				}
			}
			bool flag;
			if (target != null)
			{
				await CreatureCmd.Heal(target.Creature, MendRestSiteOption.GetHealAmount(target), true);
				await Hook.AfterRestSiteHeal(target.RunState, target, false);
				if (TestMode.IsOff)
				{
					string scenePath = SceneHelper.GetScenePath("vfx/vfx_cross_heal");
					Node2D node2D = PreloadManager.Cache.GetScene(scenePath).Instantiate<Node2D>(0L);
					NRestSiteRoom instance3 = NRestSiteRoom.Instance;
					NRestSiteCharacter nrestSiteCharacter3 = ((instance3 != null) ? instance3.GetCharacterForPlayer(target) : null);
					if (nrestSiteCharacter3 != null)
					{
						nrestSiteCharacter3.AddChildSafely(node2D);
					}
					node2D.Position = Vector2.Zero;
				}
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		/// <summary>
		/// Called when a targetable node (rest site character, player state display) is hovered.
		/// </summary>
		// Token: 0x0600827D RID: 33405 RVA: 0x002C1828 File Offset: 0x002BFA28
		private void OnNodeHovered(Node node)
		{
			Player player = this.NodeToPlayer(node);
			if (player != null)
			{
				this.Description.Add("HasTarget", true);
				this.Description.Add("Name", PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, player.NetId));
				this._healVar.BaseValue = HealRestSiteOption.GetBaseHealAmount(player.Creature);
				this._healVar.PreviewValue = MendRestSiteOption.GetHealAmount(player);
				NRestSiteRoom instance = NRestSiteRoom.Instance;
				if (instance == null)
				{
					return;
				}
				NRestSiteButton buttonForOption = instance.GetButtonForOption(this);
				if (buttonForOption == null)
				{
					return;
				}
				buttonForOption.RefreshTextState();
			}
		}

		/// <summary>
		/// Called when a targetable node (rest site character, player state display) is unhovered.
		/// </summary>
		// Token: 0x0600827E RID: 33406 RVA: 0x002C18BC File Offset: 0x002BFABC
		private void OnNodeUnhovered(Node _)
		{
			this.Description.Add("HasTarget", false);
			NRestSiteRoom instance = NRestSiteRoom.Instance;
			if (instance == null)
			{
				return;
			}
			NRestSiteButton buttonForOption = instance.GetButtonForOption(this);
			if (buttonForOption == null)
			{
				return;
			}
			buttonForOption.RefreshTextState();
		}

		/// <summary>
		/// Translates a node target to a player.
		/// The node target can be a character at the rest site or a remote player's state display at the top-left.
		/// </summary>
		// Token: 0x0600827F RID: 33407 RVA: 0x002C18EC File Offset: 0x002BFAEC
		[NullableContext(2)]
		private Player NodeToPlayer(Node node)
		{
			if (node == null)
			{
				return null;
			}
			NMultiplayerPlayerState nmultiplayerPlayerState = node as NMultiplayerPlayerState;
			Player player;
			if (nmultiplayerPlayerState == null)
			{
				NRestSiteCharacter nrestSiteCharacter = node as NRestSiteCharacter;
				if (nrestSiteCharacter == null)
				{
					player = null;
				}
				else
				{
					player = nrestSiteCharacter.Player;
				}
			}
			else
			{
				player = nmultiplayerPlayerState.Player;
			}
			return player;
		}

		// Token: 0x06008280 RID: 33408 RVA: 0x002C1929 File Offset: 0x002BFB29
		private bool ShouldCancelTargeting()
		{
			return NOverlayStack.Instance.ScreenCount > 0 || NCapstoneContainer.Instance.InUse;
		}

		// Token: 0x06008281 RID: 33409 RVA: 0x002C1944 File Offset: 0x002BFB44
		private bool AllowHoveringNode(Node node)
		{
			return !LocalContext.IsMe(this.NodeToPlayer(node));
		}

		// Token: 0x04002BE5 RID: 11237
		private const string _hasTargetKey = "HasTarget";

		// Token: 0x04002BE6 RID: 11238
		private const string _playerNameKey = "Name";

		// Token: 0x04002BE7 RID: 11239
		private readonly HealVar _healVar = new HealVar(0m);

		// Token: 0x04002BE8 RID: 11240
		[Nullable(2)]
		private LocString _description;
	}
}
