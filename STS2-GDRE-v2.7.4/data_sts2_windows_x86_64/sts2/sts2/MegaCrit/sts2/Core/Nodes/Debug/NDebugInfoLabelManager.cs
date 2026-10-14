using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Nodes.Debug
{
	/// <summary>
	/// Manages label displays for debug info both in main menu and in run.
	/// </summary>
	// Token: 0x020003C6 RID: 966
	[ScriptPath("res://src/Core/Nodes/Debug/NDebugInfoLabelManager.cs")]
	public class NDebugInfoLabelManager : Node
	{
		// Token: 0x06003A68 RID: 14952 RVA: 0x001BC8AC File Offset: 0x001BAAAC
		public override void _Ready()
		{
			this._releaseInfo = base.GetNode<MegaLabel>("%ReleaseInfo");
			this._moddedWarning = base.GetNode<MegaLabel>("%ModdedWarning");
			this._seed = base.GetNodeOrNull<MegaLabel>("%DebugSeed");
			this._modWarningContainer = base.GetNodeOrNull<Control>("%ModWarningContainer");
			this._modWarningLabel = base.GetNodeOrNull<MegaRichTextLabel>("%ModWarningLabel");
			this._moddedWarning.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnModdedWarningHovered)), 0U);
			this._moddedWarning.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnModdedWarningUnhovered)), 0U);
			Control modWarningContainer = this._modWarningContainer;
			if (modWarningContainer != null)
			{
				modWarningContainer.SetVisible(false);
			}
			this.UpdateText(null);
			if (ReleaseInfoManager.Instance.ReleaseInfo == null)
			{
				TaskHelper.RunSafely(this.SetCommitIdInEditor());
			}
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x001BC9A0 File Offset: 0x001BABA0
		[NullableContext(1)]
		private async Task SetCommitIdInEditor()
		{
			if (GitHelper.ShortCommitIdTask != null)
			{
				string text = await GitHelper.ShortCommitIdTask;
				string text2 = text;
				this.UpdateText(text2);
			}
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x001BC9E4 File Offset: 0x001BABE4
		[NullableContext(2)]
		private void UpdateText(string commitId)
		{
			ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
			string text = ((releaseInfo != null) ? releaseInfo.Date.ToString("yyyy.MM.dd") : null) ?? "???";
			string text2 = ((releaseInfo != null) ? releaseInfo.Version : null) ?? commitId ?? "NONE";
			if (this.isMainMenu)
			{
				this._releaseInfo.Text = text2 + "\n" + text;
			}
			else
			{
				Label releaseInfo2 = this._releaseInfo;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				defaultInterpolatedStringHandler.AppendLiteral("] (");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				releaseInfo2.Text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			this._moddedWarning.Visible = ModManager.IsRunningModded();
			if (this._moddedWarning.Visible)
			{
				bool flag = ModManager.Mods.Any<Mod>(delegate(Mod m)
				{
					if (m.state != ModLoadState.Failed)
					{
						List<LocString> errors = m.errors;
						return errors != null && errors.Count > 0;
					}
					return true;
				});
				if (this.isMainMenu)
				{
					LocString locString = new LocString("main_menu_ui", "MODDED_WARNING");
					locString.Add("count", ModManager.GetLoadedMods().Count<Mod>());
					locString.Add("hasError", flag);
					this._moddedWarning.SetTextAutoSize(locString.GetFormattedText());
					LocString[] array = ModManager.Mods.SelectMany<Mod, LocString>((Mod m) => m.errors ?? new List<LocString>()).ToArray<LocString>();
					if (array.Length != 0)
					{
						this._modWarningLabel.Text = string.Join("\n", array.Select<LocString, string>((LocString s) => s.GetFormattedText()));
					}
					else
					{
						LocString locString2 = new LocString("main_menu_ui", "MOD_ERROR.NONE");
						locString2.Add("mods", string.Join(", ", ModManager.GetLoadedMods().Select<Mod, string>(delegate(Mod m)
						{
							ModManifest manifest = m.manifest;
							string text3;
							if ((text3 = ((manifest != null) ? manifest.name : null)) == null)
							{
								ModManifest manifest2 = m.manifest;
								if (manifest2 == null)
								{
									return null;
								}
								text3 = manifest2.id;
							}
							return text3;
						})));
						this._modWarningLabel.Text = locString2.GetFormattedText();
					}
				}
				else
				{
					MegaLabel moddedWarning = this._moddedWarning;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
					defaultInterpolatedStringHandler.AppendLiteral("MODDED (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(ModManager.GetLoadedMods().Count<Mod>());
					defaultInterpolatedStringHandler.AppendLiteral(")");
					moddedWarning.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (flag)
				{
					this._moddedWarning.Modulate = StsColors.redGlow;
				}
			}
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x001BCC7E File Offset: 0x001BAE7E
		private void OnModdedWarningHovered()
		{
			if (this._modWarningContainer != null)
			{
				this._modWarningContainer.Visible = true;
			}
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x001BCC94 File Offset: 0x001BAE94
		private void OnModdedWarningUnhovered()
		{
			if (this._modWarningContainer != null)
			{
				this._modWarningContainer.Visible = false;
			}
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x001BCCAC File Offset: 0x001BAEAC
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionReleased(DebugHotkey.hideVersionInfo, false))
			{
				this._releaseInfo.Visible = !this._releaseInfo.Visible;
				this._moddedWarning.Visible = ModManager.IsRunningModded() && !this._moddedWarning.Visible;
				MegaLabel seed = this._seed;
				if (seed != null)
				{
					seed.SetVisible(!this._seed.Visible);
				}
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(this._releaseInfo.Visible ? "Show Version Info" : "Hide Version Info"));
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A6E RID: 14958 RVA: 0x001BCD50 File Offset: 0x001BAF50
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NDebugInfoLabelManager.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDebugInfoLabelManager.MethodName.UpdateText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "commitId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDebugInfoLabelManager.MethodName.OnModdedWarningHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDebugInfoLabelManager.MethodName.OnModdedWarningUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDebugInfoLabelManager.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A6F RID: 14959 RVA: 0x001BCEA8 File Offset: 0x001BB0A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDebugInfoLabelManager.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugInfoLabelManager.MethodName.UpdateText && args.Count == 1)
			{
				this.UpdateText(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugInfoLabelManager.MethodName.OnModdedWarningHovered && args.Count == 0)
			{
				this.OnModdedWarningHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugInfoLabelManager.MethodName.OnModdedWarningUnhovered && args.Count == 0)
			{
				this.OnModdedWarningUnhovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDebugInfoLabelManager.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003A70 RID: 14960 RVA: 0x001BCF94 File Offset: 0x001BB194
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDebugInfoLabelManager.MethodName._Ready || (ref method) == NDebugInfoLabelManager.MethodName.UpdateText || (ref method) == NDebugInfoLabelManager.MethodName.OnModdedWarningHovered || (ref method) == NDebugInfoLabelManager.MethodName.OnModdedWarningUnhovered || (ref method) == NDebugInfoLabelManager.MethodName._Input || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003A71 RID: 14961 RVA: 0x001BCFF4 File Offset: 0x001BB1F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDebugInfoLabelManager.PropertyName.isMainMenu)
			{
				this.isMainMenu = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NDebugInfoLabelManager.PropertyName._releaseInfo)
			{
				this._releaseInfo = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDebugInfoLabelManager.PropertyName._moddedWarning)
			{
				this._moddedWarning = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDebugInfoLabelManager.PropertyName._seed)
			{
				this._seed = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDebugInfoLabelManager.PropertyName._modWarningContainer)
			{
				this._modWarningContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDebugInfoLabelManager.PropertyName._modWarningLabel)
			{
				this._modWarningLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003A72 RID: 14962 RVA: 0x001BD0AC File Offset: 0x001BB2AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDebugInfoLabelManager.PropertyName.isMainMenu)
			{
				value = VariantUtils.CreateFrom<bool>(ref this.isMainMenu);
				return true;
			}
			if ((ref name) == NDebugInfoLabelManager.PropertyName._releaseInfo)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._releaseInfo);
				return true;
			}
			if ((ref name) == NDebugInfoLabelManager.PropertyName._moddedWarning)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._moddedWarning);
				return true;
			}
			if ((ref name) == NDebugInfoLabelManager.PropertyName._seed)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._seed);
				return true;
			}
			if ((ref name) == NDebugInfoLabelManager.PropertyName._modWarningContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._modWarningContainer);
				return true;
			}
			if ((ref name) == NDebugInfoLabelManager.PropertyName._modWarningLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._modWarningLabel);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A73 RID: 14963 RVA: 0x001BD184 File Offset: 0x001BB384
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NDebugInfoLabelManager.PropertyName.isMainMenu, 0L, "", 4102L, true),
				new PropertyInfo(24L, NDebugInfoLabelManager.PropertyName._releaseInfo, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDebugInfoLabelManager.PropertyName._moddedWarning, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDebugInfoLabelManager.PropertyName._seed, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDebugInfoLabelManager.PropertyName._modWarningContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDebugInfoLabelManager.PropertyName._modWarningLabel, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A74 RID: 14964 RVA: 0x001BD260 File Offset: 0x001BB460
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDebugInfoLabelManager.PropertyName.isMainMenu, Variant.From<bool>(ref this.isMainMenu));
			info.AddProperty(NDebugInfoLabelManager.PropertyName._releaseInfo, Variant.From<MegaLabel>(ref this._releaseInfo));
			info.AddProperty(NDebugInfoLabelManager.PropertyName._moddedWarning, Variant.From<MegaLabel>(ref this._moddedWarning));
			info.AddProperty(NDebugInfoLabelManager.PropertyName._seed, Variant.From<MegaLabel>(ref this._seed));
			info.AddProperty(NDebugInfoLabelManager.PropertyName._modWarningContainer, Variant.From<Control>(ref this._modWarningContainer));
			info.AddProperty(NDebugInfoLabelManager.PropertyName._modWarningLabel, Variant.From<MegaRichTextLabel>(ref this._modWarningLabel));
		}

		/// <inheritdoc />
		// Token: 0x06003A75 RID: 14965 RVA: 0x001BD2F8 File Offset: 0x001BB4F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDebugInfoLabelManager.PropertyName.isMainMenu, ref variant))
			{
				this.isMainMenu = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDebugInfoLabelManager.PropertyName._releaseInfo, ref variant2))
			{
				this._releaseInfo = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDebugInfoLabelManager.PropertyName._moddedWarning, ref variant3))
			{
				this._moddedWarning = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDebugInfoLabelManager.PropertyName._seed, ref variant4))
			{
				this._seed = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDebugInfoLabelManager.PropertyName._modWarningContainer, ref variant5))
			{
				this._modWarningContainer = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDebugInfoLabelManager.PropertyName._modWarningLabel, ref variant6))
			{
				this._modWarningLabel = variant6.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x040016CF RID: 5839
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public bool isMainMenu;

		// Token: 0x040016D0 RID: 5840
		[Nullable(1)]
		private MegaLabel _releaseInfo;

		// Token: 0x040016D1 RID: 5841
		[Nullable(1)]
		private MegaLabel _moddedWarning;

		// Token: 0x040016D2 RID: 5842
		[Nullable(2)]
		private MegaLabel _seed;

		// Token: 0x040016D3 RID: 5843
		[Nullable(2)]
		private Control _modWarningContainer;

		// Token: 0x040016D4 RID: 5844
		[Nullable(2)]
		private MegaRichTextLabel _modWarningLabel;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017F7 RID: 6135
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005D8C RID: 23948
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'UpdateText' method.
			/// </summary>
			// Token: 0x04005D8D RID: 23949
			public static readonly StringName UpdateText = "UpdateText";

			/// <summary>
			/// Cached name for the 'OnModdedWarningHovered' method.
			/// </summary>
			// Token: 0x04005D8E RID: 23950
			public static readonly StringName OnModdedWarningHovered = "OnModdedWarningHovered";

			/// <summary>
			/// Cached name for the 'OnModdedWarningUnhovered' method.
			/// </summary>
			// Token: 0x04005D8F RID: 23951
			public static readonly StringName OnModdedWarningUnhovered = "OnModdedWarningUnhovered";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005D90 RID: 23952
			public static readonly StringName _Input = "_Input";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017F8 RID: 6136
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the 'isMainMenu' field.
			/// </summary>
			// Token: 0x04005D91 RID: 23953
			public static readonly StringName isMainMenu = "isMainMenu";

			/// <summary>
			/// Cached name for the '_releaseInfo' field.
			/// </summary>
			// Token: 0x04005D92 RID: 23954
			public static readonly StringName _releaseInfo = "_releaseInfo";

			/// <summary>
			/// Cached name for the '_moddedWarning' field.
			/// </summary>
			// Token: 0x04005D93 RID: 23955
			public static readonly StringName _moddedWarning = "_moddedWarning";

			/// <summary>
			/// Cached name for the '_seed' field.
			/// </summary>
			// Token: 0x04005D94 RID: 23956
			public static readonly StringName _seed = "_seed";

			/// <summary>
			/// Cached name for the '_modWarningContainer' field.
			/// </summary>
			// Token: 0x04005D95 RID: 23957
			public static readonly StringName _modWarningContainer = "_modWarningContainer";

			/// <summary>
			/// Cached name for the '_modWarningLabel' field.
			/// </summary>
			// Token: 0x04005D96 RID: 23958
			public static readonly StringName _modWarningLabel = "_modWarningLabel";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017F9 RID: 6137
		public class SignalName : Node.SignalName
		{
		}
	}
}
