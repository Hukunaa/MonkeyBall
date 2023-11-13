using UnityEngine;

namespace CustomUtilities
{
	public struct MathCalculation
	{
		public static bool ApproximatelyEqualFloat(float a, float b, float tolerance)
		{
			return (Mathf.Abs(a - b) < tolerance);
		}

		public static bool ApproximatelyEqualVector2(Vector2 a, Vector2 b, float tolerance)
		{
			return ((Mathf.Abs(a.x - b.x) < tolerance) && (Mathf.Abs(a.y - b.y) < tolerance));
		}

		public static bool AreAngleApproximatelyEqual(float angle1, float angle2, float tolerance)
		{
			var deltaAngle = Mathf.Abs(Mathf.DeltaAngle(angle1, angle2));
			return deltaAngle <= tolerance;
		}

		public static Vector2 RadianToVector2(float radian)
		{
			return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
		}

		public static Vector2 RadianToVector2(float radian, float length)
		{
			return RadianToVector2(radian) * length;
		}

		public static Vector2 ConvertAngleToDirection(float degree)
		{
			return RadianToVector2((degree) * Mathf.Deg2Rad);
		}

		public static float ConvertDirectionToAngle(Vector2 direction)
		{
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			angle = Mathf.RoundToInt(angle);

			if (angle < 0)
				angle += 360;

			return angle;
		}

		public static float Remap(float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		public static float ClampAngle(float angle)
		{
			if (angle < 0)
				angle += 360;

			if (angle > 360)
				angle -= 360;

			return angle;
		}
		
		public static Vector2 RotateVector(Vector2 v, float angle)
		{
			float radian = angle * Mathf.Deg2Rad;
			float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
			float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
			return new Vector2(_x, _y);
		}
		public static Vector2Int RotateVector(Vector2Int v, float angle)
        {
			Vector2 value = RotateVector(new Vector2((float)v.x, (float)v.y), angle);
			return new Vector2Int((int)value.x, (int)value.y);
        }
		public static Vector2 GetMiddlePositionBetween2Points(Vector2 pointA, Vector2 pointB)
		{
			return (pointA + pointB) / 2;
		}
	
		public static Vector2 GetDirectionalVectorBetween2Points(Vector2 pointA, Vector2 pointB)
		{
			return (pointB - pointA).normalized;
		}

		public static Vector2 GetWorldToScreenPos(Camera _cam, RectTransform _canvasRect, Transform _target, Vector2 _offset)
		{
			Vector2 ViewportPosition = _cam.WorldToViewportPoint(_target.transform.position);
			Vector2 WorldObject_ScreenPosition = new Vector2(
			 ((ViewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * _canvasRect.pivot.x)),
			 ((ViewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * _canvasRect.pivot.y)));
			return WorldObject_ScreenPosition + _offset;
		}

		public static float GetAngleBetween2Points(Vector2 pointA, Vector2 pointB)
		{
			return ConvertDirectionToAngle(GetDirectionalVectorBetween2Points(pointA, pointB));
		}

		public static float CalculateTopVelocity(Rigidbody2D rb2d, Vector2 addedForce)
		{
			return  ((addedForce.magnitude / rb2d.drag) - Time.fixedDeltaTime * addedForce.magnitude) / rb2d.mass;
		}

		public static Vector3 RotateVector(Vector3 vector, Vector3 axis, float angle)
		{
			Vector3 vxp = Vector3.Cross(axis, vector);
			Vector3 vxvxp = Vector3.Cross(axis, vxp);
			return vector + Mathf.Sin(Mathf.Deg2Rad * angle) * vxp + (1 - Mathf.Cos(Mathf.Deg2Rad * angle)) * vxvxp;
		}
	}
}
