#pragma once
#include "IPhysObject.h"

class PhysTriangle :public IPhysObject
{
public:
	PhysTriangle();
protected:
	virtual void CreateBoundingVolume() override;
};