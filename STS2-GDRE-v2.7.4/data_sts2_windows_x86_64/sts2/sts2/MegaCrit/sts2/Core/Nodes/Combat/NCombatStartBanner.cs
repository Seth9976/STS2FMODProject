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
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// Start of enemy combat banner (for non-boss enemies).
	/// Creates a player turn banner as well.
	/// </summary>
	// Token: 0x020003F6 RID: 1014
	[ScriptPath("res://src/Core/Nodes/Combat/NCombatStartBanner.cs")]
	public class NCombatStartBanner : Control
	{
		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06003E4F RID: 15951 RVA: 0x001DDA54 File Offset: 0x001DBC54
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCombatStartBanner._scenePath);
			}
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x001DDA60 File Offset: 0x001DBC60
		[NullableContext(2)]
		public static NCombatStartBanner Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (NCombatUi.IsDebugHideTextVfx)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCombatStartBanner._scenePath).Instantiate<NCombatStartBanner>(0L);
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x001DDA8C File Offset: 0x001DBC8C
		public override void _Ready()
		{
			this._colorRect = base.GetNode<ColorRect>("ColorRect");
			this._colorRect.Modulate = Colors.Transparent;
			this._label = base.GetNode<MegaLabel>("Label");
			this._label.SetTextAutoSize(new LocString("gameplay_ui", "BATTLE_START").GetFormattedText());
			this._label.Modulate = Colors.Transparent;
			TaskHelper.RunSafely(this.AnimateVfx());
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x001DDB10 File Offset: 0x001DBD10
		[NullableContext(1)]
		private async Task AnimateVfx()
		{
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play(Rng.Chaotic.NextItem<string>(TmpSfx.BattleStart), 1f, PitchVariance.None);
			}
			Tween tween = base.CreateTween().SetParallel(true);
			tween.TweenInterval(0.3);
			tween.Chain();
			tween.TweenProperty(this._colorRect, "modulate:a", 0.5f, 0.75).SetEase(1L).SetTrans(5L);
			tween.TweenProperty(this._colorRect, "scale", Vector2.One, 0.75).SetEase(1L).SetTrans(5L)
				.From(Vector2.One * 1.2f);
			tween.TweenProperty(this._label, "modulate:a", 1f, 1.2999999523162842).SetEase(1L).SetTrans(5L);
			tween.TweenProperty(this._label, "scale", Vector2.One, 0.75).SetEase(1L).SetTrans(5L)
				.From(Vector2.One * 2f);
			tween.TweenProperty(this._label, "modulate:a", 0f, 1.0).SetEase(1L).SetTrans(7L)
				.SetDelay(1.2999999523162842);
			TaskAwaiter<bool> taskAwaiter = tween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				base.GetParent().AddChildSafely(NPlayerTurnBanner.Create(1));
				tween = base.CreateTween();
				tween.TweenProperty(this._colorRect, "modulate:a", 0f, 1.0).SetEase(1L).SetTrans(7L)
					.SetDelay(1.5);
				bool flag = await tween.AwaitFinished(this);
				this.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003E53 RID: 15955 RVA: 0x001DDB54 File Offset: 0x001DBD54
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCombatStartBanner.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NCombatStartBanner.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003E54 RID: 15956 RVA: 0x001DDBD4 File Offset: 0x001DBDD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatStartBanner.MethodName.Create && args.Count == 0)
			{
				NCombatStartBanner ncombatStartBanner = NCombatStartBanner.Create();
				ret = VariantUtils.CreateFrom<NCombatStartBanner>(ref ncombatStartBanner);
				return true;
			}
			if ((ref method) == NCombatStartBanner.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x001DDC3C File Offset: 0x001DBE3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatStartBanner.MethodName.Create && args.Count == 0)
			{
				NCombatStartBanner ncombatStartBanner = NCombatStartBanner.Create();
				ret = VariantUtils.CreateFrom<NCombatStartBanner>(ref ncombatStartBanner);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003E56 RID: 15958 RVA: 0x001DDC7C File Offset: 0x001DBE7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatStartBanner.MethodName.Create || (ref method) == NCombatStartBanner.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003E57 RID: 15959 RVA: 0x001DDCA3 File Offset: 0x001DBEA3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatStartBanner.PropertyName._colorRect)
			{
				this._colorRect = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NCombatStartBanner.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003E58 RID: 15960 RVA: 0x001DDCE4 File Offset: 0x001DBEE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatStartBanner.PropertyName._colorRect)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._colorRect);
				return true;
			}
			if ((ref name) == NCombatStartBanner.PropertyName._label)
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
		// Token: 0x06003E59 RID: 15961 RVA: 0x001DDD3C File Offset: 0x001DBF3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCombatStartBanner.PropertyName._colorRect, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatStartBanner.PropertyName._label, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003E5A RID: 15962 RVA: 0x001DDD92 File Offset: 0x001DBF92
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCombatStartBanner.PropertyName._colorRect, Variant.From<ColorRect>(ref this._colorRect));
			info.AddProperty(NCombatStartBanner.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x06003E5B RID: 15963 RVA: 0x001DDDC8 File Offset: 0x001DBFC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatStartBanner.PropertyName._colorRect, ref variant))
			{
				this._colorRect = variant.As<ColorRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatStartBanner.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
		}

		// Token: 0x0400183A RID: 6202
		[Nullable(1)]
		private ColorRect _colorRect;

		// Token: 0x0400183B RID: 6203
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x0400183C RID: 6204
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("combat/combat_start_banner");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018B1 RID: 6321
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400607B RID: 24699
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400607C RID: 24700
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018B2 RID: 6322
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_colorRect' field.
			/// </summary>
			// Token: 0x0400607D RID: 24701
			public static readonly StringName _colorRect = "_colorRect";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x0400607E RID: 24702
			public static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018B3 RID: 6323
		public class SignalName : Control.SignalName
		{
		}
	}
}
