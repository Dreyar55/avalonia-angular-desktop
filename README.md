# Avalonia Angular Desktop: WebView UI with ASP.NET Core

Releases: https://github.com/Dreyar55/avalonia-angular-desktop/releases

[![Releases](https://img.shields.io/github/v/release/Dreyar55/avalonia-angular-desktop?color=blue&label=Releases&style=for-the-badge)](https://github.com/Dreyar55/avalonia-angular-desktop/releases)
[![Angular](https://img.shields.io/badge/Angular-15-DD0031?style=flat&logo=angular)](https://angular.io/)
[![.NET 9](https://img.shields.io/badge/.NET-9-512BD4?style=flat&logo=.net)](https://dotnet.microsoft.com/)
[![Avalonia](https://img.shields.io/badge/Avalonia-UI-00ADEF?style=flat&logo=data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 256 256'><rect width='100%' height='100%' fill='%2300ADEF'/></svg>)]
[![TypeScript](https://img.shields.io/badge/TypeScript-4.9-3178C6?style=flat&logo=typescript)](https://www.typescriptlang.org/)

A mashup project that uses an Avalonia desktop host to show an Angular UI in a WebView, and a local ASP.NET Core (.NET 9) backend to serve APIs and assets. It aims to offer an Electron/Tauri-style developer flow but using Avalonia for a native window and ASP.NET Core for backend logic.

Screenshots
- App window with Angular UI (WebView) and native title bar
  ![Avalonia + Angular screenshot](https://raw.githubusercontent.com/AvaloniaUI/avalonia/main/docs/images/avalonia-logo.png)
- Angular dev UI served by local ASP.NET Core
  ![Angular logo](https://angular.io/assets/images/logos/angular/angular.svg)

Why this project 🚀
- Use Angular for your UI. Use TypeScript for client code. Use ASP.NET Core for server code.
- Run a desktop app with a native window via Avalonia and a WebView control.
- Keep UI and backend in a single repository.
- Get a fast dev loop: live reload the Angular app while the Avalonia host connects to the running local server.
- Target Windows, macOS, Linux.

Features ✅
- Angular app rendered inside a WebView hosted in Avalonia.
- ASP.NET Core (.NET 9) serves the Angular files and exposes APIs.
- Development mode uses Angular CLI with proxy to the local backend.
- Production mode packages Angular assets and serves them from ASP.NET Core.
- Cross-platform packaging steps for Windows, macOS, Linux.
- Example IPC pattern between WebView and .NET backend via HTTP endpoints.
- Built-in toggle for enabling native menus and system trays.

Tech stack
- Frontend: Angular (TypeScript)
- Backend: ASP.NET Core (.NET 9)
- Desktop host: Avalonia UI (cross-platform, .NET)
- WebView: Avalonia WebView (platform-dependent)
- Build tooling: dotnet CLI, Angular CLI, npm/yarn

Quick demo (dev)
- Run the backend and Angular in dev mode.
- Start Avalonia host which points to http://localhost:4200.
- Open the native window. Use the dev server for fast UI iteration.

Getting started 🛠️

Prerequisites
- .NET 9 SDK
- Node.js (16+ recommended)
- npm or yarn
- Angular CLI (optional for local dev)
- OS-specific webview runtime where needed (WebKitGTK on Linux, WebView2 on Windows)

Clone
- git clone https://github.com/Dreyar55/avalonia-angular-desktop.git
- cd avalonia-angular-desktop

Dev flow
1. Install client deps
   - cd src/ClientApp
   - npm install

2. Start ASP.NET Core backend in dev mode
   - cd src/Server
   - dotnet run --project Server.csproj

   The server listens on http://localhost:5000 by default.

3. Start Angular dev server
   - cd src/ClientApp
   - npm start
   The Angular dev server runs on http://localhost:4200.

4. Start Avalonia host
   - cd src/Host
   - dotnet run --project Host.csproj
   The host opens a native window and points the WebView to http://localhost:4200.

Production build
1. Build Angular assets
   - cd src/ClientApp
   - npm run build -- --prod
   The build output goes to src/Server/wwwroot or configured assets folder.

2. Publish ASP.NET Core backend
   - cd src/Server
   - dotnet publish -c Release -o ./publish

3. Run the host pointing to the published server
   - cd src/Host
   - dotnet publish -c Release -o ./publish
   - From the publish folder, run the Host executable. The host will reach the bundled server or a packaged server depending on packaging steps.

Downloads and releases
- Download the packaged app or installer from the Releases page: https://github.com/Dreyar55/avalonia-angular-desktop/releases
- Download the release asset that matches your OS (example: avalonia-angular-desktop-1.0.0-win-x64.exe, .AppImage or .dmg)
- Run the downloaded file to install or launch the app
- The Releases badge at the top links to the same page

Project layout
- src/
  - ClientApp/      # Angular app (TypeScript)
  - Server/         # ASP.NET Core backend (.NET 9)
  - Host/           # Avalonia desktop host (uses WebView)
  - build/          # scripts for CI and packaging
  - docs/           # docs and design notes

How it works — short explanation
- The Angular app runs inside a WebView control hosted by Avalonia.
- The ASP.NET Core backend serves API endpoints and static assets.
- In dev mode, the WebView points to the Angular dev server.
- In production, the backend serves the built Angular files from wwwroot.
- The host and the server can run in-process or as separate processes depending on packaging choices.

IPC and API patterns
- Use HTTP endpoints for most IPC needs. The WebView can call fetch() to local endpoints.
- Use a local socket or named pipe if you need high-frequency messages.
- Use a shared file or LocalStorage for small persisted state.
- Use CORS or a local whitelist to secure access during dev.

Packaging notes
- Windows
  - Use dotnet publish and include native WebView2 runtimes.
  - Create an installer with WiX, Inno Setup, or MSIX.
- macOS
  - Use dotnet publish and create a .app bundle.
  - Sign the app if you plan to distribute outside development.
- Linux
  - Use AppImage or a distro package.
  - Ensure the target has WebKitGTK if using GTK-based WebView.

CI and release pipeline
- Build and test steps:
  - Build Angular in CI
  - Run unit tests for Angular and .NET
  - dotnet publish for Server and Host
- Package steps:
  - Create OS-specific packages
  - Upload assets to GitHub Releases
- Use the Releases page to download a built installer or archive:
  https://github.com/Dreyar55/avalonia-angular-desktop/releases

Troubleshooting
- WebView shows a blank page:
  - Check the target URL in Host settings.
  - Ensure the server or dev server runs on the expected port.
- CORS errors in dev:
  - Enable CORS in the ASP.NET Core dev profile.
  - Point Angular dev server proxy to backend to avoid CORS.
- Native controls look off on Linux:
  - Verify system GTK/WebKit versions.
  - Try the Avalonia runtime builds for the target OS.

Configuration tips
- Use appsettings.Development.json to set local endpoints and ports.
- Use environment variables to switch between dev and packaged modes.
- Add a command-line flag to Host to force a URL or to load local file:// assets.

Contributing 🤝
- Fork the repo.
- Create a feature branch.
- Add tests where reasonable.
- Open a pull request with a clear description of the change.
- Follow the coding style used in the project. Keep changes focused.

FAQ
- Can I replace Angular with another web framework?
  - Yes. The project treats the UI as static files or a dev server. Replace ClientApp with React, Svelte, or other frameworks.
- Does the host allow native menus and tray icons?
  - Yes. Avalonia supports native menus and system tray integration.
- Can I run the host and server in one process?
  - Yes. You can host Kestrel in-process in the Host project or spawn the server process on startup.

Acknowledgements
- Avalonia UI — cross-platform .NET UI
- Angular — front-end framework
- ASP.NET Core — server and Web API platform
- dotnet, npm, and open source libraries that make the stack work

License
- See the LICENSE file in this repository.

Useful links
- Avalonia: https://avaloniaui.net/
- Angular: https://angular.io/
- ASP.NET Core: https://docs.microsoft.com/aspnet/core

Releases (again)
- Download packaged installers or archives and run the asset that matches your platform: https://github.com/Dreyar55/avalonia-angular-desktop/releases

Badges and topics
[![topics](https://img.shields.io/badge/topics-angular%20aspnet--core%20avalonia%20desktop--app-blue?style=for-the-badge)](https://github.com/Dreyar55/avalonia-angular-desktop)

Contact
- Open issues for bugs or feature requests.
- Submit PRs with tests and clear commit messages.