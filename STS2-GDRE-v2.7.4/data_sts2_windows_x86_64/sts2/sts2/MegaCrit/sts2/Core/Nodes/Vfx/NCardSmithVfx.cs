using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000190 RID: 400
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NCardSmithVfx.cs")]
	public class NCardSmithVfx : Node2D
	{
		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x00075AB0 File Offset: 0x00073CB0
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_card_smith");
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x00075ABC File Offset: 0x00073CBC
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NCardSmithVfx.ScenePath,
					TmpSfx.GetPath("card_smith.mp3")
				});
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x00075ADE File Offset: 0x00073CDE
		// (set) Token: 0x06001381 RID: 4993 RVA: 0x00075AE6 File Offset: 0x00073CE6
		public float SfxVolume { get; set; } = 1f;

		// Token: 0x06001382 RID: 4994 RVA: 0x00075AF0 File Offset: 0x00073CF0
		[return: Nullable(2)]
		public static NCardSmithVfx Create(IEnumerable<CardModel> cards, bool playSfx = true)
		{
			NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create();
			if (ncardSmithVfx == null)
			{
				return null;
			}
			ncardSmithVfx._cards.AddRange(cards);
			ncardSmithVfx._willPlaySfx = playSfx;
			return ncardSmithVfx;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00075B1C File Offset: 0x00073D1C
		[return: Nullable(2)]
		public static NCardSmithVfx Create(NCard card, bool playSfx = true)
		{
			NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create();
			if (ncardSmithVfx == null)
			{
				return null;
			}
			ncardSmithVfx._willPlaySfx = playSfx;
			ncardSmithVfx._cardNode = card;
			return ncardSmithVfx;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00075B43 File Offset: 0x00073D43
		[NullableContext(2)]
		public static NCardSmithVfx Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCardSmithVfx.ScenePath).Instantiate<NCardSmithVfx>(0L);
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00075B64 File Offset: 0x00073D64
		public override void _Ready()
		{
			if (this._cardNode != null)
			{
				base.GlobalPosition = this._cardNode.GlobalPosition;
				base.GlobalScale = this._cardNode.Scale;
				TaskHelper.RunSafely(this.PlayAnimation());
				return;
			}
			if (this._cards.Count > 0)
			{
				TaskHelper.RunSafely(this.PlayAnimation(this._cards));
				return;
			}
			TaskHelper.RunSafely(this.PlayAnimation());
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00075BD5 File Offset: 0x00073DD5
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00075BE8 File Offset: 0x00073DE8
		private async Task PlayAnimation()
		{
			if (this._willPlaySfx)
			{
				NDebugAudioManager instance = NDebugAudioManager.Instance;
				if (instance != null)
				{
					instance.Play("card_smith.mp3", this.SfxVolume, PitchVariance.Small);
				}
			}
			this._tween = base.CreateTween();
			this._tween.Parallel().TweenCallback(Callable.From(delegate
			{
				this.PlaySubParticles(base.GetNode<Control>("Spark1"));
			}));
			this._tween.TweenInterval(0.25);
			this._tween.Chain().TweenCallback(Callable.From(delegate
			{
				this.PlaySubParticles(base.GetNode<Control>("Spark2"));
			}));
			this._tween.TweenInterval(0.25);
			this._tween.Chain().TweenCallback(Callable.From(delegate
			{
				this.PlaySubParticles(base.GetNode<Control>("Spark3"));
			}));
			this._tween.TweenInterval(0.4000000059604645);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00075C2C File Offset: 0x00073E2C
		private async Task PlayAnimation(IEnumerable<CardModel> cards)
		{
			Control node = base.GetNode<Control>("%CardContainer");
			List<NCard> cardNodes = new List<NCard>();
			foreach (CardModel cardModel in cards)
			{
				NCard ncard = NCard.Create(cardModel, ModelVisibility.Visible);
				node.AddChildSafely(ncard);
				ncard.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
				cardNodes.Add(ncard);
			}
			this._tween = base.CreateTween();
			foreach (NCard ncard2 in cardNodes)
			{
				this._tween.Parallel().TweenProperty(ncard2, "scale", Vector2.One * 1f, 0.25).From(Vector2.Zero)
					.SetEase(1L)
					.SetTrans(7L);
			}
			if (this._willPlaySfx)
			{
				this._tween.Chain().TweenCallback(Callable.From(delegate
				{
					NDebugAudioManager instance2 = NDebugAudioManager.Instance;
					if (instance2 == null)
					{
						return;
					}
					instance2.Play("card_smith.mp3", this.SfxVolume, PitchVariance.Small);
				}));
			}
			this._tween.Parallel().TweenCallback(Callable.From(delegate
			{
				this.PlaySubParticles(base.GetNode<Control>("Spark1"));
			}));
			this._tween.Parallel().TweenCallback(Callable.From(delegate
			{
				NGame instance3 = NGame.Instance;
				if (instance3 == null)
				{
					return;
				}
				instance3.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
			}));
			foreach (NCard ncard3 in cardNodes)
			{
				this._tween.Parallel().TweenProperty(ncard3, "rotation_degrees", 20, 0.15000000596046448).SetTrans(6L)
					.SetEase(1L);
			}
			this._tween.TweenInterval(0.10000000149011612);
			this._tween.Chain().TweenCallback(Callable.From(delegate
			{
				this.PlaySubParticles(base.GetNode<Control>("Spark2"));
			}));
			foreach (NCard ncard4 in cardNodes)
			{
				this._tween.Parallel().TweenProperty(ncard4, "rotation_degrees", -10, 0.15000000596046448).SetTrans(6L)
					.SetEase(1L);
			}
			this._tween.Parallel().TweenCallback(Callable.From(delegate
			{
				NGame instance4 = NGame.Instance;
				if (instance4 == null)
				{
					return;
				}
				instance4.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
			}));
			this._tween.TweenInterval(0.10000000149011612);
			this._tween.Chain().TweenCallback(Callable.From(delegate
			{
				this.PlaySubParticles(base.GetNode<Control>("Spark3"));
			}));
			foreach (NCard ncard5 in cardNodes)
			{
				this._tween.Parallel().TweenProperty(ncard5, "rotation_degrees", 5, 0.15000000596046448).SetTrans(6L)
					.SetEase(1L);
			}
			this._tween.Parallel().TweenCallback(Callable.From(delegate
			{
				NGame instance5 = NGame.Instance;
				if (instance5 == null)
				{
					return;
				}
				instance5.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
			}));
			this._tween.TweenInterval(0.3499999940395355);
			TaskAwaiter<bool> taskAwaiter = this._tween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				if (cardNodes[0].IsInsideTree() && this._cards[0].Pile == null)
				{
					this._tween = base.CreateTween();
					foreach (NCard ncard6 in cardNodes)
					{
						this._tween.SetParallel(true).TweenProperty(ncard6, "scale", Vector2.Zero, 0.20000000298023224);
					}
					bool flag = await this._tween.AwaitFinished(this);
				}
				else if (cardNodes[0].IsInsideTree())
				{
					for (int i = 0; i < cardNodes.Count; i++)
					{
						cardNodes[i].Model.Pile.Type.GetTargetPosition(cardNodes[i]);
						Vector2 globalPosition = cardNodes[i].GlobalPosition;
						cardNodes[i].Reparent(this, true);
						cardNodes[i].GlobalPosition = globalPosition;
						NCardFlyVfx ncardFlyVfx = NCardFlyVfx.Create(cardNodes[i], cardNodes[i].Model.Pile.Type, false, cardNodes[i].Model.Owner.Character.TrailPath);
						NRun instance = NRun.Instance;
						if (instance != null)
						{
							instance.GlobalUi.TopBar.TrailContainer.AddChildSafely(ncardFlyVfx);
						}
						if (ncardFlyVfx.SwooshAwayCompletion != null && i == cardNodes.Count - 1)
						{
							await ncardFlyVfx.SwooshAwayCompletion.Task;
						}
					}
					this.QueueFreeSafely();
				}
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00075C78 File Offset: 0x00073E78
		private void PlaySubParticles(Node node)
		{
			foreach (GpuParticles2D gpuParticles2D in node.GetChildren(false).OfType<GpuParticles2D>())
			{
				gpuParticles2D.Restart();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600138A RID: 5002 RVA: 0x00075CCC File Offset: 0x00073ECC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NCardSmithVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(1L, "playSfx", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardSmithVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, null, null),
				new MethodInfo(NCardSmithVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardSmithVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardSmithVfx.MethodName.PlaySubParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Node"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600138B RID: 5003 RVA: 0x00075E68 File Offset: 0x00074068
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardSmithVfx.MethodName.Create && args.Count == 2)
			{
				NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = VariantUtils.CreateFrom<NCardSmithVfx>(ref ncardSmithVfx);
				return true;
			}
			if ((ref method) == NCardSmithVfx.MethodName.Create && args.Count == 0)
			{
				NCardSmithVfx ncardSmithVfx2 = NCardSmithVfx.Create();
				ret = VariantUtils.CreateFrom<NCardSmithVfx>(ref ncardSmithVfx2);
				return true;
			}
			if ((ref method) == NCardSmithVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardSmithVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardSmithVfx.MethodName.PlaySubParticles && args.Count == 1)
			{
				this.PlaySubParticles(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00075F6C File Offset: 0x0007416C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardSmithVfx.MethodName.Create && args.Count == 2)
			{
				NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = VariantUtils.CreateFrom<NCardSmithVfx>(ref ncardSmithVfx);
				return true;
			}
			if ((ref method) == NCardSmithVfx.MethodName.Create && args.Count == 0)
			{
				NCardSmithVfx ncardSmithVfx2 = NCardSmithVfx.Create();
				ret = VariantUtils.CreateFrom<NCardSmithVfx>(ref ncardSmithVfx2);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600138D RID: 5005 RVA: 0x00075FF4 File Offset: 0x000741F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardSmithVfx.MethodName.Create || (ref method) == NCardSmithVfx.MethodName._Ready || (ref method) == NCardSmithVfx.MethodName._ExitTree || (ref method) == NCardSmithVfx.MethodName.PlaySubParticles || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600138E RID: 5006 RVA: 0x00076044 File Offset: 0x00074244
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardSmithVfx.PropertyName.SfxVolume)
			{
				this.SfxVolume = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardSmithVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardSmithVfx.PropertyName._willPlaySfx)
			{
				this._willPlaySfx = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardSmithVfx.PropertyName._cardNode)
			{
				this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			if ((ref name) == NCardSmithVfx.PropertyName._cardContainer)
			{
				this._cardContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600138F RID: 5007 RVA: 0x000760E0 File Offset: 0x000742E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardSmithVfx.PropertyName.SfxVolume)
			{
				float sfxVolume = this.SfxVolume;
				value = VariantUtils.CreateFrom<float>(ref sfxVolume);
				return true;
			}
			if ((ref name) == NCardSmithVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NCardSmithVfx.PropertyName._willPlaySfx)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._willPlaySfx);
				return true;
			}
			if ((ref name) == NCardSmithVfx.PropertyName._cardNode)
			{
				value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
				return true;
			}
			if ((ref name) == NCardSmithVfx.PropertyName._cardContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001390 RID: 5008 RVA: 0x00076198 File Offset: 0x00074398
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardSmithVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardSmithVfx.PropertyName._willPlaySfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardSmithVfx.PropertyName._cardNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardSmithVfx.PropertyName._cardContainer, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardSmithVfx.PropertyName.SfxVolume, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001391 RID: 5009 RVA: 0x00076250 File Offset: 0x00074450
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName sfxVolume = NCardSmithVfx.PropertyName.SfxVolume;
			float sfxVolume2 = this.SfxVolume;
			info.AddProperty(sfxVolume, Variant.From<float>(ref sfxVolume2));
			info.AddProperty(NCardSmithVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NCardSmithVfx.PropertyName._willPlaySfx, Variant.From<bool>(ref this._willPlaySfx));
			info.AddProperty(NCardSmithVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
			info.AddProperty(NCardSmithVfx.PropertyName._cardContainer, Variant.From<Control>(ref this._cardContainer));
		}

		/// <inheritdoc />
		// Token: 0x06001392 RID: 5010 RVA: 0x000762D8 File Offset: 0x000744D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardSmithVfx.PropertyName.SfxVolume, ref variant))
			{
				this.SfxVolume = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardSmithVfx.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardSmithVfx.PropertyName._willPlaySfx, ref variant3))
			{
				this._willPlaySfx = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardSmithVfx.PropertyName._cardNode, ref variant4))
			{
				this._cardNode = variant4.As<NCard>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardSmithVfx.PropertyName._cardContainer, ref variant5))
			{
				this._cardContainer = variant5.As<Control>();
			}
		}

		// Token: 0x040006FE RID: 1790
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040006FF RID: 1791
		public const string smithSfx = "card_smith.mp3";

		// Token: 0x04000700 RID: 1792
		private bool _willPlaySfx = true;

		// Token: 0x04000701 RID: 1793
		private readonly List<CardModel> _cards = new List<CardModel>();

		// Token: 0x04000702 RID: 1794
		[Nullable(2)]
		private NCard _cardNode;

		// Token: 0x04000703 RID: 1795
		private Control _cardContainer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FC9 RID: 4041
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003EDE RID: 16094
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003EDF RID: 16095
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003EE0 RID: 16096
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'PlaySubParticles' method.
			/// </summary>
			// Token: 0x04003EE1 RID: 16097
			public static readonly StringName PlaySubParticles = "PlaySubParticles";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FCA RID: 4042
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'SfxVolume' property.
			/// </summary>
			// Token: 0x04003EE2 RID: 16098
			public static readonly StringName SfxVolume = "SfxVolume";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003EE3 RID: 16099
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_willPlaySfx' field.
			/// </summary>
			// Token: 0x04003EE4 RID: 16100
			public static readonly StringName _willPlaySfx = "_willPlaySfx";

			/// <summary>
			/// Cached name for the '_cardNode' field.
			/// </summary>
			// Token: 0x04003EE5 RID: 16101
			public static readonly StringName _cardNode = "_cardNode";

			/// <summary>
			/// Cached name for the '_cardContainer' field.
			/// </summary>
			// Token: 0x04003EE6 RID: 16102
			public static readonly StringName _cardContainer = "_cardContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FCB RID: 4043
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
