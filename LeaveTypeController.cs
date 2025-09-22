using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LEAVETYPESETUPFORM.form; // Assuming this is the namespace where your form is defined

namespace LEAVETYPESETUPFORM.Controller
{
    public class LeaveTypeController
    {
        public LeaveTypeController()
        {
            EnsureTableExists();
        }
        //private String connectionString = "Data Source=localhost;Initial Catalog=JOSHYTESTIMONYdatadb;Integrated Security=True;";
        private readonly string connectionString = 
            ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        private void EnsureTableExists()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='EmployeeLeaveTypes' AND xtype='U')
            BEGIN
                CREATE TABLE EmployeeLeaveTypes (
                    LeaveTypeID NVARCHAR(50) PRIMARY KEY,
                    LeaveTypeName NVARCHAR(100) NOT NULL,
                    Description NVARCHAR(255) NULL,
                    MaxDaysPerYear INT NOT NULL,
                    CarryForwardAllowed BIT NOT NULL,
                    CarryForwardLimit INT NULL,
                    PaidLeave BIT NOT NULL,
                    RequiresApproval BIT NOT NULL,
                    WorkflowCode NVARCHAR(50) NULL,
                    GenderRestriction NVARCHAR(20) NULL,
                    Active BIT NOT NULL DEFAULT 1,
                    CreatedBy NVARCHAR(50) NULL,
                    CreatedOn DATETIME DEFAULT GETDATE(),
                    Voided BIT NOT NULL DEFAULT 0,
                    VoidedBy NVARCHAR(50) NULL,
                    VoidedOn DATETIME NULL,
                    ImageData VARBINARY(MAX) NULL,
                    FileData VARBINARY(MAX) NULL
                )
            END";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string generateCode()
        {
            string prefix = "L";
            DateTime now = DateTime.Now;

            string monthName = now.ToString("MMMM")[0].ToString(); // First letter of month
            string dayName = now.ToString("dddd")[0].ToString();   // First letter of day
            string date = now.ToString("dd");
            string suffix = "S";
            string baseID = $"{prefix}{monthName}{dayName}{date}{suffix}";
            //return baseID;

            string lastID = null;

            int nextNum = 1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = " SELECT MAX(LeaveTypeID) FROM EmployeeLeaveTypes WHERE LeaveTypeID LIKE @prefix + '%'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@prefix", baseID);

