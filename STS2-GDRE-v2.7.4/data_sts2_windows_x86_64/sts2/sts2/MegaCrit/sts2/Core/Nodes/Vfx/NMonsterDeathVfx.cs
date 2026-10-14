using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001D5 RID: 469
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NMonsterDeathVfx.cs")]
	public class NMonsterDeathVfx : Node2D
	{
		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060017B1 RID: 6065 RVA: 0x00093E37 File Offset: 0x00092037
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_monster_death");
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060017B2 RID: 6066 RVA: 0x00093E43 File Offset: 0x00092043
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NMonsterDeathVfx.ScenePath);
			}
		}

		/// <summary>
		/// Creates an instance of NMonsterDeathVfx for the specified creature.
		/// </summary>
		/// <param name="creatureNode">The creature node.</param>
		/// <param name="cancelToken">The cancellation token.</param>
		/// <returns>The created NMonsterDeathVfx instance, or null if cancellation is requested or NCombatRoom.Instance is null.</returns>
		// Token: 0x060017B3 RID: 6067 RVA: 0x00093E50 File Offset: 0x00092050
		[return: Nullable(2)]
		public unsafe static NMonsterDeathVfx Create(NCreature creatureNode, CancellationToken cancelToken)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
			{
				return null;
			}
			if (cancelToken.IsCancellationRequested)
			{
				return null;
			}
			NMonsterDeathVfx nmonsterDeathVfx = PreloadManager.Cache.GetScene(NMonsterDeathVfx.ScenePath).Instantiate<NMonsterDeathVfx>(0L);
			NMonsterDeathVfx nmonsterDeathVfx2 = nmonsterDeathVfx;
			int num = 1;
			List<NCreature> list = new List<NCreature>(num);
			CollectionsMarshal.SetCount<NCreature>(list, num);
			Span<NCreature> span = CollectionsMarshal.AsSpan<NCreature>(list);
			int num2 = 0;
			*span[num2] = creatureNode;
			nmonsterDeathVfx2._creatureNodes = list;
			nmonsterDeathVfx._cancelToken = cancelToken;
			NMonsterDeathVfx nmonsterDeathVfx3 = nmonsterDeathVfx;
			num2 = 1;
			List<Control> list2 = new List<Control>(num2);
			CollectionsMarshal.SetCount<Control>(list2, num2);
			Span<Control> span2 = CollectionsMarshal.AsSpan<Control>(list2);
			num = 0;
			*span2[num] = creatureNode.Hitbox;
			nmonsterDeathVfx3._hitboxes = list2;
			return nmonsterDeathVfx;
		}

		/// <summary>
		/// Creates an instance of NMonsterDeathVfx for the specified creature.
		/// This should be used on creatures like the Decimillipede that are made of several different creatures that fade
		/// out together.
		/// </summary>
		/// <param name="creatureNodes">The creature nodes.</param>
		/// <returns>The created NMonsterDeathVfx instance, or null if cancellation is requested or NCombatRoom.Instance is null.</returns>
		// Token: 0x060017B4 RID: 6068 RVA: 0x00093EFC File Offset: 0x000920FC
		[return: Nullable(2)]
		public static NMonsterDeathVfx Create(List<NCreature> creatureNodes)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NMonsterDeathVfx nmonsterDeathVfx = PreloadManager.Cache.GetScene(NMonsterDeathVfx.ScenePath).Instantiate<NMonsterDeathVfx>(0L);
			nmonsterDeathVfx._creatureNodes = creatureNodes;
			nmonsterDeathVfx._cancelToken = default(CancellationToken);
			nmonsterDeathVfx._hitboxes = creatureNodes.Select<NCreature, Control>((NCreature c) => c.Hitbox).ToList<Control>();
			return nmonsterDeathVfx;
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00093F70 File Offset: 0x00092170
		public async Task PlayVfx()
		{
			if (!this._cancelToken.IsCancellationRequested)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance == null)
				{
					this.QueueFreeSafely();
				}
				else
				{
					SubViewport node = base.GetNode<SubViewport>("Viewport");
					Rect2? rect = null;
					for (int i = 0; i < this._creatureNodes.Count; i++)
					{
						NCreature ncreature = this._creatureNodes[i];
						NCreatureVisuals visuals = ncreature.Visuals;
						Vector2 scale = visuals.GetCurrentBody().Scale;
						MonsterModel monster = ncreature.Entity.Monster;
						Vector2 vector = ((monster != null) ? monster.ExtraDeathVfxPadding : MonsterModel.defaultDeathVfxPadding);
						if (visuals != null && visuals.HasSpineAnimation && !visuals.IsUsingPhobiaModeBody)
						{
							MegaSprite spineBody = visuals.SpineBody;
							MegaSkeleton skeleton = spineBody.GetSkeleton();
							if (skeleton != null)
							{
								Vector2 scale2 = instance.SceneContainer.Scale;
								Rect2 bounds = skeleton.GetBounds();
								Rect2 rect2;
								rect2..ctor(bounds.Position * scale * scale2, bounds.Size * scale * scale2);
								Vector2 vector2;
								vector2..ctor(Math.Min(rect2.Position.X, rect2.End.X), Math.Min(rect2.Position.Y, rect2.End.Y));
								Vector2 vector3;
								vector3..ctor(Math.Max(rect2.Position.X, rect2.End.X), Math.Max(rect2.Position.Y, rect2.End.Y));
								Vector2 vector4 = vector3 - vector2;
								Vector2 vector5 = vector4 * vector;
								Vector2 vector6 = (vector5 - vector4) * 0.5f;
								Rect2 rect3;
								rect3..ctor(visuals.GetCurrentBody().GlobalPosition + vector2 - vector6, vector5);
								if (rect == null)
								{
									rect = new Rect2?(rect3);
								}
								else
								{
									rect = new Rect2?(rect.Value.Merge(rect3));
								}
							}
						}
						else
						{
							Control control = this._hitboxes[i];
							Vector2 vector7 = control.Size * control.Scale;
							Vector2 vector8 = vector7 * vector;
							Vector2 vector9 = (vector8 - vector7) * 0.5f;
							Rect2 rect4;
							rect4..ctor(control.GlobalPosition - vector9, vector8);
							if (rect == null)
							{
								rect = new Rect2?(rect4);
							}
							else
							{
								rect = new Rect2?(rect.Value.Merge(rect4));
							}
						}
					}
					base.GlobalPosition = rect.Value.Position + rect.Value.Size * 0.5f;
					Vector2 size = rect.Value.Size;
					int num = Mathf.RoundToInt(Mathf.Max(size.X, size.Y));
					size..ctor((float)num, (float)num);
					node.Size = new Vector2I(num, num) * 2;
					node.Size2DOverride = new Vector2I(num, num);
					Sprite2D node2 = base.GetNode<Sprite2D>("Visual");
					node2.Scale /= 2f * instance.SceneContainer.Scale;
					Vector2 vector10 = base.GlobalPosition - size * 0.5f;
					foreach (NCreature ncreature2 in this._creatureNodes)
					{
						if (GodotObject.IsInstanceValid(ncreature2.Visuals.GetCurrentBody()))
						{
							Vector2 globalPosition = ncreature2.Visuals.GetCurrentBody().GlobalPosition;
							ncreature2.Visuals.GetCurrentBody().Reparent(node, true);
							if (GodotObject.IsInstanceValid(ncreature2.Visuals.GetCurrentBody()))
							{
								ncreature2.Visuals.GetCurrentBody().Position = globalPosition - vector10;
							}
						}
					}
					node.RenderTargetUpdateMode = 1L;
					await this.PlayVfxInternal();
				}
			}
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00093FB4 File Offset: 0x000921B4
		private async Task PlayVfxInternal()
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_fade", 1f);
			SubViewport node = base.GetNode<SubViewport>("Viewport");
			Sprite2D node2 = base.GetNode<Sprite2D>("Visual");
			CpuParticles2D node3 = base.GetNode<CpuParticles2D>("%Particles");
			node3.EmissionSphereRadius = (float)node.Size.Y / 4f;
			node3.Emitting = true;
			float num = 0.1f * base.GetViewportRect().Size.X;
			float num2 = Math.Min((float)node.Size.X / num * 2.5f, 2.5f);
			using (Tween tween = base.CreateTween())
			{
				tween.TweenProperty(node2.Material, "shader_parameter/threshold", 0f, (double)num2).SetEase(1L).SetTrans(1L);
				bool flag = await tween.AwaitFinished(this);
				this.QueueFreeSafely();
			}
		}

		/// <inheritdoc />
		// Token: 0x060017B7 RID: 6071 RVA: 0x00093FF7 File Offset: 0x000921F7
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060017B8 RID: 6072 RVA: 0x00094000 File Offset: 0x00092200
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x0400088A RID: 2186
		private const string _deathSfx = "event:/sfx/enemy/enemy_fade";

		// Token: 0x0400088B RID: 2187
		private const float _refLength = 0.1f;

		// Token: 0x0400088C RID: 2188
		private const float _refTweenDuration = 2.5f;

		// Token: 0x0400088D RID: 2189
		private const float _minTweenDuration = 2.5f;

		// Token: 0x0400088E RID: 2190
		private const float _tweenStartValue = 0f;

		// Token: 0x0400088F RID: 2191
		private const string _shaderParamThreshold = "shader_parameter/threshold";

		// Token: 0x04000890 RID: 2192
		private List<NCreature> _creatureNodes;

		// Token: 0x04000891 RID: 2193
		private List<Control> _hitboxes;

		// Token: 0x04000892 RID: 2194
		private CancellationToken _cancelToken;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010C9 RID: 4297
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010CA RID: 4298
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010CB RID: 4299
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
