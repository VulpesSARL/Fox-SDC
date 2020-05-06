#pragma unmanaged
#include <stdio.h>
#include <Windows.h>
#include <WtsApi32.h>

void FinalCapture(int *x, int *y, void **data, bool *failed, int *failedstep, int *datasz, int *curx, int *cury, int *GL, int ScreenNumber, int *ScreenTop, int *ScreenLeft);

struct CaptureData
{
	int MonitorNumber;
	int MonitorCount;
	HDC MonitorDC;
	RECT MonitorRect;
	void **data;
	int *datasz;
	int *GL;
	bool *failed;
	int *failedstep;
	int ResX;
	int ResY;
	int *x;
	int *y;
	int *ScreenTop;
	int *ScreenLeft;
};

BOOL MonitorenumprocCount(HMONITOR Arg1, HDC Arg2, LPRECT Arg3, LPARAM Arg4)
{
	CaptureData *C = (CaptureData*)(void*)Arg4;
	C->MonitorCount++;
	return(TRUE);
}

void CreateScreenshot(int *x, int *y, void **data, bool *failed, int *failedstep, int *datasz, int *curx, int *cury, int *GL, int ScreenNumber, int *ScreenTop, int *ScreenLeft)
{
	*failed = false;
	*failedstep = 0;

	HWINSTA oldWndSta = GetProcessWindowStation();
	if (oldWndSta != NULL)
	{
		HDESK oldDesktop = GetThreadDesktop(GetCurrentThreadId());
		if (oldDesktop != NULL)
		{
			HWINSTA WndSta = OpenWindowStation(L"WinSta0", false, MAXIMUM_ALLOWED);

			if (WndSta != NULL)
			{
				SetProcessWindowStation(WndSta);

				HDESK Desk = OpenInputDesktop(DF_ALLOWOTHERACCOUNTHOOK, 1, DESKTOP_CREATEMENU | DESKTOP_CREATEWINDOW |
					DESKTOP_ENUMERATE | DESKTOP_HOOKCONTROL |
					DESKTOP_WRITEOBJECTS | DESKTOP_READOBJECTS |
					DESKTOP_SWITCHDESKTOP | GENERIC_WRITE);

				if (Desk != NULL)
				{
					SetThreadDesktop(Desk);

					CaptureData C;
					memset(&C, 0, sizeof(C));

					EnumDisplayMonitors(NULL, NULL, (MONITORENUMPROC)MonitorenumprocCount, (LPARAM)&C);

					if (ScreenNumber >= C.MonitorCount)
						ScreenNumber = 0;

					FinalCapture(x, y, data, failed, failedstep, datasz, curx, cury, GL, ScreenNumber, ScreenTop, ScreenLeft);
					SetThreadDesktop(oldDesktop);
					CloseDesktop(Desk);
				}
				else
				{
					*failed = true;
					*failedstep = 5;
					*GL = GetLastError();
				}

				SetProcessWindowStation(oldWndSta);
				CloseWindowStation(WndSta);
			}
			else
			{
				*failed = true;
				*failedstep = 4;
				*GL = GetLastError();
			}
		}
		else
		{
			*failed = true;
			*failedstep = 2;
			*GL = GetLastError();
		}
	}
	else
	{
		*failed = true;
		*failedstep = 1;
		*GL = GetLastError();
	}
}

