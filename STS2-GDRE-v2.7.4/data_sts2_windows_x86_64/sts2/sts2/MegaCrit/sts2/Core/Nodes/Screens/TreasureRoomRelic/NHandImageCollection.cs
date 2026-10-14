using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.TreasureRelicPicking;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic
{
	/// <summary>
	/// Spawns hand images at the treasure room relic picking screen.
	/// </summary>
	// Token: 0x02000254 RID: 596
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/TreasureRoomRelic/NHandImageCollection.cs")]
	public class NHandImageCollection : Control
	{
		// Token: 0x06001EEF RID: 7919 RVA: 0x000CCCC1 File Offset: 0x000CAEC1
		public override void _EnterTree()
		{
			base._EnterTree();
			this._cts = new CancellationTokenSource();
			base.GetViewport().Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x000CCCF8 File Offset: 0x000CAEF8
		public override void _ExitTree()
		{
			this._cts.Cancel();
			if (this._synchronizer != null)
			{
				this._synchronizer.StateAdded -= this.OnInputStateAdded;
				this._synchronizer.StateChanged -= this.OnInputStateChanged;
				this._synchronizer.StateRemoved -= this.OnInputStateRemoved;
			}
			NGame.Instance.CursorManager.SetCursorShown(true);
			base.GetViewport().Disconnect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)));
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x000CCD90 File Offset: 0x000CAF90
		public void Initialize(IRunState runState)
		{
			this._runState = runState;
			if (this._runState.Players.Count > 1)
			{
				this._synchronizer = RunManager.Instance.InputSynchronizer;
				this._synchronizer.StateAdded += this.OnInputStateAdded;
				this._synchronizer.StateChanged += this.OnInputStateChanged;
				this._synchronizer.StateRemoved += this.OnInputStateRemoved;
				foreach (Player player in this._runState.Players)
				{
					this.AddHand(player.NetId);
				}
				this.UpdateHandVisibility();
			}
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x000CCE60 File Offset: 0x000CB060
		private void OnInputStateAdded(ulong playerId)
		{
			this.AddHand(playerId);
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x000CCE69 File Offset: 0x000CB069
		private void OnInputStateRemoved(ulong playerId)
		{
			this.RemoveHand(playerId);
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x000CCE74 File Offset: 0x000CB074
		private void AddHand(ulong playerId)
		{
			if (this._hands.Any<NHandImage>((NHandImage c) => c.Player.NetId == playerId))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to add hand for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
				defaultInterpolatedStringHandler.AppendLiteral(" twice!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			Player player = this._runState.GetPlayer(playerId);
			NHandImage nhandImage = NHandImage.Create(player, this._runState.GetPlayerSlotIndex(player));
			this._hands.Add(nhandImage);
			this.AddChildSafely(nhandImage);
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x000CCF1C File Offset: 0x000CB11C
		private void OnInputStateChanged(ulong playerId)
		{
			Vector2 controlSpaceFocusPosition = this._synchronizer.GetControlSpaceFocusPosition(playerId, this);
			NHandImage hand = this.GetHand(playerId);
			hand.SetIsDown(this._synchronizer.GetMouseDown(playerId));
			hand.SetPointingPosition(controlSpaceFocusPosition);
			this.UpdateHandVisibility();
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x000CCF60 File Offset: 0x000CB160
		public override void _Input(InputEvent inputEvent)
		{
			if (this._runState.Players.Count == 1)
			{
				return;
			}
			if (inputEvent is InputEventMouseMotion)
			{
				NHandImage hand = this.GetHand(LocalContext.NetId.Value);
				hand.SetPointingPosition(base.GetGlobalMousePosition());
				return;
			}
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && inputEventMouseButton.ButtonIndex == 1L)
			{
				NHandImage hand2 = this.GetHand(LocalContext.NetId.Value);
				if (inputEventMouseButton.IsPressed() && !hand2.IsDown)
				{
					hand2.SetIsDown(true);
					return;
				}
				if (inputEventMouseButton.IsReleased() && hand2.IsDown)
				{
					hand2.SetIsDown(false);
				}
			}
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x000CD004 File Offset: 0x000CB204
		private void ProcessGuiFocus(Control focusedControl)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (!NControllerManager.Instance.IsUsingController)
			{
				return;
			}
			if (this._runState.Players.Count == 1)
			{
				return;
			}
			if (focusedControl is NTreasureRoomRelicHolder)
			{
				NHandImage hand = this.GetHand(LocalContext.NetId.Value);
				Vector2 vector = Vector2.Down.Rotated(hand.Rotation);
				Vector2 vector2 = focusedControl.GlobalPosition + focusedControl.Size * 0.5f + vector * 100f;
				hand.SetPointingPosition(vector2);
				return;
			}
			NHandImage hand2 = this.GetHand(LocalContext.NetId.Value);
			hand2.AnimateAway();
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x000CD0BC File Offset: 0x000CB2BC
		[NullableContext(2)]
		public NHandImage GetHand(ulong playerId)
		{
			return this._hands.FirstOrDefault<NHandImage>((NHandImage c) => c.Player.NetId == playerId);
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x000CD0F0 File Offset: 0x000CB2F0
		private void RemoveHand(ulong playerId)
		{
			NHandImage hand = this.GetHand(playerId);
			if (hand != null)
			{
				hand.QueueFreeSafely();
				this._hands.Remove(hand);
			}
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x000CD11C File Offset: 0x000CB31C
		private void UpdateHandVisibility()
		{
			NetScreenType screenType = this._synchronizer.GetScreenType(LocalContext.NetId.Value);
			foreach (NHandImage nhandImage in this._hands)
			{
				NetScreenType netScreenType = ((!RunManager.Instance.IsSingleplayerOrFakeMultiplayer) ? this._synchronizer.GetScreenType(nhandImage.Player.NetId) : NetScreenType.SharedRelicPicking);
				bool flag = netScreenType == NetScreenType.SharedRelicPicking && screenType == NetScreenType.SharedRelicPicking;
				if (!nhandImage.IsShown && flag)
				{
					nhandImage.AnimateIn();
				}
				else if (nhandImage.IsShown && !flag)
				{
					nhandImage.AnimateAway();
				}
			}
			NGame.Instance.CursorManager.SetCursorShown(screenType != NetScreenType.SharedRelicPicking);
		}

		/// <summary>
		/// Animates hands to a neutral space near the edges of the screen.
		/// </summary>
		// Token: 0x06001EFB RID: 7931 RVA: 0x000CD1F8 File Offset: 0x000CB3F8
		public void BeforeRelicsAwarded()
		{
			foreach (NHandImage nhandImage in this._hands)
			{
				nhandImage.SetFrozenForRelicAwards(true);
			}
		}

		/// <summary>
		/// Z-indexes hands to 1 so that they appear above the fight backdrop, modulates them to white regardless of whether
		/// they belong to the local player, and adjusts their pivot in preparation for throwing RPS moves.
		/// Note that should happen before DoFight is called, because the hands should already be z-index up as the fight
		/// backdrop starts fading in.
		/// </summary>
		// Token: 0x06001EFC RID: 7932 RVA: 0x000CD24C File Offset: 0x000CB44C
		public void BeforeFightStarted(List<Player> playersInvolved)
		{
			foreach (Player player in playersInvolved)
			{
				NHandImage hand = this.GetHand(player.NetId);
				hand.SetIsInFight(true);
			}
		}

		/// <summary>
		/// Animates the hands in just before the relic picking finishes.
		/// </summary>
		// Token: 0x06001EFD RID: 7933 RVA: 0x000CD2A8 File Offset: 0x000CB4A8
		public void AnimateHandsIn()
		{
			foreach (NHandImage nhandImage in this._hands)
			{
				if (nhandImage.Visible)
				{
					nhandImage.AnimateIn();
				}
			}
		}

		/// <summary>
		/// Animates the results of a relic picking fight.
		/// result.type must be FoughtOver, and result.fight must be non-null.
		/// </summary>
		/// <param name="result">The relic fight to animate.</param>
		/// <param name="holder">The relic holder that is displaying result.relic.</param>
		// Token: 0x06001EFE RID: 7934 RVA: 0x000CD304 File Offset: 0x000CB504
		public async Task DoFight(RelicPickingResult result, NTreasureRoomRelicHolder holder)
		{
			RelicPickingFight fight = result.fight;
			List<Tween> tweens = new List<Tween>();
			List<Task> tasks = new List<Task>();
			for (int i = 0; i < fight.rounds.Count; i++)
			{
				float durationMultiplier = 1.5f / ((float)i + 1.5f);
				RelicPickingFightRound round = fight.rounds[i];
				tweens.Clear();
				for (int j = 0; j < fight.playersInvolved.Count; j++)
				{
					RelicPickingFightMove? relicPickingFightMove = round.moves[j];
					if (relicPickingFightMove != null)
					{
						Player player = fight.playersInvolved[j];
						NHandImage hand = this.GetHand(player.NetId);
						tweens.Add(hand.DoFightMove(relicPickingFightMove.Value, 1.5f * durationMultiplier));
					}
				}
				await Task.WhenAll(tweens.Select<Tween, Task>((Tween t) => t.AwaitFinished(this._cts.Token)));
				List<Player> list = new List<Player>();
				for (int k = 0; k < fight.playersInvolved.Count; k++)
				{
					Player player2 = fight.playersInvolved[k];
					if (i < fight.rounds.Count - 1)
					{
						if (round.moves[k] != null && fight.rounds[i + 1].moves[k] == null)
						{
							list.Add(player2);
						}
					}
					else if (round.moves[k] != null && result.player != player2)
					{
						list.Add(player2);
					}
				}
				tasks.Clear();
				foreach (Player player3 in list)
				{
					tasks.Add(this.DoLoseShake(player3, Mathf.Max(1f * durationMultiplier, 0.5f)));
				}
				if (i == fight.rounds.Count - 1)
				{
					await Cmd.Wait(0.5f, this._cts.Token, false);
					tasks.Add(this.GetHand(result.player.NetId).GrabRelic(holder));
				}
				if (tasks.Count == 0)
				{
					await Cmd.Wait(1f * durationMultiplier, this._cts.Token, false);
				}
				else
				{
					await Task.WhenAll(tasks);
				}
				round = null;
			}
			foreach (Player player4 in fight.playersInvolved)
			{
				this.GetHand(player4.NetId).SetIsInFight(false);
			}
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x000CD358 File Offset: 0x000CB558
		private async Task DoLoseShake(Player player, float duration)
		{
			NHandImage hand = this.GetHand(player.NetId);
			await hand.DoLoseShake(duration);
			hand.SetIsInFight(false);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F00 RID: 7936 RVA: 0x000CD3AC File Offset: 0x000CB5AC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(13)
			{
				new MethodInfo(NHandImageCollection.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandImageCollection.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandImageCollection.MethodName.OnInputStateAdded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImageCollection.MethodName.OnInputStateRemoved, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImageCollection.MethodName.AddHand, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImageCollection.MethodName.OnInputStateChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImageCollection.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NHandImageCollection.MethodName.ProcessGuiFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "focusedControl", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NHandImageCollection.MethodName.GetHand, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImageCollection.MethodName.RemoveHand, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImageCollection.MethodName.UpdateHandVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandImageCollection.MethodName.BeforeRelicsAwarded, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandImageCollection.MethodName.AnimateHandsIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F01 RID: 7937 RVA: 0x000CD770 File Offset: 0x000CB970
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHandImageCollection.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName.OnInputStateAdded && args.Count == 1)
			{
				this.OnInputStateAdded(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName.OnInputStateRemoved && args.Count == 1)
			{
				this.OnInputStateRemoved(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName.AddHand && args.Count == 1)
			{
				this.AddHand(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName.OnInputStateChanged && args.Count == 1)
			{
				this.OnInputStateChanged(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName.ProcessGuiFocus && args.Count == 1)
			{
				this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName.GetHand && args.Count == 1)
			{
				NHandImage hand = this.GetHand(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<NHandImage>(ref hand);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName.RemoveHand && args.Count == 1)
			{
				this.RemoveHand(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName.UpdateHandVisibility && args.Count == 0)
			{
				this.UpdateHandVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName.BeforeRelicsAwarded && args.Count == 0)
			{
				this.BeforeRelicsAwarded();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImageCollection.MethodName.AnimateHandsIn && args.Count == 0)
			{
				this.AnimateHandsIn();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001F02 RID: 7938 RVA: 0x000CD9E0 File Offset: 0x000CBBE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHandImageCollection.MethodName._EnterTree || (ref method) == NHandImageCollection.MethodName._ExitTree || (ref method) == NHandImageCollection.MethodName.OnInputStateAdded || (ref method) == NHandImageCollection.MethodName.OnInputStateRemoved || (ref method) == NHandImageCollection.MethodName.AddHand || (ref method) == NHandImageCollection.MethodName.OnInputStateChanged || (ref method) == NHandImageCollection.MethodName._Input || (ref method) == NHandImageCollection.MethodName.ProcessGuiFocus || (ref method) == NHandImageCollection.MethodName.GetHand || (ref method) == NHandImageCollection.MethodName.RemoveHand || (ref method) == NHandImageCollection.MethodName.UpdateHandVisibility || (ref method) == NHandImageCollection.MethodName.BeforeRelicsAwarded || (ref method) == NHandImageCollection.MethodName.AnimateHandsIn || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001F03 RID: 7939 RVA: 0x000CDAB7 File Offset: 0x000CBCB7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHandImageCollection.PropertyName._handAnimateInProgress)
			{
				this._handAnimateInProgress = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001F04 RID: 7940 RVA: 0x000CDADC File Offset: 0x000CBCDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHandImageCollection.PropertyName._handAnimateInProgress)
			{
				value = VariantUtils.CreateFrom<float>(ref this._handAnimateInProgress);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F05 RID: 7941 RVA: 0x000CDB08 File Offset: 0x000CBD08
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NHandImageCollection.PropertyName._handAnimateInProgress, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F06 RID: 7942 RVA: 0x000CDB3C File Offset: 0x000CBD3C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHandImageCollection.PropertyName._handAnimateInProgress, Variant.From<float>(ref this._handAnimateInProgress));
		}

		/// <inheritdoc />
		// Token: 0x06001F07 RID: 7943 RVA: 0x000CDB5C File Offset: 0x000CBD5C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHandImageCollection.PropertyName._handAnimateInProgress, ref variant))
			{
				this._handAnimateInProgress = variant.As<float>();
			}
		}

		// Token: 0x04000BF1 RID: 3057
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x04000BF2 RID: 3058
		[Nullable(2)]
		private PeerInputSynchronizer _synchronizer;

		// Token: 0x04000BF3 RID: 3059
		private readonly List<NHandImage> _hands = new List<NHandImage>();

		// Token: 0x04000BF4 RID: 3060
		private IRunState _runState = NullRunState.Instance;

		// Token: 0x04000BF5 RID: 3061
		private float _handAnimateInProgress;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001283 RID: 4739
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040047D8 RID: 18392
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040047D9 RID: 18393
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnInputStateAdded' method.
			/// </summary>
			// Token: 0x040047DA RID: 18394
			public static readonly StringName OnInputStateAdded = "OnInputStateAdded";

			/// <summary>
			/// Cached name for the 'OnInputStateRemoved' method.
			/// </summary>
			// Token: 0x040047DB RID: 18395
			public static readonly StringName OnInputStateRemoved = "OnInputStateRemoved";

			/// <summary>
			/// Cached name for the 'AddHand' method.
			/// </summary>
			// Token: 0x040047DC RID: 18396
			public static readonly StringName AddHand = "AddHand";

			/// <summary>
			/// Cached name for the 'OnInputStateChanged' method.
			/// </summary>
			// Token: 0x040047DD RID: 18397
			public static readonly StringName OnInputStateChanged = "OnInputStateChanged";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x040047DE RID: 18398
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'ProcessGuiFocus' method.
			/// </summary>
			// Token: 0x040047DF RID: 18399
			public static readonly StringName ProcessGuiFocus = "ProcessGuiFocus";

			/// <summary>
			/// Cached name for the 'GetHand' method.
			/// </summary>
			// Token: 0x040047E0 RID: 18400
			public static readonly StringName GetHand = "GetHand";

			/// <summary>
			/// Cached name for the 'RemoveHand' method.
			/// </summary>
			// Token: 0x040047E1 RID: 18401
			public static readonly StringName RemoveHand = "RemoveHand";

			/// <summary>
			/// Cached name for the 'UpdateHandVisibility' method.
			/// </summary>
			// Token: 0x040047E2 RID: 18402
			public static readonly StringName UpdateHandVisibility = "UpdateHandVisibility";

			/// <summary>
			/// Cached name for the 'BeforeRelicsAwarded' method.
			/// </summary>
			// Token: 0x040047E3 RID: 18403
			public static readonly StringName BeforeRelicsAwarded = "BeforeRelicsAwarded";

			/// <summary>
			/// Cached name for the 'AnimateHandsIn' method.
			/// </summary>
			// Token: 0x040047E4 RID: 18404
			public static readonly StringName AnimateHandsIn = "AnimateHandsIn";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001284 RID: 4740
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_handAnimateInProgress' field.
			/// </summary>
			// Token: 0x040047E5 RID: 18405
			public static readonly StringName _handAnimateInProgress = "_handAnimateInProgress";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001285 RID: 4741
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
