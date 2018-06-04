#include "Audio.h"
#include "..\..\Common\Helper\SystemHelper.h"

Audio::Audio(wstring audioName)
{
	XAudio2Create(&pXAudio2, 0, XAUDIO2_DEFAULT_PROCESSOR);
	IXAudio2MasteringVoice* pMasterVoice;
	pXAudio2->CreateMasteringVoice(&pMasterVoice);

	this->audioName = audioName;

	AddSoundFromFile(audioName);
}

void Audio::AddSoundFromFile(wstring soundName)
{
	HANDLE hFile = CreateFile((SystemHelper::GetCurrentPath() + L"\\Resource\\" + soundName.c_str()).c_str(), GENERIC_READ, FILE_SHARE_READ, nullptr, OPEN_EXISTING, 0, nullptr);

	SetFilePointer(hFile, 0, nullptr, FILE_BEGIN);

	DWORD dwChunkSize;
	DWORD dwChunkPosition;

	FindChunk(hFile, fourccRIFF, dwChunkSize, dwChunkPosition);
	DWORD filetype;
	ReadChunkData(hFile, &filetype, sizeof(DWORD), dwChunkPosition);

	FindChunk(hFile, fourccFMT, dwChunkSize, dwChunkPosition);
	WAVEFORMATEXTENSIBLE wfx = { 0 };
	ReadChunkData(hFile, &wfx, dwChunkSize, dwChunkPosition);

	FindChunk(hFile, fourccDATA, dwChunkSize, dwChunkPosition);
	BYTE * pDataBuffer = new BYTE[dwChunkSize];
	ReadChunkData(hFile, pDataBuffer, dwChunkSize, dwChunkPosition);

	audioBuffer = new XAUDIO2_BUFFER();

	audioBuffer->AudioBytes = dwChunkSize;
	audioBuffer->pAudioData = pDataBuffer;
	audioBuffer->Flags = XAUDIO2_END_OF_STREAM;
	audioBuffer->LoopCount = XAUDIO2_LOOP_INFINITE;

	pXAudio2->CreateSourceVoice(&soundVoice, (WAVEFORMATEX*)&wfx);

	soundVoice->SubmitSourceBuffer(audioBuffer);
}

Audio::~Audio()
{
	soundVoice->DestroyVoice();
	delete audioBuffer->pAudioData;
	delete audioBuffer;
}

void Audio::Start()
{
	isPlaying = true;
	soundVoice->Start();
}

void Audio::Pause()
{
	isPlaying = false;
	soundVoice->Stop();
}

void Audio::Stop()
{
	isPlaying = false;
	soundVoice->Stop();
	soundVoice->FlushSourceBuffers();
	soundVoice->SubmitSourceBuffer(audioBuffer);
}

void Audio::SetVolume(float volume)
{
	soundVoice->SetVolume(volume);
}

void Audio::SetIsLoop(bool isloop)
{
	this->isLoop = isloop;
	audioBuffer->LoopCount = isLoop ? XAUDIO2_LOOP_INFINITE : 0;
	soundVoice->FlushSourceBuffers();
	soundVoice->SubmitSourceBuffer(audioBuffer);
}

HRESULT Audio::FindChunk(HANDLE hFile, DWORD fourcc, DWORD& dwChunkSize, DWORD& dwChunkDataPosition)
{
	HRESULT hr = S_OK;
	if (INVALID_SET_FILE_POINTER == SetFilePointer(hFile, 0, nullptr, FILE_BEGIN))
		return HRESULT_FROM_WIN32(GetLastError());

	DWORD dwChunkType;
	DWORD dwChunkDataSize;
	DWORD dwRIFFDataSize = 0;
	DWORD dwFileType;
	DWORD bytesRead = 0;
	DWORD dwOffset = 0;

	while (hr == S_OK)
	{
		DWORD dwRead;
		if (0 == ReadFile(hFile, &dwChunkType, sizeof(DWORD), &dwRead, nullptr))
			hr = HRESULT_FROM_WIN32(GetLastError());

		if (0 == ReadFile(hFile, &dwChunkDataSize, sizeof(DWORD), &dwRead, nullptr))
			hr = HRESULT_FROM_WIN32(GetLastError());

		switch (dwChunkType)
		{
		case fourccRIFF:
			dwRIFFDataSize = dwChunkDataSize;
			dwChunkDataSize = 4;
			if (0 == ReadFile(hFile, &dwFileType, sizeof(DWORD), &dwRead, nullptr))
				hr = HRESULT_FROM_WIN32(GetLastError());
			break;

		default:
			if (INVALID_SET_FILE_POINTER == SetFilePointer(hFile, dwChunkDataSize, nullptr, FILE_CURRENT))
				return HRESULT_FROM_WIN32(GetLastError());
		}

		dwOffset += sizeof(DWORD) * 2;

		if (dwChunkType == fourcc)
		{
			dwChunkSize = dwChunkDataSize;
			dwChunkDataPosition = dwOffset;
			return S_OK;
		}

		dwOffset += dwChunkDataSize;

		if (bytesRead >= dwRIFFDataSize) return S_FALSE;

	}

	return S_OK;
}

HRESULT Audio::ReadChunkData(HANDLE hFile, void * buffer, DWORD buffersize, DWORD bufferoffset)
{
	HRESULT hr = S_OK;
	if (INVALID_SET_FILE_POINTER == SetFilePointer(hFile, bufferoffset, nullptr, FILE_BEGIN))
		return HRESULT_FROM_WIN32(GetLastError());
	DWORD dwRead;
	if (0 == ReadFile(hFile, buffer, buffersize, &dwRead, nullptr))
		hr = HRESULT_FROM_WIN32(GetLastError());
	return hr;
}
