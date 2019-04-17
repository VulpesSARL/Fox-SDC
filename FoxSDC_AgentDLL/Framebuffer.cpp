#pragma unmanaged
#include <stdio.h>
#include <Windows.h>
#include <WtsApi32.h>

void FinalCapture(int *x, int *y, void **data, bool *failed, int *failedstep, int *datasz, int *curx, int *cury, int *GL);


void CreateScreenshot(int *x, int *y, void **data, bool *failed, int *failedstep, int *datasz, int *curx, int *cury, int *GL)
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

				HDESK Desk = OpenInputDesktop(0, DF_ALLOWOTHERACCOUNTHOOK, DESKTOP_CREATEMENU | DESKTOP_CREATEWINDOW |
					DESKTOP_ENUMERATE | DESKTOP_HOOKCONTROL |
					DESKTOP_WRITEOBJECTS | DESKTOP_READOBJECTS |
					DESKTOP_SWITCHDESKTOP | GENERIC_WRITE);

				if (Desk != NULL)
				{
					SetThreadDesktop(Desk);

					FinalCapture(x, y, data, failed, failedstep, datasz, curx, cury, GL);

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

void FinalCapture(int *x, int *y, void **data, bool *failed, int *failedstep, int *datasz, int *curx, int *cury, int *GL)
{
	HDC Desktop = GetDC(NULL);
	if (Desktop != NULL)
	{
		POINT CursorPos;
		CursorPos.x = 0;
		CursorPos.y = 0;
		GetCursorPos(&CursorPos);

		*curx = CursorPos.x;
		*cury = CursorPos.y;

		int ResX = GetSystemMetrics(SM_CXVIRTUALSCREEN);
		int ResY = GetSystemMetrics(SM_CYVIRTUALSCREEN);

		HDC MemDesktop = CreateCompatibleDC(Desktop);
		if (MemDesktop != NULL)
		{
			HBITMAP tmpbitmap = CreateCompatibleBitmap(Desktop, ResX, ResY);
			if (tmpbitmap != NULL)
			{
				HBITMAP bitmap = (HBITMAP)SelectObject(MemDesktop, tmpbitmap);
				if (bitmap != NULL)
				{
					BitBlt(MemDesktop, 0, 0, ResX, ResY, Desktop, 0, 0, SRCCOPY);

					BITMAPINFOHEADER bi;

					bi.biSize = sizeof(BITMAPINFOHEADER);
					bi.biWidth = ResX;
					bi.biHeight = ResY;
					bi.biPlanes = 1;
					bi.biBitCount = 32;
					bi.biCompression = BI_RGB;
					bi.biSizeImage = 0;
					bi.biXPelsPerMeter = 0;
					bi.biYPelsPerMeter = 0;
					bi.biClrUsed = 0;
					bi.biClrImportant = 0;

					DWORD dwBmpSize = ((ResX * bi.biBitCount + 31) / 32) * 4 * ResY;

					*datasz = dwBmpSize;
					*data = malloc(dwBmpSize);
					if (*data != NULL)
					{
						HANDLE hDIB = GlobalAlloc(GHND, dwBmpSize);

						char *lpbitmap = (char*)GlobalLock(hDIB);

						GetDIBits(Desktop, tmpbitmap, 0, ResY, lpbitmap, (BITMAPINFO *)&bi, DIB_RGB_COLORS);

						memcpy(*data, lpbitmap, dwBmpSize);

						GlobalFree(hDIB);

						*x = ResX;
						*y = ResY;
					}

					DeleteObject(bitmap);
				}
				else
				{
					*failed = true;
					*failedstep = 23;
					*GL = GetLastError();
				}
				DeleteObject(tmpbitmap);
			}
			else
			{
				*failed = true;
				*failedstep = 22;
				*GL = GetLastError();
			}
			DeleteDC(MemDesktop);
		}
		else
		{
			*failed = true;
			*failedstep = 21;
			*GL = GetLastError();
		}
		DeleteDC(Desktop);
	}
	else
	{
		*failed = true;
		*failedstep = 20;
		*GL = GetLastError();
	}
}
