#pragma once
#include "IPhysObject.h"

class PhysCircle :public IPhysObject
{
public:
	PhysCircle();
	void SetRedius(float radius);
	float GetRedius();
	virtual bool IsCollision(IPhysObject* iPhysObject) override;
	virtual void DoCollision(IPhysObject* iPhysObject) override;
protected:
	virtual void CreateBoundingVolume() override;
private:
	float radius;
};