#include "PhysicsManager.h"

PhysicsWorld* PhysicsManager::CreatePhysicsWorld()
{
	return new PhysicsWorld();
}

PhysRectangle* PhysicsManager::CreateRectangle(int width, int height)
{
	PhysRectangle* rect = new PhysRectangle();
	rect->SetRect(width, height);
	return rect;
}

PhysCircle * PhysicsManager::CreateCircle(int radius)
{
	PhysCircle* circle = new PhysCircle();
	circle->SetRedius(radius);
	return circle;
}
