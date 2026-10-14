using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	/// <summary>
	/// Many of this VFX are spawned via NHellraiserVfx.
	/// This represents a single sword particle and maybe useful for re-use if we want these to spawn when the
	/// Hellraiser power procs.
	/// </summary>
	// Token: 0x02000239 RID: 569
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NHellraiserSwordVfx.cs")]
	public class NHellraiserSwordVfx : Control
	{
		// Token: 0x06001CF1 RID: 7409 RVA: 0x000BC725 File Offset: 0x000BA925
		[NullableContext(2)]
		public static NHellraiserSwordVfx Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NHellraiserSwordVfx._scenePath).Instantiate<NHellraiserSwordVfx>(0L);
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x000BC748 File Offset: 0x000BA948
		public override void _Ready()
		{
			base.Name = NHellraiserSwordVfx._swordStr;
			this._sword = base.GetNode<TextureRect>("TextureRect");
			this._sword.FlipH = Rng.Chaotic.NextBool();
			this._sword.RotationDegrees = Rng.Chaotic.NextFloat(-20f, 20f);
			this._sword.Position = new Vector2(25f, 200f);
			base.Scale = new Vector2(Rng.Chaotic.NextFloat(0.7f, 0.9f), Rng.Chaotic.NextFloat(0.8f, 1.2f)) * Rng.Chaotic.NextFloat(1f, 2f);
			base.Position += new Vector2(Rng.Chaotic.NextGaussianFloat(0f, 1f, -500f, 500f), this.posY);
			Tween tween = base.CreateTween().SetParallel(true);
			tween.TweenInterval(Rng.Chaotic.NextDouble() * 0.8);
			tween.Chain();
			tween.TweenProperty(this._sword, "position:y", 80f, 0.25).From(300f).SetEase(1L)
				.SetTrans(11L);
			tween.TweenProperty(this, "modulate", this.targetColor, 0.25).From(Colors.Red);
			tween.Chain().TweenInterval(0.25);
			tween.Chain();
			tween.TweenProperty(this._sword, "position:y", 200f, 0.5).SetEase(0L).SetTrans(5L);
			tween.TweenProperty(this, "modulate:a", 0f, 0.5);
			tween.Chain().TweenCallback(Callable.From(new Action(this.OnTweenFinished)));
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x000BC98C File Offset: 0x000BAB8C
		private void OnTweenFinished()
		{
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001CF4 RID: 7412 RVA: 0x000BC994 File Offset: 0x000BAB94
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NHellraiserSwordVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NHellraiserSwordVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHellraiserSwordVfx.MethodName.OnTweenFinished, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CF5 RID: 7413 RVA: 0x000BCA44 File Offset: 0x000BAC44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHellraiserSwordVfx.MethodName.Create && args.Count == 0)
			{
				NHellraiserSwordVfx nhellraiserSwordVfx = NHellraiserSwordVfx.Create();
				ret = VariantUtils.CreateFrom<NHellraiserSwordVfx>(ref nhellraiserSwordVfx);
				return true;
			}
			if ((ref method) == NHellraiserSwordVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHellraiserSwordVfx.MethodName.OnTweenFinished && args.Count == 0)
			{
				this.OnTweenFinished();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x000BCAD0 File Offset: 0x000BACD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHellraiserSwordVfx.MethodName.Create && args.Count == 0)
			{
				NHellraiserSwordVfx nhellraiserSwordVfx = NHellraiserSwordVfx.Create();
				ret = VariantUtils.CreateFrom<NHellraiserSwordVfx>(ref nhellraiserSwordVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001CF7 RID: 7415 RVA: 0x000BCB10 File Offset: 0x000BAD10
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHellraiserSwordVfx.MethodName.Create || (ref method) == NHellraiserSwordVfx.MethodName._Ready || (ref method) == NHellraiserSwordVfx.MethodName.OnTweenFinished || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001CF8 RID: 7416 RVA: 0x000BCB48 File Offset: 0x000BAD48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHellraiserSwordVfx.PropertyName._sword)
			{
				this._sword = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NHellraiserSwordVfx.PropertyName.posY)
			{
				this.posY = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NHellraiserSwordVfx.PropertyName.targetColor)
			{
				this.targetColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001CF9 RID: 7417 RVA: 0x000BCBB0 File Offset: 0x000BADB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHellraiserSwordVfx.PropertyName._sword)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._sword);
				return true;
			}
			if ((ref name) == NHellraiserSwordVfx.PropertyName.posY)
			{
				value = VariantUtils.CreateFrom<float>(ref this.posY);
				return true;
			}
			if ((ref name) == NHellraiserSwordVfx.PropertyName.targetColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this.targetColor);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001CFA RID: 7418 RVA: 0x000BCC28 File Offset: 0x000BAE28
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NHellraiserSwordVfx.PropertyName._sword, 0L, "", 4096L, false),
				new PropertyInfo(3L, NHellraiserSwordVfx.PropertyName.posY, 0L, "", 4096L, false),
				new PropertyInfo(20L, NHellraiserSwordVfx.PropertyName.targetColor, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CFB RID: 7419 RVA: 0x000BCCA0 File Offset: 0x000BAEA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHellraiserSwordVfx.PropertyName._sword, Variant.From<TextureRect>(ref this._sword));
			info.AddProperty(NHellraiserSwordVfx.PropertyName.posY, Variant.From<float>(ref this.posY));
			info.AddProperty(NHellraiserSwordVfx.PropertyName.targetColor, Variant.From<Color>(ref this.targetColor));
		}

		/// <inheritdoc />
		// Token: 0x06001CFC RID: 7420 RVA: 0x000BCCF8 File Offset: 0x000BAEF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHellraiserSwordVfx.PropertyName._sword, ref variant))
			{
				this._sword = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHellraiserSwordVfx.PropertyName.posY, ref variant2))
			{
				this.posY = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHellraiserSwordVfx.PropertyName.targetColor, ref variant3))
			{
				this.targetColor = variant3.As<Color>();
			}
		}

		// Token: 0x04000B26 RID: 2854
		[Nullable(1)]
		private static readonly StringName _swordStr = new StringName("Sword");

		// Token: 0x04000B27 RID: 2855
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/cards/vfx_hellraiser/hellraiser_sword_vfx");

		// Token: 0x04000B28 RID: 2856
		[Nullable(1)]
		private TextureRect _sword;

		// Token: 0x04000B29 RID: 2857
		public float posY;

		// Token: 0x04000B2A RID: 2858
		public Color targetColor;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001215 RID: 4629
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400461E RID: 17950
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400461F RID: 17951
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnTweenFinished' method.
			/// </summary>
			// Token: 0x04004620 RID: 17952
			public static readonly StringName OnTweenFinished = "OnTweenFinished";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001216 RID: 4630
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_sword' field.
			/// </summary>
			// Token: 0x04004621 RID: 17953
			public static readonly StringName _sword = "_sword";

			/// <summary>
			/// Cached name for the 'posY' field.
			/// </summary>
			// Token: 0x04004622 RID: 17954
			public static readonly StringName posY = "posY";

			/// <summary>
			/// Cached name for the 'targetColor' field.
			/// </summary>
			// Token: 0x04004623 RID: 17955
			public static readonly StringName targetColor = "targetColor";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001217 RID: 4631
		public class SignalName : Control.SignalName
		{
		}
	}
}
