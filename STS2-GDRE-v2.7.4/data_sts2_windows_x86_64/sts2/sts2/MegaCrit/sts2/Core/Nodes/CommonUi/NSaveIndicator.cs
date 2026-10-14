using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	// Token: 0x020003EA RID: 1002
	[ScriptPath("res://src/Core/Nodes/CommonUi/NSaveIndicator.cs")]
	public class NSaveIndicator : Control
	{
		// Token: 0x06003D21 RID: 15649 RVA: 0x001D4DCD File Offset: 0x001D2FCD
		public override void _Ready()
		{
			base.GetNode<MegaRichTextLabel>("Label").SetTextAutoSize(new LocString("gameplay_ui", "GAME_SAVED").GetFormattedText());
			base.Modulate = Colors.Transparent;
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x001D4E04 File Offset: 0x001D3004
		private void SavedGame()
		{
			if (NCombatUi.IsDebugHideTextVfx)
			{
				return;
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenInterval(0.5);
			this._tween.TweenProperty(this, "modulate", Colors.White, 1.0);
			this._tween.TweenInterval(0.5);
			this._tween.TweenProperty(this, "modulate", Colors.Transparent, 1.0);
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x001D4EB4 File Offset: 0x001D30B4
		public override void _EnterTree()
		{
			SaveManager.Instance.Saved += this.SavedGame;
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x001D4ECC File Offset: 0x001D30CC
		public override void _ExitTree()
		{
			SaveManager.Instance.Saved -= this.SavedGame;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D25 RID: 15653 RVA: 0x001D4EE4 File Offset: 0x001D30E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NSaveIndicator.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSaveIndicator.MethodName.SavedGame, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSaveIndicator.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSaveIndicator.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D26 RID: 15654 RVA: 0x001D4FB8 File Offset: 0x001D31B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSaveIndicator.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSaveIndicator.MethodName.SavedGame && args.Count == 0)
			{
				this.SavedGame();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSaveIndicator.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSaveIndicator.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003D27 RID: 15655 RVA: 0x001D5064 File Offset: 0x001D3264
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSaveIndicator.MethodName._Ready || (ref method) == NSaveIndicator.MethodName.SavedGame || (ref method) == NSaveIndicator.MethodName._EnterTree || (ref method) == NSaveIndicator.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003D28 RID: 15656 RVA: 0x001D50B4 File Offset: 0x001D32B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSaveIndicator.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003D29 RID: 15657 RVA: 0x001D50D9 File Offset: 0x001D32D9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSaveIndicator.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D2A RID: 15658 RVA: 0x001D5104 File Offset: 0x001D3304
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSaveIndicator.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D2B RID: 15659 RVA: 0x001D5139 File Offset: 0x001D3339
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSaveIndicator.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06003D2C RID: 15660 RVA: 0x001D5158 File Offset: 0x001D3358
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSaveIndicator.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
		}

		// Token: 0x040017E0 RID: 6112
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200187E RID: 6270
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005FCD RID: 24525
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SavedGame' method.
			/// </summary>
			// Token: 0x04005FCE RID: 24526
			public static readonly StringName SavedGame = "SavedGame";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005FCF RID: 24527
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005FD0 RID: 24528
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200187F RID: 6271
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005FD1 RID: 24529
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001880 RID: 6272
		public class SignalName : Control.SignalName
		{
		}
	}
}
