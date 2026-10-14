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
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes
{
	/// <summary>
	/// Manages the fancy screen to screen transitions that use the SDF shader.
	/// FadeOut = Fades out the current screen. This means the screen becomes black.
	/// FadeIn = Fades the screen in. This means the screen becomes revealed.
	/// </summary>
	// Token: 0x0200017C RID: 380
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/NTransition.cs")]
	public class NTransition : ColorRect
	{
		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x0006BCC0 File Offset: 0x00069EC0
		// (set) Token: 0x0600124A RID: 4682 RVA: 0x0006BCC8 File Offset: 0x00069EC8
		public bool InTransition { get; private set; }

		// Token: 0x0600124B RID: 4683 RVA: 0x0006BCD4 File Offset: 0x00069ED4
		public override void _Ready()
		{
			this._gradientTransition = base.GetNode<Control>("GradientTransition");
			this._simpleTransition = base.GetNode<Control>("SimpleTransition");
			this._initialGradientYPosition = this._gradientTransition.Position.Y;
			this._targetGradientYPosition = 0f;
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x0006BD2E File Offset: 0x00069F2E
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "res://materials/transitions/fight_transition_mat.tres", "res://materials/transitions/fade_transition_mat.tres" });
			}
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0006BD4C File Offset: 0x00069F4C
		public async Task FadeOut(float time = 0.8f, string transitionPath = "res://materials/transitions/fade_transition_mat.tres", CancellationToken? cancelToken = null)
		{
			if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
			{
				this.InTransition = true;
				base.Visible = false;
			}
			else
			{
				this.InTransition = true;
				CanvasItem simpleTransition = this._simpleTransition;
				Color color = this._simpleTransition.Modulate;
				color.A = 0f;
				simpleTransition.Modulate = color;
				CanvasItem gradientTransition = this._gradientTransition;
				color = this._gradientTransition.Modulate;
				color.A = 0f;
				gradientTransition.Modulate = color;
				Tween tween = this._tween;
				if (tween != null)
				{
					tween.Kill();
				}
				this._tween = base.CreateTween().SetParallel(true);
				this._tween.TweenProperty(this._simpleTransition, "modulate:a", 1f, (double)time).SetEase(0L).SetTrans(4L);
				ShaderMaterial shaderMaterial = base.Material as ShaderMaterial;
				if (shaderMaterial == null)
				{
					string text = "NTransition.Material is null or not a ShaderMaterial (actual: ";
					Material material = base.Material;
					Log.Warn(text + (((material != null) ? material.GetType().Name : null) ?? "null") + "). Skipping transition.", 2);
				}
				else if (shaderMaterial.GetShaderParameter(NTransition._threshold).AsSingle() <= 0.999999f)
				{
					base.Material = PreloadManager.Cache.GetMaterial(transitionPath);
					Material material2 = base.Material;
					ShaderMaterial transitionMaterial = material2 as ShaderMaterial;
					if (transitionMaterial == null)
					{
						Log.Warn("NTransition.Material failed to load from cache (path: " + transitionPath + "). Skipping transition.", 2);
					}
					else
					{
						transitionMaterial.SetShaderParameter(NTransition._threshold, 0f);
						this._tween.TweenProperty(transitionMaterial, NTransition._thresholdTweenPath, 1f, (double)time);
						double t = 0.0;
						while (t < (double)time)
						{
							if (cancelToken != null && cancelToken.GetValueOrDefault().IsCancellationRequested)
							{
								Tween tween2 = this._tween;
								if (tween2 == null)
								{
									break;
								}
								tween2.FastForwardToCompletion();
								break;
							}
							else
							{
								t += base.GetProcessDeltaTime();
								await this.AwaitProcessFrame(default(CancellationToken));
							}
						}
						base.MouseFilter = 0L;
						transitionMaterial.SetShaderParameter(NTransition._threshold, 1f);
					}
				}
			}
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0006BDA8 File Offset: 0x00069FA8
		public async Task FadeIn(float time = 0.8f, string transitionPath = "res://materials/transitions/fade_transition_mat.tres", CancellationToken? cancelToken = null)
		{
			if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
			{
				base.Visible = false;
				this.InTransition = false;
				base.MouseFilter = 2L;
			}
			else
			{
				Tween tween = this._tween;
				if (tween != null)
				{
					tween.Kill();
				}
				CanvasItem simpleTransition = this._simpleTransition;
				Color color = this._simpleTransition.Modulate;
				color.A = 0f;
				simpleTransition.Modulate = color;
				CanvasItem gradientTransition = this._gradientTransition;
				color = this._gradientTransition.Modulate;
				color.A = 0f;
				gradientTransition.Modulate = color;
				ShaderMaterial shaderMaterial = base.Material as ShaderMaterial;
				if (shaderMaterial == null)
				{
					string text = "NTransition.Material is null or not a ShaderMaterial (actual: ";
					Material material = base.Material;
					Log.Warn(text + (((material != null) ? material.GetType().Name : null) ?? "null") + "). Skipping transition.", 2);
					this.InTransition = false;
				}
				else if (shaderMaterial.GetShaderParameter(NTransition._threshold).AsSingle() < 1E-06f)
				{
					this.InTransition = false;
				}
				else
				{
					base.Material = PreloadManager.Cache.GetMaterial(transitionPath);
					Material material2 = base.Material;
					ShaderMaterial transitionMaterial = material2 as ShaderMaterial;
					if (transitionMaterial == null)
					{
						Log.Warn("NTransition.Material failed to load from cache (path: " + transitionPath + "). Skipping transition.", 2);
						this.InTransition = false;
					}
					else
					{
						transitionMaterial.SetShaderParameter(NTransition._threshold, 1f);
						this._tween = base.CreateTween();
						this._tween.TweenProperty(transitionMaterial, NTransition._thresholdTweenPath, 0f, (double)time).SetEase(0L).SetTrans(7L);
						base.MouseFilter = 2L;
						double t = 0.0;
						while (t < (double)time)
						{
							if (cancelToken != null && cancelToken.GetValueOrDefault().IsCancellationRequested)
							{
								Tween tween2 = this._tween;
								if (tween2 == null)
								{
									break;
								}
								tween2.FastForwardToCompletion();
								break;
							}
							else
							{
								t += base.GetProcessDeltaTime();
								await this.AwaitProcessFrame(default(CancellationToken));
								if (t / (double)time > 0.75)
								{
									this.InTransition = false;
								}
							}
						}
						this.InTransition = false;
						transitionMaterial.SetShaderParameter(NTransition._threshold, 0f);
						base.MouseFilter = 2L;
					}
				}
			}
		}

		/// <summary>
		/// The gradient sweep transition when players move from room to room.
		/// This replaced the spiral room transition animation.
		/// Note that this utilizes TWO images to accomplish this transition, a simple backdrop fade + gradient sweep.
		/// </summary>
		// Token: 0x0600124F RID: 4687 RVA: 0x0006BE04 File Offset: 0x0006A004
		public async Task RoomFadeOut()
		{
			this.InTransition = true;
			if (!TestMode.IsOn && SaveManager.Instance.PrefsSave.FastMode != FastModeType.Instant)
			{
				CanvasItem simpleTransition = this._simpleTransition;
				Color color = this._simpleTransition.Modulate;
				color.A = 0f;
				simpleTransition.Modulate = color;
				CanvasItem gradientTransition = this._gradientTransition;
				color = this._gradientTransition.Modulate;
				color.A = 1f;
				gradientTransition.Modulate = color;
				Control gradientTransition2 = this._gradientTransition;
				Vector2 position = this._gradientTransition.Position;
				position.Y = this._initialGradientYPosition;
				gradientTransition2.Position = position;
				Tween tween = this._tween;
				if (tween != null)
				{
					tween.Kill();
				}
				this._tween = base.CreateTween().SetParallel(true);
				if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Normal)
				{
					this._tween.TweenProperty(this._gradientTransition, "position:y", this._targetGradientYPosition, 0.6).SetDelay(0.5);
					this._tween.TweenProperty(this._simpleTransition, "modulate:a", 1f, 0.6).SetDelay(0.5);
				}
				else if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast)
				{
					this._tween.TweenProperty(this._simpleTransition, "modulate:a", 1f, 0.3).SetEase(1L).SetTrans(4L)
						.SetDelay(0.3);
				}
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
					base.MouseFilter = 0L;
				}
			}
		}

		/// <summary>
		/// Uses the SimpleTransition ColorRect to fade from black -&gt; seeing the next room.
		/// </summary>
		// Token: 0x06001250 RID: 4688 RVA: 0x0006BE48 File Offset: 0x0006A048
		public async Task RoomFadeIn(bool showTransition = true)
		{
			if (!TestMode.IsOn)
			{
				if (!showTransition || SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
				{
					CanvasItem simpleTransition = this._simpleTransition;
					Color color = this._simpleTransition.Modulate;
					color.A = 0f;
					simpleTransition.Modulate = color;
				}
				ShaderMaterial shaderMaterial = base.Material as ShaderMaterial;
				if (shaderMaterial == null)
				{
					string text = "NTransition.Material is null or not a ShaderMaterial (actual: ";
					Material material = base.Material;
					Log.Warn(text + (((material != null) ? material.GetType().Name : null) ?? "null") + "). Skipping transition.", 2);
					this.InTransition = false;
				}
				else
				{
					shaderMaterial.SetShaderParameter(NTransition._threshold, 0f);
					CanvasItem gradientTransition = this._gradientTransition;
					Color color = this._gradientTransition.Modulate;
					color.A = 0f;
					gradientTransition.Modulate = color;
					CanvasItem simpleTransition2 = this._simpleTransition;
					color = this._simpleTransition.Modulate;
					color.A = 1f;
					simpleTransition2.Modulate = color;
					Tween tween = this._tween;
					if (tween != null)
					{
						tween.Kill();
					}
					this._tween = base.CreateTween().SetParallel(true);
					if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast)
					{
						this._tween.TweenProperty(this._simpleTransition, "modulate:a", 0f, 0.3);
						base.MouseFilter = 2L;
					}
					else
					{
						this._tween.TweenProperty(this._simpleTransition, "modulate:a", 0f, 0.8);
						this._tween.TweenCallback(Callable.From(delegate
						{
							base.MouseFilter = 2L;
							this.InTransition = false;
						})).SetDelay(0.2);
					}
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
						this.InTransition = false;
					}
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001251 RID: 4689 RVA: 0x0006BE94 File Offset: 0x0006A094
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NTransition.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001252 RID: 4690 RVA: 0x0006BED8 File Offset: 0x0006A0D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTransition.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001253 RID: 4691 RVA: 0x0006BF08 File Offset: 0x0006A108
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTransition.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001254 RID: 4692 RVA: 0x0006BF20 File Offset: 0x0006A120
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTransition.PropertyName.InTransition)
			{
				this.InTransition = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NTransition.PropertyName._initialGradientYPosition)
			{
				this._initialGradientYPosition = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NTransition.PropertyName._targetGradientYPosition)
			{
				this._targetGradientYPosition = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NTransition.PropertyName._gradientTransition)
			{
				this._gradientTransition = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTransition.PropertyName._simpleTransition)
			{
				this._simpleTransition = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTransition.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001255 RID: 4693 RVA: 0x0006BFD8 File Offset: 0x0006A1D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTransition.PropertyName.InTransition)
			{
				bool inTransition = this.InTransition;
				value = VariantUtils.CreateFrom<bool>(ref inTransition);
				return true;
			}
			if ((ref name) == NTransition.PropertyName._initialGradientYPosition)
			{
				value = VariantUtils.CreateFrom<float>(ref this._initialGradientYPosition);
				return true;
			}
			if ((ref name) == NTransition.PropertyName._targetGradientYPosition)
			{
				value = VariantUtils.CreateFrom<float>(ref this._targetGradientYPosition);
				return true;
			}
			if ((ref name) == NTransition.PropertyName._gradientTransition)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._gradientTransition);
				return true;
			}
			if ((ref name) == NTransition.PropertyName._simpleTransition)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._simpleTransition);
				return true;
			}
			if ((ref name) == NTransition.PropertyName._tween)
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
		// Token: 0x06001256 RID: 4694 RVA: 0x0006C0B0 File Offset: 0x0006A2B0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NTransition.PropertyName.InTransition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NTransition.PropertyName._initialGradientYPosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NTransition.PropertyName._targetGradientYPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTransition.PropertyName._gradientTransition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTransition.PropertyName._simpleTransition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTransition.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001257 RID: 4695 RVA: 0x0006C188 File Offset: 0x0006A388
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName inTransition = NTransition.PropertyName.InTransition;
			bool inTransition2 = this.InTransition;
			info.AddProperty(inTransition, Variant.From<bool>(ref inTransition2));
			info.AddProperty(NTransition.PropertyName._initialGradientYPosition, Variant.From<float>(ref this._initialGradientYPosition));
			info.AddProperty(NTransition.PropertyName._targetGradientYPosition, Variant.From<float>(ref this._targetGradientYPosition));
			info.AddProperty(NTransition.PropertyName._gradientTransition, Variant.From<Control>(ref this._gradientTransition));
			info.AddProperty(NTransition.PropertyName._simpleTransition, Variant.From<Control>(ref this._simpleTransition));
			info.AddProperty(NTransition.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001258 RID: 4696 RVA: 0x0006C224 File Offset: 0x0006A424
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTransition.PropertyName.InTransition, ref variant))
			{
				this.InTransition = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTransition.PropertyName._initialGradientYPosition, ref variant2))
			{
				this._initialGradientYPosition = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTransition.PropertyName._targetGradientYPosition, ref variant3))
			{
				this._targetGradientYPosition = variant3.As<float>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTransition.PropertyName._gradientTransition, ref variant4))
			{
				this._gradientTransition = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTransition.PropertyName._simpleTransition, ref variant5))
			{
				this._simpleTransition = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NTransition.PropertyName._tween, ref variant6))
			{
				this._tween = variant6.As<Tween>();
			}
		}

		// Token: 0x0400066F RID: 1647
		private static readonly NodePath _thresholdTweenPath = new NodePath("shader_parameter/threshold");

		// Token: 0x04000670 RID: 1648
		private static readonly StringName _threshold = new StringName("threshold");

		// Token: 0x04000671 RID: 1649
		private const string _fightTransitionPath = "res://materials/transitions/fight_transition_mat.tres";

		// Token: 0x04000672 RID: 1650
		private const string _fadeTransitionPath = "res://materials/transitions/fade_transition_mat.tres";

		// Token: 0x04000674 RID: 1652
		private float _initialGradientYPosition;

		// Token: 0x04000675 RID: 1653
		private float _targetGradientYPosition;

		// Token: 0x04000676 RID: 1654
		private Control _gradientTransition;

		// Token: 0x04000677 RID: 1655
		private Control _simpleTransition;

		// Token: 0x04000678 RID: 1656
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F81 RID: 3969
		[NullableContext(0)]
		public class MethodName : ColorRect.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003DD9 RID: 15833
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F82 RID: 3970
		[NullableContext(0)]
		public class PropertyName : ColorRect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InTransition' property.
			/// </summary>
			// Token: 0x04003DDA RID: 15834
			public static readonly StringName InTransition = "InTransition";

			/// <summary>
			/// Cached name for the '_initialGradientYPosition' field.
			/// </summary>
			// Token: 0x04003DDB RID: 15835
			public static readonly StringName _initialGradientYPosition = "_initialGradientYPosition";

			/// <summary>
			/// Cached name for the '_targetGradientYPosition' field.
			/// </summary>
			// Token: 0x04003DDC RID: 15836
			public static readonly StringName _targetGradientYPosition = "_targetGradientYPosition";

			/// <summary>
			/// Cached name for the '_gradientTransition' field.
			/// </summary>
			// Token: 0x04003DDD RID: 15837
			public static readonly StringName _gradientTransition = "_gradientTransition";

			/// <summary>
			/// Cached name for the '_simpleTransition' field.
			/// </summary>
			// Token: 0x04003DDE RID: 15838
			public static readonly StringName _simpleTransition = "_simpleTransition";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003DDF RID: 15839
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F83 RID: 3971
		[NullableContext(0)]
		public class SignalName : ColorRect.SignalName
		{
		}
	}
}
