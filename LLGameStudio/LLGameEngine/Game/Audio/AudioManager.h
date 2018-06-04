#pragma once
#include <unordered_map>
#include "Audio.h"

class AudioManager
{
public:
	static AudioManager* GetInstance();
	void Release();
	Audio* LoadSoundFromFile(wstring musicName);
	Audio* GetAudio(wstring musicName);
	void SetBackgroundMusic(wstring musicName);
	void StartBackgroundMusic();
	void PauseBackgroundMusic();
	void StopBackgroundMusic();
	void StartSoundEffect(wstring musicName);
	void StopAll(wstring musicName);
private:
	AudioManager() {}
	static AudioManager* audioManagerInstance;
	unordered_map<wstring, Audio*> soundMap;
	wstring currentBackgroundMusic;
};