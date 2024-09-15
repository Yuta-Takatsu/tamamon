using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;


public partial class TopDownCharacterState
{

	public class TopDownCharacterState_Idol : IState
	{
		public State m_nextState = State.IDOL;
		public KeyCode m_pushKey = KeyCode.None;

		public async void OnInitialize(State beforeState)
		{
			await UniTask.WaitUntil(() => true);
		}

		public async void OnEntry()
		{
			m_pushKey = KeyCode.None;
			m_nextState = State.IDOL;

			Debug.Log("Start Idol!");
			await OnExecute();

			OnFinish();
		}

		public async UniTask<State> OnExecute()
		{
			while ( m_pushKey == KeyCode.None ) {
				Debug.Log("Idoling!");

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

			m_nextState = State.MOVE;
			return m_nextState;
		}

		public NextStatePassInfo OnFinish()
		{
			Debug.Log("End Idol!");

			var nextInfo = new NextStatePassInfo(){
				nextState = m_nextState,
				keycode = m_pushKey
			};

			return nextInfo;
		}
	}

}
