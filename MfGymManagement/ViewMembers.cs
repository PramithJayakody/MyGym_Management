using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MfGymManagement // <-- ඔයාගෙ project එකේ නම
{
    public partial class ViewMembers : Form
    {
        private string dbFileName = "gym_database.sqlite";
        private SQLiteConnection dbConnection;
        private SQLiteDataAdapter dataAdapter;
        private DataTable dataTable;

        public ViewMembers()
        {
            InitializeComponent();
            dbConnection = new SQLiteConnection($"Data Source={dbFileName};Version=3;");
        }

        // Form එක Load වෙනකොට
        private void ViewMembers_Load(object sender, EventArgs e)
        {
            LoadAllMembers();
        }

        // Database එකෙන් Members ලා Load කරන Function එක
        private void LoadAllMembers()
        {
            try
            {
                dbConnection.Open();
                string sql = "SELECT ID, Name, ContactNo, Address, AdmissionDate, Photo FROM Members";
                dataAdapter = new SQLiteDataAdapter(sql, dbConnection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dgvMembers.DataSource = dataTable;

                if (dgvMembers.Columns.Contains("Photo"))
                {
                    dgvMembers.Columns["Photo"].Visible = false;
                }

                // අලුත් Labels ටිකත් clear කරනවා
                ClearSelectionDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading members: " + ex.Message);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // "Search" Button එක
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text;
                (dgvMembers.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("Name LIKE '%{0}%' OR ContactNo LIKE '%{0}%'", searchText);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message);
            }
        }

        // "Show All" Button එක
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            (dgvMembers.DataSource as DataTable).DefaultView.RowFilter = string.Empty;

            // අලුත් Labels ටිකත් clear කරනවා
            ClearSelectionDetails();
        }

        // "Close" Button එක
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // **** මේ Function එක සම්පූර්ණයෙන්ම වෙනස් වෙලා තියෙන්නෙ ****
        // List එකේ row එකක් select කරාම
        private void dgvMembers_SelectionChanged(object sender, EventArgs e)
        {
            // Select කරපු row එකක් තියෙනවද බලනවා
            if (dgvMembers.CurrentRow == null || dgvMembers.CurrentRow.DataBoundItem == null)
            {
                ClearSelectionDetails();
                return;
            }

            try
            {
                // --- 1. Photo එක Load කිරීම (පරණ code එක) ---
                DataRowView selectedRow = dgvMembers.CurrentRow.DataBoundItem as DataRowView;
                if (selectedRow.Row["Photo"] != DBNull.Value)
                {
                    byte[] photoData = (byte[])selectedRow.Row["Photo"];
                    using (MemoryStream ms = new MemoryStream(photoData))
                    {
                        picSelectedMember.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    picSelectedMember.Image = null;
                }

                // --- 2. අලුත් Code එක: Due Date එක Calculate කිරීම ---
                int memberId = Convert.ToInt32(selectedRow.Row["ID"]);

                // අදාල member ගෙ අන්තිම payment එක හොයනවා
                string sqlPayment = "SELECT PaymentDate, FeesMode FROM Payments WHERE MemberID = @MemberID ORDER BY PaymentDate DESC LIMIT 1";

                // Connection එක close වෙලා නම් open කරනවා
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();

                SQLiteCommand cmd = new SQLiteCommand(sqlPayment, dbConnection);
                cmd.Parameters.AddWithValue("@MemberID", memberId);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // Payment එකක් හම්බවුණා නම්
                    {
                        DateTime lastPaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                        string feesMode = reader["FeesMode"].ToString();

                        // ඊළඟ date එක calculate කරනවා
                        DateTime nextDueDate = CalculateNextDueDate(lastPaymentDate, feesMode);

                        // දවස් ගාණ calculate කරනවා
                        TimeSpan remainingTime = nextDueDate.Date - DateTime.Now.Date;
                        int daysRemaining = (int)remainingTime.TotalDays;

                        // Labels වල පෙන්නනවා
                        lblDueDate.Text = "Next Due Date: " + nextDueDate.ToString("yyyy-MM-dd");

                        if (daysRemaining < 0)
                        {
                            lblDaysRemaining.Text = $"Status: EXPIRED ({Math.Abs(daysRemaining)} days ago)";
                            lblDaysRemaining.ForeColor = Color.Red;
                        }
                        else if (daysRemaining == 0)
                        {
                            lblDaysRemaining.Text = "Status: DUE TODAY";
                            lblDaysRemaining.ForeColor = Color.OrangeRed;
                        }
                        else if (daysRemaining <= 7)
                        {
                            lblDaysRemaining.Text = $"Status: Due in {daysRemaining} days";
                            lblDaysRemaining.ForeColor = Color.Orange;
                        }
                        else
                        {
                            lblDaysRemaining.Text = $"Status: Active ({daysRemaining} days left)";
                            lblDaysRemaining.ForeColor = Color.Green;
                        }
                    }
                    else // කිසිම payment එකක් හම්බවුණේ නැත්නම්
                    {
                        lblDueDate.Text = "Next Due Date: N/A";
                        lblDaysRemaining.Text = "Status: No payments found";
                        lblDaysRemaining.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                // Error එකක් ආවොත් labels clear කරනවා
                lblDueDate.Text = "Error loading details.";
                lblDaysRemaining.Text = "";
                picSelectedMember.Image = null;
                Console.WriteLine("Selection Changed Error: " + ex.Message);
            }
            finally
            {
                if (dbConnection.State == ConnectionState.Open) dbConnection.Close();
            }
        }

        // ***** අලුත්ම Helper Function එක *****
        // ඊළඟ date එක calculate කරන්න
        private DateTime CalculateNextDueDate(DateTime lastPaymentDate, string feesMode)
        {
            switch (feesMode)
            {
                case "Monthly":
                    return lastPaymentDate.AddMonths(1);
                case "Quarterly":
                    return lastPaymentDate.AddMonths(3);
                case "Half Yearly":
                    return lastPaymentDate.AddMonths(6);
                case "Yearly":
                    return lastPaymentDate.AddYears(1);
                default:
                    return lastPaymentDate.AddMonths(1); // Default එක Monthly දානවා
            }
        }

        // ***** අලුත්ම Helper Function එක *****
        // Photo එකයි අලුත් labels ටිකයි clear කරන්න
        private void ClearSelectionDetails()
        {
            picSelectedMember.Image = null;
            lblDueDate.Text = "Next Due Date:";
            lblDaysRemaining.Text = "Status:";
            lblDaysRemaining.ForeColor = Color.Black;
        }

        // Member කෙනෙක්ව Double-Click කරාම (වෙනසක් නෑ)
        private void dgvMembers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataRowView selectedRow = dgvMembers.Rows[e.RowIndex].DataBoundItem as DataRowView;
                    int memberId = Convert.ToInt32(selectedRow.Row["ID"]);
                    string memberName = selectedRow.Row["Name"].ToString();

                    PaymentDetails paymentForm = new PaymentDetails(memberId, memberName);
                    paymentForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open payment details: " + ex.Message);
                }
            }
        }

        private void btnEditMember_Click(object sender, EventArgs e)
        {
            // 1. List එකෙන් row එකක් select කරලද බලනවා
            if (dgvMembers.CurrentRow == null || dgvMembers.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show("Please select a member from the list to edit.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. Select කරපු member ගෙ ID එක අරගන්නවා
            DataRowView selectedRow = dgvMembers.CurrentRow.DataBoundItem as DataRowView;
            int memberId = Convert.ToInt32(selectedRow.Row["ID"]);

            // 3. Form1 එක "Edit Mode" එකෙන් open කරනවා (ID එක pass කරලා)
            Form1 editForm = new Form1(memberId);
            editForm.ShowDialog(); // .ShowDialog() දාන්නෙ edit කරලා ඉවරවෙනකල් ViewMembers form එක click කරන්න බැරිවෙන්න

            // 4. Edit form එක close කලාට පස්සෙ, List එක refresh කරනවා
            LoadAllMembers();
        }
    }
}