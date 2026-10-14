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
	// Token: 0x02000205 RID: 517
	[NullableContext(1)]
	[Nullable(0)]
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NTheInsatiableVfx.cs")]
	public class NTheInsatiableVfx : Node
	{
		// Token: 0x06001AAD RID: 6829 RVA: 0x000AB654 File Offset: 0x000A9854
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._salivaFountainParticles = this._parent.GetNode<CpuParticles2D>("SalivaSlotNode/SalivaFountainParticles");
			this._salivaDroolParticles = this._parent.GetNode<CpuParticles2D>("SalivaSlotNode/SalivaDroolParticles");
			this._salivaCloudParticles = this._parent.GetNode<CpuParticles2D>("SalivaSlotNode/SalivaCloudParticles");
			this._baseBlastParticles = this._parent.GetNode<GpuParticles2D>("BaseBlastSlot/BaseBlastParticles");
			this._salivaFountainParticles.Emitting = false;
			this._salivaDroolParticles.Emitting = false;
			this._salivaCloudParticles.Emitting = false;
			this._baseBlastParticles.Emitting = false;
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x000AB75C File Offset: 0x000A995C
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName != null)
			{
				switch (eventName.Length)
				{
				case 9:
				{
					char c = eventName[1];
					if (c != 'e')
					{
						if (c != 'r')
						{
							return;
						}
						if (!(eventName == "drool_end"))
						{
							return;
						}
						this.TurnOffDrool();
						return;
					}
					else
					{
						if (!(eventName == "death_end"))
						{
							return;
						}
						this.TurnOffContinuousParticles();
					}
					break;
				}
				case 10:
					if (!(eventName == "saliva_end"))
					{
						return;
					}
					this.TurnOffSaliva();
					return;
				case 11:
					if (!(eventName == "drool_start"))
					{
						return;
					}
					this.TurnOnDrool();
					return;
				case 12:
					if (!(eventName == "saliva_start"))
					{
						return;
					}
					this.TurnOnSaliva();
					return;
				case 13:
				case 15:
					break;
				case 14:
					if (!(eventName == "base_blast_end"))
					{
						return;
					}
					this.TurnOffBaseBlast();
					return;
				case 16:
					if (!(eventName == "base_blast_start"))
					{
						return;
					}
					this.TurnOnBaseBlast();
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x000AB85C File Offset: 0x000A9A5C
		private void TurnOnSaliva()
		{
			this._salivaFountainParticles.Restart();
			this._salivaCloudParticles.Restart();
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x000AB874 File Offset: 0x000A9A74
		private void TurnOffSaliva()
		{
			this._salivaFountainParticles.Emitting = false;
			this._salivaCloudParticles.Emitting = false;
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x000AB88E File Offset: 0x000A9A8E
		private void TurnOnDrool()
		{
			this._salivaDroolParticles.Restart();
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x000AB89B File Offset: 0x000A9A9B
		private void TurnOffDrool()
		{
			this._salivaDroolParticles.Emitting = false;
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x000AB8A9 File Offset: 0x000A9AA9
		private void TurnOnBaseBlast()
		{
			this._baseBlastParticles.Emitting = true;
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x000AB8B7 File Offset: 0x000A9AB7
		private void TurnOffBaseBlast()
		{
			this._baseBlastParticles.Emitting = false;
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x000AB8C8 File Offset: 0x000A9AC8
		private void TurnOffContinuousParticles()
		{
			foreach (CpuParticles2D cpuParticles2D in this._continuousParticles)
			{
				cpuParticles2D.Emitting = false;
			}
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x000AB8F8 File Offset: 0x000A9AF8
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			string currentAnimationName = new MegaAnimationState(animationState).GetCurrentAnimationName(0);
			if (currentAnimationName != "attack_thrash")
			{
				this.TurnOffBaseBlast();
			}
			if (currentAnimationName != "salivate")
			{
				this.TurnOffSaliva();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001AB7 RID: 6839 RVA: 0x000AB940 File Offset: 0x000A9B40
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NTheInsatiableVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTheInsatiableVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NTheInsatiableVfx.MethodName.TurnOnSaliva, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTheInsatiableVfx.MethodName.TurnOffSaliva, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTheInsatiableVfx.MethodName.TurnOnDrool, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTheInsatiableVfx.MethodName.TurnOffDrool, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTheInsatiableVfx.MethodName.TurnOnBaseBlast, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTheInsatiableVfx.MethodName.TurnOffBaseBlast, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTheInsatiableVfx.MethodName.TurnOffContinuousParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTheInsatiableVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001AB8 RID: 6840 RVA: 0x000ABC68 File Offset: 0x000A9E68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTheInsatiableVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheInsatiableVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheInsatiableVfx.MethodName.TurnOnSaliva && args.Count == 0)
			{
				this.TurnOnSaliva();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheInsatiableVfx.MethodName.TurnOffSaliva && args.Count == 0)
			{
				this.TurnOffSaliva();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheInsatiableVfx.MethodName.TurnOnDrool && args.Count == 0)
			{
				this.TurnOnDrool();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheInsatiableVfx.MethodName.TurnOffDrool && args.Count == 0)
			{
				this.TurnOffDrool();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheInsatiableVfx.MethodName.TurnOnBaseBlast && args.Count == 0)
			{
				this.TurnOnBaseBlast();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheInsatiableVfx.MethodName.TurnOffBaseBlast && args.Count == 0)
			{
				this.TurnOffBaseBlast();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheInsatiableVfx.MethodName.TurnOffContinuousParticles && args.Count == 0)
			{
				this.TurnOffContinuousParticles();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTheInsatiableVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001AB9 RID: 6841 RVA: 0x000ABE50 File Offset: 0x000AA050
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTheInsatiableVfx.MethodName._Ready || (ref method) == NTheInsatiableVfx.MethodName.OnAnimationEvent || (ref method) == NTheInsatiableVfx.MethodName.TurnOnSaliva || (ref method) == NTheInsatiableVfx.MethodName.TurnOffSaliva || (ref method) == NTheInsatiableVfx.MethodName.TurnOnDrool || (ref method) == NTheInsatiableVfx.MethodName.TurnOffDrool || (ref method) == NTheInsatiableVfx.MethodName.TurnOnBaseBlast || (ref method) == NTheInsatiableVfx.MethodName.TurnOffBaseBlast || (ref method) == NTheInsatiableVfx.MethodName.TurnOffContinuousParticles || (ref method) == NTheInsatiableVfx.MethodName.OnAnimationStart || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001ABA RID: 6842 RVA: 0x000ABEFC File Offset: 0x000AA0FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTheInsatiableVfx.PropertyName._continuousParticles)
			{
				this._continuousParticles = VariantUtils.ConvertToSystemArrayOfGodotObject<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTheInsatiableVfx.PropertyName._salivaFountainParticles)
			{
				this._salivaFountainParticles = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTheInsatiableVfx.PropertyName._salivaDroolParticles)
			{
				this._salivaDroolParticles = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTheInsatiableVfx.PropertyName._salivaCloudParticles)
			{
				this._salivaCloudParticles = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTheInsatiableVfx.PropertyName._baseBlastParticles)
			{
				this._baseBlastParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NTheInsatiableVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001ABB RID: 6843 RVA: 0x000ABFB4 File Offset: 0x000AA1B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTheInsatiableVfx.PropertyName._continuousParticles)
			{
				GodotObject[] continuousParticles = this._continuousParticles;
				value = VariantUtils.CreateFromSystemArrayOfGodotObject(continuousParticles);
				return true;
			}
			if ((ref name) == NTheInsatiableVfx.PropertyName._salivaFountainParticles)
			{
				value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._salivaFountainParticles);
				return true;
			}
			if ((ref name) == NTheInsatiableVfx.PropertyName._salivaDroolParticles)
			{
				value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._salivaDroolParticles);
				return true;
			}
			if ((ref name) == NTheInsatiableVfx.PropertyName._salivaCloudParticles)
			{
				value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._salivaCloudParticles);
				return true;
			}
			if ((ref name) == NTheInsatiableVfx.PropertyName._baseBlastParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._baseBlastParticles);
				return true;
			}
			if ((ref name) == NTheInsatiableVfx.PropertyName._parent)
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
		// Token: 0x06001ABC RID: 6844 RVA: 0x000AC08C File Offset: 0x000AA28C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NTheInsatiableVfx.PropertyName._continuousParticles, 23L, "24/34:CPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NTheInsatiableVfx.PropertyName._salivaFountainParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTheInsatiableVfx.PropertyName._salivaDroolParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTheInsatiableVfx.PropertyName._salivaCloudParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTheInsatiableVfx.PropertyName._baseBlastParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTheInsatiableVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001ABD RID: 6845 RVA: 0x000AC168 File Offset: 0x000AA368
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName continuousParticles = NTheInsatiableVfx.PropertyName._continuousParticles;
			GodotObject[] continuousParticles2 = this._continuousParticles;
			info.AddProperty(continuousParticles, Variant.CreateFrom(continuousParticles2));
			info.AddProperty(NTheInsatiableVfx.PropertyName._salivaFountainParticles, Variant.From<CpuParticles2D>(ref this._salivaFountainParticles));
			info.AddProperty(NTheInsatiableVfx.PropertyName._salivaDroolParticles, Variant.From<CpuParticles2D>(ref this._salivaDroolParticles));
			info.AddProperty(NTheInsatiableVfx.PropertyName._salivaCloudParticles, Variant.From<CpuParticles2D>(ref this._salivaCloudParticles));
			info.AddProperty(NTheInsatiableVfx.PropertyName._baseBlastParticles, Variant.From<GpuParticles2D>(ref this._baseBlastParticles));
			info.AddProperty(NTheInsatiableVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001ABE RID: 6846 RVA: 0x000AC204 File Offset: 0x000AA404
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTheInsatiableVfx.PropertyName._continuousParticles, ref variant))
			{
				this._continuousParticles = variant.AsGodotObjectArray<CpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTheInsatiableVfx.PropertyName._salivaFountainParticles, ref variant2))
			{
				this._salivaFountainParticles = variant2.As<CpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTheInsatiableVfx.PropertyName._salivaDroolParticles, ref variant3))
			{
				this._salivaDroolParticles = variant3.As<CpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTheInsatiableVfx.PropertyName._salivaCloudParticles, ref variant4))
			{
				this._salivaCloudParticles = variant4.As<CpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTheInsatiableVfx.PropertyName._baseBlastParticles, ref variant5))
			{
				this._baseBlastParticles = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NTheInsatiableVfx.PropertyName._parent, ref variant6))
			{
				this._parent = variant6.As<Node2D>();
			}
		}

		// Token: 0x040009E8 RID: 2536
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private CpuParticles2D[] _continuousParticles;

		// Token: 0x040009E9 RID: 2537
		private CpuParticles2D _salivaFountainParticles;

		// Token: 0x040009EA RID: 2538
		private CpuParticles2D _salivaDroolParticles;

		// Token: 0x040009EB RID: 2539
		private CpuParticles2D _salivaCloudParticles;

		// Token: 0x040009EC RID: 2540
		private GpuParticles2D _baseBlastParticles;

		// Token: 0x040009ED RID: 2541
		private Node2D _parent;

		// Token: 0x040009EE RID: 2542
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001181 RID: 4481
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400443F RID: 17471
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004440 RID: 17472
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'TurnOnSaliva' method.
			/// </summary>
			// Token: 0x04004441 RID: 17473
			public static readonly StringName TurnOnSaliva = "TurnOnSaliva";

			/// <summary>
			/// Cached name for the 'TurnOffSaliva' method.
			/// </summary>
			// Token: 0x04004442 RID: 17474
			public static readonly StringName TurnOffSaliva = "TurnOffSaliva";

			/// <summary>
			/// Cached name for the 'TurnOnDrool' method.
			/// </summary>
			// Token: 0x04004443 RID: 17475
			public static readonly StringName TurnOnDrool = "TurnOnDrool";

			/// <summary>
			/// Cached name for the 'TurnOffDrool' method.
			/// </summary>
			// Token: 0x04004444 RID: 17476
			public static readonly StringName TurnOffDrool = "TurnOffDrool";

			/// <summary>
			/// Cached name for the 'TurnOnBaseBlast' method.
			/// </summary>
			// Token: 0x04004445 RID: 17477
			public static readonly StringName TurnOnBaseBlast = "TurnOnBaseBlast";

			/// <summary>
			/// Cached name for the 'TurnOffBaseBlast' method.
			/// </summary>
			// Token: 0x04004446 RID: 17478
			public static readonly StringName TurnOffBaseBlast = "TurnOffBaseBlast";

			/// <summary>
			/// Cached name for the 'TurnOffContinuousParticles' method.
			/// </summary>
			// Token: 0x04004447 RID: 17479
			public static readonly StringName TurnOffContinuousParticles = "TurnOffContinuousParticles";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x04004448 RID: 17480
			public static readonly StringName OnAnimationStart = "OnAnimationStart";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001182 RID: 4482
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_continuousParticles' field.
			/// </summary>
			// Token: 0x04004449 RID: 17481
			public static readonly StringName _continuousParticles = "_continuousParticles";

			/// <summary>
			/// Cached name for the '_salivaFountainParticles' field.
			/// </summary>
			// Token: 0x0400444A RID: 17482
			public static readonly StringName _salivaFountainParticles = "_salivaFountainParticles";

			/// <summary>
			/// Cached name for the '_salivaDroolParticles' field.
			/// </summary>
			// Token: 0x0400444B RID: 17483
			public static readonly StringName _salivaDroolParticles = "_salivaDroolParticles";

			/// <summary>
			/// Cached name for the '_salivaCloudParticles' field.
			/// </summary>
			// Token: 0x0400444C RID: 17484
			public static readonly StringName _salivaCloudParticles = "_salivaCloudParticles";

			/// <summary>
			/// Cached name for the '_baseBlastParticles' field.
			/// </summary>
			// Token: 0x0400444D RID: 17485
			public static readonly StringName _baseBlastParticles = "_baseBlastParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x0400444E RID: 17486
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001183 RID: 4483
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}
