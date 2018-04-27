#include "PhysRectangle.h"

PhysRectangle::PhysRectangle()
{
	physicsShapeType = PhysicsShapeType::Rectangle;
}

void PhysRectangle::SetRect(float width, float height)
{
	this->width = width;
	this->height = height;
}

float PhysRectangle::GetWidth()
{
	return width;
}

float PhysRectangle::GetHeight()
{
	return height;
}

bool PhysRectangle::IsCollision(IPhysObject* iPhysObject)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void PhysRectangle::DoCollision(IPhysObject * iPhysObject)
{
}

void PhysRectangle::CreateBoundingVolume()
{

}
