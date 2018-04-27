#pragma once
#include "IPhysObject.h"

class PhysicsConstraint
{
public:
	PhysicsConstraint(IPhysObject* iPhysObject);
	//设置约束点和最大移动距离
	void SetPointConstrain(Vector2 constrainPoint,float maxLength);
	void CheckConstrain();
	IPhysObject* iPhysObject;
	Vector2 constrainPoint;
	float maxLength;
};