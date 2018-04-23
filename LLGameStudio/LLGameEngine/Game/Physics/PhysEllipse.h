#pragma once
#include "IPhysObject.h"

class PhysEllipse :public IPhysObject
{
public:
	PhysEllipse();
protected:
	virtual void CreateBoundingVolume() override;
};