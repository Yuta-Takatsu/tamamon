using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public partial class TopDownCharacterState
{
	public class TopDownCharacterState_Idol : TopDownCharacterStateBase
	{
		public override void OnInitialize(StateBredgeInfo beforeStateInfo,TopDownCharacterController controller)
		{
			base.OnInitialize(beforeStateInfo, controller);
		}

		public override async UniTask<StateBredgeInfo> OnExecute()
		{
			m_nextStateInfo.nextState = State.IDOL;

			Debug.Log("Start Idol!");
			await OnExecuteInner();

			var info = GetFinishData();
			return info;
		}

		public override async UniTask<State> OnExecuteInner()
		{
			await UniTask.Yield();

			m_nextStateInfo.nextState = State.IDOL;
			return m_nextStateInfo.nextState;
		}

		public override StateBredgeInfo GetFinishData()
		{
			Debug.Log("End Idol!");
			return m_nextStateInfo;
		}
	}

}
