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
	// Token: 0x020001B8 RID: 440
	[ScriptPath("res://src/Core/Nodes/Vfx/NHauntedShipVfx.cs")]
	public class NHauntedShipVfx : Node
	{
		// Token: 0x060015E3 RID: 5603 RVA: 0x00085A48 File Offset: 0x00083C48
		public override void _Ready()
		{
			this._eyeParticles1 = base.GetNode<GpuParticles2D>("../EyeBone1/BubbleParticles");
			this._eyeParticles2 = base.GetNode<GpuParticles2D>("../EyeBone2/BubbleParticles");
			this._eyeParticles3 = base.GetNode<GpuParticles2D>("../EyeBone3/BubbleParticles");
			this._headParticles1 = base.GetNode<GpuParticles2D>("../HeadSlot/BubbleParticles");
			this._headParticles2 = base.GetNode<GpuParticles2D>("../HeadSlot/BubbleParticles2");
			this._eyeParticles1.Emitting = false;
			this._eyeParticles2.Emitting = false;
			this._eyeParticles3.Emitting = false;
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00085B1C File Offset: 0x00083D1C
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "eye_bubbles_start")
			{
				this.OnEyeBubblesStart();
				return;
			}
			if (eventName == "eye_bubbles_end")
			{
				this.OnEyeBubblesEnd();
				return;
			}
			if (eventName == "head_bubbles_start")
			{
				this.OnHeadBubblesStart();
				return;
			}
			if (!(eventName == "head_bubbles_end"))
			{
				return;
			}
			this.OnHeadBubblesEnd();
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x00085B90 File Offset: 0x00083D90
		private void OnEyeBubblesStart()
		{
			this._eyeParticles1.Emitting = true;
			this._eyeParticles2.Emitting = true;
			this._eyeParticles3.Emitting = true;
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00085BB6 File Offset: 0x00083DB6
		private void OnEyeBubblesEnd()
		{
			this._eyeParticles1.Emitting = false;
			this._eyeParticles2.Emitting = false;
			this._eyeParticles3.Emitting = false;
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00085BDC File Offset: 0x00083DDC
		private void OnHeadBubblesStart()
		{
			this._headParticles1.Emitting = true;
			this._headParticles2.Emitting = true;
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x00085BF6 File Offset: 0x00083DF6
		private void OnHeadBubblesEnd()
		{
			this._headParticles1.Emitting = false;
			this._headParticles2.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060015E9 RID: 5609 RVA: 0x00085C10 File Offset: 0x00083E10
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NHauntedShipVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHauntedShipVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NHauntedShipVfx.MethodName.OnEyeBubblesStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHauntedShipVfx.MethodName.OnEyeBubblesEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHauntedShipVfx.MethodName.OnHeadBubblesStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHauntedShipVfx.MethodName.OnHeadBubblesEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015EA RID: 5610 RVA: 0x00085DF4 File Offset: 0x00083FF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHauntedShipVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHauntedShipVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHauntedShipVfx.MethodName.OnEyeBubblesStart && args.Count == 0)
			{
				this.OnEyeBubblesStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHauntedShipVfx.MethodName.OnEyeBubblesEnd && args.Count == 0)
			{
				this.OnEyeBubblesEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHauntedShipVfx.MethodName.OnHeadBubblesStart && args.Count == 0)
			{
				this.OnHeadBubblesStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHauntedShipVfx.MethodName.OnHeadBubblesEnd && args.Count == 0)
			{
				this.OnHeadBubblesEnd();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060015EB RID: 5611 RVA: 0x00085F20 File Offset: 0x00084120
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHauntedShipVfx.MethodName._Ready || (ref method) == NHauntedShipVfx.MethodName.OnAnimationEvent || (ref method) == NHauntedShipVfx.MethodName.OnEyeBubblesStart || (ref method) == NHauntedShipVfx.MethodName.OnEyeBubblesEnd || (ref method) == NHauntedShipVfx.MethodName.OnHeadBubblesStart || (ref method) == NHauntedShipVfx.MethodName.OnHeadBubblesEnd || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060015EC RID: 5612 RVA: 0x00085F90 File Offset: 0x00084190
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHauntedShipVfx.PropertyName._eyeParticles1)
			{
				this._eyeParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHauntedShipVfx.PropertyName._eyeParticles2)
			{
				this._eyeParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHauntedShipVfx.PropertyName._eyeParticles3)
			{
				this._eyeParticles3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHauntedShipVfx.PropertyName._headParticles1)
			{
				this._headParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHauntedShipVfx.PropertyName._headParticles2)
			{
				this._headParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060015ED RID: 5613 RVA: 0x0008602C File Offset: 0x0008422C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHauntedShipVfx.PropertyName._eyeParticles1)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._eyeParticles1);
				return true;
			}
			if ((ref name) == NHauntedShipVfx.PropertyName._eyeParticles2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._eyeParticles2);
				return true;
			}
			if ((ref name) == NHauntedShipVfx.PropertyName._eyeParticles3)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._eyeParticles3);
				return true;
			}
			if ((ref name) == NHauntedShipVfx.PropertyName._headParticles1)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._headParticles1);
				return true;
			}
			if ((ref name) == NHauntedShipVfx.PropertyName._headParticles2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._headParticles2);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060015EE RID: 5614 RVA: 0x000860E4 File Offset: 0x000842E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NHauntedShipVfx.PropertyName._eyeParticles1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHauntedShipVfx.PropertyName._eyeParticles2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHauntedShipVfx.PropertyName._eyeParticles3, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHauntedShipVfx.PropertyName._headParticles1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHauntedShipVfx.PropertyName._headParticles2, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015EF RID: 5615 RVA: 0x000861A0 File Offset: 0x000843A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHauntedShipVfx.PropertyName._eyeParticles1, Variant.From<GpuParticles2D>(ref this._eyeParticles1));
			info.AddProperty(NHauntedShipVfx.PropertyName._eyeParticles2, Variant.From<GpuParticles2D>(ref this._eyeParticles2));
			info.AddProperty(NHauntedShipVfx.PropertyName._eyeParticles3, Variant.From<GpuParticles2D>(ref this._eyeParticles3));
			info.AddProperty(NHauntedShipVfx.PropertyName._headParticles1, Variant.From<GpuParticles2D>(ref this._headParticles1));
			info.AddProperty(NHauntedShipVfx.PropertyName._headParticles2, Variant.From<GpuParticles2D>(ref this._headParticles2));
		}

		/// <inheritdoc />
		// Token: 0x060015F0 RID: 5616 RVA: 0x00086224 File Offset: 0x00084424
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHauntedShipVfx.PropertyName._eyeParticles1, ref variant))
			{
				this._eyeParticles1 = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHauntedShipVfx.PropertyName._eyeParticles2, ref variant2))
			{
				this._eyeParticles2 = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHauntedShipVfx.PropertyName._eyeParticles3, ref variant3))
			{
				this._eyeParticles3 = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NHauntedShipVfx.PropertyName._headParticles1, ref variant4))
			{
				this._headParticles1 = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NHauntedShipVfx.PropertyName._headParticles2, ref variant5))
			{
				this._headParticles2 = variant5.As<GpuParticles2D>();
			}
		}

		// Token: 0x040007C8 RID: 1992
		[Nullable(1)]
		private MegaSprite _megaSprite;

		// Token: 0x040007C9 RID: 1993
		[Nullable(1)]
		private GpuParticles2D _eyeParticles1;

		// Token: 0x040007CA RID: 1994
		[Nullable(1)]
		private GpuParticles2D _eyeParticles2;

		// Token: 0x040007CB RID: 1995
		[Nullable(1)]
		private GpuParticles2D _eyeParticles3;

		// Token: 0x040007CC RID: 1996
		[Nullable(1)]
		private GpuParticles2D _headParticles1;

		// Token: 0x040007CD RID: 1997
		[Nullable(1)]
		private GpuParticles2D _headParticles2;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200105E RID: 4190
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004080 RID: 16512
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004081 RID: 16513
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnEyeBubblesStart' method.
			/// </summary>
			// Token: 0x04004082 RID: 16514
			public static readonly StringName OnEyeBubblesStart = "OnEyeBubblesStart";

			/// <summary>
			/// Cached name for the 'OnEyeBubblesEnd' method.
			/// </summary>
			// Token: 0x04004083 RID: 16515
			public static readonly StringName OnEyeBubblesEnd = "OnEyeBubblesEnd";

			/// <summary>
			/// Cached name for the 'OnHeadBubblesStart' method.
			/// </summary>
			// Token: 0x04004084 RID: 16516
			public static readonly StringName OnHeadBubblesStart = "OnHeadBubblesStart";

			/// <summary>
			/// Cached name for the 'OnHeadBubblesEnd' method.
			/// </summary>
			// Token: 0x04004085 RID: 16517
			public static readonly StringName OnHeadBubblesEnd = "OnHeadBubblesEnd";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200105F RID: 4191
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_eyeParticles1' field.
			/// </summary>
			// Token: 0x04004086 RID: 16518
			public static readonly StringName _eyeParticles1 = "_eyeParticles1";

			/// <summary>
			/// Cached name for the '_eyeParticles2' field.
			/// </summary>
			// Token: 0x04004087 RID: 16519
			public static readonly StringName _eyeParticles2 = "_eyeParticles2";

			/// <summary>
			/// Cached name for the '_eyeParticles3' field.
			/// </summary>
			// Token: 0x04004088 RID: 16520
			public static readonly StringName _eyeParticles3 = "_eyeParticles3";

			/// <summary>
			/// Cached name for the '_headParticles1' field.
			/// </summary>
			// Token: 0x04004089 RID: 16521
			public static readonly StringName _headParticles1 = "_headParticles1";

			/// <summary>
			/// Cached name for the '_headParticles2' field.
			/// </summary>
			// Token: 0x0400408A RID: 16522
			public static readonly StringName _headParticles2 = "_headParticles2";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001060 RID: 4192
		public class SignalName : Node.SignalName
		{
		}
	}
}
