# Remora Views POC
A (proof of concept) implementation of the MVC concept adapted for Discord.

This library only provides the view layer of M**V**C, the controller part coming from Remora's [built in interactivity system](https://github.com/Nihlus/Remora.Discord/tree/master/Remora.Interactivity).

---

## What does this library actually *do*?
Not much! As explained above, it mainly just provides a way to implement views into Remora.

In esssence, given a pre-defined class, you can ensure a consistent look and feel for a given message
without having to clutter your code with imperative code.

## How do I use this, then?
To begin, you'll want to nab `VTP.Remora.Views` from NuGet [(click)](https://www.nuget.org/packages/VTP.Remora.Views).

To create a view, simply make your class (or preferably a record!) implement `IView`; this  interface doesn't contain any logic; it's just for marking and class constraint. 

In this newly created view, you'll define properties that get picked up when the view is "rendered" (aka it's sent to Discord).

---

Take the following example:

```csharp
public record ExampleView(string Content) : IView 
{
    public IEmbed ExampleEmbed { get; init; } = new Embed { Title = "Hello there", Colour = Color.Goldenrod };
}   
```

The above code will generate a result something like this:

![](https://pics.wahs.uk/v/1tLYz)

Interesting, but not really that complex. Lets add some buttons!

Firstly, you'll want to know that when defining components, it's assumed that each component goes in it's own row; there's no assumptions made about the type of component.

If this is not desirable behavior, simply mark the component with `Row`, and passing an index.

Similiarly, you can change the order of the components by marking them with `Order`.

```csharp
public record View(string Content = "This is an example view") : IView
{
    public Embed Embed1 { get; init; } = new() { Title = "Record momeent" };
    
    public Embed Embed2 { get; init; } = new() { Title = "Example view embed!! But it's blue!!", Colour = Color.CornflowerBlue };
    
    [Row(0)]
    public ButtonComponent Button1 { get; init; } = new ButtonComponent(ButtonComponentStyle.Secondary, "I am second!", CustomID: "2");
    
    [Row(0)]
    [Order(1)]
    public ButtonComponent Button2 { get; init; } = new ButtonComponent(ButtonComponentStyle.Primary, "I am first!", CustomID: "1");
    
    [Row(0)]
    public ButtonComponent Button3 { get; init; } = new ButtonComponent(ButtonComponentStyle.Danger, "I am third!", CustomID: "3");
}
```

The following produces a view like such:

![](https://pics.wahs.uk/v/HiFKu)

---

That's about all there is to it! 