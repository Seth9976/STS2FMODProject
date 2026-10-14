using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;

namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen
{
	/// <summary>
	/// Opens the feedback screen from anywhere when a hotkey is pressed.
	/// </summary>
	// Token: 0x02000323 RID: 803
	[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NFeedbackScreenOpener.cs")]
	public class NFeedbackScreenOpener : Node
	{
		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002CCC RID: 11468 RVA: 0x001400D4 File Offset: 0x0013E2D4
		// (set) Token: 0x06002CCD RID: 11469 RVA: 0x001400DB File Offset: 0x0013E2DB
		[Nullable(1)]
		public static NFeedbackScreenOpener Instance
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			private set;
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x001400E3 File Offset: 0x0013E2E3
		public override void _EnterTree()
		{
			NFeedbackScreenOpener.Instance = this;
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x001400EB File Offset: 0x0013E2EB
		public override void _ExitTree()
		{
			NFeedbackScreenOpener.Instance = null;
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x001400F4 File Offset: 0x0013E2F4
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			InputEventKey inputEventKey = inputEvent as InputEventKey;
			if (inputEventKey == null)
			{
				return;
			}
			if (!inputEventKey.Pressed)
			{
				return;
			}
			if (inputEventKey.Keycode != 4194333L)
			{
				return;
			}
			if (NGame.Instance.GetOrCreateFeedbackScreen().Visible)
			{
				return;
			}
			NCapstoneContainer instance = NCapstoneContainer.Instance;
			NCapstoneSubmenuStack ncapstoneSubmenuStack = ((instance != null) ? instance.CurrentCapstoneScreen : null) as NCapstoneSubmenuStack;
			if (ncapstoneSubmenuStack != null && ncapstoneSubmenuStack.ScreenType == NetScreenType.Feedback)
			{
				return;
			}
			TaskHelper.RunSafely(this.OpenFeedbackScreen());
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x00140168 File Offset: 0x0013E368
		[NullableContext(1)]
		public async Task OpenFeedbackScreen()
		{
			Image screenshot = base.GetViewport().GetTexture().GetImage();
			await this.AwaitProcessFrame(default(CancellationToken));
			NGame.Instance.GetInspectCardScreen().Close();
			NGame.Instance.GetInspectRelicScreen().Close();
			NSendFeedbackScreen orCreateFeedbackScreen = NGame.Instance.GetOrCreateFeedbackScreen();
			orCreateFeedbackScreen.SetScreenshot(screenshot);
			orCreateFeedbackScreen.Open();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CD2 RID: 11474 RVA: 0x001401AC File Offset: 0x0013E3AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NFeedbackScreenOpener.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFeedbackScreenOpener.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFeedbackScreenOpener.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CD3 RID: 11475 RVA: 0x00140280 File Offset: 0x0013E480
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFeedbackScreenOpener.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFeedbackScreenOpener.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFeedbackScreenOpener.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002CD4 RID: 11476 RVA: 0x00140313 File Offset: 0x0013E513
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFeedbackScreenOpener.MethodName._EnterTree || (ref method) == NFeedbackScreenOpener.MethodName._ExitTree || (ref method) == NFeedbackScreenOpener.MethodName._Input || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002CD5 RID: 11477 RVA: 0x00140349 File Offset: 0x0013E549
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002CD6 RID: 11478 RVA: 0x00140352 File Offset: 0x0013E552
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001571 RID: 5489
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x0400522C RID: 21036
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400522D RID: 21037
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x0400522E RID: 21038
			public static readonly StringName _Input = "_Input";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001572 RID: 5490
		public class PropertyName : Node.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001573 RID: 5491
		public class SignalName : Node.SignalName
		{
		}
	}
}
