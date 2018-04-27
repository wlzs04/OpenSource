#include "GameTimer.h"

GameTimer::GameTimer()
{
	stop = true;
	countPerSecond = 0; baseCount = 0; lastStopCount = 0; lastStartCount = 0; lastTickCount = 0; countPerTick = 0; secondPerTick = 0;
	QueryPerformanceFrequency((LARGE_INTEGER*)&countPerSecond);
	QueryPerformanceCounter((LARGE_INTEGER*)&baseCount);
}

float GameTimer::GetAllTime()
{
	__int64 currentCount;
	QueryPerformanceCounter((LARGE_INTEGER*)&currentCount);
	return (currentCount - baseCount) / countPerSecond;
}

float GameTimer::GetThisTickTime() const
{
	return secondPerTick;
}

float GameTimer::GetRunTime()
{
	return runCount / countPerSecond;
}

void GameTimer::Reset()
{
	stop = false;
	countPerSecond = 0; baseCount = 0; lastStopCount = 0; lastStartCount = 0; lastTickCount = 0; countPerTick = 0; secondPerTick = 0; runCount = 0;
	QueryPerformanceFrequency((LARGE_INTEGER*)&countPerSecond);
	QueryPerformanceCounter((LARGE_INTEGER*)&baseCount);
}

void GameTimer::Start()
{
	stop = false;
	QueryPerformanceCounter((LARGE_INTEGER*)&lastTickCount);
	QueryPerformanceCounter((LARGE_INTEGER*)&lastStartCount);
}

void GameTimer::Stop()
{
	stop = true;
	QueryPerformanceCounter((LARGE_INTEGER*)&lastTickCount);
	QueryPerformanceCounter((LARGE_INTEGER*)&lastStopCount);
}

void GameTimer::Tick()
{
	if (stop)
	{
		countPerTick = 0.0f;
		secondPerTick = 0.0f;
		return;
	}
	__int64 tempCount;
	QueryPerformanceCounter((LARGE_INTEGER*)&tempCount);
	countPerTick = tempCount - lastTickCount;
	runCount += countPerTick;
	secondPerTick = (float)countPerTick / (double)countPerSecond;
	lastTickCount = tempCount;
}