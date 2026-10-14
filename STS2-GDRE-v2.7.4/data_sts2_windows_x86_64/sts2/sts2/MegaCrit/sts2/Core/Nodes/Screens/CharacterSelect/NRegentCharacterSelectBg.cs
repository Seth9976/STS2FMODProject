using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect
{
	// Token: 0x02000342 RID: 834
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NRegentCharacterSelectBg.cs")]
	public class NRegentCharacterSelectBg : Control
	{
		// Token: 0x06002F80 RID: 12160 RVA: 0x0015B358 File Offset: 0x00159558
		public override void _Ready()
		{
			this._spineController = new MegaSprite(base.GetNode("SpineSprite"));
			this._sphereGuardianHover = base.GetNode<Control>("SphereGuardianHover");
			this._sphereGuardianHover.Connect(Control.SignalName.MouseEntered, Callable.From(delegate
			{
				this.SetSkin("spheric guardian constellation");
			}), 0U);
			this._sphereGuardianHover.Connect(Control.SignalName.MouseExited, Callable.From(delegate
			{
				this.SetSkin("normal");
			}), 0U);
			this._decaHover = base.GetNode<Control>("DecaHover");
			this._decaHover.Connect(Control.SignalName.MouseEntered, Callable.From(delegate
			{
				this.SetSkin("deca outline");
			}), 0U);
			this._decaHover.Connect(Control.SignalName.MouseExited, Callable.From(delegate
			{
				this.SetSkin("normal");
			}), 0U);
			this._sentryHover = base.GetNode<Control>("SentryHover");
			this._sentryHover.Connect(Control.SignalName.MouseEntered, Callable.From(delegate
			{
				this.SetSkin("sentry constellation");
			}), 0U);
			this._sentryHover.Connect(Control.SignalName.MouseExited, Callable.From(delegate
			{
				this.SetSkin("normal");
			}), 0U);
			this._sneckoHover = base.GetNode<Control>("SneckoHover");
			this._sneckoHover.Connect(Control.SignalName.MouseEntered, Callable.From(delegate
			{
				this.SetSkin("snecko constellation");
			}), 0U);
			this._sneckoHover.Connect(Control.SignalName.MouseExited, Callable.From(delegate
			{
				this.SetSkin("normal");
			}), 0U);
			this._cultistHover = base.GetNode<Control>("CultistHover");
			this._cultistHover.Connect(Control.SignalName.MouseEntered, Callable.From(delegate
			{
				this.SetSkin("cultist constellation");
			}), 0U);
			this._cultistHover.Connect(Control.SignalName.MouseExited, Callable.From(delegate
			{
				this.SetSkin("normal");
			}), 0U);
			this._shapesHover = base.GetNode<Control>("ShapesHover");
			this._shapesHover.Connect(Control.SignalName.MouseEntered, Callable.From(delegate
			{
				this.SetSkin("shapes constellation");
			}), 0U);
			this._shapesHover.Connect(Control.SignalName.MouseExited, Callable.From(delegate
			{
				this.SetSkin("normal");
			}), 0U);
			this._amogusHover = base.GetNode<Control>("AmogusHover");
			this._amogusHover.Connect(Control.SignalName.MouseEntered, Callable.From(delegate
			{
				this.SetSkin("amogus constellation");
			}), 0U);
			this._amogusHover.Connect(Control.SignalName.MouseExited, Callable.From(delegate
			{
				this.SetSkin("normal");
			}), 0U);
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x0015B60C File Offset: 0x0015980C
		private void SetSkin(string skinName)
		{
			MegaSkeleton skeleton = this._spineController.GetSkeleton();
			if (skeleton == null)
			{
				return;
			}
			skeleton.SetSkin(skeleton.GetData().FindSkin(skinName));
			skeleton.SetSlotsToSetupPose();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002F82 RID: 12162 RVA: 0x0015B644 File Offset: 0x00159844
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NRegentCharacterSelectBg.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRegentCharacterSelectBg.MethodName.SetSkin, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "skinName", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002F83 RID: 12163 RVA: 0x0015B6DC File Offset: 0x001598DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRegentCharacterSelectBg.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRegentCharacterSelectBg.MethodName.SetSkin && args.Count == 1)
			{
				this.SetSkin(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002F84 RID: 12164 RVA: 0x0015B74A File Offset: 0x0015994A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRegentCharacterSelectBg.MethodName._Ready || (ref method) == NRegentCharacterSelectBg.MethodName.SetSkin || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002F85 RID: 12165 RVA: 0x0015B774 File Offset: 0x00159974
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._sphereGuardianHover)
			{
				this._sphereGuardianHover = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._decaHover)
			{
				this._decaHover = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._sentryHover)
			{
				this._sentryHover = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._sneckoHover)
			{
				this._sneckoHover = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._cultistHover)
			{
				this._cultistHover = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._shapesHover)
			{
				this._shapesHover = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._amogusHover)
			{
				this._amogusHover = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002F86 RID: 12166 RVA: 0x0015B848 File Offset: 0x00159A48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._sphereGuardianHover)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._sphereGuardianHover);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._decaHover)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._decaHover);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._sentryHover)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._sentryHover);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._sneckoHover)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._sneckoHover);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._cultistHover)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cultistHover);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._shapesHover)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._shapesHover);
				return true;
			}
			if ((ref name) == NRegentCharacterSelectBg.PropertyName._amogusHover)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._amogusHover);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002F87 RID: 12167 RVA: 0x0015B940 File Offset: 0x00159B40
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRegentCharacterSelectBg.PropertyName._sphereGuardianHover, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentCharacterSelectBg.PropertyName._decaHover, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentCharacterSelectBg.PropertyName._sentryHover, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentCharacterSelectBg.PropertyName._sneckoHover, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentCharacterSelectBg.PropertyName._cultistHover, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentCharacterSelectBg.PropertyName._shapesHover, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRegentCharacterSelectBg.PropertyName._amogusHover, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002F88 RID: 12168 RVA: 0x0015BA3C File Offset: 0x00159C3C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRegentCharacterSelectBg.PropertyName._sphereGuardianHover, Variant.From<Control>(ref this._sphereGuardianHover));
			info.AddProperty(NRegentCharacterSelectBg.PropertyName._decaHover, Variant.From<Control>(ref this._decaHover));
			info.AddProperty(NRegentCharacterSelectBg.PropertyName._sentryHover, Variant.From<Control>(ref this._sentryHover));
			info.AddProperty(NRegentCharacterSelectBg.PropertyName._sneckoHover, Variant.From<Control>(ref this._sneckoHover));
			info.AddProperty(NRegentCharacterSelectBg.PropertyName._cultistHover, Variant.From<Control>(ref this._cultistHover));
			info.AddProperty(NRegentCharacterSelectBg.PropertyName._shapesHover, Variant.From<Control>(ref this._shapesHover));
			info.AddProperty(NRegentCharacterSelectBg.PropertyName._amogusHover, Variant.From<Control>(ref this._amogusHover));
		}

		/// <inheritdoc />
		// Token: 0x06002F89 RID: 12169 RVA: 0x0015BAEC File Offset: 0x00159CEC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._sphereGuardianHover, ref variant))
			{
				this._sphereGuardianHover = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._decaHover, ref variant2))
			{
				this._decaHover = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._sentryHover, ref variant3))
			{
				this._sentryHover = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._sneckoHover, ref variant4))
			{
				this._sneckoHover = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._cultistHover, ref variant5))
			{
				this._cultistHover = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._shapesHover, ref variant6))
			{
				this._shapesHover = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._amogusHover, ref variant7))
			{
				this._amogusHover = variant7.As<Control>();
			}
		}

		// Token: 0x04001244 RID: 4676
		private MegaSprite _spineController;

		// Token: 0x04001245 RID: 4677
		private Control _sphereGuardianHover;

		// Token: 0x04001246 RID: 4678
		private Control _decaHover;

		// Token: 0x04001247 RID: 4679
		private Control _sentryHover;

		// Token: 0x04001248 RID: 4680
		private Control _sneckoHover;

		// Token: 0x04001249 RID: 4681
		private Control _cultistHover;

		// Token: 0x0400124A RID: 4682
		private Control _shapesHover;

		// Token: 0x0400124B RID: 4683
		private Control _amogusHover;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015EE RID: 5614
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040054F6 RID: 21750
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetSkin' method.
			/// </summary>
			// Token: 0x040054F7 RID: 21751
			public static readonly StringName SetSkin = "SetSkin";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015EF RID: 5615
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_sphereGuardianHover' field.
			/// </summary>
			// Token: 0x040054F8 RID: 21752
			public static readonly StringName _sphereGuardianHover = "_sphereGuardianHover";

			/// <summary>
			/// Cached name for the '_decaHover' field.
			/// </summary>
			// Token: 0x040054F9 RID: 21753
			public static readonly StringName _decaHover = "_decaHover";

			/// <summary>
			/// Cached name for the '_sentryHover' field.
			/// </summary>
			// Token: 0x040054FA RID: 21754
			public static readonly StringName _sentryHover = "_sentryHover";

			/// <summary>
			/// Cached name for the '_sneckoHover' field.
			/// </summary>
			// Token: 0x040054FB RID: 21755
			public static readonly StringName _sneckoHover = "_sneckoHover";

			/// <summary>
			/// Cached name for the '_cultistHover' field.
			/// </summary>
			// Token: 0x040054FC RID: 21756
			public static readonly StringName _cultistHover = "_cultistHover";

			/// <summary>
			/// Cached name for the '_shapesHover' field.
			/// </summary>
			// Token: 0x040054FD RID: 21757
			public static readonly StringName _shapesHover = "_shapesHover";

			/// <summary>
			/// Cached name for the '_amogusHover' field.
			/// </summary>
			// Token: 0x040054FE RID: 21758
			public static readonly StringName _amogusHover = "_amogusHover";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015F0 RID: 5616
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
