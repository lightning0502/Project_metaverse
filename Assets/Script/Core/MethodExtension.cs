using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

// ? 연산자 (삼항)
// int a = a != 0 ? 100 : 50;

// ?? 연산자
// gameobject obj a ?? b;     => information : a == null 이면, b를 할당. a != null 이면 a를 할당.

class ExtensionCoroutine : MonoBehaviour
{
    private static ExtensionCoroutine instance;

    public static ExtensionCoroutine Instance
    {
        get
        {
            if (object.ReferenceEquals(instance, null))
                instance = new GameObject("Extension Coroutine").AddComponent<ExtensionCoroutine>();

            return instance;
        }
    }
}

namespace Extension
{
    /*
        public enum SkillName : int
        {
            // basic
            Slash_Attack,
            Rapid_Stab,
            Magic_Missile,
            Burning_Touch,

            // expert
            Chaotic_Slain,
            Death_Blow,
            Frost_Slash,
            Hoarfrost_Sphere_Storm,
            Hilt_Charging,
            Block_Casting,
            Pillar_of_Fireblast,
            Firebloom_of_the_Cursed,
            Bandaging_Oneself,
            Summon_Solar_Flare,
        }

        public static class EnumExtension
        {
            public static short GetValue(this SkillName enumValue)
            {
                return (short)enumValue;
            }

            public static int Code(this SkillIndex enumValue)
            {
                return (int)enumValue;
            }

            public static int Index(this SkillIndex enumValue)
            {
                return (int)enumValue - 1;
            }

            public static bool IsLeft(this SkillIndex indexValue)
            {
                return indexValue == SkillIndex.Left;
            }

            public static bool IsRight(this SkillIndex indexValue)
            {
                return indexValue == SkillIndex.Right;
            }

            ex)
                SkillIndex index = SkillIndex.Left;
                index.IsLeft();
        }


        or


        public class Question
        {
            // Attributes
            protected int index;
            protected string name;
            // Go with a dictionary to enforce unique index
            //protected static readonly ICollection<Question> values = new Collection<Question>();
            protected static readonly IDictionary<int,Question> values = new Dictionary<int,Question>();

            // Define the "enum" values
            public static readonly Question Role = new Question(2,"Role");
            public static readonly Question ProjectFunding = new Question(3, "Project Funding");
            public static readonly Question TotalEmployee = new Question(4, "Total Employee");
            public static readonly Question NumberOfServers = new Question(5, "Number of Servers");
            public static readonly Question TopBusinessConcern = new Question(6, "Top Business Concern");

            // Constructors
            protected Question(int index, string name)
            {
                this.index = index;
                this.name = name;
                values.Add(index, this);
            }

            // Easy int conversion
            public static implicit operator int(Question question) =>
                question.index; //nb: if question is null this will return a null pointer exception

            public static implicit operator Question(int index) =>
                values.TryGetValue(index, out var question) ? question : null;

            // Easy string conversion (also update ToString for the same effect)
            public override string ToString() =>
                this.name;

            public static implicit operator string(Question question) =>
                question?.ToString();

            public static implicit operator Question(string name) =>
                name == null ? null : values.Values.FirstOrDefault(item => name.Equals(item.name, StringComparison.CurrentCultureIgnoreCase));


            // If you specifically want a Get(int x) function (though not required given the implicit converstion)
            public Question Get(int foo) =>
                foo; //(implicit conversion will take care of the conversion for you)
        }
    */


    public static class MethodExtension
    {
        /*
        // Coroutine Ex
        public static void SomeFunction(this GameObject gameObject)
        {
            ExtensionCoroutine.Instance.StartCoroutine(SomeCoroutine(gameObject)); // StartCoroutine을 어떻게 없애야 할까..
            // 1. Update에서 call을 받아온다.
            // 2. Enumerator 구조를 구현하고 돌릴 코루틴을 리스트에 추가한다.
            // 3. 리스트안에 코루틴이 있으면 1번의 call을 해당 코루틴에 연결 -> 실행시킨다.
            // 4. 해당 코루틴이 종료되면 리스트에서 삭제하고, yield return false등의 예외처리로 빠져나옴 or 4-1
            // 4-1 대기 루틴 : yield return에 대한 WaitForSeconds를 구현해야함. 해당 코루틴의 WaitForSeconds만큼 다른 리스트의 코루틴 호출
            // 5. 리스트가 채워질때까지 대기 (?)
            // yield break; // == function return, coroutine return;
        }
        private static IEnumerator SomeCoroutine(GameObject gameObject)
        {
            int timeCount = 0;

            while (timeCount < 5)
            {
                ++timeCount;
                Debug.Log("Some Coroutine is Work!");
                // if (skillButton.interactable == false) yield break; // == function return;
                yield return Coop.WaitForSeconds(1);
            }
        }
        */

