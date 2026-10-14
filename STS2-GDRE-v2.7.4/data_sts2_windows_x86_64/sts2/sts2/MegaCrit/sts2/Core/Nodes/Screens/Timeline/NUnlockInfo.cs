using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	/// <summary>
	/// The screen which opens when you click on an Epoch to view the bigger image + flavor text.
	/// Note that there are two UX for this screen. One for first time open (plays the animation of
	/// the text crawling in) and the second time open, which skips this animation.
	/// </summary>
	// Token: 0x02000268 RID: 616
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NUnlockInfo.cs")]
	public class NUnlockInfo : Control
	{
		// Token: 0x06002069 RID: 8297 RVA: 0x000DBBB0 File Offset: 0x000D9DB0
		public override void _Ready()
		{
			this._icon = base.GetNode<TextureRect>("%Icon");
			this._label = base.GetNode<MegaRichTextLabel>("%Text");
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x000DBBE0 File Offset: 0x000D9DE0
		public void HideImmediately()
		{
			Color modulate = base.Modulate;
			modulate.A = 0f;
			base.Modulate = modulate;
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x000DBC08 File Offset: 0x000D9E08
		[NullableContext(1)]
		public void AnimIn(string text)
		{
			Color modulate = base.Modulate;
			modulate.A = 0f;
			base.Modulate = modulate;
			this._label.Text = text;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 0.8f, 1.0);
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x000DBC84 File Offset: 0x000D9E84
		[NullableContext(1)]
		public async Task AnimInViaPaginator(string text)
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.1);
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
				this._label.Text = text;
				Tween tween2 = this._tween;
				if (tween2 != null)
				{
					tween2.Kill();
				}
				this._tween = base.CreateTween();
				this._tween.TweenProperty(this, "modulate:a", 0.8f, 1.0);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600206D RID: 8301 RVA: 0x000DBCD0 File Offset: 0x000D9ED0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NUnlockInfo.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockInfo.MethodName.HideImmediately, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUnlockInfo.MethodName.AnimIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600206E RID: 8302 RVA: 0x000DBD98 File Offset: 0x000D9F98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUnlockInfo.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockInfo.MethodName.HideImmediately && args.Count == 0)
			{
				this.HideImmediately();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUnlockInfo.MethodName.AnimIn && args.Count == 1)
			{
				this.AnimIn(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600206F RID: 8303 RVA: 0x000DBE2B File Offset: 0x000DA02B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUnlockInfo.MethodName._Ready || (ref method) == NUnlockInfo.MethodName.HideImmediately || (ref method) == NUnlockInfo.MethodName.AnimIn || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002070 RID: 8304 RVA: 0x000DBE64 File Offset: 0x000DA064
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUnlockInfo.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NUnlockInfo.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NUnlockInfo.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002071 RID: 8305 RVA: 0x000DBECC File Offset: 0x000DA0CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUnlockInfo.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NUnlockInfo.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NUnlockInfo.PropertyName._tween)
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
		// Token: 0x06002072 RID: 8306 RVA: 0x000DBF44 File Offset: 0x000DA144
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUnlockInfo.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockInfo.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NUnlockInfo.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002073 RID: 8307 RVA: 0x000DBFBC File Offset: 0x000DA1BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUnlockInfo.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NUnlockInfo.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NUnlockInfo.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06002074 RID: 8308 RVA: 0x000DC014 File Offset: 0x000DA214
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUnlockInfo.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NUnlockInfo.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NUnlockInfo.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
		}

		// Token: 0x04000CA6 RID: 3238
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x04000CA7 RID: 3239
		[Nullable(1)]
		private MegaRichTextLabel _label;

		// Token: 0x04000CA8 RID: 3240
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012D5 RID: 4821
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400496E RID: 18798
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'HideImmediately' method.
			/// </summary>
			// Token: 0x0400496F RID: 18799
			public static readonly StringName HideImmediately = "HideImmediately";

			/// <summary>
			/// Cached name for the 'AnimIn' method.
			/// </summary>
			// Token: 0x04004970 RID: 18800
			public static readonly StringName AnimIn = "AnimIn";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012D6 RID: 4822
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004971 RID: 18801
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004972 RID: 18802
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004973 RID: 18803
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012D7 RID: 4823
		public class SignalName : Control.SignalName
		{
		}
	}
}
