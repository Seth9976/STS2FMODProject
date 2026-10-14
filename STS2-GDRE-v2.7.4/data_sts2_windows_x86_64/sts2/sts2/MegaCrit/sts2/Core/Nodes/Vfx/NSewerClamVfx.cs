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
	// Token: 0x020001ED RID: 493
	[ScriptPath("res://src/Core/Nodes/Vfx/NSewerClamVfx.cs")]
	public class NSewerClamVfx : Node
	{
		// Token: 0x06001917 RID: 6423 RVA: 0x0009D4E0 File Offset: 0x0009B6E0
		public override void _Ready()
		{
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._deathParticles = base.GetParent().GetNode<GpuParticles2D>("MouthSlot/DeathParticles");
			this._buffParticles = base.GetParent().GetNode<GpuParticles2D>("MouthSlot/BuffParticles");
			this._chompParticles = base.GetParent().GetNode<GpuParticles2D>("MouthSlot/ChompParticles");
			this._scaleNode = base.GetParent().GetNode<Node2D>("CoralScaleBone");
			this._deathParticles.Emitting = false;
			this._deathParticles.OneShot = true;
			this._buffParticles.Emitting = false;
			this._chompParticles.OneShot = true;
			this._chompParticles.Emitting = false;
			this._scaleNode.Scale = new Vector2(0.1f, 0.1f);
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x0009D5E4 File Offset: 0x0009B7E4
		private void ScaleCoralTo(float targetScale)
		{
			Vector2 vector;
			vector..ctor(targetScale - 0.2f, targetScale - 0.2f);
			Vector2 vector2;
			vector2..ctor(targetScale, targetScale);
			Tween tween = base.CreateTween();
			tween.SetEase(1L).SetTrans(6L);
			tween.TweenProperty(this._scaleNode, "scale", vector2, 0.5).From(vector).SetDelay(0.5);
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x0009D668 File Offset: 0x0009B868
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "death_explode")
			{
				this.OnDeathStart();
				return;
			}
			if (eventName == "darkness_start")
			{
				this.OnDarknessStart();
				return;
			}
			if (eventName == "darkness_end")
			{
				this.OnDarknessEnd();
				return;
			}
			if (eventName == "chomp")
			{
				this.OnChomp();
				return;
			}
			if (!(eventName == "grow"))
			{
				return;
			}
			this.OnGrow();
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0009D6F0 File Offset: 0x0009B8F0
		private void OnDeathStart()
		{
			this._deathParticles.Restart();
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0009D6FD File Offset: 0x0009B8FD
		private void OnDeathEnd()
		{
			this._deathParticles.Emitting = false;
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0009D70B File Offset: 0x0009B90B
		private void OnDarknessStart()
		{
			this._buffParticles.Restart();
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x0009D718 File Offset: 0x0009B918
		private void OnDarknessEnd()
		{
			this._buffParticles.Emitting = false;
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x0009D726 File Offset: 0x0009B926
		private void OnChomp()
		{
			this._chompParticles.Restart();
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x0009D734 File Offset: 0x0009B934
		private void OnGrow()
		{
			float num = this._scaleNode.Scale.X + 0.2f;
			if (num >= 1.5f)
			{
				num = 1.5f;
			}
			this.ScaleCoralTo(num);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001920 RID: 6432 RVA: 0x0009D770 File Offset: 0x0009B970
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NSewerClamVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSewerClamVfx.MethodName.ScaleCoralTo, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "targetScale", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSewerClamVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NSewerClamVfx.MethodName.OnDeathStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSewerClamVfx.MethodName.OnDeathEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSewerClamVfx.MethodName.OnDarknessStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSewerClamVfx.MethodName.OnDarknessEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSewerClamVfx.MethodName.OnChomp, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSewerClamVfx.MethodName.OnGrow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001921 RID: 6433 RVA: 0x0009DA08 File Offset: 0x0009BC08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSewerClamVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSewerClamVfx.MethodName.ScaleCoralTo && args.Count == 1)
			{
				this.ScaleCoralTo(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSewerClamVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSewerClamVfx.MethodName.OnDeathStart && args.Count == 0)
			{
				this.OnDeathStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSewerClamVfx.MethodName.OnDeathEnd && args.Count == 0)
			{
				this.OnDeathEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSewerClamVfx.MethodName.OnDarknessStart && args.Count == 0)
			{
				this.OnDarknessStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSewerClamVfx.MethodName.OnDarknessEnd && args.Count == 0)
			{
				this.OnDarknessEnd();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSewerClamVfx.MethodName.OnChomp && args.Count == 0)
			{
				this.OnChomp();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSewerClamVfx.MethodName.OnGrow && args.Count == 0)
			{
				this.OnGrow();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001922 RID: 6434 RVA: 0x0009DBB0 File Offset: 0x0009BDB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSewerClamVfx.MethodName._Ready || (ref method) == NSewerClamVfx.MethodName.ScaleCoralTo || (ref method) == NSewerClamVfx.MethodName.OnAnimationEvent || (ref method) == NSewerClamVfx.MethodName.OnDeathStart || (ref method) == NSewerClamVfx.MethodName.OnDeathEnd || (ref method) == NSewerClamVfx.MethodName.OnDarknessStart || (ref method) == NSewerClamVfx.MethodName.OnDarknessEnd || (ref method) == NSewerClamVfx.MethodName.OnChomp || (ref method) == NSewerClamVfx.MethodName.OnGrow || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001923 RID: 6435 RVA: 0x0009DC4C File Offset: 0x0009BE4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSewerClamVfx.PropertyName._deathParticles)
			{
				this._deathParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSewerClamVfx.PropertyName._buffParticles)
			{
				this._buffParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSewerClamVfx.PropertyName._chompParticles)
			{
				this._chompParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSewerClamVfx.PropertyName._scaleNode)
			{
				this._scaleNode = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NSewerClamVfx.PropertyName._keyDown)
			{
				this._keyDown = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSewerClamVfx.PropertyName._onState)
			{
				this._onState = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001924 RID: 6436 RVA: 0x0009DD04 File Offset: 0x0009BF04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSewerClamVfx.PropertyName._deathParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathParticles);
				return true;
			}
			if ((ref name) == NSewerClamVfx.PropertyName._buffParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._buffParticles);
				return true;
			}
			if ((ref name) == NSewerClamVfx.PropertyName._chompParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._chompParticles);
				return true;
			}
			if ((ref name) == NSewerClamVfx.PropertyName._scaleNode)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._scaleNode);
				return true;
			}
			if ((ref name) == NSewerClamVfx.PropertyName._keyDown)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._keyDown);
				return true;
			}
			if ((ref name) == NSewerClamVfx.PropertyName._onState)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._onState);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001925 RID: 6437 RVA: 0x0009DDDC File Offset: 0x0009BFDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSewerClamVfx.PropertyName._deathParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSewerClamVfx.PropertyName._buffParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSewerClamVfx.PropertyName._chompParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSewerClamVfx.PropertyName._scaleNode, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSewerClamVfx.PropertyName._keyDown, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSewerClamVfx.PropertyName._onState, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001926 RID: 6438 RVA: 0x0009DEB4 File Offset: 0x0009C0B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSewerClamVfx.PropertyName._deathParticles, Variant.From<GpuParticles2D>(ref this._deathParticles));
			info.AddProperty(NSewerClamVfx.PropertyName._buffParticles, Variant.From<GpuParticles2D>(ref this._buffParticles));
			info.AddProperty(NSewerClamVfx.PropertyName._chompParticles, Variant.From<GpuParticles2D>(ref this._chompParticles));
			info.AddProperty(NSewerClamVfx.PropertyName._scaleNode, Variant.From<Node2D>(ref this._scaleNode));
			info.AddProperty(NSewerClamVfx.PropertyName._keyDown, Variant.From<bool>(ref this._keyDown));
			info.AddProperty(NSewerClamVfx.PropertyName._onState, Variant.From<bool>(ref this._onState));
		}

		/// <inheritdoc />
		// Token: 0x06001927 RID: 6439 RVA: 0x0009DF4C File Offset: 0x0009C14C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSewerClamVfx.PropertyName._deathParticles, ref variant))
			{
				this._deathParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSewerClamVfx.PropertyName._buffParticles, ref variant2))
			{
				this._buffParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSewerClamVfx.PropertyName._chompParticles, ref variant3))
			{
				this._chompParticles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSewerClamVfx.PropertyName._scaleNode, ref variant4))
			{
				this._scaleNode = variant4.As<Node2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSewerClamVfx.PropertyName._keyDown, ref variant5))
			{
				this._keyDown = variant5.As<bool>();
			}
			Variant variant6;
			if (info.TryGetProperty(NSewerClamVfx.PropertyName._onState, ref variant6))
			{
				this._onState = variant6.As<bool>();
			}
		}

		// Token: 0x04000920 RID: 2336
		private const float _coralScaleAmount = 0.2f;

		// Token: 0x04000921 RID: 2337
		private const float _maxCoralScale = 1.5f;

		// Token: 0x04000922 RID: 2338
		private const float _coralTweenDelay = 0.5f;

		// Token: 0x04000923 RID: 2339
		[Nullable(1)]
		private MegaSprite _megaSprite;

		// Token: 0x04000924 RID: 2340
		[Nullable(1)]
		private GpuParticles2D _deathParticles;

		// Token: 0x04000925 RID: 2341
		[Nullable(1)]
		private GpuParticles2D _buffParticles;

		// Token: 0x04000926 RID: 2342
		[Nullable(1)]
		private GpuParticles2D _chompParticles;

		// Token: 0x04000927 RID: 2343
		[Nullable(1)]
		private Node2D _scaleNode;

		// Token: 0x04000928 RID: 2344
		private bool _keyDown;

		// Token: 0x04000929 RID: 2345
		private bool _onState;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001124 RID: 4388
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040042E2 RID: 17122
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ScaleCoralTo' method.
			/// </summary>
			// Token: 0x040042E3 RID: 17123
			public static readonly StringName ScaleCoralTo = "ScaleCoralTo";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040042E4 RID: 17124
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnDeathStart' method.
			/// </summary>
			// Token: 0x040042E5 RID: 17125
			public static readonly StringName OnDeathStart = "OnDeathStart";

			/// <summary>
			/// Cached name for the 'OnDeathEnd' method.
			/// </summary>
			// Token: 0x040042E6 RID: 17126
			public static readonly StringName OnDeathEnd = "OnDeathEnd";

			/// <summary>
			/// Cached name for the 'OnDarknessStart' method.
			/// </summary>
			// Token: 0x040042E7 RID: 17127
			public static readonly StringName OnDarknessStart = "OnDarknessStart";

			/// <summary>
			/// Cached name for the 'OnDarknessEnd' method.
			/// </summary>
			// Token: 0x040042E8 RID: 17128
			public static readonly StringName OnDarknessEnd = "OnDarknessEnd";

			/// <summary>
			/// Cached name for the 'OnChomp' method.
			/// </summary>
			// Token: 0x040042E9 RID: 17129
			public static readonly StringName OnChomp = "OnChomp";

			/// <summary>
			/// Cached name for the 'OnGrow' method.
			/// </summary>
			// Token: 0x040042EA RID: 17130
			public static readonly StringName OnGrow = "OnGrow";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001125 RID: 4389
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_deathParticles' field.
			/// </summary>
			// Token: 0x040042EB RID: 17131
			public static readonly StringName _deathParticles = "_deathParticles";

			/// <summary>
			/// Cached name for the '_buffParticles' field.
			/// </summary>
			// Token: 0x040042EC RID: 17132
			public static readonly StringName _buffParticles = "_buffParticles";

			/// <summary>
			/// Cached name for the '_chompParticles' field.
			/// </summary>
			// Token: 0x040042ED RID: 17133
			public static readonly StringName _chompParticles = "_chompParticles";

			/// <summary>
			/// Cached name for the '_scaleNode' field.
			/// </summary>
			// Token: 0x040042EE RID: 17134
			public static readonly StringName _scaleNode = "_scaleNode";

			/// <summary>
			/// Cached name for the '_keyDown' field.
			/// </summary>
			// Token: 0x040042EF RID: 17135
			public static readonly StringName _keyDown = "_keyDown";

			/// <summary>
			/// Cached name for the '_onState' field.
			/// </summary>
			// Token: 0x040042F0 RID: 17136
			public static readonly StringName _onState = "_onState";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001126 RID: 4390
		public class SignalName : Node.SignalName
		{
		}
	}
}
