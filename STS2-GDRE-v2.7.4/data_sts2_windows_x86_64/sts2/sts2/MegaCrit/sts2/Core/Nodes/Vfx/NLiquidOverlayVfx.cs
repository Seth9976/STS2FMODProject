using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001CC RID: 460
	[ScriptPath("res://src/Core/Nodes/Vfx/NLiquidOverlayVfx.cs")]
	public class NLiquidOverlayVfx : Node2D
	{
		// Token: 0x0600171E RID: 5918 RVA: 0x0008F464 File Offset: 0x0008D664
		[NullableContext(2)]
		public static NLiquidOverlayVfx Create(Creature targetCreature, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NLiquidOverlayVfx nliquidOverlayVfx = PreloadManager.Cache.GetScene(NLiquidOverlayVfx.scenePath).Instantiate<NLiquidOverlayVfx>(0L);
			nliquidOverlayVfx._targetCreature = targetCreature;
			nliquidOverlayVfx._tint = tint;
			return nliquidOverlayVfx;
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x0008F4A0 File Offset: 0x0008D6A0
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlayVfx());
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x0008F4AE File Offset: 0x0008D6AE
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x0008F4C0 File Offset: 0x0008D6C0
		[NullableContext(1)]
		private async Task PlayVfx()
		{
			this._cts = new CancellationTokenSource();
			if (this._targetCreature != null)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(this._targetCreature) : null);
				if (ncreature != null)
				{
					ncreature.Visuals.TryApplyLiquidOverlay(this._tint);
				}
			}
			await Cmd.Wait(0.5f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001722 RID: 5922 RVA: 0x0008F504 File Offset: 0x0008D704
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NLiquidOverlayVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLiquidOverlayVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001723 RID: 5923 RVA: 0x0008F578 File Offset: 0x0008D778
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLiquidOverlayVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLiquidOverlayVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001724 RID: 5924 RVA: 0x0008F5D8 File Offset: 0x0008D7D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLiquidOverlayVfx.MethodName._Ready || (ref method) == NLiquidOverlayVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001725 RID: 5925 RVA: 0x0008F5FF File Offset: 0x0008D7FF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLiquidOverlayVfx.PropertyName._tint)
			{
				this._tint = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001726 RID: 5926 RVA: 0x0008F624 File Offset: 0x0008D824
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLiquidOverlayVfx.PropertyName._tint)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._tint);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001727 RID: 5927 RVA: 0x0008F650 File Offset: 0x0008D850
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(20L, NLiquidOverlayVfx.PropertyName._tint, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001728 RID: 5928 RVA: 0x0008F685 File Offset: 0x0008D885
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLiquidOverlayVfx.PropertyName._tint, Variant.From<Color>(ref this._tint));
		}

		/// <inheritdoc />
		// Token: 0x06001729 RID: 5929 RVA: 0x0008F6A4 File Offset: 0x0008D8A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLiquidOverlayVfx.PropertyName._tint, ref variant))
			{
				this._tint = variant.As<Color>();
			}
		}

		// Token: 0x04000843 RID: 2115
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_liquid_overlay");

		// Token: 0x04000844 RID: 2116
		private Color _tint = Colors.White;

		// Token: 0x04000845 RID: 2117
		[Nullable(2)]
		private Creature _targetCreature;

		// Token: 0x04000846 RID: 2118
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010A8 RID: 4264
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004172 RID: 16754
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004173 RID: 16755
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010A9 RID: 4265
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tint' field.
			/// </summary>
			// Token: 0x04004174 RID: 16756
			public static readonly StringName _tint = "_tint";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010AA RID: 4266
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
