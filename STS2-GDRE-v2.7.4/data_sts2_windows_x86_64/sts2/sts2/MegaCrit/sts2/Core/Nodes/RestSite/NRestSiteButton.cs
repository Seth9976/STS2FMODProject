using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.RestSite
{
	/// <summary>
	/// Corresponding button node for the RestSiteOption entity.
	/// </summary>
	// Token: 0x02000374 RID: 884
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/RestSite/NRestSiteButton.cs")]
	public class NRestSiteButton : NButton
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06003415 RID: 13333 RVA: 0x00183DB0 File Offset: 0x00181FB0
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NRestSiteButton._scenePath);
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06003416 RID: 13334 RVA: 0x00183DBC File Offset: 0x00181FBC
		// (set) Token: 0x06003417 RID: 13335 RVA: 0x00183DD3 File Offset: 0x00181FD3
		public RestSiteOption Option
		{
			get
			{
				RestSiteOption option = this._option;
				if (option == null)
				{
					throw new InvalidOperationException("Option accessed before being set");
				}
				return option;
			}
			set
			{
				this._option = value;
				this.Reload();
			}
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x00183DE4 File Offset: 0x00181FE4
		public override void _Ready()
		{
			this.ConnectSignals();
			base.Modulate = StsColors.transparentBlack;
			this._visuals = base.GetNode<Control>("%Visuals");
			this._icon = base.GetNode<TextureRect>("%Icon");
			this._outline = base.GetNode<Control>("%Outline");
			this._label = base.GetNode<MegaLabel>("%Label");
			this._hsv = (ShaderMaterial)this._icon.Material;
			this._labelPosition = this._label.Position;
			TaskHelper.RunSafely(this.AnimateIn());
			this.Reload();
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x00183E94 File Offset: 0x00182094
		private async Task AnimateIn()
		{
			Tween tween = base.CreateTween();
			tween.TweenProperty(this, "modulate", Colors.White, 0.5).SetEase(1L).SetTrans(7L);
			await tween.AwaitFinished(this._cts.Token);
			if (this.IsValid())
			{
				base.MouseFilter = 0L;
			}
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x00183ED7 File Offset: 0x001820D7
		public override void _ExitTree()
		{
			base._ExitTree();
			this._cts.Cancel();
			Tween currentTween = this._currentTween;
			if (currentTween == null)
			{
				return;
			}
			currentTween.Kill();
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x00183EFC File Offset: 0x001820FC
		public static NRestSiteButton Create(RestSiteOption option)
		{
			NRestSiteButton nrestSiteButton = PreloadManager.Cache.GetScene(NRestSiteButton._scenePath).Instantiate<NRestSiteButton>(0L);
			nrestSiteButton.Option = option;
			nrestSiteButton._isUnclickable = !option.IsEnabled;
			return nrestSiteButton;
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x00183F38 File Offset: 0x00182138
		private void Reload()
		{
			if (!base.IsNodeReady())
			{
				return;
			}
			if (this._option == null)
			{
				return;
			}
			this._icon.Texture = this.Option.Icon;
			this._label.SetTextAutoSize(this.Option.Title.GetFormattedText());
			if (!this._option.IsEnabled)
			{
				this._hsv.SetShaderParameter(NRestSiteButton._s, 0f);
				this._hsv.SetShaderParameter(NRestSiteButton._v, 0.6f);
				return;
			}
			this._hsv.SetShaderParameter(NRestSiteButton._s, 1f);
			this._hsv.SetShaderParameter(NRestSiteButton._v, 1f);
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x00184004 File Offset: 0x00182204
		protected override void OnRelease()
		{
			if (this._isUnclickable)
			{
				return;
			}
			base.OnRelease();
			TaskHelper.RunSafely(this.SelectOption(this.Option));
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x00184028 File Offset: 0x00182228
		private async Task SelectOption(RestSiteOption option)
		{
			int num = NRestSiteRoom.Instance.Options.IndexOf(option);
			if (num < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(80, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Rest site option ");
				defaultInterpolatedStringHandler.AppendFormatted<RestSiteOption>(option);
				defaultInterpolatedStringHandler.AppendLiteral(" was selected, but it was not in the list of rest site options!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._executingOption = true;
			NRestSiteRoom.Instance.DisableOptions();
			this.RefreshTextState();
			bool success = false;
			object obj = null;
			try
			{
				bool flag = await RunManager.Instance.RestSiteSynchronizer.ChooseLocalOption(num);
				success = flag;
				if (success)
				{
					NRestSiteRoom.Instance.AfterSelectingOption(option);
				}
			}
			catch (object obj)
			{
			}
			this._executingOption = false;
			if (this.IsValid())
			{
				this.RefreshTextState();
			}
			if (!success && this.IsValid())
			{
				await this.AwaitProcessFrame(default(CancellationToken));
				NRestSiteRoom.Instance.EnableOptions();
			}
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			obj = null;
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x00184074 File Offset: 0x00182274
		protected override void OnPress()
		{
			if (this._isUnclickable)
			{
				return;
			}
			base.OnPress();
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween().SetParallel(true);
			this._currentTween.TweenProperty(this._visuals, "scale", Vector2.One * 0.9f, 1.0).SetEase(1L).SetTrans(5L);
			this._currentTween.TweenProperty(this._label, "position", this._labelPosition, 1.0).SetEase(1L).SetTrans(5L);
			this._currentTween.TweenProperty(this._outline, "scale", Vector2.One * 0.9f, 1.0).SetEase(1L).SetTrans(5L);
			this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), 1.2f, 1f, 1.0).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x001841C8 File Offset: 0x001823C8
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween().SetParallel(true);
			this._currentTween.TweenProperty(this._visuals, "scale", Vector2.One * 1.1f, 0.05);
			this._currentTween.TweenProperty(this._outline, "scale", Vector2.One, 0.05);
			this._currentTween.TweenProperty(this._label, "position", this._labelPosition + new Vector2(0f, 6f), 0.05);
			if (!this._isUnclickable)
			{
				this._hsv.SetShaderParameter(NRestSiteButton._v, 1.2f);
			}
			this.RefreshTextState();
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x001842D4 File Offset: 0x001824D4
		protected override void OnUnfocus()
		{
			Tween currentTween = this._currentTween;
			if (currentTween != null)
			{
				currentTween.Kill();
			}
			this._currentTween = base.CreateTween().SetParallel(true);
			this._currentTween.TweenProperty(this._visuals, "scale", Vector2.One, 1.0).SetEase(1L).SetTrans(5L);
			this._currentTween.TweenProperty(this._label, "position", this._labelPosition, 1.0).SetEase(1L).SetTrans(5L);
			this._currentTween.TweenProperty(this._outline, "scale", Vector2.One * 0.9f, 1.0).SetEase(1L).SetTrans(5L);
			if (!this._isUnclickable)
			{
				this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), 1.2f, 1f, 1.0).SetEase(1L).SetTrans(5L);
			}
			this.RefreshTextState();
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x0018441C File Offset: 0x0018261C
		public void RefreshTextState()
		{
			NRestSiteRoom instance = NRestSiteRoom.Instance;
			if (instance == null)
			{
				return;
			}
			if (base.IsFocused || this._executingOption)
			{
				instance.SetText(this.Option.Description.GetFormattedText());
				return;
			}
			instance.FadeOutOptionDescription();
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x00184460 File Offset: 0x00182660
		private void UpdateShaderParam(float value)
		{
			this._hsv.SetShaderParameter(NRestSiteButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003424 RID: 13348 RVA: 0x00184478 File Offset: 0x00182678
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NRestSiteButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteButton.MethodName.Reload, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteButton.MethodName.RefreshTextState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteButton.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003425 RID: 13349 RVA: 0x0018465C File Offset: 0x0018285C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRestSiteButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteButton.MethodName.Reload && args.Count == 0)
			{
				this.Reload();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteButton.MethodName.RefreshTextState && args.Count == 0)
			{
				this.RefreshTextState();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteButton.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003426 RID: 13350 RVA: 0x001847D0 File Offset: 0x001829D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRestSiteButton.MethodName._Ready || (ref method) == NRestSiteButton.MethodName._ExitTree || (ref method) == NRestSiteButton.MethodName.Reload || (ref method) == NRestSiteButton.MethodName.OnRelease || (ref method) == NRestSiteButton.MethodName.OnPress || (ref method) == NRestSiteButton.MethodName.OnFocus || (ref method) == NRestSiteButton.MethodName.OnUnfocus || (ref method) == NRestSiteButton.MethodName.RefreshTextState || (ref method) == NRestSiteButton.MethodName.UpdateShaderParam || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003427 RID: 13351 RVA: 0x0018486C File Offset: 0x00182A6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRestSiteButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._visuals)
			{
				this._visuals = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._labelPosition)
			{
				this._labelPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._isUnclickable)
			{
				this._isUnclickable = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._executingOption)
			{
				this._executingOption = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._currentTween)
			{
				this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003428 RID: 13352 RVA: 0x00184974 File Offset: 0x00182B74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRestSiteButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._visuals)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._visuals);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outline);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._labelPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._labelPosition);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._isUnclickable)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isUnclickable);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._executingOption)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._executingOption);
				return true;
			}
			if ((ref name) == NRestSiteButton.PropertyName._currentTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003429 RID: 13353 RVA: 0x00184AAC File Offset: 0x00182CAC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRestSiteButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteButton.PropertyName._visuals, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteButton.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(5L, NRestSiteButton.PropertyName._labelPosition, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRestSiteButton.PropertyName._isUnclickable, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRestSiteButton.PropertyName._executingOption, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteButton.PropertyName._currentTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600342A RID: 13354 RVA: 0x00184BE8 File Offset: 0x00182DE8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRestSiteButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NRestSiteButton.PropertyName._visuals, Variant.From<Control>(ref this._visuals));
			info.AddProperty(NRestSiteButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NRestSiteButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
			info.AddProperty(NRestSiteButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NRestSiteButton.PropertyName._labelPosition, Variant.From<Vector2>(ref this._labelPosition));
			info.AddProperty(NRestSiteButton.PropertyName._isUnclickable, Variant.From<bool>(ref this._isUnclickable));
			info.AddProperty(NRestSiteButton.PropertyName._executingOption, Variant.From<bool>(ref this._executingOption));
			info.AddProperty(NRestSiteButton.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
		}

		/// <inheritdoc />
		// Token: 0x0600342B RID: 13355 RVA: 0x00184CC4 File Offset: 0x00182EC4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRestSiteButton.PropertyName._hsv, ref variant))
			{
				this._hsv = variant.As<ShaderMaterial>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRestSiteButton.PropertyName._visuals, ref variant2))
			{
				this._visuals = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRestSiteButton.PropertyName._icon, ref variant3))
			{
				this._icon = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRestSiteButton.PropertyName._outline, ref variant4))
			{
				this._outline = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRestSiteButton.PropertyName._label, ref variant5))
			{
				this._label = variant5.As<MegaLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRestSiteButton.PropertyName._labelPosition, ref variant6))
			{
				this._labelPosition = variant6.As<Vector2>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRestSiteButton.PropertyName._isUnclickable, ref variant7))
			{
				this._isUnclickable = variant7.As<bool>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRestSiteButton.PropertyName._executingOption, ref variant8))
			{
				this._executingOption = variant8.As<bool>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRestSiteButton.PropertyName._currentTween, ref variant9))
			{
				this._currentTween = variant9.As<Tween>();
			}
		}

		// Token: 0x0400142E RID: 5166
		private static readonly StringName _v = new StringName("v");

		// Token: 0x0400142F RID: 5167
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04001430 RID: 5168
		private ShaderMaterial _hsv;

		// Token: 0x04001431 RID: 5169
		private Control _visuals;

		// Token: 0x04001432 RID: 5170
		private TextureRect _icon;

		// Token: 0x04001433 RID: 5171
		private Control _outline;

		// Token: 0x04001434 RID: 5172
		private MegaLabel _label;

		// Token: 0x04001435 RID: 5173
		private Vector2 _labelPosition;

		/// <summary>
		/// Special bool for this button as disabling this button means we can still hover/focus unlike
		/// other buttons which are enabled/disabled which behave differently.
		/// </summary>
		// Token: 0x04001436 RID: 5174
		private bool _isUnclickable;

		// Token: 0x04001437 RID: 5175
		private bool _executingOption;

		// Token: 0x04001438 RID: 5176
		private const double _unfocusAnimDur = 1.0;

		// Token: 0x04001439 RID: 5177
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x0400143A RID: 5178
		[Nullable(2)]
		private Tween _currentTween;

		// Token: 0x0400143B RID: 5179
		[Nullable(2)]
		private RestSiteOption _option;

		// Token: 0x0400143C RID: 5180
		private static readonly string _scenePath = SceneHelper.GetScenePath("rest_site/rest_site_button");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016B8 RID: 5816
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005882 RID: 22658
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005883 RID: 22659
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Reload' method.
			/// </summary>
			// Token: 0x04005884 RID: 22660
			public static readonly StringName Reload = "Reload";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005885 RID: 22661
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005886 RID: 22662
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005887 RID: 22663
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005888 RID: 22664
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'RefreshTextState' method.
			/// </summary>
			// Token: 0x04005889 RID: 22665
			public static readonly StringName RefreshTextState = "RefreshTextState";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x0400588A RID: 22666
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016B9 RID: 5817
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x0400588B RID: 22667
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_visuals' field.
			/// </summary>
			// Token: 0x0400588C RID: 22668
			public static readonly StringName _visuals = "_visuals";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x0400588D RID: 22669
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x0400588E RID: 22670
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x0400588F RID: 22671
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_labelPosition' field.
			/// </summary>
			// Token: 0x04005890 RID: 22672
			public static readonly StringName _labelPosition = "_labelPosition";

			/// <summary>
			/// Cached name for the '_isUnclickable' field.
			/// </summary>
			// Token: 0x04005891 RID: 22673
			public static readonly StringName _isUnclickable = "_isUnclickable";

			/// <summary>
			/// Cached name for the '_executingOption' field.
			/// </summary>
			// Token: 0x04005892 RID: 22674
			public static readonly StringName _executingOption = "_executingOption";

			/// <summary>
			/// Cached name for the '_currentTween' field.
			/// </summary>
			// Token: 0x04005893 RID: 22675
			public static readonly StringName _currentTween = "_currentTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016BA RID: 5818
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