                    lastID = cmd.ExecuteScalar()?.ToString();
                }

                /*    if (!String.IsNullOrEmpty(lastID) && lastID.Length >= baseID.Length + 4)
                    {
                        string numPart = lastID.Substring(baseID.Length);
                        if (int.TryParse(numPart, out int lastnum))
                            {
                            nextNum = lastnum + 1;
                        }

                    } */
                if (!string.IsNullOrEmpty(lastID) && lastID.StartsWith(baseID))
                {
                    string numberPart = lastID.Substring(baseID.Length);
                    if (int.TryParse(numberPart, out int lastNum))
                        nextNum = lastNum + 1;
                }
            }

            return baseID + nextNum.ToString("D3");

        }
        /* public bool SaveNewLeaveTypeID(string leaveTypeID)
         {
             using (SqlConnection conn = new SqlConnection(connectionString))
             {
                 conn.Open();
                 string query = "INSERT INTO EmployeeLeaveTypes (LeaveTypeID, LeaveTypeName, Description, MaxDaysPerYear, CarryForwardAllowed, CarryForwardLimit, PaidLeave, RequiresApproval) " +
                                "VALUES (@ID, 'New Type', 'Auto-generated', 0, 0, NULL, 0, 0)";
                 SqlCommand cmd = new SqlCommand(query, conn);
                 cmd.Parameters.AddWithValue("@ID", leaveTypeID);
                 return cmd.ExecuteNonQuery() > 0;
             }
         } */

        public void saveLeaveTypeForm(
               string leaveID,
               string leaveTypeName,
               string leaveTypeDescription,
               int maxDays  ,
               bool carryForward,
               int? carryForwardLimit,                     
               bool paidLeave,
               bool requireApproval,
               string workflowCode = null,                 
               string genderRestriction = null,            
               bool active = true,
               string createdBy = "system",
               DateTime? createdOn = null,                
               bool voided = false,
               string voidedBy = null,
               DateTime? voidedOn = null)
                {
            EnsureTableExists();
            string saveQuery = @"
                INSERT INTO EMPLOYEELEAVETYPES (
                    LeaveTypeID, LeaveTypeName, Description, MaxDaysPerYear, CarryForwardAllowed, CarryForwardLimit,
                    PaidLeave, RequiresApproval, WorkflowCode, GenderRestriction, Active, CreatedBy, CreatedOn,
                    Voided, VoidedBy, VoidedOn
                ) VALUES (
                    @LeaveTypeID, @LeaveTypeName, @Description, @MaxDaysPerYear, @CarryForwardAllowed, @CarryForwardLimit,
                    @PaidLeave, @RequiresApproval, @WorkflowCode, @GenderRestriction, @Active, @CreatedBy, @CreatedOn,
                    @Voided, @VoidedBy, @VoidedOn
                )";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(saveQuery, conn);

                        cmd.Parameters.AddWithValue("@LeaveTypeID", leaveID);
                        cmd.Parameters.AddWithValue("@LeaveTypeName", leaveTypeName);
                        cmd.Parameters.AddWithValue("@Description", (object)leaveTypeDescription ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaxDaysPerYear", maxDays);
                        cmd.Parameters.AddWithValue("@CarryForwardAllowed", carryForward);
                        cmd.Parameters.AddWithValue("@CarryForwardLimit", (object)carryForwardLimit ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PaidLeave", paidLeave);
                        cmd.Parameters.AddWithValue("@RequiresApproval", requireApproval);
                        cmd.Parameters.AddWithValue("@WorkflowCode", (object)workflowCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@GenderRestriction", (object)genderRestriction ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Active", active);
                        cmd.Parameters.AddWithValue("@CreatedBy", (object) createdBy ?? "SYSTEM");
                        cmd.Parameters.AddWithValue("@CreatedOn", (object)createdOn ?? DateTime.Now);
                        cmd.Parameters.AddWithValue("@Voided", voided);
                        cmd.Parameters.AddWithValue("@VoidedBy", (object)voidedBy ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@VoidedOn", (object)voidedOn ?? DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
        }

        internal void saveLeaveTypeForm(string leaveID, string leaveTypeName, string leaveTypeDescription, int? maxDays, bool carryForward, int? carryForwardLimit, bool paidLeave, bool requireApproval, string workflowCode, string genderRestriction, bool active, string createdBy, bool voided, string voidedBy, DateTime voidedOn)
        {
            throw new NotImplementedException();
        }

        public DataTable displayLeaveTypeData()
        {
            DataTable dataTable = new DataTable();
            string sqlQuery = " SELECT * FROM EmployeeLeaveTypes ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(sqlQuery, conn);
                adpt.Fill(dataTable);
            }

            return dataTable;
        }

        public void DeleteLeaveTypeID ()
        {
            string query;
        }

        public bool DoesLeavetypeExist(String LeaveTypeID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM EmployeeLeaveTypes WHERE LeaveTypeID = @LeaveTypeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LeaveTypeID", LeaveTypeID);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public bool UpdateLeaveType(string leaveID, string name, string desc, int maxDays, bool carryFwd, int carryLimit,
                                        bool paidLeave, bool requiresApproval, string workflow, string gender, bool active,
                                        string createdBy, bool voided, string voidedBy, DateTime voidedOn)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"
                UPDATE EmployeeLeaveTypes
                SET 
                    LeaveTypeName = @name,
                    Description = @desc,
                    MaxDaysPerYear = @max,
                   CarryForwardAllowed = @cf,
                    CarryForwardLimit = @cfLimit,
                    PaidLeave = @paid,
                    RequiresApproval = @approval,
                    WorkflowCode = @workflow,
                    GenderRestriction = @gender,
                    Active = @active,
                    CreatedBy = @created,
                    Voided = @voided,
                    VoidedBy = @voidedBy,
                    VoidedOn = @voidedOn
                WHERE LeaveTypeID = @id", conn);

                    cmd.Parameters.AddWithValue("@id", leaveID);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@desc", desc);
                    cmd.Parameters.AddWithValue("@max", maxDays);
                    cmd.Parameters.AddWithValue("@cf", carryFwd);
                    cmd.Parameters.AddWithValue("@cfLimit", carryLimit);
                    cmd.Parameters.AddWithValue("@paid", paidLeave);
                    cmd.Parameters.AddWithValue("@approval", requiresApproval);
                    cmd.Parameters.AddWithValue("@workflow", workflow);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@active", active);
                    cmd.Parameters.AddWithValue("@created", createdBy);
                    cmd.Parameters.AddWithValue("@voided", voided);
                    cmd.Parameters.AddWithValue("@voidedBy", (object)voidedBy ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@voidedOn", voidedOn);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message);
                return false;
            }
        }
       
        public void DeleteLeaveType(string leaveType)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string squery = "DELETE FROM EMPLOYEELEAVETYPES WHERE LeaveTypeID = @LeaveTypeID";

                using (SqlCommand cmd = new SqlCommand(squery, conn))
                {
                    cmd.Parameters.AddWithValue("@LeaveTypeID", leaveType);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Leave type deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No leave type found with the specified ID.");
                    }
                }
            }
        }

    }
}



/*  public string GenerateLeaveTypeID()
  {
      string prefix = "L";
      DateTime now = DateTime.Now;
      string suffix = "S";
      char MonthChar = now.ToString("MMMM")[0];
      char DayChar = now.ToString("dddd")[0];
      string DateChar = now.ToString("dd");

      string baseID = $"{prefix}{MonthChar}{DayChar}{DateChar}{suffix}";

      string lastID = null;

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
          conn.Open();

          SqlCommand cmd = new SqlCommand(
              "SELECT MAX(LeaveTypeID) FROM EmployeeLeaveTypes WHERE LeaveTypeID LIKE @Prefix + '%'",
              conn
          );

          cmd.Parameters.AddWithValue("@Prefix", baseID); // adds the base id to the prefix 
          lastID = cmd.ExecuteScalar()?.ToString();
      }

      int nextNumber = 1;

      /*   if (!string.IsNullOrEmpty(lastID) && lastID.Length >= baseID.Length + 4)
         {
             string numberPart = lastID.Substring(baseID.Length);
             if (int.TryParse(numberPart, out int lastNum))
                 nextNumber = lastNum + 1;
         */
/*
      if (!string.IsNullOrEmpty(lastID) && lastID.StartsWith(baseID))
      {
          string numberPart = lastID.Substring(baseID.Length);
          if (int.TryParse(numberPart, out int lastNum))
              nextNumber = lastNum + 1;
      }

      return baseID + nextNumber.ToString("D3");

  } */