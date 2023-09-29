using QLNhanSu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNhanSu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            QLNVContextDB context = new QLNVContextDB();
            List<Nhanvien> listPersons = context.Nhanviens.ToList();
            List<Phongban> listDepartment = context.Phongbans.ToList();
            FillDepartmentCombobox(listDepartment);
            BindGrid(listPersons);
        }

        private void FillDepartmentCombobox(List<Phongban> listDepartment)
        {
            this.cboDepartment.DataSource = listDepartment;
            this.cboDepartment.DisplayMember = "TenPB";
            this.cboDepartment.ValueMember = "MaPB";
        }

        private void BindGrid(List<Nhanvien> listPersons)
        {
            dgvPerson.Rows.Clear();
            foreach (var item in listPersons)
            {
                int index = dgvPerson.Rows.Add();
                dgvPerson.Rows[index].Cells[0].Value = item.MaNV;
                dgvPerson.Rows[index].Cells[1].Value = item.TenNV;
                dgvPerson.Rows[index].Cells[2].Value = item.Ngaysinh;
                dgvPerson.Rows[index].Cells[3].Value = item.Phongban.TenPB;
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    if (txtID.Text == "" || txtName.Text == "" || cboDepartment.Text == "")
                    {
                        throw new Exception("Vui lòng nhập đầy đủ thông tin");
                    }
                    QLNVContextDB context = new QLNVContextDB();
                    List<Nhanvien> listStudents = context.Nhanviens.ToList();

                    Nhanvien dbCheckID = context.Nhanviens.FirstOrDefault(std => std.MaNV == txtID.Text);

                    if (txtID.Text.Length != 6)
                    {
                        txtID.Focus();
                        throw new Exception("Vui lòng nhập mã nhân viên có 6 kí tự ");

                    }

                    if (dbCheckID != null)
                    {
                        throw new Exception("Ma so sv nay da ton tai");

                    }
                    string selectedDepartment = cboDepartment.Text;
                    Phongban selectedFacultyObj = context.Phongbans.FirstOrDefault(f => f.TenPB == selectedDepartment);
                    string MaPB = selectedFacultyObj.MaPB;

                    Nhanvien s = new Nhanvien() { MaNV = txtID.Text, TenNV = txtName.Text, Ngaysinh = dateTimePickerBirthday.Text, MaPB = MaPB };
                    context.Nhanviens.Add(s);
                    context.SaveChanges();

                    // Load lại datagridview với danh sách nhan viên mới.
                    List<Nhanvien> listNewStudents = context.Nhanviens.ToList();
                    dgvPerson.DataSource = null;
                    BindGrid(listNewStudents);
                    // Clear the message.
                    txtID.Clear();
                    txtName.Clear();
                    dateTimePickerBirthday.Value = DateTime.Now;

                    throw new Exception("Them moi thanh cong!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thong bao", MessageBoxButtons.OK);

                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                QLNVContextDB context = new QLNVContextDB();
                List<Nhanvien> listStudents = context.Nhanviens.ToList();

                string selectedFaculty = cboDepartment.Text;
                Phongban selectedFacultyObj = context.Phongbans.FirstOrDefault(f => f.TenPB == selectedFaculty);
                string MaPB = selectedFacultyObj.MaPB;

                Nhanvien dbUpdate = context.Nhanviens.FirstOrDefault(std => std.MaNV == txtID.Text);
                if (dbUpdate != null)
                {

                    dbUpdate.TenNV = txtName.Text;
                    dbUpdate.MaPB = MaPB;
                    dbUpdate.Ngaysinh = dateTimePickerBirthday.Text;

                    context.SaveChanges();

                    // Load lại datagridview với danh sách sinh viên mới.
                    List<Nhanvien> listNewStudents = context.Nhanviens.ToList();
                    dgvPerson.DataSource = null;
                    BindGrid(listNewStudents);
                    // Clear the message.
                    txtID.Clear();
                    txtName.Clear();

                    throw new Exception("Cap nhat thanh cong!");
                }
                else
                {
                    if (dbUpdate == null)
                    {
                        throw new Exception("Khong the thay doi ma so sv!");

                    }
                    throw new Exception("Cap nhat khong thanh cong!");


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thong bao", MessageBoxButtons.OK);

            }
        }

        private void dgvPerson_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index != -1)
            {
                txtID.Text = dgvPerson.Rows[index].Cells[0].Value.ToString();
                txtName.Text = dgvPerson.Rows[index].Cells[1].Value.ToString();
                //dateTimePickerBirthday.Text = dgvPerson.Rows[index].Cells[2].Value.ToString();
                cboDepartment.Text = dgvPerson.Rows[index].Cells[3].Value.ToString();
            }
        }

        private void dgvPerson_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                QLNVContextDB context = new QLNVContextDB();
                List<Nhanvien> listStudents = context.Nhanviens.ToList();
                DialogResult dl = MessageBox.Show("Ban co chac muon xoa nhan vien nay?", "Thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dl == DialogResult.Yes)
                {
                    Nhanvien dbDelete = context.Nhanviens.FirstOrDefault(std => std.MaNV == txtID.Text);
                    if (dbDelete != null)
                    {
                        context.Nhanviens.Remove(dbDelete);
                        context.SaveChanges();


                        // Load lại datagridview với danh sách nhân viên mới.
                        List<Nhanvien> listNewStudents = context.Nhanviens.ToList();
                        dgvPerson.DataSource = null;
                        BindGrid(listNewStudents);
                        // Clear the message.
                        txtID.Clear();
                        txtName.Clear();

                        throw new Exception("Xoa nhan vien thanh cong!");
                    }
                    else
                    {
                        throw new Exception("Vui long chon nhan vien can xoa!");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dl = MessageBox.Show("Ban co chac muon thoat ung dung khong?", "Thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dl == DialogResult.Yes)
            {
                this.Close();
            }
        }

        
    }
}
    

