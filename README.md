# NSScreenshotOrganiser

A Windows desktop application to sort Nintendo Switch Album folder into subfolders named by game.

## TODO
* Make the program asynchronous. UI currently freezes while copying files, giving the impression the app is frozen. 
* Set up proper branches for Dev and Release. Clean up releases, tags.. etc. Also implement proper GPG signing.

## Requirements
*  [.NET 8 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Usage
1. Ensure your Switch's Album folder is accessible (either copied to your PC or your microSD card inserted)
2. Run **NSScreenshotOrganiser.exe**
3. Select both your Album and desired output folders, then click **Let's-a go!**.
4. When the program encounters a game it has not seen before, it will preview the file if it can and ask you what game it belongs to.

## Notes
* Depending on the size of your Album folder and other factors (disk read/write speeds, etc.), it may take some time to copy them all. 
