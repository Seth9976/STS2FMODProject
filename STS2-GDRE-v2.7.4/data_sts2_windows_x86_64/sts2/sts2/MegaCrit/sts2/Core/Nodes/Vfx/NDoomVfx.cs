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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Manages the VFX for Doom (and its sub-emitters).
	/// Moves the creatures visuals into itself to handle sucking it into Hell.
	/// </summary>
	// Token: 0x020001A1 RID: 417
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NDoomVfx.cs")]
	public class NDoomVfx : Node2D
	{
		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x0007D62E File Offset: 0x0007B82E
		private CancellationTokenSource VfxCancellationToken { get; } = new CancellationTokenSource();

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x0007D636 File Offset: 0x0007B836
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_doom");
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x0007D642 File Offset: 0x0007B842
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NDoomVfx.ScenePath);
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x0007D64E File Offset: 0x0007B84E
		// (set) Token: 0x06001499 RID: 5273 RVA: 0x0007D656 File Offset: 0x0007B856
		[Nullable(2)]
		public Task VfxTask
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0007D660 File Offset: 0x0007B860
		[return: Nullable(2)]
		public static NDoomVfx Create(NCreatureVisuals creatureVisuals, Vector2 position, Vector2 size, bool shouldDie)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NDoomVfx ndoomVfx = PreloadManager.Cache.GetScene(NDoomVfx.ScenePath).Instantiate<NDoomVfx>(0L);
			ndoomVfx._creatureVisuals = creatureVisuals;
			ndoomVfx._position = position;
			ndoomVfx._size = size;
			ndoomVfx._shouldDie = shouldDie;
			return ndoomVfx;
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0007D6AA File Offset: 0x0007B8AA
		public override void _ExitTree()
		{
			base._ExitTree();
			this.VfxCancellationToken.Cancel();
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0007D6D0 File Offset: 0x0007B8D0
		public override void _Ready()
		{
			this._back = base.GetNode<NDoomSubEmitterVfx>("DoomVfxBack");
			this._front = base.GetNode<NDoomSubEmitterVfx>("DoomVfxFront");
			this._cancelToken = this.VfxCancellationToken.Token;
			this.VfxTask = TaskHelper.RunSafely(this.PlayVfx(this._creatureVisuals, this._position, this._size, this._shouldDie));
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x0007D744 File Offset: 0x0007B944
		private async Task PlayVfx(NCreatureVisuals creatureVisuals, Vector2 position, Vector2 size, bool shouldDie)
		{
			if (!this._cancelToken.IsCancellationRequested)
			{
				SfxCmd.Play("event:/sfx/characters/necrobinder/necrobinder_doom_kill", 1f);
				base.GlobalPosition = position + new Vector2(size.X * 0.5f, size.Y) * NCombatRoom.Instance.SceneContainer.Scale;
				base.Scale = NCombatRoom.Instance.SceneContainer.Scale;
				SubViewport node = base.GetNode<SubViewport>("Viewport");
				Vector2 vector = size;
				vector.X *= 1.5f;
				vector.Y *= 1.5f;
				node.Size = (Vector2I)vector;
				if (shouldDie)
				{
					Node2D creatureBody = creatureVisuals.GetCurrentBody();
					Vector2 creatureOffset = new Vector2(vector.X / 2f, (float)node.Size.Y) + creatureBody.Position;
					Vector2 originalGlobalScale = creatureBody.GlobalScale;
					await this.Reparent(creatureBody, node);
					creatureBody.Position = creatureOffset;
					creatureBody.Scale = originalGlobalScale;
					creatureBody = null;
				}
				if (!this._cancelToken.IsCancellationRequested)
				{
					await this.PlayVfxInternal();
				}
			}
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0007D7A8 File Offset: 0x0007B9A8
		private async Task PlayVfxInternal()
		{
			try
			{
				SubViewport node = base.GetNode<SubViewport>("Viewport");
				Sprite2D node2 = base.GetNode<Sprite2D>("%Visual");
				node2.Position += Vector2.Up * (float)node.Size.Y * 0.5f;
				NGame instance = NGame.Instance;
				if (instance != null)
				{
					instance.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
				}
				this.ShowOrHideParticles((float)node.Size.X / 260f, 0.5f);
				this._tween = base.CreateTween();
				this._tween.TweenProperty(node2, "position:y", node2.Position.Y + (float)node.Size.Y, 0.75).SetEase(0L).SetDelay(0.75)
					.SetTrans(5L);
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
					this.ShowOrHideParticles(0f, 0.25f);
					await Task.Delay(2000, this._cancelToken);
				}
			}
			finally
			{
				if (GodotObject.IsInstanceValid(this))
				{
					this.QueueFreeSafely();
				}
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0007D7EB File Offset: 0x0007B9EB
		private void ShowOrHideParticles(float widthScale, float tweenTime)
		{
			this._back.ShowOrHide(widthScale, tweenTime);
			this._front.ShowOrHide(widthScale, tweenTime);
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0007D808 File Offset: 0x0007BA08
		private Task Reparent(Node creatureNode, Node newParent)
		{
			NDoomVfx.<Reparent>d__26 <Reparent>d__;
			<Reparent>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Reparent>d__.<>4__this = this;
			<Reparent>d__.creatureNode = creatureNode;
			<Reparent>d__.newParent = newParent;
			<Reparent>d__.<>1__state = -1;
			<Reparent>d__.<>t__builder.Start<NDoomVfx.<Reparent>d__26>(ref <Reparent>d__);
			return <Reparent>d__.<>t__builder.Task;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014A1 RID: 5281 RVA: 0x0007D85C File Offset: 0x0007BA5C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NDoomVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "creatureVisuals", 0L, "", 6L, new StringName("Node2D"), false),
					new PropertyInfo(5L, "position", 0L, "", 6L, false),
					new PropertyInfo(5L, "size", 0L, "", 6L, false),
					new PropertyInfo(1L, "shouldDie", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDoomVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDoomVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDoomVfx.MethodName.ShowOrHideParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "widthScale", 0L, "", 6L, false),
					new PropertyInfo(3L, "tweenTime", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014A2 RID: 5282 RVA: 0x0007DA14 File Offset: 0x0007BC14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDoomVfx.MethodName.Create && args.Count == 4)
			{
				NDoomVfx ndoomVfx = NDoomVfx.Create(VariantUtils.ConvertTo<NCreatureVisuals>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]), VariantUtils.ConvertTo<Vector2>(args[2]), VariantUtils.ConvertTo<bool>(args[3]));
				ret = VariantUtils.CreateFrom<NDoomVfx>(ref ndoomVfx);
				return true;
			}
			if ((ref method) == NDoomVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDoomVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDoomVfx.MethodName.ShowOrHideParticles && args.Count == 2)
			{
				this.ShowOrHideParticles(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x0007DB14 File Offset: 0x0007BD14
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDoomVfx.MethodName.Create && args.Count == 4)
			{
				NDoomVfx ndoomVfx = NDoomVfx.Create(VariantUtils.ConvertTo<NCreatureVisuals>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]), VariantUtils.ConvertTo<Vector2>(args[2]), VariantUtils.ConvertTo<bool>(args[3]));
				ret = VariantUtils.CreateFrom<NDoomVfx>(ref ndoomVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060014A4 RID: 5284 RVA: 0x0007DB8C File Offset: 0x0007BD8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDoomVfx.MethodName.Create || (ref method) == NDoomVfx.MethodName._ExitTree || (ref method) == NDoomVfx.MethodName._Ready || (ref method) == NDoomVfx.MethodName.ShowOrHideParticles || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060014A5 RID: 5285 RVA: 0x0007DBDC File Offset: 0x0007BDDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDoomVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._back)
			{
				this._back = VariantUtils.ConvertTo<NDoomSubEmitterVfx>(ref value);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._front)
			{
				this._front = VariantUtils.ConvertTo<NDoomSubEmitterVfx>(ref value);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._creatureVisuals)
			{
				this._creatureVisuals = VariantUtils.ConvertTo<NCreatureVisuals>(ref value);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._position)
			{
				this._position = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._size)
			{
				this._size = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._shouldDie)
			{
				this._shouldDie = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060014A6 RID: 5286 RVA: 0x0007DCB0 File Offset: 0x0007BEB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDoomVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._back)
			{
				value = VariantUtils.CreateFrom<NDoomSubEmitterVfx>(ref this._back);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._front)
			{
				value = VariantUtils.CreateFrom<NDoomSubEmitterVfx>(ref this._front);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._creatureVisuals)
			{
				value = VariantUtils.CreateFrom<NCreatureVisuals>(ref this._creatureVisuals);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._position)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._position);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._size)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._size);
				return true;
			}
			if ((ref name) == NDoomVfx.PropertyName._shouldDie)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._shouldDie);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014A7 RID: 5287 RVA: 0x0007DDA8 File Offset: 0x0007BFA8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDoomVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDoomVfx.PropertyName._back, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDoomVfx.PropertyName._front, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDoomVfx.PropertyName._creatureVisuals, 0L, "", 4096L, false),
				new PropertyInfo(5L, NDoomVfx.PropertyName._position, 0L, "", 4096L, false),
				new PropertyInfo(5L, NDoomVfx.PropertyName._size, 0L, "", 4096L, false),
				new PropertyInfo(1L, NDoomVfx.PropertyName._shouldDie, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014A8 RID: 5288 RVA: 0x0007DEA0 File Offset: 0x0007C0A0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDoomVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NDoomVfx.PropertyName._back, Variant.From<NDoomSubEmitterVfx>(ref this._back));
			info.AddProperty(NDoomVfx.PropertyName._front, Variant.From<NDoomSubEmitterVfx>(ref this._front));
			info.AddProperty(NDoomVfx.PropertyName._creatureVisuals, Variant.From<NCreatureVisuals>(ref this._creatureVisuals));
			info.AddProperty(NDoomVfx.PropertyName._position, Variant.From<Vector2>(ref this._position));
			info.AddProperty(NDoomVfx.PropertyName._size, Variant.From<Vector2>(ref this._size));
			info.AddProperty(NDoomVfx.PropertyName._shouldDie, Variant.From<bool>(ref this._shouldDie));
		}

		/// <inheritdoc />
		// Token: 0x060014A9 RID: 5289 RVA: 0x0007DF50 File Offset: 0x0007C150
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDoomVfx.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDoomVfx.PropertyName._back, ref variant2))
			{
				this._back = variant2.As<NDoomSubEmitterVfx>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDoomVfx.PropertyName._front, ref variant3))
			{
				this._front = variant3.As<NDoomSubEmitterVfx>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDoomVfx.PropertyName._creatureVisuals, ref variant4))
			{
				this._creatureVisuals = variant4.As<NCreatureVisuals>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDoomVfx.PropertyName._position, ref variant5))
			{
				this._position = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDoomVfx.PropertyName._size, ref variant6))
			{
				this._size = variant6.As<Vector2>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDoomVfx.PropertyName._shouldDie, ref variant7))
			{
				this._shouldDie = variant7.As<bool>();
			}
		}

		// Token: 0x04000762 RID: 1890
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000763 RID: 1891
		private NDoomSubEmitterVfx _back;

		// Token: 0x04000764 RID: 1892
		private NDoomSubEmitterVfx _front;

		// Token: 0x04000765 RID: 1893
		private NCreatureVisuals _creatureVisuals;

		// Token: 0x04000766 RID: 1894
		private Vector2 _position;

		// Token: 0x04000767 RID: 1895
		private Vector2 _size;

		// Token: 0x04000768 RID: 1896
		private bool _shouldDie;

		// Token: 0x04000769 RID: 1897
		private CancellationToken _cancelToken;

		// Token: 0x0400076A RID: 1898
		private const float _doomVfxSize = 260f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200100B RID: 4107
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003FAF RID: 16303
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003FB0 RID: 16304
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003FB1 RID: 16305
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ShowOrHideParticles' method.
			/// </summary>
			// Token: 0x04003FB2 RID: 16306
			public static readonly StringName ShowOrHideParticles = "ShowOrHideParticles";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200100C RID: 4108
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003FB3 RID: 16307
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_back' field.
			/// </summary>
			// Token: 0x04003FB4 RID: 16308
			public static readonly StringName _back = "_back";

			/// <summary>
			/// Cached name for the '_front' field.
			/// </summary>
			// Token: 0x04003FB5 RID: 16309
			public static readonly StringName _front = "_front";

			/// <summary>
			/// Cached name for the '_creatureVisuals' field.
			/// </summary>
			// Token: 0x04003FB6 RID: 16310
			public static readonly StringName _creatureVisuals = "_creatureVisuals";

			/// <summary>
			/// Cached name for the '_position' field.
			/// </summary>
			// Token: 0x04003FB7 RID: 16311
			public static readonly StringName _position = "_position";

			/// <summary>
			/// Cached name for the '_size' field.
			/// </summary>
			// Token: 0x04003FB8 RID: 16312
			public static readonly StringName _size = "_size";

			/// <summary>
			/// Cached name for the '_shouldDie' field.
			/// </summary>
			// Token: 0x04003FB9 RID: 16313
			public static readonly StringName _shouldDie = "_shouldDie";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200100D RID: 4109
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
