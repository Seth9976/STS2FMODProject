using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	// Token: 0x02000234 RID: 564
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NCardRemoveVfx.cs")]
	public class NCardRemoveVfx : Control
	{
		// Token: 0x06001CA7 RID: 7335 RVA: 0x000B9F1C File Offset: 0x000B811C
		[return: Nullable(2)]
		public static NCardRemoveVfx Create(NCard cardNode)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCardRemoveVfx ncardRemoveVfx = PreloadManager.Cache.GetScene(NCardRemoveVfx.scenePath).Instantiate<NCardRemoveVfx>(0L);
			ncardRemoveVfx._cardNode = cardNode;
			return ncardRemoveVfx;
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000B9F51 File Offset: 0x000B8151
		public override void _Ready()
		{
			base.GlobalPosition = this._cardNode.GlobalPosition;
			base.Rotation = this._cardNode.Rotation;
			this._cardParticles.SetEmitting(false);
			TaskHelper.RunSafely(this.PlayAnimation());
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x000B9F90 File Offset: 0x000B8190
		private async Task PlayAnimation()
		{
			this._anticipationParticles.Restart();
			await Cmd.Wait(this._anticipationDuration, false);
			this._slashStartParticles.Restart();
			await Cmd.Wait(this._slashEndDelay, false);
			this._slashEndParticles.Restart();
			this._cardParticles.Restart();
			TaskHelper.RunSafely(this.DelayedFree());
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x000B9FD4 File Offset: 0x000B81D4
		private async Task DelayedFree()
		{
			await Cmd.Wait(2f, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001CAB RID: 7339 RVA: 0x000BA018 File Offset: 0x000B8218
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCardRemoveVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardNode", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardRemoveVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CAC RID: 7340 RVA: 0x000BA0C8 File Offset: 0x000B82C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardRemoveVfx.MethodName.Create && args.Count == 1)
			{
				NCardRemoveVfx ncardRemoveVfx = NCardRemoveVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = VariantUtils.CreateFrom<NCardRemoveVfx>(ref ncardRemoveVfx);
				return true;
			}
			if ((ref method) == NCardRemoveVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x000BA13C File Offset: 0x000B833C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardRemoveVfx.MethodName.Create && args.Count == 1)
			{
				NCardRemoveVfx ncardRemoveVfx = NCardRemoveVfx.Create(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = VariantUtils.CreateFrom<NCardRemoveVfx>(ref ncardRemoveVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001CAE RID: 7342 RVA: 0x000BA18A File Offset: 0x000B838A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardRemoveVfx.MethodName.Create || (ref method) == NCardRemoveVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001CAF RID: 7343 RVA: 0x000BA1B4 File Offset: 0x000B83B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardRemoveVfx.PropertyName._anticipationParticles)
			{
				this._anticipationParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._slashStartParticles)
			{
				this._slashStartParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._slashEndParticles)
			{
				this._slashEndParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._cardParticles)
			{
				this._cardParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._anticipationDuration)
			{
				this._anticipationDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._slashEndDelay)
			{
				this._slashEndDelay = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._cardNode)
			{
				this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001CB0 RID: 7344 RVA: 0x000BA288 File Offset: 0x000B8488
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardRemoveVfx.PropertyName._anticipationParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._anticipationParticles);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._slashStartParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._slashStartParticles);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._slashEndParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._slashEndParticles);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._cardParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._cardParticles);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._anticipationDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._anticipationDuration);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._slashEndDelay)
			{
				value = VariantUtils.CreateFrom<float>(ref this._slashEndDelay);
				return true;
			}
			if ((ref name) == NCardRemoveVfx.PropertyName._cardNode)
			{
				value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001CB1 RID: 7345 RVA: 0x000BA380 File Offset: 0x000B8580
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardRemoveVfx.PropertyName._anticipationParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCardRemoveVfx.PropertyName._slashStartParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCardRemoveVfx.PropertyName._slashEndParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCardRemoveVfx.PropertyName._cardParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(3L, NCardRemoveVfx.PropertyName._anticipationDuration, 0L, "", 4102L, true),
				new PropertyInfo(3L, NCardRemoveVfx.PropertyName._slashEndDelay, 0L, "", 4102L, true),
				new PropertyInfo(24L, NCardRemoveVfx.PropertyName._cardNode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CB2 RID: 7346 RVA: 0x000BA480 File Offset: 0x000B8680
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardRemoveVfx.PropertyName._anticipationParticles, Variant.From<NParticlesContainer>(ref this._anticipationParticles));
			info.AddProperty(NCardRemoveVfx.PropertyName._slashStartParticles, Variant.From<NParticlesContainer>(ref this._slashStartParticles));
			info.AddProperty(NCardRemoveVfx.PropertyName._slashEndParticles, Variant.From<NParticlesContainer>(ref this._slashEndParticles));
			info.AddProperty(NCardRemoveVfx.PropertyName._cardParticles, Variant.From<NParticlesContainer>(ref this._cardParticles));
			info.AddProperty(NCardRemoveVfx.PropertyName._anticipationDuration, Variant.From<float>(ref this._anticipationDuration));
			info.AddProperty(NCardRemoveVfx.PropertyName._slashEndDelay, Variant.From<float>(ref this._slashEndDelay));
			info.AddProperty(NCardRemoveVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
		}

		/// <inheritdoc />
		// Token: 0x06001CB3 RID: 7347 RVA: 0x000BA530 File Offset: 0x000B8730
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardRemoveVfx.PropertyName._anticipationParticles, ref variant))
			{
				this._anticipationParticles = variant.As<NParticlesContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardRemoveVfx.PropertyName._slashStartParticles, ref variant2))
			{
				this._slashStartParticles = variant2.As<NParticlesContainer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardRemoveVfx.PropertyName._slashEndParticles, ref variant3))
			{
				this._slashEndParticles = variant3.As<NParticlesContainer>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardRemoveVfx.PropertyName._cardParticles, ref variant4))
			{
				this._cardParticles = variant4.As<NParticlesContainer>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardRemoveVfx.PropertyName._anticipationDuration, ref variant5))
			{
				this._anticipationDuration = variant5.As<float>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardRemoveVfx.PropertyName._slashEndDelay, ref variant6))
			{
				this._slashEndDelay = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardRemoveVfx.PropertyName._cardNode, ref variant7))
			{
				this._cardNode = variant7.As<NCard>();
			}
		}

		// Token: 0x04000AF3 RID: 2803
		public const float deleteCardDelay = 0.4f;

		// Token: 0x04000AF4 RID: 2804
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/ui/card/vfx_card_remove");

		// Token: 0x04000AF5 RID: 2805
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _anticipationParticles;

		// Token: 0x04000AF6 RID: 2806
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _slashStartParticles;

		// Token: 0x04000AF7 RID: 2807
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _slashEndParticles;

		// Token: 0x04000AF8 RID: 2808
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _cardParticles;

		// Token: 0x04000AF9 RID: 2809
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _anticipationDuration = 0.25f;

		// Token: 0x04000AFA RID: 2810
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _slashEndDelay = 0.1f;

		// Token: 0x04000AFB RID: 2811
		private NCard _cardNode;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011FC RID: 4604
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040045BA RID: 17850
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040045BB RID: 17851
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011FD RID: 4605
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_anticipationParticles' field.
			/// </summary>
			// Token: 0x040045BC RID: 17852
			public static readonly StringName _anticipationParticles = "_anticipationParticles";

			/// <summary>
			/// Cached name for the '_slashStartParticles' field.
			/// </summary>
			// Token: 0x040045BD RID: 17853
			public static readonly StringName _slashStartParticles = "_slashStartParticles";

			/// <summary>
			/// Cached name for the '_slashEndParticles' field.
			/// </summary>
			// Token: 0x040045BE RID: 17854
			public static readonly StringName _slashEndParticles = "_slashEndParticles";

			/// <summary>
			/// Cached name for the '_cardParticles' field.
			/// </summary>
			// Token: 0x040045BF RID: 17855
			public static readonly StringName _cardParticles = "_cardParticles";

			/// <summary>
			/// Cached name for the '_anticipationDuration' field.
			/// </summary>
			// Token: 0x040045C0 RID: 17856
			public static readonly StringName _anticipationDuration = "_anticipationDuration";

			/// <summary>
			/// Cached name for the '_slashEndDelay' field.
			/// </summary>
			// Token: 0x040045C1 RID: 17857
			public static readonly StringName _slashEndDelay = "_slashEndDelay";

			/// <summary>
			/// Cached name for the '_cardNode' field.
			/// </summary>
			// Token: 0x040045C2 RID: 17858
			public static readonly StringName _cardNode = "_cardNode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011FE RID: 4606
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
