#include <intrin.h>
#include <msclr\marshal_cppstd.h>
#include <Wtsapi32.h>
#using <System.dll>
#using <FoxSDC_Common.dll>

using namespace System;
using namespace System::Text;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace System::Security::Cryptography::X509Certificates;
using namespace msclr::interop;
using namespace System::IO;
using namespace FoxSDC_Common;

#pragma unmanaged

extern BOOL GetFirmwareType_CXX(PFIRMWARE_TYPE firmwaretype);
extern VOID SendSAS(BOOL AsUser);

bool LauchApp(WCHAR* FILE, WCHAR* ARGS);
bool LauchApp(WCHAR* FILE, WCHAR* ARGS, DWORD SessionID);
bool LauchAppWait(WCHAR* FILE, WCHAR* ARGS, DWORD SessionID);
bool LauchAppIntoWinLogon(WCHAR* FILE, WCHAR* ARGS);
bool CSetToken(LPWSTR NAME);
BOOL VerifyEmbeddedSignature(LPCWSTR pwszSourceFile);
void CreateScreenshot(int *x, int *y, void **data, bool *failed, int *failedstep, int *datasz, int *curx, int *cury, int *GL);
void DbgPrintf(char *format, ...);

void WriteEventLogVerbose(WCHAR* Blahblah)
{
	HANDLE log = RegisterEventSource(NULL, L"Fox SDC Agent");
	if (log == 0)
		return;
	LPWSTR strlist[1] = { Blahblah };

	ReportEvent(log, EVENTLOG_SUCCESS, 0, 32767, NULL, 1, 0, (LPCWSTR*)strlist, NULL);

	DeregisterEventSource(log);
}

bool UMIsInHypervisor()
{
	unsigned int cpuInfo[4];
	__cpuid((int*)cpuInfo, 1);
	return(((cpuInfo[2] >> 31) & 1) == 1);
}

void DbgPrintf(char *format, ...)
{
	char DbgText[1024];
	va_list l;

	va_start(l, format);
	vsprintf_s(DbgText, sizeof(DbgText) / 2, format, l);
	va_end(l);
	OutputDebugStringA(DbgText);
}

#pragma managed

