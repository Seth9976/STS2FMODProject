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
	// Token: 0x020001B0 RID: 432
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NFuzzyWurmCrawlerSpitTrailVfx.cs")]
	public class NFuzzyWurmCrawlerSpitTrailVfx : Node
	{
		// Token: 0x06001564 RID: 5476 RVA: 0x000829DC File Offset: 0x00080BDC
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._trailParticles = this._parent.GetNode<CpuParticles2D>("SpitParticlesBone/TrailParticles");
			this._trailParticles.Emitting = false;
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00082A6C File Offset: 0x00080C6C
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "launch_start")
			{
				this.TurnOnTrail();
				return;
			}
			if (!(eventName == "launch_end"))
			{
				return;
			}
			this.TurnOffTrail();
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00082AB8 File Offset: 0x00080CB8
		[NullableContext(1)]
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			if (new MegaAnimationState(animationState).GetCurrentAnimationName(0) != "attack")
			{
				this.TurnOffTrail();
			}
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x00082ADD File Offset: 0x00080CDD
		private void TurnOnTrail()
		{
			this._trailParticles.Restart();
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x00082AEA File Offset: 0x00080CEA
		private void TurnOffTrail()
		{
			this._trailParticles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001569 RID: 5481 RVA: 0x00082AF8 File Offset: 0x00080CF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NFuzzyWurmCrawlerSpitTrailVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFuzzyWurmCrawlerSpitTrailVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NFuzzyWurmCrawlerSpitTrailVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NFuzzyWurmCrawlerSpitTrailVfx.MethodName.TurnOnTrail, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFuzzyWurmCrawlerSpitTrailVfx.MethodName.TurnOffTrail, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600156A RID: 5482 RVA: 0x00082D34 File Offset: 0x00080F34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFuzzyWurmCrawlerSpitTrailVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFuzzyWurmCrawlerSpitTrailVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFuzzyWurmCrawlerSpitTrailVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFuzzyWurmCrawlerSpitTrailVfx.MethodName.TurnOnTrail && args.Count == 0)
			{
				this.TurnOnTrail();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFuzzyWurmCrawlerSpitTrailVfx.MethodName.TurnOffTrail && args.Count == 0)
			{
				this.TurnOffTrail();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600156B RID: 5483 RVA: 0x00082E60 File Offset: 0x00081060
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFuzzyWurmCrawlerSpitTrailVfx.MethodName._Ready || (ref method) == NFuzzyWurmCrawlerSpitTrailVfx.MethodName.OnAnimationEvent || (ref method) == NFuzzyWurmCrawlerSpitTrailVfx.MethodName.OnAnimationStart || (ref method) == NFuzzyWurmCrawlerSpitTrailVfx.MethodName.TurnOnTrail || (ref method) == NFuzzyWurmCrawlerSpitTrailVfx.MethodName.TurnOffTrail || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600156C RID: 5484 RVA: 0x00082EC0 File Offset: 0x000810C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._isKeyDown)
			{
				this._isKeyDown = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._trailParticles)
			{
				this._trailParticles = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600156D RID: 5485 RVA: 0x00082F28 File Offset: 0x00081128
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._isKeyDown)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isKeyDown);
				return true;
			}
			if ((ref name) == NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._trailParticles)
			{
				value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._trailParticles);
				return true;
			}
			if ((ref name) == NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._parent)
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
		// Token: 0x0600156E RID: 5486 RVA: 0x00082FA0 File Offset: 0x000811A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._isKeyDown, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._trailParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600156F RID: 5487 RVA: 0x00083018 File Offset: 0x00081218
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._isKeyDown, Variant.From<bool>(ref this._isKeyDown));
			info.AddProperty(NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._trailParticles, Variant.From<CpuParticles2D>(ref this._trailParticles));
			info.AddProperty(NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001570 RID: 5488 RVA: 0x00083070 File Offset: 0x00081270
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._isKeyDown, ref variant))
			{
				this._isKeyDown = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._trailParticles, ref variant2))
			{
				this._trailParticles = variant2.As<CpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NFuzzyWurmCrawlerSpitTrailVfx.PropertyName._parent, ref variant3))
			{
				this._parent = variant3.As<Node2D>();
			}
		}

		// Token: 0x0400079A RID: 1946
		private bool _isKeyDown;

		// Token: 0x0400079B RID: 1947
		[Nullable(1)]
		private CpuParticles2D _trailParticles;

		// Token: 0x0400079C RID: 1948
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x0400079D RID: 1949
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001040 RID: 4160
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400402E RID: 16430
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x0400402F RID: 16431
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x04004030 RID: 16432
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'TurnOnTrail' method.
			/// </summary>
			// Token: 0x04004031 RID: 16433
			public static readonly StringName TurnOnTrail = "TurnOnTrail";

			/// <summary>
			/// Cached name for the 'TurnOffTrail' method.
			/// </summary>
			// Token: 0x04004032 RID: 16434
			public static readonly StringName TurnOffTrail = "TurnOffTrail";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001041 RID: 4161
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_isKeyDown' field.
			/// </summary>
			// Token: 0x04004033 RID: 16435
			public static readonly StringName _isKeyDown = "_isKeyDown";

			/// <summary>
			/// Cached name for the '_trailParticles' field.
			/// </summary>
			// Token: 0x04004034 RID: 16436
			public static readonly StringName _trailParticles = "_trailParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004035 RID: 16437
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001042 RID: 4162
		public class SignalName : Node.SignalName
		{
		}
	}
}
