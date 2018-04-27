#include "PhysicsConstraint.h"

PhysicsConstraint::PhysicsConstraint(IPhysObject * iPhysObject)
{
	this->iPhysObject = iPhysObject;
}

void PhysicsConstraint::SetPointConstrain(Vector2 constrainPoint, float maxLength)
{
	this->constrainPoint = constrainPoint;
	this->maxLength = maxLength;
}

void PhysicsConstraint::CheckConstrain()
{
	Vector2 objectPoint = iPhysObject->GetPosition();
	if (maxLength < MathHelper::GetLengthBetweenPoints(constrainPoint, objectPoint))
	{
		Vector2 vd = MathHelper::GetNormalVector2ByPoints(constrainPoint, objectPoint);
		vd = MathHelper::GetVector2SetLength(vd, maxLength);
		objectPoint.x = constrainPoint.x + vd.x;
		objectPoint.y = constrainPoint.y + vd.y;

		iPhysObject->SetVelocity(0, 0);
		iPhysObject->SetPosition(objectPoint);
	}
}

