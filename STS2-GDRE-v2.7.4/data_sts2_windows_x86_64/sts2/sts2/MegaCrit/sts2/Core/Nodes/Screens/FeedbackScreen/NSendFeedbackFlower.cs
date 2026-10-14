using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen
{
	// Token: 0x02000327 RID: 807
	[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NSendFeedbackFlower.cs")]
	public class NSendFeedbackFlower : Control
	{
		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002CFF RID: 11519 RVA: 0x00140F34 File Offset: 0x0013F134
		// (set) Token: 0x06002D00 RID: 11520 RVA: 0x00140F3C File Offset: 0x0013F13C
		[Nullable(1)]
		public NSendFeedbackCartoon Cartoon
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			private set;
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002D01 RID: 11521 RVA: 0x00140F45 File Offset: 0x0013F145
		// (set) Token: 0x06002D02 RID: 11522 RVA: 0x00140F4D File Offset: 0x0013F14D
		public NSendFeedbackFlower.State MyState { get; private set; }

		// Token: 0x06002D03 RID: 11523 RVA: 0x00140F56 File Offset: 0x0013F156
		public override void _Ready()
		{
			this._originalPosition = base.Position;
			this.Cartoon = base.GetNode<NSendFeedbackCartoon>("Flower");
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x00140F7C File Offset: 0x0013F17C
		public void SetState(NSendFeedbackFlower.State state)
		{
			if (state == NSendFeedbackFlower.State.Nodding)
			{
				Tween tween = this._tween;
				if (tween != null)
				{
					tween.Kill();
				}
				this._tween = base.CreateTween();
				this._tween.TweenProperty(this, "rotation", Mathf.DegToRad(8f), 0.5);
				this._tween.TweenProperty(this, "rotation", Mathf.DegToRad(-8f), 0.5);
				this._tween.SetLoops(0);
				this.Cartoon.Texture = PreloadManager.Cache.GetTexture2D("res://images/atlases/compressed.sprites/feedback/flower_happy.tres");
			}
			else if (state == NSendFeedbackFlower.State.NoddingFast)
			{
				Tween tween2 = this._tween;
				if (tween2 != null)
				{
					tween2.Kill();
				}
				this._tween = base.CreateTween();
				this._tween.TweenProperty(this, "rotation", Mathf.DegToRad(8f), 0.2);
				this._tween.TweenProperty(this, "rotation", Mathf.DegToRad(-8f), 0.2);
				this._tween.SetLoops(0);
				this.Cartoon.Texture = PreloadManager.Cache.GetTexture2D("res://images/atlases/compressed.sprites/feedback/flower_happy.tres");
			}
			else if (state == NSendFeedbackFlower.State.Anticipation)
			{
				Tween tween3 = this._tween;
				if (tween3 != null)
				{
					tween3.Kill();
				}
				this._tween = null;
				this.Cartoon.Texture = PreloadManager.Cache.GetTexture2D("res://images/atlases/compressed.sprites/feedback/flower_anticipation.tres");
				this._tween = base.CreateTween();
				this._tween.TweenInterval(0.05000000074505806);
				this._tween.TweenCallback(Callable.From(new Action(this.SetRandomPosition)));
				this._tween.SetLoops(0);
			}
			else
			{
				base.Rotation = 0f;
				Tween tween4 = this._tween;
				if (tween4 != null)
				{
					tween4.Kill();
				}
				this._tween = null;
				this.Cartoon.Texture = PreloadManager.Cache.GetTexture2D("res://images/atlases/compressed.sprites/feedback/flower.tres");
			}
			this.MyState = state;
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x001411AC File Offset: 0x0013F3AC
		private void SetRandomPosition()
		{
			base.Position = this._originalPosition + new Vector2(Rng.Chaotic.NextFloat(-3f, 3f), Rng.Chaotic.NextFloat(-3f, 3f));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D06 RID: 11526 RVA: 0x001411EC File Offset: 0x0013F3EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSendFeedbackFlower.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackFlower.MethodName.SetState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "state", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSendFeedbackFlower.MethodName.SetRandomPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D07 RID: 11527 RVA: 0x001412B4 File Offset: 0x0013F4B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSendFeedbackFlower.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackFlower.MethodName.SetState && args.Count == 1)
			{
				this.SetState(VariantUtils.ConvertTo<NSendFeedbackFlower.State>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackFlower.MethodName.SetRandomPosition && args.Count == 0)
			{
				this.SetRandomPosition();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002D08 RID: 11528 RVA: 0x00141347 File Offset: 0x0013F547
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSendFeedbackFlower.MethodName._Ready || (ref method) == NSendFeedbackFlower.MethodName.SetState || (ref method) == NSendFeedbackFlower.MethodName.SetRandomPosition || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002D09 RID: 11529 RVA: 0x00141380 File Offset: 0x0013F580
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSendFeedbackFlower.PropertyName.Cartoon)
			{
				this.Cartoon = VariantUtils.ConvertTo<NSendFeedbackCartoon>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackFlower.PropertyName.MyState)
			{
				this.MyState = VariantUtils.ConvertTo<NSendFeedbackFlower.State>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackFlower.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NSendFeedbackFlower.PropertyName._originalPosition)
			{
				this._originalPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002D0A RID: 11530 RVA: 0x00141404 File Offset: 0x0013F604
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSendFeedbackFlower.PropertyName.Cartoon)
			{
				NSendFeedbackCartoon cartoon = this.Cartoon;
				value = VariantUtils.CreateFrom<NSendFeedbackCartoon>(ref cartoon);
				return true;
			}
			if ((ref name) == NSendFeedbackFlower.PropertyName.MyState)
			{
				NSendFeedbackFlower.State myState = this.MyState;
				value = VariantUtils.CreateFrom<NSendFeedbackFlower.State>(ref myState);
				return true;
			}
			if ((ref name) == NSendFeedbackFlower.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NSendFeedbackFlower.PropertyName._originalPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._originalPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002D0B RID: 11531 RVA: 0x001414A0 File Offset: 0x0013F6A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSendFeedbackFlower.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NSendFeedbackFlower.PropertyName._originalPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSendFeedbackFlower.PropertyName.Cartoon, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSendFeedbackFlower.PropertyName.MyState, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002D0C RID: 11532 RVA: 0x00141538 File Offset: 0x0013F738
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName cartoon = NSendFeedbackFlower.PropertyName.Cartoon;
			NSendFeedbackCartoon cartoon2 = this.Cartoon;
			info.AddProperty(cartoon, Variant.From<NSendFeedbackCartoon>(ref cartoon2));
			StringName myState = NSendFeedbackFlower.PropertyName.MyState;
			NSendFeedbackFlower.State myState2 = this.MyState;
			info.AddProperty(myState, Variant.From<NSendFeedbackFlower.State>(ref myState2));
			info.AddProperty(NSendFeedbackFlower.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NSendFeedbackFlower.PropertyName._originalPosition, Variant.From<Vector2>(ref this._originalPosition));
		}

		/// <inheritdoc />
		// Token: 0x06002D0D RID: 11533 RVA: 0x001415AC File Offset: 0x0013F7AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSendFeedbackFlower.PropertyName.Cartoon, ref variant))
			{
				this.Cartoon = variant.As<NSendFeedbackCartoon>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSendFeedbackFlower.PropertyName.MyState, ref variant2))
			{
				this.MyState = variant2.As<NSendFeedbackFlower.State>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSendFeedbackFlower.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSendFeedbackFlower.PropertyName._originalPosition, ref variant4))
			{
				this._originalPosition = variant4.As<Vector2>();
			}
		}

		// Token: 0x04001110 RID: 4368
		[Nullable(1)]
		private const string _normalImage = "res://images/atlases/compressed.sprites/feedback/flower.tres";

		// Token: 0x04001111 RID: 4369
		[Nullable(1)]
		private const string _noddingImage = "res://images/atlases/compressed.sprites/feedback/flower_happy.tres";

		// Token: 0x04001112 RID: 4370
		[Nullable(1)]
		private const string _anticipationImage = "res://images/atlases/compressed.sprites/feedback/flower_anticipation.tres";

		// Token: 0x04001113 RID: 4371
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001114 RID: 4372
		private Vector2 _originalPosition;

		// Token: 0x0200157E RID: 5502
		public enum State
		{
			// Token: 0x04005245 RID: 21061
			None,
			// Token: 0x04005246 RID: 21062
			Nodding,
			// Token: 0x04005247 RID: 21063
			Anticipation,
			// Token: 0x04005248 RID: 21064
			NoddingFast
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200157F RID: 5503
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005249 RID: 21065
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetState' method.
			/// </summary>
			// Token: 0x0400524A RID: 21066
			public static readonly StringName SetState = "SetState";

			/// <summary>
			/// Cached name for the 'SetRandomPosition' method.
			/// </summary>
			// Token: 0x0400524B RID: 21067
			public static readonly StringName SetRandomPosition = "SetRandomPosition";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001580 RID: 5504
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Cartoon' property.
			/// </summary>
			// Token: 0x0400524C RID: 21068
			public static readonly StringName Cartoon = "Cartoon";

			/// <summary>
			/// Cached name for the 'MyState' property.
			/// </summary>
			// Token: 0x0400524D RID: 21069
			public static readonly StringName MyState = "MyState";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400524E RID: 21070
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_originalPosition' field.
			/// </summary>
			// Token: 0x0400524F RID: 21071
			public static readonly StringName _originalPosition = "_originalPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001581 RID: 5505
		public class SignalName : Control.SignalName
		{
		}
	}
}
