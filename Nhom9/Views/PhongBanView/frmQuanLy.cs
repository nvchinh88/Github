﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VB2QLNS.Models;
using VB2QLNS.Controllers;

namespace VB2QLNS.Views.PhongBanView
{
    public partial class frmQuanLy : Form
    {
        private PhongBanController ctrlPhongBan;
        public static DataGridViewRow selectCurrRow = null;
        private bool isPopup = false;

        public frmQuanLy(bool isPopup = false)
        {
            InitializeComponent();
            ctrlPhongBan = new PhongBanController();
            this.isPopup = isPopup;
        }

        private void LoadData()
        {
            dgwv.DataSource = ctrlPhongBan.ListAllChild();
            dgwv.Refresh();
            labelStatus.Text = "Có tổng số " + ctrlPhongBan.ListAllChild().Count + " bản ghi";
        }

        private void frmQoanLy_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frmNhapLieu frm = new frmNhapLieu("Add");
            frm.ShowDialog();
            frmQoanLy_Load(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNoiDungTimKiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgwv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (!isPopup)
                {
                    int idRecord = int.Parse(dgwv.CurrentRow.Cells[0].Value.ToString());
                    frmNhapLieu frm = new frmNhapLieu("Edit", idRecord);
                    frm.ShowDialog();
                    frmQoanLy_Load(sender, e);
                }
                else
                {
                    selectCurrRow = dgwv.CurrentRow;
                    this.Close();
                }
            }
        }

        private void dgwv_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
            {
                ContextMenuStrip m = new ContextMenuStrip();
                int positionMouseRow = dgwv.HitTest(e.X, e.Y).RowIndex;
                if (positionMouseRow >= 0)
                {
                    //Image newImage = Image.FromFile("D:\\apple-touch-icon-144x144-precomposed.png");
                    dgwv.Rows[positionMouseRow].Selected = true;
                    m.Items.Add("Sửa").Name = "Edit";
                    m.Items.Add("Xóa").Name = "Delete";
                }
                m.Show(dgwv, new Point(e.X, e.Y));

                m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
            }
        }
        private void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            frmNhapLieu frm;
            int idRecord = int.Parse(dgwv.CurrentRow.Cells[0].Value.ToString());
            switch (e.ClickedItem.Name.ToString())
            {
                case "Edit":
                    frm = new frmNhapLieu("Edit", idRecord);
                    frm.ShowDialog();
                    break;
                case "Delete":
                    ((ContextMenuStrip)sender).Hide();
                    if (MessageBox.Show("Bạn có muốn xóa bản ghi này?", "Thông báo...", MessageBoxButtons.YesNo).Equals(DialogResult.Yes))
                    {
                        ctrlPhongBan.Delete(new PhongBanModel() { Id = idRecord });
                    }
                    break;
            }
            frmQoanLy_Load(sender, new EventArgs());
        }

    }
}
