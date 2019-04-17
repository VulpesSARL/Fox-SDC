#pragma unmanaged
#include <Windows.h>

HINSTANCE GlobalhInstDLL;

extern void InitDLLWin8Plus();


BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	if (fdwReason == DLL_PROCESS_ATTACH)
	{
		InitDLLWin8Plus();
		GlobalhInstDLL = hinstDLL;
		return(TRUE);
	}
}
