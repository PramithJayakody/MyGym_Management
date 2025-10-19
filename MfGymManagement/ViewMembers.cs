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

        // 1. Form එක Load වෙනකොට (ඔයාගෙ ඉල්ලීම 2)
        // Auto data load වෙනවා
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
                // List එක full load කලාම PictureBox එක clear කරනවා
                picSelectedMember.Image = null;
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

        // 2. Search Box එක (මේකෙන් search logic එක අයින් කලා)
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Auto-search නැහැ.
        }

        // 3. "Search" Button එක (ඔයාගෙ ඉල්ලීම 1 සහ 3)
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

        // 4. අලුත් "Show All" Button එක
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = ""; // Search box එක clear කරනවා

            // DataGridView එකට සම්බන්ධ කරපු DataTable එකේ Filter එක අයින් කරනවා
            (dgvMembers.DataSource as DataTable).DefaultView.RowFilter = string.Empty;

            // PictureBox එකත් clear කරනවා
            picSelectedMember.Image = null;
        }

        // 5. "Close" Button
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 6. Photo එක පෙන්නන function එක (වෙනසක් නෑ)
        private void dgvMembers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMembers.CurrentRow != null && dgvMembers.CurrentRow.DataBoundItem != null)
            {
                try
                {
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
                }
                catch (Exception)
                {
                    picSelectedMember.Image = null;
                }
            }
        }

        // 7. අලුත්ම Function එක (ඔයාගෙ ඉල්ලීම 4 සහ 5)
        // Member කෙනෙක් මත Double-Click කරාම
        private void dgvMembers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Row එකක් ඇතුළෙද double-click කලේ කියලා බලනවා (Header එකේ නෙවෙයි)
            if (e.RowIndex >= 0)
            {
                try
                {
                    // Select කරපු member ගෙ ID එකයි Name එකයි අරගන්නවා
                    DataRowView selectedRow = dgvMembers.Rows[e.RowIndex].DataBoundItem as DataRowView;
                    int memberId = Convert.ToInt32(selectedRow.Row["ID"]);
                    string memberName = selectedRow.Row["Name"].ToString();

                    // අලුත් 'PaymentDetails' form එක හදලා, ඒකට ID එකයි Name එකයි pass කරනවා
                    PaymentDetails paymentForm = new PaymentDetails(memberId, memberName);
                    paymentForm.ShowDialog(); // අලුත් form එක open කරනවා

                    // Payment form එක close කලාට පස්සෙ, Member list එක refresh කරනවා
                    // (Payment එකක් add කරා නම් ඒක බලන්න බැරි නිසා, මේක අවශ්‍ය නෑ)
                    // LoadAllMembers(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open payment details: " + ex.Message);
                }
            }
        }

        
    }
}