#pragma once
#include "IPhysObject.h"
#include <functional>
#include "PhysicsConstraint.h"

typedef function<void(IPhysObject* object1, IPhysObject* object2)> HandlePhysEvent;

class PhysicsWorld
{
public:
	PhysicsWorld();
	~PhysicsWorld();
	//添加物体到世界中
	void AddObject(IPhysObject* object);
	//添加约束到世界中
	void AddConstraint(PhysicsConstraint* constraint);
	float GetAllEnergyInWorld();
	void Start();
	void Stop();
	void Update(double time);

	vector<IPhysObject*> vectorActiveIPhysObject;
	vector<IPhysObject*> vectorDynamicIPhysObject;
	vector<IPhysObject*> vectorStaticIPhysObject;

	vector<PhysicsConstraint*> vectorConstraint;

	HandlePhysEvent OnCollisionEvent = nullptr;
private:
	bool run = false;
};