﻿using ConsoleGUI;
using Tlcmm.Core;
using Tlcmm.Gui;

Execution.StartUp(args);

ConsoleManager.Setup();

ConsoleManager.Content = Layout.Build();

var globalHotkeys = new GlobalHotkeys();

Layout.InputListeners.Add(globalHotkeys);

while (!globalHotkeys.TerminationRequested)
{
    Thread.Sleep(50);
    ConsoleManager.AdjustBufferSize();

    ConsoleManager.ReadInput(Layout.InputListeners);
}

Console.BackgroundColor = (ConsoleColor)(-1);
Console.ForegroundColor = (ConsoleColor)(-1);
Console.Clear();
Console.CursorVisible = true;

Execution.Finish();
