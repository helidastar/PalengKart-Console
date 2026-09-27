# PalengKart Store System

A console inventory and point-of-sale system for a small store, written in C# (.NET 10).

## Features

- View, add, update and remove products
- Sell products by barcode, with stock checks
- Auto-generated EAN-13 barcodes (with check digit), shown as a simple ASCII barcode
- Low stock alerts based on each product's minimum stock
- Inventory saved to `inventory.txt` after every change

## Requirements

- .NET 10 SDK

## Run

```bash
dotnet run
```

On first run, three sample products are added.

## Project structure

| File | Description |
|---|---|
| `Program.cs` | Main menu and user input |
| `Product.cs` | Product model |
| `Inventory.cs` | Product list, selling, and saving/loading `inventory.txt` |
| `BarcodeGenerator.cs` | EAN-13 barcode generation and display |

<div align="center">

**CPE261 Object Oriented Programming 1 · 2024** · Instructor: Engr. Julian M. Semblante

</div>
