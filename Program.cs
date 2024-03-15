// See https://aka.ms/new-console-template for more information
using ArmyDatabase;
Interface i = new();
AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

void CurrentDomain_ProcessExit(object? sender, EventArgs e)
{
    i.Serialize();
}


i.Start();