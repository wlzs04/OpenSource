#pragma once
#include "IPhysObject.h"
#include <functional>

typedef function<void(IPhysObject* object1, IPhysObject* object2)> HandlePhysEvent;

class PhysicsWorld
{
public:
	PhysicsWorld();
	~PhysicsWorld();
	void AddObject(IPhysObject* object);
	float GetAllEnergyInWorld();
	void Start();
	void Stop();
	void Update(double time);

	vector<IPhysObject*> vectorDynamicIPhysObject;
	vector<IPhysObject*> vectorStaticIPhysObject;

	HandlePhysEvent OnCollisionEvent = nullptr;
private:
	bool run = false;
};