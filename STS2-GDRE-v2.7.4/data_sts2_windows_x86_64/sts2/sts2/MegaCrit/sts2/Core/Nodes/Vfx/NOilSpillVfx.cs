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
	// Token: 0x020001D8 RID: 472
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NOilSpillVfx.cs")]
	public class NOilSpillVfx : Node
	{
		// Token: 0x060017D5 RID: 6101 RVA: 0x00094D0C File Offset: 0x00092F0C
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._droolParticles = this._parent.GetNode<GpuParticles2D>("MouthDribbleBoneNode/DribbleParticles");
			this._sprayParticles = this._parent.GetNode<GpuParticles2D>("MouthSpraySlot/SprayParticles");
			this._rainDropParticles = this._parent.GetNode<GpuParticles2D>("MouthSpraySlot/RainDropParticles");
			this._rainDropParticles.OneShot = true;
			this._droolParticles.Restart();
			this._rainDropParticles.Restart();
			this._sprayParticles.Restart();
			this.TurnOffSprayAttack();
			this.TurnOffSlamSpray();
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00094E00 File Offset: 0x00093000
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName != null)
			{
				switch (eventName.Length)
				{
				case 9:
				{
					char c = eventName[0];
					if (c != 'd')
					{
						if (c != 's')
						{
							return;
						}
						if (!(eventName == "spray_end"))
						{
							return;
						}
						this.TurnOffSprayAttack();
						return;
					}
					else
					{
						if (!(eventName == "drool_end"))
						{
							return;
						}
						this.TurnOffDrool();
					}
					break;
				}
				case 10:
				case 12:
				case 13:
					break;
				case 11:
				{
					char c = eventName[0];
					if (c != 'd')
					{
						if (c != 's')
						{
							return;
						}
						if (!(eventName == "spray_start"))
						{
							return;
						}
						this.TurnOnSprayAttack();
						return;
					}
					else
					{
						if (!(eventName == "drool_start"))
						{
							return;
						}
						this.TurnOnDrool();
						return;
					}
					break;
				}
				case 14:
					if (!(eventName == "slam_spray_end"))
					{
						return;
					}
					this.TurnOffSlamSpray();
					return;
				case 15:
					if (!(eventName == "death_spray_end"))
					{
						return;
					}
					this.TurnOffDeathSpray();
					return;
				case 16:
					if (!(eventName == "slam_spray_start"))
					{
						return;
					}
					this.TurnOnSlamSpray();
					return;
				case 17:
					if (!(eventName == "death_spray_start"))
					{
						return;
					}
					this.TurnOnDeathSpray();
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00094F2C File Offset: 0x0009312C
		[NullableContext(1)]
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			string currentAnimationName = new MegaAnimationState(animationState).GetCurrentAnimationName(0);
			if (currentAnimationName != "slam")
			{
				this.TurnOffSprayAttack();
			}
			if (currentAnimationName != "spray")
			{
				this.TurnOffSlamSpray();
			}
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00094F71 File Offset: 0x00093171
		private void TurnOnSprayAttack()
		{
			this._sprayParticles.Amount = 2000;
			this._sprayParticles.Emitting = true;
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00094F8F File Offset: 0x0009318F
		private void TurnOffSprayAttack()
		{
			this._sprayParticles.Emitting = false;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x00094FA0 File Offset: 0x000931A0
		private void TurnOnSlamSpray()
		{
			this._rainDropParticles.OneShot = false;
			this._rainDropParticles.Amount = 800;
			this._rainDropParticles.Explosiveness = 0f;
			this._rainDropParticles.Lifetime = 0.75;
			this._rainDropParticles.Restart();
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00094FF8 File Offset: 0x000931F8
		private void TurnOffSlamSpray()
		{
			this._rainDropParticles.Emitting = false;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00095006 File Offset: 0x00093206
		private void TurnOnDeathSpray()
		{
			this._sprayParticles.Amount = 500;
			this._sprayParticles.Emitting = true;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00095024 File Offset: 0x00093224
		private void TurnOffDeathSpray()
		{
			this._sprayParticles.Emitting = false;
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00095032 File Offset: 0x00093232
		private void TurnOnDrool()
		{
			this._droolParticles.Emitting = true;
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x00095040 File Offset: 0x00093240
		private void TurnOffDrool()
		{
			this._droolParticles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060017E0 RID: 6112 RVA: 0x00095050 File Offset: 0x00093250
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NOilSpillVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOilSpillVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NOilSpillVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NOilSpillVfx.MethodName.TurnOnSprayAttack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOilSpillVfx.MethodName.TurnOffSprayAttack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOilSpillVfx.MethodName.TurnOnSlamSpray, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOilSpillVfx.MethodName.TurnOffSlamSpray, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOilSpillVfx.MethodName.TurnOnDeathSpray, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOilSpillVfx.MethodName.TurnOffDeathSpray, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOilSpillVfx.MethodName.TurnOnDrool, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOilSpillVfx.MethodName.TurnOffDrool, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060017E1 RID: 6113 RVA: 0x000953A8 File Offset: 0x000935A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NOilSpillVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOilSpillVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOilSpillVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOilSpillVfx.MethodName.TurnOnSprayAttack && args.Count == 0)
			{
				this.TurnOnSprayAttack();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOilSpillVfx.MethodName.TurnOffSprayAttack && args.Count == 0)
			{
				this.TurnOffSprayAttack();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOilSpillVfx.MethodName.TurnOnSlamSpray && args.Count == 0)
			{
				this.TurnOnSlamSpray();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOilSpillVfx.MethodName.TurnOffSlamSpray && args.Count == 0)
			{
				this.TurnOffSlamSpray();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOilSpillVfx.MethodName.TurnOnDeathSpray && args.Count == 0)
			{
				this.TurnOnDeathSpray();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOilSpillVfx.MethodName.TurnOffDeathSpray && args.Count == 0)
			{
				this.TurnOffDeathSpray();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOilSpillVfx.MethodName.TurnOnDrool && args.Count == 0)
			{
				this.TurnOnDrool();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOilSpillVfx.MethodName.TurnOffDrool && args.Count == 0)
			{
				this.TurnOffDrool();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060017E2 RID: 6114 RVA: 0x000955B4 File Offset: 0x000937B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NOilSpillVfx.MethodName._Ready || (ref method) == NOilSpillVfx.MethodName.OnAnimationEvent || (ref method) == NOilSpillVfx.MethodName.OnAnimationStart || (ref method) == NOilSpillVfx.MethodName.TurnOnSprayAttack || (ref method) == NOilSpillVfx.MethodName.TurnOffSprayAttack || (ref method) == NOilSpillVfx.MethodName.TurnOnSlamSpray || (ref method) == NOilSpillVfx.MethodName.TurnOffSlamSpray || (ref method) == NOilSpillVfx.MethodName.TurnOnDeathSpray || (ref method) == NOilSpillVfx.MethodName.TurnOffDeathSpray || (ref method) == NOilSpillVfx.MethodName.TurnOnDrool || (ref method) == NOilSpillVfx.MethodName.TurnOffDrool || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060017E3 RID: 6115 RVA: 0x00095670 File Offset: 0x00093870
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NOilSpillVfx.PropertyName._droolParticles)
			{
				this._droolParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NOilSpillVfx.PropertyName._sprayParticles)
			{
				this._sprayParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NOilSpillVfx.PropertyName._rainDropParticles)
			{
				this._rainDropParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NOilSpillVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060017E4 RID: 6116 RVA: 0x000956F4 File Offset: 0x000938F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NOilSpillVfx.PropertyName._droolParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._droolParticles);
				return true;
			}
			if ((ref name) == NOilSpillVfx.PropertyName._sprayParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sprayParticles);
				return true;
			}
			if ((ref name) == NOilSpillVfx.PropertyName._rainDropParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._rainDropParticles);
				return true;
			}
			if ((ref name) == NOilSpillVfx.PropertyName._parent)
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
		// Token: 0x060017E5 RID: 6117 RVA: 0x0009578C File Offset: 0x0009398C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NOilSpillVfx.PropertyName._droolParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOilSpillVfx.PropertyName._sprayParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOilSpillVfx.PropertyName._rainDropParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOilSpillVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060017E6 RID: 6118 RVA: 0x00095824 File Offset: 0x00093A24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NOilSpillVfx.PropertyName._droolParticles, Variant.From<GpuParticles2D>(ref this._droolParticles));
			info.AddProperty(NOilSpillVfx.PropertyName._sprayParticles, Variant.From<GpuParticles2D>(ref this._sprayParticles));
			info.AddProperty(NOilSpillVfx.PropertyName._rainDropParticles, Variant.From<GpuParticles2D>(ref this._rainDropParticles));
			info.AddProperty(NOilSpillVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x060017E7 RID: 6119 RVA: 0x00095890 File Offset: 0x00093A90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NOilSpillVfx.PropertyName._droolParticles, ref variant))
			{
				this._droolParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NOilSpillVfx.PropertyName._sprayParticles, ref variant2))
			{
				this._sprayParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NOilSpillVfx.PropertyName._rainDropParticles, ref variant3))
			{
				this._rainDropParticles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NOilSpillVfx.PropertyName._parent, ref variant4))
			{
				this._parent = variant4.As<Node2D>();
			}
		}

		// Token: 0x0400089D RID: 2205
		private const int _slamSprayAmount = 800;

		// Token: 0x0400089E RID: 2206
		private const int _sprayAttackAmount = 2000;

		// Token: 0x0400089F RID: 2207
		private const int _deathSprayAmount = 500;

		// Token: 0x040008A0 RID: 2208
		private const float _slamSprayLifetime = 0.75f;

		// Token: 0x040008A1 RID: 2209
		[Nullable(1)]
		private GpuParticles2D _droolParticles;

		// Token: 0x040008A2 RID: 2210
		[Nullable(1)]
		private GpuParticles2D _sprayParticles;

		// Token: 0x040008A3 RID: 2211
		[Nullable(1)]
		private GpuParticles2D _rainDropParticles;

		// Token: 0x040008A4 RID: 2212
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x040008A5 RID: 2213
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010D6 RID: 4310
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004205 RID: 16901
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004206 RID: 16902
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x04004207 RID: 16903
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'TurnOnSprayAttack' method.
			/// </summary>
			// Token: 0x04004208 RID: 16904
			public static readonly StringName TurnOnSprayAttack = "TurnOnSprayAttack";

			/// <summary>
			/// Cached name for the 'TurnOffSprayAttack' method.
			/// </summary>
			// Token: 0x04004209 RID: 16905
			public static readonly StringName TurnOffSprayAttack = "TurnOffSprayAttack";

			/// <summary>
			/// Cached name for the 'TurnOnSlamSpray' method.
			/// </summary>
			// Token: 0x0400420A RID: 16906
			public static readonly StringName TurnOnSlamSpray = "TurnOnSlamSpray";

			/// <summary>
			/// Cached name for the 'TurnOffSlamSpray' method.
			/// </summary>
			// Token: 0x0400420B RID: 16907
			public static readonly StringName TurnOffSlamSpray = "TurnOffSlamSpray";

			/// <summary>
			/// Cached name for the 'TurnOnDeathSpray' method.
			/// </summary>
			// Token: 0x0400420C RID: 16908
			public static readonly StringName TurnOnDeathSpray = "TurnOnDeathSpray";

			/// <summary>
			/// Cached name for the 'TurnOffDeathSpray' method.
			/// </summary>
			// Token: 0x0400420D RID: 16909
			public static readonly StringName TurnOffDeathSpray = "TurnOffDeathSpray";

			/// <summary>
			/// Cached name for the 'TurnOnDrool' method.
			/// </summary>
			// Token: 0x0400420E RID: 16910
			public static readonly StringName TurnOnDrool = "TurnOnDrool";

			/// <summary>
			/// Cached name for the 'TurnOffDrool' method.
			/// </summary>
			// Token: 0x0400420F RID: 16911
			public static readonly StringName TurnOffDrool = "TurnOffDrool";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010D7 RID: 4311
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_droolParticles' field.
			/// </summary>
			// Token: 0x04004210 RID: 16912
			public static readonly StringName _droolParticles = "_droolParticles";

			/// <summary>
			/// Cached name for the '_sprayParticles' field.
			/// </summary>
			// Token: 0x04004211 RID: 16913
			public static readonly StringName _sprayParticles = "_sprayParticles";

			/// <summary>
			/// Cached name for the '_rainDropParticles' field.
			/// </summary>
			// Token: 0x04004212 RID: 16914
			public static readonly StringName _rainDropParticles = "_rainDropParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004213 RID: 16915
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010D8 RID: 4312
		public class SignalName : Node.SignalName
		{
		}
	}
}
