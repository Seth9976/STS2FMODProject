using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001B6 RID: 438
	[ScriptPath("res://src/Core/Nodes/Vfx/NGrandFinaleVfx.cs")]
	public class NGrandFinaleVfx : Node2D
	{
		// Token: 0x060015C2 RID: 5570 RVA: 0x00084BB8 File Offset: 0x00082DB8
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NGrandFinaleVfx Create(Creature creature)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NGrandFinaleVfx.Create(ncreature.VfxSpawnPosition);
			}
			return null;
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00084BF4 File Offset: 0x00082DF4
		[NullableContext(2)]
		public static NGrandFinaleVfx Create(Vector2 playerPosition)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NGrandFinaleVfx ngrandFinaleVfx = PreloadManager.Cache.GetScene(NGrandFinaleVfx.scenePath).Instantiate<NGrandFinaleVfx>(0L);
			ngrandFinaleVfx.Initialize(playerPosition);
			return ngrandFinaleVfx;
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x00084C2C File Offset: 0x00082E2C
		private void Initialize(Vector2 playerPosition)
		{
			this._anticipationParticles.GlobalPosition = playerPosition;
			this._slashParticles.GlobalPosition = playerPosition;
			this._endParticles.GlobalPosition = playerPosition;
			this._spotlight.Modulate = new Color(1f, 1f, 1f, 0f);
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x00084C81 File Offset: 0x00082E81
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x00084C8F File Offset: 0x00082E8F
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00084CA4 File Offset: 0x00082EA4
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			this._spotlightParticles.GlobalPosition = new Vector2(base.GetViewportRect().Size.X / 2f, 0f);
			Tween tween = base.GetTree().CreateTween();
			tween.TweenProperty(this._spotlight, "modulate", new Color(1f, 1f, 1f, 1f), 1.0);
			this._spotlightParticles.Restart();
			await Cmd.Wait(NGrandFinaleVfx._spotlightDuration, this._cts.Token, false);
			this._anticipationParticles.Restart();
			await Cmd.Wait(NGrandFinaleVfx._anticipationDuration, this._cts.Token, false);
			this._slashParticles.Restart();
			await Cmd.Wait(NGrandFinaleVfx._slashDuration, this._cts.Token, false);
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, -1f);
			}
			await Cmd.Wait(NGrandFinaleVfx._hitDuration, this._cts.Token, false);
			this._endParticles.Restart();
			Tween tween2 = base.GetTree().CreateTween();
			tween2.TweenProperty(this._spotlight, "modulate", new Color(1f, 1f, 1f, 0f), 0.5);
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060015C8 RID: 5576 RVA: 0x00084CE8 File Offset: 0x00082EE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NGrandFinaleVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "playerPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGrandFinaleVfx.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "playerPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGrandFinaleVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGrandFinaleVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015C9 RID: 5577 RVA: 0x00084E10 File Offset: 0x00083010
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGrandFinaleVfx.MethodName.Create && args.Count == 1)
			{
				NGrandFinaleVfx ngrandFinaleVfx = NGrandFinaleVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NGrandFinaleVfx>(ref ngrandFinaleVfx);
				return true;
			}
			if ((ref method) == NGrandFinaleVfx.MethodName.Initialize && args.Count == 1)
			{
				this.Initialize(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGrandFinaleVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGrandFinaleVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x00084EDC File Offset: 0x000830DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGrandFinaleVfx.MethodName.Create && args.Count == 1)
			{
				NGrandFinaleVfx ngrandFinaleVfx = NGrandFinaleVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NGrandFinaleVfx>(ref ngrandFinaleVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060015CB RID: 5579 RVA: 0x00084F2C File Offset: 0x0008312C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGrandFinaleVfx.MethodName.Create || (ref method) == NGrandFinaleVfx.MethodName.Initialize || (ref method) == NGrandFinaleVfx.MethodName._Ready || (ref method) == NGrandFinaleVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060015CC RID: 5580 RVA: 0x00084F7C File Offset: 0x0008317C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGrandFinaleVfx.PropertyName._spotlight)
			{
				this._spotlight = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NGrandFinaleVfx.PropertyName._spotlightParticles)
			{
				this._spotlightParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NGrandFinaleVfx.PropertyName._anticipationParticles)
			{
				this._anticipationParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NGrandFinaleVfx.PropertyName._slashParticles)
			{
				this._slashParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NGrandFinaleVfx.PropertyName._endParticles)
			{
				this._endParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060015CD RID: 5581 RVA: 0x00085018 File Offset: 0x00083218
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGrandFinaleVfx.PropertyName._spotlight)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._spotlight);
				return true;
			}
			if ((ref name) == NGrandFinaleVfx.PropertyName._spotlightParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._spotlightParticles);
				return true;
			}
			if ((ref name) == NGrandFinaleVfx.PropertyName._anticipationParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._anticipationParticles);
				return true;
			}
			if ((ref name) == NGrandFinaleVfx.PropertyName._slashParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._slashParticles);
				return true;
			}
			if ((ref name) == NGrandFinaleVfx.PropertyName._endParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._endParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060015CE RID: 5582 RVA: 0x000850D0 File Offset: 0x000832D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGrandFinaleVfx.PropertyName._spotlight, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NGrandFinaleVfx.PropertyName._spotlightParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NGrandFinaleVfx.PropertyName._anticipationParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NGrandFinaleVfx.PropertyName._slashParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NGrandFinaleVfx.PropertyName._endParticles, 34L, "Node2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015CF RID: 5583 RVA: 0x00085190 File Offset: 0x00083390
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGrandFinaleVfx.PropertyName._spotlight, Variant.From<Node2D>(ref this._spotlight));
			info.AddProperty(NGrandFinaleVfx.PropertyName._spotlightParticles, Variant.From<NParticlesContainer>(ref this._spotlightParticles));
			info.AddProperty(NGrandFinaleVfx.PropertyName._anticipationParticles, Variant.From<NParticlesContainer>(ref this._anticipationParticles));
			info.AddProperty(NGrandFinaleVfx.PropertyName._slashParticles, Variant.From<NParticlesContainer>(ref this._slashParticles));
			info.AddProperty(NGrandFinaleVfx.PropertyName._endParticles, Variant.From<NParticlesContainer>(ref this._endParticles));
		}

		/// <inheritdoc />
		// Token: 0x060015D0 RID: 5584 RVA: 0x00085214 File Offset: 0x00083414
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGrandFinaleVfx.PropertyName._spotlight, ref variant))
			{
				this._spotlight = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGrandFinaleVfx.PropertyName._spotlightParticles, ref variant2))
			{
				this._spotlightParticles = variant2.As<NParticlesContainer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NGrandFinaleVfx.PropertyName._anticipationParticles, ref variant3))
			{
				this._anticipationParticles = variant3.As<NParticlesContainer>();
			}
			Variant variant4;
			if (info.TryGetProperty(NGrandFinaleVfx.PropertyName._slashParticles, ref variant4))
			{
				this._slashParticles = variant4.As<NParticlesContainer>();
			}
			Variant variant5;
			if (info.TryGetProperty(NGrandFinaleVfx.PropertyName._endParticles, ref variant5))
			{
				this._endParticles = variant5.As<NParticlesContainer>();
			}
		}

		// Token: 0x040007B4 RID: 1972
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_grand_finale");

		// Token: 0x040007B5 RID: 1973
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _spotlight;

		// Token: 0x040007B6 RID: 1974
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _spotlightParticles;

		// Token: 0x040007B7 RID: 1975
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _anticipationParticles;

		// Token: 0x040007B8 RID: 1976
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _slashParticles;

		// Token: 0x040007B9 RID: 1977
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _endParticles;

		// Token: 0x040007BA RID: 1978
		[Nullable(2)]
		private CancellationTokenSource _cts;

		// Token: 0x040007BB RID: 1979
		private static readonly float _spotlightDuration = 1.25f;

		// Token: 0x040007BC RID: 1980
		private static readonly float _anticipationDuration = 0.25f;

		// Token: 0x040007BD RID: 1981
		private static readonly float _slashDuration = 0.125f;

		// Token: 0x040007BE RID: 1982
		private static readonly float _hitDuration = 0.0125f;

		// Token: 0x040007BF RID: 1983
		public static readonly float totalAnticipationDuration = NGrandFinaleVfx._spotlightDuration + NGrandFinaleVfx._anticipationDuration + NGrandFinaleVfx._slashDuration;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001056 RID: 4182
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004065 RID: 16485
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x04004066 RID: 16486
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004067 RID: 16487
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004068 RID: 16488
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001057 RID: 4183
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_spotlight' field.
			/// </summary>
			// Token: 0x04004069 RID: 16489
			public static readonly StringName _spotlight = "_spotlight";

			/// <summary>
			/// Cached name for the '_spotlightParticles' field.
			/// </summary>
			// Token: 0x0400406A RID: 16490
			public static readonly StringName _spotlightParticles = "_spotlightParticles";

			/// <summary>
			/// Cached name for the '_anticipationParticles' field.
			/// </summary>
			// Token: 0x0400406B RID: 16491
			public static readonly StringName _anticipationParticles = "_anticipationParticles";

			/// <summary>
			/// Cached name for the '_slashParticles' field.
			/// </summary>
			// Token: 0x0400406C RID: 16492
			public static readonly StringName _slashParticles = "_slashParticles";

			/// <summary>
			/// Cached name for the '_endParticles' field.
			/// </summary>
			// Token: 0x0400406D RID: 16493
			public static readonly StringName _endParticles = "_endParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001058 RID: 4184
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
