using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;

namespace MegaCrit.Sts2.Core.Debug
{
	// Token: 0x02000D0E RID: 3342
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Debug/NCombatVfxSpawner.cs")]
	public class NCombatVfxSpawner : Control
	{
		// Token: 0x060086D6 RID: 34518 RVA: 0x002D41BC File Offset: 0x002D23BC
		public override void _Ready()
		{
		}

		// Token: 0x060086D7 RID: 34519 RVA: 0x002D41BE File Offset: 0x002D23BE
		public override void _Process(double delta)
		{
			base._Process(delta);
			this._shiftPressed = Input.IsKeyPressed(4194325L);
		}

		// Token: 0x060086D8 RID: 34520 RVA: 0x002D41D8 File Offset: 0x002D23D8
		public override void _Input(InputEvent inputEvent)
		{
			InputEventKey inputEventKey = inputEvent as InputEventKey;
			if (inputEventKey != null && inputEventKey.Keycode == 81L && inputEventKey.Pressed)
			{
				this.TestFunctionA(this._shiftPressed);
			}
			inputEventKey = inputEvent as InputEventKey;
			if (inputEventKey != null && inputEventKey.Keycode == 87L && inputEventKey.Pressed)
			{
				this.TestFunctionB(this._shiftPressed);
			}
			inputEventKey = inputEvent as InputEventKey;
			if (inputEventKey != null && inputEventKey.Keycode == 69L && inputEventKey.Pressed)
			{
				this.TestFunctionC(this._shiftPressed);
			}
		}

		// Token: 0x060086D9 RID: 34521 RVA: 0x002D4260 File Offset: 0x002D2460
		private static Color GetRandomColor()
		{
			return new Color(Mathf.Lerp(0.35f, 1f, GD.Randf()), Mathf.Lerp(0.35f, 1f, GD.Randf()), Mathf.Lerp(0.35f, 1f, GD.Randf()), 1f);
		}

		// Token: 0x060086DA RID: 34522 RVA: 0x002D42B3 File Offset: 0x002D24B3
		private void TestFunctionA(bool shiftPressed)
		{
			this.TestProjectileHandler();
		}

		// Token: 0x060086DB RID: 34523 RVA: 0x002D42BB File Offset: 0x002D24BB
		private void TestFunctionB(bool shiftPressed)
		{
			TaskHelper.RunSafely(this.PlayingGrandFinale());
		}

		// Token: 0x060086DC RID: 34524 RVA: 0x002D42CC File Offset: 0x002D24CC
		private void TestFunctionC(bool shiftPressed)
		{
			NWormyImpactVfx nwormyImpactVfx = NWormyImpactVfx.Create(this._playerGroundPosition.GlobalPosition, this._playerPosition.GlobalPosition);
			this._combatVfxContainer.AddChildSafely(nwormyImpactVfx);
		}

		// Token: 0x060086DD RID: 34525 RVA: 0x002D4304 File Offset: 0x002D2504
		private void TestProjectileHandler()
		{
			NVfxProjectileHandler nvfxProjectileHandler = NVfxProjectileHandler.Create("debug/vfx/vfx_test_projectile_handler", "debug/vfx/vfx_test_projectile", this._playerPosition.GlobalPosition, this._enemyPosition.GlobalPosition, default(Callable));
			this._combatVfxContainer.AddChildSafely(nvfxProjectileHandler);
		}

		// Token: 0x060086DE RID: 34526 RVA: 0x002D434C File Offset: 0x002D254C
		private void SpawnVfx()
		{
			NGaseousImpactVfx ngaseousImpactVfx = NGaseousImpactVfx.Create(this._enemyPosition.GlobalPosition, NCombatVfxSpawner.GetRandomColor());
			this._combatVfxContainer.AddChildSafely(ngaseousImpactVfx);
		}

		// Token: 0x060086DF RID: 34527 RVA: 0x002D437C File Offset: 0x002D257C
		private async Task SpawningVfx()
		{
			NItemThrowVfx nitemThrowVfx = NItemThrowVfx.Create(this._playerPosition.GlobalPosition + Vector2.Up * 150f, this._enemyPosition.GlobalPosition, null, null);
			this._combatVfxContainer.AddChildSafely(nitemThrowVfx);
			await Cmd.Wait(0.55f, false);
			NSplashVfx nsplashVfx = NSplashVfx.Create(this._enemyPosition.GlobalPosition, new Color(0.25f, 1f, 0.4f, 1f));
			this._combatVfxContainer.AddChildSafely(nsplashVfx);
		}

