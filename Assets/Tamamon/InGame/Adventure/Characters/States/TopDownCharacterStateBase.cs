using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public partial class TopDownCharacterState
{
	public class TopDownCharacterStateBase : IState
	{
		public StateBredgeInfo m_nextStateInfo;
		public TopDownCharacterController m_parentController;

		public virtual void OnInitialize(StateBredgeInfo beforeStateInfo,TopDownCharacterController controller)
		{
			m_nextStateInfo = beforeStateInfo;
			m_parentController = controller;
		}

		public virtual async UniTask<StateBredgeInfo> OnExecute()
		{
			await UniTask.WaitUntil(() => true);
			return new StateBredgeInfo();
		}

		public virtual async UniTask<State> OnExecuteInner()
		{
			await UniTask.WaitUntil(() => true);
			return m_nextStateInfo.nextState;
		}

		public virtual StateBredgeInfo GetFinishData()
		{
			Debug.Log("End Waiting!");

			var nextInfo = new StateBredgeInfo(){
				nextState = m_nextStateInfo.nextState,
			};

			return nextInfo;
		}
	}

}
