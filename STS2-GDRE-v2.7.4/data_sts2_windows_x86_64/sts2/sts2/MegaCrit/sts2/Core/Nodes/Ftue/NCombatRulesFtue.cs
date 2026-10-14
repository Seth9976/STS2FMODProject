using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	// Token: 0x020003AC RID: 940
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Ftue/NCombatRulesFtue.cs")]
	public class NCombatRulesFtue : NFtue
	{
		// Token: 0x06003896 RID: 14486 RVA: 0x001AB268 File Offset: 0x001A9468
		public override void _Ready()
		{
			this._image = base.GetNode<TextureRect>("Image");
			this._bodyText = base.GetNode<MegaRichTextLabel>("%Description");
			this._pageCount = base.GetNode<MegaLabel>("PageCount");
			this._header = base.GetNode<MegaLabel>("Header");
			this._prevButton = base.GetNode<NButton>("LeftArrow");
			this._nextButton = base.GetNode<NButton>("RightArrow");
			this._image.Modulate = Colors.Transparent;
			this._bodyText.Modulate = Colors.Transparent;
			this._prevButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ToggleLeft)), 0U);
			this._nextButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ToggleRight)), 0U);
			this._prevButton.Visible = false;
			this._prevButton.Disable();
			this._nextButton.Visible = false;
			this._nextButton.Disable();
			this._pageCount.Visible = false;
			this._header.Visible = false;
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x001AB3A8 File Offset: 0x001A95A8
		public void Start()
		{
			NModalContainer instance = NModalContainer.Instance;
			if (instance != null)
			{
				instance.ShowBackstop();
			}
			this._currentPage = 1;
			this._imagePosition = this._image.Position;
			this._bodyText.Text = new LocString("ftues", "TUTORIAL_FTUE_BODY_1").GetFormattedText();
			this._bodyText.Modulate = StsColors.transparentWhite;
			this._textPosition = this._bodyText.Position;
			LocString locString = new LocString("ftues", "COMBAT_BASICS_FTUE_PAGE_COUNT");
			locString.Add("totalPages", 3m);
			locString.Add("currentPage", this._currentPage);
			this._pageCount.SetTextAutoSize(locString.GetFormattedText());
			this._header.SetTextAutoSize(new LocString("ftues", "COMBAT_BASICS_FTUE_HEADER").GetFormattedText());
			this._nextButton.Visible = true;
			this._nextButton.Enable();
			this._pageCount.Visible = true;
			this._header.Visible = true;
			this._pageTurnTween = base.CreateTween().SetParallel(true);
			this._pageTurnTween.TweenProperty(this._image, "modulate:a", 1f, 0.5).SetEase(1L).SetTrans(7L)
				.From(0f);
			this._pageTurnTween.TweenProperty(this._bodyText, "modulate:a", 1f, 1.0).SetEase(1L).SetTrans(0L);
			this._pageTurnTween.TweenProperty(this._bodyText, "visible_ratio", 1f, 0.6).SetEase(1L).SetTrans(1L)
				.From(0f);
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x001AB59A File Offset: 0x001A979A
		[NullableContext(2)]
		public static NCombatRulesFtue Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCombatRulesFtue._scenePath).Instantiate<NCombatRulesFtue>(0L);
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x001AB5BC File Offset: 0x001A97BC
		public override void _Input(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (NDevConsole.IsConsoleVisible)
			{
				return;
			}
			Control control = base.GetViewport().GuiGetFocusOwner();
			bool flag = control is TextEdit || control is LineEdit;
			if (flag)
			{
				return;
			}
			if (inputEvent.IsActionPressed(MegaInput.left, false, false) && this._prevButton.IsEnabled)
			{
				this.ToggleLeft(this._prevButton);
			}
			if (inputEvent.IsActionPressed(MegaInput.right, false, false) && this._nextButton.IsEnabled)
			{
				this.ToggleRight(this._nextButton);
			}
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x001AB650 File Offset: 0x001A9850
		private void ToggleLeft(NButton _)
		{
			this._currentPage--;
			int currentPage = this._currentPage;
			if (currentPage != 1)
			{
				if (currentPage == 2)
				{
					this._bodyText.SetTextAutoSize(new LocString("ftues", "TUTORIAL_FTUE_BODY_2").GetFormattedText());
					this._image.Texture = this._image2;
				}
			}
			else
			{
				this._prevButton.Visible = false;
				this._prevButton.Disable();
				this._bodyText.SetTextAutoSize(new LocString("ftues", "TUTORIAL_FTUE_BODY_1").GetFormattedText());
				this._image.Texture = this._image1;
			}
			LocString locString = new LocString("ftues", "COMBAT_BASICS_FTUE_PAGE_COUNT");
			locString.Add("totalPages", 3m);
			locString.Add("currentPage", this._currentPage);
			this._pageCount.SetTextAutoSize(locString.GetFormattedText());
			Tween pageTurnTween = this._pageTurnTween;
			if (pageTurnTween != null)
			{
				pageTurnTween.Kill();
			}
			this._pageTurnTween = base.CreateTween().SetParallel(true);
			this._pageTurnTween.TweenProperty(this._image, "modulate:a", 1f, 0.5).SetEase(1L).SetTrans(7L)
				.From(0.5f);
			this._pageTurnTween.TweenProperty(this._image, "position", this._imagePosition, 0.5).SetEase(1L).SetTrans(5L)
				.From(this._imagePosition - NCombatRulesFtue._imageAnimOffset);
			this._pageTurnTween.TweenProperty(this._bodyText, "position", this._textPosition, 0.5).SetEase(1L).SetTrans(5L)
				.From(this._textPosition - NCombatRulesFtue._imageAnimOffset);
			this._pageTurnTween.TweenProperty(this._bodyText, "modulate:a", 1f, 0.6).SetEase(1L).SetTrans(0L)
				.From(0f);
			this._pageTurnTween.TweenProperty(this._bodyText, "visible_ratio", 1f, 0.6).SetEase(1L).SetTrans(1L)
				.From(0f);
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x001AB8F4 File Offset: 0x001A9AF4
		private void ToggleRight(NButton _)
		{
			if (this._currentPage == 3)
			{
				Tween pageTurnTween = this._pageTurnTween;
				if (pageTurnTween != null)
				{
					pageTurnTween.Kill();
				}
				SaveManager.Instance.MarkFtueAsComplete("combat_rules_ftue");
				NCombatRoom.Instance.AddChildSafely(NCombatStartBanner.Create());
				base.CloseFtue();
				return;
			}
			this._currentPage++;
			int currentPage = this._currentPage;
			if (currentPage != 2)
			{
				if (currentPage == 3)
				{
					this._bodyText.SetTextAutoSize(new LocString("ftues", "TUTORIAL_FTUE_BODY_3").GetFormattedText());
					this._image.Texture = this._image3;
				}
			}
			else
			{
				this._prevButton.Visible = true;
				this._prevButton.Enable();
				this._bodyText.SetTextAutoSize(new LocString("ftues", "TUTORIAL_FTUE_BODY_2").GetFormattedText());
				this._image.Texture = this._image2;
			}
			LocString locString = new LocString("ftues", "COMBAT_BASICS_FTUE_PAGE_COUNT");
			locString.Add("totalPages", 3m);
			locString.Add("currentPage", this._currentPage);
			this._pageCount.SetTextAutoSize(locString.GetFormattedText());
			Tween pageTurnTween2 = this._pageTurnTween;
			if (pageTurnTween2 != null)
			{
				pageTurnTween2.Kill();
			}
			this._pageTurnTween = base.CreateTween().SetParallel(true);
			this._pageTurnTween.TweenProperty(this._image, "modulate:a", 1f, 0.5).SetEase(1L).SetTrans(7L)
				.From(0.5f);
			this._pageTurnTween.TweenProperty(this._image, "position", this._imagePosition, 0.5).SetEase(1L).SetTrans(5L)
				.From(this._imagePosition + NCombatRulesFtue._imageAnimOffset);
			this._pageTurnTween.TweenProperty(this._bodyText, "position", this._textPosition, 0.5).SetEase(1L).SetTrans(5L)
				.From(this._textPosition + NCombatRulesFtue._imageAnimOffset);
			this._pageTurnTween.TweenProperty(this._bodyText, "modulate:a", 1f, 0.6).SetEase(1L).SetTrans(0L)
				.From(0f);
			this._pageTurnTween.TweenProperty(this._bodyText, "visible_ratio", 1f, 0.6).SetEase(1L).SetTrans(1L)
				.From(0f);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600389C RID: 14492 RVA: 0x001ABBD8 File Offset: 0x001A9DD8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NCombatRulesFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRulesFtue.MethodName.Start, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatRulesFtue.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NCombatRulesFtue.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCombatRulesFtue.MethodName.ToggleLeft, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCombatRulesFtue.MethodName.ToggleRight, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600389D RID: 14493 RVA: 0x001ABDA4 File Offset: 0x001A9FA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatRulesFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRulesFtue.MethodName.Start && args.Count == 0)
			{
				this.Start();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRulesFtue.MethodName.Create && args.Count == 0)
			{
				NCombatRulesFtue ncombatRulesFtue = NCombatRulesFtue.Create();
				ret = VariantUtils.CreateFrom<NCombatRulesFtue>(ref ncombatRulesFtue);
				return true;
			}
			if ((ref method) == NCombatRulesFtue.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRulesFtue.MethodName.ToggleLeft && args.Count == 1)
			{
				this.ToggleLeft(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatRulesFtue.MethodName.ToggleRight && args.Count == 1)
			{
				this.ToggleRight(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x001ABEC8 File Offset: 0x001AA0C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatRulesFtue.MethodName.Create && args.Count == 0)
			{
				NCombatRulesFtue ncombatRulesFtue = NCombatRulesFtue.Create();
				ret = VariantUtils.CreateFrom<NCombatRulesFtue>(ref ncombatRulesFtue);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600389F RID: 14495 RVA: 0x001ABF08 File Offset: 0x001AA108
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatRulesFtue.MethodName._Ready || (ref method) == NCombatRulesFtue.MethodName.Start || (ref method) == NCombatRulesFtue.MethodName.Create || (ref method) == NCombatRulesFtue.MethodName._Input || (ref method) == NCombatRulesFtue.MethodName.ToggleLeft || (ref method) == NCombatRulesFtue.MethodName.ToggleRight || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060038A0 RID: 14496 RVA: 0x001ABF78 File Offset: 0x001AA178
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatRulesFtue.PropertyName._image1)
			{
				this._image1 = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._image2)
			{
				this._image2 = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._image3)
			{
				this._image3 = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._prevButton)
			{
				this._prevButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._nextButton)
			{
				this._nextButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._pageCount)
			{
				this._pageCount = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._bodyText)
			{
				this._bodyText = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._header)
			{
				this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._currentPage)
			{
				this._currentPage = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._imagePosition)
			{
				this._imagePosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._textPosition)
			{
				this._textPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._pageTurnTween)
			{
				this._pageTurnTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060038A1 RID: 14497 RVA: 0x001AC0EC File Offset: 0x001AA2EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatRulesFtue.PropertyName._image1)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._image1);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._image2)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._image2);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._image3)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._image3);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._prevButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._prevButton);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._nextButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._nextButton);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._pageCount)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._pageCount);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._bodyText)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._bodyText);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._header)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._currentPage)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentPage);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._imagePosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._imagePosition);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._textPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._textPosition);
				return true;
			}
			if ((ref name) == NCombatRulesFtue.PropertyName._pageTurnTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._pageTurnTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060038A2 RID: 14498 RVA: 0x001AC2A4 File Offset: 0x001AA4A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCombatRulesFtue.PropertyName._image1, 17L, "Texture2D", 4102L, true),
				new PropertyInfo(24L, NCombatRulesFtue.PropertyName._image2, 17L, "Texture2D", 4102L, true),
				new PropertyInfo(24L, NCombatRulesFtue.PropertyName._image3, 17L, "Texture2D", 4102L, true),
				new PropertyInfo(24L, NCombatRulesFtue.PropertyName._prevButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRulesFtue.PropertyName._nextButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRulesFtue.PropertyName._pageCount, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRulesFtue.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRulesFtue.PropertyName._bodyText, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRulesFtue.PropertyName._header, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCombatRulesFtue.PropertyName._currentPage, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCombatRulesFtue.PropertyName._imagePosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCombatRulesFtue.PropertyName._textPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatRulesFtue.PropertyName._pageTurnTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060038A3 RID: 14499 RVA: 0x001AC468 File Offset: 0x001AA668
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCombatRulesFtue.PropertyName._image1, Variant.From<Texture2D>(ref this._image1));
			info.AddProperty(NCombatRulesFtue.PropertyName._image2, Variant.From<Texture2D>(ref this._image2));
			info.AddProperty(NCombatRulesFtue.PropertyName._image3, Variant.From<Texture2D>(ref this._image3));
			info.AddProperty(NCombatRulesFtue.PropertyName._prevButton, Variant.From<NButton>(ref this._prevButton));
			info.AddProperty(NCombatRulesFtue.PropertyName._nextButton, Variant.From<NButton>(ref this._nextButton));
			info.AddProperty(NCombatRulesFtue.PropertyName._pageCount, Variant.From<MegaLabel>(ref this._pageCount));
			info.AddProperty(NCombatRulesFtue.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NCombatRulesFtue.PropertyName._bodyText, Variant.From<MegaRichTextLabel>(ref this._bodyText));
			info.AddProperty(NCombatRulesFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
			info.AddProperty(NCombatRulesFtue.PropertyName._currentPage, Variant.From<int>(ref this._currentPage));
			info.AddProperty(NCombatRulesFtue.PropertyName._imagePosition, Variant.From<Vector2>(ref this._imagePosition));
			info.AddProperty(NCombatRulesFtue.PropertyName._textPosition, Variant.From<Vector2>(ref this._textPosition));
			info.AddProperty(NCombatRulesFtue.PropertyName._pageTurnTween, Variant.From<Tween>(ref this._pageTurnTween));
		}

		/// <inheritdoc />
		// Token: 0x060038A4 RID: 14500 RVA: 0x001AC59C File Offset: 0x001AA79C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._image1, ref variant))
			{
				this._image1 = variant.As<Texture2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._image2, ref variant2))
			{
				this._image2 = variant2.As<Texture2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._image3, ref variant3))
			{
				this._image3 = variant3.As<Texture2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._prevButton, ref variant4))
			{
				this._prevButton = variant4.As<NButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._nextButton, ref variant5))
			{
				this._nextButton = variant5.As<NButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._pageCount, ref variant6))
			{
				this._pageCount = variant6.As<MegaLabel>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._image, ref variant7))
			{
				this._image = variant7.As<TextureRect>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._bodyText, ref variant8))
			{
				this._bodyText = variant8.As<MegaRichTextLabel>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._header, ref variant9))
			{
				this._header = variant9.As<MegaLabel>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._currentPage, ref variant10))
			{
				this._currentPage = variant10.As<int>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._imagePosition, ref variant11))
			{
				this._imagePosition = variant11.As<Vector2>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._textPosition, ref variant12))
			{
				this._textPosition = variant12.As<Vector2>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCombatRulesFtue.PropertyName._pageTurnTween, ref variant13))
			{
				this._pageTurnTween = variant13.As<Tween>();
			}
		}

		// Token: 0x040015E9 RID: 5609
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Texture2D _image1;

		// Token: 0x040015EA RID: 5610
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Texture2D _image2;

		// Token: 0x040015EB RID: 5611
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Texture2D _image3;

		// Token: 0x040015EC RID: 5612
		public const string id = "combat_rules_ftue";

		// Token: 0x040015ED RID: 5613
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/combat_rules_ftue");

		// Token: 0x040015EE RID: 5614
		private NButton _prevButton;

		// Token: 0x040015EF RID: 5615
		private NButton _nextButton;

		// Token: 0x040015F0 RID: 5616
		private MegaLabel _pageCount;

		// Token: 0x040015F1 RID: 5617
		private TextureRect _image;

		// Token: 0x040015F2 RID: 5618
		private MegaRichTextLabel _bodyText;

		// Token: 0x040015F3 RID: 5619
		private MegaLabel _header;

		// Token: 0x040015F4 RID: 5620
		private int _currentPage = 1;

		// Token: 0x040015F5 RID: 5621
		private const int _totalPages = 3;

		// Token: 0x040015F6 RID: 5622
		private Vector2 _imagePosition;

		// Token: 0x040015F7 RID: 5623
		private Vector2 _textPosition;

		// Token: 0x040015F8 RID: 5624
		[Nullable(2)]
		private Tween _pageTurnTween;

		// Token: 0x040015F9 RID: 5625
		private const double _textTweenSpeed = 0.6;

		// Token: 0x040015FA RID: 5626
		private static readonly Vector2 _imageAnimOffset = new Vector2(200f, 0f);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200179F RID: 6047
		[NullableContext(0)]
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C10 RID: 23568
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Start' method.
			/// </summary>
			// Token: 0x04005C11 RID: 23569
			public static readonly StringName Start = "Start";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005C12 RID: 23570
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005C13 RID: 23571
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'ToggleLeft' method.
			/// </summary>
			// Token: 0x04005C14 RID: 23572
			public static readonly StringName ToggleLeft = "ToggleLeft";

			/// <summary>
			/// Cached name for the 'ToggleRight' method.
			/// </summary>
			// Token: 0x04005C15 RID: 23573
			public static readonly StringName ToggleRight = "ToggleRight";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017A0 RID: 6048
		[NullableContext(0)]
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_image1' field.
			/// </summary>
			// Token: 0x04005C16 RID: 23574
			public static readonly StringName _image1 = "_image1";

			/// <summary>
			/// Cached name for the '_image2' field.
			/// </summary>
			// Token: 0x04005C17 RID: 23575
			public static readonly StringName _image2 = "_image2";

			/// <summary>
			/// Cached name for the '_image3' field.
			/// </summary>
			// Token: 0x04005C18 RID: 23576
			public static readonly StringName _image3 = "_image3";

			/// <summary>
			/// Cached name for the '_prevButton' field.
			/// </summary>
			// Token: 0x04005C19 RID: 23577
			public static readonly StringName _prevButton = "_prevButton";

			/// <summary>
			/// Cached name for the '_nextButton' field.
			/// </summary>
			// Token: 0x04005C1A RID: 23578
			public static readonly StringName _nextButton = "_nextButton";

			/// <summary>
			/// Cached name for the '_pageCount' field.
			/// </summary>
			// Token: 0x04005C1B RID: 23579
			public static readonly StringName _pageCount = "_pageCount";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04005C1C RID: 23580
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_bodyText' field.
			/// </summary>
			// Token: 0x04005C1D RID: 23581
			public static readonly StringName _bodyText = "_bodyText";

			/// <summary>
			/// Cached name for the '_header' field.
			/// </summary>
			// Token: 0x04005C1E RID: 23582
			public static readonly StringName _header = "_header";

			/// <summary>
			/// Cached name for the '_currentPage' field.
			/// </summary>
			// Token: 0x04005C1F RID: 23583
			public static readonly StringName _currentPage = "_currentPage";

			/// <summary>
			/// Cached name for the '_imagePosition' field.
			/// </summary>
			// Token: 0x04005C20 RID: 23584
			public static readonly StringName _imagePosition = "_imagePosition";

			/// <summary>
			/// Cached name for the '_textPosition' field.
			/// </summary>
			// Token: 0x04005C21 RID: 23585
			public static readonly StringName _textPosition = "_textPosition";

			/// <summary>
			/// Cached name for the '_pageTurnTween' field.
			/// </summary>
			// Token: 0x04005C22 RID: 23586
			public static readonly StringName _pageTurnTween = "_pageTurnTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017A1 RID: 6049
		[NullableContext(0)]
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}
