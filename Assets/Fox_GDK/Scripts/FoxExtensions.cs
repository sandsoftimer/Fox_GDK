using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

#if DOTWEEN
using DG.Tweening;
#endif

public static class FoxExtensions
{
    public static List<Transform> GetChildrenWithTag(this Transform transform, string tag, bool only_Immidiate_Children = true, List<Transform> childrenList = null)
    {
        if (transform.childCount == 0)
        {
            return null;
        }

        if (childrenList == null)
        {
            childrenList = new List<Transform>();
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag(tag))
            {
                childrenList.Add(transform);
            }

            if (!only_Immidiate_Children)
                GetChildrenWithTag(transform.GetChild(i), tag, only_Immidiate_Children, childrenList);
        }

        return childrenList;
    }

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    public static void SqueezeEffect(this Transform target, SqueezableData squeezableData, float duration, Action action = null)
    {
#if DOTWEEN
        target.DOScale(Vector3.one.FOXE_ModifyThisVector(-squeezableData.squeezeUpValue, squeezableData.bumpUpValue, -squeezableData.squeezeUpValue), duration / 2).SetEase(Ease.InOutQuart).OnComplete(() =>
        {
            target.DOScale(Vector3.one.FOXE_ModifyThisVector(squeezableData.squeezeDownValue, -squeezableData.bumpDownValue, squeezableData.squeezeDownValue), duration / 2).SetEase(Ease.InOutQuart).OnComplete(() =>
            {
                target.DOScale(Vector3.one, duration / 4).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    action?.Invoke();
                });
            }).SetEase(Ease.InQuint);
        }).SetEase(Ease.OutQuint);
#else
        Debug.LogError("DO TWEEN not installed");
