using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001CD RID: 461
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NLivingGasVfx.cs")]
	public class NLivingGasVfx : Node
	{
		// Token: 0x0600172C RID: 5932 RVA: 0x0008F6F8 File Offset: 0x0008D8F8
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._attackPuffParticles = base.GetNode<GpuParticles2D>("../AttackFXNode/AttackPuffParticles");
			this._attackSparkParticles = base.GetNode<GpuParticles2D>("../AttackFXNode/AttackSparkParticles");
			this._debuffPuffParticles = base.GetNode<GpuParticles2D>("../AttackFXNode/DebuffPuffParticles");
			this._gasPuffParticles = base.GetNode<GpuParticles2D>("../AttackFXNode/GasPuffParticles");
			this._smokeSlot1 = new MegaSlotNode(base.GetNode<Node2D>("../SmokeSlot1"));
			this._smokeSlot2 = new MegaSlotNode(base.GetNode<Node2D>("../SmokeSlot2"));
			this._smokeSlot3 = new MegaSlotNode(base.GetNode<Node2D>("../SmokeSlot3"));
			this._smokeMaterials = new List<ShaderMaterial>();
			this._smokeMaterials.Add(this._smokeSlot1.GetNormalMaterial() as ShaderMaterial);
			this._smokeMaterials.Add(this._smokeSlot2.GetNormalMaterial() as ShaderMaterial);
			this._smokeMaterials.Add(this._smokeSlot3.GetNormalMaterial() as ShaderMaterial);
			this._smokeSteps = new List<Vector2>();
			this._smokeSteps.Add((Vector2)this._smokeMaterials[0].GetShaderParameter(NLivingGasVfx._alphaStep));
			this._smokeSteps.Add((Vector2)this._smokeMaterials[1].GetShaderParameter(NLivingGasVfx._alphaStep));
			this._smokeSteps.Add((Vector2)this._smokeMaterials[2].GetShaderParameter(NLivingGasVfx._alphaStep));
			this._attackPuffParticles.Emitting = false;
			this._attackSparkParticles.Emitting = false;
			this._debuffPuffParticles.Emitting = false;
			this._gasPuffParticles.Emitting = false;
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x0008F908 File Offset: 0x0008DB08
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName != null)
			{
				switch (eventName.Length)
				{
				case 8:
					if (!(eventName == "hurt_end"))
					{
						return;
					}
					this.OnHurtEnd();
					return;
				case 9:
					if (!(eventName == "dissipate"))
					{
						return;
					}
					this.OnDissipate();
					return;
				case 10:
				{
					char c = eventName[0];
					if (c != 'a')
					{
						if (c != 'd')
						{
							if (c != 'h')
							{
								return;
							}
							if (!(eventName == "hurt_start"))
							{
								return;
							}
							this.OnHurtStart();
							return;
						}
						else
						{
							if (!(eventName == "debuff_end"))
							{
								return;
							}
							this.OnDebuffEnd();
							return;
						}
					}
					else
					{
						if (!(eventName == "attack_end"))
						{
							return;
						}
						this.OnAttackEnd();
						return;
					}
					break;
				}
				case 11:
				case 13:
				case 15:
					break;
				case 12:
				{
					char c = eventName[0];
					if (c != 'a')
					{
						if (c != 'd')
						{
							if (c != 'r')
							{
								return;
							}
							if (!(eventName == "reconstitute"))
							{
								return;
							}
							this.OnReconstitute();
						}
						else
						{
							if (!(eventName == "debuff_start"))
							{
								return;
							}
							this.OnDebuffStart();
							return;
						}
					}
					else
					{
						if (!(eventName == "attack_start"))
						{
							return;
						}
						this.OnAttackStart();
						return;
					}
					break;
				}
				case 14:
					if (!(eventName == "gas_breath_end"))
					{
						return;
					}
					this.OnDeathBreathEnd();
					return;
				case 16:
					if (!(eventName == "gas_breath_start"))
					{
						return;
					}
					this.OnDeathBreathStart();
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x0008FA70 File Offset: 0x0008DC70
		private void OnAttackStart()
		{
			this._attackPuffParticles.Emitting = true;
			this._attackSparkParticles.Amount = 24;
			this._attackSparkParticles.Restart();
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x0008FA96 File Offset: 0x0008DC96
		private void OnAttackEnd()
		{
			this._attackPuffParticles.Emitting = false;
			this._attackSparkParticles.Emitting = false;
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x0008FAB0 File Offset: 0x0008DCB0
		private void OnHurtStart()
		{
			this._attackSparkParticles.Amount = 100;
			this._attackSparkParticles.Emitting = true;
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x0008FACB File Offset: 0x0008DCCB
		private void OnHurtEnd()
		{
			this._attackSparkParticles.Emitting = false;
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x0008FAD9 File Offset: 0x0008DCD9
		private void OnDebuffStart()
		{
			this._debuffPuffParticles.Emitting = true;
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x0008FAE7 File Offset: 0x0008DCE7
		private void OnDebuffEnd()
		{
			this._debuffPuffParticles.Emitting = false;
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x0008FAF8 File Offset: 0x0008DCF8
		private void OnDissipate()
		{
			Tween tween = base.CreateTween();
			tween.TweenMethod(Callable.From<float>(new Action<float>(this.DissipateFunction)), 0f, 1f, 1.399999976158142);
			tween.SetEase(0L).SetTrans(3L);
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x0008FB54 File Offset: 0x0008DD54
		private void DissipateFunction(float t)
		{
			for (int i = 0; i < this._smokeMaterials.Count; i++)
			{
				this._smokeMaterials[i].SetShaderParameter(NLivingGasVfx._alphaStep, this._smokeSteps[i] + (Vector2.One - this._smokeSteps[i]) * t);
			}
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x0008FBBF File Offset: 0x0008DDBF
		private void OnDeathBreathStart()
		{
			this._gasPuffParticles.Emitting = true;
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x0008FBCD File Offset: 0x0008DDCD
		private void OnDeathBreathEnd()
		{
			this._gasPuffParticles.Emitting = false;
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x0008FBDC File Offset: 0x0008DDDC
		private void OnReconstitute()
		{
			for (int i = 0; i < this._smokeMaterials.Count; i++)
			{
				this._smokeMaterials[i].SetShaderParameter(NLivingGasVfx._alphaStep, this._smokeSteps[i]);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001739 RID: 5945 RVA: 0x0008FC28 File Offset: 0x0008DE28
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(13)
			{
				new MethodInfo(NLivingGasVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLivingGasVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NLivingGasVfx.MethodName.OnAttackStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLivingGasVfx.MethodName.OnAttackEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLivingGasVfx.MethodName.OnHurtStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLivingGasVfx.MethodName.OnHurtEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLivingGasVfx.MethodName.OnDebuffStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLivingGasVfx.MethodName.OnDebuffEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLivingGasVfx.MethodName.OnDissipate, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLivingGasVfx.MethodName.DissipateFunction, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "t", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NLivingGasVfx.MethodName.OnDeathBreathStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLivingGasVfx.MethodName.OnDeathBreathEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLivingGasVfx.MethodName.OnReconstitute, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600173A RID: 5946 RVA: 0x0008FF7C File Offset: 0x0008E17C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLivingGasVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.OnAttackStart && args.Count == 0)
			{
				this.OnAttackStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.OnAttackEnd && args.Count == 0)
			{
				this.OnAttackEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.OnHurtStart && args.Count == 0)
			{
				this.OnHurtStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.OnHurtEnd && args.Count == 0)
			{
				this.OnHurtEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.OnDebuffStart && args.Count == 0)
			{
				this.OnDebuffStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.OnDebuffEnd && args.Count == 0)
			{
				this.OnDebuffEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.OnDissipate && args.Count == 0)
			{
				this.OnDissipate();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.DissipateFunction && args.Count == 1)
			{
				this.DissipateFunction(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.OnDeathBreathStart && args.Count == 0)
			{
				this.OnDeathBreathStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.OnDeathBreathEnd && args.Count == 0)
			{
				this.OnDeathBreathEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLivingGasVfx.MethodName.OnReconstitute && args.Count == 0)
			{
				this.OnReconstitute();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600173B RID: 5947 RVA: 0x000901B8 File Offset: 0x0008E3B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLivingGasVfx.MethodName._Ready || (ref method) == NLivingGasVfx.MethodName.OnAnimationEvent || (ref method) == NLivingGasVfx.MethodName.OnAttackStart || (ref method) == NLivingGasVfx.MethodName.OnAttackEnd || (ref method) == NLivingGasVfx.MethodName.OnHurtStart || (ref method) == NLivingGasVfx.MethodName.OnHurtEnd || (ref method) == NLivingGasVfx.MethodName.OnDebuffStart || (ref method) == NLivingGasVfx.MethodName.OnDebuffEnd || (ref method) == NLivingGasVfx.MethodName.OnDissipate || (ref method) == NLivingGasVfx.MethodName.DissipateFunction || (ref method) == NLivingGasVfx.MethodName.OnDeathBreathStart || (ref method) == NLivingGasVfx.MethodName.OnDeathBreathEnd || (ref method) == NLivingGasVfx.MethodName.OnReconstitute || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600173C RID: 5948 RVA: 0x00090290 File Offset: 0x0008E490
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLivingGasVfx.PropertyName._attackPuffParticles)
			{
				this._attackPuffParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NLivingGasVfx.PropertyName._attackSparkParticles)
			{
				this._attackSparkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NLivingGasVfx.PropertyName._debuffPuffParticles)
			{
				this._debuffPuffParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NLivingGasVfx.PropertyName._gasPuffParticles)
			{
				this._gasPuffParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NLivingGasVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600173D RID: 5949 RVA: 0x0009032C File Offset: 0x0008E52C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLivingGasVfx.PropertyName._attackPuffParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackPuffParticles);
				return true;
			}
			if ((ref name) == NLivingGasVfx.PropertyName._attackSparkParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackSparkParticles);
				return true;
			}
			if ((ref name) == NLivingGasVfx.PropertyName._debuffPuffParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._debuffPuffParticles);
				return true;
			}
			if ((ref name) == NLivingGasVfx.PropertyName._gasPuffParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._gasPuffParticles);
				return true;
			}
			if ((ref name) == NLivingGasVfx.PropertyName._parent)
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
		// Token: 0x0600173E RID: 5950 RVA: 0x000903E4 File Offset: 0x0008E5E4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NLivingGasVfx.PropertyName._attackPuffParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLivingGasVfx.PropertyName._attackSparkParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLivingGasVfx.PropertyName._debuffPuffParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLivingGasVfx.PropertyName._gasPuffParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLivingGasVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600173F RID: 5951 RVA: 0x000904A0 File Offset: 0x0008E6A0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLivingGasVfx.PropertyName._attackPuffParticles, Variant.From<GpuParticles2D>(ref this._attackPuffParticles));
			info.AddProperty(NLivingGasVfx.PropertyName._attackSparkParticles, Variant.From<GpuParticles2D>(ref this._attackSparkParticles));
			info.AddProperty(NLivingGasVfx.PropertyName._debuffPuffParticles, Variant.From<GpuParticles2D>(ref this._debuffPuffParticles));
			info.AddProperty(NLivingGasVfx.PropertyName._gasPuffParticles, Variant.From<GpuParticles2D>(ref this._gasPuffParticles));
			info.AddProperty(NLivingGasVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001740 RID: 5952 RVA: 0x00090524 File Offset: 0x0008E724
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLivingGasVfx.PropertyName._attackPuffParticles, ref variant))
			{
				this._attackPuffParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLivingGasVfx.PropertyName._attackSparkParticles, ref variant2))
			{
				this._attackSparkParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NLivingGasVfx.PropertyName._debuffPuffParticles, ref variant3))
			{
				this._debuffPuffParticles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NLivingGasVfx.PropertyName._gasPuffParticles, ref variant4))
			{
				this._gasPuffParticles = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NLivingGasVfx.PropertyName._parent, ref variant5))
			{
				this._parent = variant5.As<Node2D>();
			}
		}

		// Token: 0x04000847 RID: 2119
		private static readonly StringName _alphaStep = new StringName("AlphaStep");

		// Token: 0x04000848 RID: 2120
		private GpuParticles2D _attackPuffParticles;

		// Token: 0x04000849 RID: 2121
		private GpuParticles2D _attackSparkParticles;

		// Token: 0x0400084A RID: 2122
		private GpuParticles2D _debuffPuffParticles;

		// Token: 0x0400084B RID: 2123
		private GpuParticles2D _gasPuffParticles;

		// Token: 0x0400084C RID: 2124
		private MegaSlotNode _smokeSlot1;

		// Token: 0x0400084D RID: 2125
		private MegaSlotNode _smokeSlot2;

		// Token: 0x0400084E RID: 2126
		private MegaSlotNode _smokeSlot3;

		// Token: 0x0400084F RID: 2127
		[Nullable(new byte[] { 1, 2 })]
		private List<ShaderMaterial> _smokeMaterials;

		// Token: 0x04000850 RID: 2128
		private List<Vector2> _smokeSteps;

		// Token: 0x04000851 RID: 2129
		private Node2D _parent;

		// Token: 0x04000852 RID: 2130
		private MegaSprite _megaSprite;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010AC RID: 4268
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004179 RID: 16761
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x0400417A RID: 16762
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAttackStart' method.
			/// </summary>
			// Token: 0x0400417B RID: 16763
			public static readonly StringName OnAttackStart = "OnAttackStart";

			/// <summary>
			/// Cached name for the 'OnAttackEnd' method.
			/// </summary>
			// Token: 0x0400417C RID: 16764
			public static readonly StringName OnAttackEnd = "OnAttackEnd";

			/// <summary>
			/// Cached name for the 'OnHurtStart' method.
			/// </summary>
			// Token: 0x0400417D RID: 16765
			public static readonly StringName OnHurtStart = "OnHurtStart";

			/// <summary>
			/// Cached name for the 'OnHurtEnd' method.
			/// </summary>
			// Token: 0x0400417E RID: 16766
			public static readonly StringName OnHurtEnd = "OnHurtEnd";

			/// <summary>
			/// Cached name for the 'OnDebuffStart' method.
			/// </summary>
			// Token: 0x0400417F RID: 16767
			public static readonly StringName OnDebuffStart = "OnDebuffStart";

			/// <summary>
			/// Cached name for the 'OnDebuffEnd' method.
			/// </summary>
			// Token: 0x04004180 RID: 16768
			public static readonly StringName OnDebuffEnd = "OnDebuffEnd";

			/// <summary>
			/// Cached name for the 'OnDissipate' method.
			/// </summary>
			// Token: 0x04004181 RID: 16769
			public static readonly StringName OnDissipate = "OnDissipate";

			/// <summary>
			/// Cached name for the 'DissipateFunction' method.
			/// </summary>
			// Token: 0x04004182 RID: 16770
			public static readonly StringName DissipateFunction = "DissipateFunction";

			/// <summary>
			/// Cached name for the 'OnDeathBreathStart' method.
			/// </summary>
			// Token: 0x04004183 RID: 16771
			public static readonly StringName OnDeathBreathStart = "OnDeathBreathStart";

			/// <summary>
			/// Cached name for the 'OnDeathBreathEnd' method.
			/// </summary>
			// Token: 0x04004184 RID: 16772
			public static readonly StringName OnDeathBreathEnd = "OnDeathBreathEnd";

			/// <summary>
			/// Cached name for the 'OnReconstitute' method.
			/// </summary>
			// Token: 0x04004185 RID: 16773
			public static readonly StringName OnReconstitute = "OnReconstitute";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010AD RID: 4269
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_attackPuffParticles' field.
			/// </summary>
			// Token: 0x04004186 RID: 16774
			public static readonly StringName _attackPuffParticles = "_attackPuffParticles";

			/// <summary>
			/// Cached name for the '_attackSparkParticles' field.
			/// </summary>
			// Token: 0x04004187 RID: 16775
			public static readonly StringName _attackSparkParticles = "_attackSparkParticles";

			/// <summary>
			/// Cached name for the '_debuffPuffParticles' field.
			/// </summary>
			// Token: 0x04004188 RID: 16776
			public static readonly StringName _debuffPuffParticles = "_debuffPuffParticles";

			/// <summary>
			/// Cached name for the '_gasPuffParticles' field.
			/// </summary>
			// Token: 0x04004189 RID: 16777
			public static readonly StringName _gasPuffParticles = "_gasPuffParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x0400418A RID: 16778
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010AE RID: 4270
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
