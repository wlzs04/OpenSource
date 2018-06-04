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
	//��ʼ����
	void Start();
	//��ͣ
	void Pause();
	//ֹͣ
	void Stop();
	//���������С
	void SetVolume(float volume);
	//����ʱ��ѭ������
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