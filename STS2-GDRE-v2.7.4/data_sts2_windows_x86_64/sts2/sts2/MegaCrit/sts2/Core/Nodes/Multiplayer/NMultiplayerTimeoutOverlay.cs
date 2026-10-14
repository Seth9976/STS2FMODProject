using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	/// <summary>
	/// This overlay is displayed over everything on the client when we lose connection with the host for a long period of
	/// time.
	/// It is in both Run and Game. The Game one is only used when we are in the middle of a transition, or if the run isn't
	/// started yet. Usually, we want to use the Run one because it allows usage of menus (it's under the top bar), but
	/// sometimes we get locked while we're loading and we still want to display this over everything.
	/// </summary>
	// Token: 0x02000392 RID: 914
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerTimeoutOverlay.cs")]
	public class NMultiplayerTimeoutOverlay : Control
	{
		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x060036AE RID: 13998 RVA: 0x0019A813 File Offset: 0x00198A13
		// (set) Token: 0x060036AF RID: 13999 RVA: 0x0019A81B File Offset: 0x00198A1B
		public bool IsShown { get; private set; }

		// Token: 0x060036B0 RID: 14000 RVA: 0x0019A824 File Offset: 0x00198A24
		public override void _Ready()
		{
			this._icon = base.GetNode<TextureRect>("%Icon");
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x0019A83C File Offset: 0x00198A3C
		public void Relocalize()
		{
			MegaLabel node = base.GetNode<MegaLabel>("%Title");
			MegaRichTextLabel node2 = base.GetNode<MegaRichTextLabel>("%Description");
			node.SetTextAutoSize(new LocString("main_menu_ui", "TIMEOUT_OVERLAY.title").GetFormattedText());
			node2.SetTextAutoSize(new LocString("main_menu_ui", "TIMEOUT_OVERLAY.description").GetFormattedText());
			node.RefreshFont();
			node2.RefreshFont();
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x0019A8AC File Offset: 0x00198AAC
		[NullableContext(1)]
		public void Initialize(INetGameService netService, bool isGameLevel)
		{
			NetClientGameService netClientGameService = netService as NetClientGameService;
			if (netClientGameService == null)
			{
				return;
			}
			this._netService = netClientGameService;
			this._gameLevel = isGameLevel;
			TaskHelper.RunSafely(this.UpdateLoop());
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x0019A8E0 File Offset: 0x00198AE0
		[NullableContext(1)]
		private async Task UpdateLoop()
		{
			for (;;)
			{
				NetClientGameService netService = this._netService;
				if (netService == null || !netService.IsConnected || !this.IsValid())
				{
					goto IL_0164;
				}
				ConnectionStats statsForPeer = this._netService.GetStatsForPeer(this._netService.HostNetId);
				if (statsForPeer == null)
				{
					break;
				}
				int num = ((statsForPeer.LastReceivedTime != null) ? ((int)(Time.GetTicksMsec() - statsForPeer.LastReceivedTime.Value)) : 0);
				bool flag = this._gameLevel == (this._netService.IsGameLoading || !RunManager.Instance.IsInProgress);
				int num2 = (statsForPeer.RemoteIsLoading ? 8000 : 3000);
				bool flag2 = flag && num >= num2;
				if (!this.IsShown && flag2)
				{
					base.Visible = true;
				}
				else if (this.IsShown && !flag2)
				{
					base.Visible = false;
				}
				this.IsShown = flag2;
				await Task.Delay(200);
			}
			return;
			IL_0164:
			if (this.IsValid())
			{
				base.Visible = false;
			}
			this._netService = null;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060036B4 RID: 14004 RVA: 0x0019A924 File Offset: 0x00198B24
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NMultiplayerTimeoutOverlay.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTimeoutOverlay.MethodName.Relocalize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036B5 RID: 14005 RVA: 0x0019A998 File Offset: 0x00198B98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerTimeoutOverlay.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTimeoutOverlay.MethodName.Relocalize && args.Count == 0)
			{
				this.Relocalize();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060036B6 RID: 14006 RVA: 0x0019A9F8 File Offset: 0x00198BF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerTimeoutOverlay.MethodName._Ready || (ref method) == NMultiplayerTimeoutOverlay.MethodName.Relocalize || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060036B7 RID: 14007 RVA: 0x0019AA20 File Offset: 0x00198C20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerTimeoutOverlay.PropertyName.IsShown)
			{
				this.IsShown = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerTimeoutOverlay.PropertyName._gameLevel)
			{
				this._gameLevel = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerTimeoutOverlay.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060036B8 RID: 14008 RVA: 0x0019AA88 File Offset: 0x00198C88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerTimeoutOverlay.PropertyName.IsShown)
			{
				bool isShown = this.IsShown;
				value = VariantUtils.CreateFrom<bool>(ref isShown);
				return true;
			}
			if ((ref name) == NMultiplayerTimeoutOverlay.PropertyName._gameLevel)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._gameLevel);
				return true;
			}
			if ((ref name) == NMultiplayerTimeoutOverlay.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060036B9 RID: 14009 RVA: 0x0019AB00 File Offset: 0x00198D00
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NMultiplayerTimeoutOverlay.PropertyName._gameLevel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerTimeoutOverlay.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerTimeoutOverlay.PropertyName.IsShown, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060036BA RID: 14010 RVA: 0x0019AB78 File Offset: 0x00198D78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isShown = NMultiplayerTimeoutOverlay.PropertyName.IsShown;
			bool isShown2 = this.IsShown;
			info.AddProperty(isShown, Variant.From<bool>(ref isShown2));
			info.AddProperty(NMultiplayerTimeoutOverlay.PropertyName._gameLevel, Variant.From<bool>(ref this._gameLevel));
			info.AddProperty(NMultiplayerTimeoutOverlay.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
		}

		/// <inheritdoc />
		// Token: 0x060036BB RID: 14011 RVA: 0x0019ABD4 File Offset: 0x00198DD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerTimeoutOverlay.PropertyName.IsShown, ref variant))
			{
				this.IsShown = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMultiplayerTimeoutOverlay.PropertyName._gameLevel, ref variant2))
			{
				this._gameLevel = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMultiplayerTimeoutOverlay.PropertyName._icon, ref variant3))
			{
				this._icon = variant3.As<TextureRect>();
			}
		}

		// Token: 0x0400152C RID: 5420
		private const int _noResponseMsec = 3000;

		// Token: 0x0400152D RID: 5421
		private const int _loadingNoResponseMsec = 8000;

		// Token: 0x0400152E RID: 5422
		private bool _gameLevel;

		// Token: 0x0400152F RID: 5423
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x04001530 RID: 5424
		[Nullable(2)]
		private NetClientGameService _netService;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001739 RID: 5945
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005AAF RID: 23215
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Relocalize' method.
			/// </summary>
			// Token: 0x04005AB0 RID: 23216
			public static readonly StringName Relocalize = "Relocalize";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200173A RID: 5946
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsShown' property.
			/// </summary>
			// Token: 0x04005AB1 RID: 23217
			public static readonly StringName IsShown = "IsShown";

			/// <summary>
			/// Cached name for the '_gameLevel' field.
			/// </summary>
			// Token: 0x04005AB2 RID: 23218
			public static readonly StringName _gameLevel = "_gameLevel";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04005AB3 RID: 23219
			public static readonly StringName _icon = "_icon";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200173B RID: 5947
		public class SignalName : Control.SignalName
		{
		}
	}
}
