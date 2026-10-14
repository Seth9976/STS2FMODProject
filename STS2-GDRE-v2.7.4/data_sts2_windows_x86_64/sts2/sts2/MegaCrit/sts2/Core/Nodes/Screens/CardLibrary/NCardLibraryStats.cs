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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary
{
	// Token: 0x02000354 RID: 852
	[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardLibraryStats.cs")]
	public class NCardLibraryStats : Control
	{
		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06003131 RID: 12593 RVA: 0x0016DBAF File Offset: 0x0016BDAF
		[Nullable(1)]
		private static LocString Victories
		{
			[NullableContext(1)]
			get
			{
				return new LocString("card_library", "VICTORIES");
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06003132 RID: 12594 RVA: 0x0016DBC0 File Offset: 0x0016BDC0
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("screens/card_library/card_library_stats");
			}
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x0016DBCC File Offset: 0x0016BDCC
		[NullableContext(1)]
		public static NCardLibraryStats Create()
		{
			return PreloadManager.Cache.GetScene(NCardLibraryStats.ScenePath).Instantiate<NCardLibraryStats>(0L);
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x0016DBE4 File Offset: 0x0016BDE4
		public override void _Ready()
		{
			this._label = base.GetNode<MegaRichTextLabel>("%Label");
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x0016DBFC File Offset: 0x0016BDFC
		[NullableContext(1)]
		public void UpdateStats(CardModel card)
		{
			LocString victories = NCardLibraryStats.Victories;
			CardStats cardStats;
			long num;
			if (SaveManager.Instance.Progress.CardStats.TryGetValue(card.Id, out cardStats))
			{
				num = cardStats.TimesWon;
			}
			else
			{
				num = 0L;
			}
			victories.Add("Victories", num);
			this._label.Text = victories.GetFormattedText();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003136 RID: 12598 RVA: 0x0016DC5C File Offset: 0x0016BE5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCardLibraryStats.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NCardLibraryStats.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003137 RID: 12599 RVA: 0x0016DCDC File Offset: 0x0016BEDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardLibraryStats.MethodName.Create && args.Count == 0)
			{
				NCardLibraryStats ncardLibraryStats = NCardLibraryStats.Create();
				ret = VariantUtils.CreateFrom<NCardLibraryStats>(ref ncardLibraryStats);
				return true;
			}
			if ((ref method) == NCardLibraryStats.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x0016DD44 File Offset: 0x0016BF44
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardLibraryStats.MethodName.Create && args.Count == 0)
			{
				NCardLibraryStats ncardLibraryStats = NCardLibraryStats.Create();
				ret = VariantUtils.CreateFrom<NCardLibraryStats>(ref ncardLibraryStats);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003139 RID: 12601 RVA: 0x0016DD84 File Offset: 0x0016BF84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardLibraryStats.MethodName.Create || (ref method) == NCardLibraryStats.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600313A RID: 12602 RVA: 0x0016DDAB File Offset: 0x0016BFAB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardLibraryStats.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600313B RID: 12603 RVA: 0x0016DDD0 File Offset: 0x0016BFD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardLibraryStats.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600313C RID: 12604 RVA: 0x0016DDFC File Offset: 0x0016BFFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardLibraryStats.PropertyName._label, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600313D RID: 12605 RVA: 0x0016DE31 File Offset: 0x0016C031
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardLibraryStats.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x0600313E RID: 12606 RVA: 0x0016DE50 File Offset: 0x0016C050
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardLibraryStats.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x04001332 RID: 4914
		[Nullable(1)]
		private MegaRichTextLabel _label;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001638 RID: 5688
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005699 RID: 22169
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400569A RID: 22170
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001639 RID: 5689
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x0400569B RID: 22171
			public static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200163A RID: 5690
		public class SignalName : Control.SignalName
		{
		}
	}
}
