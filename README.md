# Full Stack Cemetery Management System

**BSc Thesis Project** - Budapest University of Technology and Economics

## 📖 Overview
A full-stack web application designed to modernize cemetery record-keeping. It features a custom interactive map engine built on processed drone imagery, allowing users to virtually navigate the cemetery, locate specific graves, and visit digital memorial pages.

## 🛠 Tech Stack
* **Frontend:** React, TypeScript, Chakra UI, React Leaflet
* **Backend:** ASP.NET Core (.NET 8), Entity Framework Core
* **Database:** PostgreSQL
* **Architecture:** Clean Architecture (Onion) with Repository Pattern
* **Tools:** Docker, GitHub Actions

## 🏗 Architecture
The application follows **Clean Architecture** principles to ensure separation of concerns and maintainability. It utilizes the **Repository Pattern** to decouple business logic from data access.

<img width="769" alt="Clean Architecture Flow" src="https://github.com/user-attachments/assets/0780efd0-4956-41e2-b8b7-4d9638aaad71" />
<br>
<em>Data flow example: Adding a deceased record through the architectural layers.</em>

## 🗄 Database Design
The system uses a relational database (PostgreSQL) managed via Entity Framework Core (Code-First).

<img width="886" alt="Database Schema" src="https://github.com/user-attachments/assets/9dca2677-cef9-45cf-86e0-f95e7aa8b4c3" />

## ✨ Key Features & Screenshots

### 1. Interactive Mapping
Users can navigate a high-resolution drone map. The system uses custom tile generation to handle large image datasets efficiently.
<img width="660" alt="Interactive Map" src="https://github.com/user-attachments/assets/b66e4b6f-da38-46e8-9b8e-4838faf19466" />

### 2. Search & Filtering
Advanced filtering by name, birth year, and death year with pagination.
<img width="895" alt="Search Results" src="https://github.com/user-attachments/assets/56e538fd-25d1-492a-a649-2bcabd86a8e8" />

### 3. Digital Memorials
Dedicated pages for the deceased where users can light virtual candles and leave messages.
<img width="886" alt="Memorial Page" src="https://github.com/user-attachments/assets/490f0af7-bb30-4464-9075-8b0bbf94a42f" />

### 4. Admin Management
Secure login and dashboard for cemetery caretakers to manage grave polygons and deceased records directly on the map.

| Login Interface | Admin Edit Modal |
| :---: | :---: |
| <img width="400" src="https://github.com/user-attachments/assets/de3b56c2-86dc-49bc-a7c2-703148bc3e05" /> | <img width="400" src="https://github.com/user-attachments/assets/90307569-a5d7-46dc-a68e-e2707fa5194a" /> |

## 🚀 Getting Started


## 👤 Author
**József Urak**
* [LinkedIn Profile](https://www.linkedin.com/in/jozsef-urak/)
