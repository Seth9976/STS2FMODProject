using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	/// <summary>
	/// Shows run info in the main menu! Handy!
	/// </summary>
	// Token: 0x020002FE RID: 766
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NContinueRunInfo.cs")]
	public class NContinueRunInfo : Control
	{
		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060029E8 RID: 10728 RVA: 0x00124C6D File Offset: 0x00122E6D
		// (set) Token: 0x060029E9 RID: 10729 RVA: 0x00124C75 File Offset: 0x00122E75
		public bool HasResult { get; private set; }

		// Token: 0x060029EA RID: 10730 RVA: 0x00124C80 File Offset: 0x00122E80
		public override void _Ready()
		{
			this._initPosition = base.Position;
			base.Modulate = StsColors.transparentWhite;
			this._runInfoContainer = base.GetNode<VBoxContainer>("%RunInfoContainer");
			this._errorContainer = base.GetNode<Control>("%ErrorContainer");
			this._dateLabel = base.GetNode<MegaRichTextLabel>("%DateLabel");
			this._goldLabel = base.GetNode<MegaRichTextLabel>("%GoldLabel");
			this._healthLabel = base.GetNode<MegaRichTextLabel>("%HealthLabel");
			this._progressLabel = base.GetNode<MegaRichTextLabel>("%ProgressLabel");
			this._charIcon = base.GetNode<TextureRect>("%CharacterIcon");
			this._ascensionLabel = base.GetNode<MegaRichTextLabel>("%AscensionLabel");
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x00124D54 File Offset: 0x00122F54
		public void AnimShow()
		{
			Tween visTween = this._visTween;
			if (visTween != null)
			{
				visTween.Kill();
			}
			this._visTween = base.CreateTween().SetParallel(true);
			this._visTween.TweenProperty(this, "position", this._initPosition + new Vector2(0f, -20f), 0.20000000298023224).SetEase(1L).SetTrans(10L);
			this._visTween.TweenProperty(this, "modulate:a", 1f, 0.20000000298023224);
			this._isShown = true;
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x00124E04 File Offset: 0x00123004
		public void AnimHide()
		{
			if (!this._isShown)
			{
				return;
			}
			Tween visTween = this._visTween;
			if (visTween != null)
			{
				visTween.Kill();
			}
			this._visTween = base.CreateTween().SetParallel(true);
			this._visTween.TweenProperty(this, "position", this._initPosition, 0.20000000298023224);
			this._visTween.TweenProperty(this, "modulate:a", 0f, 0.20000000298023224);
			this._isShown = false;
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x00124E9C File Offset: 0x0012309C
		public void SetResult([Nullable(new byte[] { 2, 1 })] ReadSaveResult<SerializableRun> result)
		{
			if (result != null && result.Success && result.SaveData != null)
			{
				try
				{
					this.ShowInfo(result.SaveData);
					goto IL_005E;
				}
				catch (Exception ex)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Exception occurred while displaying continue run info: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					this.ShowError();
					goto IL_005E;
				}
			}
			if (result != null)
			{
				this.ShowError();
			}
			IL_005E:
			this.HasResult = result != null;
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x00124F24 File Offset: 0x00123124
		private void ShowInfo(SerializableRun save)
		{
			this._errorContainer.Visible = false;
			this._runInfoContainer.Visible = true;
			DateTimeFormatInfo dateTimeFormat = LocManager.Instance.CultureInfo.DateTimeFormat;
			DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeSeconds(save.SaveTime).UtcDateTime, TimeZoneInfo.Local);
			string rawText = new LocString("main_menu_ui", "CONTINUE_RUN_INFO.savedTimeFormat").GetRawText();
			string text = dateTime.ToString(rawText, dateTimeFormat);
			LocString locString = new LocString("main_menu_ui", "CONTINUE_RUN_INFO.saved");
			locString.Add("LastSavedTime", text);
			this._dateLabel.Text = locString.GetFormattedText();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (save.Ascension > 0)
			{
				LocString locString2 = new LocString("main_menu_ui", "CONTINUE_RUN_INFO.ascension");
				MegaRichTextLabel ascensionLabel = this._ascensionLabel;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted(locString2.GetFormattedText());
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(save.Ascension);
				ascensionLabel.Text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				this._ascensionLabel.Visible = false;
			}
			ActModel byId = ModelDb.GetById<ActModel>(save.Acts[save.CurrentActIndex].Id);
			SerializablePlayer serializablePlayer = save.Players[0];
			this._charIcon.Texture = ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId).IconTexture;
			string formattedText = byId.Title.GetFormattedText();
			string formattedText2 = new LocString("main_menu_ui", "CONTINUE_RUN_INFO.floor").GetFormattedText();
			int num = save.VisitedMapCoords.Count;
			for (int i = 0; i < save.CurrentActIndex; i++)
			{
				num += ModelDb.GetById<ActModel>(save.Acts[i].Id).GetNumberOfFloors(save.Players.Count > 1);
			}
			MegaRichTextLabel progressLabel = this._progressLabel;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 3);
			defaultInterpolatedStringHandler.AppendFormatted(formattedText);
			defaultInterpolatedStringHandler.AppendLiteral(" [blue]- ");
			defaultInterpolatedStringHandler.AppendFormatted(formattedText2);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral("[/blue]");
			progressLabel.Text = defaultInterpolatedStringHandler.ToStringAndClear();
			MegaRichTextLabel healthLabel = this._healthLabel;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 2);
			defaultInterpolatedStringHandler.AppendLiteral("[red]");
			defaultInterpolatedStringHandler.AppendFormatted<int>(serializablePlayer.CurrentHp);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(serializablePlayer.MaxHp);
			defaultInterpolatedStringHandler.AppendLiteral("[/red]");
			healthLabel.Text = defaultInterpolatedStringHandler.ToStringAndClear();
			MegaRichTextLabel goldLabel = this._goldLabel;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 1);
			defaultInterpolatedStringHandler.AppendLiteral("[gold]");
			defaultInterpolatedStringHandler.AppendFormatted<int>(serializablePlayer.Gold);
			defaultInterpolatedStringHandler.AppendLiteral("[/gold]");
			goldLabel.Text = defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x001251E7 File Offset: 0x001233E7
		private void ShowError()
		{
			this._runInfoContainer.Visible = false;
			this._errorContainer.Visible = true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060029F0 RID: 10736 RVA: 0x00125204 File Offset: 0x00123404
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NContinueRunInfo.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NContinueRunInfo.MethodName.AnimShow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NContinueRunInfo.MethodName.AnimHide, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NContinueRunInfo.MethodName.ShowError, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060029F1 RID: 10737 RVA: 0x001252D8 File Offset: 0x001234D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NContinueRunInfo.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NContinueRunInfo.MethodName.AnimShow && args.Count == 0)
			{
				this.AnimShow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NContinueRunInfo.MethodName.AnimHide && args.Count == 0)
			{
				this.AnimHide();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NContinueRunInfo.MethodName.ShowError && args.Count == 0)
			{
				this.ShowError();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060029F2 RID: 10738 RVA: 0x00125384 File Offset: 0x00123584
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NContinueRunInfo.MethodName._Ready || (ref method) == NContinueRunInfo.MethodName.AnimShow || (ref method) == NContinueRunInfo.MethodName.AnimHide || (ref method) == NContinueRunInfo.MethodName.ShowError || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060029F3 RID: 10739 RVA: 0x001253D4 File Offset: 0x001235D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NContinueRunInfo.PropertyName.HasResult)
			{
				this.HasResult = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._visTween)
			{
				this._visTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._initPosition)
			{
				this._initPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._runInfoContainer)
			{
				this._runInfoContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._errorContainer)
			{
				this._errorContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._dateLabel)
			{
				this._dateLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._goldLabel)
			{
				this._goldLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._healthLabel)
			{
				this._healthLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._progressLabel)
			{
				this._progressLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._ascensionLabel)
			{
				this._ascensionLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._charIcon)
			{
				this._charIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._isShown)
			{
				this._isShown = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060029F4 RID: 10740 RVA: 0x00125530 File Offset: 0x00123730
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NContinueRunInfo.PropertyName.HasResult)
			{
				bool hasResult = this.HasResult;
				value = VariantUtils.CreateFrom<bool>(ref hasResult);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._visTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._visTween);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._initPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._initPosition);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._runInfoContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._runInfoContainer);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._errorContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._errorContainer);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._dateLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._dateLabel);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._goldLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._goldLabel);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._healthLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._healthLabel);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._progressLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._progressLabel);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._ascensionLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._ascensionLabel);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._charIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._charIcon);
				return true;
			}
			if ((ref name) == NContinueRunInfo.PropertyName._isShown)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isShown);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060029F5 RID: 10741 RVA: 0x001256C8 File Offset: 0x001238C8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NContinueRunInfo.PropertyName._visTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NContinueRunInfo.PropertyName._initPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NContinueRunInfo.PropertyName._runInfoContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NContinueRunInfo.PropertyName._errorContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NContinueRunInfo.PropertyName._dateLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NContinueRunInfo.PropertyName._goldLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NContinueRunInfo.PropertyName._healthLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NContinueRunInfo.PropertyName._progressLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NContinueRunInfo.PropertyName._ascensionLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NContinueRunInfo.PropertyName._charIcon, 0L, "", 4096L, false),
				new PropertyInfo(1L, NContinueRunInfo.PropertyName._isShown, 0L, "", 4096L, false),
				new PropertyInfo(1L, NContinueRunInfo.PropertyName.HasResult, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060029F6 RID: 10742 RVA: 0x00125868 File Offset: 0x00123A68
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName hasResult = NContinueRunInfo.PropertyName.HasResult;
			bool hasResult2 = this.HasResult;
			info.AddProperty(hasResult, Variant.From<bool>(ref hasResult2));
			info.AddProperty(NContinueRunInfo.PropertyName._visTween, Variant.From<Tween>(ref this._visTween));
			info.AddProperty(NContinueRunInfo.PropertyName._initPosition, Variant.From<Vector2>(ref this._initPosition));
			info.AddProperty(NContinueRunInfo.PropertyName._runInfoContainer, Variant.From<Control>(ref this._runInfoContainer));
			info.AddProperty(NContinueRunInfo.PropertyName._errorContainer, Variant.From<Control>(ref this._errorContainer));
			info.AddProperty(NContinueRunInfo.PropertyName._dateLabel, Variant.From<MegaRichTextLabel>(ref this._dateLabel));
			info.AddProperty(NContinueRunInfo.PropertyName._goldLabel, Variant.From<MegaRichTextLabel>(ref this._goldLabel));
			info.AddProperty(NContinueRunInfo.PropertyName._healthLabel, Variant.From<MegaRichTextLabel>(ref this._healthLabel));
			info.AddProperty(NContinueRunInfo.PropertyName._progressLabel, Variant.From<MegaRichTextLabel>(ref this._progressLabel));
			info.AddProperty(NContinueRunInfo.PropertyName._ascensionLabel, Variant.From<MegaRichTextLabel>(ref this._ascensionLabel));
			info.AddProperty(NContinueRunInfo.PropertyName._charIcon, Variant.From<TextureRect>(ref this._charIcon));
			info.AddProperty(NContinueRunInfo.PropertyName._isShown, Variant.From<bool>(ref this._isShown));
		}

		/// <inheritdoc />
		// Token: 0x060029F7 RID: 10743 RVA: 0x00125988 File Offset: 0x00123B88
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName.HasResult, ref variant))
			{
				this.HasResult = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName._visTween, ref variant2))
			{
				this._visTween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName._initPosition, ref variant3))
			{
				this._initPosition = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName._runInfoContainer, ref variant4))
			{
				this._runInfoContainer = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName._errorContainer, ref variant5))
			{
				this._errorContainer = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName._dateLabel, ref variant6))
			{
				this._dateLabel = variant6.As<MegaRichTextLabel>();
			}
			Variant variant7;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName._goldLabel, ref variant7))
			{
				this._goldLabel = variant7.As<MegaRichTextLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName._healthLabel, ref variant8))
			{
				this._healthLabel = variant8.As<MegaRichTextLabel>();
			}
			Variant variant9;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName._progressLabel, ref variant9))
			{
				this._progressLabel = variant9.As<MegaRichTextLabel>();
			}
			Variant variant10;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName._ascensionLabel, ref variant10))
			{
				this._ascensionLabel = variant10.As<MegaRichTextLabel>();
			}
			Variant variant11;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName._charIcon, ref variant11))
			{
				this._charIcon = variant11.As<TextureRect>();
			}
			Variant variant12;
			if (info.TryGetProperty(NContinueRunInfo.PropertyName._isShown, ref variant12))
			{
				this._isShown = variant12.As<bool>();
			}
		}

		// Token: 0x04000FAD RID: 4013
		[Nullable(2)]
		private Tween _visTween;

		// Token: 0x04000FAE RID: 4014
		private Vector2 _initPosition;

		// Token: 0x04000FAF RID: 4015
		private Control _runInfoContainer;

		// Token: 0x04000FB0 RID: 4016
		private Control _errorContainer;

		// Token: 0x04000FB1 RID: 4017
		private MegaRichTextLabel _dateLabel;

		// Token: 0x04000FB2 RID: 4018
		private MegaRichTextLabel _goldLabel;

		// Token: 0x04000FB3 RID: 4019
		private MegaRichTextLabel _healthLabel;

		// Token: 0x04000FB4 RID: 4020
		private MegaRichTextLabel _progressLabel;

		// Token: 0x04000FB5 RID: 4021
		private MegaRichTextLabel _ascensionLabel;

		// Token: 0x04000FB6 RID: 4022
		private TextureRect _charIcon;

		// Token: 0x04000FB7 RID: 4023
		private bool _isShown;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014DA RID: 5338
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004F86 RID: 20358
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'AnimShow' method.
			/// </summary>
			// Token: 0x04004F87 RID: 20359
			public static readonly StringName AnimShow = "AnimShow";

			/// <summary>
			/// Cached name for the 'AnimHide' method.
			/// </summary>
			// Token: 0x04004F88 RID: 20360
			public static readonly StringName AnimHide = "AnimHide";

			/// <summary>
			/// Cached name for the 'ShowError' method.
			/// </summary>
			// Token: 0x04004F89 RID: 20361
			public static readonly StringName ShowError = "ShowError";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014DB RID: 5339
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'HasResult' property.
			/// </summary>
			// Token: 0x04004F8A RID: 20362
			public static readonly StringName HasResult = "HasResult";

			/// <summary>
			/// Cached name for the '_visTween' field.
			/// </summary>
			// Token: 0x04004F8B RID: 20363
			public static readonly StringName _visTween = "_visTween";

			/// <summary>
			/// Cached name for the '_initPosition' field.
			/// </summary>
			// Token: 0x04004F8C RID: 20364
			public static readonly StringName _initPosition = "_initPosition";

			/// <summary>
			/// Cached name for the '_runInfoContainer' field.
			/// </summary>
			// Token: 0x04004F8D RID: 20365
			public static readonly StringName _runInfoContainer = "_runInfoContainer";

			/// <summary>
			/// Cached name for the '_errorContainer' field.
			/// </summary>
			// Token: 0x04004F8E RID: 20366
			public static readonly StringName _errorContainer = "_errorContainer";

			/// <summary>
			/// Cached name for the '_dateLabel' field.
			/// </summary>
			// Token: 0x04004F8F RID: 20367
			public static readonly StringName _dateLabel = "_dateLabel";

			/// <summary>
			/// Cached name for the '_goldLabel' field.
			/// </summary>
			// Token: 0x04004F90 RID: 20368
			public static readonly StringName _goldLabel = "_goldLabel";

			/// <summary>
			/// Cached name for the '_healthLabel' field.
			/// </summary>
			// Token: 0x04004F91 RID: 20369
			public static readonly StringName _healthLabel = "_healthLabel";

			/// <summary>
			/// Cached name for the '_progressLabel' field.
			/// </summary>
			// Token: 0x04004F92 RID: 20370
			public static readonly StringName _progressLabel = "_progressLabel";

			/// <summary>
			/// Cached name for the '_ascensionLabel' field.
			/// </summary>
			// Token: 0x04004F93 RID: 20371
			public static readonly StringName _ascensionLabel = "_ascensionLabel";

			/// <summary>
			/// Cached name for the '_charIcon' field.
			/// </summary>
			// Token: 0x04004F94 RID: 20372
			public static readonly StringName _charIcon = "_charIcon";

			/// <summary>
			/// Cached name for the '_isShown' field.
			/// </summary>
			// Token: 0x04004F95 RID: 20373
			public static readonly StringName _isShown = "_isShown";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014DC RID: 5340
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
