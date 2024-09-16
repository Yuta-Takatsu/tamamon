using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public partial class TopDownCharacterState
{
	public class TopDownCharacterState_WaitKey : TopDownCharacterStateBase
	{
		public KeyCode m_pushKey = KeyCode.None;

		public override void OnInitialize(StateBredgeInfo beforeStateInfo,TopDownCharacterController controller)
		{
			base.OnInitialize(beforeStateInfo, controller);
		}

		public override async UniTask<StateBredgeInfo> OnExecute()
		{
			m_pushKey = KeyCode.None;
			m_nextStateInfo.nextState = State.IDOL;

			Debug.Log("Start WaitKey!");
			await OnExecuteInner();

			var info = GetFinishData();
			return info;
		}

		public override async UniTask<State> OnExecuteInner()
		{
			while ( m_pushKey == KeyCode.None ) {
				Debug.Log("Waiting!");

				if( Input.GetKey(KeyCode.UpArrow)) {
					m_pushKey = KeyCode.UpArrow;
				}
				if( Input.GetKey(KeyCode.RightArrow)) {
					m_pushKey = KeyCode.RightArrow;
				}
				if( Input.GetKey(KeyCode.DownArrow)) {
					m_pushKey = KeyCode.DownArrow;
				}
				if( Input.GetKey(KeyCode.LeftArrow)) {
					m_pushKey = KeyCode.LeftArrow;
				}

				await UniTask.Yield();
			}

			m_nextStateInfo.nextState = State.MOVE_KEY;
			m_nextStateInfo.keycode = m_pushKey;
			return m_nextStateInfo.nextState;
		}

		public override StateBredgeInfo GetFinishData()
		{
			Debug.Log("End Waiting!");

			return m_nextStateInfo;
		}
	}

}
