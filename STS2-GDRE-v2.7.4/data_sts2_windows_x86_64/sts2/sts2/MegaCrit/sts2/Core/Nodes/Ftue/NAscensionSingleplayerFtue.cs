using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	/// <summary>
	/// This is a popup that lets you know that you have unlocked Ascensions and briefly explains it.
	/// This is NOT a true FTUE as disabling tutorials will not prevent this from showing up.
	/// </summary>
	// Token: 0x020003A8 RID: 936
	[ScriptPath("res://src/Core/Nodes/Ftue/NAscensionSingleplayerFtue.cs")]
	public class NAscensionSingleplayerFtue : NFtue
	{
		// Token: 0x0600385F RID: 14431 RVA: 0x001A9BAC File Offset: 0x001A7DAC
		public override void _Ready()
		{
			base.GetNode<MegaLabel>("%Header").SetTextAutoSize(new LocString("ftues", "ASCENSION_SINGLEPLAYER_FTUE_TITLE").GetFormattedText());
			base.GetNode<MegaRichTextLabel>("%Description").SetTextAutoSize(new LocString("ftues", "ASCENSION_SINGLEPLAYER_FTUE_DESCRIPTION").GetFormattedText());
			base.GetNode<MegaRichTextLabel>("%Disclaimer").SetTextAutoSize(new LocString("ftues", "ASCENSION_SINGLEPLAYER_FTUE_DISCLAIMER").GetFormattedText());
			base.GetNode<NButton>("%FtueConfirmButton").Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
			Tween tween = base.CreateTween().SetParallel(true);
			Color modulate = base.Modulate;
			modulate.A = 0f;
			base.Modulate = modulate;
			tween.TweenProperty(this, "position:y", base.Position.Y, 0.3).SetEase(1L).SetTrans(10L)
				.From(base.Position.Y + 100f)
				.SetDelay(1.0);
			tween.TweenProperty(this, "modulate:a", 1f, 0.3).SetEase(1L).SetDelay(1.0);
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x001A9D25 File Offset: 0x001A7F25
		[NullableContext(2)]
		public static NAscensionSingleplayerFtue Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NAscensionSingleplayerFtue._scenePath).Instantiate<NAscensionSingleplayerFtue>(0L);
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x001A9D46 File Offset: 0x001A7F46
		[NullableContext(1)]
		private void CloseFtue(NButton _)
		{
			SaveManager.Instance.MarkFtueAsComplete("ascension_singleplayer_ftue");
			base.CloseFtue();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003862 RID: 14434 RVA: 0x001A9D60 File Offset: 0x001A7F60
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NAscensionSingleplayerFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionSingleplayerFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NAscensionSingleplayerFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003863 RID: 14435 RVA: 0x001A9E40 File Offset: 0x001A8040
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAscensionSingleplayerFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionSingleplayerFtue.MethodName.Create && args.Count == 0)
			{
				NAscensionSingleplayerFtue nascensionSingleplayerFtue = NAscensionSingleplayerFtue.Create();
				ret = VariantUtils.CreateFrom<NAscensionSingleplayerFtue>(ref nascensionSingleplayerFtue);
				return true;
			}
			if ((ref method) == NAscensionSingleplayerFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x001A9EDC File Offset: 0x001A80DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAscensionSingleplayerFtue.MethodName.Create && args.Count == 0)
			{
				NAscensionSingleplayerFtue nascensionSingleplayerFtue = NAscensionSingleplayerFtue.Create();
				ret = VariantUtils.CreateFrom<NAscensionSingleplayerFtue>(ref nascensionSingleplayerFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003865 RID: 14437 RVA: 0x001A9F1C File Offset: 0x001A811C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAscensionSingleplayerFtue.MethodName._Ready || (ref method) == NAscensionSingleplayerFtue.MethodName.Create || (ref method) == NAscensionSingleplayerFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003866 RID: 14438 RVA: 0x001A9F52 File Offset: 0x001A8152
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003867 RID: 14439 RVA: 0x001A9F5B File Offset: 0x001A815B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x040015D5 RID: 5589
		[Nullable(1)]
		public const string id = "ascension_singleplayer_ftue";

		// Token: 0x040015D6 RID: 5590
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/ascension_singleplayer_ftue");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001792 RID: 6034
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005BF3 RID: 23539
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005BF4 RID: 23540
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005BF5 RID: 23541
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001793 RID: 6035
		public new class PropertyName : NFtue.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001794 RID: 6036
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
