using LEAVETYPESETUPFORM.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LEAVETYPESETUPFORM.form
{
    public partial class LeaveTypeSetup : Form
    {
        private int validatedMaxDays = 0 ;
        private int CarryFwdNum ;
        private string newID = "";

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        LeaveTypeController controller = new LeaveTypeController();

        public LeaveTypeSetup()
        {
            InitializeComponent();
         
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
        }

        private void LeaveTypeSetup_Load(object sender, EventArgs e)
        {
            // string connStr = "Data Source=localhost;Initial Catalog=JOSHYTESTIMONYdatadb;Integrated Security=True;";
            workflow_comboBox.Items.AddRange(new String[] { "SUPERVISOR", "MANAGER", "HR", "ADMINISTRATOR" });
            genderRest_comboBox.Items.AddRange(new String[] { "MALE ", " FEMALE ", "OTHER" });
            save_leavetype_form.Enabled = true;

            this.WindowState = FormWindowState.Normal;
        }

        
        private void minimize_fom_btn_Click(object sender, EventArgs e)
        {
             this.WindowState = FormWindowState.Minimized;
        }

        private void maximize_form_btn_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void close_form_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void new_leaveform_btn_Click(object sender, EventArgs e)
        {

            string connString = "Data Source=localhost;Initial Catalog=JOSHYTESTIMONYdatadb;Integrated Security=True;";


            newID = controller.generateCode();
            leaveId_txtbox.Text = newID;
            RELOAD_BTN.Visible = false;
            edit_leaveform_btn.Visible = false; 
            save_leavetype_form.Enabled = false;
            save_leavetype_form.Text = "SAVE";
            leaveName_textBox.Focus();

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
        }


      /* public void changeButtonViscibility(bool newbtnsts, bool savebtnsts, bool editbtnsts, bool printbtnsts, bool deletebtnsts)
        {

            new_leaveform_btn.Visible = newbtnsts;
            save_leavetype_form.Visible = savebtnsts;
            edit_leaveform_btn.Visible = editbtnsts;
            print_button.Visible = printbtnsts;
            cancel_leaveform_btn.Visible = deletebtnsts;
        } */
        private void cancel_leaveform_btn_Click(object sender, EventArgs e)
        {

          //  changeButtonViscibility(true, true, true, true, true);

            leaveId_txtbox.Text = "";
            leaveName_textBox.Text = "";
            description_txtbox.Text = "";
            maxdays_textBox.Text = "";
            genderRest_comboBox.SelectedIndex = -1;
            workflow_comboBox.SelectedIndex = -1;
            carryfwd_textBox.Text = " ";
            carryfwd_checkBox.Checked = false;
            carryfwd_textBox.Enabled = false;
            paidLeave_checkBox.Checked = false;
            requiresApproval_checkBox.Checked = false;
            voidedBy_txtBox.Text = " ";
            active_checkBox.Checked = true;

            save_leavetype_form.Enabled = true;
            save_leavetype_form.Text = "SAVE";
        }

        

        /*
        private void leaveId_txtbox_TextChanged(object sender, EventArgs e)
        private void leaveName_textBox_TextChanged(object sender, EventArgs e)
        {
            string leavename  = leaveName_textBox.Text ;
            if (string.IsNullOrWhiteSpace(leavename))
            {
                MessageBox.Show("Leave Type Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                leaveName_textBox.Focus();
            }
            else
            {
                 save_leavetype_form.Enabled = true;
            }
        }

        private void description_txtbox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(description_txtbox.Text))
            {
                MessageBox.Show("Description cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                description_txtbox.Focus();
            }
            else
            {
                save_leavetype_form.Enabled = true;
            }
        }

        private void maxdays_textBox_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(maxdays_textBox.Text, out int  maxDays))
            { 
                MessageBox.Show("Max Days Per Year must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                maxdays_textBox.Focus();

            }
            else
            {
                validatedMaxDays = maxDays;
                save_leavetype_form.Enabled = true;
            }
        }

        private void carryfwd_textBox_TextChanged(object sender, EventArgs e)
        {

            if (!carryfwd_checkBox.Checked)
                {
                carryfwd_textBox.Enabled = false;
            }
            else
                {
                    carryfwd_textBox.Enabled = true;
                if (!int.TryParse(carryfwd_textBox.Text, out int carryfwdNum))
                    {
                        MessageBox.Show("Carry Forward Limit must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        carryfwd_textBox.Focus();
                    }
                    else
                    {
                      CarryFwdNum = carryfwdNum;
                    save_leavetype_form.Enabled = true;
                    }
                
            }
        }

        private void genderRest_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
          

            if (genderRest_comboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose ", "validation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                save_leavetype_form.Enabled = true;
            }
        }

        private void workflow_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
          

            if (workflow_comboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose a workflow.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                save_leavetype_form.Enabled = true;
            }
        }

        private void createdBy_textBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(createdBy_textBox.Text))
            {
                MessageBox.Show("Created By cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                createdBy_textBox.Focus();
            }
            else
            {
                save_leavetype_form.Enabled = true;
            }
        }

        private void voidedOn_txtBox_TextChanged(object sender, EventArgs e)
        {
            if (Voided_checkBox.Checked && string.IsNullOrWhiteSpace(voidedBy_txtBox.Text))
            {
                MessageBox.Show("Voided On cannot be empty when Voided is checked.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                voidedBy_txtBox.Focus();
            }
            else
            {
                save_leavetype_form.Enabled = true;
            }
        }

        private void voidedBy_dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!Voided_checkBox.Checked) { 
                voidedOn.Enabled = false;
            }
            else
            {
                voidedOn.Enabled = true;
            }
        }

        private void carryfwd_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (carryfwd_checkBox.Checked)
            {
                save_leavetype_form.Enabled = true;
            }
            else
            {
                save_leavetype_form.Enabled = false;
            }
        }

        private void paidLeave_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (paidLeave_checkBox.Checked)
            {
                save_leavetype_form.Enabled = true;
            }
            else
            {
                save_leavetype_form.Enabled = false;
            }
        }

        private void active_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (active_checkBox.Checked)
            {
                save_leavetype_form.Enabled = true;
            }
            else
            {
                save_leavetype_form.Enabled = false;
            }
        }

        private void requiresApproval_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (requiresApproval_checkBox.Checked)
            {
                save_leavetype_form.Enabled = true;
            }
            else
            {
                save_leavetype_form.Enabled = false;
            }
        }
*/




        /*
        bool isValid = !String.IsNullOrWhiteSpace(leaveName_textBox.Text) &&
            !String.IsNullOrWhiteSpace(description_txtbox.Text) &&
            int.TryParse(maxdays_textBox.Text, out validatedMaxDays) &&
            !string.IsNullOrWhiteSpace(createdBy_textBox.Text) &&
             workflow_comboBox.SelectedIndex != -1 &&
             genderRest_comboBox.SelectedIndex != -1 &&
             !String.IsNullOrWhiteSpace(createdBy_textBox.Text);
        if (!carryfwd_checkBox.Checked)
        {

           // isValid &= int.TryParse(carryfwd_textBox.Text, out CarryFwdNum);
        }
        if (Voided_checkBox.Checked)
        {
            isValid &= !String.IsNullOrWhiteSpace(voidedBy_txtBox.Text);

        } */

        private bool ValidateInput()
        {
            bool isValid = true;

            // Leave Name
            if (string.IsNullOrWhiteSpace(leaveName_textBox.Text))
            {
                leaveName_textBox.BackColor = Color.MistyRose;
                isValid = false;
            }
            else
            {
                leaveName_textBox.BackColor = Color.White;
            }

            // Description
            if (string.IsNullOrWhiteSpace(description_txtbox.Text))
            {
                description_txtbox.BackColor = Color.MistyRose;
                isValid = false;
            }
            else
            {
                description_txtbox.BackColor = Color.White;
            }

            // Max Days
            if (!int.TryParse(maxdays_textBox.Text, out validatedMaxDays))
            {
                maxdays_textBox.BackColor = Color.MistyRose;
                isValid = false;
            }
            else
            {
                maxdays_textBox.BackColor = Color.White;
            }

            // Carry Forward (only if checked)
            if (carryfwd_checkBox.Checked)
            {
                carryfwd_textBox.Enabled = true;
                if (!int.TryParse(carryfwd_textBox.Text, out CarryFwdNum))
                {
                    carryfwd_textBox.BackColor = Color.MistyRose;
                    isValid = false;
                }
                else
                {
                    carryfwd_textBox.BackColor = Color.White;
                }
            }
            else
            {
                carryfwd_textBox.Enabled = false;
                carryfwd_textBox.BackColor = Color.White;
            }

            // Workflow
            if (workflow_comboBox.SelectedIndex == -1)
            {
                workflow_comboBox.BackColor = Color.MistyRose;
                isValid = false;
            }
            else
            {
                workflow_comboBox.BackColor = Color.White;
            }

            // Gender Restriction
            if (genderRest_comboBox.SelectedIndex == -1)
            {
                genderRest_comboBox.BackColor = Color.MistyRose;
                isValid = false;
            }
            else
            {
                genderRest_comboBox.BackColor = Color.White;
            }

            // Created By
            if (string.IsNullOrWhiteSpace(createdBy_textBox.Text))
            {
                createdBy_textBox.BackColor = Color.MistyRose;
                isValid = false;
            }
            else
            {
                createdBy_textBox.BackColor = Color.White;
            }

            // Voided
            if (Voided_checkBox.Checked)
            {
                voidedBy_txtBox.Enabled = true;
                voidedOn.Enabled = true;
                if (string.IsNullOrWhiteSpace(voidedBy_txtBox.Text))
                {
                    voidedBy_txtBox.BackColor = Color.MistyRose;
                    isValid = false;
                }
                else
                {
                    voidedBy_txtBox.BackColor = Color.White;
                }
            }
            else
            {
                voidedBy_txtBox.Enabled = false;
                voidedOn.Enabled = false;
                voidedBy_txtBox.BackColor = Color.White;
            }

            save_leavetype_form.Enabled = isValid;
            return isValid;
        }

        private void save_leavetype_form_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                MessageBox.Show("Please fill all required fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string leaveID = leaveId_txtbox.Text.Trim();

            if (save_leavetype_form.Text == "UPDATE" && controller.DoesLeavetypeExist(leaveID))
            {
                bool success = controller.UpdateLeaveType(
                    leaveID,
                    leaveName_textBox.Text.Trim(),
                    description_txtbox.Text.Trim(),
                    validatedMaxDays,
                    carryfwd_checkBox.Checked,
                    CarryFwdNum,
                    paidLeave_checkBox.Checked,
                    requiresApproval_checkBox.Checked,
                    workflow_comboBox.SelectedItem?.ToString(),
                    genderRest_comboBox.SelectedItem?.ToString(),
                    active_checkBox.Checked,
                    createdBy_textBox.Text.Trim(),
                    Voided_checkBox.Checked,
                    string.IsNullOrWhiteSpace(voidedBy_txtBox.Text) ? null : voidedBy_txtBox.Text.Trim(),
                    voidedOn.Value
                );

                if (success)
                {
                    MessageBox.Show("Leave type updated successfully.");
                    displayPrevious_btn.PerformClick(); // Refresh DataGridView
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Update failed.");
                    save_leavetype_form.Text = "SAVE";
                }
            }
            else
            {
                // Save as NEW
                controller.saveLeaveTypeForm(
                   leaveId_txtbox.Text,
                    leaveName_textBox.Text.Trim(),
                    description_txtbox.Text.Trim(),
                    validatedMaxDays,
                    carryfwd_checkBox.Checked,
                    CarryFwdNum,
                    paidLeave_checkBox.Checked,
                    requiresApproval_checkBox.Checked,
                    workflow_comboBox.SelectedItem?.ToString(),
                    genderRest_comboBox.SelectedItem?.ToString(),
                    active_checkBox.Checked,
                    createdBy_textBox.Text.Trim(),
                    DateTime.Now,
                    Voided_checkBox.Checked,
                    string.IsNullOrWhiteSpace(voidedBy_txtBox.Text) ? null : voidedBy_txtBox.Text.Trim(),
                    voidedOn.Value
                );

                MessageBox.Show("New leave type saved.");
                //displayPrevious_btn.PerformClick();
                ResetForm();
            }
        }





        private void displayPrevious_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource =  controller.displayLeaveTypeData();
            RELOAD_BTN.Visible = true;
            edit_leaveform_btn.Visible = true;
            ResetForm();

        }

        private void leaveId_txtbox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void leaveName_textBox_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void description_txtbox_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void maxdays_textBox_TextChanged(object sender, EventArgs e)
        {
             ValidateInput();
        }

        private void carryfwd_textBox_TextChanged(object sender, EventArgs e)
        {
             ValidateInput();
        }

        private void genderRest_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
             ValidateInput();
        }

        private void workflow_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void carryfwd_checkBox_CheckedChanged(object sender, EventArgs e)
        {
             ValidateInput();
        }

        private void paidLeave_checkBox_CheckedChanged(object sender, EventArgs e)
        {
             ValidateInput();
        }

        private void active_checkBox_CheckedChanged(object sender, EventArgs e)
        {
           ValidateInput();
        }

        private void requiresApproval_checkBox_CheckedChanged(object sender, EventArgs e)
        {
             ValidateInput();
        }

        private void createdBy_textBox_TextChanged(object sender, EventArgs e)
        {
             ValidateInput();
        }

        private void Voided_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void voidedBy_txtBox_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                leaveId_txtbox.Text = row.Cells["LeaveTypeID"].Value?.ToString() ?? "";
                leaveName_textBox.Text = row.Cells["LeaveTypeName"].Value?.ToString() ?? "";
                description_txtbox.Text = row.Cells["Description"].Value?.ToString() ?? "";
                maxdays_textBox.Text = row.Cells["MaxDaysPerYear"].Value?.ToString() ?? "";
                carryfwd_checkBox.Checked = Convert.ToBoolean(row.Cells["CarryForwardAllowed"].Value);
                carryfwd_textBox.Text = row.Cells["CarryForwardLimit"].Value?.ToString() ?? "";

                paidLeave_checkBox.Checked = Convert.ToBoolean(row.Cells["PaidLeave"].Value);
                requiresApproval_checkBox.Checked = Convert.ToBoolean(row.Cells["RequiresApproval"].Value);

                workflow_comboBox.SelectedItem = row.Cells["WorkflowCode"].Value?.ToString() ?? "";
                genderRest_comboBox.SelectedItem = row.Cells["GenderRestriction"].Value?.ToString() ?? "";
                active_checkBox.Checked = Convert.ToBoolean(row.Cells["Active"].Value);
                createdBy_textBox.Text = row.Cells["CreatedBy"].Value?.ToString() ?? "";
                createdOn_dateTimePicker.Value = Convert.ToDateTime(row.Cells["VoidedOn"].Value);
                Voided_checkBox.Checked = Convert.ToBoolean(row.Cells["Voided"].Value);
                voidedBy_txtBox.Text = row.Cells["VoidedBy"].Value?.ToString() ?? "";
                voidedOn.Value = Convert.ToDateTime(row.Cells["VoidedOn"].Value);

                // Set button to UPDATE mode
                save_leavetype_form.Text = "UPDATE";
                save_leavetype_form.Enabled = true;
            }

        }

        private void ResetForm()
        {
            leaveId_txtbox.Text = "";
            leaveName_textBox.Text = "";
            description_txtbox.Text = "";
            maxdays_textBox.Text = "";
            createdBy_textBox.Text = "";
            carryfwd_textBox.Text = "";
            carryfwd_checkBox.Checked = false;
            paidLeave_checkBox.Checked = false;
            requiresApproval_checkBox.Checked = false;
            workflow_comboBox.SelectedIndex = -1;
            genderRest_comboBox.SelectedIndex = -1;
            active_checkBox.Checked = true;
            Voided_checkBox.Checked = false;
            voidedBy_txtBox.Text = "";
            save_leavetype_form.Text = "SAVE";
        }

        private void RELOAD_BTN_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            ResetForm();
            LeaveTypeSetup_Load(this, EventArgs.Empty);
            RELOAD_BTN.Visible = false;
        }

        private void edit_leaveform_btn_Click(object sender, EventArgs e)
        {
           
      
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this leave type?",
                "Delete Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                controller.DeleteLeaveType(leaveId_txtbox.Text);
                ResetForm();
                MessageBox.Show("Leave type deleted.");
                edit_leaveform_btn.Visible = false;
                displayPrevious_btn_Click(sender, e);
                edit_leaveform_btn.Visible = false;

                //LoadLeaveTypesIntoGrid();
            }
            else
            {
               
                MessageBox.Show("Deletion canceled.");
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public void LoadLeaveTypesIntoGrid() 
        {
            dataGridView1.DataSource = controller.displayLeaveTypeData();
        }
    }
}






