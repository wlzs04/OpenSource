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

PhysicsShapeType IPhysObject::GetPhysicsShapeType()
{
	return physicsShapeType;
}

void IPhysObject::SetDynamic()
{
	physicsStateType = PhysicsStateType::Dynamic;
}

void IPhysObject::SetStatic()
{
	physicsStateType = PhysicsStateType::Static;
}

void IPhysObject::SetActive()
{
	physicsStateType = PhysicsStateType::Active;
}

PhysicsStateType IPhysObject::GetPhysicsStateType()
{
	return physicsStateType;
}

bool IPhysObject::IsDynamic()
{
	return physicsStateType== PhysicsStateType::Dynamic;
}

bool IPhysObject::IsCanMove()
{
	return physicsStateType == PhysicsStateType::Dynamic|| physicsStateType == PhysicsStateType::Active;
}

void IPhysObject::ResetEnergy()
{
	energy = 0.5*mass*(velocity.x*velocity.x + velocity.y*velocity.y);
}
