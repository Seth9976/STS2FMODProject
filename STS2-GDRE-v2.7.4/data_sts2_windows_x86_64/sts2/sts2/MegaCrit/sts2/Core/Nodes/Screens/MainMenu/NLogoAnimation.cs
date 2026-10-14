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
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x02000306 RID: 774
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NLogoAnimation.cs")]
	public class NLogoAnimation : Control, IScreenContext
	{
		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06002A77 RID: 10871 RVA: 0x0012896B File Offset: 0x00126B6B
		[Nullable(1)]
		public static string[] AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new string[] { "res://scenes/screens/main_menu/logo_animation.tscn" };
			}
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x0012897B File Offset: 0x00126B7B
		[NullableContext(1)]
		public static NLogoAnimation Create()
		{
			return PreloadManager.Cache.GetScene("res://scenes/screens/main_menu/logo_animation.tscn").Instantiate<NLogoAnimation>(0L);
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x00128994 File Offset: 0x00126B94
		public override void _Ready()
		{
			this._bg = base.GetNode<Control>("%Bg");
			this._logoContainer = base.GetNode<Control>("%Container");
			this._logoSpineNode = base.GetNode<Node2D>("Container/SpineSprite");
			this._spineSprite = new MegaSprite(this._logoSpineNode);
			this._logoSpineNode.Visible = false;
			MegaSkeleton skeleton = this._spineSprite.GetSkeleton();
			if (skeleton != null)
			{
				this._skeletonReady = true;
				Rect2 bounds = skeleton.GetBounds();
				float num = Math.Min(base.Size.X * 0.33f / bounds.Size.X, base.Size.Y * 0.33f / bounds.Size.Y);
				this._logoSpineNode.Scale = num * Vector2.One;
				this._logoSpineNode.Position = -bounds.Size * this._logoSpineNode.Scale * 0.5f;
			}
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x00128AB0 File Offset: 0x00126CB0
		[NullableContext(1)]
		public async Task PlayAnimation(CancellationToken token)
		{
			if (token.IsCancellationRequested || !this._skeletonReady)
			{
				this._cancelled = true;
			}
			else
			{
				this._tween = base.CreateTween();
				this._tween.TweenInterval(1.0);
				TaskAwaiter<bool> taskAwaiter = this._tween.AwaitFinished(this).GetAwaiter();
				TaskAwaiter<bool> taskAwaiter2;
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					if (token.IsCancellationRequested)
					{
						this._cancelled = true;
					}
					else
					{
						this._logoSpineNode.Visible = true;
						this._spineSprite.GetAnimationState().SetAnimation("animation", false, 0);
						NDebugAudioManager.Instance.Play("SOTE_Logo_Echoing_ShortTail.mp3", 1f, PitchVariance.None);
						this._tween.Kill();
						this._tween = base.CreateTween().SetParallel(true);
						this._tween.TweenProperty(this._logoSpineNode, "position:y", this._logoSpineNode.Position.Y, 0.5).From(this._logoSpineNode.Position.Y - 800f).SetEase(1L)
							.SetTrans(10L);
						this._tween.TweenProperty(this._logoContainer, "modulate", Colors.White, 0.5);
						await this.AwaitProcessFrame(default(CancellationToken));
						for (;;)
						{
							bool flag;
							using (MegaTrackEntry current = this._spineSprite.GetAnimationState().GetCurrent(0))
							{
								flag = current != null && !current.IsComplete();
							}
							if (!flag)
							{
								goto IL_03D1;
							}
							if (token.IsCancellationRequested)
							{
								break;
							}
							await this.AwaitProcessFrame(default(CancellationToken));
						}
						this._cancelled = true;
						this._tween.Kill();
						this._tween = base.CreateTween().SetParallel(true);
						this._tween.TweenProperty(this._logoContainer, "modulate", StsColors.transparentWhite, 0.25).SetEase(1L).SetTrans(5L);
						TaskAwaiter<bool> taskAwaiter3 = this._tween.AwaitFinished(this).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							await taskAwaiter3;
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter3.GetResult())
						{
							return;
						}
						IL_03D1:
						if (!this._cancelled)
						{
							this._tween.Kill();
							this._tween = base.CreateTween();
							this._tween.TweenProperty(this._bg, "modulate", this._logoBgColor, 2.0).SetEase(1L).SetTrans(7L);
							this._tween.Chain();
							this._tween.TweenInterval(1.0);
							bool flag2 = await this._tween.AwaitFinished(this);
						}
					}
				}
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002A7B RID: 10875 RVA: 0x00128AFB File Offset: 0x00126CFB
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A7C RID: 10876 RVA: 0x00128B00 File Offset: 0x00126D00
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NLogoAnimation.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NLogoAnimation.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A7D RID: 10877 RVA: 0x00128B80 File Offset: 0x00126D80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLogoAnimation.MethodName.Create && args.Count == 0)
			{
				NLogoAnimation nlogoAnimation = NLogoAnimation.Create();
				ret = VariantUtils.CreateFrom<NLogoAnimation>(ref nlogoAnimation);
				return true;
			}
			if ((ref method) == NLogoAnimation.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x00128BE8 File Offset: 0x00126DE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLogoAnimation.MethodName.Create && args.Count == 0)
			{
				NLogoAnimation nlogoAnimation = NLogoAnimation.Create();
				ret = VariantUtils.CreateFrom<NLogoAnimation>(ref nlogoAnimation);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002A7F RID: 10879 RVA: 0x00128C28 File Offset: 0x00126E28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLogoAnimation.MethodName.Create || (ref method) == NLogoAnimation.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002A80 RID: 10880 RVA: 0x00128C50 File Offset: 0x00126E50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLogoAnimation.PropertyName._bg)
			{
				this._bg = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._logoContainer)
			{
				this._logoContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._logoSpineNode)
			{
				this._logoSpineNode = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._logoBgColor)
			{
				this._logoBgColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._cancelled)
			{
				this._cancelled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._skeletonReady)
			{
				this._skeletonReady = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002A81 RID: 10881 RVA: 0x00128D24 File Offset: 0x00126F24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLogoAnimation.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._bg)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bg);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._logoContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._logoContainer);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._logoSpineNode)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._logoSpineNode);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._logoBgColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._logoBgColor);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._cancelled)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._cancelled);
				return true;
			}
			if ((ref name) == NLogoAnimation.PropertyName._skeletonReady)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._skeletonReady);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A82 RID: 10882 RVA: 0x00128E3C File Offset: 0x0012703C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NLogoAnimation.PropertyName._bg, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLogoAnimation.PropertyName._logoContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLogoAnimation.PropertyName._logoSpineNode, 0L, "", 4096L, false),
				new PropertyInfo(20L, NLogoAnimation.PropertyName._logoBgColor, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLogoAnimation.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NLogoAnimation.PropertyName._cancelled, 0L, "", 4096L, false),
				new PropertyInfo(1L, NLogoAnimation.PropertyName._skeletonReady, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLogoAnimation.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A83 RID: 10883 RVA: 0x00128F58 File Offset: 0x00127158
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLogoAnimation.PropertyName._bg, Variant.From<Control>(ref this._bg));
			info.AddProperty(NLogoAnimation.PropertyName._logoContainer, Variant.From<Control>(ref this._logoContainer));
			info.AddProperty(NLogoAnimation.PropertyName._logoSpineNode, Variant.From<Node2D>(ref this._logoSpineNode));
			info.AddProperty(NLogoAnimation.PropertyName._logoBgColor, Variant.From<Color>(ref this._logoBgColor));
			info.AddProperty(NLogoAnimation.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NLogoAnimation.PropertyName._cancelled, Variant.From<bool>(ref this._cancelled));
			info.AddProperty(NLogoAnimation.PropertyName._skeletonReady, Variant.From<bool>(ref this._skeletonReady));
		}

		/// <inheritdoc />
		// Token: 0x06002A84 RID: 10884 RVA: 0x00129008 File Offset: 0x00127208
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLogoAnimation.PropertyName._bg, ref variant))
			{
				this._bg = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLogoAnimation.PropertyName._logoContainer, ref variant2))
			{
				this._logoContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NLogoAnimation.PropertyName._logoSpineNode, ref variant3))
			{
				this._logoSpineNode = variant3.As<Node2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NLogoAnimation.PropertyName._logoBgColor, ref variant4))
			{
				this._logoBgColor = variant4.As<Color>();
			}
			Variant variant5;
			if (info.TryGetProperty(NLogoAnimation.PropertyName._tween, ref variant5))
			{
				this._tween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NLogoAnimation.PropertyName._cancelled, ref variant6))
			{
				this._cancelled = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NLogoAnimation.PropertyName._skeletonReady, ref variant7))
			{
				this._skeletonReady = variant7.As<bool>();
			}
		}

		// Token: 0x04000FD9 RID: 4057
		[Nullable(1)]
		private const string _scenePath = "res://scenes/screens/main_menu/logo_animation.tscn";

		// Token: 0x04000FDA RID: 4058
		[Nullable(1)]
		private Control _bg;

		// Token: 0x04000FDB RID: 4059
		[Nullable(1)]
		private Control _logoContainer;

		// Token: 0x04000FDC RID: 4060
		[Nullable(1)]
		private Node2D _logoSpineNode;

		// Token: 0x04000FDD RID: 4061
		[Nullable(1)]
		private MegaSprite _spineSprite;

		// Token: 0x04000FDE RID: 4062
		private Color _logoBgColor = new Color("074254FF");

		// Token: 0x04000FDF RID: 4063
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000FE0 RID: 4064
		private bool _cancelled;

		// Token: 0x04000FE1 RID: 4065
		private bool _skeletonReady;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014FF RID: 5375
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004FF2 RID: 20466
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004FF3 RID: 20467
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001500 RID: 5376
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004FF4 RID: 20468
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_bg' field.
			/// </summary>
			// Token: 0x04004FF5 RID: 20469
			public static readonly StringName _bg = "_bg";

			/// <summary>
			/// Cached name for the '_logoContainer' field.
			/// </summary>
			// Token: 0x04004FF6 RID: 20470
			public static readonly StringName _logoContainer = "_logoContainer";

			/// <summary>
			/// Cached name for the '_logoSpineNode' field.
			/// </summary>
			// Token: 0x04004FF7 RID: 20471
			public static readonly StringName _logoSpineNode = "_logoSpineNode";

			/// <summary>
			/// Cached name for the '_logoBgColor' field.
			/// </summary>
			// Token: 0x04004FF8 RID: 20472
			public static readonly StringName _logoBgColor = "_logoBgColor";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004FF9 RID: 20473
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_cancelled' field.
			/// </summary>
			// Token: 0x04004FFA RID: 20474
			public static readonly StringName _cancelled = "_cancelled";

			/// <summary>
			/// Cached name for the '_skeletonReady' field.
			/// </summary>
			// Token: 0x04004FFB RID: 20475
			public static readonly StringName _skeletonReady = "_skeletonReady";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001501 RID: 5377
		public class SignalName : Control.SignalName
		{
		}
	}
}
