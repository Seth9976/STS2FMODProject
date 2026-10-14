using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.TreasureRelicPicking;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic
{
	/// <summary>
	/// Image of a literal hand.
	/// This name is kind of wonky because it's definitely not the same as NPlayerHand.
	/// </summary>
	// Token: 0x02000253 RID: 595
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/TreasureRoomRelic/NHandImage.cs")]
	public class NHandImage : Control
	{
		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001ECB RID: 7883 RVA: 0x000CB406 File Offset: 0x000C9606
		// (set) Token: 0x06001ECC RID: 7884 RVA: 0x000CB40E File Offset: 0x000C960E
		public Player Player { get; private set; }

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x000CB417 File Offset: 0x000C9617
		// (set) Token: 0x06001ECE RID: 7886 RVA: 0x000CB41F File Offset: 0x000C961F
		public int Index { get; private set; }

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001ECF RID: 7887 RVA: 0x000CB428 File Offset: 0x000C9628
		// (set) Token: 0x06001ED0 RID: 7888 RVA: 0x000CB430 File Offset: 0x000C9630
		public bool IsDown { get; private set; }

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x000CB439 File Offset: 0x000C9639
		// (set) Token: 0x06001ED2 RID: 7890 RVA: 0x000CB441 File Offset: 0x000C9641
		public bool IsShown { get; private set; }

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000CB44C File Offset: 0x000C964C
		public static NHandImage Create(Player player, int slotIndex)
		{
			NHandImage nhandImage = PreloadManager.Cache.GetScene(NHandImage._scenePath).Instantiate<NHandImage>(0L);
			nhandImage.Player = player;
			nhandImage.Index = slotIndex;
			return nhandImage;
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000CB480 File Offset: 0x000C9680
		public override void _Ready()
		{
			this._textureRect = base.GetNode<TextureRect>("TextureRect");
			this._grabMarker = base.GetNode<Marker2D>("GrabMarker");
			this._originalPosition = this._textureRect.Position;
			float num;
			switch (this.Index % 4)
			{
			case 0:
				num = 0f;
				break;
			case 1:
				num = 1.5707964f;
				break;
			case 2:
				num = -1.5707964f;
				break;
			case 3:
				num = 3.1415927f;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			base.Rotation = num;
			if (!LocalContext.IsMe(this.Player))
			{
				base.Modulate = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			}
			this._textureRect.Texture = this.Player.Character.ArmPointingTexture;
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000CB55F File Offset: 0x000C975F
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000CB56C File Offset: 0x000C976C
		public override void _ExitTree()
		{
			this._cts.Cancel();
		}

		/// <summary>
		/// If true: Modulates them to white regardless of whether they belong to the local player and adjusts their pivot
		/// in preparation for throwing RPS moves.
		/// If false: undoes all the above.
		/// </summary>
		/// <param name="inFight"></param>
		// Token: 0x06001ED7 RID: 7895 RVA: 0x000CB57C File Offset: 0x000C977C
		public void SetIsInFight(bool inFight)
		{
			this._isInFight = inFight;
			if (this._isInFight)
			{
				this._textureRect.PivotOffset = NHandImage._fightingPivot;
				base.Modulate = Colors.White;
				return;
			}
			this._textureRect.PivotOffset = NHandImage._pointingPivot;
			if (!LocalContext.IsMe(this.Player))
			{
				base.Modulate = new Color(0.5f, 0.5f, 0.5f, 0.5f);
				return;
			}
			base.Modulate = Colors.White;
		}

		/// <summary>
		/// If true is passed, stops the hand from following the mouse cursor (local or remote), and tweens the hand towards
		/// the edge of the screen.
		/// </summary>
		// Token: 0x06001ED8 RID: 7896 RVA: 0x000CB5FC File Offset: 0x000C97FC
		public void SetFrozenForRelicAwards(bool frozenForRelicAwards)
		{
			if (frozenForRelicAwards)
			{
				this._state = NHandImage.State.Frozen;
				this._desiredPosition = this.GetFrozenPosition();
				return;
			}
			this._state = NHandImage.State.None;
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x000CB61C File Offset: 0x000C981C
		private Vector2 GetFrozenPosition()
		{
			Rect2 viewportRect = base.GetViewportRect();
			Vector2 vector = Vector2.Down.Rotated(base.Rotation);
			return viewportRect.Size / 2f + viewportRect.Size * vector * 0.1667f;
		}

		/// <summary>
		/// Emulates a person throwing a rock-paper-scissors move; that is, the hand shakes three times, then shows an image
		/// that looks like a fist, a peace sign, or a hand with all fingers spread.
		/// </summary>
		/// <param name="move">The RPS move to show.</param>
		/// <param name="duration">How long, in total, the three shakes should take to animate.</param>
		// Token: 0x06001EDA RID: 7898 RVA: 0x000CB674 File Offset: 0x000C9874
		public unsafe Tween DoFightMove(RelicPickingFightMove move, float duration)
		{
			float num = 0.666f * duration / 3f;
			float num2 = 0.333f * duration / 3f;
			int num3 = 6;
			List<float> list = new List<float>(num3);
			CollectionsMarshal.SetCount<float>(list, num3);
			Span<float> span = CollectionsMarshal.AsSpan<float>(list);
			int num4 = 0;
			*span[num4] = num;
			num4++;
			*span[num4] = num2;
			num4++;
			*span[num4] = num;
			num4++;
			*span[num4] = num2;
			num4++;
			*span[num4] = num;
			num4++;
			*span[num4] = num2;
			List<float> list2 = list;
			for (int i = 0; i < list2.Count - 1; i++)
			{
				float num5 = Rng.Chaotic.NextFloat(-duration / 25f, duration / 25f);
				List<float> list3 = list2;
				num4 = i;
				list3[num4] += num5;
				list3 = list2;
				num4 = i + 1;
				list3[num4] -= num5;
			}
			this.SetTextureToFightMove(RelicPickingFightMove.Rock);
			Tween tween = base.CreateTween();
			tween.Chain().TweenProperty(this._textureRect, "rotation", -0.31415927f + Rng.Chaotic.NextFloat(-0.05f, 0.05f), (double)list2[0]).SetTrans(0L)
				.SetEase(0L);
			tween.Chain().TweenProperty(this._textureRect, "rotation", Rng.Chaotic.NextFloat(-0.02f, 0.02f), (double)list2[1]).SetTrans(5L)
				.SetEase(0L);
			tween.Chain().TweenProperty(this._textureRect, "rotation", -0.31415927f + Rng.Chaotic.NextFloat(-0.05f, 0.05f), (double)list2[2]).SetTrans(0L)
				.SetEase(0L);
			tween.Chain().TweenProperty(this._textureRect, "rotation", Rng.Chaotic.NextFloat(-0.02f, 0.02f), (double)list2[3]).SetTrans(5L)
				.SetEase(0L);
			tween.Chain().TweenProperty(this._textureRect, "rotation", -0.31415927f + Rng.Chaotic.NextFloat(-0.05f, 0.05f), (double)list2[4]).SetTrans(0L)
				.SetEase(0L);
			tween.Chain().TweenProperty(this._textureRect, "rotation", Rng.Chaotic.NextFloat(-0.02f, 0.02f), (double)list2[5]).SetTrans(5L)
				.SetEase(0L);
			tween.TweenCallback(Callable.From(delegate
			{
				this.SetTextureToFightMove(move);
			}));
			return tween;
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x000CB9A0 File Offset: 0x000C9BA0
		private void SetTextureToFightMove(RelicPickingFightMove move)
		{
			TextureRect textureRect = this._textureRect;
			Texture2D texture2D;
			switch (move)
			{
			case RelicPickingFightMove.Rock:
				texture2D = this.Player.Character.ArmRockTexture;
				break;
			case RelicPickingFightMove.Paper:
				texture2D = this.Player.Character.ArmPaperTexture;
				break;
			case RelicPickingFightMove.Scissors:
				texture2D = this.Player.Character.ArmScissorsTexture;
				break;
			default:
				throw new ArgumentOutOfRangeException("move", move, null);
			}
			textureRect.Texture = texture2D;
		}

		/// <summary>
		/// Sets the position at which the hand will point.
		/// If SetFrozenForRelicAwards has been called with true, then this does nothing.
		/// </summary>
		/// <param name="position"></param>
		// Token: 0x06001EDC RID: 7900 RVA: 0x000CBA1A File Offset: 0x000C9C1A
		public void SetPointingPosition(Vector2 position)
		{
			if (this._state != NHandImage.State.None)
			{
				return;
			}
			this._desiredPosition = position;
		}

		/// <summary>
		/// Called after relic picking is done and the hand should go away.
		/// </summary>
		// Token: 0x06001EDD RID: 7901 RVA: 0x000CBA2C File Offset: 0x000C9C2C
		public void AnimateAway()
		{
			Rect2 viewportRect = base.GetViewportRect();
			Vector2 vector = Vector2.Down.Rotated(base.Rotation);
			this._desiredPosition = viewportRect.Size / 2f + viewportRect.Size * vector * 0.8f;
			this.IsShown = false;
		}

		/// <summary>
		/// Called when the hand is shown and we should animate it in from off-screen.
		/// </summary>
		// Token: 0x06001EDE RID: 7902 RVA: 0x000CBA90 File Offset: 0x000C9C90
		public void AnimateIn()
		{
			Tween tween = base.CreateTween();
			this._handAnimateInProgress = 0f;
			tween.TweenMethod(Callable.From<float>(delegate(float v)
			{
				this._handAnimateInProgress = v;
			}), 0f, 1f, 0.6000000238418579).SetTrans(5L).SetEase(1L);
			this.IsShown = true;
			if (this._state == NHandImage.State.Frozen)
			{
				this._desiredPosition = this.GetFrozenPosition();
			}
		}

		/// <summary>
		/// Does a small tween: scale down if true, scale up if false.
		/// Should be called when the left mouse button (local or remote) is pressed down.
		/// </summary>
		/// <param name="isDown"></param>
		// Token: 0x06001EDF RID: 7903 RVA: 0x000CBB10 File Offset: 0x000C9D10
		public void SetIsDown(bool isDown)
		{
			if (this.IsDown != isDown)
			{
				this.IsDown = isDown;
				Tween downTween = this._downTween;
				if (downTween != null)
				{
					downTween.Kill();
				}
				if (isDown)
				{
					this._textureRect.Scale = Vector2.One * 0.98f;
					return;
				}
				this._downTween = base.CreateTween();
				this._downTween.TweenProperty(this._textureRect, "scale", Vector2.One, 0.20000000298023224).SetTrans(5L).SetEase(1L);
			}
		}

		/// <summary>
		/// Shakes the hand very dramatically and fades it out.
		/// </summary>
		/// <param name="duration">The amount of time to shake and fade.</param>
		// Token: 0x06001EE0 RID: 7904 RVA: 0x000CBBA8 File Offset: 0x000C9DA8
		public async Task DoLoseShake(float duration)
		{
			base.CreateTween().TweenProperty(this, "modulate", new Color(0.5f, 0.5f, 0.5f, 0.5f), (double)(duration * 0.333f)).SetDelay((double)(duration * 0.667f));
			ScreenRumbleInstance rumble = new ScreenRumbleInstance(100f, (double)duration, 5f, RumbleStyle.Rumble);
			while (!rumble.IsDone)
			{
				Vector2 vector = rumble.Update(base.GetProcessDeltaTime());
				this._textureRect.Position = this._originalPosition + vector;
				await this.AwaitProcessFrame(this._cts.Token);
			}
			this._textureRect.Position = this._originalPosition;
		}

		/// <summary>
		/// Does a tween that looks like the hand is grabbing the holder.
		/// After the tween is complete, the holder will be parented to the hand.
		/// </summary>
		/// <param name="holder">The relic holder to grab.</param>
		// Token: 0x06001EE1 RID: 7905 RVA: 0x000CBBF4 File Offset: 0x000C9DF4
		public async Task GrabRelic(NTreasureRoomRelicHolder holder)
		{
			NHandImage.State oldState = this._state;
			this._state = NHandImage.State.GrabbingRelic;
			this.SetTextureToFightMove(RelicPickingFightMove.Paper);
			Tween tween = base.CreateTween();
			tween.TweenProperty(this, "global_position", holder.GlobalPosition - this._grabMarker.Position.Rotated(base.Rotation) + holder.Size * 0.5f, 0.5).SetTrans(1L).SetEase(2L);
			await tween.AwaitFinished(this._cts.Token);
			this.SetTextureToFightMove(RelicPickingFightMove.Rock);
			holder.Reparent(this, true);
			holder.Rotation = -base.Rotation;
			holder.Position = this._grabMarker.Position - holder.Size * 0.5f;
			tween = base.CreateTween();
			tween.TweenProperty(this, "global_position", this._desiredPosition, 0.5).SetTrans(1L).SetEase(2L);
			await tween.AwaitFinished(this._cts.Token);
			this._state = oldState;
		}

		/// <summary>
		/// Sets hand to fist.
		/// </summary>
		// Token: 0x06001EE2 RID: 7906 RVA: 0x000CBC3F File Offset: 0x000C9E3F
		public void SetSkipped()
		{
			this.SetTextureToFightMove(RelicPickingFightMove.Rock);
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x000CBC48 File Offset: 0x000C9E48
		public override void _Process(double delta)
		{
			Vector2 size = base.GetViewportRect().Size;
			int num = this.Index % 4;
			bool flag;
			if (this._state != NHandImage.State.GrabbingRelic)
			{
				flag = num == 0 || num == 3;
				Vector2 vector;
				if (flag)
				{
					float num2 = (float)((num == 0) ? 1 : (-1));
					vector = num2 * Vector2.Down;
				}
				else
				{
					float num3 = (float)((num == 1) ? 1 : (-1));
					vector = num3 * Vector2.Left;
				}
				Rect2 viewportRect = base.GetViewportRect();
				Vector2 vector2 = viewportRect.Size / 2f + viewportRect.Size * vector;
				float num4;
				if (this._state == NHandImage.State.Frozen)
				{
					num4 = 0.25f;
				}
				else if (LocalContext.IsMe(this.Player))
				{
					num4 = 0.01f;
				}
				else
				{
					num4 = 0.07f;
				}
				Vector2 vector3 = vector2.Lerp(this._desiredPosition, this._handAnimateInProgress);
				base.GlobalPosition = MathHelper.SmoothDamp(base.GlobalPosition, vector3, ref this._currentVelocity, num4, (float)delta, float.PositiveInfinity);
			}
			if (this._state != NHandImage.State.None)
			{
				this._textureRect.Rotation = 0f;
				return;
			}
			flag = num == 0 || num == 3;
			if (flag)
			{
				float num5 = (float)((num == 0) ? 1 : (-1));
				this._textureRect.Rotation = num5 * (base.GlobalPosition.X - size.X / 2f) / 2000f;
				return;
			}
			float num6 = (float)((num == 1) ? 1 : (-1));
			this._textureRect.Rotation = num6 * (base.GlobalPosition.Y - size.Y / 2f) / 1000f;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001EE4 RID: 7908 RVA: 0x000CBDF0 File Offset: 0x000C9FF0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NHandImage.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandImage.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandImage.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandImage.MethodName.SetIsInFight, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "inFight", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImage.MethodName.SetFrozenForRelicAwards, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "frozenForRelicAwards", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImage.MethodName.GetFrozenPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandImage.MethodName.DoFightMove, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Tween"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "move", 0L, "", 6L, false),
					new PropertyInfo(3L, "duration", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImage.MethodName.SetTextureToFightMove, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "move", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImage.MethodName.SetPointingPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImage.MethodName.AnimateAway, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandImage.MethodName.AnimateIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandImage.MethodName.SetIsDown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isDown", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHandImage.MethodName.SetSkipped, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandImage.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001EE5 RID: 7909 RVA: 0x000CC1C8 File Offset: 0x000CA3C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHandImage.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImage.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImage.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImage.MethodName.SetIsInFight && args.Count == 1)
			{
				this.SetIsInFight(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImage.MethodName.SetFrozenForRelicAwards && args.Count == 1)
			{
				this.SetFrozenForRelicAwards(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImage.MethodName.GetFrozenPosition && args.Count == 0)
			{
				Vector2 frozenPosition = this.GetFrozenPosition();
				ret = VariantUtils.CreateFrom<Vector2>(ref frozenPosition);
				return true;
			}
			if ((ref method) == NHandImage.MethodName.DoFightMove && args.Count == 2)
			{
				Tween tween = this.DoFightMove(VariantUtils.ConvertTo<RelicPickingFightMove>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = VariantUtils.CreateFrom<Tween>(ref tween);
				return true;
			}
			if ((ref method) == NHandImage.MethodName.SetTextureToFightMove && args.Count == 1)
			{
				this.SetTextureToFightMove(VariantUtils.ConvertTo<RelicPickingFightMove>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImage.MethodName.SetPointingPosition && args.Count == 1)
			{
				this.SetPointingPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImage.MethodName.AnimateAway && args.Count == 0)
			{
				this.AnimateAway();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImage.MethodName.AnimateIn && args.Count == 0)
			{
				this.AnimateIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImage.MethodName.SetIsDown && args.Count == 1)
			{
				this.SetIsDown(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImage.MethodName.SetSkipped && args.Count == 0)
			{
				this.SetSkipped();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandImage.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001EE6 RID: 7910 RVA: 0x000CC464 File Offset: 0x000CA664
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHandImage.MethodName._Ready || (ref method) == NHandImage.MethodName._EnterTree || (ref method) == NHandImage.MethodName._ExitTree || (ref method) == NHandImage.MethodName.SetIsInFight || (ref method) == NHandImage.MethodName.SetFrozenForRelicAwards || (ref method) == NHandImage.MethodName.GetFrozenPosition || (ref method) == NHandImage.MethodName.DoFightMove || (ref method) == NHandImage.MethodName.SetTextureToFightMove || (ref method) == NHandImage.MethodName.SetPointingPosition || (ref method) == NHandImage.MethodName.AnimateAway || (ref method) == NHandImage.MethodName.AnimateIn || (ref method) == NHandImage.MethodName.SetIsDown || (ref method) == NHandImage.MethodName.SetSkipped || (ref method) == NHandImage.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001EE7 RID: 7911 RVA: 0x000CC54C File Offset: 0x000CA74C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHandImage.PropertyName.Index)
			{
				this.Index = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName.IsDown)
			{
				this.IsDown = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName.IsShown)
			{
				this.IsShown = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._grabMarker)
			{
				this._grabMarker = VariantUtils.ConvertTo<Marker2D>(ref value);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._textureRect)
			{
				this._textureRect = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._currentVelocity)
			{
				this._currentVelocity = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._desiredPosition)
			{
				this._desiredPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._downTween)
			{
				this._downTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._state)
			{
				this._state = VariantUtils.ConvertTo<NHandImage.State>(ref value);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._isInFight)
			{
				this._isInFight = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._originalPosition)
			{
				this._originalPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._handAnimateInProgress)
			{
				this._handAnimateInProgress = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001EE8 RID: 7912 RVA: 0x000CC6A8 File Offset: 0x000CA8A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHandImage.PropertyName.Index)
			{
				int index = this.Index;
				value = VariantUtils.CreateFrom<int>(ref index);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName.IsDown)
			{
				bool flag = this.IsDown;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName.IsShown)
			{
				bool flag = this.IsShown;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._grabMarker)
			{
				value = VariantUtils.CreateFrom<Marker2D>(ref this._grabMarker);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._textureRect)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._textureRect);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._currentVelocity)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._currentVelocity);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._desiredPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._desiredPosition);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._downTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._downTween);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._state)
			{
				value = VariantUtils.CreateFrom<NHandImage.State>(ref this._state);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._isInFight)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isInFight);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._originalPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._originalPosition);
				return true;
			}
			if ((ref name) == NHandImage.PropertyName._handAnimateInProgress)
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
		// Token: 0x06001EE9 RID: 7913 RVA: 0x000CC848 File Offset: 0x000CAA48
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NHandImage.PropertyName._grabMarker, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHandImage.PropertyName._textureRect, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHandImage.PropertyName._currentVelocity, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHandImage.PropertyName._desiredPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHandImage.PropertyName._downTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NHandImage.PropertyName._state, 0L, "", 4096L, false),
				new PropertyInfo(1L, NHandImage.PropertyName._isInFight, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHandImage.PropertyName._originalPosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NHandImage.PropertyName._handAnimateInProgress, 0L, "", 4096L, false),
				new PropertyInfo(2L, NHandImage.PropertyName.Index, 0L, "", 4096L, false),
				new PropertyInfo(1L, NHandImage.PropertyName.IsDown, 0L, "", 4096L, false),
				new PropertyInfo(1L, NHandImage.PropertyName.IsShown, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001EEA RID: 7914 RVA: 0x000CC9E0 File Offset: 0x000CABE0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName index = NHandImage.PropertyName.Index;
			int index2 = this.Index;
			info.AddProperty(index, Variant.From<int>(ref index2));
			StringName isDown = NHandImage.PropertyName.IsDown;
			bool flag = this.IsDown;
			info.AddProperty(isDown, Variant.From<bool>(ref flag));
			StringName isShown = NHandImage.PropertyName.IsShown;
			flag = this.IsShown;
			info.AddProperty(isShown, Variant.From<bool>(ref flag));
			info.AddProperty(NHandImage.PropertyName._grabMarker, Variant.From<Marker2D>(ref this._grabMarker));
			info.AddProperty(NHandImage.PropertyName._textureRect, Variant.From<TextureRect>(ref this._textureRect));
			info.AddProperty(NHandImage.PropertyName._currentVelocity, Variant.From<Vector2>(ref this._currentVelocity));
			info.AddProperty(NHandImage.PropertyName._desiredPosition, Variant.From<Vector2>(ref this._desiredPosition));
			info.AddProperty(NHandImage.PropertyName._downTween, Variant.From<Tween>(ref this._downTween));
			info.AddProperty(NHandImage.PropertyName._state, Variant.From<NHandImage.State>(ref this._state));
			info.AddProperty(NHandImage.PropertyName._isInFight, Variant.From<bool>(ref this._isInFight));
			info.AddProperty(NHandImage.PropertyName._originalPosition, Variant.From<Vector2>(ref this._originalPosition));
			info.AddProperty(NHandImage.PropertyName._handAnimateInProgress, Variant.From<float>(ref this._handAnimateInProgress));
		}

		/// <inheritdoc />
		// Token: 0x06001EEB RID: 7915 RVA: 0x000CCB08 File Offset: 0x000CAD08
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHandImage.PropertyName.Index, ref variant))
			{
				this.Index = variant.As<int>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHandImage.PropertyName.IsDown, ref variant2))
			{
				this.IsDown = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHandImage.PropertyName.IsShown, ref variant3))
			{
				this.IsShown = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NHandImage.PropertyName._grabMarker, ref variant4))
			{
				this._grabMarker = variant4.As<Marker2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NHandImage.PropertyName._textureRect, ref variant5))
			{
				this._textureRect = variant5.As<TextureRect>();
			}
			Variant variant6;
			if (info.TryGetProperty(NHandImage.PropertyName._currentVelocity, ref variant6))
			{
				this._currentVelocity = variant6.As<Vector2>();
			}
			Variant variant7;
			if (info.TryGetProperty(NHandImage.PropertyName._desiredPosition, ref variant7))
			{
				this._desiredPosition = variant7.As<Vector2>();
			}
			Variant variant8;
			if (info.TryGetProperty(NHandImage.PropertyName._downTween, ref variant8))
			{
				this._downTween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NHandImage.PropertyName._state, ref variant9))
			{
				this._state = variant9.As<NHandImage.State>();
			}
			Variant variant10;
			if (info.TryGetProperty(NHandImage.PropertyName._isInFight, ref variant10))
			{
				this._isInFight = variant10.As<bool>();
			}
			Variant variant11;
			if (info.TryGetProperty(NHandImage.PropertyName._originalPosition, ref variant11))
			{
				this._originalPosition = variant11.As<Vector2>();
			}
			Variant variant12;
			if (info.TryGetProperty(NHandImage.PropertyName._handAnimateInProgress, ref variant12))
			{
				this._handAnimateInProgress = variant12.As<float>();
			}
		}

		// Token: 0x04000BE0 RID: 3040
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/hand_image");

		// Token: 0x04000BE1 RID: 3041
		private static readonly Vector2 _pointingPivot = new Vector2(163f, 10f);

		// Token: 0x04000BE2 RID: 3042
		private static readonly Vector2 _fightingPivot = new Vector2(197f, 600f);

		// Token: 0x04000BE3 RID: 3043
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x04000BE4 RID: 3044
		private Marker2D _grabMarker;

		// Token: 0x04000BE5 RID: 3045
		private TextureRect _textureRect;

		// Token: 0x04000BE6 RID: 3046
		private Vector2 _currentVelocity;

		// Token: 0x04000BE7 RID: 3047
		private Vector2 _desiredPosition;

		// Token: 0x04000BE8 RID: 3048
		[Nullable(2)]
		private Tween _downTween;

		// Token: 0x04000BE9 RID: 3049
		private NHandImage.State _state;

		// Token: 0x04000BEA RID: 3050
		private bool _isInFight;

		// Token: 0x04000BEB RID: 3051
		private Vector2 _originalPosition;

		// Token: 0x04000BEC RID: 3052
		private float _handAnimateInProgress;

		// Token: 0x0200127C RID: 4732
		[NullableContext(0)]
		private enum State
		{
			// Token: 0x040047AD RID: 18349
			None,
			// Token: 0x040047AE RID: 18350
			Frozen,
			// Token: 0x040047AF RID: 18351
			GrabbingRelic
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200127D RID: 4733
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040047B0 RID: 18352
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040047B1 RID: 18353
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040047B2 RID: 18354
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SetIsInFight' method.
			/// </summary>
			// Token: 0x040047B3 RID: 18355
			public static readonly StringName SetIsInFight = "SetIsInFight";

			/// <summary>
			/// Cached name for the 'SetFrozenForRelicAwards' method.
			/// </summary>
			// Token: 0x040047B4 RID: 18356
			public static readonly StringName SetFrozenForRelicAwards = "SetFrozenForRelicAwards";

			/// <summary>
			/// Cached name for the 'GetFrozenPosition' method.
			/// </summary>
			// Token: 0x040047B5 RID: 18357
			public static readonly StringName GetFrozenPosition = "GetFrozenPosition";

			/// <summary>
			/// Cached name for the 'DoFightMove' method.
			/// </summary>
			// Token: 0x040047B6 RID: 18358
			public static readonly StringName DoFightMove = "DoFightMove";

			/// <summary>
			/// Cached name for the 'SetTextureToFightMove' method.
			/// </summary>
			// Token: 0x040047B7 RID: 18359
			public static readonly StringName SetTextureToFightMove = "SetTextureToFightMove";

			/// <summary>
			/// Cached name for the 'SetPointingPosition' method.
			/// </summary>
			// Token: 0x040047B8 RID: 18360
			public static readonly StringName SetPointingPosition = "SetPointingPosition";

			/// <summary>
			/// Cached name for the 'AnimateAway' method.
			/// </summary>
			// Token: 0x040047B9 RID: 18361
			public static readonly StringName AnimateAway = "AnimateAway";

			/// <summary>
			/// Cached name for the 'AnimateIn' method.
			/// </summary>
			// Token: 0x040047BA RID: 18362
			public static readonly StringName AnimateIn = "AnimateIn";

			/// <summary>
			/// Cached name for the 'SetIsDown' method.
			/// </summary>
			// Token: 0x040047BB RID: 18363
			public static readonly StringName SetIsDown = "SetIsDown";

			/// <summary>
			/// Cached name for the 'SetSkipped' method.
			/// </summary>
			// Token: 0x040047BC RID: 18364
			public static readonly StringName SetSkipped = "SetSkipped";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040047BD RID: 18365
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200127E RID: 4734
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Index' property.
			/// </summary>
			// Token: 0x040047BE RID: 18366
			public static readonly StringName Index = "Index";

			/// <summary>
			/// Cached name for the 'IsDown' property.
			/// </summary>
			// Token: 0x040047BF RID: 18367
			public static readonly StringName IsDown = "IsDown";

			/// <summary>
			/// Cached name for the 'IsShown' property.
			/// </summary>
			// Token: 0x040047C0 RID: 18368
			public static readonly StringName IsShown = "IsShown";

			/// <summary>
			/// Cached name for the '_grabMarker' field.
			/// </summary>
			// Token: 0x040047C1 RID: 18369
			public static readonly StringName _grabMarker = "_grabMarker";

			/// <summary>
			/// Cached name for the '_textureRect' field.
			/// </summary>
			// Token: 0x040047C2 RID: 18370
			public static readonly StringName _textureRect = "_textureRect";

			/// <summary>
			/// Cached name for the '_currentVelocity' field.
			/// </summary>
			// Token: 0x040047C3 RID: 18371
			public static readonly StringName _currentVelocity = "_currentVelocity";

			/// <summary>
			/// Cached name for the '_desiredPosition' field.
			/// </summary>
			// Token: 0x040047C4 RID: 18372
			public static readonly StringName _desiredPosition = "_desiredPosition";

			/// <summary>
			/// Cached name for the '_downTween' field.
			/// </summary>
			// Token: 0x040047C5 RID: 18373
			public static readonly StringName _downTween = "_downTween";

			/// <summary>
			/// Cached name for the '_state' field.
			/// </summary>
			// Token: 0x040047C6 RID: 18374
			public static readonly StringName _state = "_state";

			/// <summary>
			/// Cached name for the '_isInFight' field.
			/// </summary>
			// Token: 0x040047C7 RID: 18375
			public static readonly StringName _isInFight = "_isInFight";

			/// <summary>
			/// Cached name for the '_originalPosition' field.
			/// </summary>
			// Token: 0x040047C8 RID: 18376
			public static readonly StringName _originalPosition = "_originalPosition";

			/// <summary>
			/// Cached name for the '_handAnimateInProgress' field.
			/// </summary>
			// Token: 0x040047C9 RID: 18377
			public static readonly StringName _handAnimateInProgress = "_handAnimateInProgress";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200127F RID: 4735
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
