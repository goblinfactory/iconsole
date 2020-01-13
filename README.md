# IConsole

[![nuget](https://img.shields.io/nuget/dt/IConsole.svg)](https://www.nuget.org/packages/IConsole/) 
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT) 
[![Join the chat at https://gitter.im/goblinfactory-konsole/community](https://badges.gitter.im/goblinfactory-konsole/community.svg)](https://gitter.im/goblinfactory-konsole/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

.NET `System.Console` abstraction. Use to remove a direct dependancy on `System.Console` and replace with a dependancy on a well used and well known console interface, `IConsole`, to allow for building rich 'testable', high quality interactive console applications and utilities.

As used by [`Goblinfactory.Konsole`](https://github.com/goblinfactory/konsole). (https://github.com/goblinfactory/konsole) The home of `ProgressBar`, `Window`, `Form` and `Drawing`.

## But I can write my own System.Console wrapper, it's takes less than 40 lines? 

It's not about writing a wrapper, that is very easy. It's about setting a standard of interoperability between everyone that uses this as their interface. Since you would have started by writing your own interface, and then also writing something that implements that interface, why not save yourself the 2 or 3 hours you will be sidetracked doing that and dive right in to cleaning up your code.

You can use `IConsole` as simply as typing, `add package IConsole`. You can always come back later and remove it. 

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

## Pre-built battle hardened thread safe implementations of IConsole, for live, and for testing

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