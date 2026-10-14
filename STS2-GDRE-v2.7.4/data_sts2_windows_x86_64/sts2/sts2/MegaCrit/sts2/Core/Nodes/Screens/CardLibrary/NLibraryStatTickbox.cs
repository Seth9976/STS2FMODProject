using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary
{
	// Token: 0x02000359 RID: 857
	[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NLibraryStatTickbox.cs")]
	public class NLibraryStatTickbox : NTickbox
	{
		// Token: 0x06003195 RID: 12693 RVA: 0x001707E0 File Offset: 0x0016E9E0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._label = base.GetNode<MegaLabel>("Label");
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x00170800 File Offset: 0x0016EA00
		protected override void OnRelease()
		{
			base.OnRelease();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One * 1.1f, 0.25).SetEase(1L).SetTrans(9L);
			this._labelTween.TweenProperty(this._label, "self_modulate", Colors.White, 0.05);
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x001708B0 File Offset: 0x0016EAB0
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One * 1.1f, 0.05);
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x00170920 File Offset: 0x0016EB20
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x00170994 File Offset: 0x0016EB94
		protected override void OnPress()
		{
			base.OnPress();
			Tween labelTween = this._labelTween;
			if (labelTween != null)
			{
				labelTween.Kill();
			}
			this._labelTween = base.CreateTween().SetParallel(true);
			this._labelTween.TweenProperty(this._label, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._labelTween.TweenProperty(this._label, "self_modulate", StsColors.lightGray, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x00170A44 File Offset: 0x0016EC44
		[NullableContext(1)]
		public void SetLabel(string text)
		{
			this._label.SetTextAutoSize(text);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600319B RID: 12699 RVA: 0x00170A54 File Offset: 0x0016EC54
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NLibraryStatTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLibraryStatTickbox.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLibraryStatTickbox.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLibraryStatTickbox.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLibraryStatTickbox.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLibraryStatTickbox.MethodName.SetLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600319C RID: 12700 RVA: 0x00170BA8 File Offset: 0x0016EDA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLibraryStatTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLibraryStatTickbox.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLibraryStatTickbox.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLibraryStatTickbox.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLibraryStatTickbox.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLibraryStatTickbox.MethodName.SetLabel && args.Count == 1)
			{
				this.SetLabel(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600319D RID: 12701 RVA: 0x00170CAC File Offset: 0x0016EEAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLibraryStatTickbox.MethodName._Ready || (ref method) == NLibraryStatTickbox.MethodName.OnRelease || (ref method) == NLibraryStatTickbox.MethodName.OnFocus || (ref method) == NLibraryStatTickbox.MethodName.OnUnfocus || (ref method) == NLibraryStatTickbox.MethodName.OnPress || (ref method) == NLibraryStatTickbox.MethodName.SetLabel || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600319E RID: 12702 RVA: 0x00170D1A File Offset: 0x0016EF1A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLibraryStatTickbox.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NLibraryStatTickbox.PropertyName._labelTween)
			{
				this._labelTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600319F RID: 12703 RVA: 0x00170D5C File Offset: 0x0016EF5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLibraryStatTickbox.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NLibraryStatTickbox.PropertyName._labelTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._labelTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060031A0 RID: 12704 RVA: 0x00170DB4 File Offset: 0x0016EFB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NLibraryStatTickbox.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLibraryStatTickbox.PropertyName._labelTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060031A1 RID: 12705 RVA: 0x00170E0A File Offset: 0x0016F00A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLibraryStatTickbox.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NLibraryStatTickbox.PropertyName._labelTween, Variant.From<Tween>(ref this._labelTween));
		}

		/// <inheritdoc />
		// Token: 0x060031A2 RID: 12706 RVA: 0x00170E40 File Offset: 0x0016F040
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLibraryStatTickbox.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLibraryStatTickbox.PropertyName._labelTween, ref variant2))
			{
				this._labelTween = variant2.As<Tween>();
			}
		}

		// Token: 0x04001353 RID: 4947
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x04001354 RID: 4948
		[Nullable(2)]
		private Tween _labelTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001649 RID: 5705
		public new class MethodName : NTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040056CC RID: 22220
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x040056CD RID: 22221
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040056CE RID: 22222
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040056CF RID: 22223
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x040056D0 RID: 22224
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'SetLabel' method.
			/// </summary>
			// Token: 0x040056D1 RID: 22225
			public static readonly StringName SetLabel = "SetLabel";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200164A RID: 5706
		public new class PropertyName : NTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x040056D2 RID: 22226
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_labelTween' field.
			/// </summary>
			// Token: 0x040056D3 RID: 22227
			public static readonly StringName _labelTween = "_labelTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200164B RID: 5707
		public new class SignalName : NTickbox.SignalName
		{
		}
	}
}