        /// <param name="floatValue"> return : 0.000 </param>
        public static float Cutting(this float floatValue)
        {
            // Debug.LogError("floatValue : " + floatValue);
            // float.Parse(((1 / RecastTime) * RecastTimeImageFPS).ToString("N3")); // "N3" == 0.000; // Performence defeat
            // (float)(System.Math.Truncate((1 / RecastTime) * RecastTimeImageFPS * 1000) * 0.001); // Performence Win
            return (float)(System.Math.Truncate(floatValue * 1000) * 0.001);
        }

        public static string ToString_N1(this float returnValue)
        {
            // ifvalue == int to return int, and value == float to "0.0"
            // Debug.LogError("value : " + returnValue + "  / System.Math.Truncate(returnValue) : " + System.Math.Truncate(returnValue));
            // return returnValue == System.Math.Truncate(returnValue) ? returnValue.ToString() : returnValue.ToString("0.0");
            return (System.Math.Truncate(returnValue * 10) * 0.1).ToString();
        }

        public static string ToString_N2(this float returnValue)
        {
            // ifvalue == int to return int, and value == float to "0.0"
            // Debug.LogError("value : " + returnValue + "  / System.Math.Truncate(returnValue) : " + System.Math.Truncate(returnValue));
            // return returnValue == System.Math.Truncate(returnValue) ? returnValue.ToString() : returnValue.ToString("0.00");
            return (System.Math.Truncate(returnValue * 100) * 0.01).ToString();
        }

        public static float ToFloat_N3(this float returnValue)
        {
            // ifvalue == int to return int, and value == float to "0.0"
            // Debug.LogError("value : " + returnValue + "  / System.Math.Truncate(returnValue) : " + System.Math.Truncate(returnValue));
            // return returnValue == System.Math.Truncate(returnValue) ? returnValue.ToString() : returnValue.ToString("0.00");
            return (float)(System.Math.Truncate(returnValue * 1000) * 0.001f);
        }

        public static void SetSizeDelta(this RectTransform rectTransform, float width, float height)
        {
            rectTransform.sizeDelta = SetVector3(width, height);
        }

        public static void SetEffectScale(this Transform transform, int scaleValue)
        {
            transform.localScale = SetVector3(scaleValue, scaleValue, 1);
        }

        // localPosition == parent, position = gameobject;
        /// <param name="x"> float x </param>
        /// <param name="y"> float y </param>
        /// <param name="z"> float z </param>
        public static void SetPosition(this Transform transform, float x, float y, float z)
        {
            transform.localPosition = SetVector3(x, y, z);
        }
        public static void SetRectPosition(this RectTransform rectTransform, float x, float y, float z)
        {
            rectTransform.localPosition = SetVector3(x, y, z);
        }
        public static void SetPosition(this RawImage rawImage, float x, float y, float z)
        {
            rawImage.rectTransform.localPosition = SetVector3(x, y, z);
        }

        /// <param name="x"> float x </param>
        /// <param name="y"> float y </param>
        public static Vector2 SetVector3(float x, float y)
        {
            // Vector2 vector = Vector2.zero; // Vector2.zero == return new Vector2(0, 0);
            Vector2 vector;
            vector.x = x;
            vector.y = y;

            return vector;
        }
        /// <param name="x"> float x </param>
        /// <param name="y"> float y </param>
        /// <param name="z"> float z </param>
        public static Vector3 SetVector3(float x, float y, float z)
        {
            // Vector3 vector = Vector3.zero; // Vector3.zero == return new Vector3(0, 0, 0);
            Vector3 vector; // Vector3 is struct, not occur GC
            vector.x = x;
            vector.y = y;
            vector.z = z;

            return vector;
        }

