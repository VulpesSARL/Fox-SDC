#include <intrin.h>
#include <msclr\marshal_cppstd.h>
#include <WinDNS.h>
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
extern BOOL SetFirmwareEnvironmentVariableEx_CXX(LPCWSTR lpName, LPCWSTR lpGuid, PVOID pValue, DWORD nSize, DWORD dwAttributes);

bool LauchApp(WCHAR* FILE, WCHAR* ARGS);
bool LauchApp(WCHAR* FILE, WCHAR* ARGS, DWORD SessionID, DWORD *PID);
bool LauchAppWait(WCHAR* FILE, WCHAR* ARGS, DWORD SessionID);
bool LauchAppIntoWinLogon(WCHAR* FILE, WCHAR* ARGS, int *ProcessID, const WCHAR *ProcessName);
bool CSetToken(LPWSTR NAME);
BOOL VerifyEmbeddedSignature(LPCWSTR pwszSourceFile);
void CreateScreenshot(int *x, int *y, void **data, bool *failed, int *failedstep, int *datasz, int *curx, int *cury, int *GL, int ScreenNumber, int *ScreenTop, int *ScreenLeft);
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

#define EFI_SPACE_GUID L"{8BE4DF61-93CA-11D2-AA0D-00E098032B8C}"

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

		virtual Boolean StartAppInWinLogon(String^ Filename, String^ Args, [Runtime::InteropServices::Out] int% ProcessId)
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

			int PID;

			bool res = LauchAppIntoWinLogon(fn, ag, &PID, L"winlogon.exe");
			ProcessId = PID;

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

			bool res = LauchApp(fn, ag, SessionID, NULL);

			Marshal::FreeHGlobal((IntPtr)fn);
			Marshal::FreeHGlobal((IntPtr)ag);

			return(res);
		}

		virtual int StartAppAsUserID(String^ Filename, String^ Args, int SessionID)
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

			DWORD d;

			bool res = LauchApp(fn, ag, SessionID, &d);

			Marshal::FreeHGlobal((IntPtr)fn);
			Marshal::FreeHGlobal((IntPtr)ag);

			if (res == false)
				return (-1);

			return(d);
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
			if (CSetToken(SE_SYSTEM_ENVIRONMENT_NAME) == false)
				return(false);
			if (CSetToken(SE_TIME_ZONE_NAME) == false)
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

		virtual void SetScreenNumber(int ScreenNumber)
		{
			this->ScreenNumber = ScreenNumber;
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

			int ScreenT;
			int ScreenL;

			CreateScreenshot(&x, &y, &screendata, &failed, &failedat, &datasz,
				&curx, &cury, &GL, ScreenNumber, &ScreenT, &ScreenL);

			ScreenTop = ScreenT + (0 - GetSystemMetrics(SM_YVIRTUALSCREEN));
			ScreenLeft = ScreenL + (0 - GetSystemMetrics(SM_XVIRTUALSCREEN));

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

							X += ScreenLeft;
							Y += ScreenTop;

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

		virtual void TypeKeyboardChar(WCHAR c)
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
							SHORT vkch = VkKeyScan(c);
							if (vkch != -1)
							{
								INPUT input;
								memset(&input, 0, sizeof(input));

								input.type = INPUT_KEYBOARD;
								input.ki.dwFlags = 0;

								if (((vkch >> 8) & 0x1) != 0) //Shift
								{
									input.ki.wVk = VK_SHIFT;
									SendInput(1, &input, sizeof(input));
								}

								if (((vkch >> 8) & 0x2) != 0) //CTRL
								{
									input.ki.wVk = VK_CONTROL;
									SendInput(1, &input, sizeof(input));
								}

								if (((vkch >> 8) & 0x4) != 0) //ALT
								{
									input.ki.wVk = VK_MENU; //odd name
									SendInput(1, &input, sizeof(input));
								}

								input.ki.wVk = vkch & 0xFF;
								SendInput(1, &input, sizeof(input));

								input.ki.dwFlags = KEYEVENTF_KEYUP;
								SendInput(1, &input, sizeof(input));

								if (((vkch >> 8) & 0x1) != 0) //Shift
								{
									input.ki.wVk = VK_SHIFT;
									SendInput(1, &input, sizeof(input));
								}

								if (((vkch >> 8) & 0x2) != 0) //CTRL
								{
									input.ki.wVk = VK_CONTROL;
									SendInput(1, &input, sizeof(input));
								}

								if (((vkch >> 8) & 0x4) != 0) //ALT
								{
									input.ki.wVk = VK_MENU; //odd name
									SendInput(1, &input, sizeof(input));
								}
							}
						}

						SetProcessWindowStation(oldWndSta);
						CloseWindowStation(WndSta);
					}
				}
			}
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

		virtual bool SetEFINextBootDevice(int ID)
		{
			DWORD bootordersz;
			byte bootorderdata[1000];
			bootordersz = GetFirmwareEnvironmentVariable(L"BootOrder", EFI_SPACE_GUID, bootorderdata, sizeof(bootorderdata));
			if (bootordersz == 0)
				return(false);

			UINT16 NextBootDevice = (UINT16)ID;

			for (DWORD i = 0; i < bootordersz; i += 2)
			{
				UINT16 BootID = (UINT16)*(&bootorderdata[i]);

				if (BootID == NextBootDevice)
				{
					WCHAR BootVar[20];
					byte data[1000];

					swprintf_s(BootVar, sizeof(BootVar) / 2, L"Boot%04X", BootID);
					memset(data, 0, sizeof(data));

					DWORD res = GetFirmwareEnvironmentVariable(BootVar, EFI_SPACE_GUID, data, sizeof(data));
					if (res == 0)
						return(false);

					if (SetFirmwareEnvironmentVariableEx_CXX(L"BootNext", EFI_SPACE_GUID, &NextBootDevice, sizeof(NextBootDevice), 0x7) == 0)
						return(false);
					else
						return(true);
				}
			}
			return(false);
		}

		virtual bool GetEFIBootDevices([Runtime::InteropServices::Out] Dictionary<int, String^>^% Dict)
		{
			DWORD bootordersz;
			byte bootorderdata[1000];
			bootordersz = GetFirmwareEnvironmentVariable(L"BootOrder", EFI_SPACE_GUID, bootorderdata, sizeof(bootorderdata));
			if (bootordersz == 0)
				return(false);

			Dict = gcnew Dictionary<int, String^>();
			for (DWORD i = 0; i < bootordersz; i += 2)
			{
				UINT16 BootID = (UINT16)*(&bootorderdata[i]);
				WCHAR BootVar[20];
				WCHAR BootName[1000];
				byte data[1000];

				swprintf_s(BootVar, sizeof(BootVar) / 2, L"Boot%04X", BootID);
				memset(data, 0, sizeof(data));

				DWORD res = GetFirmwareEnvironmentVariable(BootVar, EFI_SPACE_GUID, data, sizeof(data));
				if (res == 0)
					continue;

				int j = 6;
				do
				{
					BootName[(j - 6) / 2] = (WCHAR)*(&data[j]);
					if (BootName[(j - 6) / 2] == 0)
						break;
					j += 2;
				} while (j < sizeof(data));

				Dict->Add(BootID, gcnew String(BootName));
			}

			return(true);
		}

		virtual List<List<String^>^>^ DNSQueryTXT(String ^Name)
		{
			marshal_context^ contextdnsname = gcnew marshal_context();
			const WCHAR* NameC = contextdnsname->marshal_as<const WCHAR*>(Name);

			PDNS_RECORD records;
			List<List<String^>^>^% list = gcnew List<List<String^>^>();

			if (DnsQuery(NameC, DNS_TYPE_TEXT, DNS_QUERY_STANDARD, NULL, &records, NULL) != 0)
				return(nullptr);

			PDNS_RECORD currentrec = records;
			do
			{
				List<String^>^% sublist = gcnew List<String^>();

				for (unsigned int i = 0; i < currentrec->Data.TXT.dwStringCount; i++)
				{
					sublist->Add(gcnew String(currentrec->Data.TXT.pStringArray[i]));
				}

				list->Add(sublist);

				if (currentrec->pNext == NULL)
					break;
				else
					currentrec = currentrec->pNext;
			} while (true);

			DnsRecordListFree(records, DnsFreeRecordList);

			return (list);
		}

		virtual Nullable<Boolean> ApplyTimeZone(String ^Name)
		{
			DYNAMIC_TIME_ZONE_INFORMATION dynamicTimezone = {};
			DWORD dwResult = 0;
			DWORD i = 0;
			do
			{
				dwResult = EnumDynamicTimeZoneInformation(i++, &dynamicTimezone);
				if (dwResult == ERROR_SUCCESS)
				{
					if (gcnew String(dynamicTimezone.TimeZoneKeyName) == Name)
					{
						if (SetDynamicTimeZoneInformation(&dynamicTimezone) == 0)
							return(Nullable<Boolean>(false)); //failed
						else
							return(Nullable<Boolean>(true)); //success
					}
				}
			} while (dwResult != ERROR_NO_MORE_ITEMS);

			return(Nullable<Boolean>());
		}

	private:
		DWORD LastMouseEvent = 0;
		int ScreenNumber = 0;

		int ScreenLeft = 0;
		int ScreenTop = 0;

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
