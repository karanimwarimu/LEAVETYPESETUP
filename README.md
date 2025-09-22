# Leave Type Setup Form

This is a **C# WinForms application** for managing leave types in an organization.  
It connects to a **SQL Server database** and allows you to:

- ✅ Create (Add) new leave types  
- ✏️ Update existing leave types  
- ❌ Delete leave types  
- 📋 Display (list) all leave types  
- 🔑 Generate a **unique ID** for each leave type automatically  

---

## ⚙️ Features
- Windows Forms interface (easy to use)
- CRUD operations (Create, Read, Update, Delete)
- SQL Server backend for data storage
- Auto-generated unique IDs
- Validation to prevent duplicate or invalid entries

---

## 🗄️ Database Setup
The application uses **SQL Server**.  
Create a table similar to this in your database:

```sql
CREATE TABLE LeaveTypes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    LeaveCode NVARCHAR(20) UNIQUE NOT NULL,
    LeaveName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    DateCreated DATETIME DEFAULT GETDATE()
);