        /*
        /// <param name="alphaValue"> value 0 : alpha initialize, alphaValue > 0 : start alpha animation </param>
        public static void SetAlphaImageList(this List<Image> imageList, float alphaValue)
        {
            int i, count = imageList.Count;

            if (alphaValue == 0) // initialize
            {
                for (i = 0; i < count; ++i)
                {
                    imageList[i].SetAlpha(0);
                    imageList[i].raycastTarget = false;
                }
            }

            else // start animation
            {
                for (i = 0; i < count; ++i)
                {
                    imageList[i].AnimationAlpha(alphaValue);
                    imageList[i].raycastTarget = true;
                }
            }
        }

        /// <param name="alphaValue"> value 0 : alpha initialize, alphaValue > 0 : start alpha animation </param>
        public static void SetAlphaTextList(this List<Image> imageList, float alphaValue)
        {
            int i, count = imageList.Count;

            if (alphaValue == 0) // initialize
            {
                for (i = 0; i < count; ++i)
                {
                    imageList[i].SetAlpha(0);
                    imageList[i].raycastTarget = false;
                }
            }

            else // start animation
            {
                for (i = 0; i < count; ++i)
                {
                    imageList[i].AnimationAlpha(alphaValue);
                    imageList[i].raycastTarget = true;
                }
            }
        }
        */

        /// <param name="a"> float : 0 ~ 1 </param>
        public static void SetAlpha(this Text textObject, float a)
        {
            textObject.color = SetColor(textObject.color.r, textObject.color.g, textObject.color.b, a);
        }
        /// <param name="a"> float : 0 ~ 1 </param>
        public static void SetAlpha(this Image imageObject, float a)
        {
            imageObject.color = SetColor(imageObject.color.r, imageObject.color.g, imageObject.color.b, a);
        }
        public static void SetAlpha(this RawImage imageObject, float a)
        {
            imageObject.color = SetColor(imageObject.color.r, imageObject.color.g, imageObject.color.b, a);
        }

        /// <param name="r"> float red </param>
        /// <param name="g"> float green </param>
        /// <param name="b"> float blue </param>
        /// <param name="a"> float alpha </param>
        public static Color SetColor(float r, float g, float b, float a)
        {
            Color color; // Color is struct, not occur GC
            color.r = r;
            color.g = g;
            color.b = b;
            color.a = a;

            return color;
        }

        public static void SetActiveForChild(this Transform objectTransform, bool onDisplay)
        {
            int i, length = objectTransform.childCount;

            for (i = 0; i < length; ++i)
                objectTransform.GetChild(i).gameObject.SetActive(onDisplay);
        }

        public static string ClearJoint(this StringBuilder builder, params object[] objectArray)
        {
            int i, count = objectArray.Length;
            builder.Length = 0;
            // builder.Capacity = 0;

            for (i = 0; i < count; ++i)
                builder.Append(objectArray[i]);

            return builder.ToString();
        }

        private static StringBuilder PrivateBuilder = new StringBuilder(256);
        public static string ToStringBuilder(params object[] objectArray)
        {
            int i, count = objectArray.Length;
            PrivateBuilder.Length = 0;
            // builder.Capacity = 0;

            for (i = 0; i < count; ++i)
                PrivateBuilder.Append(objectArray[i]);

            return PrivateBuilder.ToString();
        }
        public static string ToStringBuilder_Double(params object[] objectArray)
        {
            int i, count = objectArray.Length;
            PrivateBuilder.Length = 0;
            // builder.Capacity = 0;

            for (i = 0; i < count; ++i)
                PrivateBuilder.Append(objectArray[i]);

            return PrivateBuilder.Append(System.Environment.NewLine).Append(PrivateBuilder).ToString();
        }

        private static readonly string Readonly_NewLine = System.Environment.NewLine;
        public static string ClearJointNewLineDouble(this StringBuilder builder, params object[] objectArray)
        {
            int i, count = objectArray.Length;
            builder.Length = 0;
            // builder.Capacity = 0;

            for (i = 0; i < count; ++i)
                builder.Append(objectArray[i]);

            return builder.Append(Readonly_NewLine).Append(builder).ToString();
        }

