using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	// Token: 0x02000231 RID: 561
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NCardExhaustQuickVfx.cs")]
	public class NCardExhaustQuickVfx : Control
	{
		// Token: 0x06001C77 RID: 7287 RVA: 0x000B8A78 File Offset: 0x000B6C78
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NCardExhaustQuickVfx Create(NCard cardNode)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCardExhaustQuickVfx ncardExhaustQuickVfx = PreloadManager.Cache.GetScene(NCardExhaustQuickVfx.scenePath).Instantiate<NCardExhaustQuickVfx>(0L);
			ncardExhaustQuickVfx._cardNode = cardNode;
			ncardExhaustQuickVfx._anticipationParticlesContainer.Modulate = new Color(1f, 1f, 1f, 1f);
			cardNode.CardVfxContainer.AddChildSafely(ncardExhaustQuickVfx);
			return ncardExhaustQuickVfx;
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000B8AE0 File Offset: 0x000B6CE0
		[NullableContext(1)]
		public async Task PlayAnimation()
		{
			this._isFinishing = false;
			this._anticipationParticlesContainer.Restart();
			await Cmd.Wait(this._anticipationDuration, false);
			this._isFinishing = true;
			this._anticipationParticlesContainer.Modulate = new Color(1f, 1f, 1f, 0f);
			Node parent = base.GetParent();
			Vector2 globalPosition = base.GlobalPosition;
			float rotation = base.Rotation;
			Vector2 scale = this._cardNode.Scale;
			if (parent != null)
			{
				parent.RemoveChildSafely(this);
			}
			if (NCombatRoom.Instance != null)
			{
				NCombatRoom.Instance.Ui.AddChildSafely(this);
				base.GlobalPosition = globalPosition;
				base.Rotation = rotation;
				base.Scale = scale;
			}
			this._cardNode.QueueFreeSafely();
			this._particlesContainer.Restart();
			TaskHelper.RunSafely(this.DelayedFree());
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000B8B24 File Offset: 0x000B6D24
		[NullableContext(1)]
		private async Task DelayedFree()
		{
			await Cmd.Wait(2f, false);
			this.QueueFreeSafely();
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000B8B67 File Offset: 0x000B6D67
		public override void _ExitTree()
		{
			if (!this._isFinishing && GodotObject.IsInstanceValid(this._cardNode))
			{
				this._cardNode.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C7B RID: 7291 RVA: 0x000B8B8C File Offset: 0x000B6D8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCardExhaustQuickVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardNode", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardExhaustQuickVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C7C RID: 7292 RVA: 0x000B8C3C File Offset: 0x000B6E3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardExhaustQuickVfx.MethodName.Create && args.Count == 1)
			{
				NCardExhaustQuickVfx ncardExhaustQuickVfx = NCardExhaustQuickVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = VariantUtils.CreateFrom<NCardExhaustQuickVfx>(ref ncardExhaustQuickVfx);
				return true;
			}
			if ((ref method) == NCardExhaustQuickVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x000B8CB0 File Offset: 0x000B6EB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardExhaustQuickVfx.MethodName.Create && args.Count == 1)
			{
				NCardExhaustQuickVfx ncardExhaustQuickVfx = NCardExhaustQuickVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = VariantUtils.CreateFrom<NCardExhaustQuickVfx>(ref ncardExhaustQuickVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001C7E RID: 7294 RVA: 0x000B8CFE File Offset: 0x000B6EFE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardExhaustQuickVfx.MethodName.Create || (ref method) == NCardExhaustQuickVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001C7F RID: 7295 RVA: 0x000B8D28 File Offset: 0x000B6F28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardExhaustQuickVfx.PropertyName._anticipationParticlesContainer)
			{
				this._anticipationParticlesContainer = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustQuickVfx.PropertyName._particlesContainer)
			{
				this._particlesContainer = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustQuickVfx.PropertyName._anticipationDuration)
			{
				this._anticipationDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustQuickVfx.PropertyName._isFinishing)
			{
				this._isFinishing = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardExhaustQuickVfx.PropertyName._cardNode)
			{
				this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001C80 RID: 7296 RVA: 0x000B8DC4 File Offset: 0x000B6FC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardExhaustQuickVfx.PropertyName._anticipationParticlesContainer)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._anticipationParticlesContainer);
				return true;
			}
			if ((ref name) == NCardExhaustQuickVfx.PropertyName._particlesContainer)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._particlesContainer);
				return true;
			}
			if ((ref name) == NCardExhaustQuickVfx.PropertyName._anticipationDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._anticipationDuration);
				return true;
			}
			if ((ref name) == NCardExhaustQuickVfx.PropertyName._isFinishing)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isFinishing);
				return true;
			}
			if ((ref name) == NCardExhaustQuickVfx.PropertyName._cardNode)
			{
				value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C81 RID: 7297 RVA: 0x000B8E7C File Offset: 0x000B707C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardExhaustQuickVfx.PropertyName._anticipationParticlesContainer, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCardExhaustQuickVfx.PropertyName._particlesContainer, 34L, "Node2D", 4102L, true),
				new PropertyInfo(3L, NCardExhaustQuickVfx.PropertyName._anticipationDuration, 0L, "", 4102L, true),
				new PropertyInfo(1L, NCardExhaustQuickVfx.PropertyName._isFinishing, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardExhaustQuickVfx.PropertyName._cardNode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C82 RID: 7298 RVA: 0x000B8F38 File Offset: 0x000B7138
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardExhaustQuickVfx.PropertyName._anticipationParticlesContainer, Variant.From<NParticlesContainer>(ref this._anticipationParticlesContainer));
			info.AddProperty(NCardExhaustQuickVfx.PropertyName._particlesContainer, Variant.From<NParticlesContainer>(ref this._particlesContainer));
			info.AddProperty(NCardExhaustQuickVfx.PropertyName._anticipationDuration, Variant.From<float>(ref this._anticipationDuration));
			info.AddProperty(NCardExhaustQuickVfx.PropertyName._isFinishing, Variant.From<bool>(ref this._isFinishing));
			info.AddProperty(NCardExhaustQuickVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
		}

		/// <inheritdoc />
		// Token: 0x06001C83 RID: 7299 RVA: 0x000B8FBC File Offset: 0x000B71BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardExhaustQuickVfx.PropertyName._anticipationParticlesContainer, ref variant))
			{
				this._anticipationParticlesContainer = variant.As<NParticlesContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardExhaustQuickVfx.PropertyName._particlesContainer, ref variant2))
			{
				this._particlesContainer = variant2.As<NParticlesContainer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardExhaustQuickVfx.PropertyName._anticipationDuration, ref variant3))
			{
				this._anticipationDuration = variant3.As<float>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardExhaustQuickVfx.PropertyName._isFinishing, ref variant4))
			{
				this._isFinishing = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardExhaustQuickVfx.PropertyName._cardNode, ref variant5))
			{
				this._cardNode = variant5.As<NCard>();
			}
		}

		// Token: 0x04000ADF RID: 2783
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/ui/card/vfx_card_exhaust_quick");

		// Token: 0x04000AE0 RID: 2784
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _anticipationParticlesContainer;

		// Token: 0x04000AE1 RID: 2785
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _particlesContainer;

		// Token: 0x04000AE2 RID: 2786
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _anticipationDuration = 0.4f;

		// Token: 0x04000AE3 RID: 2787
		private bool _isFinishing;

		// Token: 0x04000AE4 RID: 2788
		[Nullable(1)]
		private NCard _cardNode;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011EF RID: 4591
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004590 RID: 17808
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004591 RID: 17809
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011F0 RID: 4592
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_anticipationParticlesContainer' field.
			/// </summary>
			// Token: 0x04004592 RID: 17810
			public static readonly StringName _anticipationParticlesContainer = "_anticipationParticlesContainer";

			/// <summary>
			/// Cached name for the '_particlesContainer' field.
			/// </summary>
			// Token: 0x04004593 RID: 17811
			public static readonly StringName _particlesContainer = "_particlesContainer";

			/// <summary>
			/// Cached name for the '_anticipationDuration' field.
			/// </summary>
			// Token: 0x04004594 RID: 17812
			public static readonly StringName _anticipationDuration = "_anticipationDuration";

			/// <summary>
			/// Cached name for the '_isFinishing' field.
			/// </summary>
			// Token: 0x04004595 RID: 17813
			public static readonly StringName _isFinishing = "_isFinishing";

			/// <summary>
			/// Cached name for the '_cardNode' field.
			/// </summary>
			// Token: 0x04004596 RID: 17814
			public static readonly StringName _cardNode = "_cardNode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011F1 RID: 4593
		public class SignalName : Control.SignalName
		{
		}
	}
}
