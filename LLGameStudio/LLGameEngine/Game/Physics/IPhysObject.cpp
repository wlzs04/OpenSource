#include "IPhysObject.h"

IPhysObject::IPhysObject()
{
	ResetEnergy();
}

void IPhysObject::SetPosition(Vector2 position)
{
	SetPosition(position.x, position.y);
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
	SetVelocity(velocity.x, velocity.y);
}

void IPhysObject::SetVelocity(float x, float y)
{
	velocity.x = x;
	velocity.y = y;
	ResetEnergy();
}

void IPhysObject::SetMass(float mass)
{
	this->mass = mass;
	ResetEnergy();
}

float IPhysObject::GetMass()
{
	return mass;
}

Vector2 IPhysObject::GetVelocity()
{
	return velocity;
}

float IPhysObject::GetEnergy()
{
	return energy;
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

void IPhysObject::ResetEnergy()
{
	energy = 0.5*mass*(velocity.x*velocity.x + velocity.y*velocity.y);
}