BOOL Monitorenumproc(HMONITOR Arg1, HDC Arg2, LPRECT Arg3, LPARAM Arg4)
{
	CaptureData *C = (CaptureData*)(void*)Arg4;

	if (C->MonitorNumber == C->MonitorCount)
	{
		C->MonitorDC = Arg2;
		C->MonitorRect.bottom = Arg3->bottom;
		C->MonitorRect.left = Arg3->left;
		C->MonitorRect.right = Arg3->right;
		C->MonitorRect.top = Arg3->top;
		if (C->MonitorRect.right > C->MonitorRect.left)
		{
			C->ResX = C->MonitorRect.right - C->MonitorRect.left;
			C->ResY = C->MonitorRect.bottom - C->MonitorRect.top;
		}
		else
		{
			C->ResX = C->MonitorRect.left - C->MonitorRect.right;
			C->ResY = C->MonitorRect.top - C->MonitorRect.bottom;
		}
		*C->ScreenTop = C->MonitorRect.top;
		*C->ScreenLeft = C->MonitorRect.left;

		HDC MemDesktop = CreateCompatibleDC(C->MonitorDC);
		if (MemDesktop != NULL)
		{
			HBITMAP tmpbitmap = CreateCompatibleBitmap(C->MonitorDC, C->ResX, C->ResY);
			if (tmpbitmap != NULL)
			{
				HBITMAP bitmap = (HBITMAP)SelectObject(MemDesktop, tmpbitmap);
				if (bitmap != NULL)
				{
					BitBlt(MemDesktop, 0, 0, C->ResX, C->ResY, C->MonitorDC, C->MonitorRect.left, C->MonitorRect.top, CAPTUREBLT | SRCCOPY);

					BITMAPINFOHEADER bi;

					bi.biSize = sizeof(BITMAPINFOHEADER);
					bi.biWidth = C->ResX;
					bi.biHeight = C->ResY;
					bi.biPlanes = 1;
					bi.biBitCount = 32;
					bi.biCompression = BI_RGB;
					bi.biSizeImage = 0;
					bi.biXPelsPerMeter = 0;
					bi.biYPelsPerMeter = 0;
					bi.biClrUsed = 0;
					bi.biClrImportant = 0;

					DWORD dwBmpSize = ((C->ResX * bi.biBitCount + 31) / 32) * 4 * C->ResY;

					*C->datasz = dwBmpSize;
					*C->data = malloc(dwBmpSize);
					if (*C->data != NULL)
					{
						HANDLE hDIB = GlobalAlloc(GHND, dwBmpSize);

						char *lpbitmap = (char*)GlobalLock(hDIB);

						GetDIBits(C->MonitorDC, tmpbitmap, 0, C->ResY, lpbitmap, (BITMAPINFO *)&bi, DIB_RGB_COLORS);

						memcpy(*C->data, lpbitmap, dwBmpSize);

						GlobalFree(hDIB);

						*C->x = C->ResX;
						*C->y = C->ResY;

						*C->failed = false;
						*C->failedstep = 0;
						*C->GL = 0;
					}

					DeleteObject(bitmap);
				}
				else
				{
					*C->failed = true;
					*C->failedstep = 23;
					*C->GL = GetLastError();
				}
				DeleteObject(tmpbitmap);
			}
			else
			{
				*C->failed = true;
				*C->failedstep = 22;
				*C->GL = GetLastError();
			}
			DeleteDC(MemDesktop);
		}
		else
		{
			*C->failed = true;
			*C->failedstep = 21;
			*C->GL = GetLastError();
		}
		DeleteDC(C->MonitorDC);

		return(FALSE);
	}
	else
	{
		C->MonitorCount++;
		return(TRUE);
	}
}


void FinalCapture(int *x, int *y, void **data, bool *failed, int *failedstep, int *datasz, int *curx, int *cury, int *GL, int ScreenNumber, int *ScreenTop, int *ScreenLeft)
{
	CaptureData C;
	memset(&C, 0, sizeof(C));
	C.MonitorNumber = ScreenNumber;
	C.data = data;
	C.datasz = datasz;
	C.failed = failed;
	C.failedstep = failedstep;
	C.GL = GL;
	C.x = x;
	C.y = y;
	C.ScreenLeft = ScreenLeft;
	C.ScreenTop = ScreenTop;

	*C.failed = true;
	*C.failedstep = 3000;
	*C.GL = 0;

	HDC Desktop = GetDC(GetDesktopWindow());
	if (Desktop != NULL)
	{
		if (EnumDisplayMonitors(GetDC(NULL), NULL, (MONITORENUMPROC)Monitorenumproc, (LPARAM)&C) != 0)
		{
			*failed = true;
			*failedstep = 30;
			return;
		}

		POINT CursorPos;
		CursorPos.x = 0;
		CursorPos.y = 0;
		GetCursorPos(&CursorPos);

		*curx = CursorPos.x - *C.ScreenLeft;
		*cury = CursorPos.y - *C.ScreenTop;
	}
	else
	{
		*failed = true;
		*failedstep = 20;
		*GL = GetLastError();
	}
}
