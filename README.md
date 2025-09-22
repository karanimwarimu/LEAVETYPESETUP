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
create a databse to test this feature , then 
Connect to the db using :

```sqlconnection
	<connectionStrings>
		<add name="MyDbConnection"
				 connectionString="Server=MACHINENAME/SERVERINSTANCE ;Database=DATABASENAME;Trusted_Connection=True;TrustServerCertificate=True"
				 providerName="System.Data.SqlClient" />
	</connectionStrings>

```
