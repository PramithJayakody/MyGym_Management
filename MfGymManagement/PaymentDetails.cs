using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace MfGymManagement // <-- ඔයාගෙ project එකේ නම
{
    public partial class PaymentDetails : Form
    {
        // Member ගෙ ID එක තියාගන්න
        private int currentMemberId;
        private SQLiteConnection dbConnection;
        private string dbFileName = "gym_database.sqlite";

        // Form එක හදනකොට ID එකයි Name එකයි ඉල්ලන අලුත් Constructor එකක්
        public PaymentDetails(int memberId, string memberName)
        {
            InitializeComponent();
            dbConnection = new SQLiteConnection($"Data Source={dbFileName};Version=3;");

            // 'ViewMembers' form එකෙන් එවපු ID එකයි Name එකයි පාවිච්චි කරනවා
            this.currentMemberId = memberId;
            lblMemberName.Text = "Payments for: " + memberName;
        }

        // Form එක Load වෙනකොට Payment History එක load කරනවා
        private void PaymentDetails_Load(object sender, EventArgs e)
        {
            LoadPaymentHistory();

            // ComboBox එකට default value එකක් දානවා
            cmbNewFeesMode.SelectedIndex = 0; // "Monthly"

            txtNewReceiptNo.ReadOnly = true;
            txtNewReceiptNo.Text = "(Will be generated after save)";
        }

        // Payment History එක load කරන function එක
        private void LoadPaymentHistory()
        {
            try
            {
                dbConnection.Open();
                // 'Payments' table එකෙන් අදාල member ගෙ ID එක තියෙන payments විතරක් select කරනවා
                string sql = "SELECT PaymentID AS 'Receipt No', PaymentDate, FeesMode, Amount FROM Payments WHERE MemberID = @MemberID ORDER BY PaymentDate DESC";

                SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@MemberID", this.currentMemberId);

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dgvPaymentHistory.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payment history: " + ex.Message);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // "Save Payment" Button එක click කරාම (ඔයාගෙ ඉල්ලීම 5)
        private void btnSavePayment_Click(object sender, EventArgs e)
        {
            
            string feesMode = cmbNewFeesMode.SelectedItem.ToString();

            double amount = 0;
            if (!double.TryParse(txtNewAmount.Text, out amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                dbConnection.Open();

                string sql = @"INSERT INTO Payments (MemberID, PaymentDate, FeesMode, Amount) 
                               VALUES (@MemberID, @PaymentDate, @FeesMode, @Amount)";

                SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@MemberID", this.currentMemberId);
                
                cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now.ToString("yyyy-MM-dd")); // අද date එක
                cmd.Parameters.AddWithValue("@FeesMode", feesMode);
                cmd.Parameters.AddWithValue("@Amount", amount);

                cmd.ExecuteNonQuery();

                long lastPaymentId = dbConnection.LastInsertRowId;

                MessageBox.Show($"Payment added successfully!\nNew Receipt No: {lastPaymentId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Form එක clear කරලා, History එක refresh කරනවා
                txtNewReceiptNo.Text = "(Will be generated after save)";
                txtNewAmount.Text = "";
                LoadPaymentHistory(); // අලුත් payment එක list එකේ පෙන්නන්න
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving payment: " + ex.Message);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // "Close" Button එක
        private void btnClosePaymentForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}