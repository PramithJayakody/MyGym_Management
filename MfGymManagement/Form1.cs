using System;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

// 'InitializeComponent' error එක ආවොත්, 'public class Form1' කියන එක 'public partial class Form1' කරන්න
namespace MfGymManagement
{
    public partial class Form1 : Form
    {
        private string dbFileName = "gym_database.sqlite";
        private SQLiteConnection dbConnection;
        private byte[] memberPhotoData = null;

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            // පරණ database file එක delete කලාට පස්සෙ, මේකෙන් අලුතෙන් file එක හැදෙයි
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
            }

            dbConnection = new SQLiteConnection($"Data Source={dbFileName};Version=3;");

            try
            {
                dbConnection.Open();

                // 1. "Members" Table එක - ඔයාගෙ අලුත් design එකටම ගැලපෙන විදිහට
                // (AlternateNo, Batch අයින් කලා)
                string sqlMembers = @"CREATE TABLE IF NOT EXISTS Members (
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                Name TEXT,
                                Gender TEXT,
                                BirthDate TEXT,
                                Address TEXT,
                                Height TEXT,
                                Weight TEXT,
                                ContactNo TEXT,
                                AdmissionDate TEXT,
                                MemberType TEXT,
                                WorkoutGym INTEGER,
                                WorkoutCardio INTEGER,
                                WorkoutPersonalTrainer INTEGER,
                                Photo BLOB 
                             )";
                SQLiteCommand cmdMembers = new SQLiteCommand(sqlMembers, dbConnection);
                cmdMembers.ExecuteNonQuery();

