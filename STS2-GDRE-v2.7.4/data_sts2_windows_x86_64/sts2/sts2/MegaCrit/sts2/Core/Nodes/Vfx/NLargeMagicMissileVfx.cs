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
	// Token: 0x020001C9 RID: 457
	[ScriptPath("res://src/Core/Nodes/Vfx/NLargeMagicMissileVfx.cs")]
	public class NLargeMagicMissileVfx : Node2D
	{
		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060016ED RID: 5869 RVA: 0x0008DE00 File Offset: 0x0008C000
		// (set) Token: 0x060016EE RID: 5870 RVA: 0x0008DE08 File Offset: 0x0008C008
		public float WaitTime { get; private set; } = 0.2f;

		// Token: 0x060016EF RID: 5871 RVA: 0x0008DE14 File Offset: 0x0008C014
		[NullableContext(2)]
		public static NLargeMagicMissileVfx Create(Vector2 targetFloorPosition, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NLargeMagicMissileVfx nlargeMagicMissileVfx = PreloadManager.Cache.GetScene(NLargeMagicMissileVfx.scenePath).Instantiate<NLargeMagicMissileVfx>(0L);
			nlargeMagicMissileVfx.GlobalPosition = targetFloorPosition;
			nlargeMagicMissileVfx.Initialize();
			nlargeMagicMissileVfx.ModulateParticles(tint);
			return nlargeMagicMissileVfx;
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x0008DE58 File Offset: 0x0008C058
		private Vector2 GetProjectileDirection()
		{
			Vector3 vector = Quaternion.FromEuler(new Vector3(0f, 0f, Mathf.DegToRad(-30f))) * Vector3.Up;
			return new Vector2(vector.X, vector.Y).Normalized();
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x0008DEAC File Offset: 0x0008C0AC
		private Vector2 GetTopPosition(Vector2 projectileDirection)
		{
			return (Vector2)Geometry2D.LineIntersectsLine(base.GlobalPosition, projectileDirection, new Vector2(0f, 80f), Vector2.Right);
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x0008DEE0 File Offset: 0x0008C0E0
		private void Initialize()
		{
			Vector2 projectileDirection = this.GetProjectileDirection();
			Vector2 topPosition = this.GetTopPosition(projectileDirection);
			this._anticipationContainer.GlobalPosition = topPosition;
			this._projectileStartPoint.GlobalPosition = topPosition + projectileDirection * this._projectileOffset;
			this._projectileEndPoint.GlobalPosition = base.GlobalPosition + projectileDirection * this._projectileOffset;
			this._projectileContainer.Visible = false;
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x0008DF54 File Offset: 0x0008C154
		private void ModulateParticles(Color tint)
		{
			for (int i = 0; i < this._modulateParticles.Count; i++)
			{
				this._modulateParticles[i].SelfModulate = tint;
			}
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x0008DF89 File Offset: 0x0008C189
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x0008DF97 File Offset: 0x0008C197
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x0008DFAC File Offset: 0x0008C1AC
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
					instance.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, -1f);
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
		// Token: 0x060016F7 RID: 5879 RVA: 0x0008DFF0 File Offset: 0x0008C1F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NLargeMagicMissileVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetFloorPosition", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NLargeMagicMissileVfx.MethodName.GetProjectileDirection, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLargeMagicMissileVfx.MethodName.GetTopPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "projectileDirection", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NLargeMagicMissileVfx.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLargeMagicMissileVfx.MethodName.ModulateParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NLargeMagicMissileVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLargeMagicMissileVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016F8 RID: 5880 RVA: 0x0008E1EC File Offset: 0x0008C3EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLargeMagicMissileVfx.MethodName.Create && args.Count == 2)
			{
				NLargeMagicMissileVfx nlargeMagicMissileVfx = NLargeMagicMissileVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NLargeMagicMissileVfx>(ref nlargeMagicMissileVfx);
				return true;
			}
			if ((ref method) == NLargeMagicMissileVfx.MethodName.GetProjectileDirection && args.Count == 0)
			{
				Vector2 projectileDirection = this.GetProjectileDirection();
				ret = VariantUtils.CreateFrom<Vector2>(ref projectileDirection);
				return true;
			}
			if ((ref method) == NLargeMagicMissileVfx.MethodName.GetTopPosition && args.Count == 1)
			{
				Vector2 topPosition = this.GetTopPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<Vector2>(ref topPosition);
				return true;
			}
			if ((ref method) == NLargeMagicMissileVfx.MethodName.Initialize && args.Count == 0)
			{
				this.Initialize();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLargeMagicMissileVfx.MethodName.ModulateParticles && args.Count == 1)
			{
				this.ModulateParticles(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLargeMagicMissileVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLargeMagicMissileVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x0008E350 File Offset: 0x0008C550
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLargeMagicMissileVfx.MethodName.Create && args.Count == 2)
			{
				NLargeMagicMissileVfx nlargeMagicMissileVfx = NLargeMagicMissileVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NLargeMagicMissileVfx>(ref nlargeMagicMissileVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060016FA RID: 5882 RVA: 0x0008E3AC File Offset: 0x0008C5AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLargeMagicMissileVfx.MethodName.Create || (ref method) == NLargeMagicMissileVfx.MethodName.GetProjectileDirection || (ref method) == NLargeMagicMissileVfx.MethodName.GetTopPosition || (ref method) == NLargeMagicMissileVfx.MethodName.Initialize || (ref method) == NLargeMagicMissileVfx.MethodName.ModulateParticles || (ref method) == NLargeMagicMissileVfx.MethodName._Ready || (ref method) == NLargeMagicMissileVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060016FB RID: 5883 RVA: 0x0008E42C File Offset: 0x0008C62C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLargeMagicMissileVfx.PropertyName.WaitTime)
			{
				this.WaitTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._anticipationParticles)
			{
				this._anticipationParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileStartParticles)
			{
				this._projectileStartParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileParticles)
			{
				this._projectileParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._impactParticles)
			{
				this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._modulateParticles)
			{
				this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._anticipationContainer)
			{
				this._anticipationContainer = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._anticipationDuration)
			{
				this._anticipationDuration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileContainer)
			{
				this._projectileContainer = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileStartPoint)
			{
				this._projectileStartPoint = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileEndPoint)
			{
				this._projectileEndPoint = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileOffset)
			{
				this._projectileOffset = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060016FC RID: 5884 RVA: 0x0008E588 File Offset: 0x0008C788
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLargeMagicMissileVfx.PropertyName.WaitTime)
			{
				float waitTime = this.WaitTime;
				value = VariantUtils.CreateFrom<float>(ref waitTime);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._anticipationParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._anticipationParticles);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileStartParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._projectileStartParticles);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._projectileParticles);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._impactParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._modulateParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._anticipationContainer)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._anticipationContainer);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._anticipationDuration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._anticipationDuration);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileContainer)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._projectileContainer);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileStartPoint)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._projectileStartPoint);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileEndPoint)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._projectileEndPoint);
				return true;
			}
			if ((ref name) == NLargeMagicMissileVfx.PropertyName._projectileOffset)
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
		// Token: 0x060016FD RID: 5885 RVA: 0x0008E720 File Offset: 0x0008C920
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NLargeMagicMissileVfx.PropertyName.WaitTime, 0L, "", 4096L, false),
				new PropertyInfo(28L, NLargeMagicMissileVfx.PropertyName._anticipationParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NLargeMagicMissileVfx.PropertyName._projectileStartParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NLargeMagicMissileVfx.PropertyName._projectileParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NLargeMagicMissileVfx.PropertyName._impactParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NLargeMagicMissileVfx.PropertyName._modulateParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NLargeMagicMissileVfx.PropertyName._anticipationContainer, 34L, "Node2D", 4102L, true),
				new PropertyInfo(3L, NLargeMagicMissileVfx.PropertyName._anticipationDuration, 0L, "", 4102L, true),
				new PropertyInfo(24L, NLargeMagicMissileVfx.PropertyName._projectileContainer, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NLargeMagicMissileVfx.PropertyName._projectileStartPoint, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NLargeMagicMissileVfx.PropertyName._projectileEndPoint, 34L, "Node2D", 4102L, true),
				new PropertyInfo(3L, NLargeMagicMissileVfx.PropertyName._projectileOffset, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016FE RID: 5886 RVA: 0x0008E8C8 File Offset: 0x0008CAC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName waitTime = NLargeMagicMissileVfx.PropertyName.WaitTime;
			float waitTime2 = this.WaitTime;
			info.AddProperty(waitTime, Variant.From<float>(ref waitTime2));
			info.AddProperty(NLargeMagicMissileVfx.PropertyName._anticipationParticles, Variant.CreateFrom<GpuParticles2D>(this._anticipationParticles));
			info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileStartParticles, Variant.CreateFrom<GpuParticles2D>(this._projectileStartParticles));
			info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileParticles, Variant.CreateFrom<GpuParticles2D>(this._projectileParticles));
			info.AddProperty(NLargeMagicMissileVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
			info.AddProperty(NLargeMagicMissileVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
			info.AddProperty(NLargeMagicMissileVfx.PropertyName._anticipationContainer, Variant.From<Node2D>(ref this._anticipationContainer));
			info.AddProperty(NLargeMagicMissileVfx.PropertyName._anticipationDuration, Variant.From<float>(ref this._anticipationDuration));
			info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileContainer, Variant.From<Node2D>(ref this._projectileContainer));
			info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileStartPoint, Variant.From<Node2D>(ref this._projectileStartPoint));
			info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileEndPoint, Variant.From<Node2D>(ref this._projectileEndPoint));
			info.AddProperty(NLargeMagicMissileVfx.PropertyName._projectileOffset, Variant.From<float>(ref this._projectileOffset));
		}

		/// <inheritdoc />
		// Token: 0x060016FF RID: 5887 RVA: 0x0008E9E8 File Offset: 0x0008CBE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName.WaitTime, ref variant))
			{
				this.WaitTime = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._anticipationParticles, ref variant2))
			{
				this._anticipationParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileStartParticles, ref variant3))
			{
				this._projectileStartParticles = variant3.AsGodotArray<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileParticles, ref variant4))
			{
				this._projectileParticles = variant4.AsGodotArray<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._impactParticles, ref variant5))
			{
				this._impactParticles = variant5.AsGodotArray<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._modulateParticles, ref variant6))
			{
				this._modulateParticles = variant6.AsGodotArray<GpuParticles2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._anticipationContainer, ref variant7))
			{
				this._anticipationContainer = variant7.As<Node2D>();
			}
			Variant variant8;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._anticipationDuration, ref variant8))
			{
				this._anticipationDuration = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileContainer, ref variant9))
			{
				this._projectileContainer = variant9.As<Node2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileStartPoint, ref variant10))
			{
				this._projectileStartPoint = variant10.As<Node2D>();
			}
			Variant variant11;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileEndPoint, ref variant11))
			{
				this._projectileEndPoint = variant11.As<Node2D>();
			}
			Variant variant12;
			if (info.TryGetProperty(NLargeMagicMissileVfx.PropertyName._projectileOffset, ref variant12))
			{
				this._projectileOffset = variant12.As<float>();
			}
		}

		// Token: 0x04000831 RID: 2097
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_large_magic_missile");

		// Token: 0x04000833 RID: 2099
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _anticipationParticles = new Array<GpuParticles2D>();

		// Token: 0x04000834 RID: 2100
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _projectileStartParticles = new Array<GpuParticles2D>();

		// Token: 0x04000835 RID: 2101
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _projectileParticles = new Array<GpuParticles2D>();

		// Token: 0x04000836 RID: 2102
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();

		// Token: 0x04000837 RID: 2103
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();

		// Token: 0x04000838 RID: 2104
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _anticipationContainer;

		// Token: 0x04000839 RID: 2105
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _anticipationDuration = 0.2f;

		// Token: 0x0400083A RID: 2106
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _projectileContainer;

		// Token: 0x0400083B RID: 2107
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _projectileStartPoint;

		// Token: 0x0400083C RID: 2108
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _projectileEndPoint;

		// Token: 0x0400083D RID: 2109
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _projectileOffset = 100f;

		// Token: 0x0400083E RID: 2110
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200109C RID: 4252
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400414A RID: 16714
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'GetProjectileDirection' method.
			/// </summary>
			// Token: 0x0400414B RID: 16715
			public static readonly StringName GetProjectileDirection = "GetProjectileDirection";

			/// <summary>
			/// Cached name for the 'GetTopPosition' method.
			/// </summary>
			// Token: 0x0400414C RID: 16716
			public static readonly StringName GetTopPosition = "GetTopPosition";

			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x0400414D RID: 16717
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the 'ModulateParticles' method.
			/// </summary>
			// Token: 0x0400414E RID: 16718
			public static readonly StringName ModulateParticles = "ModulateParticles";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400414F RID: 16719
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004150 RID: 16720
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200109D RID: 4253
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'WaitTime' property.
			/// </summary>
			// Token: 0x04004151 RID: 16721
			public static readonly StringName WaitTime = "WaitTime";

			/// <summary>
			/// Cached name for the '_anticipationParticles' field.
			/// </summary>
			// Token: 0x04004152 RID: 16722
			public static readonly StringName _anticipationParticles = "_anticipationParticles";

			/// <summary>
			/// Cached name for the '_projectileStartParticles' field.
			/// </summary>
			// Token: 0x04004153 RID: 16723
			public static readonly StringName _projectileStartParticles = "_projectileStartParticles";

			/// <summary>
			/// Cached name for the '_projectileParticles' field.
			/// </summary>
			// Token: 0x04004154 RID: 16724
			public static readonly StringName _projectileParticles = "_projectileParticles";

			/// <summary>
			/// Cached name for the '_impactParticles' field.
			/// </summary>
			// Token: 0x04004155 RID: 16725
			public static readonly StringName _impactParticles = "_impactParticles";

			/// <summary>
			/// Cached name for the '_modulateParticles' field.
			/// </summary>
			// Token: 0x04004156 RID: 16726
			public static readonly StringName _modulateParticles = "_modulateParticles";

			/// <summary>
			/// Cached name for the '_anticipationContainer' field.
			/// </summary>
			// Token: 0x04004157 RID: 16727
			public static readonly StringName _anticipationContainer = "_anticipationContainer";

			/// <summary>
			/// Cached name for the '_anticipationDuration' field.
			/// </summary>
			// Token: 0x04004158 RID: 16728
			public static readonly StringName _anticipationDuration = "_anticipationDuration";

			/// <summary>
			/// Cached name for the '_projectileContainer' field.
			/// </summary>
			// Token: 0x04004159 RID: 16729
			public static readonly StringName _projectileContainer = "_projectileContainer";

			/// <summary>
			/// Cached name for the '_projectileStartPoint' field.
			/// </summary>
			// Token: 0x0400415A RID: 16730
			public static readonly StringName _projectileStartPoint = "_projectileStartPoint";

			/// <summary>
			/// Cached name for the '_projectileEndPoint' field.
			/// </summary>
			// Token: 0x0400415B RID: 16731
			public static readonly StringName _projectileEndPoint = "_projectileEndPoint";

			/// <summary>
			/// Cached name for the '_projectileOffset' field.
			/// </summary>
			// Token: 0x0400415C RID: 16732
			public static readonly StringName _projectileOffset = "_projectileOffset";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200109E RID: 4254
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
