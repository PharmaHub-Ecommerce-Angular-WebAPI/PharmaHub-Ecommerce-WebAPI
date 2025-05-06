# PharmaHub Ecommerce - ASPDotNET Web API

PharmaHub is a powerful backend solution for an ecommerce platform built for pharmacy owners. It allows pharmacies to create and manage their online stores for selling medicines and beauty products. End-users can search for available medicines and find the nearest pharmacy that stocks them.

This API was developed using **ASPDotNET Core Web API** following a clean **6-layer N-Tier architecture** for maintainability, scalability, and separation of concerns.

## 🚀 Features

- ✅ **JWT Authentication & Authorization**
- ✅ **Email Confirmation via SMTP (SmtpClient)**
- ✅ **Stripe Payment Integration**
- ✅ **Cloudinary Integration for Image Storage**
- ✅ **Advanced Search (Exact, Partial, Fuzzy)**
- ✅ **Role-based Access Control (Admin, Pharmacy Owner, User)**
- ✅ **Product Package Support (Weak Entities)**
- ✅ **CRUD Operations (Pharmacy, Products, Categories, etc.)**
- ✅ **SQL Server Deployed Database**

## 🛠️ Technologies Used

- ASPDotNET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Stripe .NET SDK
- Cloudinary SDK
- SMTP (Email Confirmation)
- C#
- LINQ

## 🔐 Authentication & Roles

- **User Registration** with email confirmation
- **JWT Tokens** for secure login
- **Role Management** (Admin, Pharmacy, Customer)
- **Token Validation** via middleware

## 📷 Cloudinary Integration

- Uploads pharmacy product images to Cloudinary.
- Returns secure URLs stored in the database.

## 💳 Payment Gateway

- Integrated **Stripe** for handling secure online payments.

## 🔎 Search Functionalities

Supports:
- **Exact Match**
- **Partial Match (Contains)**
- **Fuzzy Match** using Levenshtein distance

## 📦 Product Packages

A product can be marked as a **Package** (enum), which is associated with one or more weak entity products using a one-to-many relationship.

## 📁 Folder Structure

```bash
PharmaHub.Ecommerce.WebAPI/
├── Controllers/
├── Services/
├── Business/
├── Data/
├── Domain/
├── DTOs/
├── Models/
└── Helpers/
