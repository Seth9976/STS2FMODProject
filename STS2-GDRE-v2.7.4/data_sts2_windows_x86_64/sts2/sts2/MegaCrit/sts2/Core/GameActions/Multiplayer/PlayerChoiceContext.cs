using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	// Token: 0x02000C3D RID: 3133
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class PlayerChoiceContext
	{
		// Token: 0x17002354 RID: 9044
		// (get) Token: 0x0600814D RID: 33101 RVA: 0x002BDCCD File Offset: 0x002BBECD
		[Nullable(new byte[] { 2, 1 })]
		public IEnumerable<AbstractModel> ModelStack
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._modelStack;
			}
		}

		// Token: 0x17002355 RID: 9045
		// (get) Token: 0x0600814E RID: 33102 RVA: 0x002BDCD8 File Offset: 0x002BBED8
		[Nullable(2)]
		public AbstractModel LastInvolvedModel
		{
			[NullableContext(2)]
			get
			{
				Stack<AbstractModel> modelStack = this._modelStack;
				AbstractModel abstractModel;
				if (modelStack == null || !modelStack.TryPeek(out abstractModel))
				{
					return null;
				}
				return abstractModel;
			}
		}

		// Token: 0x17002356 RID: 9046
		// (get) Token: 0x0600814F RID: 33103
		public abstract ulong? OwnerId { get; }

		/// <summary>
		/// Add a new model to the top of the context stack.
		/// For example, while <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Prepared" /> is executing, it will be at the top of the context stack.
		/// If it discards a card with <see cref="F:MegaCrit.Sts2.Core.Entities.Cards.CardKeyword.Sly" />, then the Sly'd card should be pushed to the top of
		/// the context stack.
		/// </summary>
		// Token: 0x06008150 RID: 33104 RVA: 0x002BDCFE File Offset: 0x002BBEFE
		public void PushModel(AbstractModel model)
		{
			if (this._modelStack == null)
			{
				this._modelStack = new Stack<AbstractModel>();
			}
			this._modelStack.Push(model);
		}

		// Token: 0x06008151 RID: 33105 RVA: 0x002BDD20 File Offset: 0x002BBF20
		public void PopModel(AbstractModel model)
		{
			AbstractModel abstractModel = null;
			if (this._modelStack == null || !this._modelStack.TryPeek(out abstractModel) || abstractModel != model)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(113, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to pop model ");
				defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>(model);
				defaultInterpolatedStringHandler.AppendLiteral(" from stack of player choice context ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceContext>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" but ");
				defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>(abstractModel);
				defaultInterpolatedStringHandler.AppendLiteral(" was on the top of the stack instead! (Stack size: ");
				Stack<AbstractModel> modelStack = this._modelStack;
				defaultInterpolatedStringHandler.AppendFormatted<int?>((modelStack != null) ? new int?(modelStack.Count) : null);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			this._modelStack.Pop();
		}

		// Token: 0x06008152 RID: 33106
		public abstract Task SignalPlayerChoiceBegun(Player chooser, PlayerChoiceOptions options);

		// Token: 0x06008153 RID: 33107
		public abstract Task SignalPlayerChoiceEnded();

		/// <summary>
		/// A stack of models that are involved with this choice context.
		/// A model can invoke other models to do some work, and those models can invoke a player choice. For example, a
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Survivor" /> may autoplay a Sly'd <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Prepared" />. In these cases, when we display the context
		/// to remote players, we want to show the <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Prepared" /> as the model that is involved in the choice, not
		/// the <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Survivor" />.
		/// </summary>
		// Token: 0x04002B52 RID: 11090
		[Nullable(new byte[] { 2, 1 })]
		private Stack<AbstractModel> _modelStack;
	}
}
