using Npgsql;
using System;
using System.Windows.Forms;

namespace NyobakJadwalGym
{
    public partial class Form1 : Form
    {
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=Fushihara.6;Database=Database_Gymly";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validasi input
            if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(comboBox2.Text) || string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Harap lengkapi semua field!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Simpan data ke database
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Jadwal_gym (Nama_hari, Tanggal, Nama_aktivitas, Waktu_latihan) VALUES (@Nama_hari, @Tanggal, @Jenis_aktivitas, @Waktu_latihan)";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("Nama_hari", comboBox1.Text);
                        cmd.Parameters.AddWithValue("Tanggal", textBox1.Text);
                        cmd.Parameters.AddWithValue("Jenis_aktivitas", comboBox2.Text);
                        cmd.Parameters.AddWithValue("Waktu_latihan", GetSelectedTime());

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Data berhasil disimpan!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OpenForm2();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenForm2()
        {
            Form2 form2 = new Form2();
            this.Hide();
            form2.Show();
        }

        private string GetSelectedTime()
        {
            foreach (var control in this.Controls)
            {
                if (control is RadioButton rb && rb.Checked)
                {
                    return rb.Text;
                }
            }
            return string.Empty;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Isi combobox hari
            comboBox1.Items.AddRange(new[] { "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu", "Minggu" });
            // Isi combobox jenis aktivitas
            comboBox2.Items.AddRange(new[] { "Cardio", "Arm Day", "Leg Day", "Back Day", "Chest Day", "Shoulder Day", "Rest" });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form3 = new Form2();
            this.Hide();
            form3.Show();
        }
    }
}