#endif
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static T FOXE_Get_A_Random_List_Item<T>(this List<T> tList) where T : new()
    {
        return tList[Random.Range(0, tList.Count)];
    }

    public static void FOXE_ToFloat(this float value, float destinationValue, float time, Action<float> OnUpdate = null, Action OnComplete = null)
    {
#if DOTWEEN
        DOTween.To(() => value, x => value = x, destinationValue, time)
            .OnUpdate(() =>
            {
                //Debug.Log(Mathf.Sin(value * Mathf.Rad2Deg));
                OnUpdate?.Invoke(value);
            }).OnComplete(() =>
            {
                OnComplete?.Invoke();
            });
#else
        Debug.LogError("DO TWEEN not installed");
#endif
    }

    public static int FOXE_Get_Enum_Count(this Enum _enum)
    {
        return Enum.GetNames(_enum.GetType()).Length;
    }

    public static string FOXE_Placement(this int value)
    {
        string placement = "";
        switch (value)
        {
            case 1:
                placement = "st";
                break;
            case 2:
                placement = "nd";
                break;
            case 3:
                placement = "rd";
                break;
            default:
                placement = "th";
                break;
        }
        return placement;
    }

    public static float FOXE_Get_Angle(this Transform transform, Vector3 direction, Vector3 axis)
    {
        return -Vector3.SignedAngle(direction, transform.forward, axis);
    }

    public static bool FOXE_IsInLayerMask(this LayerMask mask, GameObject obj)
    {
        return (mask.value & (1 << obj.layer)) > 0;
    }

    public static void FOXE_SetClip(this Animator anim, string statName, AnimationClip clip, float animationSpeed = 1)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (var a in aoc.animationClips)
        {
            if (a.name.Equals(statName))
            {
                //Debug.LogError($"{a.Name} == {clip.Name}");
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, clip));
            }
            else
            {
                //Debug.LogError($"{a.Name} != {statName}");
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, a));
            }
        }
        aoc.ApplyOverrides(anims);
        anim.runtimeAnimatorController = aoc;
        anim.speed = animationSpeed;
    }

    public static void FOXE_SetClip(this RuntimeAnimatorController runtimeConctroller, string statName, AnimationClip clip, float animationSpeed = 1)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(runtimeConctroller);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (var a in aoc.animationClips)
        {
            if (a.name.Equals(statName))
            {
                //Debug.LogError($"{a.Name} == {clip.Name}");
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, clip));
            }
            else
            {
                //Debug.LogError($"{a.Name} != {statName}");
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, a));
            }
        }
        aoc.ApplyOverrides(anims);
        runtimeConctroller = aoc;
    }

    public static void FOXE_Reset(this Transform transfrom)
    {
        transfrom.localPosition = Vector3.zero;
        transfrom.localEulerAngles = Vector3.zero;
        transfrom.localScale = Vector3.one;
    }

    public static void FOXE_Rotate_ClockWise_Up(this Transform transform)
    {
        transform.eulerAngles = transform.eulerAngles.FOXE_ModifyThisVector(0, 90, 0);
    }

    public static bool FOXE_IsOffScreen(this Transform transform, Camera camera, Vector2 viewOffset)
    {
        Vector3 viewPos = camera.WorldToViewportPoint(transform.position);
        if (viewPos.x >= -viewOffset.x && viewPos.x <= 1 + viewOffset.x && viewPos.y >= -viewOffset.y && viewPos.y <= 1 + viewOffset.y && viewPos.z > 0)
            return false;
        else
            return true;
    }

    public static Vector2 FOXE_OffScreenIndicatorPoint(this Transform transform, Camera camera, Vector2 iconOffset)
    {
        Vector2 point = Vector2.zero;
        Vector3 viewPos = camera.WorldToScreenPoint(transform.position);
        point.x = Mathf.Clamp(viewPos.x, 0, Screen.width);
        point.y = Mathf.Clamp(viewPos.y, 0, Screen.height);

        point.x += iconOffset.x * (point.x > (Screen.width / 2) ? -1 : 1);
        point.y += iconOffset.y * (point.y > (Screen.height / 2) ? -1 : 1);

        return point;
    }

    public static bool FOXE_IsGrounded(this Transform transform, Vector3 raycastOffse, float groundThreshold, int groundLayer)
    {
        return Physics.Raycast(transform.position + raycastOffse, Vector3.down, groundThreshold, groundLayer);
    }

    public static void FOXE_TryUpdateShapeToAttachedSprite(this PolygonCollider2D collider)
    {
        collider.FOXE_UpdateShapeToSprite(collider.GetComponent<SpriteRenderer>().sprite);
    }

    public static void FOXE_UpdateShapeToSprite(this PolygonCollider2D collider, Sprite sprite)
    {
        // ensure both valid
        if (collider != null && sprite != null)
        {
            // update count
            collider.pathCount = sprite.GetPhysicsShapeCount();

            // new paths variable
            List<Vector2> path = new List<Vector2>();

            // loop path count
            for (int i = 0; i < collider.pathCount; i++)
            {
                // clear
                path.Clear();
                // get shape
                sprite.GetPhysicsShape(i, path);
                // set path
                collider.SetPath(i, path.ToArray());
            }
        }
    }

    public static Vector3 FOXE_ClampVector(this Vector3 value, Vector3 minVector, Vector3 maxVector)
    {
        value.x = Mathf.Clamp(value.x, minVector.x, maxVector.x);
        value.y = Mathf.Clamp(value.y, minVector.y, maxVector.y);
        value.z = Mathf.Clamp(value.z, minVector.z, maxVector.z);
        return value;
    }

    public static Texture2D FOXE_Texture2D(this RenderTexture renderTexture)
    {
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        var old_rt = RenderTexture.active;
        RenderTexture.active = renderTexture;

        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        RenderTexture.active = old_rt;
        return tex;
    }

    public static void FOXE_DestroyAllChild(this Transform transform, bool imidiatly = false)
    {
        for (int i = transform.childCount - 1; i > -1; i--)
        {
            if (imidiatly)
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            else
                GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public static Color FOXE_GetRandomColor()
    {
        return Random.ColorHSV();
    }

    public static RaycastHit FOXE_GetRaycastHitPoint(this Transform t, Vector3 direction, int layerMask)
    {
        RaycastHit hit;
        Physics.Raycast(new Ray(t.position, direction), out hit, Mathf.Infinity, layerMask);
        return hit;
    }

    public static RaycastHit FOXE_GetRaycastHitPoint(this Transform t, Vector3 direction, Vector3 positionOffset, float distance, int layerMask)
    {
        RaycastHit hit;
        Physics.Raycast(new Ray(t.position + positionOffset, direction), out hit, distance, layerMask);
        return hit;
    }

    public static void FOXE_GetRaycastFromScreenTouch(ref this RaycastHit hit, Vector3 offset, int layerMask = 1)
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition + offset), out hit, Mathf.Infinity, layerMask);
        //return hit;
    }

    // Generate random normalized direction
    public static Vector2 FOXE_GetRandomDirection2D()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    // Generate random normalized direction
    public static Vector3 FOXE_GetRandomDirection3D()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    public static GameObject FOXE_ActiveChild(this Transform transform)
    {
        return transform.FOXE_ActiveChild(Random.Range(0, transform.childCount));
    }

    public static GameObject FOXE_ActiveChild(this Transform transform, int childIndex)
    {
        GameObject go = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == childIndex)
            {
                go = transform.GetChild(i).gameObject;
            }
            transform.GetChild(i).gameObject.SetActive(i == childIndex);
        }
        return go;
    }

    public static GameObject FOXE_ActiveChild(this Transform transfrom, GameObject gameObject)
    {
        GameObject go = null;
        for (int i = 0; i < transfrom.childCount; i++)
        {
            if (transfrom.GetChild(i).gameObject == gameObject)
            {
                go = transfrom.GetChild(i).gameObject;
            }
            transfrom.GetChild(i).gameObject.SetActive(transfrom.GetChild(i).gameObject == gameObject);
        }
        return go;
    }

    public static GameObject FOXE_ActiveChild(this Transform transfrom, string name)
    {
        GameObject go = null;
        for (int i = 0; i < transfrom.childCount; i++)
        {
            if (transfrom.GetChild(i).gameObject.name.Equals(name))
            {
                go = transfrom.GetChild(i).gameObject;
            }
            transfrom.GetChild(i).gameObject.SetActive(transfrom.GetChild(i).name.Equals(name));
        }
        return go;
    }

    public static float FOXE_DistanceFrom(this Transform _transform, Transform comparingTransform, KV_Axis aPAxis = KV_Axis.ALL)
    {
        return FOXE_DistanceFrom(_transform.position, comparingTransform.position, aPAxis);
    }

    public static float FOXE_DistanceFrom(this Transform _transform, Vector3 comparingPosition, KV_Axis aPAxis = KV_Axis.ALL)
    {
        return FOXE_DistanceFrom(_transform.position, comparingPosition, aPAxis);
    }

    public static float FOXE_DistanceFrom(this Vector3 _transform, Vector3 comparingPosition, KV_Axis aPAxis = KV_Axis.ALL)
    {
        float distance = Mathf.Infinity;
        switch (aPAxis)
        {
            case KV_Axis.ALL:
                distance = Vector3.Distance(_transform, comparingPosition);
                break;
            case KV_Axis.X:
                distance = Mathf.Abs(_transform.x - comparingPosition.x);
                break;
            case KV_Axis.Y:
                distance = Mathf.Abs(_transform.y - comparingPosition.y);
                break;
            case KV_Axis.Z:
                distance = Mathf.Abs(_transform.z - comparingPosition.z);
                break;
        }
        return distance;
    }

    public static Vector3 FOXE_ModifyThisVector(this Vector3 value, float x, float y, float z)
    {
        return new Vector3(value.x + x, value.y + y, value.z + z);
    }

    public static Vector3 FOXE_ModifyThisVector(this Vector3 value, Vector3 vector)
    {
        return value.FOXE_ModifyThisVector(vector.x, vector.y, vector.z);
    }

    public static Transform FOXE_GetClosestTransform(this Transform t, List<Transform> list)
    {
        List<Transform> transforms = list.OrderBy(i => Vector3.Distance(t.position, i.position)).ToList();
        return transforms.Count > 0 ? transforms[0] : null;
    }

    public static RaycastHit[] FOXE_Get_Surrounding_Enemies(this Transform transform, float radious, LayerMask enemyLayer)
    {
        return Physics.SphereCastAll(transform.position, radious, Vector3.one, radious / 2, enemyLayer);
    }

    public static void UI_Linerenderer(this RectTransform lineImageRect, RectTransform point1, RectTransform point2, float lineWidth)
    {
        Vector2 dotPositionA, dotPositionB;
        dotPositionA = point1.anchoredPosition;
        dotPositionB = point2.anchoredPosition;

        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        lineImageRect.sizeDelta = new Vector2(lineWidth, distance);
        lineImageRect.anchoredPosition = dir * distance * 0.5f;
        lineImageRect.localEulerAngles = new Vector3(0, 0, angle - 90);
    }

}
