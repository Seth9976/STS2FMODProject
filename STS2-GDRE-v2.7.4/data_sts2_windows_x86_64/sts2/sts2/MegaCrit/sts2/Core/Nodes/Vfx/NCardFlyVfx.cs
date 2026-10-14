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
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// This vfx is called when cards fly from one spot to another, leaving a pretty trail.
	/// </summary>
	// Token: 0x0200018F RID: 399
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NCardFlyVfx.cs")]
	public class NCardFlyVfx : Node2D
	{
		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x0600136A RID: 4970 RVA: 0x00074CF4 File Offset: 0x00072EF4
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCardFlyVfx._scenePath);
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x00074D00 File Offset: 0x00072F00
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x00074D08 File Offset: 0x00072F08
		[Nullable(2)]
		public TaskCompletionSource SwooshAwayCompletion
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00074D14 File Offset: 0x00072F14
		[return: Nullable(2)]
		public static NCardFlyVfx Create(NCard card, PileType pileType, bool isAddingToPile, string trailPath)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCardFlyVfx ncardFlyVfx = PreloadManager.Cache.GetScene(NCardFlyVfx._scenePath).Instantiate<NCardFlyVfx>(0L);
			ncardFlyVfx._startPos = card.GlobalPosition;
			ncardFlyVfx._endPos = pileType.GetTargetPosition(card);
			ncardFlyVfx._card = card;
			ncardFlyVfx._isAddingToPile = isAddingToPile;
			ncardFlyVfx._trailPath = trailPath;
			return ncardFlyVfx;
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00074D70 File Offset: 0x00072F70
		[return: Nullable(2)]
		public static NCardFlyVfx Create(NCard card, Creature target, string trailPath)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (NCombatRoom.Instance == null)
			{
				return null;
			}
			NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
			if (creatureNode == null)
			{
				return null;
			}
			NCardFlyVfx ncardFlyVfx = PreloadManager.Cache.GetScene(NCardFlyVfx._scenePath).Instantiate<NCardFlyVfx>(0L);
			ncardFlyVfx._startPos = card.GlobalPosition;
			ncardFlyVfx._endPos = creatureNode.VfxSpawnPosition;
			ncardFlyVfx._card = card;
			ncardFlyVfx._isAddingToPile = false;
			ncardFlyVfx._trailPath = trailPath;
			return ncardFlyVfx;
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00074DE8 File Offset: 0x00072FE8
		public override void _Ready()
		{
			this._vfx = NCardTrailVfx.Create(this._card, this._trailPath);
			if (this._vfx != null)
			{
				base.GetParent().AddChildSafely(this._vfx);
			}
			this._controlPointOffset = Rng.Chaotic.NextFloat(100f, 400f);
			this._speed = Rng.Chaotic.NextFloat(1.1f, 1.25f);
			this._accel = Rng.Chaotic.NextFloat(2f, 2.5f);
			this._arcDir = ((this._endPos.Y < base.GetViewportRect().Size.Y * 0.5f) ? (-500f) : (500f + this._controlPointOffset));
			this._duration = Rng.Chaotic.NextFloat(1f, 1.75f);
			this._card.Connect(Node.SignalName.TreeExited, Callable.From(new Action(this.OnCardExitedTree)), 0U);
			if (NCombatUi.IsDebugHidingPlayContainer)
			{
				this._card.Modulate = Colors.Transparent;
				this._card.Visible = false;
				base.Visible = false;
			}
			TaskHelper.RunSafely(this.PlayAnim());
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00074F25 File Offset: 0x00073125
		public override void _ExitTree()
		{
			Tween fadeOutTween = this._fadeOutTween;
			if (fadeOutTween != null)
			{
				fadeOutTween.Kill();
			}
			this._cancelToken.Cancel();
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00074F44 File Offset: 0x00073144
		private void OnCardExitedTree()
		{
			try
			{
				NCardTrailVfx vfx = this._vfx;
				if (vfx != null)
				{
					vfx.QueueFreeSafely();
				}
			}
			catch (ObjectDisposedException)
			{
			}
			TaskCompletionSource swooshAwayCompletion = this.SwooshAwayCompletion;
			if (swooshAwayCompletion != null)
			{
				swooshAwayCompletion.TrySetResult();
			}
			this.QueueFreeSafely();
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x00074F90 File Offset: 0x00073190
		private async Task PlayAnim()
		{
			CardPile pile = this._card.Model.Pile;
			if (pile != null)
			{
				SfxCmd.PlayCardSwooshSfx(pile, null);
			}
			this.SwooshAwayCompletion = new TaskCompletionSource();
			float time = 0f;
			while (time / this._duration <= 1f)
			{
				await this.AwaitProcessFrame(default(CancellationToken));
				if (this._cancelToken.IsCancellationRequested)
				{
					TaskCompletionSource swooshAwayCompletion = this.SwooshAwayCompletion;
					if (swooshAwayCompletion != null)
					{
						swooshAwayCompletion.SetResult();
					}
					return;
				}
				float num = (float)base.GetProcessDeltaTime();
				time += this._speed * num;
				this._speed += this._accel * num;
				Vector2 vector = this._startPos + (this._endPos - this._startPos) * 0.5f;
				vector.Y -= this._arcDir;
				Vector2 vector2 = MathHelper.BezierCurve(this._startPos, this._endPos, vector, (time + 0.05f) / this._duration);
				this._card.GlobalPosition = MathHelper.BezierCurve(this._startPos, this._endPos, vector, time / this._duration);
				float num2 = (vector2 - this._card.GlobalPosition).Angle() + 1.5707964f;
				Node parent = this._card.GetParent();
				Control control = parent as Control;
				if (control != null)
				{
					num2 -= control.Rotation;
				}
				else
				{
					Node2D node2D = parent as Node2D;
					if (node2D != null)
					{
						num2 -= node2D.Rotation;
					}
				}
				this._card.Rotation = Mathf.LerpAngle(this._card.Rotation, num2, num * 12f);
				this._card.Body.Modulate = Colors.White.Lerp(Colors.Black, Mathf.Clamp(time * 3f / this._duration, 0f, 1f));
				this._card.Body.Scale = Vector2.One * Mathf.Lerp(1f, 0.1f, Mathf.Clamp(time * 3f / this._duration, 0f, 1f));
			}
			this._card.GlobalPosition = this._endPos;
			if (this._isAddingToPile)
			{
				CardPile pile2 = this._card.Model.Pile;
				if (pile2 != null)
				{
					pile2.InvokeCardAddFinished();
				}
			}
			time = 0f;
			while (time / this._duration <= 1f)
			{
				await this.AwaitProcessFrame(default(CancellationToken));
				if (this._cancelToken.IsCancellationRequested)
				{
					TaskCompletionSource swooshAwayCompletion2 = this.SwooshAwayCompletion;
					if (swooshAwayCompletion2 != null)
					{
						swooshAwayCompletion2.SetResult();
					}
					return;
				}
				float num3 = (float)base.GetProcessDeltaTime();
				time += this._speed * num3;
				if (time / this._duration > 0.25f && !this._vfxFading)
				{
					if (this._vfx != null)
					{
						TaskHelper.RunSafely(this._vfx.FadeOut());
					}
					this._vfxFading = true;
				}
				this._card.Body.Scale = Vector2.One * Mathf.Max(Mathf.Lerp(0.1f, -0.15f, time / this._duration), 0f);
			}
			TaskCompletionSource swooshAwayCompletion3 = this.SwooshAwayCompletion;
			if (swooshAwayCompletion3 != null)
			{
				swooshAwayCompletion3.SetResult();
			}
			this._card.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001373 RID: 4979 RVA: 0x00074FD4 File Offset: 0x000731D4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCardFlyVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(2L, "pileType", 0L, "", 6L, false),
					new PropertyInfo(1L, "isAddingToPile", 0L, "", 6L, false),
					new PropertyInfo(4L, "trailPath", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardFlyVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardFlyVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardFlyVfx.MethodName.OnCardExitedTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001374 RID: 4980 RVA: 0x00075144 File Offset: 0x00073344
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardFlyVfx.MethodName.Create && args.Count == 4)
			{
				NCardFlyVfx ncardFlyVfx = NCardFlyVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<PileType>(args[1]), VariantUtils.ConvertTo<bool>(args[2]), VariantUtils.ConvertTo<string>(args[3]));
				ret = VariantUtils.CreateFrom<NCardFlyVfx>(ref ncardFlyVfx);
				return true;
			}
			if ((ref method) == NCardFlyVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardFlyVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardFlyVfx.MethodName.OnCardExitedTree && args.Count == 0)
			{
				this.OnCardExitedTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0007522C File Offset: 0x0007342C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardFlyVfx.MethodName.Create && args.Count == 4)
			{
				NCardFlyVfx ncardFlyVfx = NCardFlyVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<PileType>(args[1]), VariantUtils.ConvertTo<bool>(args[2]), VariantUtils.ConvertTo<string>(args[3]));
				ret = VariantUtils.CreateFrom<NCardFlyVfx>(ref ncardFlyVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001376 RID: 4982 RVA: 0x000752A4 File Offset: 0x000734A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardFlyVfx.MethodName.Create || (ref method) == NCardFlyVfx.MethodName._Ready || (ref method) == NCardFlyVfx.MethodName._ExitTree || (ref method) == NCardFlyVfx.MethodName.OnCardExitedTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001377 RID: 4983 RVA: 0x000752F4 File Offset: 0x000734F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardFlyVfx.PropertyName._card)
			{
				this._card = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._trailPath)
			{
				this._trailPath = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._vfx)
			{
				this._vfx = VariantUtils.ConvertTo<NCardTrailVfx>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._fadeOutTween)
			{
				this._fadeOutTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._vfxFading)
			{
				this._vfxFading = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._isAddingToPile)
			{
				this._isAddingToPile = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._startPos)
			{
				this._startPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._endPos)
			{
				this._endPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._controlPointOffset)
			{
				this._controlPointOffset = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._duration)
			{
				this._duration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._speed)
			{
				this._speed = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._accel)
			{
				this._accel = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._arcDir)
			{
				this._arcDir = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001378 RID: 4984 RVA: 0x00075468 File Offset: 0x00073668
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardFlyVfx.PropertyName._card)
			{
				value = VariantUtils.CreateFrom<NCard>(ref this._card);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._trailPath)
			{
				value = VariantUtils.CreateFrom<string>(ref this._trailPath);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._vfx)
			{
				value = VariantUtils.CreateFrom<NCardTrailVfx>(ref this._vfx);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._fadeOutTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._fadeOutTween);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._vfxFading)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._vfxFading);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._isAddingToPile)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isAddingToPile);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._startPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._startPos);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._endPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._endPos);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._controlPointOffset)
			{
				value = VariantUtils.CreateFrom<float>(ref this._controlPointOffset);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._duration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._duration);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._speed)
			{
				value = VariantUtils.CreateFrom<float>(ref this._speed);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._accel)
			{
				value = VariantUtils.CreateFrom<float>(ref this._accel);
				return true;
			}
			if ((ref name) == NCardFlyVfx.PropertyName._arcDir)
			{
				value = VariantUtils.CreateFrom<float>(ref this._arcDir);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001379 RID: 4985 RVA: 0x00075620 File Offset: 0x00073820
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardFlyVfx.PropertyName._card, 0L, "", 4096L, false),
				new PropertyInfo(4L, NCardFlyVfx.PropertyName._trailPath, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardFlyVfx.PropertyName._vfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardFlyVfx.PropertyName._fadeOutTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardFlyVfx.PropertyName._vfxFading, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardFlyVfx.PropertyName._isAddingToPile, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCardFlyVfx.PropertyName._startPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCardFlyVfx.PropertyName._endPos, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardFlyVfx.PropertyName._controlPointOffset, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardFlyVfx.PropertyName._duration, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardFlyVfx.PropertyName._speed, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardFlyVfx.PropertyName._accel, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardFlyVfx.PropertyName._arcDir, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600137A RID: 4986 RVA: 0x000757D8 File Offset: 0x000739D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardFlyVfx.PropertyName._card, Variant.From<NCard>(ref this._card));
			info.AddProperty(NCardFlyVfx.PropertyName._trailPath, Variant.From<string>(ref this._trailPath));
			info.AddProperty(NCardFlyVfx.PropertyName._vfx, Variant.From<NCardTrailVfx>(ref this._vfx));
			info.AddProperty(NCardFlyVfx.PropertyName._fadeOutTween, Variant.From<Tween>(ref this._fadeOutTween));
			info.AddProperty(NCardFlyVfx.PropertyName._vfxFading, Variant.From<bool>(ref this._vfxFading));
			info.AddProperty(NCardFlyVfx.PropertyName._isAddingToPile, Variant.From<bool>(ref this._isAddingToPile));
			info.AddProperty(NCardFlyVfx.PropertyName._startPos, Variant.From<Vector2>(ref this._startPos));
			info.AddProperty(NCardFlyVfx.PropertyName._endPos, Variant.From<Vector2>(ref this._endPos));
			info.AddProperty(NCardFlyVfx.PropertyName._controlPointOffset, Variant.From<float>(ref this._controlPointOffset));
			info.AddProperty(NCardFlyVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
			info.AddProperty(NCardFlyVfx.PropertyName._speed, Variant.From<float>(ref this._speed));
			info.AddProperty(NCardFlyVfx.PropertyName._accel, Variant.From<float>(ref this._accel));
			info.AddProperty(NCardFlyVfx.PropertyName._arcDir, Variant.From<float>(ref this._arcDir));
		}

		/// <inheritdoc />
		// Token: 0x0600137B RID: 4987 RVA: 0x0007590C File Offset: 0x00073B0C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._card, ref variant))
			{
				this._card = variant.As<NCard>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._trailPath, ref variant2))
			{
				this._trailPath = variant2.As<string>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._vfx, ref variant3))
			{
				this._vfx = variant3.As<NCardTrailVfx>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._fadeOutTween, ref variant4))
			{
				this._fadeOutTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._vfxFading, ref variant5))
			{
				this._vfxFading = variant5.As<bool>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._isAddingToPile, ref variant6))
			{
				this._isAddingToPile = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._startPos, ref variant7))
			{
				this._startPos = variant7.As<Vector2>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._endPos, ref variant8))
			{
				this._endPos = variant8.As<Vector2>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._controlPointOffset, ref variant9))
			{
				this._controlPointOffset = variant9.As<float>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._duration, ref variant10))
			{
				this._duration = variant10.As<float>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._speed, ref variant11))
			{
				this._speed = variant11.As<float>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._accel, ref variant12))
			{
				this._accel = variant12.As<float>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCardFlyVfx.PropertyName._arcDir, ref variant13))
			{
				this._arcDir = variant13.As<float>();
			}
		}

		// Token: 0x040006EE RID: 1774
		private NCard _card;

		// Token: 0x040006EF RID: 1775
		private string _trailPath;

		// Token: 0x040006F0 RID: 1776
		[Nullable(2)]
		private NCardTrailVfx _vfx;

		// Token: 0x040006F1 RID: 1777
		[Nullable(2)]
		private Tween _fadeOutTween;

		// Token: 0x040006F2 RID: 1778
		private bool _vfxFading;

		// Token: 0x040006F3 RID: 1779
		private bool _isAddingToPile;

		// Token: 0x040006F4 RID: 1780
		private Vector2 _startPos;

		// Token: 0x040006F5 RID: 1781
		private Vector2 _endPos;

		// Token: 0x040006F6 RID: 1782
		private float _controlPointOffset;

		// Token: 0x040006F7 RID: 1783
		private float _duration;

		// Token: 0x040006F8 RID: 1784
		private float _speed;

		// Token: 0x040006F9 RID: 1785
		private float _accel;

		// Token: 0x040006FA RID: 1786
		private float _arcDir;

		// Token: 0x040006FB RID: 1787
		private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

		// Token: 0x040006FC RID: 1788
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_card_fly");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FC5 RID: 4037
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003EC8 RID: 16072
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003EC9 RID: 16073
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003ECA RID: 16074
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnCardExitedTree' method.
			/// </summary>
			// Token: 0x04003ECB RID: 16075
			public static readonly StringName OnCardExitedTree = "OnCardExitedTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FC6 RID: 4038
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_card' field.
			/// </summary>
			// Token: 0x04003ECC RID: 16076
			public static readonly StringName _card = "_card";

			/// <summary>
			/// Cached name for the '_trailPath' field.
			/// </summary>
			// Token: 0x04003ECD RID: 16077
			public static readonly StringName _trailPath = "_trailPath";

			/// <summary>
			/// Cached name for the '_vfx' field.
			/// </summary>
			// Token: 0x04003ECE RID: 16078
			public static readonly StringName _vfx = "_vfx";

			/// <summary>
			/// Cached name for the '_fadeOutTween' field.
			/// </summary>
			// Token: 0x04003ECF RID: 16079
			public static readonly StringName _fadeOutTween = "_fadeOutTween";

			/// <summary>
			/// Cached name for the '_vfxFading' field.
			/// </summary>
			// Token: 0x04003ED0 RID: 16080
			public static readonly StringName _vfxFading = "_vfxFading";

			/// <summary>
			/// Cached name for the '_isAddingToPile' field.
			/// </summary>
			// Token: 0x04003ED1 RID: 16081
			public static readonly StringName _isAddingToPile = "_isAddingToPile";

			/// <summary>
			/// Cached name for the '_startPos' field.
			/// </summary>
			// Token: 0x04003ED2 RID: 16082
			public static readonly StringName _startPos = "_startPos";

			/// <summary>
			/// Cached name for the '_endPos' field.
			/// </summary>
			// Token: 0x04003ED3 RID: 16083
			public static readonly StringName _endPos = "_endPos";

			/// <summary>
			/// Cached name for the '_controlPointOffset' field.
			/// </summary>
			// Token: 0x04003ED4 RID: 16084
			public static readonly StringName _controlPointOffset = "_controlPointOffset";

			/// <summary>
			/// Cached name for the '_duration' field.
			/// </summary>
			// Token: 0x04003ED5 RID: 16085
			public static readonly StringName _duration = "_duration";

			/// <summary>
			/// Cached name for the '_speed' field.
			/// </summary>
			// Token: 0x04003ED6 RID: 16086
			public static readonly StringName _speed = "_speed";

			/// <summary>
			/// Cached name for the '_accel' field.
			/// </summary>
			// Token: 0x04003ED7 RID: 16087
			public static readonly StringName _accel = "_accel";

			/// <summary>
			/// Cached name for the '_arcDir' field.
			/// </summary>
			// Token: 0x04003ED8 RID: 16088
			public static readonly StringName _arcDir = "_arcDir";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FC7 RID: 4039
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
