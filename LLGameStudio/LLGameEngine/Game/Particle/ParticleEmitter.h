#pragma once
#include "..\..\Common\LLBase.h"
#include "..\UI\IUIProperty.h"
#include "..\UI\IUINode.h"

class ParticleSystem;

class Particle
{
public:
	float x;
	float y;
	float radius;
	float leftTime;
	Vector2 particleDirection;

	Particle(float x, float y, float radius, Vector2 particleDirection)
	{
		this->x = x;
		this->y = y;
		this->radius = radius;
		this->particleDirection = particleDirection;
		leftTime = FLT_MAX;
	}

	Particle(double x, double y, double radius, double directionX, double directionY)
	{
		this->x = x;
		this->y = y;
		this->radius = radius;
		particleDirection = Vector2(directionX, directionY);
		leftTime = FLT_MAX;
	}

	Particle(double x, double y, double radius, double leftTime, double directionX, double directionY)
	{
		this->x = x;
		this->y = y;
		this->radius = radius;
		particleDirection = Vector2(directionX, directionY);
		this->leftTime = leftTime;
	}

	// 粒子移动给定时间,计算移动后的位置。
	bool MoveByTime(double time)
	{
		x += particleDirection.x * time;
		y += particleDirection.y * time;
		leftTime -= time;
		return leftTime <= 0;
	}
};

class ParticleEmitter :public IUINode
{
public:
	ParticleEmitter();
	void SetProperty(wstring name, wstring value);
	virtual void Update() override;
	virtual void Render() override;

	void StartPlay();
	void PausePlay();
	void StopPlay();

private:
	void ResetParticle();
	void AddParticle();
	void InitParticle();

	ParticleSystem* particleSystem;

	PropertyIsLoop isLoop;
	PropertyLoopTime loopTime;
	PropertyParticleType particleType;
	PropertyMaxNumber maxNumber;
	PropertyStartNumber startNumber;
	PropertyCreateNumberBySecond createNumberBySecond;
	PropertyRadius radius;
	PropertyRadiusError radiusError;
	PropertyColor color;
	PropertyVelocity velocity;
	PropertyVelocityError velocityError;
	PropertyDirection direction;
	PropertyAngleRange angleRange;
	PropertyPosition position;
	PropertyPositionError positionError;
	PropertyImagePath imagePath;
	PropertyRow row;
	PropertyColumn column;

	bool play = true;
	double currentPlayTime = 0;//粒子当前已经播放的时间，每循环一次重新计时。
	int currentImageIndex = 0;//粒子类型为序列图时，当前播放的是第几帧。

	vector<Particle> particleList;
	void* polygon = nullptr;
	void* colorBrush = nullptr;
};