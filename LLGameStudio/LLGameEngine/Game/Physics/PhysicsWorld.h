#pragma once
#include "IPhysObject.h"

class PhysicsWorld
{
public:
	PhysicsWorld();
	~PhysicsWorld();
	void AddObject(IPhysObject* object);
	void Update(double time);
};