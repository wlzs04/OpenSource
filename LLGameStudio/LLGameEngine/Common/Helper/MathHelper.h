#pragma once

class MathHelper
{
public:
	//判断参数是否在1和0之间，包括1不包括0。
	static bool IsRange1To0(double d)
	{
		return d > 0 && d <= 1;
	}
};