using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen
{
	// Token: 0x0200031E RID: 798
	[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NScoreLine.cs")]
	public class NScoreLine : Control
	{
		// Token: 0x06002C8F RID: 11407 RVA: 0x0013E650 File Offset: 0x0013C850
		[NullableContext(1)]
		public static NScoreLine Create(string label, string score, [Nullable(2)] Texture2D icon = null)
		{
			NScoreLine nscoreLine = PreloadManager.Cache.GetScene(NScoreLine._scenePath).Instantiate<NScoreLine>(0L);
			nscoreLine.GetNode<MegaLabel>("%Label").SetTextAutoSize(label);
			nscoreLine.GetNode<MegaLabel>("%Score").SetTextAutoSize(score);
			if (icon != null)
			{
				nscoreLine.GetNode<TextureRect>("%Icon").Texture = icon;
			}
			return nscoreLine;
		}

		/// <summary>
		/// 0.4 second animation of a score line animating in.
		/// Deferred due to how GridContainer lays out its elements after AddChildSafely().
		/// </summary>
		// Token: 0x06002C90 RID: 11408 RVA: 0x0013E6BC File Offset: 0x0013C8BC
		[NullableContext(1)]
		public async Task AnimateIn()
		{
			if (this.IsValid())
			{
				this._tween = base.CreateTween().SetParallel(true);
				this._tween.TweenProperty(this, "modulate:a", 1f, 0.3);
				this._tween.TweenProperty(this, "position:x", base.Position.X, 0.3).SetEase(1L).SetTrans(11L)
					.From(base.Position.X - 50f);
				if (SaveManager.Instance.PrefsSave.FastMode != FastModeType.Instant)
				{
					this._tween.Chain();
					this._tween.TweenInterval(0.1);
				}
				bool flag = await this._tween.AwaitFinished(this);
			}
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x0013E6FF File Offset: 0x0013C8FF
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002C92 RID: 11410 RVA: 0x0013E714 File Offset: 0x0013C914
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NScoreLine.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "label", 0L, "", 6L, false),
					new PropertyInfo(4L, "score", 0L, "", 6L, false),
					new PropertyInfo(24L, "icon", 0L, "", 6L, new StringName("Texture2D"), false)
				}, null),
				new MethodInfo(NScoreLine.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C93 RID: 11411 RVA: 0x0013E808 File Offset: 0x0013CA08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScoreLine.MethodName.Create && args.Count == 3)
			{
				NScoreLine nscoreLine = NScoreLine.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]), VariantUtils.ConvertTo<Texture2D>(args[2]));
				ret = VariantUtils.CreateFrom<NScoreLine>(ref nscoreLine);
				return true;
			}
			if ((ref method) == NScoreLine.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x0013E898 File Offset: 0x0013CA98
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScoreLine.MethodName.Create && args.Count == 3)
			{
				NScoreLine nscoreLine = NScoreLine.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]), VariantUtils.ConvertTo<Texture2D>(args[2]));
				ret = VariantUtils.CreateFrom<NScoreLine>(ref nscoreLine);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002C95 RID: 11413 RVA: 0x0013E900 File Offset: 0x0013CB00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NScoreLine.MethodName.Create || (ref method) == NScoreLine.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002C96 RID: 11414 RVA: 0x0013E927 File Offset: 0x0013CB27
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NScoreLine.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002C97 RID: 11415 RVA: 0x0013E94C File Offset: 0x0013CB4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NScoreLine.PropertyName._tween)
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
		// Token: 0x06002C98 RID: 11416 RVA: 0x0013E978 File Offset: 0x0013CB78
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NScoreLine.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C99 RID: 11417 RVA: 0x0013E9AD File Offset: 0x0013CBAD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NScoreLine.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06002C9A RID: 11418 RVA: 0x0013E9CC File Offset: 0x0013CBCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NScoreLine.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
		}

		// Token: 0x040010E6 RID: 4326
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/game_over_screen/score_line");

		// Token: 0x040010E7 RID: 4327
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001564 RID: 5476
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400520C RID: 21004
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400520D RID: 21005
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001565 RID: 5477
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400520E RID: 21006
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001566 RID: 5478
		public class SignalName : Control.SignalName
		{
		}
	}
}
