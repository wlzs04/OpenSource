#include "PhysicsWorld.h"

PhysicsWorld::PhysicsWorld()
{
}

PhysicsWorld::~PhysicsWorld()
{
	for (auto var : vectorIPhysObject)
	{
		delete var;
		var = nullptr;
	}
}

void PhysicsWorld::AddObject(IPhysObject * object)
{
	vectorIPhysObject.push_back(object);
}

void PhysicsWorld::Update(double time)
{
	for (auto var1 : vectorIPhysObject)
	{
		if (var1->IsDynamic())
		{
			for (auto var2 : vectorIPhysObject)
			{
				if (var1 != var2 && var1->IsCollision(var2))
				{
					var1->DoCollision(var2);
				}
			}
		}
	}

	for (auto var : vectorIPhysObject)
	{
		if (var->IsDynamic())
		{
			Vector2 vp = var->GetPosition();
			Vector2 vv = var->GetVelocity();
			var->SetPosition(vp.x + vv.x, vp.y + vv.y);
		}
	}
}
