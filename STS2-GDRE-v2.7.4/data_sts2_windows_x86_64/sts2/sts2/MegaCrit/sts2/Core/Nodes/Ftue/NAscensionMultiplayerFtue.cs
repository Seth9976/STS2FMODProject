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
	/// This is a popup that lets you know that you have unlocked Multiplayer Ascensions and briefly explains it.
	/// This is NOT a true FTUE as disabling tutorials will not prevent this from showing up.
	/// </summary>
	// Token: 0x020003A7 RID: 935
	[ScriptPath("res://src/Core/Nodes/Ftue/NAscensionMultiplayerFtue.cs")]
	public class NAscensionMultiplayerFtue : NFtue
	{
		// Token: 0x06003854 RID: 14420 RVA: 0x001A97D8 File Offset: 0x001A79D8
		public override void _Ready()
		{
			base.GetNode<MegaLabel>("%Header").SetTextAutoSize(new LocString("ftues", "ASCENSION_MULTIPLAYER_FTUE_TITLE").GetFormattedText());
			base.GetNode<MegaRichTextLabel>("%Description").SetTextAutoSize(new LocString("ftues", "ASCENSION_MULTIPLAYER_FTUE_DESCRIPTION").GetFormattedText());
			base.GetNode<MegaRichTextLabel>("%Disclaimer").SetTextAutoSize(new LocString("ftues", "ASCENSION_MULTIPLAYER_FTUE_DISCLAIMER").GetFormattedText());
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

		// Token: 0x06003855 RID: 14421 RVA: 0x001A9951 File Offset: 0x001A7B51
		[NullableContext(2)]
		public static NAscensionMultiplayerFtue Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NAscensionMultiplayerFtue._scenePath).Instantiate<NAscensionMultiplayerFtue>(0L);
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x001A9972 File Offset: 0x001A7B72
		[NullableContext(1)]
		private void CloseFtue(NButton _)
		{
			SaveManager.Instance.MarkFtueAsComplete("ascension_multiplayer_ftue");
			base.CloseFtue();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003857 RID: 14423 RVA: 0x001A998C File Offset: 0x001A7B8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NAscensionMultiplayerFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAscensionMultiplayerFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NAscensionMultiplayerFtue.MethodName.CloseFtue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003858 RID: 14424 RVA: 0x001A9A6C File Offset: 0x001A7C6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAscensionMultiplayerFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAscensionMultiplayerFtue.MethodName.Create && args.Count == 0)
			{
				NAscensionMultiplayerFtue nascensionMultiplayerFtue = NAscensionMultiplayerFtue.Create();
				ret = VariantUtils.CreateFrom<NAscensionMultiplayerFtue>(ref nascensionMultiplayerFtue);
				return true;
			}
			if ((ref method) == NAscensionMultiplayerFtue.MethodName.CloseFtue && args.Count == 1)
			{
				this.CloseFtue(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x001A9B08 File Offset: 0x001A7D08
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAscensionMultiplayerFtue.MethodName.Create && args.Count == 0)
			{
				NAscensionMultiplayerFtue nascensionMultiplayerFtue = NAscensionMultiplayerFtue.Create();
				ret = VariantUtils.CreateFrom<NAscensionMultiplayerFtue>(ref nascensionMultiplayerFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600385A RID: 14426 RVA: 0x001A9B48 File Offset: 0x001A7D48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAscensionMultiplayerFtue.MethodName._Ready || (ref method) == NAscensionMultiplayerFtue.MethodName.Create || (ref method) == NAscensionMultiplayerFtue.MethodName.CloseFtue || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600385B RID: 14427 RVA: 0x001A9B7E File Offset: 0x001A7D7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x0600385C RID: 14428 RVA: 0x001A9B87 File Offset: 0x001A7D87
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x040015D3 RID: 5587
		[Nullable(1)]
		public const string id = "ascension_multiplayer_ftue";

		// Token: 0x040015D4 RID: 5588
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/ascension_multiplayer_ftue");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200178F RID: 6031
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005BF0 RID: 23536
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005BF1 RID: 23537
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'CloseFtue' method.
			/// </summary>
			// Token: 0x04005BF2 RID: 23538
			public new static readonly StringName CloseFtue = "CloseFtue";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001790 RID: 6032
		public new class PropertyName : NFtue.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001791 RID: 6033
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