        #region Animation
        /// <param name="strongValue"> float : alpha power (ex : -0.1f or 0.1f) </param>
        public static void AnimationTextAlpha(this Text textObject, float strongValue)
        {
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationTextAlpha(textObject, strongValue));
        }
        private static IEnumerator WorkAnimationTextAlpha(Text textObject, float floatValue)
        {
            Color color = SetColor(0, 0, 0, floatValue);
            float checkAlpha = 0;

            if (floatValue > 0)
            {
                checkAlpha = 1;

                while (textObject.color.a < checkAlpha)
                {
                    textObject.color += color;
                    yield return Coop.WaitForSeconds(0.03f);
                }
            }

            else if (floatValue < 0)
            {
                while (textObject.color.a > checkAlpha)
                {
                    textObject.color += color;
                    yield return Coop.WaitForSeconds(0.03f);
                }
            }
        }
        /*
        /// <param name="strongValue"> float : alpha power (ex : -0.1f or 0.1f) </param>
        public static void AnimationAlpha(this Image imageObject, float strongValue)
        {
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationAlpha(imageObject, strongValue));
        }
        private static IEnumerator WorkAnimationAlpha(Image imageObject, float floatValue)
        {
            Color color = SetColor(0, 0, 0, floatValue);
            float checkAlpha = 0;

            if (floatValue > 0)
            {
                checkAlpha = 1;

                while (imageObject.color.a < checkAlpha)
                {
                    imageObject.color += color;
                    yield return Coop.WaitForSeconds(0.03f);
                }
            }

            else if (floatValue < 0)
            {
                while (imageObject.color.a > checkAlpha)
                {
                    imageObject.color += color;
                    yield return Coop.WaitForSeconds(0.03f);
                }
            }
        }

        /*
        /// <param name="startIndex"> set for parent basic index </param>
        public static int GetThisChildIndex(this GameObject childObject, int startIndex)
        {
            Transform parentObject = childObject.transform.parent;
            int i, parentCount = parentObject.childCount;

            for (i = startIndex; i < parentCount; ++i)
            {
                Debug.LogError("child name : " + parentObject.GetChild(i).name);
                if (parentObject.GetChild(i).name.Equals(childObject.name))
                    return i;
            }

            return -1;
        }

        /// <param name="strongValue"> float : alpha power (ex : -0.1f or 0.1f) </param>
        /// <param name="limit"> float : 0 ~ 1 </param>
        public static void AnimationLimitAlpha(this Image imageObject, float strongValue, float limit)
        {
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationLimitAlpha(imageObject, strongValue, limit));
        }
        private static IEnumerator WorkAnimationLimitAlpha(Image imageObject, float floatValue, float limit)
        {
            Color color = SetColor(0, 0, 0, floatValue);

            if (floatValue > 0)
            {
                while (imageObject.color.a < limit)
                {
                    imageObject.color += color;
                    yield return Coop.WaitForSeconds(0.05f);
                }
            }

            else if (floatValue < 0)
            {
                while (imageObject.color.a > limit)
                {
                    imageObject.color += color;
                    yield return Coop.WaitForSeconds(0.05f);
                }
            }
        }

        public static void AnimationOnHeal(this GameObject gameObject)
        {
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationOnHeal(gameObject));
        }
        private static IEnumerator WorkAnimationOnHeal(GameObject gameObject)
        {
            Image objectImage = gameObject.GetComponent<Image>();
            Color healColor = SetColor(0.05f, 0, 0.05f, 0);
            // count 구현이 필요할까? 그리고 지속적인 힐링에 대해서는 어떻게 적용해야할까?? 조금 연하게..?
            while (objectImage.color.b > 0.1f) // red 비교는 hit color와 문제를 일으킬 수 있다.
            {
                objectImage.color -= healColor;
                yield return Coop.WaitForSeconds(0.02f);
            }

            yield return Coop.WaitForSeconds(0.5f);

            while (objectImage.color.b < 1)
            {
                objectImage.color += healColor;
                yield return Coop.WaitForSeconds(0.02f);
            }

            objectImage.color = Set Color(1, 1, 1, 1); // initialize
        }

        /// <param name="duration"> duration == 0 ? gameObject.activeInHierarchy : float </param>
        public static void AnimationRotation(this Image imageObject, float strongValue)
        {
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationRotation(imageObject.transform, imageObject.gameObject, strongValue));
        }
        private static IEnumerator WorkAnimationRotation(Transform imageTransform, GameObject imageObject, float strongValue)
        {
            while (imageObject.activeInHierarchy)
            {
                yield return Coop.WaitForSeconds(0.04f);
                imageTransform.Rotate(0, 0, strongValue);
            }
        }

        /*
        /// <param name="colorIndex"> 0 = [fire][burn][flame], 1 = [poison][venom][toxic], 2 = [cold][chill][frost], 3 = [dark][curse], 4 = [heal] </param>
        public static void AnimationStateColor(this GameObject gameObject, int colorIndex, float duration) // List로 변경해야함.
        {
            Image imageObject = gameObject.GetComponent<Image>();
            IEnumerator stateCoroutine = WorkAnimationStateColor(imageObject, colorIndex);

            ExtensionCoroutine.Instance.StartCoroutine(stateCoroutine);
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationStateTimer(stateCoroutine, imageObject, duration));
        }
        private static IEnumerator WorkAnimationStateColor(Image imageObject, int colorIndex)
        {
            Color setColor;
            float colorChanger = 1f;
            float timer = 0.05f;

            switch (colorIndex)
            {
                default:
                    DebugText.Instance.OnDebugText("Error : WorkAnimationStateColor, index : " + colorIndex);
                    yield break;

                case 0: // red
                    setColor = SetColor(0.005f, 0.025f, 0.025f, 0);
                    break;

                case 1: // dark green (poison)
                    setColor = SetColor(0.03f, 0.025f, 0.03f, 0);
                    break;

                case 2: // blue
                    setColor = SetColor(0.025f, 0.02f, 0, 0);
                    break;

                case 3: // black
                    setColor = SetColor(0.02f, 0.02f, 0.02f, 0);
                    break;

                case 4: // yellow green (healing)
                    setColor = SetColor(0.02f, 0, 0.03f, 0);
                    break;
            }

            while (true) // will be stop WorkAnimationStateTimer()
            {
                while (colorChanger > 0.15f)
                {
                    imageObject.color -= setColor;
                    colorChanger -= 0.02f;
                    yield return Coop.WaitForSeconds(timer);
                }

                while (colorChanger < 0.45f)
                {
                    imageObject.color += setColor;
                    colorChanger += 0.02f;
                    yield return Coop.WaitForSeconds(timer);
                }
            }
        }

        private static IEnumerator WorkAnimationStateTimer(IEnumerator stateCoroutine, Image imageObject, float duration)
        {
            float timer = 0.2f;

            while (duration > 0f)
            {
                duration -= timer + Time.deltaTime;
                yield return Coop.WaitForSeconds(timer);
            }

            ExtensionCoroutine.Instance.StopCoroutine(stateCoroutine);
            stateCoroutine = null;
            imageObject.color = Set Color(1, 1, 1, 1); // color initialize
        }

        /// <param name="blinkDuration"> float : blink time (0.01 ~ 2.0) and call with WorkAnimationHitEarthQuake() </param>
        public static void AnimationHitBlink(this Image imageObject, float blinkDuration, int count)
        {
            // 일단 Animation HitBlink를 EarthQuake 기능과 Red hit animaton 기능과 떨어트려야함..
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationHitBlink(imageObject, blinkDuration, count, 0));
        }
        public static void AnimationHitBlink(this Image imageObject, float blinkDuration, int count, float delay)
        {
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationHitBlink(imageObject, blinkDuration, count, delay));
        }

        public static void AnimationHitBlinkForSkill(this Image imageObject)
        {
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationHitBlink(imageObject, 0.2f, 0, 0));
        }
        private static IEnumerator WorkAnimationHitEarthQuake(Transform trasformObject)
        {
            int i, count = 4;
            int minValue = -30, maxValue = 30;

            for (i = 0; i < count; ++i)
            {
                trasformObject.SetPosition(Random.Range(minValue, maxValue), Random.Range(minValue, maxValue), 0);
                yield return Coop.WaitForSeconds(0.06f);
                maxValue -= 5;
                minValue += 5;
            }

            trasformObject.localPosition = SetVector3(0, 0, 0);
        }

        private static IEnumerator WorkAnimationHitBlink(Image imageObject, float blinkDuration, int count, float delay)
        {
            yield return Coop.WaitForSeconds(delay);

            if (count > 0)
            {
                int i;
                float time = blinkDuration * 0.5f;

                for (i = 0; i < count; ++i)
                {
                    imageObject.color = ReadonlyColor_Red;
                    yield return Coop.WaitForSeconds(time);
                    ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationHitEarthQuake(imageObject.transform));
                    imageObject.color = ReadonlyColor_White;
                    yield return Coop.WaitForSeconds(time);
                }
            }

            else
            {
                imageObject.color = ReadonlyColor_Red;
                ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationHitEarthQuake(imageObject.transform));
                yield return Coop.WaitForSeconds(blinkDuration);
                imageObject.color = ReadonlyColor_White;
            }
        }

        /// <param name="strongValue"> float </param>
        /// <param name="onPositionDivider"> true : move x, false : move y </param>
        public static void AnimationAccelMove(this Transform transformObject, bool onPositionDivider, float strongValue, float duration)
        {
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationAccelMove(transformObject, onPositionDivider, strongValue, duration));
        }
        private static IEnumerator WorkAnimationAccelMove(Transform transformObject, bool onPositionDivider, float strongValue, float duration)
        {
            Vector3 vectorValue;
            vectorValue.x = 0;
            vectorValue.y = 0;
            vectorValue.z = 0;

            float acceleration = 3f;
            bool onReverse = strongValue < 0 ? true : false;

            while (duration > 0)
            {
                vectorValue = onPositionDivider ? SetVector3(acceleration, 0, 0) : SetVector3(0, acceleration, 0);
                transformObject.localPosition += onReverse ? vectorValue : -vectorValue;
                duration -= onReverse ? -strongValue : strongValue;
                acceleration += onReverse ? strongValue : -strongValue;

                yield return Coop.WaitForSeconds(0.02f);
            }
        }

        /// <param name="isMinusMove"> isMinusMove == ture ? minus move : plus move </param>
        /// <param name="onPositionDivider"> positionDivider ==  true ? move x : move y </param>
        /// <param name="moveResist"> 0.1f ~ 2 </param>
        public static void AnimationLimitMoveUI(this Transform transformObject, bool onPositionDivider, bool isMinusMove, float acceleration, float moveResist, float goalPosition)
        {
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationLimitMoveUI(transformObject, onPositionDivider, isMinusMove, acceleration, moveResist, goalPosition));
        }
        private static IEnumerator WorkAnimationLimitMoveUI(Transform transformObject, bool onPositionDivider, bool isMinusMove, float acceleration, float moveResist, float goalPosition)
        {
            Vector3 vectorValue;
            vectorValue.x = 0;
            vectorValue.y = 0;
            vectorValue.z = 0;

            float accel = acceleration;
            float checkValue = 0.3f;

            if (onPositionDivider) // move to x
            {
                float saveYPos = transformObject.localPosition.y;
                vectorValue = MethodExtension.SetVector3(accel, 0, 0);

                if (isMinusMove)
                {
                    while (transformObject.localPosition.x > goalPosition)
                    {
                        accel -= moveResist;

                        if (accel < checkValue)
                            accel = checkValue;

                        vectorValue = MethodExtension.SetVector3(accel, 0, 0);
                        transformObject.localPosition -= vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }
                }

                else
                {
                    while (transformObject.localPosition.x < goalPosition)
                    {
                        accel -= moveResist;

                        if (accel < checkValue)
                            accel = checkValue;

                        vectorValue = MethodExtension.SetVector3(accel, 0, 0);
                        transformObject.localPosition += vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }
                }

                transformObject.SetPosition(goalPosition, saveYPos, 0); // goal position
            }

            else // move to y
            {
                float saveXPos = transformObject.localPosition.x;
                vectorValue = MethodExtension.SetVector3(0, accel, 0);

                if (isMinusMove)
                {
                    while (transformObject.localPosition.y > goalPosition)
                    {
                        accel -= moveResist;

                        if (accel < checkValue)
                            accel = checkValue;

                        vectorValue = MethodExtension.SetVector3(0, accel, 0);
                        transformObject.localPosition -= vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }
                }

                else
                {
                    while (transformObject.localPosition.y < goalPosition)
                    {
                        accel -= moveResist;

                        if (accel < checkValue)
                            accel = checkValue;

                        vectorValue = MethodExtension.SetVector3(0, accel, 0);
                        transformObject.localPosition += vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }
                }

                transformObject.SetPosition(saveXPos, goalPosition, 0); // goal position
            }
        }

        /// <param name="isMinusMove"> isMinusMove == ture ? minus move : plus move </param>
        /// <param name="onPositionDivider"> onPositionDivider ==  true ? move x : move y </param>
        /// <param name="moveResist"> 0.1f ~ 2 </param>
        public static void AnimationAccelMoveLimit(this Transform transformObject, bool onPositionDivider, bool isMinusMove, float moveResist, float goalXPos, float goalYPos)
        {
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationAccelMoveLimit(transformObject, onPositionDivider, isMinusMove, moveResist, goalXPos, goalYPos));
        }
        private static IEnumerator WorkAnimationAccelMoveLimit(Transform transformObject, bool onPositionDivider, bool isMinusMove, float moveResist, float goalXPos, float goalYPos)
        {
            Vector3 vectorValue;
            vectorValue.x = 0;
            vectorValue.y = 0;
            vectorValue.z = 0;

            float acceleration = 20f, minimumValue = 0.2f;

            if (onPositionDivider) // move to x
            {
                vectorValue.x = acceleration;
                float goalNear = goalXPos * 1.15f;

                if (isMinusMove)
                {
                    while (transformObject.localPosition.x > goalNear)
                    {
                        transformObject.localPosition -= vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }

                    while (transformObject.localPosition.x > goalXPos)
                    {
                        acceleration -= moveResist;

                        if (acceleration < minimumValue)
                            acceleration = minimumValue;

                        vectorValue.x = acceleration;
                        transformObject.localPosition -= vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }
                }

                else
                {
                    if (goalNear > 0)
                        goalNear = goalXPos * 0.8f;

                    while (transformObject.localPosition.x < goalNear)
                    {
                        transformObject.localPosition += vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }

                    while (transformObject.localPosition.x < goalXPos)
                    {
                        acceleration -= moveResist;

                        if (acceleration < minimumValue)
                            acceleration = minimumValue;

                        vectorValue.x = acceleration;
                        transformObject.localPosition += vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }
                }
            }

            else // move to y
            {
                vectorValue.y = acceleration;
                float goalNear = goalYPos * 1.15f;

                if (isMinusMove)
                {
                    while (transformObject.localPosition.y > goalNear)
                    {
                        transformObject.localPosition -= vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }

                    while (transformObject.localPosition.y > goalYPos)
                    {
                        acceleration -= moveResist;

                        if (acceleration < minimumValue)
                            acceleration = minimumValue;

                        vectorValue.y = acceleration;
                        transformObject.localPosition -= vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }
                }

                else
                {
                    if (goalNear > 0)
                        goalNear = goalXPos * 0.8f;

                    while (transformObject.localPosition.y < goalNear)
                    {
                        transformObject.localPosition += vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }

                    while (transformObject.localPosition.y < goalYPos)
                    {
                        acceleration -= moveResist;

                        if (acceleration < minimumValue)
                            acceleration = minimumValue;

                        vectorValue.y = acceleration;
                        transformObject.localPosition += vectorValue;
                        yield return Coop.WaitForSeconds(0.02f);
                    }
                }
            }

            transformObject.SetPosition(goalXPos, goalYPos, 0); // goal position
        }

        /// <param name="alpha"> alpha Power : 0 ~ 0.5f </param>
        public static void AnimationAlphaRally(this Image imageObject, float alpha, float delay)
        {
            ExtensionCoroutine.Instance.StartCoroutine(WorkAnimationAlphaRally(imageObject, alpha, delay));
        }
        private static IEnumerator WorkAnimationAlphaRally(Image imageObject, float alpha, float delay)
        {
            Color alphaColor = SetColor(0, 0, 0, alpha);

            while (imageObject.gameObject.activeInHierarchy)
            {
                while (imageObject.color.a < 1)
                {
                    imageObject.color += alphaColor;
                    yield return Coop.WaitForSeconds(0.05f);
                }

                yield return Coop.WaitForSeconds(delay);

                while (imageObject.color.a > 0.01f)
                {
                    imageObject.color -= alphaColor;
                    yield return Coop.WaitForSeconds(0.05f);
                }

                yield return Coop.WaitForSeconds(delay);
            }
        }
    #endregion

    #region Object Clear 를 하면 안되잖아...
        public static void ClearChildObject(this GameObject parentObject)
        {
            int i, childCount = parentObject.transform.childCount;

            for (i = 0; i < childCount; ++i)
                MonoBehaviour.Destroy(parentObject.transform.GetChild(i).gameObject);
        }
        public static void ClearChildObject(this Transform transformObject)
        {
            int i, childCount = transformObject.childCount;

            for (i = 0; i < childCount; ++i)
                MonoBehaviour.Destroy(transformObject.GetChild(i).gameObject);
        }
    */
        #endregion
    }
}
