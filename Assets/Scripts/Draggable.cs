//#define DEVELOP

using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class Draggable : MonoBehaviour
{
#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || DEVELOP

	[DllImport("user32.dll")]
	public static extern IntPtr FindWindow(string className, string windowName);

	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int W, int H, uint uFlags);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool GetCursorPos(out POINT lpPoint);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	public static extern int GetSystemMetrics(int nIndex);

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		public int X;
		public int Y;
		public static implicit operator Vector2(POINT p)
		{
			return new Vector2(p.X, p.Y);
		}
	}

	static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
	const uint SWP_NOSIZE = 0x0001;

	const int SM_CXSCREEN = 0;

	POINT DragStart;
	bool Dragging = true;
	const int DragLimit = 150;

	void Start()
	{

	}

	void Update()
	{

	}

	void OnMouseDown()
	{
		var hwnd = FindWindow(null, "CooldogAssistant");

		if (hwnd != IntPtr.Zero)
		{
			POINT p;
			GetCursorPos(out p);
			p.X -= Screen.width / 2;
			p.Y -= Screen.width / 4;

			DragStart = p;
			Dragging = false;
		}
	}
	void OnMouseDrag()
	{
		var hwnd = FindWindow(null, "CooldogAssistant");

		if (hwnd != IntPtr.Zero)
		{
			POINT p;
			GetCursorPos(out p);
			p.X -= Screen.width / 2;
			p.Y -= Screen.width / 4;

			if (Dragging || (new Vector2(p.X, p.Y) - new Vector2(DragStart.X, DragStart.Y)).magnitude > DragLimit)
			{
				Dragging = true;

				SetWindowPos(hwnd, HWND_TOPMOST, p.X, p.Y, 0, 0, SWP_NOSIZE);
				GetComponent<SpriteRenderer>().flipX = p.X < GetSystemMetrics(SM_CXSCREEN) / 2;
			}
		}
	}
#endif
}
