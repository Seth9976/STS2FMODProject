using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	// Token: 0x0200032A RID: 810
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunCharacterContainer.cs")]
	public class NDailyRunCharacterContainer : Control
	{
		// Token: 0x06002D34 RID: 11572 RVA: 0x00143C20 File Offset: 0x00141E20
		public override void _Ready()
		{
			this._characterIconContainer = base.GetNode<Control>("%CharacterIconContainer");
			this._playerNameLabel = base.GetNode<MegaLabel>("%PlayerNameLabel");
			this._characterNameLabel = base.GetNode<MegaLabel>("%CharacterNameLabel");
			this._ascensionLabel = base.GetNode<MegaLabel>("%AscensionLabel");
			this._ascensionNumberLabel = base.GetNode<MegaLabel>("%AscensionNumberLabel");
			this._readyIndicator = base.GetNode<Control>("%ReadyIndicator");
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x00143CB4 File Offset: 0x00141EB4
		[NullableContext(1)]
		public void Fill(CharacterModel character, ulong playerId, int ascension, INetGameService netService)
		{
			NDailyRunCharacterContainer._ascensionLoc.Add("ascension", ascension);
			bool flag = netService.Type.IsMultiplayer();
			Control icon = character.Icon;
			foreach (Node node in this._characterIconContainer.GetChildren(false))
			{
				this._characterIconContainer.RemoveChildSafely(node);
			}
			this._playerNameLabel.Visible = flag;
			this._characterNameLabel.Modulate = (flag ? StsColors.cream : StsColors.gold);
			this._characterIconContainer.AddChildSafely(icon);
			this._characterNameLabel.SetTextAutoSize(character.Title.GetFormattedText());
			this._playerNameLabel.SetTextAutoSize(PlatformUtil.GetPlayerNameRaw(netService.Platform, playerId));
			this._ascensionLabel.SetTextAutoSize(NDailyRunCharacterContainer._ascensionLoc.GetFormattedText());
			this._ascensionNumberLabel.SetTextAutoSize(ascension.ToString());
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x00143DBC File Offset: 0x00141FBC
		public void SetIsReady(bool isReady)
		{
			this._readyIndicator.Visible = isReady;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D37 RID: 11575 RVA: 0x00143DCC File Offset: 0x00141FCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NDailyRunCharacterContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunCharacterContainer.MethodName.SetIsReady, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isReady", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D38 RID: 11576 RVA: 0x00143E64 File Offset: 0x00142064
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunCharacterContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunCharacterContainer.MethodName.SetIsReady && args.Count == 1)
			{
				this.SetIsReady(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002D39 RID: 11577 RVA: 0x00143ED2 File Offset: 0x001420D2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDailyRunCharacterContainer.MethodName._Ready || (ref method) == NDailyRunCharacterContainer.MethodName.SetIsReady || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002D3A RID: 11578 RVA: 0x00143EFC File Offset: 0x001420FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._characterIconContainer)
			{
				this._characterIconContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._playerNameLabel)
			{
				this._playerNameLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._characterNameLabel)
			{
				this._characterNameLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._ascensionLabel)
			{
				this._ascensionLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._ascensionNumberLabel)
			{
				this._ascensionNumberLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._readyIndicator)
			{
				this._readyIndicator = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002D3B RID: 11579 RVA: 0x00143FB4 File Offset: 0x001421B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._characterIconContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._characterIconContainer);
				return true;
			}
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._playerNameLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._playerNameLabel);
				return true;
			}
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._characterNameLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._characterNameLabel);
				return true;
			}
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._ascensionLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._ascensionLabel);
				return true;
			}
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._ascensionNumberLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._ascensionNumberLabel);
				return true;
			}
			if ((ref name) == NDailyRunCharacterContainer.PropertyName._readyIndicator)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._readyIndicator);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D3C RID: 11580 RVA: 0x0014408C File Offset: 0x0014228C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDailyRunCharacterContainer.PropertyName._characterIconContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunCharacterContainer.PropertyName._playerNameLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunCharacterContainer.PropertyName._characterNameLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunCharacterContainer.PropertyName._ascensionLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunCharacterContainer.PropertyName._ascensionNumberLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDailyRunCharacterContainer.PropertyName._readyIndicator, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D3D RID: 11581 RVA: 0x00144168 File Offset: 0x00142368
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDailyRunCharacterContainer.PropertyName._characterIconContainer, Variant.From<Control>(ref this._characterIconContainer));
			info.AddProperty(NDailyRunCharacterContainer.PropertyName._playerNameLabel, Variant.From<MegaLabel>(ref this._playerNameLabel));
			info.AddProperty(NDailyRunCharacterContainer.PropertyName._characterNameLabel, Variant.From<MegaLabel>(ref this._characterNameLabel));
			info.AddProperty(NDailyRunCharacterContainer.PropertyName._ascensionLabel, Variant.From<MegaLabel>(ref this._ascensionLabel));
			info.AddProperty(NDailyRunCharacterContainer.PropertyName._ascensionNumberLabel, Variant.From<MegaLabel>(ref this._ascensionNumberLabel));
			info.AddProperty(NDailyRunCharacterContainer.PropertyName._readyIndicator, Variant.From<Control>(ref this._readyIndicator));
		}

		/// <inheritdoc />
		// Token: 0x06002D3E RID: 11582 RVA: 0x00144200 File Offset: 0x00142400
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._characterIconContainer, ref variant))
			{
				this._characterIconContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._playerNameLabel, ref variant2))
			{
				this._playerNameLabel = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._characterNameLabel, ref variant3))
			{
				this._characterNameLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._ascensionLabel, ref variant4))
			{
				this._ascensionLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._ascensionNumberLabel, ref variant5))
			{
				this._ascensionNumberLabel = variant5.As<MegaLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._readyIndicator, ref variant6))
			{
				this._readyIndicator = variant6.As<Control>();
			}
		}

		// Token: 0x0400113F RID: 4415
		[Nullable(1)]
		private static readonly LocString _ascensionLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.ASCENSION");

		// Token: 0x04001140 RID: 4416
		[Nullable(1)]
		private Control _characterIconContainer;

		// Token: 0x04001141 RID: 4417
		[Nullable(1)]
		private MegaLabel _playerNameLabel;

		// Token: 0x04001142 RID: 4418
		[Nullable(1)]
		private MegaLabel _characterNameLabel;

		// Token: 0x04001143 RID: 4419
		[Nullable(1)]
		private MegaLabel _ascensionLabel;

		// Token: 0x04001144 RID: 4420
		[Nullable(1)]
		private MegaLabel _ascensionNumberLabel;

		// Token: 0x04001145 RID: 4421
		[Nullable(1)]
		private Control _readyIndicator;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001589 RID: 5513
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005297 RID: 21143
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetIsReady' method.
			/// </summary>
			// Token: 0x04005298 RID: 21144
			public static readonly StringName SetIsReady = "SetIsReady";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200158A RID: 5514
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_characterIconContainer' field.
			/// </summary>
			// Token: 0x04005299 RID: 21145
			public static readonly StringName _characterIconContainer = "_characterIconContainer";

			/// <summary>
			/// Cached name for the '_playerNameLabel' field.
			/// </summary>
			// Token: 0x0400529A RID: 21146
			public static readonly StringName _playerNameLabel = "_playerNameLabel";

			/// <summary>
			/// Cached name for the '_characterNameLabel' field.
			/// </summary>
			// Token: 0x0400529B RID: 21147
			public static readonly StringName _characterNameLabel = "_characterNameLabel";

			/// <summary>
			/// Cached name for the '_ascensionLabel' field.
			/// </summary>
			// Token: 0x0400529C RID: 21148
			public static readonly StringName _ascensionLabel = "_ascensionLabel";

			/// <summary>
			/// Cached name for the '_ascensionNumberLabel' field.
			/// </summary>
			// Token: 0x0400529D RID: 21149
			public static readonly StringName _ascensionNumberLabel = "_ascensionNumberLabel";

			/// <summary>
			/// Cached name for the '_readyIndicator' field.
			/// </summary>
			// Token: 0x0400529E RID: 21150
			public static readonly StringName _readyIndicator = "_readyIndicator";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200158B RID: 5515
		public class SignalName : Control.SignalName
		{
		}
	}
}
