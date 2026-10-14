using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens
{
	/// <summary>
	/// Unlock screen which appears after you place the first Epoch and you unlock the three Epochs:
	/// Neow, Silent, and Regent Epochs. Only used in one place lol.
	/// </summary>
	// Token: 0x0200026C RID: 620
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockMiscScreen.cs")]
	public class NUnlockMiscScreen : NUnlockScreen
	{
		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060020A6 RID: 8358 RVA: 0x000DD83C File Offset: 0x000DBA3C
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NUnlockMiscScreen._scenePath);
			}
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x000DD848 File Offset: 0x000DBA48
		[NullableContext(1)]
		public static NUnlockMiscScreen Create()
		{
			return PreloadManager.Cache.GetScene(NUnlockMiscScreen._scenePath).Instantiate<NUnlockMiscScreen>(0L);
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000DD860 File Offset: 0x000DBA60
		public override void _Ready()
		{
			base.ConnectSignals();
			this._label = base.GetNode<MegaRichTextLabel>("%Label");
			this._label.Text = this._textToSet;
			this._label.Modulate = StsColors.transparentBlack;
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000DD8A0 File Offset: 0x000DBAA0
		public override void Open()
		{
			base.Open();
			SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock", 1f);
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._label, "modulate", Colors.White, 1.0).SetDelay(0.25);
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x000DD911 File Offset: 0x000DBB11
		[NullableContext(1)]
		public void SetUnlocks(string text)
		{
			this._textToSet = text;
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x000DD91A File Offset: 0x000DBB1A
		protected override void OnScreenPreClose()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x000DD92C File Offset: 0x000DBB2C
		protected override void OnScreenClose()
		{
			NTimelineScreen.Instance.EnableInput();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060020AD RID: 8365 RVA: 0x000DD938 File Offset: 0x000DBB38
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NUnlockMiscScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NUnlockMiscScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockMiscScreen.MethodName.Open, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockMiscScreen.MethodName.SetUnlocks, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NUnlockMiscScreen.MethodName.OnScreenPreClose, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockMiscScreen.MethodName.OnScreenClose, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060020AE RID: 8366 RVA: 0x000DDA98 File Offset: 0x000DBC98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockMiscScreen.MethodName.Create && args.Count == 0)
			{
				NUnlockMiscScreen nunlockMiscScreen = NUnlockMiscScreen.Create();
				ret = VariantUtils.CreateFrom<NUnlockMiscScreen>(ref nunlockMiscScreen);
				return true;
			}
			if ((ref method) == NUnlockMiscScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockMiscScreen.MethodName.Open && args.Count == 0)
			{
				this.Open();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockMiscScreen.MethodName.SetUnlocks && args.Count == 1)
			{
				this.SetUnlocks(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockMiscScreen.MethodName.OnScreenPreClose && args.Count == 0)
			{
				this.OnScreenPreClose();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockMiscScreen.MethodName.OnScreenClose && args.Count == 0)
			{
				this.OnScreenClose();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000DDBA0 File Offset: 0x000DBDA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockMiscScreen.MethodName.Create && args.Count == 0)
			{
				NUnlockMiscScreen nunlockMiscScreen = NUnlockMiscScreen.Create();
				ret = VariantUtils.CreateFrom<NUnlockMiscScreen>(ref nunlockMiscScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060020B0 RID: 8368 RVA: 0x000DDBE0 File Offset: 0x000DBDE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUnlockMiscScreen.MethodName.Create || (ref method) == NUnlockMiscScreen.MethodName._Ready || (ref method) == NUnlockMiscScreen.MethodName.Open || (ref method) == NUnlockMiscScreen.MethodName.SetUnlocks || (ref method) == NUnlockMiscScreen.MethodName.OnScreenPreClose || (ref method) == NUnlockMiscScreen.MethodName.OnScreenClose || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060020B1 RID: 8369 RVA: 0x000DDC50 File Offset: 0x000DBE50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUnlockMiscScreen.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NUnlockMiscScreen.PropertyName._textToSet)
			{
				this._textToSet = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NUnlockMiscScreen.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060020B2 RID: 8370 RVA: 0x000DDCB8 File Offset: 0x000DBEB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUnlockMiscScreen.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NUnlockMiscScreen.PropertyName._textToSet)
			{
				value = VariantUtils.CreateFrom<string>(ref this._textToSet);
				return true;
			}
			if ((ref name) == NUnlockMiscScreen.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060020B3 RID: 8371 RVA: 0x000DDD30 File Offset: 0x000DBF30
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUnlockMiscScreen.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(4L, NUnlockMiscScreen.PropertyName._textToSet, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockMiscScreen.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060020B4 RID: 8372 RVA: 0x000DDDA8 File Offset: 0x000DBFA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUnlockMiscScreen.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NUnlockMiscScreen.PropertyName._textToSet, Variant.From<string>(ref this._textToSet));
			info.AddProperty(NUnlockMiscScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x060020B5 RID: 8373 RVA: 0x000DDE00 File Offset: 0x000DC000
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUnlockMiscScreen.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NUnlockMiscScreen.PropertyName._textToSet, ref variant2))
			{
				this._textToSet = variant2.As<string>();
			}
			Variant variant3;
			if (info.TryGetProperty(NUnlockMiscScreen.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
		}

		// Token: 0x04000CBD RID: 3261
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_misc_screen");

		// Token: 0x04000CBE RID: 3262
		[Nullable(1)]
		private MegaRichTextLabel _label;

		// Token: 0x04000CBF RID: 3263
		[Nullable(1)]
		private string _textToSet;

		// Token: 0x04000CC0 RID: 3264
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012E2 RID: 4834
		public new class MethodName : NUnlockScreen.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004991 RID: 18833
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004992 RID: 18834
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Open' method.
			/// </summary>
			// Token: 0x04004993 RID: 18835
			public new static readonly StringName Open = "Open";

			/// <summary>
			/// Cached name for the 'SetUnlocks' method.
			/// </summary>
			// Token: 0x04004994 RID: 18836
			public static readonly StringName SetUnlocks = "SetUnlocks";

			/// <summary>
			/// Cached name for the 'OnScreenPreClose' method.
			/// </summary>
			// Token: 0x04004995 RID: 18837
			public new static readonly StringName OnScreenPreClose = "OnScreenPreClose";

			/// <summary>
			/// Cached name for the 'OnScreenClose' method.
			/// </summary>
			// Token: 0x04004996 RID: 18838
			public new static readonly StringName OnScreenClose = "OnScreenClose";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012E3 RID: 4835
		public new class PropertyName : NUnlockScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004997 RID: 18839
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_textToSet' field.
			/// </summary>
			// Token: 0x04004998 RID: 18840
			public static readonly StringName _textToSet = "_textToSet";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004999 RID: 18841
			public new static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012E4 RID: 4836
		public new class SignalName : NUnlockScreen.SignalName
		{
		}
	}
}
