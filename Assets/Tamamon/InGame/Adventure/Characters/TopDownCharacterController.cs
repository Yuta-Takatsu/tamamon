using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// キャラクターコントローラー(見下ろし視点)
/// </summary>
public class TopDownCharacterController
{
    protected TopDownCharacterBase m_characterClass;
    protected TopDownCharacterState m_state;

    public enum Direction
	{
        NEUTORAL = 0,
        UP,
        RIGHT,
        DOWN,
        LEFT
	}

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void OnInitialize(TopDownCharacterBase controllCharacter)
    {
        m_characterClass = controllCharacter;
        m_state = new TopDownCharacterState();

        m_state.OnInitialize(TopDownCharacterState.State.IDOL, this);

        var info = new TopDownCharacterState.StateBredgeInfo(){
            nextState = TopDownCharacterState.State.IDOL
		};
        m_state.RequestSwitchState(info);
    }

    /// <summary>
    /// １歩あるく
    /// </summary>
    public async UniTask<bool> MoveCell(Direction direc)
    {
        if( !m_characterClass ) return false;

        var charaObj = m_characterClass.gameObject;
        var currentPos = charaObj.transform.position;

        charaObj.transform.position = new Vector3(Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.y), currentPos.z);
        var totalMoveVal = 0f;

        if( direc == Direction.UP) {
            var curPosY = currentPos.y;
            var targetPosY = curPosY + 1;

            while( totalMoveVal < 1 ) {
                totalMoveVal += 0.02f;
                charaObj.transform.Translate(0, 0.02f, 0);

                await UniTask.Yield();
			}
		}
         if( direc == Direction.RIGHT) {
            var curPosX = currentPos.x;
            var targetPosX = curPosX + 1;

            while( totalMoveVal < 1 ) {
                totalMoveVal += 0.02f;
                charaObj.transform.Translate(0.02f, 0, 0);

                await UniTask.Yield();
			}
		}
        if( direc == Direction.DOWN) {
            var curPosY = currentPos.y;
            var targetPosY = curPosY - 1;

            while( totalMoveVal > -1 ) {
                totalMoveVal -= 0.02f;
                charaObj.transform.Translate(0, -0.02f, 0);

                await UniTask.Yield();
			}
		}
         if( direc == Direction.LEFT) {
            var curPosX = currentPos.x;
            var targetPosX = curPosX - 1;

            while( totalMoveVal > -1 ) {
                totalMoveVal -= 0.02f;
                charaObj.transform.Translate(-0.02f, 0, 0);

                await UniTask.Yield();
			}
		}

        currentPos = charaObj.transform.position;
        charaObj.transform.position = new Vector3(Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.y), currentPos.z);

        return true;
    }
}
