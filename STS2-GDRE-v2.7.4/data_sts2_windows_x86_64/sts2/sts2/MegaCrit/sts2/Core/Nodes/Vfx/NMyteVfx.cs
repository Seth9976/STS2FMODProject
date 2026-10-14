using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001D6 RID: 470
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NMyteVfx.cs")]
	public class NMyteVfx : Node
	{
		// Token: 0x060017BA RID: 6074 RVA: 0x00094014 File Offset: 0x00092214
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._targetBone = this._parent.GetNode<Node2D>("TargetBone");
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("cast", true, 0);
			});
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x000940A6 File Offset: 0x000922A6
		[NullableContext(2)]
		public void SetTarget(Creature target)
		{
			this._target = target;
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x000940B0 File Offset: 0x000922B0
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "start_cast")
			{
				this.StartCast();
			}
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x000940E8 File Offset: 0x000922E8
		private void StartCast()
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(this._target) : null);
			if (ncreature != null)
			{
				this._targetBone.GlobalPosition = new Vector2(ncreature.GlobalPosition.X, ncreature.GlobalPosition.Y - 150f);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060017BE RID: 6078 RVA: 0x0009413C File Offset: 0x0009233C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMyteVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMyteVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NMyteVfx.MethodName.StartCast, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060017BF RID: 6079 RVA: 0x00094294 File Offset: 0x00092494
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMyteVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMyteVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMyteVfx.MethodName.StartCast && args.Count == 0)
			{
				this.StartCast();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060017C0 RID: 6080 RVA: 0x0009434E File Offset: 0x0009254E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMyteVfx.MethodName._Ready || (ref method) == NMyteVfx.MethodName.OnAnimationEvent || (ref method) == NMyteVfx.MethodName.StartCast || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060017C1 RID: 6081 RVA: 0x00094384 File Offset: 0x00092584
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMyteVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NMyteVfx.PropertyName._targetBone)
			{
				this._targetBone = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060017C2 RID: 6082 RVA: 0x000943C4 File Offset: 0x000925C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMyteVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			if ((ref name) == NMyteVfx.PropertyName._targetBone)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._targetBone);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060017C3 RID: 6083 RVA: 0x0009441C File Offset: 0x0009261C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMyteVfx.PropertyName._parent, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMyteVfx.PropertyName._targetBone, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060017C4 RID: 6084 RVA: 0x00094472 File Offset: 0x00092672
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMyteVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
			info.AddProperty(NMyteVfx.PropertyName._targetBone, Variant.From<Node2D>(ref this._targetBone));
		}

		/// <inheritdoc />
		// Token: 0x060017C5 RID: 6085 RVA: 0x000944A8 File Offset: 0x000926A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMyteVfx.PropertyName._parent, ref variant))
			{
				this._parent = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMyteVfx.PropertyName._targetBone, ref variant2))
			{
				this._targetBone = variant2.As<Node2D>();
			}
		}

		// Token: 0x04000893 RID: 2195
		private const float _attackHeight = 150f;

		// Token: 0x04000894 RID: 2196
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000895 RID: 2197
		[Nullable(1)]
		private MegaSprite _animController;

		// Token: 0x04000896 RID: 2198
		[Nullable(1)]
		private Node2D _targetBone;

		// Token: 0x04000897 RID: 2199
		[Nullable(2)]
		private Creature _target;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010CF RID: 4303
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040041F5 RID: 16885
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040041F6 RID: 16886
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartCast' method.
			/// </summary>
			// Token: 0x040041F7 RID: 16887
			public static readonly StringName StartCast = "StartCast";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010D0 RID: 4304
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x040041F8 RID: 16888
			public static readonly StringName _parent = "_parent";

			/// <summary>
			/// Cached name for the '_targetBone' field.
			/// </summary>
			// Token: 0x040041F9 RID: 16889
			public static readonly StringName _targetBone = "_targetBone";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010D1 RID: 4305
		public class SignalName : Node.SignalName
		{
		}
	}
}
