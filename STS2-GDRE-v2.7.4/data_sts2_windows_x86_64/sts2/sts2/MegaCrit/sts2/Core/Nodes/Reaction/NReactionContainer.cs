using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game;

namespace MegaCrit.Sts2.Core.Nodes.Reaction
{
	// Token: 0x0200037B RID: 891
	[ScriptPath("res://src/Core/Nodes/Reaction/NReactionContainer.cs")]
	public class NReactionContainer : Control
	{
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x060034C9 RID: 13513 RVA: 0x001890CF File Offset: 0x001872CF
		public bool InMultiplayer
		{
			get
			{
				return this._synchronizer != null && this._synchronizer.NetService.Type != NetGameType.Singleplayer;
			}
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x001890F1 File Offset: 0x001872F1
		[NullableContext(1)]
		public void InitializeNetworking(INetGameService netService)
		{
			if (this._synchronizer != null)
			{
				this.DeinitializeNetworking();
			}
			this._synchronizer = new ReactionSynchronizer(netService, this);
			this._synchronizer.NetService.Disconnected += this.NetServiceDisconnected;
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x0018912A File Offset: 0x0018732A
		private void NetServiceDisconnected(NetErrorInfo _)
		{
			this.DeinitializeNetworking();
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x00189132 File Offset: 0x00187332
		public void DeinitializeNetworking()
		{
			if (this._synchronizer != null)
			{
				this._synchronizer.NetService.Disconnected -= this.NetServiceDisconnected;
				this._synchronizer.Dispose();
				this._synchronizer = null;
			}
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x0018916A File Offset: 0x0018736A
		public override void _ExitTree()
		{
			this.DeinitializeNetworking();
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x00189174 File Offset: 0x00187374
		[NullableContext(1)]
		public void DoLocalReaction(Texture2D tex, Vector2 position)
		{
			NReaction nreaction = NReaction.Create(tex);
			this.AddChildSafely(nreaction);
			nreaction.GlobalPosition = position - nreaction.Size / 2f;
			nreaction.BeginAnim();
			ReactionSynchronizer synchronizer = this._synchronizer;
			if (synchronizer == null)
			{
				return;
			}
			synchronizer.SendLocalReaction(nreaction.Type, position);
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x001891C8 File Offset: 0x001873C8
		public void DoRemoteReaction(ReactionType type, Vector2 position)
		{
			NReaction nreaction = NReaction.Create(type);
			this.AddChildSafely(nreaction);
			nreaction.GlobalPosition = position - nreaction.Size / 2f;
			nreaction.BeginAnim();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060034D0 RID: 13520 RVA: 0x00189208 File Offset: 0x00187408
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NReactionContainer.MethodName.DeinitializeNetworking, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReactionContainer.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReactionContainer.MethodName.DoLocalReaction, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tex", 0L, "", 6L, new StringName("Texture2D"), false),
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NReactionContainer.MethodName.DoRemoteReaction, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "type", 0L, "", 6L, false),
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060034D1 RID: 13521 RVA: 0x00189370 File Offset: 0x00187570
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NReactionContainer.MethodName.DeinitializeNetworking && args.Count == 0)
			{
				this.DeinitializeNetworking();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionContainer.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionContainer.MethodName.DoLocalReaction && args.Count == 2)
			{
				this.DoLocalReaction(VariantUtils.ConvertTo<Texture2D>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReactionContainer.MethodName.DoRemoteReaction && args.Count == 2)
			{
				this.DoRemoteReaction(VariantUtils.ConvertTo<ReactionType>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060034D2 RID: 13522 RVA: 0x00189450 File Offset: 0x00187650
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NReactionContainer.MethodName.DeinitializeNetworking || (ref method) == NReactionContainer.MethodName._ExitTree || (ref method) == NReactionContainer.MethodName.DoLocalReaction || (ref method) == NReactionContainer.MethodName.DoRemoteReaction || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060034D3 RID: 13523 RVA: 0x001894A0 File Offset: 0x001876A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NReactionContainer.PropertyName.InMultiplayer)
			{
				bool inMultiplayer = this.InMultiplayer;
				value = VariantUtils.CreateFrom<bool>(ref inMultiplayer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060034D4 RID: 13524 RVA: 0x001894D8 File Offset: 0x001876D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NReactionContainer.PropertyName.InMultiplayer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060034D5 RID: 13525 RVA: 0x0018950C File Offset: 0x0018770C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060034D6 RID: 13526 RVA: 0x00189515 File Offset: 0x00187715
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x0400147C RID: 5244
		[Nullable(2)]
		private ReactionSynchronizer _synchronizer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016DB RID: 5851
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'DeinitializeNetworking' method.
			/// </summary>
			// Token: 0x0400590F RID: 22799
			public static readonly StringName DeinitializeNetworking = "DeinitializeNetworking";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005910 RID: 22800
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'DoLocalReaction' method.
			/// </summary>
			// Token: 0x04005911 RID: 22801
			public static readonly StringName DoLocalReaction = "DoLocalReaction";

			/// <summary>
			/// Cached name for the 'DoRemoteReaction' method.
			/// </summary>
			// Token: 0x04005912 RID: 22802
			public static readonly StringName DoRemoteReaction = "DoRemoteReaction";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016DC RID: 5852
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InMultiplayer' property.
			/// </summary>
			// Token: 0x04005913 RID: 22803
			public static readonly StringName InMultiplayer = "InMultiplayer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016DD RID: 5853
		public class SignalName : Control.SignalName
		{
		}
	}
}
