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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000194 RID: 404
	[ScriptPath("res://src/Core/Nodes/Vfx/NCardUpgradeVfx.cs")]
	public class NCardUpgradeVfx : Node2D
	{
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x00077580 File Offset: 0x00075780
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_card_upgrade");
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x0007758C File Offset: 0x0007578C
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCardUpgradeVfx.ScenePath);
			}
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00077598 File Offset: 0x00075798
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NCardUpgradeVfx Create(CardModel card)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCardUpgradeVfx ncardUpgradeVfx = PreloadManager.Cache.GetScene(NCardUpgradeVfx.ScenePath).Instantiate<NCardUpgradeVfx>(0L);
			ncardUpgradeVfx._card = card;
			return ncardUpgradeVfx;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x000775CD File Offset: 0x000757CD
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlayAnimation());
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x000775DB File Offset: 0x000757DB
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x000775F0 File Offset: 0x000757F0
		[NullableContext(1)]
		private async Task PlayAnimation()
		{
			this._cts = new CancellationTokenSource();
			NCard cardNode = NCard.Create(this._card, ModelVisibility.Visible);
			this.AddChildSafely(cardNode);
			this.MoveChildSafely(cardNode, 0);
			cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
			base.GetNode<CpuParticles2D>("%Particle").Emitting = true;
			PileType pileType = this._card.Pile.Type;
			Tween tween = base.CreateTween();
			tween.TweenProperty(cardNode, "scale", Vector2.One * 1f, 0.25).From(Vector2.Zero).SetEase(1L)
				.SetTrans(7L);
			await Cmd.Wait(1.75f, this._cts.Token, false);
			if (this._card.Pile != null)
			{
				pileType = this._card.Pile.Type;
			}
			NCardFlyVfx ncardFlyVfx = NCardFlyVfx.Create(cardNode, pileType, false, this._card.Owner.Character.TrailPath);
			Node node;
			if (pileType == PileType.Deck)
			{
				NRun instance = NRun.Instance;
				node = ((instance != null) ? instance.GlobalUi.TopBar.TrailContainer : null);
			}
			else
			{
				NCombatRoom instance2 = NCombatRoom.Instance;
				node = ((instance2 != null) ? instance2.CombatVfxContainer : null);
			}
			if (node != null)
			{
				node.AddChildSafely(ncardFlyVfx);
			}
			if (((ncardFlyVfx != null) ? ncardFlyVfx.SwooshAwayCompletion : null) != null)
			{
				await ncardFlyVfx.SwooshAwayCompletion.Task;
			}
			if (!this._cts.IsCancellationRequested)
			{
				this.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060013CE RID: 5070 RVA: 0x00077634 File Offset: 0x00075834
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCardUpgradeVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardUpgradeVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013CF RID: 5071 RVA: 0x000776A8 File Offset: 0x000758A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardUpgradeVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardUpgradeVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060013D0 RID: 5072 RVA: 0x00077708 File Offset: 0x00075908
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardUpgradeVfx.MethodName._Ready || (ref method) == NCardUpgradeVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060013D1 RID: 5073 RVA: 0x0007772F File Offset: 0x0007592F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060013D2 RID: 5074 RVA: 0x00077738 File Offset: 0x00075938
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000713 RID: 1811
		[Nullable(1)]
		private CardModel _card;

		// Token: 0x04000714 RID: 1812
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FDB RID: 4059
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F1A RID: 16154
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003F1B RID: 16155
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FDC RID: 4060
		public class PropertyName : Node2D.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FDD RID: 4061
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
