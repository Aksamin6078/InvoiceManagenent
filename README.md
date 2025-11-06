Invoice Management System

A full-featured Invoice Management Web Application built with ASP.NET Core MVC for managing products, categories, customers, invoices, and generating insightful reports. Designed for small businesses to streamline inventory and sales management.

Features

✅ Dashboard: Overview of products, categories, customers, and sales.
✅ Product Management: Create, edit, delete, and view products.
✅ Category Management: Organize products by categories.
✅ Customer Management: Maintain customer details.
✅ Invoice Management: Create invoices, add items, calculate totals.
✅ Reports:

Monthly sales charts

Top selling products

Low stock alerts

Top customers by revenue
✅ Responsive Design: Works on desktop and mobile devices.

Technologies

Backend: ASP.NET Core MVC, C#

Frontend: Razor Pages, Bootstrap 5, Chart.js

Database: SQL Server / EF Core (Entity Framework Core)

Tools: Visual Studio, Git, NuGet packages for services


Setup & Installation


Restore NuGet packages

dotnet restore


Update connection string in appsettings.json:

"ConnectionStrings": { "con": "Server=YOUR_SERVER; Database=Invoi_DB; Trusted_Connection=True" }


Apply migrations (NuGet packages)

Update-database
