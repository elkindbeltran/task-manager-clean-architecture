# Task Manager using Clean Architecture

## Overview
This project implements a **Task Management System** using **.NET Web API** and **Angular** SPA, following **Clean Architecture**, CQRS with MediatR, and SQL Server.

It supports user and task management, including advanced filtering and JSON data handling in SQL Server.

---

## Architecture

### Solution structure

```
src/backend/
 ├── TaskManagerCleanArchitecture.API
 ├── TaskManagerCleanArchitecture.Application
 ├── TaskManagerCleanArchitecture.Domain
 ├── TaskManagerCleanArchitecture.Infrastructure

src/frontend/
 ├── task-manager-angular

tests/
 ├── TaskManagerCleanArchitecture.Tests
```

### Layers - Backend

- **API** → Controllers, middleware  
- **Application** → Commands, Queries, Validators  
- **Domain** → Entities, business rules  
- **Infrastructure** → EF Core, repositories  

---

## Getting Started

### Run the API

```
cd src/backend
dotnet run --project TaskManagerCleanArchitecture.API --launch-profile "https" --urls="https://localhost:44342"
```

Swagger available at:

https://localhost:PORT/swagger

---

## Database Setup

### Apply migrations

```
cd src/backend
dotnet ef database update --project TaskManagerCleanArchitecture.Infrastructure --startup-project TaskManagerCleanArchitecture.API
```
---

## API Endpoints

### Users

- POST /api/users  
- GET /api/users  

---

### Tasks

- POST /api/tasks  
- GET /api/tasks  
- PUT /api/tasks/{id}/status  

---

### Filtering & Sorting

GET /api/tasks?userId={guid}&status={status}&order=asc|desc

Supports:
- Filter by user  
- Filter by status  
- Sort by creation date  

---

### Run Angular SPA
```
cd src/frontend/task-manager-angular
npm install
ng serve -o
```
### If required, change API url on Angular SPA

Edit: src/frontend/task-manager-angular/src/environments/environment.ts
```
change port on: 

    apiUrl: 'https://localhost:44342/api'
```

---

## Business Rules

- Task title is required  
- Task must have an assigned user  
- Task cannot transition directly from Pending → Done  

---

## Validations

Implemented using FluentValidation:
- Input validation at application layer  
- Domain invariants enforced in entities  

---

## JSON Support in SQL Server

Tasks include an AdditionalData column (NVARCHAR(MAX)) to store JSON.

### Example JSON

{
  "priority": "High",
  "estimatedDate": "2026-05-01",
  "tags": ["backend", "urgent"]
}

---

## JSON Validation (SQL)

ALTER TABLE Tasks  
ADD CONSTRAINT CK_Tasks_AdditionalData_IsJson  
CHECK (  
    AdditionalData IS NULL  
    OR AdditionalData = ''  
    OR ISJSON(AdditionalData) = 1  
);

---

## JSON Queries

### Get a JSON field

SELECT  
    Id,  
    Title,  
    JSON_VALUE(AdditionalData, '$.priority') AS Priority  
FROM Tasks;

---

### Filter by JSON value

SELECT *  
FROM Tasks  
WHERE JSON_VALUE(AdditionalData, '$.priority') = 'High';

---

### Extract JSON object

SELECT  
    JSON_QUERY(AdditionalData, '$.metadata') AS Metadata  
FROM Tasks;

---

### Work with arrays

SELECT *  
FROM Tasks  
CROSS APPLY OPENJSON(AdditionalData, '$.tags');

---

### Update JSON field

UPDATE Tasks  
SET AdditionalData = JSON_MODIFY(AdditionalData, '$.priority', 'Low')  
WHERE Id = 'YOUR_ID';

---

## Technical Highlights

- Clean Architecture  
- CQRS with MediatR  
- FluentValidation pipeline  
- Global exception handling  
- SQL Server JSON functions (ISJSON, JSON_VALUE, JSON_QUERY, OPENJSON)  