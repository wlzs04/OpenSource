#include "AudioManager.h"

AudioManager* AudioManager::audioManagerInstance = new AudioManager();

AudioManager * AudioManager::GetInstance()
{
	return audioManagerInstance;
}

void AudioManager::Release()
{
	for(auto var : soundMap)
	{
		delete var.second;
	}
	soundMap.clear();
}

Audio* AudioManager::LoadSoundFromFile(wstring musicName)
{
	if (soundMap.count(musicName)==0)
	{
		Audio* audio = new Audio(musicName);
		soundMap[musicName] = audio;
		return audio;
	}
	return nullptr;
}

Audio * AudioManager::GetAudio(wstring musicName)
{
	if (soundMap.count(musicName) != 0)
	{
		return soundMap[musicName];
	}
	return nullptr;
}

void AudioManager::SetBackgroundMusic(wstring musicName)
{
	currentBackgroundMusic = musicName;
}

void AudioManager::StartBackgroundMusic()
{
	soundMap[currentBackgroundMusic]->Start();
}

void AudioManager::PauseBackgroundMusic()
{
	soundMap[currentBackgroundMusic]->Pause();
}

void AudioManager::StopBackgroundMusic()
{
	soundMap[currentBackgroundMusic]->Stop();
}

void AudioManager::StartSoundEffect(wstring musicName)
{
	soundMap[musicName]->Start();
}

void AudioManager::StopAll(wstring musicName)
{
	for (auto var : soundMap)
	{
		var.second->Stop();
	}
}
