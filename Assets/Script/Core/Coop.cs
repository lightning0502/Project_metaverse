using System.Collections.Generic;
using UnityEngine;

// Coroutine Optimize = Coop
internal static class Coop
{
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x == y;
        }

        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    // public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    // public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    // public static readonly WaitUntil WaitUntil = new WaitUntil(new BoolComparer());

    private static readonly Dictionary<float, WaitForSeconds> TimeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());
    private static WaitForSeconds tempWaitForSeconds;
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        tempWaitForSeconds = null;

        if (TimeInterval.TryGetValue(seconds, out tempWaitForSeconds) == false)
            TimeInterval.Add(seconds, tempWaitForSeconds = new WaitForSeconds(seconds));

        return tempWaitForSeconds;
    }

    /*
        public class AdvancedCoroutines : MonoBehaviour
        {
            IEnumerator Start()
            {
                var isDone = new IsDoneObject();
                StartCoroutine(MyActions.DelayedAction(1,() => print("HUZZAH!"), timeRemaining => print(timeRemaining), isDone));
                while (isDone == false) yield return null;
                print("Finished");
            }
        }

        public static class MyActions
        {
            public static IEnumerator DelayedAction(float delayTime, Action action, Action<float> timeRemaingUpdate = null, IsDoneObject isDone = null)
            {
                while (delayTime > 0)
                {
                    if (timeRemaingUpdate != null) timeRemaingUpdate(delayTime -= Time.deltaTime);
                    yield return null;
                }

                action();
                if (isDone != null) isDone.SetDone();
            }
        }

        public class IsDoneObject
        {
            private bool isDone = false;

            public void SetDone()
            {
                isDone = true;
            }

            public static implicit operator bool(IsDoneObject x)
            {
                return x.isDone;
            }
        }
        public class tests
        {
            private static tests _instance;
            private tests() { }

            List<IEnumerator> m_routines;

            static tests()
            {
                _instance = new tests();
                _instance.m_routines = new List<IEnumerator>();
            }

            public static IEnumerator StartCoroutine(IEnumerator routine)
            {
                _instance.m_routines.Add(routine);
                return routine;
            }

            public static bool StopCoroutine(IEnumerator routine)
            {
                return _instance.m_routines.Remove(routine);
            }

            public static void StopAllCoroutines()
            {
                _instance.m_routines.Clear();
            }

            public static bool IsRunning(IEnumerator routine)
            {
                return _instance.m_routines.Contains(routine);
            }

            public static int Runnings()
            {
                return _instance.m_routines.Count;
            }

            private static bool Process(IEnumerator routine)
            {
                do{
                    object current = routine.Current;
                    if (current is IEnumerator)
                    {
                        IEnumerator other_routine = current as IEnumerator;
                        if (Process(other_routine) == false) return false;
                        else continue;
                    }
                    else if (current is CustomYieldInstruction)
                    {
                        CustomYieldInstruction yieldInstruction = current as CustomYieldInstruction;
                        if (yieldInstruction.keepWaiting) return false;
                    }
                }
                while (routine.MoveNext());

                if (routine.MoveNext() == false) _instance.m_routines.Remove(routine);
                return true;
            }

            public static void Update()
            {
                for (int i = 0; i < _instance.m_routines.Count; i++)
                {
                    Process(_instance.m_routines[i]);
                }
            }
        }
    */
}