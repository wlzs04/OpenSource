#include "Direct2DApi.h"
#include "..\..\Helper\SystemHelper.h"

using namespace D2D1;
using namespace Microsoft::WRL;

Direct2DApi::Direct2DApi(HWND hWnd):GraphicsApi(hWnd)
{
	
}

Direct2DApi::~Direct2DApi()
{
	CoUninitialize();
}

void Direct2DApi::Init()
{
	D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, d2dFactory.GetAddressOf());

	d2dFactory->CreateHwndRenderTarget(RenderTargetProperties(),
		HwndRenderTargetProperties(hWnd,SizeU(width, height)),
		&d2dRenderTarget);

	d2dRenderTarget->CreateSolidColorBrush(D2D1::ColorF(
		D2D1::ColorF::Black),&d2dBlackBrush);
	d2dCurrentBrush = d2dBlackBrush.Get();

	CoInitialize(NULL);
	CoCreateInstance(CLSID_WICImagingFactory, nullptr, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&wicImageFactory));

	DWriteCreateFactory(DWRITE_FACTORY_TYPE_SHARED, __uuidof(IDWriteFactory), (IUnknown**)&writeFactory);
}

void Direct2DApi::DrawRect(float x, float y, float width, float height)
{
	d2dRenderTarget->DrawRectangle(D2D1::RectF(x, y, x+ width, y+height), d2dCurrentBrush);
}

void Direct2DApi::DrawImage(std::wstring image, float x, float y, float width, float height)
{
	d2dRenderTarget->DrawBitmap(imageMap[image].Get(), D2D1::RectF(x, y, x + width, y + height));
}

void Direct2DApi::DrawText(wstring text,wstring  textFormatName,float x, float y,float width, float height)
{
	d2dRenderTarget->DrawTextW(text.c_str(),text.size(), textFormatMap[textFormatName].Get(), D2D1::RectF(x, y, x + width, y + height),d2dCurrentBrush);
}

void Direct2DApi::AddImage(wstring image)
{
	if (imageMap.count(image) > 0)
	{
		return;
	}

	ComPtr<IWICBitmapDecoder> decoder;
	ComPtr<IWICBitmapFrameDecode> source;
	ComPtr<IWICFormatConverter> converter;
	ComPtr<IWICBitmap> wicBitmap;
	ComPtr<ID2D1Bitmap> d2dBitmap;
	wicImageFactory->CreateDecoderFromFilename((SystemHelper::GetCurrentPath()+L"\\Resource\\"+ image.c_str()).c_str(), NULL, GENERIC_READ, WICDecodeMetadataCacheOnLoad, &decoder);

	decoder->GetFrame(0, &source);
	wicImageFactory->CreateFormatConverter(&converter);
	converter->Initialize(source.Get(), GUID_WICPixelFormat32bppPBGRA, WICBitmapDitherTypeNone, nullptr, 0, WICBitmapPaletteTypeMedianCut);
	wicImageFactory->CreateBitmapFromSource(converter.Get(), WICBitmapCacheOnDemand, &wicBitmap);
	d2dRenderTarget->CreateBitmapFromWicBitmap(wicBitmap.Get(), &d2dBitmap);
	imageMap[image] = d2dBitmap;
}

void Direct2DApi::AddTextFormat(wstring textFormatName,wstring fontFamilyName,float fontSize)
{
	ComPtr<IDWriteTextFormat> writeTextFormat;
	writeFactory->CreateTextFormat(fontFamilyName.c_str(), NULL, DWRITE_FONT_WEIGHT_NORMAL, DWRITE_FONT_STYLE_NORMAL, DWRITE_FONT_STRETCH_NORMAL, fontSize, L"", &writeTextFormat);
	textFormatMap[textFormatName] = writeTextFormat;
}

void Direct2DApi::Clear()
{
	d2dRenderTarget->Clear(D2D1::ColorF(D2D1::ColorF::White));
}

void Direct2DApi::BeginRender()
{
	d2dRenderTarget->BeginDraw();
}

void Direct2DApi::EndRender()
{
	d2dRenderTarget->EndDraw();
}