namespace Fox
{
	public ref class FoxCWrapper : CPPInterface
	{
	public:
		virtual Boolean IsInHypervisor()
		{
			return(UMIsInHypervisor());
		}

		virtual Boolean StartAppAsUser(String^ Filename, String^ Args)
		{
			Filename = Filename->Trim();
			if (Args == nullptr)
				Args = "";

			if (Filename->StartsWith("\"") == true && Filename->EndsWith("\"") == true)
				Args = Filename + (Args == "" ? "" : " " + Args);
			else
				Args = "\"" + Filename + "\"" + (Args == "" ? "" : " " + Args);

			WCHAR* fn = (WCHAR*)(void*)Marshal::StringToHGlobalUni(Filename);
			WCHAR* ag = (WCHAR*)(void*)Marshal::StringToHGlobalUni(Args);

			bool res = LauchApp(fn, ag);

			Marshal::FreeHGlobal((IntPtr)fn);
			Marshal::FreeHGlobal((IntPtr)ag);

			return(res);
		}

		virtual Boolean StartAppInWinLogon(String^ Filename, String^ Args)
		{
			Filename = Filename->Trim();
			if (Args == nullptr)
				Args = "";

			if (Filename->StartsWith("\"") == true && Filename->EndsWith("\"") == true)
				Args = Filename + (Args == "" ? "" : " " + Args);
			else
				Args = "\"" + Filename + "\"" + (Args == "" ? "" : " " + Args);

			WCHAR* fn = (WCHAR*)(void*)Marshal::StringToHGlobalUni(Filename);
			WCHAR* ag = (WCHAR*)(void*)Marshal::StringToHGlobalUni(Args);

			bool res = LauchAppIntoWinLogon(fn, ag);

			Marshal::FreeHGlobal((IntPtr)fn);
			Marshal::FreeHGlobal((IntPtr)ag);

			return(res);
		}

		virtual Boolean StartAppAsUser(String^ Filename, String^ Args, int SessionID)
		{
			Filename = Filename->Trim();
			if (Args == nullptr)
				Args = "";

			if (Filename->StartsWith("\"") == true && Filename->EndsWith("\"") == true)
				Args = Filename + (Args == "" ? "" : " " + Args);
			else
				Args = "\"" + Filename + "\"" + (Args == "" ? "" : " " + Args);

			WCHAR* fn = (WCHAR*)(void*)Marshal::StringToHGlobalUni(Filename);
			WCHAR* ag = (WCHAR*)(void*)Marshal::StringToHGlobalUni(Args);

			bool res = LauchApp(fn, ag, SessionID);

			Marshal::FreeHGlobal((IntPtr)fn);
			Marshal::FreeHGlobal((IntPtr)ag);

			return(res);
		}

		virtual Boolean StartAppAsUserWait(String^ Filename, String^ Args, int SessionID)
		{
			Filename = Filename->Trim();
			if (Args == nullptr)
				Args = "";

			if (Filename->StartsWith("\"") == true && Filename->EndsWith("\"") == true)
				Args = Filename + (Args == "" ? "" : " " + Args);
			else
				Args = "\"" + Filename + "\"" + (Args == "" ? "" : " " + Args);

			WCHAR* fn = (WCHAR*)(void*)Marshal::StringToHGlobalUni(Filename);
			WCHAR* ag = (WCHAR*)(void*)Marshal::StringToHGlobalUni(Args);

			bool res = LauchAppWait(fn, ag, SessionID);

			Marshal::FreeHGlobal((IntPtr)fn);
			Marshal::FreeHGlobal((IntPtr)ag);

			return(res);
		}

		virtual Int64 WGetLastError()
		{
			return (GetLastError());
		}

		virtual bool SetDateTime(System::DateTime UTCTime)
		{
			SYSTEMTIME syst;
			syst.wYear = UTCTime.Year;
			syst.wMonth = UTCTime.Month;
			syst.wDay = UTCTime.Day;
			syst.wHour = UTCTime.Hour;
			syst.wMinute = UTCTime.Minute;
			syst.wSecond = UTCTime.Second;
			syst.wMilliseconds = UTCTime.Millisecond;
			SetSystemTime(&syst);
			return(true);
		}

		virtual Boolean SetToken()
		{
			if (CSetToken(SE_IMPERSONATE_NAME) == false)
				return(false);
			if (CSetToken(SE_SHUTDOWN_NAME) == false)
				return(false);
			if (CSetToken(SE_TCB_NAME) == false)
				return(false);
			if (CSetToken(SE_ASSIGNPRIMARYTOKEN_NAME) == false)
				return(false);
			if (CSetToken(SE_INCREASE_QUOTA_NAME) == false)
				return(false);
			if (CSetToken(SE_DEBUG_NAME) == false)
				return(false);
			return(true);
		}

		virtual Boolean VerifyEXESignature(String^ Filename)
		{
			if (File::Exists(Filename) == false)
				return(false);

			WCHAR* fn = (WCHAR*)(void*)Marshal::StringToHGlobalUni(Filename);
			bool res = VerifyEmbeddedSignature(fn);
			Marshal::FreeHGlobal((IntPtr)fn);

			if (res == true) //perform additional tests
			{
				X509Certificate^ cer = X509Certificate::CreateFromSignedFile(Filename);
				String^ pubkeys = cer->GetPublicKeyString();
				String^ intkeys = Certificates::ExtractMainCERX509()->GetPublicKeyString();
				System::Diagnostics::Debug::WriteLine("EXE: " + pubkeys);
				System::Diagnostics::Debug::WriteLine("INT: " + intkeys);
				if (pubkeys != intkeys)
					return(false);
			}

			return (res);
		}

		virtual String^ FoxGetFirmwareType()
		{
			String^ BIOSType = "Unknown";
			if (IsFirmwareLEGACY() == true)
				BIOSType = "Legacy";
			if (IsFirmwareEFI() == true)
				BIOSType = "EFI";
			return (BIOSType);
		}

		virtual void RestartSystem()
		{
			ExitWindowsEx(EWX_REBOOT, SHTDN_REASON_FLAG_PLANNED);
		}

		virtual void RestartSystemForced()
		{
			ExitWindowsEx(EWX_REBOOT | EWX_FORCEIFHUNG, SHTDN_REASON_FLAG_PLANNED);
		}

		virtual PushRunningSessionList^ GetActiveTSSessions()
		{
			PushRunningSessionList^ r = gcnew PushRunningSessionList();
			r->Data = gcnew List<PushRunningSessionElement^>();

			WTS_SESSION_INFO *sessions;
			DWORD num;

			if (WTSEnumerateSessions(WTS_CURRENT_SERVER_HANDLE, 0, 1, &sessions, &num) == 0)
				return(r);

			for (DWORD i = 0; i < num; i++)
			{
				PushRunningSessionElement^element = gcnew PushRunningSessionElement();
				WTS_SESSION_INFO si = sessions[i];
				element->SessionID = si.SessionId;
				switch (si.State)
				{
				case WTSListen:
				case WTSReset:
				case WTSDown:
				case WTSInit:
				case WTSConnectQuery:
					continue;
				}

				WCHAR *str;
				DWORD strret;
				if (WTSQuerySessionInformation(WTS_CURRENT_SERVER_HANDLE, si.SessionId, WTSUserName, &str, &strret) != 0)
				{
					element->User = gcnew String(str);
					WTSFreeMemory(str);
				}
				else
				{
					element->User = "N/A";
				}

				if (WTSQuerySessionInformation(WTS_CURRENT_SERVER_HANDLE, si.SessionId, WTSDomainName, &str, &strret) != 0)
				{
					element->Domain = gcnew String(str);
					WTSFreeMemory(str);
				}
				else
				{
					element->Domain = "N/A";
				}

				r->Data->Add(element);
			}

			WTSFreeMemory(sessions);

			return(r);
		}

		virtual CPPFrameBufferData^ GetFrameBufferData()
		{
			CPPFrameBufferData^ fbuff = gcnew CPPFrameBufferData();

			int x;
			int y;
			int datasz;
			void *screendata;
			bool failed;
			int failedat;
			int curx;
			int cury;
			int GL;

			CreateScreenshot(&x, &y, &screendata, &failed, &failedat, &datasz, &curx, &cury, &GL);

			fbuff->X = x;
			fbuff->Y = y;
			fbuff->Failed = failed;
			fbuff->FailedAt = failedat;
			fbuff->Win32Error = GL;

			if (failed == false)
			{
				fbuff->Data = gcnew array<Byte>(datasz);
				Marshal::Copy(IntPtr(screendata), fbuff->Data, 0, datasz);
				free(screendata);
			}

			fbuff->CursorX = curx;
			fbuff->CursorY = cury;

			//DbgPrintf("Curx: %i Cury: %i\n", curx, cury);

			return(fbuff);
		}

		virtual Int64 MoveMouse(int X, int Y, int delta, int flags)
		{
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


							INPUT input;
							memset(&input, 0, sizeof(input));

							input.type = INPUT_MOUSE;

							int ResX = GetSystemMetrics(SM_CXVIRTUALSCREEN);
							int ResY = GetSystemMetrics(SM_CYVIRTUALSCREEN);

							input.mi.dx = (LONG)(((double)65535 / (double)ResX)*(double)X);
							input.mi.dy = (LONG)(((double)65535 / (double)ResY)*(double)Y);
							input.mi.mouseData = delta;

							input.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE | MOUSEEVENTF_VIRTUALDESK;

							input.mi.dwFlags |= TestButtons(flags, (int)MouseDataFlags::LeftButton, MOUSEEVENTF_LEFTDOWN, MOUSEEVENTF_LEFTUP);
							input.mi.dwFlags |= TestButtons(flags, (int)MouseDataFlags::RightButton, MOUSEEVENTF_RIGHTDOWN, MOUSEEVENTF_RIGHTUP);
							input.mi.dwFlags |= TestButtons(flags, (int)MouseDataFlags::MiddleButton, MOUSEEVENTF_MIDDLEDOWN, MOUSEEVENTF_MIDDLEUP);
							input.mi.dwFlags |= TestButtons(flags, (int)MouseDataFlags::XButton1, MOUSEEVENTF_XDOWN, MOUSEEVENTF_XUP);

							SendInput(1, &input, sizeof(input));
						}

						SetProcessWindowStation(oldWndSta);
						CloseWindowStation(WndSta);
					}
				}
			}
			return(0);
		}

		virtual Int64 SetKeyboard(int VirtualKey, int ScanCode, int Flags)
		{
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
							INPUT input;
							memset(&input, 0, sizeof(input));

							input.type = INPUT_KEYBOARD;

							input.ki.wVk = VirtualKey;
							input.ki.wScan = ScanCode;
							input.ki.dwFlags = Flags;

							SendInput(1, &input, sizeof(input));
						}

						SetProcessWindowStation(oldWndSta);
						CloseWindowStation(WndSta);
					}
				}
			}
			return(0);
		}

		virtual Int64 GetConsoleSessionID()
		{
			return (WTSGetActiveConsoleSessionId());
		}

		virtual void SendCTRLALTDELETE()
		{		
			SendSAS(false);
		}

		virtual int SetKeyboardLayout(INT64 ID)
		{
			WCHAR Layout[50];
			swprintf_s(Layout, sizeof(Layout) / 2, L"%08llX", ID);
			HKL layout = LoadKeyboardLayout(Layout, KLF_ACTIVATE | KLF_SUBSTITUTE_OK);
			if (layout == NULL)
				return(1);
			if (ActivateKeyboardLayout(layout, 0) == 0)
				return(2);
			if (SystemParametersInfo(SPI_SETDEFAULTINPUTLANG, 0, &layout, SPIF_SENDCHANGE) == 0)
				return(3);
			HWND hwnd;
			hwnd = GetTopWindow(NULL);
			while (hwnd != NULL)
			{
				PostMessage(hwnd, WM_INPUTLANGCHANGEREQUEST, INPUTLANGCHANGE_SYSCHARSET, (LPARAM)layout);
				hwnd = GetNextWindow(hwnd, GW_HWNDNEXT);
			}
			return(0);
		}
	private:
		DWORD LastMouseEvent = 0;

		virtual int TestButtons(int FoxMouseFlag, int FoxMouseButton, int WindowsFlagSET, int WindowsFlagUNSET) sealed
		{
			int AdditionalFlags = 0;

			if ((FoxMouseFlag&FoxMouseButton) != 0)
			{
				if ((LastMouseEvent&FoxMouseButton) == 0)
				{
					LastMouseEvent |= FoxMouseButton;
					AdditionalFlags |= WindowsFlagSET;
				}
			}
			else
			{
				if ((LastMouseEvent&FoxMouseButton) != 0)
				{
					LastMouseEvent &= ~FoxMouseButton;
					AdditionalFlags |= WindowsFlagUNSET;
				}
			}
			return(AdditionalFlags);
		}

		virtual Boolean IsFirmwareEFI() sealed
		{
			FIRMWARE_TYPE fwtype;
			GetFirmwareType_CXX(&fwtype);
			if (fwtype == FirmwareTypeUefi)
				return(true);
			return(false);
		}

		virtual Boolean IsFirmwareLEGACY() sealed
		{
			FIRMWARE_TYPE fwtype;
			GetFirmwareType_CXX(&fwtype);
			if (fwtype == FirmwareTypeBios)
				return(true);
			return(false);
		}

	};
}
