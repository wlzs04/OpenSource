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
	currentD2DBrush = d2dBlackBrush.Get();

	d2dRenderTarget->CreateSolidColorBrush(D2D1::ColorF(
		D2D1::ColorF(0,0,0,0.5)), &d2dModalBrush);

	CoInitialize(NULL);
	CoCreateInstance(CLSID_WICImagingFactory, nullptr, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&wicImageFactory));

	DWriteCreateFactory(DWRITE_FACTORY_TYPE_SHARED, __uuidof(IDWriteFactory), (IUnknown**)&writeFactory);
	AddTextFormat(L"", L"宋体",30);
}

void Direct2DApi::DrawRect(bool fill, float x, float y, float width, float height)
{
	if (fill)
	{
		d2dRenderTarget->FillRectangle(D2D1::RectF(x, y, x + width, y + height), currentD2DBrush);
	}
	else
	{
		d2dRenderTarget->DrawRectangle(D2D1::RectF(x, y, x + width, y + height), currentD2DBrush);
	}
}

void Direct2DApi::DrawEllipse(bool fill, float x, float y, float radiusX, float radiusY)
{
	D2D1_ELLIPSE ellipse;
	ellipse.point.x = x;
	ellipse.point.y = y;
	ellipse.radiusX = radiusX;
	ellipse.radiusY = radiusY;
	if (fill)
	{
		d2dRenderTarget->FillEllipse(ellipse, currentD2DBrush);
	}
	else
	{
		d2dRenderTarget->DrawEllipse(ellipse, currentD2DBrush);
	}
}

void Direct2DApi::DrawLine(float x1, float y1, float x2, float y2,float width)
{
	d2dRenderTarget->DrawLine(D2D1_POINT_2F{ x1,y1 }, D2D1_POINT_2F{ x2, y2 }, currentD2DBrush, width);
}

void Direct2DApi::DrawPolygon(void* polygon, bool fill, float x, float y, float width, float height)
{
	d2dRenderTarget->FillGeometry((ID2D1Geometry*)polygon, currentD2DBrush);
}

void* Direct2DApi::CreatePolygon()
{
	ID2D1PathGeometry* polygon;
	d2dFactory->CreatePathGeometry(&polygon);
	return polygon;
}

void Direct2DApi::DrawImage(std::wstring image, float x, float y, float width, float height)
{
	d2dRenderTarget->DrawBitmap(imageMap[image].Get(), D2D1::RectF(x, y, x + width, y + height));
}

void Direct2DApi::DrawImagePart(wstring image, float x, float y, float width, float height, float xO, float yO, float widthO, float heightO)
{
	float imageWidth = imageMap[image].Get()->GetSize().width;
	float imageHeight = imageMap[image].Get()->GetSize().height;
	d2dRenderTarget->DrawBitmap(imageMap[image].Get(), D2D1::RectF(x, y, x + width, y + height), 1, D2D1_BITMAP_INTERPOLATION_MODE_LINEAR, D2D1::RectF(xO*imageWidth, yO*imageHeight, (xO + widthO)*imageWidth, (yO + heightO)* imageHeight));
}

void Direct2DApi::DrawText(wstring text, float x, float y, float width, float height, wstring  textFormatName)
{
	d2dRenderTarget->DrawTextW(text.c_str(),text.size(), textFormatMap[textFormatName].Get(), D2D1::RectF(x, y, x + width, y + height), currentD2DBrush);
}

void Direct2DApi::AddImage(wstring image)
{
	if (image==L""||imageMap.count(image) > 0)
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
	writeTextFormat->SetTextAlignment(DWRITE_TEXT_ALIGNMENT_CENTER);
	writeTextFormat->SetParagraphAlignment(DWRITE_PARAGRAPH_ALIGNMENT_CENTER);
	textFormatMap[textFormatName] = writeTextFormat;
}

void* Direct2DApi::CreateColorBrush(float r, float g, float b, float a)
{
	ID2D1SolidColorBrush* colorBrush=nullptr;
	d2dRenderTarget->CreateSolidColorBrush(D2D1::ColorF(r,g,b,a), &colorBrush);
	return colorBrush;
}

void* Direct2DApi::CreateColorBrush(wstring colorValue)
{
	return nullptr;
}

void Direct2DApi::SetCurrentBrush(void * colorBrush)
{
	currentD2DBrush = (ID2D1SolidColorBrush*)colorBrush;
}

void Direct2DApi::ResetDefaultBrush()
{
	currentD2DBrush = d2dBlackBrush.Get();
}

void Direct2DApi::SetModalCurrentBrush()
{
	currentD2DBrush = d2dModalBrush.Get();
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
