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
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Manages full card transformations that take place in the center of the screen
	/// (ie transformations for cards in the deck/draw/discard)
	/// Centers the card and plays the transform animation. The actual transform animation
	/// logic lives in NCardTransformShineVfx
	/// </summary>
	// Token: 0x02000193 RID: 403
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NCardTransformVfx.cs")]
	public class NCardTransformVfx : Node2D
	{
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x0007728F File Offset: 0x0007548F
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_card_transform");
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x0007729B File Offset: 0x0007549B
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCardTransformVfx.ScenePath);
			}
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x000772A8 File Offset: 0x000754A8
		[return: Nullable(2)]
		public static NCardTransformVfx Create(CardModel startCard, CardModel endCard, [Nullable(new byte[] { 2, 1 })] IEnumerable<RelicModel> relicsToFlash)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCardTransformVfx ncardTransformVfx = PreloadManager.Cache.GetScene(NCardTransformVfx.ScenePath).Instantiate<NCardTransformVfx>(0L);
			ncardTransformVfx._startCard = startCard;
			ncardTransformVfx._endCard = endCard;
			ncardTransformVfx._relicsToFlash = relicsToFlash;
			return ncardTransformVfx;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x000772EB File Offset: 0x000754EB
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlayAnimation());
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x000772FC File Offset: 0x000754FC
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

		// Token: 0x060013BD RID: 5053 RVA: 0x0007734F File Offset: 0x0007554F
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00077364 File Offset: 0x00075564
		private async Task PlayAnimation()
		{
			SfxCmd.Play("event:/sfx/ui/cards/card_transform", 1f);
			Control node = base.GetNode<Control>("%CardContainer");
			NCard cardNode = NCard.Create(this._startCard, ModelVisibility.Visible);
			node.AddChildSafely(cardNode);
			cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
			this._tween = base.CreateTween();
			this._tween.TweenProperty(cardNode, "scale", Vector2.One * 1f, 0.25).From(Vector2.Zero).SetEase(1L)
				.SetTrans(7L);
			TaskAwaiter<bool> taskAwaiter = this.WaitAndInterruptIfNecessary(0.75f, cardNode).GetAwaiter();
			TaskAwaiter<bool> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				this.QueueFreeSafely();
			}
			else
			{
				NCardTransformShineVfx ncardTransformShineVfx = NCardTransformShineVfx.Create(cardNode, this._endCard, this._relicsToFlash);
				if (ncardTransformShineVfx != null)
				{
					await ncardTransformShineVfx.PlayAnimation(false);
				}
				TaskAwaiter<bool> taskAwaiter3 = this.WaitAndInterruptIfNecessary(0.3f, cardNode).GetAwaiter();
				if (!taskAwaiter3.IsCompleted)
				{
					await taskAwaiter3;
					taskAwaiter3 = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter3.GetResult())
				{
					this.QueueFreeSafely();
				}
				else
				{
					if (this._relicsToFlash != null)
					{
						foreach (RelicModel relicModel in this._relicsToFlash)
						{
							relicModel.Flash();
							cardNode.FlashRelicOnCard(relicModel);
						}
					}
					TaskAwaiter<bool> taskAwaiter4 = this.WaitAndInterruptIfNecessary(0.5f, cardNode).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						await taskAwaiter4;
						taskAwaiter4 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter4.GetResult())
					{
						this.QueueFreeSafely();
					}
					else if (this._endCard.Pile == null)
					{
						this.QueueFreeSafely();
					}
					else
					{
						cardNode.Reparent(this, true);
						cardNode.Position = Vector2.Zero;
						NCardFlyVfx ncardFlyVfx = NCardFlyVfx.Create(cardNode, this._endCard.Pile.Type, false, this._endCard.Owner.Character.TrailPath);
						Node node2;
						if (this._endCard.Pile.Type == PileType.Deck)
						{
							NRun instance = NRun.Instance;
							node2 = ((instance != null) ? instance.GlobalUi.TopBar.TrailContainer : null);
						}
						else
						{
							NCombatRoom instance2 = NCombatRoom.Instance;
							node2 = ((instance2 != null) ? instance2.CombatVfxContainer : null);
						}
						if (node2 != null)
						{
							node2.AddChildSafely(ncardFlyVfx);
						}
						if (((ncardFlyVfx != null) ? ncardFlyVfx.SwooshAwayCompletion : null) != null)
						{
							await ncardFlyVfx.SwooshAwayCompletion.Task;
						}
						this.QueueFreeSafely();
					}
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060013BF RID: 5055 RVA: 0x000773A8 File Offset: 0x000755A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCardTransformVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardTransformVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013C0 RID: 5056 RVA: 0x0007741C File Offset: 0x0007561C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardTransformVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTransformVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060013C1 RID: 5057 RVA: 0x0007747C File Offset: 0x0007567C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardTransformVfx.MethodName._Ready || (ref method) == NCardTransformVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060013C2 RID: 5058 RVA: 0x000774A3 File Offset: 0x000756A3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardTransformVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060013C3 RID: 5059 RVA: 0x000774C8 File Offset: 0x000756C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardTransformVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060013C4 RID: 5060 RVA: 0x000774F4 File Offset: 0x000756F4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardTransformVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013C5 RID: 5061 RVA: 0x00077529 File Offset: 0x00075729
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardTransformVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x060013C6 RID: 5062 RVA: 0x00077548 File Offset: 0x00075748
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardTransformVfx.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
		}

		// Token: 0x0400070F RID: 1807
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000710 RID: 1808
		private CardModel _startCard;

		// Token: 0x04000711 RID: 1809
		private CardModel _endCard;

		// Token: 0x04000712 RID: 1810
		[Nullable(new byte[] { 2, 1 })]
		private IEnumerable<RelicModel> _relicsToFlash;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FD6 RID: 4054
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F0A RID: 16138
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003F0B RID: 16139
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FD7 RID: 4055
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003F0C RID: 16140
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FD8 RID: 4056
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
