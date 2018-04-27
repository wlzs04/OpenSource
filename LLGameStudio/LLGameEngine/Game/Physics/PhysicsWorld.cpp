#include "PhysicsWorld.h"

PhysicsWorld::PhysicsWorld()
{
}

PhysicsWorld::~PhysicsWorld()
{
	for (auto var : vectorDynamicIPhysObject)
	{
		delete var;
		var = nullptr;
	}
	for (auto var : vectorStaticIPhysObject)
	{
		delete var;
		var = nullptr;
	}
}

void PhysicsWorld::AddObject(IPhysObject * object)
{
	if (object->IsDynamic())
	{
		vectorDynamicIPhysObject.push_back(object);
	}
	else
	{
		vectorStaticIPhysObject.push_back(object);
	}
}

float PhysicsWorld::GetAllEnergyInWorld()
{
	float allEnergy = 0;
	for (auto var : vectorDynamicIPhysObject)
	{
		allEnergy += var->GetEnergy();
	}
	return allEnergy;
}

void PhysicsWorld::Start()
{
	run = true;
}

void PhysicsWorld::Stop()
{
	run = false;
}

void PhysicsWorld::Update(double time)
{
	if (run)
	{
		vector<IPhysObject*> newvectorIPhysObject;
		for (auto var : vectorStaticIPhysObject)
		{
			newvectorIPhysObject.push_back(var);
		}
		for (auto var1 : vectorDynamicIPhysObject)
		{
			for (auto var2 : newvectorIPhysObject)
			{
				if (var1 != var2 && var1->IsCollision(var2))
				{
					if (OnCollisionEvent)
					{
						OnCollisionEvent(var1, var2);
					}
					var1->DoCollision(var2);
				}
			}
			newvectorIPhysObject.push_back(var1);
		}

		for (auto var : vectorDynamicIPhysObject)
		{
			Vector2 vp = var->GetPosition();
			Vector2 vv = var->GetVelocity();
			var->SetPosition(vp.x + vv.x*time, vp.y + vv.y*time);
		}
	}
	
}
