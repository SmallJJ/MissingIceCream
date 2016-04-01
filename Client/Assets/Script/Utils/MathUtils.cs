using UnityEngine;
public static class MathUtils
{
	private static Vector2? scaleVect;
	public static Vector3 GetVectorByCamera(Camera cam, Vector2 vect)
	{
        float num = Mathf.Cos(0.0174532924f * cam.transform.localEulerAngles.x);
		float num2 = Mathf.Sin(0.0174532924f * cam.transform.localEulerAngles.x);
		float num3 = Mathf.Cos(0.0174532924f * cam.transform.localEulerAngles.y);
		float num4 = Mathf.Sin(0.0174532924f * cam.transform.localEulerAngles.y);
		return new Vector3(vect.x * num3 + vect.y * num4, 0f, -vect.x * num * num4 + vect.y * num3 * num2);
	}
	public static bool Contains(this Rect rect, Rect otherRect)
	{
		Vector2 vector = new Vector2(otherRect.xMin, otherRect.yMin);
		Vector2 vector2 = new Vector2(otherRect.xMax, otherRect.yMax);
		return rect.Contains(vector) && rect.Contains(vector2);
	}
	public static bool IsStartPositionInRect(this Gesture gesture, Rect rect)
	{
		return rect.Contains(gesture.startPosition.GetGUIPosition());
	}
	public static bool IsCurrentPositionInRect(this Gesture gesture, Rect rect)
	{
		return rect.Contains(gesture.position.GetGUIPosition());
	}
	public static bool IsStartPositionAndCurrentPositionInRect(this Gesture gesture, Rect rect)
	{
		return gesture.IsStartPositionInRect(rect) && gesture.IsCurrentPositionInRect(rect);
	}
	public static Vector2 GetScreenScale()
	{
		if (!scaleVect.HasValue)
		{
			Vector2 value = new Vector2((float)Screen.width / UIConst.ScreenWidth, (float)Screen.height / UIConst.ScreenHeight);
			scaleVect = new Vector2?(value);
		}
        return scaleVect.Value;
	}
	public static Rect AdjustScreen(this Rect rect)
	{
		Vector2 screenScale = MathUtils.GetScreenScale();
		return new Rect(rect.x * screenScale.x, rect.x * screenScale.y, rect.width * screenScale.x, rect.height * screenScale.y);
	}
	public static Vector2 AdjustScreen(this Vector2 vect)
	{
		Vector2 screenScale = MathUtils.GetScreenScale();
		return new Vector2(vect.x / screenScale.x, vect.y / screenScale.y);
	}
	public static Vector2 GetGUIPosition(this Vector2 position)
	{
		return new Vector2(position.x, (float)Screen.height - position.y);
	}
	public static Rect GetTouchRect(Vector3 position, Vector2 touchCenter, Vector2 touchSize)
	{
		Vector2 position2 = CameraCtrl.Instance.MainCamera.WorldToScreenPoint(position);
		Vector2 gUIPosition = position2.GetGUIPosition();
		Vector2 vector = gUIPosition.AdjustScreen();
		Rect rect = new Rect(vector.x + touchCenter.x - touchSize.x / 2f, vector.y + touchCenter.y - touchSize.y / 2f, touchSize.x, touchSize.y);
		return rect.AdjustScreen();
	}
	