                // 2. "Payments" Table එක (මේකෙ වෙනසක් නෑ)
                string sqlPayments = @"CREATE TABLE IF NOT EXISTS Payments (
                                PaymentID INTEGER PRIMARY KEY AUTOINCREMENT,
                                MemberID INTEGER,
                                ReceiptNo TEXT,
                                PaymentDate TEXT,
                                FeesMode TEXT,
                                Amount REAL,
                                FOREIGN KEY (MemberID) REFERENCES Members(ID)
                             )";
                SQLiteCommand cmdPayments = new SQLiteCommand(sqlPayments, dbConnection);
                cmdPayments.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // "Browse Photo" Button
        // මේක වැඩ කරන්න, ඔයාගෙ button එකේ (Name) එක 'btnBrowsePhoto' වෙන්න ඕන
        // ඒ වගේම, 'OpenFileDialog' control එකක් form එකට දාලා තියෙන්න ඕන
        private void btnBrowsePhoto_Click(object sender, EventArgs e)
        {
            // 'openFileDialog1' control එකක් form එකට add කරලා තියෙන්න ඕන
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string imagePath = openFileDialog1.FileName;
                    picMemberPhoto.Image = Image.FromFile(imagePath); // PictureBox එකේ (Name) එක 'picMemberPhoto'

                    using (Image image = Image.FromFile(imagePath))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            image.Save(ms, ImageFormat.Png);
                            memberPhotoData = ms.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }

        // "Save" Button
        // මේක වැඩ කරන්න, ඔයාගෙ button එකේ (Name) එක 'btnSave' වෙන්න ඕන
        private void btnSave_Click(object sender, EventArgs e)
        {
            // --- 1. Form එකෙන් Data ඔක්කොම එකතු කරගැනීම ---

            // Member Details
            string name = txtName.Text;
            string gender = rbMale.Checked ? "Male" : (rbFemale.Checked ? "Female" : "");
            string birthDate = dtpBirthDate.Text;
            string address = txtAddress.Text;
            string height = txtHeight.Text;
            string weight = txtWeight.Text;
            string contactNo = txtContactNo.Text;
            string admissionDate = dtpAdmissionDate.Text;
            string memberType = rbNewMember.Checked ? "New" : (rbOldMember.Checked ? "Old" : "");

            // Workout Checkboxes
            int workoutGym = chkGym.Checked ? 1 : 0; // 1 = True, 0 = False
            int workoutCardio = chkCardio.Checked ? 1 : 0;
            int workoutPersonalTrainer = chkPersonalTrainer.Checked ? 1 : 0;

            // Fees Details
            string receiptNo = txtReceiptNo.Text;
            string feesMode = "";
            if (rbMonthly.Checked) feesMode = "Monthly";
            else if (rbQuarterly.Checked) feesMode = "Quarterly";
            else if (rbHalfYearly.Checked) feesMode = "Half Yearly"; // Photo එකේ "Half Year" වුනාට, "Half Yearly" හොඳයි
            else if (rbYearly.Checked) feesMode = "Yearly";

            double feesAmount = 0;
            if (!double.TryParse(txtFeesAmount.Text, out feesAmount) && !string.IsNullOrWhiteSpace(txtFeesAmount.Text))
            {
                MessageBox.Show("Please enter a valid amount for fees.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- 2. Data Validation ---
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(contactNo))
            {
                MessageBox.Show("Please enter a Contact No.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- 3. Database එකට Save කිරීම ---
            long lastInsertedMemberId = -1;

            try
            {
                dbConnection.Open();

                // --- Part A: Member Details, "Members" table එකට save කරනවා ---
                string sqlMemberInsert = @"INSERT INTO Members (
                                            Name, Gender, BirthDate, Address, Height, Weight, ContactNo, 
                                            AdmissionDate, MemberType, WorkoutGym, WorkoutCardio, WorkoutPersonalTrainer, Photo
                                         ) VALUES (
                                            @Name, @Gender, @BirthDate, @Address, @Height, @Weight, @ContactNo, 
                                            @AdmissionDate, @MemberType, @WorkoutGym, @WorkoutCardio, @WorkoutPersonalTrainer, @Photo
                                         );
                                         SELECT last_insert_rowid();";

                SQLiteCommand cmdMember = new SQLiteCommand(sqlMemberInsert, dbConnection);
                cmdMember.Parameters.AddWithValue("@Name", name);
                cmdMember.Parameters.AddWithValue("@Gender", gender);
                cmdMember.Parameters.AddWithValue("@BirthDate", birthDate);
                cmdMember.Parameters.AddWithValue("@Address", address);
                cmdMember.Parameters.AddWithValue("@Height", height);
                cmdMember.Parameters.AddWithValue("@Weight", weight);
                cmdMember.Parameters.AddWithValue("@ContactNo", contactNo);
                cmdMember.Parameters.AddWithValue("@AdmissionDate", admissionDate);
                cmdMember.Parameters.AddWithValue("@MemberType", memberType);
                cmdMember.Parameters.AddWithValue("@WorkoutGym", workoutGym);
                cmdMember.Parameters.AddWithValue("@WorkoutCardio", workoutCardio);
                cmdMember.Parameters.AddWithValue("@WorkoutPersonalTrainer", workoutPersonalTrainer);

                if (memberPhotoData != null)
                {
                    cmdMember.Parameters.AddWithValue("@Photo", memberPhotoData);
                }
                else
                {
                    cmdMember.Parameters.AddWithValue("@Photo", DBNull.Value);
                }

                lastInsertedMemberId = (long)cmdMember.ExecuteScalar();

                // --- Part B: Fee එක, "Payments" table එකට save කරනවා ---
                if (feesAmount > 0 || !string.IsNullOrWhiteSpace(receiptNo))
                {
                    string sqlPaymentInsert = @"INSERT INTO Payments (
                                                    MemberID, ReceiptNo, PaymentDate, FeesMode, Amount
                                                ) VALUES (
                                                    @MemberID, @ReceiptNo, @PaymentDate, @FeesMode, @Amount
                                                )";

                    SQLiteCommand cmdPayment = new SQLiteCommand(sqlPaymentInsert, dbConnection);
                    cmdPayment.Parameters.AddWithValue("@MemberID", lastInsertedMemberId);
                    cmdPayment.Parameters.AddWithValue("@ReceiptNo", receiptNo);
                    cmdPayment.Parameters.AddWithValue("@PaymentDate", admissionDate);
                    cmdPayment.Parameters.AddWithValue("@FeesMode", feesMode);
                    cmdPayment.Parameters.AddWithValue("@Amount", feesAmount);

                    cmdPayment.ExecuteNonQuery();
                }

                MessageBox.Show("Member and Payment saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // "New" Button
        // මේක වැඩ කරන්න, ඔයාගෙ button එකේ (Name) එක 'btnNew' වෙන්න ඕන
        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        // "Exit" Button
        // මේක වැඩ කරන්න, ඔයාගෙ button එකේ (Name) එක 'btnExit' වෙන්න ඕන
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Form එක clear කරන function එක
        private void ClearForm()
        {
            // TextBoxes
            txtName.Text = "";
            txtAddress.Text = "";
            txtHeight.Text = "";
            txtWeight.Text = "";
            txtContactNo.Text = "";
            txtReceiptNo.Text = "";
            txtFeesAmount.Text = "";

            // DateTimePickers
            dtpBirthDate.Value = DateTime.Now;
            dtpAdmissionDate.Value = DateTime.Now;

            // RadioButtons
            rbMale.Checked = false;
            rbFemale.Checked = false;
            rbNewMember.Checked = false;
            rbOldMember.Checked = false;
            rbMonthly.Checked = false;
            rbQuarterly.Checked = false;
            rbHalfYearly.Checked = false;
            rbYearly.Checked = false;

            // CheckBoxes
            chkGym.Checked = false;
            chkCardio.Checked = false;
            chkPersonalTrainer.Checked = false;

            // Photo
            picMemberPhoto.Image = null;
            memberPhotoData = null;
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            // ViewMembers form එක හදලා, ඒක පෙන්නනවා
            ViewMembers viewForm = new ViewMembers();
            viewForm.Show(); // .ShowDialog() නෙවෙයි, .Show() දාන්න.
        }
    }
}