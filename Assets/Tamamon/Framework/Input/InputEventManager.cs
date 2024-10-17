using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 入力によるイベントを管理する
    /// </summary>
    public class InputEventManager : MonoBehaviourSingleton<InputEventManager>
    {
        private Dictionary<InputManager.Key, EventHandler> onKeyEvents = new Dictionary<InputManager.Key, EventHandler>();
        private Dictionary<InputManager.Key, EventHandler> onKeyDownEvents = new Dictionary<InputManager.Key, EventHandler>();
        private Dictionary<InputManager.Key, EventHandler> onKeyUpEvents = new Dictionary<InputManager.Key, EventHandler>();
        private Dictionary<InputManager.Key, EventHandler> onKeyNotPressedEvents = new Dictionary<InputManager.Key, EventHandler>();
        private Dictionary<InputManager.Axes, EventHandler> onAxesEvents = new Dictionary<InputManager.Axes, EventHandler>();
        private Dictionary<InputManager.Axes, EventHandler> onAxesRowEvents = new Dictionary<InputManager.Axes, EventHandler>();

        // 入力できるかどうか
        public bool IsInput { get; set; }

        public override void Awake()
        {
            base.Awake();

            IsInput = true;

            //キーの種類の数だけイベントを生成する
            foreach (InputManager.Key key in InputManager.Key.AllKeyData)
            {
                onKeyEvents.Add(key, (o, a) => { });
                onKeyDownEvents.Add(key, (o, a) => { });
                onKeyUpEvents.Add(key, (o, a) => { });
                onKeyNotPressedEvents.Add(key, (o, a) => { });
            }

            foreach (InputManager.Axes axes in InputManager.Axes.AllAxesData)
            {
                onAxesEvents.Add(axes, (o, a) => { });
                onAxesRowEvents.Add(axes, (o, a) => { });
            }
        }

        public void Update()
        {
            if (!IsInput)
                return;

            // HACK:EventArgsを継承して様々なデータを受け渡せるように出来る

            KeyEventInvoke(InputManager.Instance.GetKey, onKeyEvents, new EventArgs());
            KeyEventInvoke(InputManager.Instance.GetKeyDown, onKeyDownEvents, new EventArgs());
            KeyEventInvoke(InputManager.Instance.GetKeyUp, onKeyUpEvents, new EventArgs());
            KeyEventInvoke((key) => { return !InputManager.Instance.GetKey(key); }, onKeyNotPressedEvents, new EventArgs());

            AxesEventInvoke(onAxesEvents, new EventArgs());
            AxesEventInvoke(onAxesRowEvents, new EventArgs());
        }

        /// <summary>
        /// キー入力イベントをセットする
        /// </summary>
        /// <param name="key">キーの種類</param>
        /// <param name="eventHandler">実行するイベント</param>
        public void SetKeyEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyEvents[key] += eventHandler;
        }

        /// <summary>
        /// キー入力開始時イベントをセットする
        /// </summary>
        /// <param name="key">キーの種類</param>
        /// <param name="eventHandler">実行するイベント</param>
        public void SetKeyDownEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyDownEvents[key] += eventHandler;
        }

        /// <summary>
        /// キー入力終了時イベントをセットする
        /// </summary>
        /// <param name="key">キーの種類</param>
        /// <param name="eventHandler">実行するイベント</param>
        public void SetKeyUpEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyUpEvents[key] += eventHandler;
        }

        /// <summary>
        /// キーが押されていない場合のイベントをセットする
        /// </summary>
        /// <param name="key">キーの種類</param>
        /// <param name="eventHandler">実行するイベント</param>
        public void SetKeyNotPressedEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyNotPressedEvents[key] += eventHandler;
        }

        /// <summary>
        /// 軸入力時イベントをセットする
        /// </summary>
        /// <param name="axes">軸の種類</param>
        /// <param name="eventHandler">実行するイベント</param>
        public void SetAxesEvent(InputManager.Axes axes, EventHandler eventHandler)
        {
            onAxesEvents[axes] += eventHandler;
        }

        /// <summary>
        /// 軸入力時イベントをセットする
        /// </summary>
        /// <param name="axes">軸の種類</param>
        /// <param name="eventHandler">実行するイベント</param>
        public void SetAxesRowEvent(InputManager.Axes axes, EventHandler eventHandler)
        {
            onAxesRowEvents[axes] += eventHandler;
        }

        /// <summary>
        /// キー入力イベントから指定したイベントを削除する
        /// </summary>
        /// <param name="key">キーの種類</param>
        /// <param name="eventHandler">削除するイベント</param>
        public void RemoveKeyEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyEvents[key] -= eventHandler;
        }

        /// <summary>
        /// キー入力時イベントから指定したイベントを削除する
        /// </summary>
        /// <param name="key">キーの種類</param>
        /// <param name="eventHandler">削除するイベント</param>
        public void RemoveKeyDownEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyDownEvents[key] -= eventHandler;
        }

        /// <summary>
        /// キー入力終了時イベントから指定したイベントを削除する
        /// </summary>
        /// <param name="key">キーの種類</param>
        /// <param name="eventHandler">削除するイベント</param>
        public void RemoveKeyUpEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyUpEvents[key] -= eventHandler;
        }

        /// <summary>
        /// キーが入力されていない場合のイベントから指定したイベントを削除する
        /// </summary>
        /// <param name="key">キーの種類</param>
        /// <param name="eventHandler">削除するイベント</param>
        public void RemoveKeyNotPressedEvent(InputManager.Key key, EventHandler eventHandler)
        {
            onKeyNotPressedEvents[key] -= eventHandler;
        }

        /// <summary>
        /// 軸入力時イベントを削除する
        /// </summary>
        /// <param name="axes">軸の種類</param>
        /// <param name="eventHandler">削除するイベント</param>
        public void RemoveAxesEvent(InputManager.Axes axes, EventHandler eventHandler)
        {
            onAxesEvents[axes] -= eventHandler;
        }

        /// <summary>
        /// 軸入力時イベントを削除する
        /// </summary>
        /// <param name="axes">軸の種類</param>
        /// <param name="eventHandler">削除するイベント</param>
        public void RemoveAxesRowEvent(InputManager.Axes axes, EventHandler eventHandler)
        {
            onAxesRowEvents[axes] -= eventHandler;
        }

        /// <summary>
        /// 登録されたイベントを全て削除する
        /// </summary>
        public void ClearEvent()
        {
            onKeyEvents.Clear();
            onKeyDownEvents.Clear();
            onKeyUpEvents.Clear();
            onKeyNotPressedEvents.Clear();
            onAxesEvents.Clear();
            onAxesRowEvents.Clear();
        }

        /// <summary>
        /// キーイベントを実行する
        /// キーが入力されていない状態の時はイベントを実行しない
        /// </summary>
        /// <param name="keyEntryDecision">キー入力判定を行う述語</param>
        /// <param name="keyEvent">キーごとのイベントを格納するハッシュマップ</param>
        /// <param name="args">イベント実行に用いる引数</param>
        private void KeyEventInvoke(Func<InputManager.Key, bool> keyEntryDecision, Dictionary<InputManager.Key, EventHandler> keyEvent, EventArgs args)
        {
            foreach (InputManager.Key key in InputManager.Key.AllKeyData)
                if (keyEntryDecision(key))
                    if (keyEvent[key] != null)
                        keyEvent[key](this, args);
        }

        /// <summary>
        /// 軸イベントを実行する
        /// </summary>
        /// <param name="axesEntryDecision">軸入力値取得を行う述語</param>
        /// <param name="axesEvent">軸ごとのイベントを格納するハッシュマップ</param>
        /// <param name="args">イベント実行に用いる引数</param>
        private void AxesEventInvoke(Dictionary<InputManager.Axes, EventHandler> axesEvent, EventArgs args)
        {
            foreach (InputManager.Axes axes in InputManager.Axes.AllAxesData)
                if (axesEvent[axes] != null)
                    axesEvent[axes](this, args);
        }
    }
}