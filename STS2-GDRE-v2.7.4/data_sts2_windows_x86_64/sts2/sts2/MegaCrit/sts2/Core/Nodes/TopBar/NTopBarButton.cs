using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.TopBar
{
	// Token: 0x02000243 RID: 579
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarButton.cs")]
	public abstract class NTopBarButton : NButton
	{
		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001D7D RID: 7549 RVA: 0x000BFE2A File Offset: 0x000BE02A
		// (set) Token: 0x06001D7E RID: 7550 RVA: 0x000BFE32 File Offset: 0x000BE032
		private protected bool IsScreenOpen { protected get; private set; }

		// Token: 0x06001D7F RID: 7551 RVA: 0x000BFE3B File Offset: 0x000BE03B
		public override void _Ready()
		{
			this.ConnectSignals();
			this._icon = base.GetNode<Control>("Control/Icon");
			this._hsv = (ShaderMaterial)this._icon.Material;
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x000BFE6F File Offset: 0x000BE06F
		protected void InitTopBarButton()
		{
			this.ConnectSignals();
			this._icon = base.GetNode<Control>("Control/Icon");
			this._hsv = (ShaderMaterial)this._icon.Material;
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x000BFEA3 File Offset: 0x000BE0A3
		public override void _ExitTree()
		{
			base._ExitTree();
			this.CancelAnimations();
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x000BFEB1 File Offset: 0x000BE0B1
		protected override void OnRelease()
		{
			CancellationTokenSource pressDownCancelToken = this._pressDownCancelToken;
			if (pressDownCancelToken != null)
			{
				pressDownCancelToken.Cancel();
			}
			this._hoverAnimCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimHover(this._hoverAnimCancelToken));
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x000BFEE1 File Offset: 0x000BE0E1
		protected override void OnPress()
		{
			base.OnPress();
			CancellationTokenSource hoverAnimCancelToken = this._hoverAnimCancelToken;
			if (hoverAnimCancelToken != null)
			{
				hoverAnimCancelToken.Cancel();
			}
			this._pressDownCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimPressDown(this._pressDownCancelToken));
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x000BFF18 File Offset: 0x000BE118
		[NullableContext(1)]
		protected virtual async Task AnimPressDown(CancellationTokenSource cancelToken)
		{
			if (this._icon.IsValid())
			{
				float num = 0f;
				float startAngle = this._icon.Rotation;
				float targetAngle = startAngle + 0.41887903f;
				while (num < 0.25f)
				{
					this._icon.Rotation = Mathf.LerpAngle(startAngle, targetAngle, Ease.CubicOut(num / 0.25f));
					ShaderMaterial hsv = this._hsv;
					if (hsv != null)
					{
						hsv.SetShaderParameter(NTopBarButton._v, Mathf.Lerp(1.1f, 0.4f, Ease.CubicOut(num / 0.25f)));
					}
					float num2 = num;
					float num3 = await this.AwaitProcessFrame(cancelToken.Token);
					num = num2 + num3;
				}
				this._icon.Rotation = targetAngle;
				ShaderMaterial hsv2 = this._hsv;
				if (hsv2 != null)
				{
					hsv2.SetShaderParameter(NTopBarButton._v, 0.4f);
				}
			}
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000BFF64 File Offset: 0x000BE164
		protected override void OnFocus()
		{
			base.OnFocus();
			if (this.IsScreenOpen)
			{
				ShaderMaterial hsv = this._hsv;
				if (hsv != null)
				{
					hsv.SetShaderParameter(NTopBarButton._v, 1.1f);
				}
				this._icon.Scale = NTopBarButton._hoverScale;
				return;
			}
			ShaderMaterial hsv2 = this._hsv;
			if (hsv2 != null)
			{
				hsv2.SetShaderParameter(NTopBarButton._v, 1.1f);
			}
			this._icon.Scale = NTopBarButton._hoverScale;
			CancellationTokenSource unhoverAnimCancelToken = this._unhoverAnimCancelToken;
			if (unhoverAnimCancelToken != null)
			{
				unhoverAnimCancelToken.Cancel();
			}
			CancellationTokenSource hoverAnimCancelToken = this._hoverAnimCancelToken;
			if (hoverAnimCancelToken != null)
			{
				hoverAnimCancelToken.Cancel();
			}
			this._hoverAnimCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimHover(this._hoverAnimCancelToken));
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x000C001F File Offset: 0x000BE21F
		protected override void OnEnable()
		{
			base.OnEnable();
			base.Modulate = Colors.White;
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000C0032 File Offset: 0x000BE232
		protected override void OnDisable()
		{
			base.OnDisable();
			base.Modulate = StsColors.disabledTopBarButton;
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000C0048 File Offset: 0x000BE248
		[NullableContext(1)]
		protected virtual async Task AnimHover(CancellationTokenSource cancelToken)
		{
			if (this._icon.IsValid())
			{
				float num = 0f;
				float startAngle = this._icon.Rotation;
				while (num < 0.5f)
				{
					this._icon.Rotation = Mathf.LerpAngle(startAngle, -0.20943952f, Ease.BackOut(num / 0.5f, 1f));
					float num2 = num;
					float num3 = await this.AwaitProcessFrame(cancelToken.Token);
					num = num2 + num3;
				}
				this._icon.Rotation = -0.20943952f;
			}
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x000C0094 File Offset: 0x000BE294
		protected override void OnUnfocus()
		{
			if (this.IsScreenOpen)
			{
				CancellationTokenSource pressDownCancelToken = this._pressDownCancelToken;
				if (pressDownCancelToken != null)
				{
					pressDownCancelToken.Cancel();
				}
				ShaderMaterial hsv = this._hsv;
				if (hsv != null)
				{
					hsv.SetShaderParameter(NTopBarButton._v, 1f);
				}
				this._icon.Scale = Vector2.One;
				return;
			}
			CancellationTokenSource hoverAnimCancelToken = this._hoverAnimCancelToken;
			if (hoverAnimCancelToken != null)
			{
				hoverAnimCancelToken.Cancel();
			}
			CancellationTokenSource pressDownCancelToken2 = this._pressDownCancelToken;
			if (pressDownCancelToken2 != null)
			{
				pressDownCancelToken2.Cancel();
			}
			CancellationTokenSource unhoverAnimCancelToken = this._unhoverAnimCancelToken;
			if (unhoverAnimCancelToken != null)
			{
				unhoverAnimCancelToken.Cancel();
			}
			this._unhoverAnimCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimUnhover(this._unhoverAnimCancelToken));
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000C013C File Offset: 0x000BE33C
		[NullableContext(1)]
		protected virtual async Task AnimUnhover(CancellationTokenSource cancelToken)
		{
			if (this._icon.IsValid())
			{
				float num = 0f;
				float startAngle = this._icon.Rotation;
				while (num < 1f)
				{
					this._icon.Rotation = Mathf.LerpAngle(startAngle, 0f, Ease.ElasticOut(num / 1f));
					ShaderMaterial hsv = this._hsv;
					if (hsv != null)
					{
						hsv.SetShaderParameter(NTopBarButton._v, Mathf.Lerp(1.1f, 1f, Ease.ExpoOut(num / 1f)));
					}
					this._icon.Scale = NTopBarButton._hoverScale.Lerp(Vector2.One, Ease.ExpoOut(num / 1f));
					float num2 = num;
					float num3 = await this.AwaitProcessFrame(cancelToken.Token);
					num = num2 + num3;
				}
				ShaderMaterial hsv2 = this._hsv;
				if (hsv2 != null)
				{
					hsv2.SetShaderParameter(NTopBarButton._v, 1f);
				}
				this._icon.Rotation = 0f;
				this._icon.Scale = Vector2.One;
			}
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x000C0188 File Offset: 0x000BE388
		protected void UpdateScreenOpen()
		{
			bool flag = this.IsOpen();
			if (this.IsScreenOpen != flag)
			{
				this.IsScreenOpen = flag;
				if (!this.IsScreenOpen)
				{
					this.OnScreenClosed();
				}
			}
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x000C01BA File Offset: 0x000BE3BA
		private void OnScreenClosed()
		{
			this.CancelAnimations();
			this._unhoverAnimCancelToken = new CancellationTokenSource();
			TaskHelper.RunSafely(this.AnimUnhover(this._unhoverAnimCancelToken));
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x000C01DF File Offset: 0x000BE3DF
		private void CancelAnimations()
		{
			CancellationTokenSource hoverAnimCancelToken = this._hoverAnimCancelToken;
			if (hoverAnimCancelToken != null)
			{
				hoverAnimCancelToken.Cancel();
			}
			CancellationTokenSource pressDownCancelToken = this._pressDownCancelToken;
			if (pressDownCancelToken != null)
			{
				pressDownCancelToken.Cancel();
			}
			CancellationTokenSource unhoverAnimCancelToken = this._unhoverAnimCancelToken;
			if (unhoverAnimCancelToken == null)
			{
				return;
			}
			unhoverAnimCancelToken.Cancel();
		}

		// Token: 0x06001D8E RID: 7566
		protected abstract bool IsOpen();

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D8F RID: 7567 RVA: 0x000C0214 File Offset: 0x000BE414
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(13)
			{
				new MethodInfo(NTopBarButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName.InitTopBarButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName.UpdateScreenOpen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName.OnScreenClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName.CancelAnimations, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarButton.MethodName.IsOpen, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D90 RID: 7568 RVA: 0x000C0490 File Offset: 0x000BE690
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBarButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName.InitTopBarButton && args.Count == 0)
			{
				this.InitTopBarButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName.UpdateScreenOpen && args.Count == 0)
			{
				this.UpdateScreenOpen();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName.OnScreenClosed && args.Count == 0)
			{
				this.OnScreenClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName.CancelAnimations && args.Count == 0)
			{
				this.CancelAnimations();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarButton.MethodName.IsOpen && args.Count == 0)
			{
				bool flag = this.IsOpen();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001D91 RID: 7569 RVA: 0x000C0690 File Offset: 0x000BE890
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBarButton.MethodName._Ready || (ref method) == NTopBarButton.MethodName.InitTopBarButton || (ref method) == NTopBarButton.MethodName._ExitTree || (ref method) == NTopBarButton.MethodName.OnRelease || (ref method) == NTopBarButton.MethodName.OnPress || (ref method) == NTopBarButton.MethodName.OnFocus || (ref method) == NTopBarButton.MethodName.OnEnable || (ref method) == NTopBarButton.MethodName.OnDisable || (ref method) == NTopBarButton.MethodName.OnUnfocus || (ref method) == NTopBarButton.MethodName.UpdateScreenOpen || (ref method) == NTopBarButton.MethodName.OnScreenClosed || (ref method) == NTopBarButton.MethodName.CancelAnimations || (ref method) == NTopBarButton.MethodName.IsOpen || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001D92 RID: 7570 RVA: 0x000C0768 File Offset: 0x000BE968
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTopBarButton.PropertyName.IsScreenOpen)
			{
				this.IsScreenOpen = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NTopBarButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTopBarButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06001D93 RID: 7571 RVA: 0x000C07D0 File Offset: 0x000BE9D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBarButton.PropertyName.IsScreenOpen)
			{
				bool isScreenOpen = this.IsScreenOpen;
				value = VariantUtils.CreateFrom<bool>(ref isScreenOpen);
				return true;
			}
			if ((ref name) == NTopBarButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._icon);
				return true;
			}
			if ((ref name) == NTopBarButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D94 RID: 7572 RVA: 0x000C0848 File Offset: 0x000BEA48
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NTopBarButton.PropertyName.IsScreenOpen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBarButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBarButton.PropertyName._hsv, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D95 RID: 7573 RVA: 0x000C08C0 File Offset: 0x000BEAC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isScreenOpen = NTopBarButton.PropertyName.IsScreenOpen;
			bool isScreenOpen2 = this.IsScreenOpen;
			info.AddProperty(isScreenOpen, Variant.From<bool>(ref isScreenOpen2));
			info.AddProperty(NTopBarButton.PropertyName._icon, Variant.From<Control>(ref this._icon));
			info.AddProperty(NTopBarButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
		}

		/// <inheritdoc />
		// Token: 0x06001D96 RID: 7574 RVA: 0x000C091C File Offset: 0x000BEB1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTopBarButton.PropertyName.IsScreenOpen, ref variant))
			{
				this.IsScreenOpen = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTopBarButton.PropertyName._icon, ref variant2))
			{
				this._icon = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTopBarButton.PropertyName._hsv, ref variant3))
			{
				this._hsv = variant3.As<ShaderMaterial>();
			}
		}

		// Token: 0x04000B55 RID: 2901
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000B57 RID: 2903
		[Nullable(1)]
		protected Control _icon;

		// Token: 0x04000B58 RID: 2904
		[Nullable(2)]
		protected ShaderMaterial _hsv;

		// Token: 0x04000B59 RID: 2905
		private const float _hoverAngle = -0.20943952f;

		// Token: 0x04000B5A RID: 2906
		private const float _hoverShaderV = 1.1f;

		// Token: 0x04000B5B RID: 2907
		protected const float _hoverAnimDur = 0.5f;

		// Token: 0x04000B5C RID: 2908
		protected static readonly Vector2 _hoverScale = Vector2.One * 1.1f;

		// Token: 0x04000B5D RID: 2909
		[Nullable(2)]
		private CancellationTokenSource _hoverAnimCancelToken;

		// Token: 0x04000B5E RID: 2910
		private const float _defaultV = 1f;

		// Token: 0x04000B5F RID: 2911
		protected const float _unhoverAnimDur = 1f;

		// Token: 0x04000B60 RID: 2912
		[Nullable(2)]
		private CancellationTokenSource _unhoverAnimCancelToken;

		// Token: 0x04000B61 RID: 2913
		private const float _pressDownV = 0.4f;

		// Token: 0x04000B62 RID: 2914
		protected const float _pressDownDur = 0.25f;

		// Token: 0x04000B63 RID: 2915
		[Nullable(2)]
		private CancellationTokenSource _pressDownCancelToken;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200123E RID: 4670
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400468A RID: 18058
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'InitTopBarButton' method.
			/// </summary>
			// Token: 0x0400468B RID: 18059
			public static readonly StringName InitTopBarButton = "InitTopBarButton";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400468C RID: 18060
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x0400468D RID: 18061
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x0400468E RID: 18062
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400468F RID: 18063
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04004690 RID: 18064
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04004691 RID: 18065
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004692 RID: 18066
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateScreenOpen' method.
			/// </summary>
			// Token: 0x04004693 RID: 18067
			public static readonly StringName UpdateScreenOpen = "UpdateScreenOpen";

			/// <summary>
			/// Cached name for the 'OnScreenClosed' method.
			/// </summary>
			// Token: 0x04004694 RID: 18068
			public static readonly StringName OnScreenClosed = "OnScreenClosed";

			/// <summary>
			/// Cached name for the 'CancelAnimations' method.
			/// </summary>
			// Token: 0x04004695 RID: 18069
			public static readonly StringName CancelAnimations = "CancelAnimations";

			/// <summary>
			/// Cached name for the 'IsOpen' method.
			/// </summary>
			// Token: 0x04004696 RID: 18070
			public static readonly StringName IsOpen = "IsOpen";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200123F RID: 4671
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsScreenOpen' property.
			/// </summary>
			// Token: 0x04004697 RID: 18071
			public static readonly StringName IsScreenOpen = "IsScreenOpen";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004698 RID: 18072
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004699 RID: 18073
			public static readonly StringName _hsv = "_hsv";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001240 RID: 4672
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
