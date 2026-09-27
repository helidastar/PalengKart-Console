<div align="center">

# PalengKart

**Console Inventory and Point-of-Sale System for a Small Store**

A store system where the admin manages products and stock, and customers shop with a cart and check out. Every sale is recorded, and the admin is warned when a product runs low.

![Status](https://img.shields.io/badge/status-complete-brightgreen)
![C#](https://img.shields.io/badge/C%23-239120?logoColor=white)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
[![Build](https://github.com/helidastar/PalengKart-Console/actions/workflows/dotnet.yml/badge.svg)](https://github.com/helidastar/PalengKart-Console/actions/workflows/dotnet.yml)

**[Read the Full Documentation](docs/DOCUMENTATION.md)**

</div>

---

## About

Small stores and market stalls often track stock on paper, so it is hard to know what is running low, what was sold, and how much was earned.

**PalengKart** keeps one list of products with their category, price, quantity and unit. Customers build a cart and check out, stock goes down automatically, and every sale is recorded for the admin's sales report. It is built around a class diagram that uses abstraction, inheritance and polymorphism (`User` → `Admin` / `Customer`).

## Key Features

- **Admin Menu** — add, update and remove products, view the inventory
- **Customer Accounts** — create an account and log in with a password
- **Shopping Cart** — add products, see the total, and check out
- **Sales Report** — every sale with date, customer, product and amount
- **Low Stock Alerts** — products at or below their minimum stock
- **Barcodes** — auto-generated EAN-13 product IDs, shown as an ASCII barcode
- **Saved Inventory** — stored in `inventory.txt` after every change

## Tech Stack

C# · .NET 10 · console application · GitHub Actions

## Project Status

**Complete.** All classes from the class diagram are built, and the admin and customer menus work end to end. Tested with a full customer and admin session; see [Testing](docs/DOCUMENTATION.md#9-testing).

## Getting Started

Requires the **[.NET 10 SDK](https://dotnet.microsoft.com/download)**.

```bash
dotnet run
```

| Login | How |
|---|---|
| Admin | Username `admin` |
| Customer | Any new username creates an account |

## Documentation

> **The complete project documentation is in [docs/DOCUMENTATION.md](docs/DOCUMENTATION.md).**
>
> It covers the user flow, every menu option, the class diagram and the OOP concepts used, the save file format, setup, testing, known limitations, and the branch and commit rules.

## Contributors

| Name | GitHub |
|------|--------|
| Charity T. Ricabo | [@helidastar](https://github.com/helidastar) |

<div align="center">

**CPE261 Object Oriented Programming 1 · 2024** · Instructor: Engr. Julian M. Semblante

</div>
