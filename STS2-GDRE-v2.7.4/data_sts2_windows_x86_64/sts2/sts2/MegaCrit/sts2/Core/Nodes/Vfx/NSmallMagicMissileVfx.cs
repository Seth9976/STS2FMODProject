using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001F2 RID: 498
	[ScriptPath("res://src/Core/Nodes/Vfx/NSmallMagicMissileVfx.cs")]
	public class NSmallMagicMissileVfx : Node2D
	{
		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x0009FE64 File Offset: 0x0009E064
		// (set) Token: 0x0600196B RID: 6507 RVA: 0x0009FE6C File Offset: 0x0009E06C
		public float WaitTime { get; private set; } = 0.2f;

		// Token: 0x0600196C RID: 6508 RVA: 0x0009FE78 File Offset: 0x0009E078
		[NullableContext(2)]
		public static NSmallMagicMissileVfx Create(Vector2 targetCenterPosition, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSmallMagicMissileVfx nsmallMagicMissileVfx = PreloadManager.Cache.GetScene(NSmallMagicMissileVfx.scenePath).Instantiate<NSmallMagicMissileVfx>(0L);
			nsmallMagicMissileVfx.GlobalPosition = targetCenterPosition;
			nsmallMagicMissileVfx.Initialize();
			nsmallMagicMissileVfx.ModulateParticles(tint);
			return nsmallMagicMissileVfx;
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x0009FEBC File Offset: 0x0009E0BC
		private Vector2 GetProjectileDirection()
		{
			Vector3 vector = Quaternion.FromEuler(new Vector3(0f, 0f, Mathf.DegToRad(-30f))) * Vector3.Up;
			return new Vector2(vector.X, vector.Y).Normalized();
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0009FF10 File Offset: 0x0009E110
		private Vector2 GetTopPosition(Vector2 projectileDirection)
		{
			return (Vector2)Geometry2D.LineIntersectsLine(base.GlobalPosition, projectileDirection, new Vector2(0f, 80f), Vector2.Right);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0009FF44 File Offset: 0x0009E144
		private void Initialize()
		{
			Vector2 projectileDirection = this.GetProjectileDirection();
			Vector2 topPosition = this.GetTopPosition(projectileDirection);
			this._anticipationContainer.GlobalPosition = topPosition;
			this._projectileStartPoint.GlobalPosition = topPosition + projectileDirection * this._projectileOffset;
			this._projectileEndPoint.GlobalPosition = base.GlobalPosition + projectileDirection * this._projectileOffset;
			this._projectileContainer.Visible = false;
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0009FFB8 File Offset: 0x0009E1B8
		private void ModulateParticles(Color tint)
		{
			for (int i = 0; i < this._modulateParticles.Count; i++)
			{
				this._modulateParticles[i].SelfModulate = tint;
			}
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x0009FFED File Offset: 0x0009E1ED
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0009FFFB File Offset: 0x0009E1FB
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x000A0010 File Offset: 0x0009E210
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			for (int i = 0; i < this._anticipationParticles.Count; i++)
			{
				this._anticipationParticles[i].Restart();
			}
			await Cmd.Wait(this._anticipationDuration, this._cts.Token, false);
			for (int j = 0; j < this._projectileStartParticles.Count; j++)
			{
				this._projectileStartParticles[j].Restart();
			}
			this._projectileContainer.GlobalPosition = this._projectileStartPoint.GlobalPosition;
			this._projectileContainer.Visible = true;
			for (int k = 0; k < this._projectileParticles.Count; k++)
			{
				this._projectileParticles[k].Restart();
			}
			double timer = 0.0;
			while (timer < (double)this.WaitTime && !this._cts.IsCancellationRequested)
			{
				float num = (float)timer / this.WaitTime;
				this._projectileContainer.GlobalPosition = this._projectileStartPoint.GlobalPosition.Lerp(this._projectileEndPoint.GlobalPosition, num);
				timer += base.GetProcessDeltaTime();
				await this.AwaitProcessFrame(default(CancellationToken));
			}
			if (!this._cts.IsCancellationRequested)
			{
				this._projectileContainer.Visible = false;
				for (int l = 0; l < this._impactParticles.Count; l++)
				{
					this._impactParticles[l].Restart();
				}
				NGame instance = NGame.Instance;
				if (instance != null)
				{
					instance.ScreenShake(ShakeStrength.Medium, ShakeDuration.Short, -1f);
				}
				await Cmd.Wait(2f, this._cts.Token, false);
				this.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001974 RID: 6516 RVA: 0x000A0054 File Offset: 0x0009E254
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NSmallMagicMissileVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSmallMagicMissileVfx.MethodName.GetProjectileDirection, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSmallMagicMissileVfx.MethodName.GetTopPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "projectileDirection", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSmallMagicMissileVfx.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSmallMagicMissileVfx.MethodName.ModulateParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSmallMagicMissileVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSmallMagicMissileVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001975 RID: 6517 RVA: 0x000A0250 File Offset: 0x0009E450
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSmallMagicMissileVfx.MethodName.Create && args.Count == 2)
			{
				NSmallMagicMissileVfx nsmallMagicMissileVfx = NSmallMagicMissileVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NSmallMagicMissileVfx>(ref nsmallMagicMissileVfx);
				return true;
			}
			if ((ref method) == NSmallMagicMissileVfx.MethodName.GetProjectileDirection && args.Count == 0)
			{
				Vector2 projectileDirection = this.GetProjectileDirection();
				ret = VariantUtils.CreateFrom<Vector2>(ref projectileDirection);
				return true;
			}
			if ((ref method) == NSmallMagicMissileVfx.MethodName.GetTopPosition && args.Count == 1)
			{
				Vector2 topPosition = this.GetTopPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<Vector2>(ref topPosition);
				return true;
			}
			if ((ref method) == NSmallMagicMissileVfx.MethodName.Initialize && args.Count == 0)
			{
				this.Initialize();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSmallMagicMissileVfx.MethodName.ModulateParticles && args.Count == 1)
			{
				this.ModulateParticles(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSmallMagicMissileVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSmallMagicMissileVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x000A03B4 File Offset: 0x0009E5B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSmallMagicMissileVfx.MethodName.Create && args.Count == 2)
			{
				NSmallMagicMissileVfx nsmallMagicMissileVfx = NSmallMagicMissileVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NSmallMagicMissileVfx>(ref nsmallMagicMissileVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001977 RID: 6519 RVA: 0x000A0410 File Offset: 0x0009E610
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSmallMagicMissileVfx.MethodName.Create || (ref method) == NSmallMagicMissileVfx.MethodName.GetProjectileDirection || (ref method) == NSmallMagicMissileVfx.MethodName.GetTopPosition || (ref method) == NSmallMagicMissileVfx.MethodName.Initialize || (ref method) == NSmallMagicMissileVfx.MethodName.ModulateParticles || (ref method) == NSmallMagicMissileVfx.MethodName._Ready || (ref method) == NSmallMagicMissileVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001978 RID: 6520 RVA: 0x000A0490 File Offset: 0x0009E690
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSmallMagicMissileVfx.PropertyName.WaitTime)
			{
				this.WaitTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._anticipationParticles)
			{
				this._anticipationParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileStartParticles)
			{
				this._projectileStartParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileParticles)
			{
				this._projectileParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._impactParticles)
			{
				this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._modulateParticles)
			{
				this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._anticipationContainer)
			{
				this._anticipationContainer = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._anticipationDuration)
			{
				this._anticipationDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileContainer)
			{
				this._projectileContainer = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileStartPoint)
			{
				this._projectileStartPoint = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileEndPoint)
			{
				this._projectileEndPoint = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileOffset)
			{
				this._projectileOffset = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001979 RID: 6521 RVA: 0x000A05EC File Offset: 0x0009E7EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSmallMagicMissileVfx.PropertyName.WaitTime)
			{
				float waitTime = this.WaitTime;
				value = VariantUtils.CreateFrom<float>(ref waitTime);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._anticipationParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._anticipationParticles);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileStartParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._projectileStartParticles);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._projectileParticles);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._impactParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._modulateParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._anticipationContainer)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._anticipationContainer);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._anticipationDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._anticipationDuration);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileContainer)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._projectileContainer);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileStartPoint)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._projectileStartPoint);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileEndPoint)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._projectileEndPoint);
				return true;
			}
			if ((ref name) == NSmallMagicMissileVfx.PropertyName._projectileOffset)
			{
				value = VariantUtils.CreateFrom<float>(ref this._projectileOffset);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600197A RID: 6522 RVA: 0x000A0784 File Offset: 0x0009E984
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NSmallMagicMissileVfx.PropertyName.WaitTime, 0L, "", 4096L, false),
				new PropertyInfo(28L, NSmallMagicMissileVfx.PropertyName._anticipationParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NSmallMagicMissileVfx.PropertyName._projectileStartParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NSmallMagicMissileVfx.PropertyName._projectileParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NSmallMagicMissileVfx.PropertyName._impactParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NSmallMagicMissileVfx.PropertyName._modulateParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NSmallMagicMissileVfx.PropertyName._anticipationContainer, 34L, "Node2D", 4102L, true),
				new PropertyInfo(3L, NSmallMagicMissileVfx.PropertyName._anticipationDuration, 0L, "", 4102L, true),
				new PropertyInfo(24L, NSmallMagicMissileVfx.PropertyName._projectileContainer, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NSmallMagicMissileVfx.PropertyName._projectileStartPoint, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NSmallMagicMissileVfx.PropertyName._projectileEndPoint, 34L, "Node2D", 4102L, true),
				new PropertyInfo(3L, NSmallMagicMissileVfx.PropertyName._projectileOffset, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600197B RID: 6523 RVA: 0x000A092C File Offset: 0x0009EB2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName waitTime = NSmallMagicMissileVfx.PropertyName.WaitTime;
			float waitTime2 = this.WaitTime;
			info.AddProperty(waitTime, Variant.From<float>(ref waitTime2));
			info.AddProperty(NSmallMagicMissileVfx.PropertyName._anticipationParticles, Variant.CreateFrom<GpuParticles2D>(this._anticipationParticles));
			info.AddProperty(NSmallMagicMissileVfx.PropertyName._projectileStartParticles, Variant.CreateFrom<GpuParticles2D>(this._projectileStartParticles));
			info.AddProperty(NSmallMagicMissileVfx.PropertyName._projectileParticles, Variant.CreateFrom<GpuParticles2D>(this._projectileParticles));
			info.AddProperty(NSmallMagicMissileVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
			info.AddProperty(NSmallMagicMissileVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
			info.AddProperty(NSmallMagicMissileVfx.PropertyName._anticipationContainer, Variant.From<Node2D>(ref this._anticipationContainer));
			info.AddProperty(NSmallMagicMissileVfx.PropertyName._anticipationDuration, Variant.From<float>(ref this._anticipationDuration));
			info.AddProperty(NSmallMagicMissileVfx.PropertyName._projectileContainer, Variant.From<Node2D>(ref this._projectileContainer));
			info.AddProperty(NSmallMagicMissileVfx.PropertyName._projectileStartPoint, Variant.From<Node2D>(ref this._projectileStartPoint));
			info.AddProperty(NSmallMagicMissileVfx.PropertyName._projectileEndPoint, Variant.From<Node2D>(ref this._projectileEndPoint));
			info.AddProperty(NSmallMagicMissileVfx.PropertyName._projectileOffset, Variant.From<float>(ref this._projectileOffset));
		}

		/// <inheritdoc />
		// Token: 0x0600197C RID: 6524 RVA: 0x000A0A4C File Offset: 0x0009EC4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName.WaitTime, ref variant))
			{
				this.WaitTime = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName._anticipationParticles, ref variant2))
			{
				this._anticipationParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName._projectileStartParticles, ref variant3))
			{
				this._projectileStartParticles = variant3.AsGodotArray<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName._projectileParticles, ref variant4))
			{
				this._projectileParticles = variant4.AsGodotArray<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName._impactParticles, ref variant5))
			{
				this._impactParticles = variant5.AsGodotArray<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName._modulateParticles, ref variant6))
			{
				this._modulateParticles = variant6.AsGodotArray<GpuParticles2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName._anticipationContainer, ref variant7))
			{
				this._anticipationContainer = variant7.As<Node2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName._anticipationDuration, ref variant8))
			{
				this._anticipationDuration = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName._projectileContainer, ref variant9))
			{
				this._projectileContainer = variant9.As<Node2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName._projectileStartPoint, ref variant10))
			{
				this._projectileStartPoint = variant10.As<Node2D>();
			}
			Variant variant11;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName._projectileEndPoint, ref variant11))
			{
				this._projectileEndPoint = variant11.As<Node2D>();
			}
			Variant variant12;
			if (info.TryGetProperty(NSmallMagicMissileVfx.PropertyName._projectileOffset, ref variant12))
			{
				this._projectileOffset = variant12.As<float>();
			}
		}

		// Token: 0x04000940 RID: 2368
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_small_magic_missile");

		// Token: 0x04000942 RID: 2370
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _anticipationParticles = new Array<GpuParticles2D>();

		// Token: 0x04000943 RID: 2371
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _projectileStartParticles = new Array<GpuParticles2D>();

		// Token: 0x04000944 RID: 2372
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _projectileParticles = new Array<GpuParticles2D>();

		// Token: 0x04000945 RID: 2373
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();

		// Token: 0x04000946 RID: 2374
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();

		// Token: 0x04000947 RID: 2375
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _anticipationContainer;

		// Token: 0x04000948 RID: 2376
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _anticipationDuration = 0.2f;

		// Token: 0x04000949 RID: 2377
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _projectileContainer;

		// Token: 0x0400094A RID: 2378
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _projectileStartPoint;

		// Token: 0x0400094B RID: 2379
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _projectileEndPoint;

		// Token: 0x0400094C RID: 2380
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _projectileOffset = 100f;

		// Token: 0x0400094D RID: 2381
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001135 RID: 4405
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400431D RID: 17181
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'GetProjectileDirection' method.
			/// </summary>
			// Token: 0x0400431E RID: 17182
			public static readonly StringName GetProjectileDirection = "GetProjectileDirection";

			/// <summary>
			/// Cached name for the 'GetTopPosition' method.
			/// </summary>
			// Token: 0x0400431F RID: 17183
			public static readonly StringName GetTopPosition = "GetTopPosition";

			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x04004320 RID: 17184
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the 'ModulateParticles' method.
			/// </summary>
			// Token: 0x04004321 RID: 17185
			public static readonly StringName ModulateParticles = "ModulateParticles";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004322 RID: 17186
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004323 RID: 17187
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001136 RID: 4406
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'WaitTime' property.
			/// </summary>
			// Token: 0x04004324 RID: 17188
			public static readonly StringName WaitTime = "WaitTime";

			/// <summary>
			/// Cached name for the '_anticipationParticles' field.
			/// </summary>
			// Token: 0x04004325 RID: 17189
			public static readonly StringName _anticipationParticles = "_anticipationParticles";

			/// <summary>
			/// Cached name for the '_projectileStartParticles' field.
			/// </summary>
			// Token: 0x04004326 RID: 17190
			public static readonly StringName _projectileStartParticles = "_projectileStartParticles";

			/// <summary>
			/// Cached name for the '_projectileParticles' field.
			/// </summary>
			// Token: 0x04004327 RID: 17191
			public static readonly StringName _projectileParticles = "_projectileParticles";

			/// <summary>
			/// Cached name for the '_impactParticles' field.
			/// </summary>
			// Token: 0x04004328 RID: 17192
			public static readonly StringName _impactParticles = "_impactParticles";

			/// <summary>
			/// Cached name for the '_modulateParticles' field.
			/// </summary>
			// Token: 0x04004329 RID: 17193
			public static readonly StringName _modulateParticles = "_modulateParticles";

			/// <summary>
			/// Cached name for the '_anticipationContainer' field.
			/// </summary>
			// Token: 0x0400432A RID: 17194
			public static readonly StringName _anticipationContainer = "_anticipationContainer";

			/// <summary>
			/// Cached name for the '_anticipationDuration' field.
			/// </summary>
			// Token: 0x0400432B RID: 17195
			public static readonly StringName _anticipationDuration = "_anticipationDuration";

			/// <summary>
			/// Cached name for the '_projectileContainer' field.
			/// </summary>
			// Token: 0x0400432C RID: 17196
			public static readonly StringName _projectileContainer = "_projectileContainer";

			/// <summary>
			/// Cached name for the '_projectileStartPoint' field.
			/// </summary>
			// Token: 0x0400432D RID: 17197
			public static readonly StringName _projectileStartPoint = "_projectileStartPoint";

			/// <summary>
			/// Cached name for the '_projectileEndPoint' field.
			/// </summary>
			// Token: 0x0400432E RID: 17198
			public static readonly StringName _projectileEndPoint = "_projectileEndPoint";

			/// <summary>
			/// Cached name for the '_projectileOffset' field.
			/// </summary>
			// Token: 0x0400432F RID: 17199
			public static readonly StringName _projectileOffset = "_projectileOffset";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001137 RID: 4407
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
