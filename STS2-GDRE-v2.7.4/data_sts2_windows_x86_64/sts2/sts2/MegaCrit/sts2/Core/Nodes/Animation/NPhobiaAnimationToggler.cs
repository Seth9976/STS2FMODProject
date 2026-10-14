using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Animation
{
	/// <summary>
	/// Will start/stop the animation from an AnimationPlayer depending on if Phobia Mode is on
	/// Used in instances where the animation can make people uncomfortable (ie the Infection card overlay)
	/// </summary>
	// Token: 0x02000425 RID: 1061
	[ScriptPath("res://src/Core/Nodes/Animation/NPhobiaAnimationToggler.cs")]
	public class NPhobiaAnimationToggler : Node
	{
		// Token: 0x060043CC RID: 17356 RVA: 0x0020F1EA File Offset: 0x0020D3EA
		public override void _Ready()
		{
			base._Ready();
			this.UpdatePhobiaMode();
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x0020F1F8 File Offset: 0x0020D3F8
		public override void _EnterTree()
		{
			NGame instance = NGame.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Connect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)), 0U);
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x0020F221 File Offset: 0x0020D421
		public override void _ExitTree()
		{
			NGame instance = NGame.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Disconnect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)));
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x0020F248 File Offset: 0x0020D448
		private void UpdatePhobiaMode()
		{
			if (this._animationPlayer == null)
			{
				return;
			}
			this._animationPlayer.Active = !SaveManager.Instance.PrefsSave.PhobiaMode;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060043D0 RID: 17360 RVA: 0x0020F270 File Offset: 0x0020D470
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NPhobiaAnimationToggler.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPhobiaAnimationToggler.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPhobiaAnimationToggler.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPhobiaAnimationToggler.MethodName.UpdatePhobiaMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060043D1 RID: 17361 RVA: 0x0020F344 File Offset: 0x0020D544
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPhobiaAnimationToggler.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPhobiaAnimationToggler.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPhobiaAnimationToggler.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPhobiaAnimationToggler.MethodName.UpdatePhobiaMode && args.Count == 0)
			{
				this.UpdatePhobiaMode();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060043D2 RID: 17362 RVA: 0x0020F3F0 File Offset: 0x0020D5F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPhobiaAnimationToggler.MethodName._Ready || (ref method) == NPhobiaAnimationToggler.MethodName._EnterTree || (ref method) == NPhobiaAnimationToggler.MethodName._ExitTree || (ref method) == NPhobiaAnimationToggler.MethodName.UpdatePhobiaMode || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060043D3 RID: 17363 RVA: 0x0020F440 File Offset: 0x0020D640
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPhobiaAnimationToggler.PropertyName._animationPlayer)
			{
				this._animationPlayer = VariantUtils.ConvertTo<AnimationPlayer>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060043D4 RID: 17364 RVA: 0x0020F465 File Offset: 0x0020D665
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPhobiaAnimationToggler.PropertyName._animationPlayer)
			{
				value = VariantUtils.CreateFrom<AnimationPlayer>(ref this._animationPlayer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060043D5 RID: 17365 RVA: 0x0020F490 File Offset: 0x0020D690
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPhobiaAnimationToggler.PropertyName._animationPlayer, 34L, "AnimationPlayer", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x060043D6 RID: 17366 RVA: 0x0020F4C6 File Offset: 0x0020D6C6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPhobiaAnimationToggler.PropertyName._animationPlayer, Variant.From<AnimationPlayer>(ref this._animationPlayer));
		}

		/// <inheritdoc />
		// Token: 0x060043D7 RID: 17367 RVA: 0x0020F4E8 File Offset: 0x0020D6E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPhobiaAnimationToggler.PropertyName._animationPlayer, ref variant))
			{
				this._animationPlayer = variant.As<AnimationPlayer>();
			}
		}

		// Token: 0x04001A5C RID: 6748
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private AnimationPlayer _animationPlayer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001985 RID: 6533
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040064E2 RID: 25826
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040064E3 RID: 25827
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040064E4 RID: 25828
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdatePhobiaMode' method.
			/// </summary>
			// Token: 0x040064E5 RID: 25829
			public static readonly StringName UpdatePhobiaMode = "UpdatePhobiaMode";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001986 RID: 6534
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_animationPlayer' field.
			/// </summary>
			// Token: 0x040064E6 RID: 25830
			public static readonly StringName _animationPlayer = "_animationPlayer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001987 RID: 6535
		public class SignalName : Node.SignalName
		{
		}
	}
}
