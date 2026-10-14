using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	// Token: 0x02000266 RID: 614
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NTimelineTutorial.cs")]
	public class NTimelineTutorial : Control, IScreenContext
	{
		// Token: 0x06002049 RID: 8265 RVA: 0x000DAC2B File Offset: 0x000D8E2B
		[NullableContext(1)]
		public void Init(NTimelineScreen screen)
		{
			this._timeline = screen;
			screen.HideBackButtonImmediately();
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000DAC3C File Offset: 0x000D8E3C
		public override void _Ready()
		{
			SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock", 1f);
			this._text = base.GetNode<MegaRichTextLabel>("%TutorialText");
			this._text.Text = "[center]" + new LocString("timeline", "TUTORIAL_TEXT").GetRawText() + "[/center]";
			this._acknowledgeButton = base.GetNode<NAcknowledgeButton>("%AcknowledgeButton");
			this._acknowledgeButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseTutorial)), 0U);
			this.AnimateTutorial();
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000DACDC File Offset: 0x000D8EDC
		[NullableContext(1)]
		private void CloseTutorial(NButton _)
		{
			this._acknowledgeButton.Disable();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.FastForwardToCompletion();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.5);
			this._tween.Chain().TweenCallback(Callable.From(delegate
			{
				TaskHelper.RunSafely(this._timeline.SpawnFirstTimeTimeline());
				this.QueueFreeSafely();
			}));
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000DAD60 File Offset: 0x000D8F60
		private void AnimateTutorial()
		{
			this._acknowledgeButton.Disable();
			this._text.VisibleRatio = 0f;
			CanvasItem text = this._text;
			Color modulate = this._text.Modulate;
			modulate.A = 0f;
			text.Modulate = modulate;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.FastForwardToCompletion();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this._text, "visible_ratio", 1f, 2.0).SetEase(1L).SetTrans(4L);
			this._tween.Parallel().TweenProperty(this._text, "modulate:a", 1f, 1.0);
			this._tween.Chain().TweenCallback(Callable.From(delegate
			{
				this._acknowledgeButton.Enable();
			}));
			this._tween.Parallel().TweenProperty(this._acknowledgeButton, "modulate:a", 1f, 0.3).SetEase(1L)
				.SetTrans(7L)
				.SetDelay(1.0);
			this._tween.Parallel().TweenProperty(this._acknowledgeButton, "position:y", 920f, 0.3).SetEase(1L)
				.SetTrans(10L)
				.SetDelay(1.0);
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600204D RID: 8269 RVA: 0x000DAEFD File Offset: 0x000D90FD
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
		// Token: 0x0600204E RID: 8270 RVA: 0x000DAF00 File Offset: 0x000D9100
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NTimelineTutorial.MethodName.Init, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "screen", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NTimelineTutorial.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTimelineTutorial.MethodName.CloseTutorial, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NTimelineTutorial.MethodName.AnimateTutorial, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600204F RID: 8271 RVA: 0x000DB034 File Offset: 0x000D9234
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTimelineTutorial.MethodName.Init && args.Count == 1)
			{
				this.Init(VariantUtils.ConvertTo<NTimelineScreen>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineTutorial.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineTutorial.MethodName.CloseTutorial && args.Count == 1)
			{
				this.CloseTutorial(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTimelineTutorial.MethodName.AnimateTutorial && args.Count == 0)
			{
				this.AnimateTutorial();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002050 RID: 8272 RVA: 0x000DB0FC File Offset: 0x000D92FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTimelineTutorial.MethodName.Init || (ref method) == NTimelineTutorial.MethodName._Ready || (ref method) == NTimelineTutorial.MethodName.CloseTutorial || (ref method) == NTimelineTutorial.MethodName.AnimateTutorial || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002051 RID: 8273 RVA: 0x000DB14C File Offset: 0x000D934C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTimelineTutorial.PropertyName._text)
			{
				this._text = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NTimelineTutorial.PropertyName._acknowledgeButton)
			{
				this._acknowledgeButton = VariantUtils.ConvertTo<NAcknowledgeButton>(ref value);
				return true;
			}
			if ((ref name) == NTimelineTutorial.PropertyName._timeline)
			{
				this._timeline = VariantUtils.ConvertTo<NTimelineScreen>(ref value);
				return true;
			}
			if ((ref name) == NTimelineTutorial.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002052 RID: 8274 RVA: 0x000DB1D0 File Offset: 0x000D93D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTimelineTutorial.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NTimelineTutorial.PropertyName._text)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._text);
				return true;
			}
			if ((ref name) == NTimelineTutorial.PropertyName._acknowledgeButton)
			{
				value = VariantUtils.CreateFrom<NAcknowledgeButton>(ref this._acknowledgeButton);
				return true;
			}
			if ((ref name) == NTimelineTutorial.PropertyName._timeline)
			{
				value = VariantUtils.CreateFrom<NTimelineScreen>(ref this._timeline);
				return true;
			}
			if ((ref name) == NTimelineTutorial.PropertyName._tween)
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
		// Token: 0x06002053 RID: 8275 RVA: 0x000DB288 File Offset: 0x000D9488
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTimelineTutorial.PropertyName._text, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineTutorial.PropertyName._acknowledgeButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineTutorial.PropertyName._timeline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineTutorial.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTimelineTutorial.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002054 RID: 8276 RVA: 0x000DB344 File Offset: 0x000D9544
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTimelineTutorial.PropertyName._text, Variant.From<MegaRichTextLabel>(ref this._text));
			info.AddProperty(NTimelineTutorial.PropertyName._acknowledgeButton, Variant.From<NAcknowledgeButton>(ref this._acknowledgeButton));
			info.AddProperty(NTimelineTutorial.PropertyName._timeline, Variant.From<NTimelineScreen>(ref this._timeline));
			info.AddProperty(NTimelineTutorial.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06002055 RID: 8277 RVA: 0x000DB3B0 File Offset: 0x000D95B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTimelineTutorial.PropertyName._text, ref variant))
			{
				this._text = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTimelineTutorial.PropertyName._acknowledgeButton, ref variant2))
			{
				this._acknowledgeButton = variant2.As<NAcknowledgeButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTimelineTutorial.PropertyName._timeline, ref variant3))
			{
				this._timeline = variant3.As<NTimelineScreen>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTimelineTutorial.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
		}

		// Token: 0x04000CA0 RID: 3232
		[Nullable(1)]
		private MegaRichTextLabel _text;

		// Token: 0x04000CA1 RID: 3233
		[Nullable(1)]
		private NAcknowledgeButton _acknowledgeButton;

		// Token: 0x04000CA2 RID: 3234
		[Nullable(1)]
		private NTimelineScreen _timeline;

		// Token: 0x04000CA3 RID: 3235
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012CF RID: 4815
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Init' method.
			/// </summary>
			// Token: 0x0400495C RID: 18780
			public static readonly StringName Init = "Init";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400495D RID: 18781
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'CloseTutorial' method.
			/// </summary>
			// Token: 0x0400495E RID: 18782
			public static readonly StringName CloseTutorial = "CloseTutorial";

			/// <summary>
			/// Cached name for the 'AnimateTutorial' method.
			/// </summary>
			// Token: 0x0400495F RID: 18783
			public static readonly StringName AnimateTutorial = "AnimateTutorial";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012D0 RID: 4816
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004960 RID: 18784
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_text' field.
			/// </summary>
			// Token: 0x04004961 RID: 18785
			public static readonly StringName _text = "_text";

			/// <summary>
			/// Cached name for the '_acknowledgeButton' field.
			/// </summary>
			// Token: 0x04004962 RID: 18786
			public static readonly StringName _acknowledgeButton = "_acknowledgeButton";

			/// <summary>
			/// Cached name for the '_timeline' field.
			/// </summary>
			// Token: 0x04004963 RID: 18787
			public static readonly StringName _timeline = "_timeline";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004964 RID: 18788
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012D1 RID: 4817
		public class SignalName : Control.SignalName
		{
		}
	}
}