		// Token: 0x060086E0 RID: 34528 RVA: 0x002D43C0 File Offset: 0x002D25C0
		private async Task Hyperbeaming()
		{
			NHyperbeamVfx nhyperbeamVfx = NHyperbeamVfx.Create(this._defectEyePosition.GlobalPosition, this._enemyPosition.GlobalPosition);
			this._combatVfxContainer.AddChildSafely(nhyperbeamVfx);
			await Cmd.Wait(NHyperbeamVfx.hyperbeamAnticipationDuration, false);
			NHyperbeamImpactVfx nhyperbeamImpactVfx = NHyperbeamImpactVfx.Create(this._defectEyePosition.GlobalPosition, this._enemyPosition.GlobalPosition);
			this._combatVfxContainer.AddChildSafely(nhyperbeamImpactVfx);
		}

		// Token: 0x060086E1 RID: 34529 RVA: 0x002D4404 File Offset: 0x002D2604
		private async Task PlayingGrandFinale()
		{
			NGrandFinaleVfx ngrandFinaleVfx = NGrandFinaleVfx.Create(this._playerPosition.GlobalPosition);
			this._combatVfxContainer.AddChildSafely(ngrandFinaleVfx);
			await Cmd.Wait(NGrandFinaleVfx.totalAnticipationDuration, false);
			NGrandFinaleImpactVfx ngrandFinaleImpactVfx = NGrandFinaleImpactVfx.Create(this._enemyPosition.GlobalPosition, this._enemyGroundPosition.GlobalPosition);
			this._combatVfxContainer.AddChildSafely(ngrandFinaleImpactVfx);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060086E2 RID: 34530 RVA: 0x002D4448 File Offset: 0x002D2648
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NCombatVfxSpawner.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatVfxSpawner.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCombatVfxSpawner.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCombatVfxSpawner.MethodName.GetRandomColor, new PropertyInfo(20L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NCombatVfxSpawner.MethodName.TestFunctionA, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "shiftPressed", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCombatVfxSpawner.MethodName.TestFunctionB, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "shiftPressed", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCombatVfxSpawner.MethodName.TestFunctionC, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "shiftPressed", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCombatVfxSpawner.MethodName.TestProjectileHandler, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatVfxSpawner.MethodName.SpawnVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060086E3 RID: 34531 RVA: 0x002D46CC File Offset: 0x002D28CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatVfxSpawner.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatVfxSpawner.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatVfxSpawner.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatVfxSpawner.MethodName.GetRandomColor && args.Count == 0)
			{
				Color randomColor = NCombatVfxSpawner.GetRandomColor();
				ret = VariantUtils.CreateFrom<Color>(ref randomColor);
				return true;
			}
			if ((ref method) == NCombatVfxSpawner.MethodName.TestFunctionA && args.Count == 1)
			{
				this.TestFunctionA(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatVfxSpawner.MethodName.TestFunctionB && args.Count == 1)
			{
				this.TestFunctionB(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatVfxSpawner.MethodName.TestFunctionC && args.Count == 1)
			{
				this.TestFunctionC(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatVfxSpawner.MethodName.TestProjectileHandler && args.Count == 0)
			{
				this.TestProjectileHandler();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatVfxSpawner.MethodName.SpawnVfx && args.Count == 0)
			{
				this.SpawnVfx();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060086E4 RID: 34532 RVA: 0x002D487C File Offset: 0x002D2A7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatVfxSpawner.MethodName.GetRandomColor && args.Count == 0)
			{
				Color randomColor = NCombatVfxSpawner.GetRandomColor();
				ret = VariantUtils.CreateFrom<Color>(ref randomColor);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060086E5 RID: 34533 RVA: 0x002D48BC File Offset: 0x002D2ABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatVfxSpawner.MethodName._Ready || (ref method) == NCombatVfxSpawner.MethodName._Process || (ref method) == NCombatVfxSpawner.MethodName._Input || (ref method) == NCombatVfxSpawner.MethodName.GetRandomColor || (ref method) == NCombatVfxSpawner.MethodName.TestFunctionA || (ref method) == NCombatVfxSpawner.MethodName.TestFunctionB || (ref method) == NCombatVfxSpawner.MethodName.TestFunctionC || (ref method) == NCombatVfxSpawner.MethodName.TestProjectileHandler || (ref method) == NCombatVfxSpawner.MethodName.SpawnVfx || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060086E6 RID: 34534 RVA: 0x002D4958 File Offset: 0x002D2B58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatVfxSpawner.PropertyName._backCombatVfxContainer)
			{
				this._backCombatVfxContainer = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._combatVfxContainer)
			{
				this._combatVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._env)
			{
				this._env = VariantUtils.ConvertTo<WorldEnvironment>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._playerTopPosition)
			{
				this._playerTopPosition = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._playerPosition)
			{
				this._playerPosition = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._playerGroundPosition)
			{
				this._playerGroundPosition = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._enemyTopPosition)
			{
				this._enemyTopPosition = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._enemyPosition)
			{
				this._enemyPosition = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._enemyGroundPosition)
			{
				this._enemyGroundPosition = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._defectEyePosition)
			{
				this._defectEyePosition = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._lowHpBorderVfx)
			{
				this._lowHpBorderVfx = VariantUtils.ConvertTo<NLowHpBorderVfx>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._gaseousScreenVfx)
			{
				this._gaseousScreenVfx = VariantUtils.ConvertTo<NGaseousScreenVfx>(ref value);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._shiftPressed)
			{
				this._shiftPressed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060086E7 RID: 34535 RVA: 0x002D4ACC File Offset: 0x002D2CCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatVfxSpawner.PropertyName._backCombatVfxContainer)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._backCombatVfxContainer);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._combatVfxContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._combatVfxContainer);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._env)
			{
				value = VariantUtils.CreateFrom<WorldEnvironment>(ref this._env);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._playerTopPosition)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._playerTopPosition);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._playerPosition)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._playerPosition);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._playerGroundPosition)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._playerGroundPosition);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._enemyTopPosition)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._enemyTopPosition);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._enemyPosition)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._enemyPosition);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._enemyGroundPosition)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._enemyGroundPosition);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._defectEyePosition)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._defectEyePosition);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._lowHpBorderVfx)
			{
				value = VariantUtils.CreateFrom<NLowHpBorderVfx>(ref this._lowHpBorderVfx);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._gaseousScreenVfx)
			{
				value = VariantUtils.CreateFrom<NGaseousScreenVfx>(ref this._gaseousScreenVfx);
				return true;
			}
			if ((ref name) == NCombatVfxSpawner.PropertyName._shiftPressed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._shiftPressed);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060086E8 RID: 34536 RVA: 0x002D4C84 File Offset: 0x002D2E84
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._backCombatVfxContainer, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._combatVfxContainer, 34L, "Control", 4102L, true),
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._env, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._playerTopPosition, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._playerPosition, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._playerGroundPosition, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._enemyTopPosition, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._enemyPosition, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._enemyGroundPosition, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._defectEyePosition, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._lowHpBorderVfx, 34L, "ColorRect", 4102L, true),
				new PropertyInfo(24L, NCombatVfxSpawner.PropertyName._gaseousScreenVfx, 34L, "AspectRatioContainer", 4102L, true),
				new PropertyInfo(1L, NCombatVfxSpawner.PropertyName._shiftPressed, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060086E9 RID: 34537 RVA: 0x002D4E50 File Offset: 0x002D3050
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCombatVfxSpawner.PropertyName._backCombatVfxContainer, Variant.From<Node2D>(ref this._backCombatVfxContainer));
			info.AddProperty(NCombatVfxSpawner.PropertyName._combatVfxContainer, Variant.From<Control>(ref this._combatVfxContainer));
			info.AddProperty(NCombatVfxSpawner.PropertyName._env, Variant.From<WorldEnvironment>(ref this._env));
			info.AddProperty(NCombatVfxSpawner.PropertyName._playerTopPosition, Variant.From<Node2D>(ref this._playerTopPosition));
			info.AddProperty(NCombatVfxSpawner.PropertyName._playerPosition, Variant.From<Node2D>(ref this._playerPosition));
			info.AddProperty(NCombatVfxSpawner.PropertyName._playerGroundPosition, Variant.From<Node2D>(ref this._playerGroundPosition));
			info.AddProperty(NCombatVfxSpawner.PropertyName._enemyTopPosition, Variant.From<Node2D>(ref this._enemyTopPosition));
			info.AddProperty(NCombatVfxSpawner.PropertyName._enemyPosition, Variant.From<Node2D>(ref this._enemyPosition));
			info.AddProperty(NCombatVfxSpawner.PropertyName._enemyGroundPosition, Variant.From<Node2D>(ref this._enemyGroundPosition));
			info.AddProperty(NCombatVfxSpawner.PropertyName._defectEyePosition, Variant.From<Node2D>(ref this._defectEyePosition));
			info.AddProperty(NCombatVfxSpawner.PropertyName._lowHpBorderVfx, Variant.From<NLowHpBorderVfx>(ref this._lowHpBorderVfx));
			info.AddProperty(NCombatVfxSpawner.PropertyName._gaseousScreenVfx, Variant.From<NGaseousScreenVfx>(ref this._gaseousScreenVfx));
			info.AddProperty(NCombatVfxSpawner.PropertyName._shiftPressed, Variant.From<bool>(ref this._shiftPressed));
		}

		/// <inheritdoc />
		// Token: 0x060086EA RID: 34538 RVA: 0x002D4F84 File Offset: 0x002D3184
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._backCombatVfxContainer, ref variant))
			{
				this._backCombatVfxContainer = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._combatVfxContainer, ref variant2))
			{
				this._combatVfxContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._env, ref variant3))
			{
				this._env = variant3.As<WorldEnvironment>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._playerTopPosition, ref variant4))
			{
				this._playerTopPosition = variant4.As<Node2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._playerPosition, ref variant5))
			{
				this._playerPosition = variant5.As<Node2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._playerGroundPosition, ref variant6))
			{
				this._playerGroundPosition = variant6.As<Node2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._enemyTopPosition, ref variant7))
			{
				this._enemyTopPosition = variant7.As<Node2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._enemyPosition, ref variant8))
			{
				this._enemyPosition = variant8.As<Node2D>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._enemyGroundPosition, ref variant9))
			{
				this._enemyGroundPosition = variant9.As<Node2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._defectEyePosition, ref variant10))
			{
				this._defectEyePosition = variant10.As<Node2D>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._lowHpBorderVfx, ref variant11))
			{
				this._lowHpBorderVfx = variant11.As<NLowHpBorderVfx>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._gaseousScreenVfx, ref variant12))
			{
				this._gaseousScreenVfx = variant12.As<NGaseousScreenVfx>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._shiftPressed, ref variant13))
			{
				this._shiftPressed = variant13.As<bool>();
			}
		}

		// Token: 0x04002EB7 RID: 11959
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _backCombatVfxContainer;

		// Token: 0x04002EB8 RID: 11960
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Control _combatVfxContainer;

		// Token: 0x04002EB9 RID: 11961
		private WorldEnvironment _env;

		// Token: 0x04002EBA RID: 11962
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _playerTopPosition;

		// Token: 0x04002EBB RID: 11963
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _playerPosition;

		// Token: 0x04002EBC RID: 11964
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _playerGroundPosition;

		// Token: 0x04002EBD RID: 11965
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _enemyTopPosition;

		// Token: 0x04002EBE RID: 11966
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _enemyPosition;

		// Token: 0x04002EBF RID: 11967
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _enemyGroundPosition;

		// Token: 0x04002EC0 RID: 11968
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _defectEyePosition;

		// Token: 0x04002EC1 RID: 11969
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NLowHpBorderVfx _lowHpBorderVfx;

		// Token: 0x04002EC2 RID: 11970
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NGaseousScreenVfx _gaseousScreenVfx;

		// Token: 0x04002EC3 RID: 11971
		private decimal _damage = 10m;

		// Token: 0x04002EC4 RID: 11972
		private bool _shiftPressed;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002414 RID: 9236
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04009871 RID: 39025
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04009872 RID: 39026
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04009873 RID: 39027
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'GetRandomColor' method.
			/// </summary>
			// Token: 0x04009874 RID: 39028
			public static readonly StringName GetRandomColor = "GetRandomColor";

			/// <summary>
			/// Cached name for the 'TestFunctionA' method.
			/// </summary>
			// Token: 0x04009875 RID: 39029
			public static readonly StringName TestFunctionA = "TestFunctionA";

			/// <summary>
			/// Cached name for the 'TestFunctionB' method.
			/// </summary>
			// Token: 0x04009876 RID: 39030
			public static readonly StringName TestFunctionB = "TestFunctionB";

			/// <summary>
			/// Cached name for the 'TestFunctionC' method.
			/// </summary>
			// Token: 0x04009877 RID: 39031
			public static readonly StringName TestFunctionC = "TestFunctionC";

			/// <summary>
			/// Cached name for the 'TestProjectileHandler' method.
			/// </summary>
			// Token: 0x04009878 RID: 39032
			public static readonly StringName TestProjectileHandler = "TestProjectileHandler";

			/// <summary>
			/// Cached name for the 'SpawnVfx' method.
			/// </summary>
			// Token: 0x04009879 RID: 39033
			public static readonly StringName SpawnVfx = "SpawnVfx";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002415 RID: 9237
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_backCombatVfxContainer' field.
			/// </summary>
			// Token: 0x0400987A RID: 39034
			public static readonly StringName _backCombatVfxContainer = "_backCombatVfxContainer";

			/// <summary>
			/// Cached name for the '_combatVfxContainer' field.
			/// </summary>
			// Token: 0x0400987B RID: 39035
			public static readonly StringName _combatVfxContainer = "_combatVfxContainer";

			/// <summary>
			/// Cached name for the '_env' field.
			/// </summary>
			// Token: 0x0400987C RID: 39036
			public static readonly StringName _env = "_env";

			/// <summary>
			/// Cached name for the '_playerTopPosition' field.
			/// </summary>
			// Token: 0x0400987D RID: 39037
			public static readonly StringName _playerTopPosition = "_playerTopPosition";

			/// <summary>
			/// Cached name for the '_playerPosition' field.
			/// </summary>
			// Token: 0x0400987E RID: 39038
			public static readonly StringName _playerPosition = "_playerPosition";

			/// <summary>
			/// Cached name for the '_playerGroundPosition' field.
			/// </summary>
			// Token: 0x0400987F RID: 39039
			public static readonly StringName _playerGroundPosition = "_playerGroundPosition";

			/// <summary>
			/// Cached name for the '_enemyTopPosition' field.
			/// </summary>
			// Token: 0x04009880 RID: 39040
			public static readonly StringName _enemyTopPosition = "_enemyTopPosition";

			/// <summary>
			/// Cached name for the '_enemyPosition' field.
			/// </summary>
			// Token: 0x04009881 RID: 39041
			public static readonly StringName _enemyPosition = "_enemyPosition";

			/// <summary>
			/// Cached name for the '_enemyGroundPosition' field.
			/// </summary>
			// Token: 0x04009882 RID: 39042
			public static readonly StringName _enemyGroundPosition = "_enemyGroundPosition";

			/// <summary>
			/// Cached name for the '_defectEyePosition' field.
			/// </summary>
			// Token: 0x04009883 RID: 39043
			public static readonly StringName _defectEyePosition = "_defectEyePosition";

			/// <summary>
			/// Cached name for the '_lowHpBorderVfx' field.
			/// </summary>
			// Token: 0x04009884 RID: 39044
			public static readonly StringName _lowHpBorderVfx = "_lowHpBorderVfx";

			/// <summary>
			/// Cached name for the '_gaseousScreenVfx' field.
			/// </summary>
			// Token: 0x04009885 RID: 39045
			public static readonly StringName _gaseousScreenVfx = "_gaseousScreenVfx";

			/// <summary>
			/// Cached name for the '_shiftPressed' field.
			/// </summary>
			// Token: 0x04009886 RID: 39046
			public static readonly StringName _shiftPressed = "_shiftPressed";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002416 RID: 9238
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
