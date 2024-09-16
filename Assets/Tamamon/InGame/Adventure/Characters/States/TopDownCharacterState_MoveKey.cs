using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public partial class TopDownCharacterState
{
	public class TopDownCharacterState_MoveKey : TopDownCharacterStateBase
	{
		private TopDownCharacterController.Direction m_direction;

		public override void OnInitialize(StateBredgeInfo beforeStateInfo,TopDownCharacterController controller)
		{
			base.OnInitialize(beforeStateInfo, controller);

			var key = beforeStateInfo.keycode;
			switch ( key ) {
				case KeyCode.UpArrow:
					m_direction = TopDownCharacterController.Direction.UP;
					break;
				case KeyCode.RightArrow:
					m_direction = TopDownCharacterController.Direction.RIGHT;
					break;
				case KeyCode.DownArrow:
					m_direction = TopDownCharacterController.Direction.DOWN;
					break;
				case KeyCode.LeftArrow:
					m_direction = TopDownCharacterController.Direction.LEFT;
					break;
			}
			m_nextStateInfo.keycode = KeyCode.None;
		}

		public override async UniTask<StateBredgeInfo> OnExecute()
		{
			m_nextStateInfo.nextState = State.IDOL;

			var key = m_nextStateInfo.keycode;
			switch ( key ) {
				case KeyCode.UpArrow:
					m_direction = TopDownCharacterController.Direction.UP;
					break;
				case KeyCode.RightArrow:
					m_direction = TopDownCharacterController.Direction.RIGHT;
					break;
				case KeyCode.DownArrow:
					m_direction = TopDownCharacterController.Direction.DOWN;
					break;
				case KeyCode.LeftArrow:
					m_direction = TopDownCharacterController.Direction.LEFT;
					break;
			}

			Debug.Log("Start Move!");
			await OnExecuteInner();

			var info = GetFinishData();
			return info;
		}

		public override async UniTask<State> OnExecuteInner()
		{
			var isMoveSuccess =  await m_parentController.MoveCell(m_direction);
			await UniTask.Yield();

			m_nextStateInfo.nextState = State.WAIT_KEY;
			return m_nextStateInfo.nextState;
		}

		public override StateBredgeInfo GetFinishData()
		{
			Debug.Log("End Idol!");

			return m_nextStateInfo;
		}
	}
}
