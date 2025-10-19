using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace MfGymManagement // <-- ඔයාගෙ project එකේ නම
{
    public partial class Form1 : Form
    {
        private string dbFileName = "gym_database.sqlite";
        private SQLiteConnection dbConnection;
        private byte[] memberPhotoData = null;

        // Edit mode එක handle කරන්න අලුත් variables 2ක්
        private bool isEditMode = false;
        private int? currentMemberId = null;

        // 1. අලුත් Member කෙනෙක් add කරන්න පාවිච්චි කරන Constructor (පරණ එක)
        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
        }

        // 2. Member කෙනෙක්ව Edit කරන්න පාවිච්චි කරන අලුත් Constructor එක
        public Form1(int memberIdToEdit)
        {
            InitializeComponent();
            InitializeDatabase();

            // Edit Mode එකට අදාල දේවල් set කරනවා
            this.isEditMode = true;
            this.currentMemberId = memberIdToEdit;

            // Form එක Edit Mode එකට සූදානම් කරනවා
            PrepareFormForEditMode();

            // Database එකෙන් ඒ member ගෙ data load කරනවා
            LoadMemberDataForEditing(memberIdToEdit);
        }

        // Form එක Edit Mode එකට සූදානම් කරන function එක
        private void PrepareFormForEditMode()
        {
            this.Text = "Edit Member Details";
            btnSaveOrUpdate.Text = "Update Details"; // Save button එකේ නම වෙනස් කරනවා

            // Fees කොටස disable කරනවා
            // මොකද fees update කරන්න 'PaymentDetails' form එක තියෙන නිසා
            gbFeesDetails.Enabled = false;

            // Member type වෙනස් කරන්න දෙන් නෑ
            rbNewMember.Enabled = false;
            rbOldMember.Enabled = false;
        }

        // Member ගෙ data ටික database එකෙන් අරන් form එකේ පුරවන function එක
        private void LoadMemberDataForEditing(int memberId)
        {
            try
            {
                dbConnection.Open();
                string sql = "SELECT * FROM Members WHERE ID = @MemberID LIMIT 1";
                SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@MemberID", memberId);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // TextBoxes
                        txtName.Text = reader["Name"].ToString();
                        txtAddress.Text = reader["Address"].ToString();
                        txtHeight.Text = reader["Height"].ToString();
                        txtWeight.Text = reader["Weight"].ToString();
                        txtContactNo.Text = reader["ContactNo"].ToString();

                        // DateTimePickers
                        dtpBirthDate.Value = Convert.ToDateTime(reader["BirthDate"]);
                        dtpAdmissionDate.Value = Convert.ToDateTime(reader["AdmissionDate"]);

                        // RadioButtons (Gender)
                        if (reader["Gender"].ToString() == "Male") rbMale.Checked = true;
                        else rbFemale.Checked = true;

                        // RadioButtons (MemberType)
                        if (reader["MemberType"].ToString() == "New") rbNewMember.Checked = true;
                        else rbOldMember.Checked = true;

                        // CheckBoxes (Workout)
                        chkGym.Checked = Convert.ToInt32(reader["WorkoutGym"]) == 1;
                        chkCardio.Checked = Convert.ToInt32(reader["WorkoutCardio"]) == 1;
                        chkPersonalTrainer.Checked = Convert.ToInt32(reader["WorkoutPersonalTrainer"]) == 1;

                        // Photo (වැදගත්ම දේ)
                        if (reader["Photo"] != DBNull.Value)
                        {
                            memberPhotoData = (byte[])reader["Photo"]; // Photo data එක variable එකට දාගන්නවා
                            using (MemoryStream ms = new MemoryStream(memberPhotoData))
                            {
                                picMemberPhoto.Image = Image.FromStream(ms); // PictureBox එකේ පෙන්නනවා
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not find member details to edit.", "Error");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading member data: " + ex.Message);
            }
            finally
            {
                dbConnection.Close();
            }
        }


        // "Save" or "Update" Button Click (මේකත් සම්පූර්ණයෙන්ම වෙනස් කලා)
        private void btnSaveOrUpdate_Click(object sender, EventArgs e)
        {
            // Form එකෙන් Data එකතු කරගැනීම (මේක පරණ code එකමයි)
            string name = txtName.Text;
            string gender = rbMale.Checked ? "Male" : (rbFemale.Checked ? "Female" : "");
            string birthDate = dtpBirthDate.Text;
            string address = txtAddress.Text;
            string height = txtHeight.Text;
            string weight = txtWeight.Text;
            string contactNo = txtContactNo.Text;
            string admissionDate = dtpAdmissionDate.Text;
            string memberType = rbNewMember.Checked ? "New" : (rbOldMember.Checked ? "Old" : "");
            int workoutGym = chkGym.Checked ? 1 : 0;
            int workoutCardio = chkCardio.Checked ? 1 : 0;
            int workoutPersonalTrainer = chkPersonalTrainer.Checked ? 1 : 0;

            // Data Validation
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(contactNo))
            {
                MessageBox.Show("Please enter at least Name and Contact No.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                dbConnection.Open();

                // **** Edit Mode එකේද, Add Mode එකේද කියලා බලනවා ****
                if (isEditMode)
                {
                    // ----- EDIT MODE (UPDATE) -----
                    string sqlUpdate = @"UPDATE Members SET 
                                            Name = @Name, Gender = @Gender, BirthDate = @BirthDate, Address = @Address, 
                                            Height = @Height, Weight = @Weight, ContactNo = @ContactNo, AdmissionDate = @AdmissionDate, 
                                            MemberType = @MemberType, WorkoutGym = @WorkoutGym, WorkoutCardio = @WorkoutCardio, 
                                            WorkoutPersonalTrainer = @WorkoutPersonalTrainer, Photo = @Photo 
                                         WHERE ID = @MemberID";

                    SQLiteCommand cmd = new SQLiteCommand(sqlUpdate, dbConnection);
                    cmd.Parameters.AddWithValue("@MemberID", this.currentMemberId.Value); // Edit කරන member ගෙ ID එක
                    // අනිත් parameters ටික
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@BirthDate", birthDate);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Height", height);
                    cmd.Parameters.AddWithValue("@Weight", weight);
                    cmd.Parameters.AddWithValue("@ContactNo", contactNo);
                    cmd.Parameters.AddWithValue("@AdmissionDate", admissionDate);
                    cmd.Parameters.AddWithValue("@MemberType", memberType);
                    cmd.Parameters.AddWithValue("@WorkoutGym", workoutGym);
                    cmd.Parameters.AddWithValue("@WorkoutCardio", workoutCardio);
                    cmd.Parameters.AddWithValue("@WorkoutPersonalTrainer", workoutPersonalTrainer);

                    if (memberPhotoData != null) cmd.Parameters.AddWithValue("@Photo", memberPhotoData);
                    else cmd.Parameters.AddWithValue("@Photo", DBNull.Value);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Member details updated successfully!", "Success");
                    this.Close(); // Update කලාට පස්සෙ form එක close කරනවා
                }
                else
                {
                    // ----- ADD NEW MODE (INSERT) -----
                    // මේක ඔයාගෙ පරණ 'Save' code එකමයි
                    string sqlMemberInsert = @"INSERT INTO Members (
                                                Name, Gender, BirthDate, Address, Height, Weight, ContactNo, 
                                                AdmissionDate, MemberType, WorkoutGym, WorkoutCardio, WorkoutPersonalTrainer, Photo
                                             ) VALUES (
                                                @Name, @Gender, @BirthDate, @Address, @Height, @Weight, @ContactNo, 
                                                @AdmissionDate, @MemberType, @WorkoutGym, @WorkoutCardio, @WorkoutPersonalTrainer, @Photo
                                             );
                                             SELECT last_insert_rowid();";

                    SQLiteCommand cmdMember = new SQLiteCommand(sqlMemberInsert, dbConnection);
                    // Parameters (values) ටික add කරනවා
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
                    if (memberPhotoData != null) cmdMember.Parameters.AddWithValue("@Photo", memberPhotoData);
                    else cmdMember.Parameters.AddWithValue("@Photo", DBNull.Value);

                    long lastInsertedMemberId = (long)cmdMember.ExecuteScalar();

                    // Fees Details
                    string receiptNo = txtReceiptNo.Text;
                    double feesAmount = 0;
                    double.TryParse(txtFeesAmount.Text, out feesAmount);

                    if (feesAmount > 0 || !string.IsNullOrWhiteSpace(receiptNo))
                    {
                        string feesMode = "";
                        if (rbMonthly.Checked) feesMode = "Monthly";
                        else if (rbQuarterly.Checked) feesMode = "Quarterly";
                        else if (rbHalfYearly.Checked) feesMode = "Half Yearly";
                        else if (rbYearly.Checked) feesMode = "Yearly";

                        string sqlPaymentInsert = @"INSERT INTO Payments (MemberID, ReceiptNo, PaymentDate, FeesMode, Amount) 
                                                    VALUES (@MemberID, @ReceiptNo, @PaymentDate, @FeesMode, @Amount)";
                        SQLiteCommand cmdPayment = new SQLiteCommand(sqlPaymentInsert, dbConnection);
                        cmdPayment.Parameters.AddWithValue("@MemberID", lastInsertedMemberId);
                        cmdPayment.Parameters.AddWithValue("@ReceiptNo", receiptNo);
                        cmdPayment.Parameters.AddWithValue("@PaymentDate", admissionDate);
                        cmdPayment.Parameters.AddWithValue("@FeesMode", feesMode);
                        cmdPayment.Parameters.AddWithValue("@Amount", feesAmount);
                        cmdPayment.ExecuteNonQuery();
                    }

                    MessageBox.Show("Member and Payment saved successfully!", "Success");
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data: " + ex.Message);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // --- අනිත් functions (වෙනසක් නෑ) ---
        private void InitializeDatabase()
        {
            // ... (ඔයාගෙ පරණ InitializeDatabase code එක, වෙනස් කරන්න එපා)
            // ... (ඒකෙ Attendance table එක හදන code එකත් තියෙන්න ඕන)
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
            }
            dbConnection = new SQLiteConnection($"Data Source={dbFileName};Version=3;");
            try
            {
                dbConnection.Open();
                // 1. Members Table
                string sqlMembers = @"CREATE TABLE IF NOT EXISTS Members (
                                ID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Gender TEXT, BirthDate TEXT, Address TEXT, 
                                Height TEXT, Weight TEXT, ContactNo TEXT, AdmissionDate TEXT, MemberType TEXT, 
                                WorkoutGym INTEGER, WorkoutCardio INTEGER, WorkoutPersonalTrainer INTEGER, Photo BLOB 
                             )";
                SQLiteCommand cmdMembers = new SQLiteCommand(sqlMembers, dbConnection);
                cmdMembers.ExecuteNonQuery();

                // 2. Payments Table
                string sqlPayments = @"CREATE TABLE IF NOT EXISTS Payments (
                                PaymentID INTEGER PRIMARY KEY AUTOINCREMENT, MemberID INTEGER, ReceiptNo TEXT, 
                                PaymentDate TEXT, FeesMode TEXT, Amount REAL, 
                                FOREIGN KEY (MemberID) REFERENCES Members(ID)
                             )";
                SQLiteCommand cmdPayments = new SQLiteCommand(sqlPayments, dbConnection);
                cmdPayments.ExecuteNonQuery();

                // 3. Attendance Table
                string sqlAttendance = @"CREATE TABLE IF NOT EXISTS Attendance (
                                            AttendanceID INTEGER PRIMARY KEY AUTOINCREMENT,
                                            MemberID INTEGER,
                                            CheckInDate TEXT,
                                            CheckInTime TEXT,
                                            FOREIGN KEY (MemberID) REFERENCES Members(ID)
                                         )";
                SQLiteCommand cmdAttendance = new SQLiteCommand(sqlAttendance, dbConnection);
                cmdAttendance.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show("Database Error: " + ex.Message); }
            finally { dbConnection.Close(); }
        }

        private void btnBrowsePhoto_Click(object sender, EventArgs e)
        {
            // ... (ඔයාගෙ පරණ btnBrowsePhoto_Click code එක, වෙනස් කරන්න එපා)
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string imagePath = openFileDialog1.FileName;
                    picMemberPhoto.Image = Image.FromFile(imagePath);
                    using (Image image = Image.FromFile(imagePath))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            image.Save(ms, ImageFormat.Png);
                            memberPhotoData = ms.ToArray();
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error loading image: " + ex.Message); }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearForm()
        {
            // ... (ඔයාගෙ පරණ ClearForm code එක, වෙනස් කරන්න එපා)
            txtName.Text = "";
            txtAddress.Text = "";
            txtHeight.Text = "";
            txtWeight.Text = "";
            txtContactNo.Text = "";
            txtReceiptNo.Text = "";
            txtFeesAmount.Text = "";
            dtpBirthDate.Value = DateTime.Now;
            dtpAdmissionDate.Value = DateTime.Now;
            rbMale.Checked = false;
            rbFemale.Checked = false;
            rbNewMember.Checked = false;
            rbOldMember.Checked = false;
            rbMonthly.Checked = false;
            rbQuarterly.Checked = false;
            rbHalfYearly.Checked = false;
            rbYearly.Checked = false;
            chkGym.Checked = false;
            chkCardio.Checked = false;
            chkPersonalTrainer.Checked = false;
            picMemberPhoto.Image = null;
            memberPhotoData = null;
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            // ViewMembers form එක හදලා, ඒක පෙන්නනවා
            ViewMembers viewForm = new ViewMembers();
            viewForm.Show(); // .ShowDialog() නෙවෙයි, .Show() දාන්න.
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}