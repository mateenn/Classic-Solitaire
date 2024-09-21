using System;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class EventManager : MonoBehaviour
    {
        public static event Action onLineComplete;
        public static event Action onCheckPathProgress;
        public static event Action<bool> onPathCompleted;
        public static event Action onLevelFail;
        public static event Action onDisplayHint;
        public static event Action<bool> onBlockUserInput;
        public static event Action<bool> onUiOpen;
        public static event Action<bool> onInputHeldDown;
        public static event Action<bool> onGameModeChange;
        public static event Action onSkipLevel;
        public static event Action<bool> onActivateTutorial;
        public static event Action<bool,bool,float,int> OnTest;

        public static void InvokeLineComplete()
        {
            onLineComplete?.Invoke();
        }

        public static void InvokeCheckPathProgress()
        {
            onCheckPathProgress?.Invoke();
        }
        public static void InvokePathCompleted(bool isPathCompleted)
        {
            onPathCompleted?.Invoke(isPathCompleted);
        }
        public static void InvokeLevelFail()
        {
            onLevelFail?.Invoke();
        }
        public static void InvokeDisplayHint()
        {
            onDisplayHint?.Invoke();
        }
        public static void InvokeBlockUserInput(bool isToBlock)
        {
            onBlockUserInput?.Invoke(isToBlock);
        }
        public static void InvokeUiOpen(bool isOpened)
        {
            onUiOpen?.Invoke(isOpened);
        }
        public static void InvokeInputHeldDown(bool isHeldDown)
        {
            onInputHeldDown?.Invoke(isHeldDown);
        }
        public static void InvokeGameModeChange(bool isDarkMode)
        {
            onGameModeChange?.Invoke(isDarkMode);
        }
        public static void InvokeTest(bool isCashedMode, bool isLerpDisabled,float failValue, int passPercentage)
        {
            OnTest?.Invoke(isCashedMode,isLerpDisabled,failValue,passPercentage);
        }
        public static void InvokeSkipLevel()
        {
            onSkipLevel?.Invoke();
        }
        public static void InvokeActivateTutorial(bool isToActivate)
        {
            onActivateTutorial?.Invoke(isToActivate);
        }
    }
}
