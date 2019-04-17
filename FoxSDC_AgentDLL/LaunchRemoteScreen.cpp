#pragma unmanaged
#include <stdio.h>
#include <Windows.h>
#include <Wtsapi32.h>
#include <Userenv.h>

bool LauchAppIntoWinLogon(WCHAR* FILE, WCHAR* ARGS)
{
	//Find PID of the current Winlogon.exe where the CurrentSessionID is
	PWTS_PROCESS_INFO ProcessInfo;
	DWORD ProcessCount;
	DWORD Id = -1;
	DWORD SessionId = WTSGetActiveConsoleSessionId();
	if (SessionId == 0xFFFFFFFF)
		return(false);

	if (WTSEnumerateProcesses(WTS_CURRENT_SERVER_HANDLE, 0, 1, &ProcessInfo, &ProcessCount) == 0)
		return(false);

	for (DWORD CurrentProcess = 0; CurrentProcess < ProcessCount; CurrentProcess++)
	{
		if (_wcsicmp(ProcessInfo[CurrentProcess].pProcessName, L"winlogon.exe") == 0)
		{
			if (SessionId == ProcessInfo[CurrentProcess].SessionId)
			{
				Id = ProcessInfo[CurrentProcess].ProcessId;
				break;
			}
		}
	}

	WTSFreeMemory(ProcessInfo);

	if (Id == -1)
		return(false);

	//Steal the token & copy it

	HANDLE WinLogonProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, Id);
	if (WinLogonProcess == NULL)
		return(false);

	HANDLE WinLogonToken;

	if (OpenProcessToken(WinLogonProcess, TOKEN_ASSIGN_PRIMARY | TOKEN_ALL_ACCESS, &WinLogonToken) == 0)
	{
		CloseHandle(WinLogonProcess);
		return(false);
	}

	HANDLE StolenToken;

	if (DuplicateTokenEx(WinLogonToken, TOKEN_ASSIGN_PRIMARY | TOKEN_ALL_ACCESS, NULL, SecurityImpersonation, TokenPrimary, &StolenToken) == 0)
	{
		CloseHandle(WinLogonToken);
		CloseHandle(WinLogonProcess);
		return(false);
	}

	DWORD SessionIdCopy = SessionId;
	if (SetTokenInformation(StolenToken, TokenSessionId, &SessionIdCopy, sizeof(DWORD)) == 0)
	{
		CloseHandle(WinLogonToken);
		CloseHandle(WinLogonProcess);
		return(false);
	}

	CloseHandle(WinLogonToken);
	CloseHandle(WinLogonProcess);

	//Execute application in Session, with Winlogon's rights

	STARTUPINFO StartupInfo;
	PROCESS_INFORMATION processInfo;

	SECURITY_ATTRIBUTES Security1;
	SECURITY_ATTRIBUTES Security2;

	memset(&StartupInfo, 0, sizeof(STARTUPINFO));
	memset(&processInfo, 0, sizeof(PROCESS_INFORMATION));
	memset(&Security1, 0, sizeof(SECURITY_ATTRIBUTES));
	memset(&Security2, 0, sizeof(SECURITY_ATTRIBUTES));
	StartupInfo.cb = sizeof(STARTUPINFO);
	StartupInfo.lpDesktop = L"Winsta0\\Default";
	StartupInfo.wShowWindow = SW_HIDE;

	void *Environment;
	if (CreateEnvironmentBlock(&Environment, StolenToken, FALSE) == 0)
	{
		CloseHandle(StolenToken);
		return(false);
	}

	int res = CreateProcessAsUser(StolenToken, FILE, ARGS, &Security1, &Security2, false,
		NORMAL_PRIORITY_CLASS | CREATE_UNICODE_ENVIRONMENT | DETACHED_PROCESS, Environment, NULL, &StartupInfo, &processInfo);

	if (res == 0)
	{
		DestroyEnvironmentBlock(Environment);
		CloseHandle(StolenToken);
		return(false);
	}

	DestroyEnvironmentBlock(Environment);
	CloseHandle(StolenToken);

	CloseHandle(processInfo.hProcess);
	CloseHandle(processInfo.hThread);

	return(true);
}
