using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// Base class for C# Spine bindings.
	/// </summary>
	/// <remarks>
	/// Lifetime gotcha for any wrapper around a native-created RefCounted, not just Spine: while the managed
	/// wrapper is the sole owner, Godot gives it a weak GCHandle, so the GC can finalize it (freeing the native
	/// object) the moment no managed code will read it again, even mid-call into native. The classic trigger is
	/// an unrooted temporary in a chain like wrapper.GetX().GetName(): GetX() returns a fresh sole-owned
	/// wrapper, collectible while the native GetName() is still using its pointer. Use-after-free on the
	/// finalizer thread.
	///
	/// The defense is the choke point in Call below, which keeps the receiver rooted via GC.KeepAlive across
	/// every native call routed through it, plus value-returning accessors so transient wrappers never escape.
	/// Keep that pattern in any new binding of this kind. See GC.KeepAlive
	/// (https://learn.microsoft.com/dotnet/api/system.gc.keepalive) and PRG-6985.
	/// </remarks>
	// Token: 0x02000D65 RID: 3429
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class MegaSpineBinding : IDisposable
	{
		/// <summary>
		/// Deterministically releases the wrapped native object on the CALLING thread. Every Spine wrapper
		/// connects to its owner's _internal_spine_objects_invalidated signal on construction and disconnects
		/// on native destruction (spine-godot SpineObjectWrapper). Godot's Object signal connect/disconnect is
		/// not thread-safe, so if a transient wrapper is left to the .NET finalizer thread, its disconnect can
		/// race a main-thread read's connect on the same shared owner and corrupt the connection list (a
		/// finalizer-thread heap corruption, PRG-6985). Disposing transient wrappers here keeps both the
		/// connect and the disconnect on one thread. Long-lived wrappers (MegaSprite/MegaAnimationState the
		/// caller retains) are simply never disposed through this path.
		/// </summary>
		// Token: 0x06008A84 RID: 35460 RVA: 0x002E4C50 File Offset: 0x002E2E50
		public void Dispose()
		{
			this.BoundObject.Dispose();
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// The Spine GodotObject that this binding wraps.
		/// ONLY USE THIS TO PASS TO OTHER SPINE API METHODS!
		/// If you want to call methods directly on this object, you probably want to add a method binding for the relevant
		/// MegaSpineBinding subclass instead.
		/// </summary>
		// Token: 0x1700261F RID: 9759
		// (get) Token: 0x06008A85 RID: 35461 RVA: 0x002E4C63 File Offset: 0x002E2E63
		// (set) Token: 0x06008A86 RID: 35462 RVA: 0x002E4C6B File Offset: 0x002E2E6B
		public GodotObject BoundObject { get; private set; }

		/// <summary>
		/// The name of this class in the Spine API (https://en.esotericsoftware.com/spine-api-reference).
		/// </summary>
		// Token: 0x17002620 RID: 9760
		// (get) Token: 0x06008A87 RID: 35463
		protected abstract string SpineClassName { get; }

		/// <summary>
		/// The methods from the Spine API (https://en.esotericsoftware.com/spine-api-reference) that this binding exposes.
		/// </summary>
		// Token: 0x17002621 RID: 9761
		// (get) Token: 0x06008A88 RID: 35464 RVA: 0x002E4C74 File Offset: 0x002E2E74
		protected virtual IEnumerable<string> SpineMethods
		{
			get
			{
				return Array.Empty<string>();
			}
		}

		/// <summary>
		/// The signals from the Spine API (https://en.esotericsoftware.com/spine-api-reference) that this binding exposes.
		/// </summary>
		// Token: 0x17002622 RID: 9762
		// (get) Token: 0x06008A89 RID: 35465 RVA: 0x002E4C7B File Offset: 0x002E2E7B
		protected virtual IEnumerable<string> SpineSignals
		{
			get
			{
				return Array.Empty<string>();
			}
		}

		// Token: 0x06008A8A RID: 35466 RVA: 0x002E4C84 File Offset: 0x002E2E84
		protected MegaSpineBinding(Variant native)
		{
			if (native.VariantType != 24L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Expected a GodotObject but was ");
				defaultInterpolatedStringHandler.AppendFormatted<Variant.Type>(native.VariantType);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.BoundObject = native.AsGodotObject();
			this.ValidateBoundObject();
		}

		/// <summary>
		/// Connect to the specified signal on the bound Spine object.
		/// </summary>
		/// <param name="signalName">Name of the signal to connect to.</param>
		/// <param name="callable">Callable to connect to the signal.</param>
		// Token: 0x06008A8B RID: 35467 RVA: 0x002E4CF3 File Offset: 0x002E2EF3
		protected Error Connect(string signalName, Callable callable)
		{
			return this.BoundObject.Connect(signalName, callable, 0U);
		}

		/// <summary>
		/// Disconnect from the specified signal on the bound Spine object.
		/// </summary>
		/// <param name="signalName">Name of the signal to disconnect from.</param>
		/// <param name="callable">Callable to disconnect from the signal.</param>
		// Token: 0x06008A8C RID: 35468 RVA: 0x002E4D08 File Offset: 0x002E2F08
		protected void Disconnect(string signalName, Callable callable)
		{
			this.BoundObject.Disconnect(signalName, callable);
		}

		/// <summary>
		/// Call the specified method on the bound Spine object
		/// </summary>
		/// <param name="methodName">Name of method to call.</param>
		/// <param name="args">Arguments to pass to the method.</param>
		/// <returns>Value returned by the bound Spine object, cast to the expected type.</returns>
		// Token: 0x06008A8D RID: 35469 RVA: 0x002E4D1C File Offset: 0x002E2F1C
		protected Variant Call(string methodName, params Variant[] args)
		{
			if (!this.SpineMethods.Contains(methodName))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
				defaultInterpolatedStringHandler.AppendLiteral("You must add ");
				defaultInterpolatedStringHandler.AppendFormatted(methodName);
				defaultInterpolatedStringHandler.AppendLiteral(" to ");
				defaultInterpolatedStringHandler.AppendFormatted(base.GetType().Name);
				defaultInterpolatedStringHandler.AppendLiteral(".SpineMethods before calling it!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Variant variant = this.BoundObject.Call(methodName, args);
			GC.KeepAlive(this.BoundObject);
			GC.KeepAlive(args);
			return variant;
		}

		// Token: 0x06008A8E RID: 35470 RVA: 0x002E4DB4 File Offset: 0x002E2FB4
		protected Variant? CallNullable(string methodName, params Variant[] args)
		{
			Variant variant = this.Call(methodName, args);
			return new Variant?((variant.VariantType == null) ? default(Variant) : variant);
		}

		// Token: 0x06008A8F RID: 35471 RVA: 0x002E4DE4 File Offset: 0x002E2FE4
		private void ValidateBoundObject()
		{
			if (this.BoundObject == null)
			{
				return;
			}
			if (this.BoundObject.GetClass() != this.SpineClassName)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Expected ");
				defaultInterpolatedStringHandler.AppendFormatted("BoundObject");
				defaultInterpolatedStringHandler.AppendLiteral(" to be a ");
				defaultInterpolatedStringHandler.AppendFormatted(this.SpineClassName);
				defaultInterpolatedStringHandler.AppendLiteral(", but it is a ");
				defaultInterpolatedStringHandler.AppendFormatted(this.BoundObject.GetClass());
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			foreach (string text in this.SpineMethods)
			{
				if (!this.BoundObject.HasMethod(text))
				{
					throw new InvalidOperationException(this.SpineClassName + " does not have method " + text + "!");
				}
			}
			foreach (string text2 in this.SpineSignals)
			{
				if (!this.BoundObject.HasSignal(text2))
				{
					throw new InvalidOperationException(this.SpineClassName + " does not have signal " + text2 + "!");
				}
			}
		}
	}
}
