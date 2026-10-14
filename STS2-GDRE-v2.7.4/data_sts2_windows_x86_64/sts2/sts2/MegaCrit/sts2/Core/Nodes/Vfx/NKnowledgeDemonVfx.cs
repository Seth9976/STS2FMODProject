using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001C8 RID: 456
	[NullableContext(1)]
	[Nullable(0)]
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NKnowledgeDemonVfx.cs")]
	public class NKnowledgeDemonVfx : Node
	{
		// Token: 0x060016D9 RID: 5849 RVA: 0x0008CE98 File Offset: 0x0008B098
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._fireNode1 = this._parent.GetNode<Node2D>("FireSlot1/FireHolder1");
			this._fireNode2 = this._parent.GetNode<Node2D>("FireSlot2/FireHolder2");
			this._fireNode3 = this._parent.GetNode<Node2D>("FireSlot3/FireHolder3");
			this._fireNode4 = this._parent.GetNode<Node2D>("FireSlot4/FireHolder4");
			this._explosionParticles = this._parent.GetNode<GpuParticles2D>("ExplosionParticles");
			this._damageParticles = this._parent.GetNode<GpuParticles2D>("DamageParticles");
			this._emberParticles = this._parent.GetNode<GpuParticles2D>("EmberParticles");
			this._thinEmberParticles = this._parent.GetNode<GpuParticles2D>("ThinEmberParticles");
			this._fireNode1.Visible = false;
			this._fireNode2.Visible = false;
			this._fireNode3.Visible = false;
			this._fireNode4.Visible = false;
			this._explosionParticles.Emitting = false;
			this._explosionParticles.OneShot = true;
			this._damageParticles.Emitting = false;
			this._damageParticles.OneShot = true;
			this._emberParticles.Emitting = false;
			this._thinEmberParticles.Emitting = false;
			this.OnBurningEnd();
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("idle_loop", true, 0);
			});
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x0008D084 File Offset: 0x0008B284
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName != null)
			{
				switch (eventName.Length)
				{
				case 7:
					if (!(eventName == "explode"))
					{
						return;
					}
					this.OnExplode();
					return;
				case 8:
				case 9:
				case 14:
				case 16:
					break;
				case 10:
					if (!(eventName == "embers_end"))
					{
						return;
					}
					this.OnEmbersEnd();
					return;
				case 11:
				{
					char c = eventName[0];
					if (c != 'b')
					{
						if (c != 't')
						{
							return;
						}
						if (!(eventName == "take_damage"))
						{
							return;
						}
						this.OnTakeDamage();
						return;
					}
					else
					{
						if (!(eventName == "burning_end"))
						{
							return;
						}
						this.OnBurningEnd();
						return;
					}
					break;
				}
				case 12:
					if (!(eventName == "embers_start"))
					{
						return;
					}
					this.OnEmbersStart();
					return;
				case 13:
					if (!(eventName == "burning_start"))
					{
						return;
					}
					this.OnBurningStart();
					return;
				case 15:
					if (!(eventName == "thin_embers_end"))
					{
						return;
					}
					this.OnThinEmbersEnd();
					break;
				case 17:
					if (!(eventName == "thin_embers_start"))
					{
						return;
					}
					this.OnThinEmbersStart();
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x0008D1A4 File Offset: 0x0008B3A4
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			this.OnBurningEnd();
			this.OnEmbersEnd();
			this.OnThinEmbersEnd();
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x0008D1B8 File Offset: 0x0008B3B8
		private void OnExplode()
		{
			this._explosionParticles.Restart();
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x0008D1C5 File Offset: 0x0008B3C5
		private void OnTakeDamage()
		{
			this._damageParticles.Restart();
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x0008D1D2 File Offset: 0x0008B3D2
		private void OnBurningStart()
		{
			this._fireNode1.Visible = true;
			this._fireNode2.Visible = true;
			this._fireNode3.Visible = true;
			this._fireNode4.Visible = true;
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x0008D204 File Offset: 0x0008B404
		private void OnEmbersStart()
		{
			this._emberParticles.Restart();
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x0008D211 File Offset: 0x0008B411
		private void OnThinEmbersStart()
		{
			this._thinEmberParticles.Restart();
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x0008D21E File Offset: 0x0008B41E
		private void OnBurningEnd()
		{
			this._fireNode1.Visible = false;
			this._fireNode2.Visible = false;
			this._fireNode3.Visible = false;
			this._fireNode4.Visible = false;
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x0008D250 File Offset: 0x0008B450
		private void OnEmbersEnd()
		{
			this._emberParticles.Emitting = false;
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x0008D25E File Offset: 0x0008B45E
		private void OnThinEmbersEnd()
		{
			this._thinEmberParticles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060016E4 RID: 5860 RVA: 0x0008D26C File Offset: 0x0008B46C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NKnowledgeDemonVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKnowledgeDemonVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NKnowledgeDemonVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NKnowledgeDemonVfx.MethodName.OnExplode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKnowledgeDemonVfx.MethodName.OnTakeDamage, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKnowledgeDemonVfx.MethodName.OnBurningStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKnowledgeDemonVfx.MethodName.OnEmbersStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKnowledgeDemonVfx.MethodName.OnThinEmbersStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKnowledgeDemonVfx.MethodName.OnBurningEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKnowledgeDemonVfx.MethodName.OnEmbersEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKnowledgeDemonVfx.MethodName.OnThinEmbersEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016E5 RID: 5861 RVA: 0x0008D5C4 File Offset: 0x0008B7C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NKnowledgeDemonVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKnowledgeDemonVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKnowledgeDemonVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKnowledgeDemonVfx.MethodName.OnExplode && args.Count == 0)
			{
				this.OnExplode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKnowledgeDemonVfx.MethodName.OnTakeDamage && args.Count == 0)
			{
				this.OnTakeDamage();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKnowledgeDemonVfx.MethodName.OnBurningStart && args.Count == 0)
			{
				this.OnBurningStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKnowledgeDemonVfx.MethodName.OnEmbersStart && args.Count == 0)
			{
				this.OnEmbersStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKnowledgeDemonVfx.MethodName.OnThinEmbersStart && args.Count == 0)
			{
				this.OnThinEmbersStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKnowledgeDemonVfx.MethodName.OnBurningEnd && args.Count == 0)
			{
				this.OnBurningEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKnowledgeDemonVfx.MethodName.OnEmbersEnd && args.Count == 0)
			{
				this.OnEmbersEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKnowledgeDemonVfx.MethodName.OnThinEmbersEnd && args.Count == 0)
			{
				this.OnThinEmbersEnd();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060016E6 RID: 5862 RVA: 0x0008D7D0 File Offset: 0x0008B9D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NKnowledgeDemonVfx.MethodName._Ready || (ref method) == NKnowledgeDemonVfx.MethodName.OnAnimationEvent || (ref method) == NKnowledgeDemonVfx.MethodName.OnAnimationStart || (ref method) == NKnowledgeDemonVfx.MethodName.OnExplode || (ref method) == NKnowledgeDemonVfx.MethodName.OnTakeDamage || (ref method) == NKnowledgeDemonVfx.MethodName.OnBurningStart || (ref method) == NKnowledgeDemonVfx.MethodName.OnEmbersStart || (ref method) == NKnowledgeDemonVfx.MethodName.OnThinEmbersStart || (ref method) == NKnowledgeDemonVfx.MethodName.OnBurningEnd || (ref method) == NKnowledgeDemonVfx.MethodName.OnEmbersEnd || (ref method) == NKnowledgeDemonVfx.MethodName.OnThinEmbersEnd || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060016E7 RID: 5863 RVA: 0x0008D88C File Offset: 0x0008BA8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._fireNode1)
			{
				this._fireNode1 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._fireNode2)
			{
				this._fireNode2 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._fireNode3)
			{
				this._fireNode3 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._fireNode4)
			{
				this._fireNode4 = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._explosionParticles)
			{
				this._explosionParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._damageParticles)
			{
				this._damageParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._emberParticles)
			{
				this._emberParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._thinEmberParticles)
			{
				this._thinEmberParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060016E8 RID: 5864 RVA: 0x0008D994 File Offset: 0x0008BB94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._fireNode1)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._fireNode1);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._fireNode2)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._fireNode2);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._fireNode3)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._fireNode3);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._fireNode4)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._fireNode4);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._explosionParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._explosionParticles);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._damageParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._damageParticles);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._emberParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._emberParticles);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._thinEmberParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._thinEmberParticles);
				return true;
			}
			if ((ref name) == NKnowledgeDemonVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060016E9 RID: 5865 RVA: 0x0008DACC File Offset: 0x0008BCCC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NKnowledgeDemonVfx.PropertyName._fireNode1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKnowledgeDemonVfx.PropertyName._fireNode2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKnowledgeDemonVfx.PropertyName._fireNode3, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKnowledgeDemonVfx.PropertyName._fireNode4, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKnowledgeDemonVfx.PropertyName._explosionParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKnowledgeDemonVfx.PropertyName._damageParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKnowledgeDemonVfx.PropertyName._emberParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKnowledgeDemonVfx.PropertyName._thinEmberParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKnowledgeDemonVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016EA RID: 5866 RVA: 0x0008DC0C File Offset: 0x0008BE0C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NKnowledgeDemonVfx.PropertyName._fireNode1, Variant.From<Node2D>(ref this._fireNode1));
			info.AddProperty(NKnowledgeDemonVfx.PropertyName._fireNode2, Variant.From<Node2D>(ref this._fireNode2));
			info.AddProperty(NKnowledgeDemonVfx.PropertyName._fireNode3, Variant.From<Node2D>(ref this._fireNode3));
			info.AddProperty(NKnowledgeDemonVfx.PropertyName._fireNode4, Variant.From<Node2D>(ref this._fireNode4));
			info.AddProperty(NKnowledgeDemonVfx.PropertyName._explosionParticles, Variant.From<GpuParticles2D>(ref this._explosionParticles));
			info.AddProperty(NKnowledgeDemonVfx.PropertyName._damageParticles, Variant.From<GpuParticles2D>(ref this._damageParticles));
			info.AddProperty(NKnowledgeDemonVfx.PropertyName._emberParticles, Variant.From<GpuParticles2D>(ref this._emberParticles));
			info.AddProperty(NKnowledgeDemonVfx.PropertyName._thinEmberParticles, Variant.From<GpuParticles2D>(ref this._thinEmberParticles));
			info.AddProperty(NKnowledgeDemonVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x060016EB RID: 5867 RVA: 0x0008DCE8 File Offset: 0x0008BEE8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._fireNode1, ref variant))
			{
				this._fireNode1 = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._fireNode2, ref variant2))
			{
				this._fireNode2 = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._fireNode3, ref variant3))
			{
				this._fireNode3 = variant3.As<Node2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._fireNode4, ref variant4))
			{
				this._fireNode4 = variant4.As<Node2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._explosionParticles, ref variant5))
			{
				this._explosionParticles = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._damageParticles, ref variant6))
			{
				this._damageParticles = variant6.As<GpuParticles2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._emberParticles, ref variant7))
			{
				this._emberParticles = variant7.As<GpuParticles2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._thinEmberParticles, ref variant8))
			{
				this._thinEmberParticles = variant8.As<GpuParticles2D>();
			}
			Variant variant9;
			if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._parent, ref variant9))
			{
				this._parent = variant9.As<Node2D>();
			}
		}

		// Token: 0x04000827 RID: 2087
		private Node2D _fireNode1;

		// Token: 0x04000828 RID: 2088
		private Node2D _fireNode2;

		// Token: 0x04000829 RID: 2089
		private Node2D _fireNode3;

		// Token: 0x0400082A RID: 2090
		private Node2D _fireNode4;

		// Token: 0x0400082B RID: 2091
		private GpuParticles2D _explosionParticles;

		// Token: 0x0400082C RID: 2092
		private GpuParticles2D _damageParticles;

		// Token: 0x0400082D RID: 2093
		private GpuParticles2D _emberParticles;

		// Token: 0x0400082E RID: 2094
		private GpuParticles2D _thinEmberParticles;

		// Token: 0x0400082F RID: 2095
		private Node2D _parent;

		// Token: 0x04000830 RID: 2096
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001098 RID: 4248
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004134 RID: 16692
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004135 RID: 16693
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x04004136 RID: 16694
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'OnExplode' method.
			/// </summary>
			// Token: 0x04004137 RID: 16695
			public static readonly StringName OnExplode = "OnExplode";

			/// <summary>
			/// Cached name for the 'OnTakeDamage' method.
			/// </summary>
			// Token: 0x04004138 RID: 16696
			public static readonly StringName OnTakeDamage = "OnTakeDamage";

			/// <summary>
			/// Cached name for the 'OnBurningStart' method.
			/// </summary>
			// Token: 0x04004139 RID: 16697
			public static readonly StringName OnBurningStart = "OnBurningStart";

			/// <summary>
			/// Cached name for the 'OnEmbersStart' method.
			/// </summary>
			// Token: 0x0400413A RID: 16698
			public static readonly StringName OnEmbersStart = "OnEmbersStart";

			/// <summary>
			/// Cached name for the 'OnThinEmbersStart' method.
			/// </summary>
			// Token: 0x0400413B RID: 16699
			public static readonly StringName OnThinEmbersStart = "OnThinEmbersStart";

			/// <summary>
			/// Cached name for the 'OnBurningEnd' method.
			/// </summary>
			// Token: 0x0400413C RID: 16700
			public static readonly StringName OnBurningEnd = "OnBurningEnd";

			/// <summary>
			/// Cached name for the 'OnEmbersEnd' method.
			/// </summary>
			// Token: 0x0400413D RID: 16701
			public static readonly StringName OnEmbersEnd = "OnEmbersEnd";

			/// <summary>
			/// Cached name for the 'OnThinEmbersEnd' method.
			/// </summary>
			// Token: 0x0400413E RID: 16702
			public static readonly StringName OnThinEmbersEnd = "OnThinEmbersEnd";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001099 RID: 4249
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_fireNode1' field.
			/// </summary>
			// Token: 0x0400413F RID: 16703
			public static readonly StringName _fireNode1 = "_fireNode1";

			/// <summary>
			/// Cached name for the '_fireNode2' field.
			/// </summary>
			// Token: 0x04004140 RID: 16704
			public static readonly StringName _fireNode2 = "_fireNode2";

			/// <summary>
			/// Cached name for the '_fireNode3' field.
			/// </summary>
			// Token: 0x04004141 RID: 16705
			public static readonly StringName _fireNode3 = "_fireNode3";

			/// <summary>
			/// Cached name for the '_fireNode4' field.
			/// </summary>
			// Token: 0x04004142 RID: 16706
			public static readonly StringName _fireNode4 = "_fireNode4";

			/// <summary>
			/// Cached name for the '_explosionParticles' field.
			/// </summary>
			// Token: 0x04004143 RID: 16707
			public static readonly StringName _explosionParticles = "_explosionParticles";

			/// <summary>
			/// Cached name for the '_damageParticles' field.
			/// </summary>
			// Token: 0x04004144 RID: 16708
			public static readonly StringName _damageParticles = "_damageParticles";

			/// <summary>
			/// Cached name for the '_emberParticles' field.
			/// </summary>
			// Token: 0x04004145 RID: 16709
			public static readonly StringName _emberParticles = "_emberParticles";

			/// <summary>
			/// Cached name for the '_thinEmberParticles' field.
			/// </summary>
			// Token: 0x04004146 RID: 16710
			public static readonly StringName _thinEmberParticles = "_thinEmberParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004147 RID: 16711
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200109A RID: 4250
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
