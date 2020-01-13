# IConsole

[![nuget](https://img.shields.io/nuget/dt/IConsole.svg)](https://www.nuget.org/packages/IConsole/) 
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT) 
[![Join the chat at https://gitter.im/goblinfactory-konsole/community](https://badges.gitter.im/goblinfactory-konsole/community.svg)](https://gitter.im/goblinfactory-konsole/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

.NET `System.Console` abstraction. Use to remove a direct dependancy on `System.Console` and replace with a dependancy on a well used and well known console interface, `IConsole`, to allow for building rich 'testable', high quality interactive console applications and utilities.

As used by [`Goblinfactory.Konsole`](https://github.com/goblinfactory/konsole). (https://github.com/goblinfactory/konsole) The home of `ProgressBar`, `Window`, `Form` and `Drawing`.

## But I can write my own System.Console wrapper, it's takes less than 40 lines? 

It's not about writing a wrapper, that is very easy. It's about setting a standard of interoperability between everyone that uses this as their interface. Since you would have started by writing your own interface, and then also writing something that implements that interface, why not save yourself the 2 or 3 hours you will be sidetracked doing that and dive right in to cleaning up your code.

You can use `IConsole` as simply as typing, `add package IConsole`. You can always come back later and remove it. 

## How to use this library?

Pick the narrowest set of features that the class that you are refactoring depends on. 

Logging, printing only? `IWrite`, Needs to print in color? `IWriteColor`, Need to change the cursor position when printing? `IPrintAt`, eed to scroll portions of the screen? `IScrolling`, Need all of the above? `IConsole`.

Need only 2, e.g. printing only (no color) and printing at? Then use interface inheritance and implement just the bits you need. For example. 

```csharp

public interface IPrint : IWrite, IPrintAt { }

public class MyClass {
    public MyClass(IPrint print) { ...}
    ...
    _print.PrintAt(0, 60, $"Total {total}");
}
```

# Interfaces

## IWrite

Typically use for Logging and printing only. Nothing fancy, just writing something out the console or the build output.

- `void WriteLine(string format, params object[] args);`
- `void WriteLine(string text);`
- `void Write(string format, params object[] args);`
- `void Write(string text);`
- `void Clear();`

## ISetColors

Change the foreground and background color of what will get printed with the next Write, or WriteLine command.

- `ConsoleColor ForegroundColor { get; set; }`
- `ConsoleColor BackgroundColor { get; set; }`

## IWriteColor 

`public interface IWriteColor : ISetColors`

If you need to print in color. 

- `void Write(ConsoleColor color, string format, params object[] args);`
- `void Write(ConsoleColor color, string text);`
- `void WriteLine(ConsoleColor color, string format, params object[] args);`
- `void WriteLine(ConsoleColor color, string text);`
- `void Clear(ConsoleColor? backgroundColor);`

## IPrintAt

Interface for a class that needs to print at a specific location in a window. 

- `void PrintAt(int x, int y, string format, params object[] args);`
- `void PrintAt(int x, int y, string text);`
- `void PrintAt(int x, int y, char c);`
- `int WindowWidth { get; }`
- `int WindowHeight { get; }`

## IPrintAtColor

- `void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background);`

## IScrolling

Interface for a class that needs to be able to scroll portions of the screen. This will most likely cause your library to require platform specific implementations for scrolling.

- `void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor);`
- `void ScrollDown();`

## IWindowed

If you are writing a windowing library like `Konsole` then each window region needs to report back an AbsoluteX and AbsoluteY position so that printing can happen at the correct (relative) position on the real console.

- `int AbsoluteX { get; }`
- `int AbsoluteY { get; }`

## IConsoleState

Interface for all the console methods that are most at risk of causing corruptions in multithreaded programs. The way to protect against corruption is to manage locking and manually save and restore state.

- `ConsoleState State { get; set; }`
- `int CursorTop { get; set; }`
- `int CursorLeft { get; set; }`
- `void DoCommand(IConsole console, Action action);`
- `ConsoleColor ForegroundColor { get; set; }`
- `ConsoleColor BackgroundColor { get; set; }`
- `bool CursorVisible { get; set; }`

#### DoCommand

`void DoCommand(IConsole console, Action action)`

Runs an action that may or may not modify the console state that can cause corruptions when thread context swaps. Must lock on a static locker, do try catch, and ensure state is back to what it was before the command ran. If you're not writing a threadsafe control or threading is not an issue, then you can simply call `action()` in your implementation.

example implementation;

```csharp
  lock(_locker)
  {
    var state = console.State;
    try
    {
      action();
    }
    finally
    {
      console.State = state;</code>
    }
  }
```

## IConsole

This is the sum of all interfaces. It will require the most work to implement. Typically you often only need `IWrite` and-or  `IPrintAt`or `IPrintAtColor`

```
public interface IConsole : 
    IWrite, 
    IWriteColor, 
    IPrintAt, 
    IConsoleState, 
    IScrolling, 
    ISetColors, 
    IPrintAtColor,
    IWindowed  { }
```

## IWrite vs IConsole

If the app you are refactoring does not set the cursor position, and merely "writes" out via `System.Console` then use the `IWrite` interface as your dependancy. IWrite is good enough for 99% of `System.Console` refactorings, where you're essentially just logging stuff to the console. 

## Getting Started

1. Find code that writes to the `System.Console` directly. Do a grep search for `Console.*` to get started.

```csharp
public class Accounts
{
  ... some code here

  // methods below writes to the console

  public void DoSomethingWithInvoice(Invoice inv) {
      ... some code
      Console.WriteLine($"Invoice #{inv.Number}, Date: {inv.Date} Amount:{inv.Amount}");
      ...
  }
}

```

2. Install `IConsole` package

> install-package IConsole

3. Create your own live and  test stubb, Mock or fake that implements `IConsole`.
4. Refactor your code to use the  `IConsole` abstraction. `IConsole` uses `Konsole` as the parent namespace. 


```csharp

using Konsole; 

public class Accounts
{
  ... some code here
  private IConsole _console;
  public Accounts(IConsole console) {
      _console = console;
  }
  // change all writes to System.Console to use the injected IConsole
  
  public void DoSomethingWithInvoice(Invoice inv) {
      ... some code
      _console_.WriteLine($"Invoice #{inv.Number}, Date: {inv.Date} Amount:{inv.Amount}");
      ...
  }
}
```

5. now you can test your class


```csharp

using Konsole; 

    [Test]
    public void doing_something_to_invoices_must_print_the_invoice_details_to_the_console()
    {
        IConsole console = new MockConsole();
        var accounts = new Accounts(console);
        accounts.DoSomethingWithInvoice(TestData.MyTestInvoice1);

        // confirm the display is what you expected

        var expected = "Invoice #101, Date:21st February 2020, Amount:£ 1234.56";
        
        console.Buffer.Should().BeEquivalentTo(expected);
    }

```


## Pre-built battle hardened thread safe implementations for production and testing uses

Please take a look at [`Goblinfactory.Konsole`](https://github.com/goblinfactory/konsole) https://github.com/goblinfactory/konsole project which has pre-built

- `ConcurrentWriter() : IConsole`
- `NullWriter() : IConsole` 
- `Window() : IConsole`
- `MockConsole() : IConsole` 

.. as well as a full suite of Console library utilities, Box, Forms, ProgressBar, Windows and more that are all `IConsole` compatible.

* https://github.com/goblinfactory/konsole

## Other libraries using IConsole

- To add your library here, [chat with me on gitter](https://gitter.im/goblinfactory-konsole/community).



enjoy

Alan Hemmings