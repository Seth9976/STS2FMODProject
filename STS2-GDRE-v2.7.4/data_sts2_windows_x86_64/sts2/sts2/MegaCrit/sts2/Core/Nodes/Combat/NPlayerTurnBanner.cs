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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x02000409 RID: 1033
	[ScriptPath("res://src/Core/Nodes/Combat/NPlayerTurnBanner.cs")]
	public class NPlayerTurnBanner : Control
	{
		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060040E5 RID: 16613 RVA: 0x001F5ADB File Offset: 0x001F3CDB
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NPlayerTurnBanner._scenePath);
			}
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x001F5AE8 File Offset: 0x001F3CE8
		[NullableContext(2)]
		public static NPlayerTurnBanner Create(int roundNumber)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (NCombatUi.IsDebugHideTextVfx)
			{
				return null;
			}
			NPlayerTurnBanner nplayerTurnBanner = PreloadManager.Cache.GetScene(NPlayerTurnBanner._scenePath).Instantiate<NPlayerTurnBanner>(0L);
			nplayerTurnBanner._roundNumber = roundNumber;
			return nplayerTurnBanner;
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x001F5B28 File Offset: 0x001F3D28
		public override void _Ready()
		{
			this._label = base.GetNode<MegaLabel>("Label");
			if (CombatManager.Instance.PlayersTakingExtraTurn.Count > 0)
			{
				this._label.SetTextAutoSize(new LocString("gameplay_ui", "PLAYER_TURN_EXTRA").GetFormattedText());
			}
			else
			{
				this._label.SetTextAutoSize(new LocString("gameplay_ui", "PLAYER_TURN").GetFormattedText());
			}
			this._turnLabel = base.GetNode<MegaLabel>("TurnNumber");
			LocString locString = new LocString("gameplay_ui", "TURN_COUNT");
			locString.Add("turnNumber", this._roundNumber);
			this._turnLabel.SetTextAutoSize(locString.GetFormattedText());
			base.Modulate = Colors.Transparent;
			TaskHelper.RunSafely(this.Display());
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x001F5C04 File Offset: 0x001F3E04
		[NullableContext(1)]
		private async Task Display()
		{
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("player_turn.mp3", 1f, PitchVariance.None);
			}
			Tween tween = base.CreateTween();
			tween.SetParallel(true);
			tween.TweenProperty(this, "modulate:a", 1f, 1.0).SetEase(1L).SetTrans(5L);
			tween.TweenProperty(this._label, "position", this._label.Position + new Vector2(0f, -50f), 1.5).SetEase(1L).SetTrans(5L);
			tween.TweenProperty(this._turnLabel, "position", this._turnLabel.Position + new Vector2(0f, 50f), 1.5).SetEase(1L).SetTrans(5L);
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
				tween = base.CreateTween();
				tween.TweenInterval(0.4);
				tween.TweenProperty(this, "modulate:a", 0f, 0.30000001192092896).SetEase(1L).SetTrans(1L);
				bool flag = await tween.AwaitFinished(this);
				this.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060040E9 RID: 16617 RVA: 0x001F5C48 File Offset: 0x001F3E48
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NPlayerTurnBanner.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "roundNumber", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPlayerTurnBanner.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060040EA RID: 16618 RVA: 0x001F5CEC File Offset: 0x001F3EEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPlayerTurnBanner.MethodName.Create && args.Count == 1)
			{
				NPlayerTurnBanner nplayerTurnBanner = NPlayerTurnBanner.Create(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<NPlayerTurnBanner>(ref nplayerTurnBanner);
				return true;
			}
			if ((ref method) == NPlayerTurnBanner.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x001F5D60 File Offset: 0x001F3F60
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPlayerTurnBanner.MethodName.Create && args.Count == 1)
			{
				NPlayerTurnBanner nplayerTurnBanner = NPlayerTurnBanner.Create(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<NPlayerTurnBanner>(ref nplayerTurnBanner);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060040EC RID: 16620 RVA: 0x001F5DAE File Offset: 0x001F3FAE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPlayerTurnBanner.MethodName.Create || (ref method) == NPlayerTurnBanner.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060040ED RID: 16621 RVA: 0x001F5DD8 File Offset: 0x001F3FD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPlayerTurnBanner.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPlayerTurnBanner.PropertyName._turnLabel)
			{
				this._turnLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPlayerTurnBanner.PropertyName._roundNumber)
			{
				this._roundNumber = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060040EE RID: 16622 RVA: 0x001F5E40 File Offset: 0x001F4040
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPlayerTurnBanner.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NPlayerTurnBanner.PropertyName._turnLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._turnLabel);
				return true;
			}
			if ((ref name) == NPlayerTurnBanner.PropertyName._roundNumber)
			{
				value = VariantUtils.CreateFrom<int>(ref this._roundNumber);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060040EF RID: 16623 RVA: 0x001F5EB8 File Offset: 0x001F40B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPlayerTurnBanner.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPlayerTurnBanner.PropertyName._turnLabel, 0L, "", 4096L, false),
				new PropertyInfo(2L, NPlayerTurnBanner.PropertyName._roundNumber, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060040F0 RID: 16624 RVA: 0x001F5F30 File Offset: 0x001F4130
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPlayerTurnBanner.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NPlayerTurnBanner.PropertyName._turnLabel, Variant.From<MegaLabel>(ref this._turnLabel));
			info.AddProperty(NPlayerTurnBanner.PropertyName._roundNumber, Variant.From<int>(ref this._roundNumber));
		}

		/// <inheritdoc />
		// Token: 0x060040F1 RID: 16625 RVA: 0x001F5F88 File Offset: 0x001F4188
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPlayerTurnBanner.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPlayerTurnBanner.PropertyName._turnLabel, ref variant2))
			{
				this._turnLabel = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPlayerTurnBanner.PropertyName._roundNumber, ref variant3))
			{
				this._roundNumber = variant3.As<int>();
			}
		}

		// Token: 0x04001942 RID: 6466
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x04001943 RID: 6467
		[Nullable(1)]
		private MegaLabel _turnLabel;

		// Token: 0x04001944 RID: 6468
		private int _roundNumber;

		// Token: 0x04001945 RID: 6469
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("combat/player_turn_banner");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001912 RID: 6418
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040062B4 RID: 25268
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040062B5 RID: 25269
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001913 RID: 6419
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x040062B6 RID: 25270
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_turnLabel' field.
			/// </summary>
			// Token: 0x040062B7 RID: 25271
			public static readonly StringName _turnLabel = "_turnLabel";

			/// <summary>
			/// Cached name for the '_roundNumber' field.
			/// </summary>
			// Token: 0x040062B8 RID: 25272
			public static readonly StringName _roundNumber = "_roundNumber";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001914 RID: 6420
		public class SignalName : Control.SignalName
		{
		}
	}
}
