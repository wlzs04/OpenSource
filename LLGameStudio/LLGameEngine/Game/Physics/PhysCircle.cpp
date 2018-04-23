#include "PhysCircle.h"
#include "PhysRectangle.h"
//class PhysRectangle;

PhysCircle::PhysCircle()
{
	physicsType = PhysicsType::Circle;
}

void PhysCircle::SetRedius(float radius)
{
	this->radius = radius;
}

float PhysCircle::GetRedius()
{
	return radius;
}

bool PhysCircle::IsCollision(IPhysObject * iPhysObject)
{
	switch (iPhysObject->GetPhysicsType())
	{
	case PhysicsType::Circle:
	{
		float length = MathHelper::GetLengthBetweenPoints(position,iPhysObject->GetPosition());
		return length < radius + ((PhysCircle*)iPhysObject)->GetRedius();
	}
	case PhysicsType::Rectangle:
	{
		PhysRectangle* rect = (PhysRectangle*)iPhysObject;
		Vector2 vp = rect->GetPosition();
		float xLength = abs(vp.x - position.x);
		float yLength = abs(vp.y - position.y);

		return xLength - radius - rect->GetWidth() < 0 || yLength - radius - rect->GetHeight() < 0;
	}
	default:
		break;
	}
	return false;
}

void PhysCircle::DoCollision(IPhysObject * iPhysObject)
{
	switch (iPhysObject->GetPhysicsType())
	{
	case PhysicsType::Circle:
	{
		//需要将方向单位化，等······
		Vector2 vv = MathHelper::GetNormalVector2(iPhysObject->GetVelocity());

		Vector2 vMid = Vector2((vv.x+velocity.x)/2, (vv.y + velocity.y)/2);

		float length = MathHelper::GetLengthBetweenPoints(position, iPhysObject->GetPosition());
		SetPosition();
	}
	default:
		break;
	}
}

void PhysCircle::CreateBoundingVolume()
{
}
