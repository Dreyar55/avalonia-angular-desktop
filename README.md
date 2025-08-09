# Avalonia Desktop Web (Angular + .NET)

This is a small desktop app in Avalonia that hosts a WebView window - an Angular application consuming a .NET backend in desktop form.

I was looking for a [Tauri.js](https://v2.tauri.app/) alternative where I could keep the backend in .NET instead of Rust. After finding nothing, I went on a journey to build a small project like that myself. The goals were to create a desktop app whose backend will be written in .NET and the frontend in one of the popular JS frameworks (in this case, Angular).

## Stack
- Avalonia UI
- ASP.NET Core
- Angular 20

## The flow
- Avalonia boots up a desktop window that hosts a WebView (`WebView.Avalonia`).
- It runs an ASP.NET Core API with a simple `/api/metadata` REST endpoint.
- In Debug mode, the WebView points to the Angular dev server at `http://localhost:4200`.
- In Release mode, the WebView points to the embedded ASP.NET Core API, which serves the endpoints and the Angular bundle previously built via `npm run build`.

## Prerequisites
- Node.js
- .NET SDK 9.0
- Visual Studio 2022, VS Code or JetBrains Rider

## How to run

### Debug mode

1) Install dependencies
```bash
npm install
```

2) Start the Angular dev server
```bash
ng serve
```

3) Run the desktop app
Open the solution in Visual Studio or Rider and run the `DesktopApp` project in Debug mode. It will open the Avalonia window and load `http://localhost:4200` inside the WebView.

### Release mode

1) Build the frontend
```bash
npm run build
```

2) Run the desktop app
Open the solution in Visual Studio or Rider and run the `DesktopApp` project in Release mode. Alternatively, publish the desktop app in Release mode and then open it. It will open the Avalonia window and load the frontend served from the embedded ASP.NET Core server (defaults to `http://localhost:54321`, or another free port) from the `wwwroot/` directory inside the WebView.

## Final word
It's very unfortunate that .NET doesn't have something like Tauri. The closest thing I found was ElectronJS in .NET but it hasn't been updated in a while and it wouldn't work the way I expected it to. This was a fun project to spend a few days on and I am surprised it worked out the way it did.

Issues and PRs are welcome as well as ideas to improve the "template" or make it work for other JS frameworks. In case anyone actually creates a proper framework like Tauri, I would love to know about it.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.