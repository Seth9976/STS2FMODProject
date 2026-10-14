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
	// Token: 0x02000204 RID: 516
	[NullableContext(1)]
	[Nullable(0)]
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NTestSubjectVfx.cs")]
	public class NTestSubjectVfx : Node
	{
		// Token: 0x06001A98 RID: 6808 RVA: 0x000AA51C File Offset: 0x000A871C
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._frontBurnVfxController = new MegaSprite(base.GetNode("../FrontBurnVfxSlot/FrontBurnVfx"));
			this._backBurnVfxController = new MegaSprite(base.GetNode("../BackBurnVfxSlot/BackBurnVfx"));
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._neckParticles = this._parent.GetNode<GpuParticles2D>("NeckParticlesSlot/NeckParticles");
			this._dizzyParticles = this._parent.GetNode<GpuParticles2D>("NeckParticlesSlot/DizzyPaticles");
			this._emberParticles = this._parent.GetNode<GpuParticles2D>("../../EmberParticles");
			this._flameParticles = this._parent.GetNode<GpuParticles2D>("../../FlameParticles");
			this._burnParticles = this._parent.GetNode<GpuParticles2D>("../../BurnParticles");
			this._targetedBurnParticle = this._parent.GetNode<GpuParticles2D>("../../TargetedBurnParticle");
			this._burnParticleFountain = this._parent.GetNode<GpuParticles2D>("../../BurnParticleFountain");
			this._ceilingParticles = this._parent.GetNode<GpuParticles2D>("../../CeilingSparks");
			this._neckParticles.OneShot = true;
			this._neckParticles.Emitting = false;
			this._dizzyParticles.Emitting = false;
			this._emberParticles.OneShot = true;
			this._emberParticles.Emitting = false;
			this._flameParticles.Emitting = false;
			this._burnParticles.Emitting = false;
			this._targetedBurnParticle.Emitting = false;
			this._burnParticleFountain.Emitting = false;
			this._ceilingParticles.OneShot = true;
			this._ceilingParticles.Emitting = false;
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("idle_loop3", true, 0);
			});
			this.RunWhenSpineReady(this._frontBurnVfxController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("empty", true, 0);
			});
			this.RunWhenSpineReady(this._backBurnVfxController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("empty", true, 0);
			});
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x000AA788 File Offset: 0x000A8988
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName != null)
			{
				switch (eventName.Length)
				{
				case 10:
					if (!(eventName == "end_flames"))
					{
						return;
					}
					this.EndFlames();
					return;
				case 11:
					if (!(eventName == "end_dizzies"))
					{
						return;
					}
					this.EndDizzies();
					return;
				case 12:
				{
					char c = eventName[6];
					if (c <= 'f')
					{
						if (c != 'e')
						{
							if (c != 'f')
							{
								return;
							}
							if (!(eventName == "start_flames"))
							{
								return;
							}
							this.StartFlames();
							return;
						}
						else
						{
							if (!(eventName == "start_embers"))
							{
								return;
							}
							this.StartEmbers();
							return;
						}
					}
					else if (c != 'r')
					{
						if (c == 'x')
						{
							if (!(eventName == "neck_explode"))
							{
								return;
							}
							this.SquirtNeck();
							return;
						}
					}
					else
					{
						if (!(eventName == "end_burn_vfx"))
						{
							return;
						}
						this.EndBurnVfx();
						return;
					}
					break;
				}
				case 13:
					if (!(eventName == "start_dizzies"))
					{
						return;
					}
					this.StartDizzies();
					return;
				case 14:
					if (!(eventName == "start_burn_vfx"))
					{
						return;
					}
					this.StartBurnVfx();
					return;
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
					break;
				case 20:
					if (!(eventName == "start_ceiling_sparks"))
					{
						return;
					}
					this.StartCeilingSparks();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000AA8D0 File Offset: 0x000A8AD0
		private void PlayAnim1()
		{
			this._animController.GetAnimationState().SetAnimation("die3", false, 0);
			this._animController.GetAnimationState().AddAnimation("idle_loop3", 0f, true, 0);
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x000AA905 File Offset: 0x000A8B05
		private void SquirtNeck()
		{
			this._neckParticles.Restart();
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x000AA912 File Offset: 0x000A8B12
		private void StartDizzies()
		{
			if (!this._dizzyParticles.Emitting)
			{
				this._dizzyParticles.Emitting = true;
			}
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x000AA92D File Offset: 0x000A8B2D
		private void EndDizzies()
		{
			this._dizzyParticles.Emitting = false;
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x000AA93B File Offset: 0x000A8B3B
		private void StartEmbers()
		{
			this._emberParticles.Restart();
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x000AA948 File Offset: 0x000A8B48
		private void StartFlames()
		{
			this._flameParticles.Emitting = true;
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x000AA956 File Offset: 0x000A8B56
		private void EndFlames()
		{
			this._flameParticles.Emitting = false;
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x000AA964 File Offset: 0x000A8B64
		private void StartBurnVfx()
		{
			this._frontBurnVfxController.GetAnimationState().SetAnimation("burn", false, 0);
			this._backBurnVfxController.GetAnimationState().SetAnimation("burn", false, 0);
			this._burnParticles.Restart();
			this._targetedBurnParticle.Emitting = true;
			this._burnParticleFountain.Restart();
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x000AA9C1 File Offset: 0x000A8BC1
		private void EndBurnVfx()
		{
			this._burnParticles.Emitting = false;
			this._targetedBurnParticle.Emitting = false;
			this._burnParticleFountain.Emitting = false;
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x000AA9E7 File Offset: 0x000A8BE7
		private void StartCeilingSparks()
		{
			this._ceilingParticles.Restart();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001AA4 RID: 6820 RVA: 0x000AA9F4 File Offset: 0x000A8BF4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(12)
			{
				new MethodInfo(NTestSubjectVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTestSubjectVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NTestSubjectVfx.MethodName.PlayAnim1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTestSubjectVfx.MethodName.SquirtNeck, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTestSubjectVfx.MethodName.StartDizzies, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTestSubjectVfx.MethodName.EndDizzies, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTestSubjectVfx.MethodName.StartEmbers, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTestSubjectVfx.MethodName.StartFlames, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTestSubjectVfx.MethodName.EndFlames, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTestSubjectVfx.MethodName.StartBurnVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTestSubjectVfx.MethodName.EndBurnVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTestSubjectVfx.MethodName.StartCeilingSparks, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001AA5 RID: 6821 RVA: 0x000AACF4 File Offset: 0x000A8EF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTestSubjectVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTestSubjectVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTestSubjectVfx.MethodName.PlayAnim1 && args.Count == 0)
			{
				this.PlayAnim1();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTestSubjectVfx.MethodName.SquirtNeck && args.Count == 0)
			{
				this.SquirtNeck();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTestSubjectVfx.MethodName.StartDizzies && args.Count == 0)
			{
				this.StartDizzies();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTestSubjectVfx.MethodName.EndDizzies && args.Count == 0)
			{
				this.EndDizzies();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTestSubjectVfx.MethodName.StartEmbers && args.Count == 0)
			{
				this.StartEmbers();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTestSubjectVfx.MethodName.StartFlames && args.Count == 0)
			{
				this.StartFlames();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTestSubjectVfx.MethodName.EndFlames && args.Count == 0)
			{
				this.EndFlames();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTestSubjectVfx.MethodName.StartBurnVfx && args.Count == 0)
			{
				this.StartBurnVfx();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTestSubjectVfx.MethodName.EndBurnVfx && args.Count == 0)
			{
				this.EndBurnVfx();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTestSubjectVfx.MethodName.StartCeilingSparks && args.Count == 0)
			{
				this.StartCeilingSparks();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001AA6 RID: 6822 RVA: 0x000AAEFC File Offset: 0x000A90FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTestSubjectVfx.MethodName._Ready || (ref method) == NTestSubjectVfx.MethodName.OnAnimationEvent || (ref method) == NTestSubjectVfx.MethodName.PlayAnim1 || (ref method) == NTestSubjectVfx.MethodName.SquirtNeck || (ref method) == NTestSubjectVfx.MethodName.StartDizzies || (ref method) == NTestSubjectVfx.MethodName.EndDizzies || (ref method) == NTestSubjectVfx.MethodName.StartEmbers || (ref method) == NTestSubjectVfx.MethodName.StartFlames || (ref method) == NTestSubjectVfx.MethodName.EndFlames || (ref method) == NTestSubjectVfx.MethodName.StartBurnVfx || (ref method) == NTestSubjectVfx.MethodName.EndBurnVfx || (ref method) == NTestSubjectVfx.MethodName.StartCeilingSparks || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001AA7 RID: 6823 RVA: 0x000AAFC4 File Offset: 0x000A91C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTestSubjectVfx.PropertyName._neckParticles)
			{
				this._neckParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._dizzyParticles)
			{
				this._dizzyParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._emberParticles)
			{
				this._emberParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._flameParticles)
			{
				this._flameParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._burnParticles)
			{
				this._burnParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._targetedBurnParticle)
			{
				this._targetedBurnParticle = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._burnParticleFountain)
			{
				this._burnParticleFountain = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._ceilingParticles)
			{
				this._ceilingParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._keyDown)
			{
				this._keyDown = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._doingThing)
			{
				this._doingThing = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001AA8 RID: 6824 RVA: 0x000AB104 File Offset: 0x000A9304
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTestSubjectVfx.PropertyName._neckParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._neckParticles);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._dizzyParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dizzyParticles);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._emberParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._emberParticles);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._flameParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameParticles);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._burnParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._burnParticles);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._targetedBurnParticle)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._targetedBurnParticle);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._burnParticleFountain)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._burnParticleFountain);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._ceilingParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._ceilingParticles);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._keyDown)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._keyDown);
				return true;
			}
			if ((ref name) == NTestSubjectVfx.PropertyName._doingThing)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._doingThing);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001AA9 RID: 6825 RVA: 0x000AB27C File Offset: 0x000A947C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTestSubjectVfx.PropertyName._neckParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTestSubjectVfx.PropertyName._dizzyParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTestSubjectVfx.PropertyName._emberParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTestSubjectVfx.PropertyName._flameParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTestSubjectVfx.PropertyName._burnParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTestSubjectVfx.PropertyName._targetedBurnParticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTestSubjectVfx.PropertyName._burnParticleFountain, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTestSubjectVfx.PropertyName._ceilingParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTestSubjectVfx.PropertyName._parent, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTestSubjectVfx.PropertyName._keyDown, 0L, "", 4096L, false),
				new PropertyInfo(1L, NTestSubjectVfx.PropertyName._doingThing, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001AAA RID: 6826 RVA: 0x000AB3FC File Offset: 0x000A95FC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTestSubjectVfx.PropertyName._neckParticles, Variant.From<GpuParticles2D>(ref this._neckParticles));
			info.AddProperty(NTestSubjectVfx.PropertyName._dizzyParticles, Variant.From<GpuParticles2D>(ref this._dizzyParticles));
			info.AddProperty(NTestSubjectVfx.PropertyName._emberParticles, Variant.From<GpuParticles2D>(ref this._emberParticles));
			info.AddProperty(NTestSubjectVfx.PropertyName._flameParticles, Variant.From<GpuParticles2D>(ref this._flameParticles));
			info.AddProperty(NTestSubjectVfx.PropertyName._burnParticles, Variant.From<GpuParticles2D>(ref this._burnParticles));
			info.AddProperty(NTestSubjectVfx.PropertyName._targetedBurnParticle, Variant.From<GpuParticles2D>(ref this._targetedBurnParticle));
			info.AddProperty(NTestSubjectVfx.PropertyName._burnParticleFountain, Variant.From<GpuParticles2D>(ref this._burnParticleFountain));
			info.AddProperty(NTestSubjectVfx.PropertyName._ceilingParticles, Variant.From<GpuParticles2D>(ref this._ceilingParticles));
			info.AddProperty(NTestSubjectVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
			info.AddProperty(NTestSubjectVfx.PropertyName._keyDown, Variant.From<bool>(ref this._keyDown));
			info.AddProperty(NTestSubjectVfx.PropertyName._doingThing, Variant.From<bool>(ref this._doingThing));
		}

		/// <inheritdoc />
		// Token: 0x06001AAB RID: 6827 RVA: 0x000AB504 File Offset: 0x000A9704
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTestSubjectVfx.PropertyName._neckParticles, ref variant))
			{
				this._neckParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTestSubjectVfx.PropertyName._dizzyParticles, ref variant2))
			{
				this._dizzyParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTestSubjectVfx.PropertyName._emberParticles, ref variant3))
			{
				this._emberParticles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTestSubjectVfx.PropertyName._flameParticles, ref variant4))
			{
				this._flameParticles = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTestSubjectVfx.PropertyName._burnParticles, ref variant5))
			{
				this._burnParticles = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NTestSubjectVfx.PropertyName._targetedBurnParticle, ref variant6))
			{
				this._targetedBurnParticle = variant6.As<GpuParticles2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NTestSubjectVfx.PropertyName._burnParticleFountain, ref variant7))
			{
				this._burnParticleFountain = variant7.As<GpuParticles2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NTestSubjectVfx.PropertyName._ceilingParticles, ref variant8))
			{
				this._ceilingParticles = variant8.As<GpuParticles2D>();
			}
			Variant variant9;
			if (info.TryGetProperty(NTestSubjectVfx.PropertyName._parent, ref variant9))
			{
				this._parent = variant9.As<Node2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NTestSubjectVfx.PropertyName._keyDown, ref variant10))
			{
				this._keyDown = variant10.As<bool>();
			}
			Variant variant11;
			if (info.TryGetProperty(NTestSubjectVfx.PropertyName._doingThing, ref variant11))
			{
				this._doingThing = variant11.As<bool>();
			}
		}

		// Token: 0x040009DA RID: 2522
		private GpuParticles2D _neckParticles;

		// Token: 0x040009DB RID: 2523
		private GpuParticles2D _dizzyParticles;

		// Token: 0x040009DC RID: 2524
		private GpuParticles2D _emberParticles;

		// Token: 0x040009DD RID: 2525
		private GpuParticles2D _flameParticles;

		// Token: 0x040009DE RID: 2526
		private GpuParticles2D _burnParticles;

		// Token: 0x040009DF RID: 2527
		private GpuParticles2D _targetedBurnParticle;

		// Token: 0x040009E0 RID: 2528
		private GpuParticles2D _burnParticleFountain;

		// Token: 0x040009E1 RID: 2529
		private GpuParticles2D _ceilingParticles;

		// Token: 0x040009E2 RID: 2530
		private Node2D _parent;

		// Token: 0x040009E3 RID: 2531
		private MegaSprite _animController;

		// Token: 0x040009E4 RID: 2532
		private MegaSprite _frontBurnVfxController;

		// Token: 0x040009E5 RID: 2533
		private MegaSprite _backBurnVfxController;

		// Token: 0x040009E6 RID: 2534
		private bool _keyDown;

		// Token: 0x040009E7 RID: 2535
		private bool _doingThing;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200117D RID: 4477
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004424 RID: 17444
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004425 RID: 17445
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'PlayAnim1' method.
			/// </summary>
			// Token: 0x04004426 RID: 17446
			public static readonly StringName PlayAnim1 = "PlayAnim1";

			/// <summary>
			/// Cached name for the 'SquirtNeck' method.
			/// </summary>
			// Token: 0x04004427 RID: 17447
			public static readonly StringName SquirtNeck = "SquirtNeck";

			/// <summary>
			/// Cached name for the 'StartDizzies' method.
			/// </summary>
			// Token: 0x04004428 RID: 17448
			public static readonly StringName StartDizzies = "StartDizzies";

			/// <summary>
			/// Cached name for the 'EndDizzies' method.
			/// </summary>
			// Token: 0x04004429 RID: 17449
			public static readonly StringName EndDizzies = "EndDizzies";

			/// <summary>
			/// Cached name for the 'StartEmbers' method.
			/// </summary>
			// Token: 0x0400442A RID: 17450
			public static readonly StringName StartEmbers = "StartEmbers";

			/// <summary>
			/// Cached name for the 'StartFlames' method.
			/// </summary>
			// Token: 0x0400442B RID: 17451
			public static readonly StringName StartFlames = "StartFlames";

			/// <summary>
			/// Cached name for the 'EndFlames' method.
			/// </summary>
			// Token: 0x0400442C RID: 17452
			public static readonly StringName EndFlames = "EndFlames";

			/// <summary>
			/// Cached name for the 'StartBurnVfx' method.
			/// </summary>
			// Token: 0x0400442D RID: 17453
			public static readonly StringName StartBurnVfx = "StartBurnVfx";

			/// <summary>
			/// Cached name for the 'EndBurnVfx' method.
			/// </summary>
			// Token: 0x0400442E RID: 17454
			public static readonly StringName EndBurnVfx = "EndBurnVfx";

			/// <summary>
			/// Cached name for the 'StartCeilingSparks' method.
			/// </summary>
			// Token: 0x0400442F RID: 17455
			public static readonly StringName StartCeilingSparks = "StartCeilingSparks";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200117E RID: 4478
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_neckParticles' field.
			/// </summary>
			// Token: 0x04004430 RID: 17456
			public static readonly StringName _neckParticles = "_neckParticles";

			/// <summary>
			/// Cached name for the '_dizzyParticles' field.
			/// </summary>
			// Token: 0x04004431 RID: 17457
			public static readonly StringName _dizzyParticles = "_dizzyParticles";

			/// <summary>
			/// Cached name for the '_emberParticles' field.
			/// </summary>
			// Token: 0x04004432 RID: 17458
			public static readonly StringName _emberParticles = "_emberParticles";

			/// <summary>
			/// Cached name for the '_flameParticles' field.
			/// </summary>
			// Token: 0x04004433 RID: 17459
			public static readonly StringName _flameParticles = "_flameParticles";

			/// <summary>
			/// Cached name for the '_burnParticles' field.
			/// </summary>
			// Token: 0x04004434 RID: 17460
			public static readonly StringName _burnParticles = "_burnParticles";

			/// <summary>
			/// Cached name for the '_targetedBurnParticle' field.
			/// </summary>
			// Token: 0x04004435 RID: 17461
			public static readonly StringName _targetedBurnParticle = "_targetedBurnParticle";

			/// <summary>
			/// Cached name for the '_burnParticleFountain' field.
			/// </summary>
			// Token: 0x04004436 RID: 17462
			public static readonly StringName _burnParticleFountain = "_burnParticleFountain";

			/// <summary>
			/// Cached name for the '_ceilingParticles' field.
			/// </summary>
			// Token: 0x04004437 RID: 17463
			public static readonly StringName _ceilingParticles = "_ceilingParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004438 RID: 17464
			public static readonly StringName _parent = "_parent";

			/// <summary>
			/// Cached name for the '_keyDown' field.
			/// </summary>
			// Token: 0x04004439 RID: 17465
			public static readonly StringName _keyDown = "_keyDown";

			/// <summary>
			/// Cached name for the '_doingThing' field.
			/// </summary>
			// Token: 0x0400443A RID: 17466
			public static readonly StringName _doingThing = "_doingThing";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200117F RID: 4479
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
