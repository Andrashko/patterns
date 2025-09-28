import ctypes
from ctypes import wintypes
from typing import Optional


# kernel32.CreateProcessW(
#     None,                     # lpApplicationName
#     "notepad.exe",            # lpCommandLine
#     None, None,               # security attrs
#     False,                    # bInheritHandles
#     0,                        # dwCreationFlags
#     None, None,               # environment, current dir
#     ctypes.byref(si),         # startup info
#     ctypes.byref(pi)          # process info
# )

# допоміжні структури
class STARTUPINFO(ctypes.Structure):
    _fields_ = [
        ("cb",             wintypes.DWORD),
        ("lpReserved",     wintypes.LPWSTR),
        ("lpDesktop",      wintypes.LPWSTR),
        ("lpTitle",        wintypes.LPWSTR),
        ("dwX",            wintypes.DWORD),
        ("dwY",            wintypes.DWORD),
        ("dwXSize",        wintypes.DWORD),
        ("dwYSize",        wintypes.DWORD),
        ("dwXCountChars",  wintypes.DWORD),
        ("dwYCountChars",  wintypes.DWORD),
        ("dwFillAttribute", wintypes.DWORD),
        ("dwFlags",        wintypes.DWORD),
        ("wShowWindow",    wintypes.WORD),
        ("cbReserved2",    wintypes.WORD),
        ("lpReserved2",    ctypes.POINTER(ctypes.c_byte)),
        ("hStdInput",      wintypes.HANDLE),
        ("hStdOutput",     wintypes.HANDLE),
        ("hStdError",      wintypes.HANDLE),
    ]


class PROCESS_INFORMATION(ctypes.Structure):
    _fields_ = [
        ("hProcess",       wintypes.HANDLE),
        ("hThread",        wintypes.HANDLE),
        ("dwProcessId",    wintypes.DWORD),
        ("dwThreadId",     wintypes.DWORD),
    ]


# Завантажуємо kernel32
kernel32 = ctypes.WinDLL("kernel32", use_last_error=True)
kernel32.CreateProcessW.restype = wintypes.BOOL


class WinApiAdapter:
    def create_process(self, process: str) -> Optional[PROCESS_INFORMATION]:
        si = STARTUPINFO()
        si.cb = ctypes.sizeof(STARTUPINFO)
        pi = PROCESS_INFORMATION()
        success: bool = kernel32.CreateProcessW(
            None,                     # lpApplicationName
            process,                  # lpCommandLine
            None, None,               # security attrs
            False,                    # bInheritHandles
            0,                        # dwCreationFlags
            None, None,               # environment, current dir
            ctypes.byref(si),         # startup info
            ctypes.byref(pi)          # process info
        )
        if not success:
            return None
        return pi
