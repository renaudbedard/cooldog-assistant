//#define DEVELOP

using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || DEVELOP
	private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, ulong dwNewLong);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int W, int H, uint uFlags);

	[DllImport("user32.dll")]
	static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);

    const int GWL_STYLE = -16;
	const int GWL_EXSTYLE = -20;	

    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;
	const uint WS_EX_LAYERED = 0x00080000;
	const ulong WS_EX_TRANSPARENT = 0x00000020L;

	const uint SWP_NOMOVE = 0x0002;
	const uint SWP_NOSIZE = 0x0001;

	const uint LWA_ALPHA = 0x00000002;

	static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    void Start()
    {
        var margins = new MARGINS() { cxLeftWidth = -1 };
        var hwnd = GetActiveWindow();

        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);

        DwmExtendFrameIntoClientArea(hwnd, ref margins);

		SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
    }

	public void SetClickthrough(bool enabled) 
	{
		var hwnd = GetActiveWindow();
		SetWindowLong(hwnd, GWL_EXSTYLE, enabled ? (WS_EX_LAYERED | WS_EX_TRANSPARENT) : 0);
	}
#endif
}