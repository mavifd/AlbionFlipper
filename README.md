# AlbionFlipper

AlbionFlipper is a Windows desktop app (WinForms) for finding profitable item flips between Albion Online markets, with optional enchantment-upgrade profit calculations.

## What it does

- Receives live market order payloads on `http://localhost:8080/`
- Tracks buy/sell market data by selected cities
- Calculates direct flip profit and upgrade-path profit
- Supports manual JSON data input for testing
- Can auto-refresh rune/soul/relic costs from Albion Data API

## Tech stack

- .NET Framework 4.8.1
- C# WinForms
- Newtonsoft.Json
- ReaLTaiizor UI components

## Requirements

- Windows with **.NET Framework 4.8.1 Developer Pack/Runtime**
- Visual Studio (recommended) for building/running the solution

## Build and run

1. Open `AlbionFlipperServer.sln` in Visual Studio.
2. Restore NuGet packages (if prompted).
3. Build the solution.
4. Start the application.

## Usage

1. Start AlbionFlipper.
2. Select buy/sell cities and save.
3. Feed market JSON to `http://localhost:8080/` (or paste JSON into the manual data box in the app).
4. (Optional) click **Update Costs** with **Cloud Update** enabled.
5. Click **Check Profits** to populate results.

## Notes

- The app stores prices in memory while running.
- Sell-side includes **Black Market** support.
- This repository currently has no CI workflow files.
