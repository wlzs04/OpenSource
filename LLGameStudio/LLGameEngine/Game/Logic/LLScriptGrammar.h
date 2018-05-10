#pragma once
#include "..\..\Common\Helper\WStringHelper.h"

//脚本语言语法：
class LLScriptGrammar
{
public:
	//是否是可忽略字符
	static bool WCharCanIgnore(wchar_t wc);
	//脚本语言语法：是否为特殊符号，用来截断读取的标记
	static bool WCharIsSpecial(wchar_t wc);
	//脚本语言语法：是否为运算符号
	static bool WCharIsOperator(wchar_t wc);
	//脚本语言语法：是否为关键字
	static bool WStringIsKeyWord(wstring ws);
};