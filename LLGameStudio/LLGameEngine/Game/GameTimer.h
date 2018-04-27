#pragma once
#include <windows.h>

class GameTimer
{
public:
	GameTimer();
	float GetAllTime();
	float GetThisTickTime()const;
	float GetRunTime();
	void Reset();
	void Start();
	void Stop();
	void Tick();

	__int64 countPerSecond;//机器每秒运行次数。
	__int64 baseCount;//在创建Timer类时记录的机器已经运行的次数。
	__int64 runCount;//只记录计时器运行时间内及其运行的次数。
	__int64 lastStopCount;
	__int64 lastStartCount;
	__int64 lastTickCount;
	__int64 countPerTick;
	float secondPerTick;
	bool stop;

};