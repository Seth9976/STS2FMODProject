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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Potions
{
	// Token: 0x0200037E RID: 894
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Potions/NPotion.cs")]
	public class NPotion : Control
	{
		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x060034FB RID: 13563 RVA: 0x0018ABE0 File Offset: 0x00188DE0
		// (set) Token: 0x060034FC RID: 13564 RVA: 0x0018ABE8 File Offset: 0x00188DE8
		public TextureRect Image { get; private set; }

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x060034FD RID: 13565 RVA: 0x0018ABF1 File Offset: 0x00188DF1
		// (set) Token: 0x060034FE RID: 13566 RVA: 0x0018ABF9 File Offset: 0x00188DF9
		public TextureRect Outline { get; private set; }

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x060034FF RID: 13567 RVA: 0x0018AC02 File Offset: 0x00188E02
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("/potions/potion");
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06003500 RID: 13568 RVA: 0x0018AC0E File Offset: 0x00188E0E
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NPotion.ScenePath,
					NPotionFlashVfx.ScenePath
				});
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06003501 RID: 13569 RVA: 0x0018AC2B File Offset: 0x00188E2B
		// (set) Token: 0x06003502 RID: 13570 RVA: 0x0018AC42 File Offset: 0x00188E42
		public PotionModel Model
		{
			get
			{
				PotionModel model = this._model;
				if (model == null)
				{
					throw new InvalidOperationException("Model was accessed before it was set.");
				}
				return model;
			}
			set
			{
				this._model = value;
				this.Reload();
			}
		}

		/// <summary>
		/// Create an instance of the node.
		/// Null if we're in test mode.
		/// </summary>
		// Token: 0x06003503 RID: 13571 RVA: 0x0018AC54 File Offset: 0x00188E54
		[return: Nullable(2)]
		public static NPotion Create(PotionModel potion)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NPotion npotion = PreloadManager.Cache.GetScene(NPotion.ScenePath).Instantiate<NPotion>(0L);
			npotion.Model = potion;
			return npotion;
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x0018AC8C File Offset: 0x00188E8C
		public override void _Ready()
		{
			this.Image = base.GetNode<TextureRect>("%Image");
			this.Outline = base.GetNode<TextureRect>("%Outline");
			this._container = base.GetNode<Control>("Container");
			this.Reload();
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x0018ACE1 File Offset: 0x00188EE1
		public override void _ExitTree()
		{
			CancellationTokenSource cancellationTokenSource = this._cancellationTokenSource;
			if (cancellationTokenSource == null)
			{
				return;
			}
			cancellationTokenSource.Cancel();
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x0018ACF3 File Offset: 0x00188EF3
		private void Reload()
		{
			if (!base.IsNodeReady() || this._model == null)
			{
				return;
			}
			this.Image.Texture = this._model.Image;
			this.Outline.Texture = this._model.Outline;
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x0018AD34 File Offset: 0x00188F34
		public async Task PlayNewlyAcquiredAnimation(Vector2? startLocation)
		{
			if (this._cancellationTokenSource != null)
			{
				await this._cancellationTokenSource.CancelAsync();
			}
			CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
			this._cancellationTokenSource = cancelTokenSource;
			await this.AwaitProcessFrame(default(CancellationToken));
			if (!cancelTokenSource.IsCancellationRequested)
			{
				Tween obtainedTween = this._obtainedTween;
				if (obtainedTween != null)
				{
					obtainedTween.Kill();
				}
				if (startLocation == null)
				{
					Control container = this._container;
					Vector2 position = this._container.Position;
					position.Y = 40f;
					container.Position = position;
					CanvasItem container2 = this._container;
					Color color = this._container.Modulate;
					color.A = 0f;
					container2.Modulate = color;
					this._obtainedTween = base.GetTree().CreateTween();
					this._obtainedTween.TweenProperty(this._container, "modulate:a", 1f, 0.10000000149011612);
					this._obtainedTween.Parallel();
					this._obtainedTween.SetEase(1L);
					this._obtainedTween.SetTrans(10L);
					this._obtainedTween.TweenProperty(this._container, "position:y", 0f, 0.3499999940395355);
					this._obtainedTween.TweenCallback(Callable.From(new Action(this.DoFlash)));
				}
				else
				{
					this._container.GlobalPosition = startLocation.Value;
					CanvasItem container3 = this._container;
					Color color = this._container.Modulate;
					color.A = 1f;
					container3.Modulate = color;
					this._obtainedTween = base.GetTree().CreateTween();
					this._obtainedTween.SetEase(1L);
					this._obtainedTween.SetTrans(4L);
					this._obtainedTween.TweenProperty(this._container, "position", Vector2.Zero, 0.3499999940395355);
					this._obtainedTween.TweenCallback(Callable.From(new Action(this.DoFlash)));
				}
			}
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x0018AD7F File Offset: 0x00188F7F
		private void DoFlash()
		{
			this.AddChildSafely(NPotionFlashVfx.Create(this));
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x0018AD90 File Offset: 0x00188F90
		public void DoBounce()
		{
			Tween bounceTween = this._bounceTween;
			if (bounceTween != null)
			{
				bounceTween.Kill();
			}
			this._bounceTween = base.CreateTween();
			this._bounceTween.TweenProperty(this._container, "position:y", base.Position.Y - 12f, 0.125).SetEase(1L).SetTrans(1L);
			this._bounceTween.TweenProperty(this._container, "position:y", 0f, 0.125).SetEase(0L).SetTrans(1L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600350A RID: 13578 RVA: 0x0018AE40 File Offset: 0x00189040
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NPotion.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotion.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotion.MethodName.Reload, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotion.MethodName.DoFlash, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotion.MethodName.DoBounce, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600350B RID: 13579 RVA: 0x0018AF40 File Offset: 0x00189140
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotion.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotion.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotion.MethodName.Reload && args.Count == 0)
			{
				this.Reload();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotion.MethodName.DoFlash && args.Count == 0)
			{
				this.DoFlash();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotion.MethodName.DoBounce && args.Count == 0)
			{
				this.DoBounce();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600350C RID: 13580 RVA: 0x0018B010 File Offset: 0x00189210
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPotion.MethodName._Ready || (ref method) == NPotion.MethodName._ExitTree || (ref method) == NPotion.MethodName.Reload || (ref method) == NPotion.MethodName.DoFlash || (ref method) == NPotion.MethodName.DoBounce || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600350D RID: 13581 RVA: 0x0018B070 File Offset: 0x00189270
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPotion.PropertyName.Image)
			{
				this.Image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPotion.PropertyName.Outline)
			{
				this.Outline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPotion.PropertyName._container)
			{
				this._container = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPotion.PropertyName._bounceTween)
			{
				this._bounceTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPotion.PropertyName._obtainedTween)
			{
				this._obtainedTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600350E RID: 13582 RVA: 0x0018B10C File Offset: 0x0018930C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPotion.PropertyName.Image)
			{
				TextureRect textureRect = this.Image;
				value = VariantUtils.CreateFrom<TextureRect>(ref textureRect);
				return true;
			}
			if ((ref name) == NPotion.PropertyName.Outline)
			{
				TextureRect textureRect = this.Outline;
				value = VariantUtils.CreateFrom<TextureRect>(ref textureRect);
				return true;
			}
			if ((ref name) == NPotion.PropertyName._container)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._container);
				return true;
			}
			if ((ref name) == NPotion.PropertyName._bounceTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._bounceTween);
				return true;
			}
			if ((ref name) == NPotion.PropertyName._obtainedTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._obtainedTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600350F RID: 13583 RVA: 0x0018B1C8 File Offset: 0x001893C8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPotion.PropertyName._container, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotion.PropertyName.Image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotion.PropertyName.Outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotion.PropertyName._bounceTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotion.PropertyName._obtainedTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003510 RID: 13584 RVA: 0x0018B284 File Offset: 0x00189484
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName image = NPotion.PropertyName.Image;
			TextureRect textureRect = this.Image;
			info.AddProperty(image, Variant.From<TextureRect>(ref textureRect));
			StringName outline = NPotion.PropertyName.Outline;
			textureRect = this.Outline;
			info.AddProperty(outline, Variant.From<TextureRect>(ref textureRect));
			info.AddProperty(NPotion.PropertyName._container, Variant.From<Control>(ref this._container));
			info.AddProperty(NPotion.PropertyName._bounceTween, Variant.From<Tween>(ref this._bounceTween));
			info.AddProperty(NPotion.PropertyName._obtainedTween, Variant.From<Tween>(ref this._obtainedTween));
		}

		/// <inheritdoc />
		// Token: 0x06003511 RID: 13585 RVA: 0x0018B30C File Offset: 0x0018950C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPotion.PropertyName.Image, ref variant))
			{
				this.Image = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPotion.PropertyName.Outline, ref variant2))
			{
				this.Outline = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPotion.PropertyName._container, ref variant3))
			{
				this._container = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPotion.PropertyName._bounceTween, ref variant4))
			{
				this._bounceTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPotion.PropertyName._obtainedTween, ref variant5))
			{
				this._obtainedTween = variant5.As<Tween>();
			}
		}

		// Token: 0x04001492 RID: 5266
		private const float _newlyAcquiredPopDuration = 0.35f;

		// Token: 0x04001493 RID: 5267
		private const float _newlyAcquiredFadeInDuration = 0.1f;

		// Token: 0x04001494 RID: 5268
		private const float _newlyAcquiredPopDistance = 40f;

		// Token: 0x04001495 RID: 5269
		[Nullable(2)]
		private PotionModel _model;

		// Token: 0x04001496 RID: 5270
		private Control _container;

		// Token: 0x04001499 RID: 5273
		[Nullable(2)]
		private Tween _bounceTween;

		// Token: 0x0400149A RID: 5274
		[Nullable(2)]
		private Tween _obtainedTween;

		// Token: 0x0400149B RID: 5275
		[Nullable(2)]
		private CancellationTokenSource _cancellationTokenSource;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016E4 RID: 5860
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005932 RID: 22834
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005933 RID: 22835
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Reload' method.
			/// </summary>
			// Token: 0x04005934 RID: 22836
			public static readonly StringName Reload = "Reload";

			/// <summary>
			/// Cached name for the 'DoFlash' method.
			/// </summary>
			// Token: 0x04005935 RID: 22837
			public static readonly StringName DoFlash = "DoFlash";

			/// <summary>
			/// Cached name for the 'DoBounce' method.
			/// </summary>
			// Token: 0x04005936 RID: 22838
			public static readonly StringName DoBounce = "DoBounce";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016E5 RID: 5861
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Image' property.
			/// </summary>
			// Token: 0x04005937 RID: 22839
			public static readonly StringName Image = "Image";

			/// <summary>
			/// Cached name for the 'Outline' property.
			/// </summary>
			// Token: 0x04005938 RID: 22840
			public static readonly StringName Outline = "Outline";

			/// <summary>
			/// Cached name for the '_container' field.
			/// </summary>
			// Token: 0x04005939 RID: 22841
			public static readonly StringName _container = "_container";

			/// <summary>
			/// Cached name for the '_bounceTween' field.
			/// </summary>
			// Token: 0x0400593A RID: 22842
			public static readonly StringName _bounceTween = "_bounceTween";

			/// <summary>
			/// Cached name for the '_obtainedTween' field.
			/// </summary>
			// Token: 0x0400593B RID: 22843
			public static readonly StringName _obtainedTween = "_obtainedTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016E6 RID: 5862
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
