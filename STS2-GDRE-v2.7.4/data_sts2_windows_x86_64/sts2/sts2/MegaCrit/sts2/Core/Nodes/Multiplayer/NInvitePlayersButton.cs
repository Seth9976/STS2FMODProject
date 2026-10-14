using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	// Token: 0x0200038B RID: 907
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NInvitePlayersButton.cs")]
	public class NInvitePlayersButton : NButton
	{
		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x060035EE RID: 13806 RVA: 0x00192825 File Offset: 0x00190A25
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.viewMap };
			}
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x0019283C File Offset: 0x00190A3C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._container = base.GetParent<Control>();
			Control node = base.GetNode<Control>("Background");
			MegaRichTextLabel node2 = base.GetNode<MegaRichTextLabel>("Label");
			this._shaderMaterial = (ShaderMaterial)node.Material;
			node2.SetTextAutoSize(new LocString("main_menu_ui", "INVITE").GetFormattedText());
			this.UpdateVisibility();
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x001928AE File Offset: 0x00190AAE
		[NullableContext(1)]
		public void Initialize(StartRunLobby lobby)
		{
			this._startRunLobby = lobby;
			this._startRunLobby.PlayerConnected += this.OnPlayerConnected;
			this._startRunLobby.PlayerDisconnected += this.OnPlayerDisconnected;
			this.UpdateVisibility();
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x001928EB File Offset: 0x00190AEB
		public override void _ExitTree()
		{
			base._ExitTree();
			if (this._startRunLobby != null)
			{
				this._startRunLobby.PlayerConnected -= this.OnPlayerConnected;
				this._startRunLobby.PlayerDisconnected -= this.OnPlayerDisconnected;
			}
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x00192929 File Offset: 0x00190B29
		private void OnPlayerConnected(LobbyPlayer player)
		{
			this.UpdateVisibility();
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x00192931 File Offset: 0x00190B31
		private void OnPlayerDisconnected(LobbyPlayer player)
		{
			this.UpdateVisibility();
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x0019293C File Offset: 0x00190B3C
		private void UpdateVisibility()
		{
			this._container.Visible = this._startRunLobby != null && PlatformUtil.SupportsInviteDialog(this._startRunLobby.NetService.Platform) && this._startRunLobby.Players.Count < this._startRunLobby.MaxPlayers;
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x00192993 File Offset: 0x00190B93
		protected override void OnRelease()
		{
			if (this._startRunLobby != null)
			{
				PlatformUtil.OpenInviteDialog(this._startRunLobby.NetService);
			}
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x001929AD File Offset: 0x00190BAD
		protected override void OnFocus()
		{
			this._shaderMaterial.SetShaderParameter(NInvitePlayersButton._s, 1.1f);
			this._shaderMaterial.SetShaderParameter(NInvitePlayersButton._v, 1.1f);
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x001929E3 File Offset: 0x00190BE3
		protected override void OnUnfocus()
		{
			this._shaderMaterial.SetShaderParameter(NInvitePlayersButton._s, 1f);
			this._shaderMaterial.SetShaderParameter(NInvitePlayersButton._v, 1f);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060035F8 RID: 13816 RVA: 0x00192A1C File Offset: 0x00190C1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NInvitePlayersButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInvitePlayersButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInvitePlayersButton.MethodName.UpdateVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInvitePlayersButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInvitePlayersButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInvitePlayersButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060035F9 RID: 13817 RVA: 0x00192B4C File Offset: 0x00190D4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NInvitePlayersButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInvitePlayersButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInvitePlayersButton.MethodName.UpdateVisibility && args.Count == 0)
			{
				this.UpdateVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInvitePlayersButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInvitePlayersButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInvitePlayersButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060035FA RID: 13818 RVA: 0x00192C40 File Offset: 0x00190E40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NInvitePlayersButton.MethodName._Ready || (ref method) == NInvitePlayersButton.MethodName._ExitTree || (ref method) == NInvitePlayersButton.MethodName.UpdateVisibility || (ref method) == NInvitePlayersButton.MethodName.OnRelease || (ref method) == NInvitePlayersButton.MethodName.OnFocus || (ref method) == NInvitePlayersButton.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060035FB RID: 13819 RVA: 0x00192CAE File Offset: 0x00190EAE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NInvitePlayersButton.PropertyName._shaderMaterial)
			{
				this._shaderMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NInvitePlayersButton.PropertyName._container)
			{
				this._container = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060035FC RID: 13820 RVA: 0x00192CF0 File Offset: 0x00190EF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NInvitePlayersButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NInvitePlayersButton.PropertyName._shaderMaterial)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._shaderMaterial);
				return true;
			}
			if ((ref name) == NInvitePlayersButton.PropertyName._container)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._container);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060035FD RID: 13821 RVA: 0x00192D68 File Offset: 0x00190F68
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NInvitePlayersButton.PropertyName._shaderMaterial, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInvitePlayersButton.PropertyName._container, 0L, "", 4096L, false),
				new PropertyInfo(34L, NInvitePlayersButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060035FE RID: 13822 RVA: 0x00192DDF File Offset: 0x00190FDF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NInvitePlayersButton.PropertyName._shaderMaterial, Variant.From<ShaderMaterial>(ref this._shaderMaterial));
			info.AddProperty(NInvitePlayersButton.PropertyName._container, Variant.From<Control>(ref this._container));
		}

		/// <inheritdoc />
		// Token: 0x060035FF RID: 13823 RVA: 0x00192E14 File Offset: 0x00191014
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NInvitePlayersButton.PropertyName._shaderMaterial, ref variant))
			{
				this._shaderMaterial = variant.As<ShaderMaterial>();
			}
			Variant variant2;
			if (info.TryGetProperty(NInvitePlayersButton.PropertyName._container, ref variant2))
			{
				this._container = variant2.As<Control>();
			}
		}

		// Token: 0x040014DE RID: 5342
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040014DF RID: 5343
		[Nullable(1)]
		private static readonly StringName _s = new StringName("s");

		// Token: 0x040014E0 RID: 5344
		[Nullable(1)]
		private ShaderMaterial _shaderMaterial;

		// Token: 0x040014E1 RID: 5345
		[Nullable(1)]
		private Control _container;

		// Token: 0x040014E2 RID: 5346
		[Nullable(2)]
		private StartRunLobby _startRunLobby;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001712 RID: 5906
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040059EB RID: 23019
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040059EC RID: 23020
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdateVisibility' method.
			/// </summary>
			// Token: 0x040059ED RID: 23021
			public static readonly StringName UpdateVisibility = "UpdateVisibility";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x040059EE RID: 23022
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040059EF RID: 23023
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040059F0 RID: 23024
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001713 RID: 5907
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x040059F1 RID: 23025
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_shaderMaterial' field.
			/// </summary>
			// Token: 0x040059F2 RID: 23026
			public static readonly StringName _shaderMaterial = "_shaderMaterial";

			/// <summary>
			/// Cached name for the '_container' field.
			/// </summary>
			// Token: 0x040059F3 RID: 23027
			public static readonly StringName _container = "_container";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001714 RID: 5908
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
