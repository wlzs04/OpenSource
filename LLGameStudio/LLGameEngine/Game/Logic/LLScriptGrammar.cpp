#include "LLScriptGrammar.h"

bool LLScriptGrammar::WCharCanIgnore(wchar_t wc)
{
	return wc == L' '//半角空格 
		|| wc == L'　' //全角空格（输入法快捷键Shift+Space可以切换半角和全角）
		|| wc == L'\n' //换行
		|| wc == L'\r'//回车（“\r”和“\r\n”编码一样）
		|| wc == L'\t'//水平制表符
		;
}

bool LLScriptGrammar::WCharIsSpecial(wchar_t wc)
{
	return LLScriptGrammar::WCharCanIgnore(wc)
		|| wc == L'.' //点
		|| wc == L',' //逗点
		|| wc == L';' //分号
		|| wc == L'=' //等号
		|| wc == L'/' //注释半个开始标记
		|| wc == L'(' //左括号
		|| wc == L')' //右括号
		|| wc == L'{' //左大括号
		|| wc == L'}' //右大括号
		|| LLScriptGrammar::WCharIsOperator(wc)
		;
}

bool LLScriptGrammar::WCharIsOperator(wchar_t wc)
{
	return wc == L'+'//加
		|| wc == L'-' //减
		|| wc == L'*' //乘
		|| wc == L'/' //除
		|| wc == L'>' //大于
		|| wc == L'<' //小于
		|| wc == L'#' //等于
		|| wc == L'!' //不等于
		|| wc == L'%' //取余
		|| wc == L'&' //交
		|| wc == L'|' //并
		;
}

bool LLScriptGrammar::WStringIsKeyWord(wstring ws)
{
	return ws == L"class"//定义类
		|| ws == L"true"//真
		|| ws == L"false"//假
		|| ws == L"return"//返回
		|| ws == L"if"//如果
		|| ws == L"else"//或者
		|| ws == L"while"//循环
		|| ws == L"for"//循环
		|| ws == L"break"//结束循环
		|| ws == L"continue"//结束当前循环，开始下一次循环
		|| ws == L"null"//空
		;
}
