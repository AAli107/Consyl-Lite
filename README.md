# Consyl-Lite
Consyl Lite is a fast, light-weight Console Game Framework that enables you to develop games written in C#

## How to Run
To run and test the project, you could go to `./Code.cs` and call any of the rendering functions (`gfx.DrawPixel` `gfx.DrawText`, `gfx.DrawLine`) inside the `Render` function. Here's an example code to run.
```c#
public static void Render(Game game, Graphics gfx)
{
    gfx.DrawPixel(      // Draws a single ASCII pixel
        x: 5, y: 10,     // X and Y Position
        c: 'A'          // which ASCII pixel to draw
    );

    gfx.DrawText(               // Draws a string of text
        x: 10, y: 10,           // X and Y Position
        message: "Hello World!" // Message to display
    );

    gfx.DrawLine(       // Draws a line between two points
        x0: 1, y0: 2,   // X and Y Position of first point
        x1: 20, y1: 5, // X and Y Position of second point
        c: 'X'          // ASCII pixel the line is made of
    );
}
```
Once you setup the render code, you can now run the following:
```bash
dotnet run
```
This would build and run the code and the loop/rendering would be displayed in the terminal itself.
**Note that rendering may flicker in some scenario, so be sure to adjust terminal size to fit the area being rendered or adjust `WIDTH` and `HEIGHT` values found in `./Engine.cs`...**
To build the project as is without running it, just run `dotnet build -c Debug` when testing or `dotnet build -c Release` for release build.

## Features
### Engine
- Keyboard Input support
- Get Delta time as well as time since game started
### Rendering
- Drawing ASCII Pixels
- Drawing Text
- Drawing Lines