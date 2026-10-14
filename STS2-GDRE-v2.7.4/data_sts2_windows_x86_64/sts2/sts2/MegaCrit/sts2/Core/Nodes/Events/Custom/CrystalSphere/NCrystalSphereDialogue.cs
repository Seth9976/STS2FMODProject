using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere
{
	// Token: 0x020003C1 RID: 961
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Events/Custom/CrystalSphere/NCrystalSphereDialogue.cs")]
	public class NCrystalSphereDialogue : Node2D
	{
		// Token: 0x06003A12 RID: 14866 RVA: 0x001B92C0 File Offset: 0x001B74C0
		public override void _Ready()
		{
			this._label = base.GetNode<MegaRichTextLabel>("%Text");
			this._dialogueBox = base.GetNode<Node2D>("%DialogueBox");
			this._bubble = base.GetNode<Sprite2D>("%Bubble");
			base.Modulate = Colors.Transparent;
			this._hsv = (ShaderMaterial)this._bubble.Material;
			this._hsv.SetShaderParameter(NCrystalSphereDialogue._h, 0.25f);
			this._hsv.SetShaderParameter(NCrystalSphereDialogue._s, 0.75f);
			this._hsv.SetShaderParameter(NCrystalSphereDialogue._v, 1f);
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x001B937E File Offset: 0x001B757E
		public void PlayStart()
		{
			this.Play(Rng.Chaotic.NextItem<LocString>(NCrystalSphereDialogue._startLines));
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x001B9395 File Offset: 0x001B7595
		public void PlayBad()
		{
			this.Play(Rng.Chaotic.NextItem<LocString>(NCrystalSphereDialogue._revealBadLines));
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x001B93AC File Offset: 0x001B75AC
		public void PlayGood()
		{
			this.Play(Rng.Chaotic.NextItem<LocString>(NCrystalSphereDialogue._revealGoodLines));
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x001B93C3 File Offset: 0x001B75C3
		public void PlayEnd()
		{
			this.Play(Rng.Chaotic.NextItem<LocString>(NCrystalSphereDialogue._endLines));
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x001B93DC File Offset: 0x001B75DC
		private void Play(LocString locString)
		{
			this._label.Text = "[fly_in]" + locString.GetFormattedText() + "[/fly_in]";
			Log.Info(this._label.Text ?? "", 2);
			base.Modulate = StsColors.transparentWhite;
			base.Position = new Vector2(Rng.Chaotic.NextFloat(NCrystalSphereDialogue._xRange.X, NCrystalSphereDialogue._xRange.Y), base.Position.Y);
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.25);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.25).SetEase(1L).SetTrans(10L);
			this._tween.TweenProperty(this._label, "visible_ratio", 1f, 0.4).From(0f);
			this._tween.TweenProperty(this._bubble, "scale", new Vector2(0.75f, 0.75f), 0.5).From(new Vector2(0.25f, 0.25f)).SetEase(1L)
				.SetTrans(5L);
			this._tween.TweenProperty(this._dialogueBox, "position:y", 0f, 0.5).From(-80f).SetEase(1L)
				.SetTrans(10L);
			this._tween.Chain();
			this._tween.TweenInterval(1.5);
			this._tween.Chain();
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.5).SetEase(1L).SetTrans(1L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A18 RID: 14872 RVA: 0x001B9638 File Offset: 0x001B7838
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NCrystalSphereDialogue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereDialogue.MethodName.PlayStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereDialogue.MethodName.PlayBad, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereDialogue.MethodName.PlayGood, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereDialogue.MethodName.PlayEnd, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A19 RID: 14873 RVA: 0x001B9738 File Offset: 0x001B7938
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCrystalSphereDialogue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereDialogue.MethodName.PlayStart && args.Count == 0)
			{
				this.PlayStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereDialogue.MethodName.PlayBad && args.Count == 0)
			{
				this.PlayBad();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereDialogue.MethodName.PlayGood && args.Count == 0)
			{
				this.PlayGood();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereDialogue.MethodName.PlayEnd && args.Count == 0)
			{
				this.PlayEnd();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003A1A RID: 14874 RVA: 0x001B9808 File Offset: 0x001B7A08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCrystalSphereDialogue.MethodName._Ready || (ref method) == NCrystalSphereDialogue.MethodName.PlayStart || (ref method) == NCrystalSphereDialogue.MethodName.PlayBad || (ref method) == NCrystalSphereDialogue.MethodName.PlayGood || (ref method) == NCrystalSphereDialogue.MethodName.PlayEnd || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003A1B RID: 14875 RVA: 0x001B9868 File Offset: 0x001B7A68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCrystalSphereDialogue.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereDialogue.PropertyName._dialogueBox)
			{
				this._dialogueBox = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereDialogue.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereDialogue.PropertyName._bubble)
			{
				this._bubble = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereDialogue.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003A1C RID: 14876 RVA: 0x001B9904 File Offset: 0x001B7B04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCrystalSphereDialogue.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NCrystalSphereDialogue.PropertyName._dialogueBox)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._dialogueBox);
				return true;
			}
			if ((ref name) == NCrystalSphereDialogue.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NCrystalSphereDialogue.PropertyName._bubble)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._bubble);
				return true;
			}
			if ((ref name) == NCrystalSphereDialogue.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A1D RID: 14877 RVA: 0x001B99BC File Offset: 0x001B7BBC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCrystalSphereDialogue.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereDialogue.PropertyName._dialogueBox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereDialogue.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereDialogue.PropertyName._bubble, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereDialogue.PropertyName._hsv, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A1E RID: 14878 RVA: 0x001B9A78 File Offset: 0x001B7C78
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCrystalSphereDialogue.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NCrystalSphereDialogue.PropertyName._dialogueBox, Variant.From<Node2D>(ref this._dialogueBox));
			info.AddProperty(NCrystalSphereDialogue.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NCrystalSphereDialogue.PropertyName._bubble, Variant.From<Sprite2D>(ref this._bubble));
			info.AddProperty(NCrystalSphereDialogue.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
		}

		/// <inheritdoc />
		// Token: 0x06003A1F RID: 14879 RVA: 0x001B9AFC File Offset: 0x001B7CFC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCrystalSphereDialogue.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCrystalSphereDialogue.PropertyName._dialogueBox, ref variant2))
			{
				this._dialogueBox = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCrystalSphereDialogue.PropertyName._tween, ref variant3))
			{
				this._tween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCrystalSphereDialogue.PropertyName._bubble, ref variant4))
			{
				this._bubble = variant4.As<Sprite2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCrystalSphereDialogue.PropertyName._hsv, ref variant5))
			{
				this._hsv = variant5.As<ShaderMaterial>();
			}
		}

		// Token: 0x0400169D RID: 5789
		private static readonly StringName _v = new StringName("v");

		// Token: 0x0400169E RID: 5790
		private static readonly StringName _s = new StringName("s");

		// Token: 0x0400169F RID: 5791
		private static readonly StringName _h = new StringName("h");

		// Token: 0x040016A0 RID: 5792
		private static readonly Vector2 _xRange = new Vector2(1100f, 1200f);

		// Token: 0x040016A1 RID: 5793
		private static readonly LocString[] _startLines = new LocString[]
		{
			new LocString("events", "CRYSTAL_SPHERE.banter.START.1"),
			new LocString("events", "CRYSTAL_SPHERE.banter.START.2")
		};

		// Token: 0x040016A2 RID: 5794
		private static readonly LocString[] _revealBadLines = new LocString[]
		{
			new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_BAD.1"),
			new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_BAD.2"),
			new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_BAD.3")
		};

		// Token: 0x040016A3 RID: 5795
		private static readonly LocString[] _revealGoodLines = new LocString[]
		{
			new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_GOOD.1"),
			new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_GOOD.2"),
			new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_GOOD.3"),
			new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_GOOD.4"),
			new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_GOOD.5")
		};

		// Token: 0x040016A4 RID: 5796
		private static readonly LocString[] _endLines = new LocString[]
		{
			new LocString("events", "CRYSTAL_SPHERE.banter.END.1"),
			new LocString("events", "CRYSTAL_SPHERE.banter.END.2")
		};

		// Token: 0x040016A5 RID: 5797
		private MegaRichTextLabel _label;

		// Token: 0x040016A6 RID: 5798
		private Node2D _dialogueBox;

		// Token: 0x040016A7 RID: 5799
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040016A8 RID: 5800
		private Sprite2D _bubble;

		// Token: 0x040016A9 RID: 5801
		private ShaderMaterial _hsv;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017E4 RID: 6116
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005D45 RID: 23877
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'PlayStart' method.
			/// </summary>
			// Token: 0x04005D46 RID: 23878
			public static readonly StringName PlayStart = "PlayStart";

			/// <summary>
			/// Cached name for the 'PlayBad' method.
			/// </summary>
			// Token: 0x04005D47 RID: 23879
			public static readonly StringName PlayBad = "PlayBad";

			/// <summary>
			/// Cached name for the 'PlayGood' method.
			/// </summary>
			// Token: 0x04005D48 RID: 23880
			public static readonly StringName PlayGood = "PlayGood";

			/// <summary>
			/// Cached name for the 'PlayEnd' method.
			/// </summary>
			// Token: 0x04005D49 RID: 23881
			public static readonly StringName PlayEnd = "PlayEnd";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017E5 RID: 6117
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005D4A RID: 23882
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_dialogueBox' field.
			/// </summary>
			// Token: 0x04005D4B RID: 23883
			public static readonly StringName _dialogueBox = "_dialogueBox";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005D4C RID: 23884
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_bubble' field.
			/// </summary>
			// Token: 0x04005D4D RID: 23885
			public static readonly StringName _bubble = "_bubble";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005D4E RID: 23886
			public static readonly StringName _hsv = "_hsv";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017E6 RID: 6118
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
