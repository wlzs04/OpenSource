#include "IPhysObject.h"

void IPhysObject::SetPosition(Vector2 position)
{
	this->position = position;
}

void IPhysObject::SetPosition(float x, float y)
{
	position.x = x;
	position.y = y;
}

Vector2 IPhysObject::GetPosition()
{
	return position;
}

void IPhysObject::SetAngle(float angle)
{
	this->angle = angle;
}

void IPhysObject::SetVelocity(Vector2 velocity)
{
	this->velocity = velocity;
}

void IPhysObject::SetVelocity(float x, float y)
{
	velocity.x = x;
	velocity.y = y;
}

Vector2 IPhysObject::GetVelocity()
{
	return velocity;
}

PhysicsType IPhysObject::GetPhysicsType()
{
	return physicsType;
}

void IPhysObject::SetDynamic()
{
	isDynamic = true;
}

void IPhysObject::SetStatic()
{
	isDynamic = false;
}

bool IPhysObject::IsDynamic()
{
	return isDynamic;
}