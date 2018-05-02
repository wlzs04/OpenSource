#include "MathHelper.h"

const float MathHelper::PI = 3.14159265358979323846;

bool MathHelper::IsRange1To0(double d)
{
	return d > 0 && d <= 1;
}

bool MathHelper::IsPointInRect(Vector2 vector2, Rect rect)
{
	return vector2.x<rect.right
		&&vector2.x>rect.left
		&&vector2.y<rect.bottom
		&&vector2.y>rect.top;
}

int MathHelper::RoundFloat(float f)
{
	return f+0.5f;
}

float MathHelper::GetLengthBetweenPoints(Vector2 p1, Vector2 p2)
{
	return sqrtf((p2.x - p1.x)*(p2.x - p1.x) + (p2.y - p1.y)*(p2.y - p1.y));
}

Vector2 MathHelper::GetNormalVector2ByPoints(Vector2 vstart, Vector2 vend)
{
	return MathHelper::GetNormalVector2(Vector2(vend.x- vstart.x, vend.y - vstart.y));
}

Vector2 MathHelper::GetNormalVector2(Vector2 v1)
{
	double length = GetVector2Length(v1);
	return Vector2(v1.x / length, v1.y / length);
}

float MathHelper::GetVector2Length(Vector2 v1)
{
	return sqrtf(v1.x * v1.x + v1.y * v1.y);
}

Vector2 MathHelper::GetPointMoveByVelocityAndLength(Vector2 point, Vector2 velocity, float length)
{
	velocity = GetNormalVector2(velocity);
	point.x += velocity.x*length;
	point.y += velocity.y*length;
	return point;
}

Vector2 MathHelper::GetReflectByPlainVector2(Vector2 v1, Vector2 v2)
{

	return Vector2();
}

Vector2 MathHelper::GetReflectByNormalVector2(Vector2 v1, Vector2 v2)
{
	//R=I-2(I·N)N;
	Vector2 i = MathHelper::GetNormalVector2(v1);
	Vector2 n = MathHelper::GetNormalVector2(v2);
	float dm = MathHelper::GetDotMultiply(i, n);

	return Vector2(i.x - 2 * dm*n.x, i.y - 2 * dm*n.y);
}

float MathHelper::GetDotMultiply(Vector2 v1, Vector2 v2)
{
	return v1.x*v2.x+v1.y*v2.y;
}

Vector2 MathHelper::GetVector2SetLength(Vector2 v1, float length)
{
	return Vector2(v1.x*length, v1.y*length);
}

Vector2 MathHelper::GetRealVector2ByMapVector2AndLength(Vector2 v1, float length)
{

	return Vector2();
}

Vector2 MathHelper::GetNormalVector2AndPoint(Vector2 v1)
{

	Vector2 v2;
	v2.x = sqrtf(v1.y*v1.y / v1.x*v1.x + v1.y*v1.y);
	v2.y = sqrtf(v1.x*v1.x / v1.x*v1.x + v1.y*v1.y);
	return v2;
}

int MathHelper::RandInt(int start, int end)
{
	return start+(int)((end-start)*MathHelper::RandFloat());
}

float MathHelper::RandFloat()
{
	return rand() / (float)RAND_MAX;
}

float MathHelper::GetAngleBetweenVectors(Vector2 v1, Vector2 v2)
{
	double length1 = GetVector2Length(v1);
	double length2 = GetVector2Length(v2);
	double cross = v1.x * v2.x + v1.y * v2.y;
	return atan2(v2.y, v2.x) - atan2(v1.y, v1.x);
}
