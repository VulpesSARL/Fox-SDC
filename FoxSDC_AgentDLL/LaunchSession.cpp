#pragma unmanaged
#include <stdio.h>
#include <Windows.h>
#include <Wtsapi32.h>
#include <Userenv.h>

void WriteEventLogVerbose(WCHAR* Blahblah);

bool CSetToken(LPWSTR NAME)
{
	HANDLE hToken;
	TOKEN_PRIVILEGES tkp;
	LUID luid;
	HANDLE pid = GetCurrentProcess();

	memset(&tkp, 0, sizeof(tkp));

	if (OpenProcessToken(pid, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken) == 0)
		return(false);

	if (LookupPrivilegeValue(NULL, NAME, &luid) == 0)
	{
		CloseHandle(hToken);
		return(false);
	}

	tkp.PrivilegeCount = 1;  // one privilege to set    
	tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
	tkp.Privileges[0].Luid = luid;

	if (AdjustTokenPrivileges(hToken, FALSE, &tkp, NULL, NULL, NULL) == 0)
	{
		CloseHandle(hToken);
		return(false);
	}

	CloseHandle(hToken);
	return(true);
}


bool LauchApp(WCHAR* FILE, WCHAR* ARGS)
{
	WTS_SESSION_INFO *sessions;
	DWORD num;

	if (WTSEnumerateSessions(WTS_CURRENT_SERVER_HANDLE, 0, 1, &sessions, &num) == 0)
		return(false);

	for (DWORD i = 0; i < num; i++)
	{
		WTS_SESSION_INFO si = sessions[i];

		if (si.SessionId == 0)
			continue;

		HANDLE token;
		HANDLE copiedtoken;
		if (WTSQueryUserToken(si.SessionId, &token) == 0)
			continue;

		if (DuplicateTokenEx(token, TOKEN_ASSIGN_PRIMARY | TOKEN_ALL_ACCESS, 0, SecurityImpersonation, TokenPrimary, &copiedtoken) == 0)
			continue;

		STARTUPINFO StartupInfo;
		PROCESS_INFORMATION processInfo;

		SECURITY_ATTRIBUTES Security1;
		SECURITY_ATTRIBUTES Security2;

		memset(&StartupInfo, 0, sizeof(STARTUPINFO));
		memset(&processInfo, 0, sizeof(PROCESS_INFORMATION));
		memset(&Security1, 0, sizeof(SECURITY_ATTRIBUTES));
		memset(&Security2, 0, sizeof(SECURITY_ATTRIBUTES));
		StartupInfo.cb = sizeof(STARTUPINFO);

		void *Environment;
		if (CreateEnvironmentBlock(&Environment, copiedtoken, FALSE) == 0)
		{
			CloseHandle(copiedtoken);
			continue;
		}

		int res = CreateProcessAsUser(copiedtoken, FILE, ARGS, &Security1, &Security2, false,
			NORMAL_PRIORITY_CLASS | CREATE_UNICODE_ENVIRONMENT, Environment, NULL, &StartupInfo, &processInfo);

		if (res == 0)
		{
			DestroyEnvironmentBlock(Environment);
			CloseHandle(copiedtoken);
			return(false);
		}

		CloseHandle(processInfo.hProcess);
		CloseHandle(processInfo.hThread);

		DestroyEnvironmentBlock(Environment);
		CloseHandle(copiedtoken);
	}

	WTSFreeMemory(sessions);

	return(true);
}

bool LauchApp(WCHAR* FILE, WCHAR* ARGS, DWORD SessionID, DWORD *PID)
{
	HANDLE token;
	HANDLE copiedtoken;
	if (WTSQueryUserToken(SessionID, &token) == 0)
		return(false);

	if (DuplicateTokenEx(token, TOKEN_ASSIGN_PRIMARY | TOKEN_ALL_ACCESS, 0, SecurityImpersonation, TokenPrimary, &copiedtoken) == 0)
		return(false);

	STARTUPINFO StartupInfo;
	PROCESS_INFORMATION processInfo;

	SECURITY_ATTRIBUTES Security1;
	SECURITY_ATTRIBUTES Security2;

	memset(&StartupInfo, 0, sizeof(STARTUPINFO));
	memset(&processInfo, 0, sizeof(PROCESS_INFORMATION));
	memset(&Security1, 0, sizeof(SECURITY_ATTRIBUTES));
	memset(&Security2, 0, sizeof(SECURITY_ATTRIBUTES));
	StartupInfo.cb = sizeof(STARTUPINFO);

	void *Environment;
	if (CreateEnvironmentBlock(&Environment, copiedtoken, FALSE) == 0)
	{
		CloseHandle(copiedtoken);
		return(false);
	}

	int res = CreateProcessAsUser(copiedtoken, FILE, ARGS, &Security1, &Security2, false,
		NORMAL_PRIORITY_CLASS | CREATE_UNICODE_ENVIRONMENT, Environment, NULL, &StartupInfo, &processInfo);

	if (PID != NULL)
	{
		*PID = processInfo.dwProcessId;
	}

	if (res == 0)
	{
		DestroyEnvironmentBlock(Environment);
		CloseHandle(copiedtoken);
		return(false);
	}

	DestroyEnvironmentBlock(Environment);
	CloseHandle(copiedtoken);

	CloseHandle(processInfo.hProcess);
	CloseHandle(processInfo.hThread);

	return(true);
}

bool LauchAppWait(WCHAR* FILE, WCHAR* ARGS, DWORD SessionID)
{
	HANDLE token;
	HANDLE copiedtoken;
	if (WTSQueryUserToken(SessionID, &token) == 0)
		return(false);

	if (DuplicateTokenEx(token, TOKEN_ASSIGN_PRIMARY | TOKEN_ALL_ACCESS, 0, SecurityImpersonation, TokenPrimary, &copiedtoken) == 0)
		return(false);

	STARTUPINFO StartupInfo;
	PROCESS_INFORMATION processInfo;

	SECURITY_ATTRIBUTES Security1;
	SECURITY_ATTRIBUTES Security2;

	memset(&StartupInfo, 0, sizeof(STARTUPINFO));
	memset(&processInfo, 0, sizeof(PROCESS_INFORMATION));
	memset(&Security1, 0, sizeof(SECURITY_ATTRIBUTES));
	memset(&Security2, 0, sizeof(SECURITY_ATTRIBUTES));
	StartupInfo.cb = sizeof(STARTUPINFO);

	void *Environment;
	if (CreateEnvironmentBlock(&Environment, copiedtoken, FALSE) == 0)
	{
		CloseHandle(copiedtoken);
		return(false);
	}

	int res = CreateProcessAsUser(copiedtoken, FILE, ARGS, &Security1, &Security2, false,
		NORMAL_PRIORITY_CLASS | CREATE_UNICODE_ENVIRONMENT, Environment, NULL, &StartupInfo, &processInfo);

	if (res == 0)
	{
		DestroyEnvironmentBlock(Environment);
		CloseHandle(copiedtoken);
		return(false);
	}

	WaitForSingleObject(processInfo.hProcess, INFINITE);

	CloseHandle(processInfo.hProcess);
	CloseHandle(processInfo.hThread);

	DestroyEnvironmentBlock(Environment);
	CloseHandle(copiedtoken);

	return(true);
}
