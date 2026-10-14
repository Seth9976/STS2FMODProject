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
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x02000400 RID: 1024
	[ScriptPath("res://src/Core/Nodes/Combat/NEnemyTurnBanner.cs")]
	public class NEnemyTurnBanner : Control
	{
		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003FBF RID: 16319 RVA: 0x001E9BEB File Offset: 0x001E7DEB
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NEnemyTurnBanner._scenePath);
			}
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x001E9BF7 File Offset: 0x001E7DF7
		[NullableContext(2)]
		public static NEnemyTurnBanner Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (NCombatUi.IsDebugHideTextVfx)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NEnemyTurnBanner._scenePath).Instantiate<NEnemyTurnBanner>(0L);
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x001E9C24 File Offset: 0x001E7E24
		public override void _Ready()
		{
			this._label = base.GetNode<MegaLabel>("Label");
			this._label.SetTextAutoSize(new LocString("gameplay_ui", "ENEMY_TURN").GetFormattedText());
			base.Modulate = Colors.Transparent;
			TaskHelper.RunSafely(this.Display());
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x001E9C80 File Offset: 0x001E7E80
		[NullableContext(1)]
		private async Task Display()
		{
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("enemy_turn.mp3", 1f, PitchVariance.None);
			}
			this._label.Scale = Vector2.One * 2f;
			Tween tween = base.CreateTween();
			tween.SetParallel(true);
			tween.TweenProperty(this._label, "scale", Vector2.One, 0.75).SetEase(1L).SetTrans(5L);
			tween.TweenProperty(this, "modulate:a", 1f, 1.2999999523162842).SetEase(1L).SetTrans(5L);
			tween.Chain();
			tween.TweenProperty(this._label, "modulate", Colors.Red, 1.0).SetEase(1L).SetTrans(5L)
				.FromCurrent();
			tween.TweenProperty(this, "modulate:a", 0f, 1.0).SetEase(1L).SetTrans(7L);
			bool flag = await tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003FC3 RID: 16323 RVA: 0x001E9CC4 File Offset: 0x001E7EC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NEnemyTurnBanner.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NEnemyTurnBanner.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003FC4 RID: 16324 RVA: 0x001E9D44 File Offset: 0x001E7F44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEnemyTurnBanner.MethodName.Create && args.Count == 0)
			{
				NEnemyTurnBanner nenemyTurnBanner = NEnemyTurnBanner.Create();
				ret = VariantUtils.CreateFrom<NEnemyTurnBanner>(ref nenemyTurnBanner);
				return true;
			}
			if ((ref method) == NEnemyTurnBanner.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x001E9DAC File Offset: 0x001E7FAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEnemyTurnBanner.MethodName.Create && args.Count == 0)
			{
				NEnemyTurnBanner nenemyTurnBanner = NEnemyTurnBanner.Create();
				ret = VariantUtils.CreateFrom<NEnemyTurnBanner>(ref nenemyTurnBanner);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003FC6 RID: 16326 RVA: 0x001E9DEC File Offset: 0x001E7FEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEnemyTurnBanner.MethodName.Create || (ref method) == NEnemyTurnBanner.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003FC7 RID: 16327 RVA: 0x001E9E13 File Offset: 0x001E8013
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEnemyTurnBanner.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003FC8 RID: 16328 RVA: 0x001E9E38 File Offset: 0x001E8038
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEnemyTurnBanner.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003FC9 RID: 16329 RVA: 0x001E9E64 File Offset: 0x001E8064
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEnemyTurnBanner.PropertyName._label, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003FCA RID: 16330 RVA: 0x001E9E99 File Offset: 0x001E8099
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEnemyTurnBanner.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x06003FCB RID: 16331 RVA: 0x001E9EB8 File Offset: 0x001E80B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEnemyTurnBanner.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
		}

		// Token: 0x040018B8 RID: 6328
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x040018B9 RID: 6329
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("combat/enemy_turn_banner");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018E4 RID: 6372
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04006199 RID: 24985
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400619A RID: 24986
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018E5 RID: 6373
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x0400619B RID: 24987
			public static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018E6 RID: 6374
		public class SignalName : Control.SignalName
		{
		}
	}
}
