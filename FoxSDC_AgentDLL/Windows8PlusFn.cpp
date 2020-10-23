#pragma unmanaged

#include <Windows.h>
#include <stdlib.h>
#include <stdio.h>

typedef BOOL(*FoxGetFirmwareType_)(PFIRMWARE_TYPE firmwaretype);
typedef VOID(*SendSAS_)(BOOL AsUser);
typedef BOOL(*FoxSetFirmwareEnvironmentVariableEx_)(LPCWSTR lpName, LPCWSTR lpGuid, PVOID pValue, DWORD nSize, DWORD dwAttributes);
typedef DWORD(*FoxEnumDynamicTimeZoneInformation_)(DWORD dwIndex, PDYNAMIC_TIME_ZONE_INFORMATION lpTimeZoneInformation);


FoxGetFirmwareType_ FoxGetFirmwareType;
SendSAS_ SASFn;
FoxSetFirmwareEnvironmentVariableEx_ FoxSetFirmwareEnvironmentVariableEx;
FoxEnumDynamicTimeZoneInformation_ FoxEnumDynamicTimeZoneInformation;
HMODULE Kernel32;
HMODULE SAS;
HMODULE Advapi32;

BOOL DummyGetFirmwareType(PFIRMWARE_TYPE firmwaretype)
{
	(*firmwaretype) = FirmwareTypeUnknown;
	return(true);
}

VOID DummySendSAS(BOOL AsUser)
{

}

DWORD DummyEnumDynamicTimeZoneInformation(DWORD dwIndex, PDYNAMIC_TIME_ZONE_INFORMATION lpTimeZoneInformation)
{
	return(ERROR_NO_MORE_ITEMS);
}

BOOL DummySetFirmwareEnviVarEx(LPCWSTR lpName, LPCWSTR lpGuid, PVOID pValue, DWORD nSize, DWORD dwAttributes)
{
	return(false);
}

BOOL GetFirmwareType_CXX(PFIRMWARE_TYPE firmwaretype)
{
	return (FoxGetFirmwareType(firmwaretype));
}

BOOL SetFirmwareEnvironmentVariableEx_CXX(LPCWSTR lpName, LPCWSTR lpGuid, PVOID pValue, DWORD nSize, DWORD dwAttributes)
{
	return(FoxSetFirmwareEnvironmentVariableEx(lpName, lpGuid, pValue, nSize, dwAttributes));
}

DWORD EnumDynamicTimeZoneInformation_CXX(DWORD dwIndex, PDYNAMIC_TIME_ZONE_INFORMATION lpTimeZoneInformation)
{
	return(FoxEnumDynamicTimeZoneInformation(dwIndex, lpTimeZoneInformation));
}


VOID SendSAS(BOOL AsUser)
{
	SASFn(AsUser);
}

void InitDLLWin8Plus()
{
	WCHAR DLLFile[1024];
	ExpandEnvironmentStrings(L"%WINDIR%\\SYSTEM32\\Kernel32.dll", DLLFile, 512);
	Kernel32 = LoadLibrary(DLLFile);
	if (Kernel32 == NULL)
	{
		FoxGetFirmwareType = &DummyGetFirmwareType;
		FoxSetFirmwareEnvironmentVariableEx = &DummySetFirmwareEnviVarEx;
	}
	else
	{
		FoxGetFirmwareType = (FoxGetFirmwareType_)GetProcAddress(Kernel32, "GetFirmwareType");
		if (FoxGetFirmwareType == NULL)
			FoxGetFirmwareType = &DummyGetFirmwareType;
		FoxSetFirmwareEnvironmentVariableEx = (FoxSetFirmwareEnvironmentVariableEx_)GetProcAddress(Kernel32, "SetFirmwareEnvironmentVariableExW");
		if (FoxSetFirmwareEnvironmentVariableEx == NULL)
			FoxSetFirmwareEnvironmentVariableEx = &DummySetFirmwareEnviVarEx;
	}

	ExpandEnvironmentStrings(L"%WINDIR%\\SYSTEM32\\Advapi32.dll", DLLFile, 512);
	Advapi32 = LoadLibrary(DLLFile);
	if (Advapi32 == NULL)
	{
		FoxEnumDynamicTimeZoneInformation = &DummyEnumDynamicTimeZoneInformation;
	}
	else
	{
		FoxEnumDynamicTimeZoneInformation = (FoxEnumDynamicTimeZoneInformation_)GetProcAddress(Advapi32, "EnumDynamicTimeZoneInformation");
		if (FoxEnumDynamicTimeZoneInformation == NULL)
			FoxEnumDynamicTimeZoneInformation = &DummyEnumDynamicTimeZoneInformation;
	}

	ExpandEnvironmentStrings(L"%WINDIR%\\SYSTEM32\\SAS.dll", DLLFile, 512);
	SAS = LoadLibrary(DLLFile);
	if (SAS == NULL)
	{
		if (GetModuleFileName(NULL, DLLFile, 512) == 0)
		{
			SASFn = &DummySendSAS;
		}
		else
		{
			SYSTEM_INFO sysinfo;
			GetNativeSystemInfo(&sysinfo);
			WCHAR Arch[20];
			switch (sysinfo.wProcessorArchitecture)
			{
			case PROCESSOR_ARCHITECTURE_AMD64:
				swprintf_s(Arch, 10, L"64"); break;
			case PROCESSOR_ARCHITECTURE_INTEL:
				swprintf_s(Arch, 10, L"32"); break;
			case 12: //PROCESSOR_ARCHITECTURE_ARM64
				swprintf_s(Arch, 10, L"ARM64"); break;
			default:
				swprintf_s(Arch, 10, L""); break;
			}

			if (wcscmp(Arch, L"") == 0)
			{
				SASFn = &DummySendSAS;
			}
			else
			{
				WCHAR Drive[512];
				WCHAR Dir[512];
				WCHAR Filename[512];
				WCHAR Ext[512];
				if (_wsplitpath_s(DLLFile, Drive, 256, Dir, 256, Filename, 256, Ext, 256) != 0)
				{
					SASFn = &DummySendSAS;
				}
				else
				{
					swprintf_s(DLLFile, 512, L"%s%sFoxSDC_SAS%s.dll", Drive, Dir, Arch);
					SAS = LoadLibrary(DLLFile);
					if (SAS == NULL)
					{
						SASFn = &DummySendSAS;
					}
					else
					{
						SASFn = (SendSAS_)GetProcAddress(SAS, "SendSAS");
						if (SASFn == NULL)
							SASFn = &DummySendSAS;
					}
				}
			}
		}
	}
	else
	{
		SASFn = (SendSAS_)GetProcAddress(SAS, "SendSAS");
		if (SASFn == NULL)
			SASFn = &DummySendSAS;
	}
}
