#pragma once
#include <xaudio2.h>
#include <wrl.h>
#include <string>

#pragma comment(lib,"Xaudio2.lib")

#define fourccRIFF 'FFIR'
#define fourccDATA 'atad'
#define fourccFMT ' tmf'
#define fourccWAVE 'EVAW'
#define fourccXWMA 'AMWX'
#define fourccDPDS 'sdpd'

using Microsoft::WRL::ComPtr;
using namespace std;

class Audio
{
public:
	Audio(wstring audioName);
	~Audio();
	//开始播放
	void Start();
	//暂停
	void Pause();
	//停止
	void Stop();
	//设置生意大小
	void SetVolume(float volume);
	//设置时候循环播放
	void SetIsLoop(bool isloop);

	wstring audioName;

private:
	void AddSoundFromFile(wstring soundName);
	HRESULT FindChunk(HANDLE hFile, DWORD fourcc, DWORD& dwChunkSize, DWORD& dwChunkDataPosition);
	HRESULT ReadChunkData(HANDLE hFile, void * buffer, DWORD buffersize, DWORD bufferoffset);

	ComPtr<IXAudio2> pXAudio2;
	IXAudio2SourceVoice* soundVoice;
	XAUDIO2_BUFFER* audioBuffer;
	bool isPlaying = false;
	bool isLoop = true;
};