	public static Vector2? VectorIntersectsCircle(Vector2 start, Vector2 end, Vector2 center, float radius, out byte step)
	{
		if (MathUtils.PointInCircle(start, center, radius))
		{
			step = 0;
			return new Vector2?(start);
		}
		Vector2 vector = end - start;
		Vector2 normalized = vector.normalized;
		Vector2 vector2 = center - start;
		float num = Vector2.Dot(normalized, vector2);
		if (num < 0f)
		{
			step = 1;
			return null;
		}
		float num2 = vector2.sqrMagnitude - num * num;
		float num3 = radius * radius;
		if (num2 >= num3)
		{
			step = 4;
			return null;
		}
		float num4 = num3 - num2;
		float num5 = Mathf.Abs(num) - Mathf.Sqrt(num4);
        float num6 = num5 / vector.magnitude;
		if (num6 > 1f)
		{
			step = 2;
			return null;
		}
		Vector2 value = start + num6 * vector;
		step = 3;
		return new Vector2?(value);
	}
	public static bool PointInCircle(Vector2 point, Vector2 center, float radius)
	{
		return Vector2.Distance(point, center) < radius;
	}
	public static void LimitInScreen(this Transform trans, Transform relative)
	{
		if (relative == null)
		{
			return;
		}
		Vector2 screenScale = MathUtils.GetScreenScale();
        Collider collider= relative.GetComponent<Collider>();
        Bounds bounds = (!(collider == null)) ? collider.bounds : UGUIMathf.calculateRelativeBounds(relative.rectTransform());
		Vector2 vector = new Vector3(bounds.extents.x, bounds.extents.y);
		Bounds bounds2 = UGUIMathf.calculateRelativeBounds(trans.rectTransform());
		Vector2 vector2 = new Vector3(bounds2.extents.x * screenScale.x, bounds2.extents.y * screenScale.y);
		Vector2 vector3 = new Vector3(bounds2.center.x * screenScale.x, bounds2.center.y * screenScale.y);
		Vector3 vector4 = relative.position + new Vector3(0f, vector.y, 0f);
		float num = CameraCtrl.Instance.MainCamera.WorldToScreenPoint(vector4).y + vector2.y;
        float num2 = num + vector2.y - (float)Screen.height;
		float num3;
		if (num2 < 0f)
		{
			num3 = CameraCtrl.Instance.MainCamera.WorldToScreenPoint(relative.position).x;
            float num4 = num3 + vector2.x - (float)Screen.width;
			if (num4 > 0f)
			{
				num3 -= num4;
			}
		}
		else
		{
			num = CameraCtrl.Instance.MainCamera.WorldToScreenPoint(relative.position).y;
            float num5 = num + vector2.y - (float)Screen.height;
			if (num5 >= 0f)
			{
				num -= num5;
			}
			num3 = CameraCtrl.Instance.MainCamera.WorldToScreenPoint(relative.position + new Vector3(vector.x, 0f, 0f)).x + vector2.x;
            float num6 = num3 + vector2.x - (float)Screen.width;
			if (num6 > 0f)
			{
				num3 = CameraCtrl.Instance.MainCamera.WorldToScreenPoint(relative.position - new Vector3(vector.x, 0f, 0f)).x - vector2.x;
			}
		}
		Vector3 vector5 = new Vector3(num3 - vector3.x, num - vector3.y, trans.position.z);
		trans.position=(CameraCtrl.Instance.MainCamera.ScreenToWorldPoint(vector5));
	}
	public static void LimitInScreen(this Transform trans, Vector3 position)
	{
        Bounds bounds = UGUIMathf.calculateWorldBounds(trans.rectTransform());
        Vector2 screenScale = GetScreenScale();
		Vector2 extents = new Vector2(bounds.extents.x * screenScale.x, bounds.extents.y * screenScale.y);
		Vector2 center = new Vector2(bounds.center.x * screenScale.x, bounds.center.y * screenScale.y);
		Vector2 size = new Vector2(bounds.size.x * screenScale.x, bounds.size.y * screenScale.y);
		float num = position.x + extents.x + center.x;
		float num2 = position.y - extents.y - center.y;
		float num3 = position.x + size.x;
		float num4 = position.y - size.y;
		num = ((num3 <= (float)Screen.width) ? num : (num - (num3 - (float)Screen.width)));
		num2 = ((num4 >= 0f) ? num2 : (num2 + Mathf.Abs(num4)));
		Vector3 vector4 = new Vector3(num, num2, 0f);
        Vector3 vector5 = CameraCtrl.Instance.MainCamera.ScreenToViewportPoint(vector4);
		trans.localPosition=new Vector3(vector5.x, vector5.y, trans.position.z);
	}
	public static void LimitInScreen(this Transform trans)
	{
        Vector2 vector = Input.mousePosition;
		trans.LimitInScreen(vector);
	}
	public static Rect ConvertBoundsToScreenRect(this Bounds bounds, Camera camera)
	{
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
		Vector3[] array = new Vector3[]
		{
			camera.WorldToScreenPoint(min),
			camera.WorldToScreenPoint(new Vector3(max.x, min.y, min.z)),
			camera.WorldToScreenPoint(new Vector3(min.x, max.y, min.z)),
			camera.WorldToScreenPoint(new Vector3(min.x, min.y, max.z)),
			camera.WorldToScreenPoint(new Vector3(max.x, max.y, min.z)),
			camera.WorldToScreenPoint(new Vector3(max.x, min.y, max.z)),
			camera.WorldToScreenPoint(new Vector3(min.x, max.y, max.z)),
			camera.WorldToScreenPoint(max)
		};
		Rect result = default(Rect);
        result.xMax = float.MaxValue;
        result.xMin = float.MinValue;
        result.yMin = float.MinValue;
        result.yMax = float.MaxValue;
		Vector3[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Vector3 vector = array2[i];
			if (vector.x < result.xMin)
			{
				result.xMin=vector.x;
			}
			if (vector.x > result.xMax)
			{
				result.xMax=vector.x;
			}
			if (vector.y < result.yMin)
			{
				result.yMin=vector.y;
			}
			if (vector.y > result.yMax)
			{
				result.yMax=vector.y;
			}
		}
		return result;
	}
}
