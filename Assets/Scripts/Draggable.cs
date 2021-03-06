﻿//#define DEVELOP
//#define DISABLE

using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
	public InputField InputField;
	public Scratch Scratch;

	public const int DragLimit = 200;

	public void Start()
	{
		Scratch = GetComponent<Scratch>();
	}

#if ((UNITY_STANDALONE_WIN && !UNITY_EDITOR) || DEVELOP) && !DISABLE

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

	void WndOnMouseDown()
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
		if (Scratch.Scratching) return;

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
				bool flipX = p.X < GetSystemMetrics(SM_CXSCREEN) / 2;
				transform.parent.GetComponent<Cooldog>().Flipped = flipX;
			}
		}
	}
#endif

	void OnMouseDown()
	{	
		InputField.interactable = true;
		InputField.ActivateInputField();
		InputField.Select();

		#if ((UNITY_STANDALONE_WIN && !UNITY_EDITOR) || DEVELOP) && !DISABLE
		WndOnMouseDown ();
		#endif
	}
}
