using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using LibPrintTicket;
using BarcodeLib;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Printing;

namespace Integradora1
{
    public partial class Operador : Form
    {
        DatosBtn btn = new DatosBtn();
        private string producto = "";
        private int codigo = 0;
        private Double precio = 0;
        int pos = 0;
        private int id_ticket;
        MysqlUsuario cn_ = new MysqlUsuario();
        private MySqlDataReader dr;
        private MySqlCommand cmd;
        private double total = 0;
        private void Limpiar()
        {
            txtTODO.Clear();

        }
        public void print()
        {
            var printDocumet = new PrintDocument();
            printDocumet.PrinterSettings = new PrinterSettings() { PrinterName = "EC-PM-5890X" };
            printDocumet.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printDocumet.Print();
        }
        private void ImprimirTicket()
        {
            try
            {
                Ticket ticket = new Ticket();
                ticket.FontSize = 6;
                ticket.AddHeaderLine("DoggoApp");
                ticket.AddHeaderLine("LOCAL EN:");
                ticket.AddHeaderLine("PUEBLO ALEGRE");
                ticket.AddHeaderLine("SON, HMO");
                ticket.AddHeaderLine("RFC: ***110110***");
                ticket.HeaderImage = Image.FromFile(@"C:\Users\hollo\Pictures\Hot-Dog-icon_30324.bmp");
                ticket.AddSubHeaderLine("Ticket # 1" + id_ticket.ToString());
                ticket.AddSubHeaderLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                

                for (int i = 0; i < DgridVenta.Rows.Count; i++)
                {
                    ticket.AddItem(DgridVenta[0,i].Value.ToString(), DgridVenta[1,i].Value.ToString().Substring(0,5), DgridVenta[2, i].Value.ToString());
                }
                ticket.AddFooterLine("SUBTOTAL " + Convert.ToInt32((total * .16-total) * -1));
                ticket.AddFooterLine("IVA " + Convert.ToInt32((total *.16)));
                ticket.AddFooterLine("TOTAL " + total.ToString());
                ticket.AddFooterLine("RECIBIDO " + Convert.ToString(Convert.ToDouble(txtTODO.Text) - total));
                ticket.AddFooterLine(" " + " ");
                ticket.AddFooterLine("Gracias por su Compra, Disfrute su comida");
                ticket.PrintTicket("EC-PM-5890X");



                BarcodeLib.Barcode Codigo = new BarcodeLib.Barcode();
                Codigo.IncludeLabel = true;
                pcbCodigoBarras.BackgroundImage = Codigo.Encode(BarcodeLib.TYPE.CODE128, id_ticket.ToString(), Color.Black, Color.White, 150, 60);
                Image imgFinal = (Image)pcbCodigoBarras.BackgroundImage.Clone();
                imgFinal.Save(Directory.GetCurrentDirectory().ToString() + "/archivo.bmp", ImageFormat.Bmp);
                imgFinal.Dispose();

                print();
               
                
                
            }
           
            catch (Exception ex)
            {
                 
                 MessageBox.Show(ex.Message);
            }
        }
        
        
        private void Total()
        {
            total = 0;
            for (int i = 0; i < DgridVenta.Rows.Count; i++)
            {
                total += Convert.ToDouble(DgridVenta[2, i].Value.ToString());
            }
            Lbtotal.Text = string.Format("total:${0:#.00} MXM", Convert.ToDouble(total));
        }
        private void Buscar(String cantidad, String producto)
        {
            cmd = new MySqlCommand("SELECT producto, id_producto, precio FROM producto WHERE id_producto ='"+producto+"'", cn_.con );
            cn_.con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                DgridVenta.Rows.Add(cantidad, dr.GetString(0),dr.GetString(2), Convert.ToInt16(cantidad)* Convert.ToDouble(dr.GetString(2)),dr.GetString(1));
                Total();
            }
            cn_.con.Close();
        }
        public Operador()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Operador_Load(object sender, EventArgs e)
        {
            lbEmpresa.Location = new Point(this.Width/2-lbEmpresa.Width/2,1);
            Lbtimer.Text = DateTime.Now.ToLongTimeString()+"  "+ DateTime.Now.ToLongDateString();
            Lbtimer.Location = new Point(this.Width / 2 - Lbtimer.Width / 2, lbEmpresa.Height+1);
            //Posicion de DGV
            DgridVenta.Location = new Point(1, lbEmpresa.Height + Lbtimer.Height + 3);
            DgridVenta.Width = Convert.ToInt32(this.Width * 0.40f);
            DgridVenta.Height = Convert.ToInt16(this.Height * 0.75);
            //posicion TXT
            txtTODO.Location = new Point(1,this.Height-txtTODO.Height-2);
            txtTODO.Focus();
            txtTODO.Width = Convert.ToInt32(this.Width * 0.40f);
            //posicion de btnborrar 
            btnborrar.Location = new Point(DgridVenta.Width + 3, lbEmpresa.Height + Lbtimer.Height + 3);
            btnborrar.Width = Convert.ToInt32(this.Width * 0.09f);
            btnborrar.Height = Convert.ToInt32(this.Height * 0.10f);
            //posicion de label total
            //btnEvent1.Visible = false;
            btnEvent1.Width = Convert.ToInt32(this.Width * 0.20f);
            Lbtotal.Location = new Point( btnEvent1.Width + 2 , lbEmpresa.Height +  Lbtimer.Height + DgridVenta.Height + 3);
            // Lbtotal.Location = new Point(DgridVenta.Height - 270 , Convert.ToInt16(this.Height * .82));
            //posicion del primer boton
            btnEvent1.Location = new Point(1,lbEmpresa.Height  + Lbtimer.Height + DgridVenta.Height + 3);
            btnEvent1.Height = this.Height - pnlTitulo.Height - DgridVenta.Height - txtTODO.Height-3;
            //posicion del segundo boton
            btnEvent2.Location = new Point(DgridVenta.Width + 3, lbEmpresa.Height + Lbtimer.Height + btnborrar.Height );
            btnEvent2.Width = Convert.ToInt32(this.Width * 0.09f);
            btnEvent2.Height = Convert.ToInt32(this.Height * 0.45f);
            //posicion del third btn
            btnEvent3.Location = new Point(DgridVenta.Width + 3, lbEmpresa.Height + Lbtimer.Height + btnEvent2.Height+ btnborrar.Height);
            btnEvent3.Width = Convert.ToInt32(this.Width * 0.09f);
            btnEvent3.Height = Convert.ToInt32(this.Height * 0.40f)+3;
            //tamana de las columnas del DGV
            DgridVenta.Columns[0].Width = Convert.ToInt16(DgridVenta.Width * 0.22);
            DgridVenta.Columns[1].Width = Convert.ToInt32(DgridVenta.Width * 0.58);
            DgridVenta.Columns[2].Width = Convert.ToInt16(DgridVenta.Width * 0.20);
            DgridVenta.Columns[3].Width = Convert.ToInt16(DgridVenta.Width * 0.20-2);
            
            DgridVenta.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DgridVenta.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            pnlComida.Width = 55;
            

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Lbtimer.Text = DateTime.Now.ToLongTimeString() + "  " + DateTime.Now.ToLongDateString();
            Lbtimer.Location = new Point(this.Width / 2 - Lbtimer.Width / 2, lbEmpresa.Height + 1);

        }

        private void txtTODO_KeyPress(object sender, KeyPressEventArgs e)
        {
            //8 = back space
            //9 = tab
            //13 = enter
            //27 = escape
            if (char.IsDigit(e.KeyChar) || (int)e.KeyChar== 8 || e.KeyChar == '*' || (int)e.KeyChar == 13 || e.KeyChar== 9 || e.KeyChar == 27 || e.KeyChar == 112 )
            {
                if (e.KeyChar == 112 )
                {
                    if (DgridVenta.Rows.Count>0)
                    {
                        if (txtTODO.TextLength > 0)
                        {
                            if (total - Convert.ToDouble (txtTODO.Text) >= 0)
                            {
                                MessageBox.Show("ESTAAA MAAALL la FERIA ");
                            }
                            else
                            {
                                Lbtotal.Text = string.Format("CAMBIO:${0:#.00}MXM", Convert.ToDouble(Convert.ToDouble(txtTODO.Text)- total));
                                String query = "INSERT INTO ventas (id_venta, Fecha_venta, id_empleado, total_venta) VALUES ( NULL, NOW(), 1,"+total+");";
                                cn_.con.Open();
                                cmd = new MySqlCommand(query,cn_.con);
                                cmd.ExecuteNonQuery();
                                query = "SELECT lAST_INSERT_ID() ventas;";
                                cmd = new MySqlCommand(query, cn_.con);
                                cn_.con.Close();
                                cn_.con.Open();
                                dr = cmd.ExecuteReader();
                                dr.Read();
                                query = "INSERT INTO ventas_detalles(id_venta, cantidad, codigo, total) VALUES ";
                                for (int i = 0; i < DgridVenta.Rows.Count; i++)
                                {
                                    id_ticket = dr.GetInt16(0);
                                    query += "("+dr.GetInt16(0)+","+DgridVenta[0,i].Value.ToString()+","+DgridVenta[4,i].Value.ToString()+","+DgridVenta[3,i].Value.ToString()+")";
                                    if (i == DgridVenta.Rows.Count -1)
                                    {
                                        query += ";";
                                    }
                                    else
                                    {
                                        query += ",";
                                    }
                                    
                                }
                                dr.Close();
                                cmd = new MySqlCommand(query, cn_.con);
                                cmd.ExecuteNonQuery();
                                ImprimirTicket();
                                DgridVenta.Rows.Clear();
                                txtTODO.Clear();
                                cn_.con.Close();
                            }

                            e.Handled = true;
                        }
                        else
                        {
                            e.Handled = true;
                        }

                    }
                    else
                    {
                        e.Handled = true;
                    }

                }
                

                if (e.KeyChar == 27)
                {
                    if (DgridVenta.Rows.Count > 0)
                    {
                        DgridVenta.Rows.RemoveAt(DgridVenta.Rows.Count-1);
                        Total();
                    }
                }
                if (e.KeyChar == 9)
                {
                    if (DgridVenta.Rows.Count > 0)
                    {
                        DgridVenta.Rows.Add(DgridVenta[0, DgridVenta.Rows.Count - 1].Value.ToString(), DgridVenta[1, DgridVenta.Rows.Count - 1].Value.ToString(), DgridVenta[2, DgridVenta.Rows.Count - 1].Value.ToString(), DgridVenta[3,DgridVenta.Rows.Count-1].Value.ToString(), DgridVenta[4, DgridVenta.Rows.Count - 1].Value.ToString());
                        Total();
                        
                    }
                }
                if (e.KeyChar == '*' && txtTODO.TextLength == 0)
                {
                    e.Handled = true;
                }
                if (txtTODO.Text.IndexOf('*')>= 0 && e.KeyChar == '*')
                {
                    e.Handled = true;
                }
                else
                {
                    if (e.KeyChar == 13 && txtTODO.TextLength != 0)
                    {
                        
                        if (txtTODO.Text.IndexOf('*') != -1)
                        {
                            String[] CANT_codigo = txtTODO.Text.Split('*');
                              Buscar(CANT_codigo[0], CANT_codigo[1]);
                          
                        }
                        else
                        {
                            Buscar("1", txtTODO.Text);
                             
                        }
                        Limpiar();
                    }
                   
                }
            }
            else
            {
                e.Handled = true;
            }

            for (int i = 0; i < DgridVenta.Rows.Count; i++)
            {
                DgridVenta.Rows[i].HeaderCell.Value = (i + 1).ToString();
                if (i % 2 == 1)
                {
                    DgridVenta.Rows[i].DefaultCellStyle.BackColor = Color.AliceBlue;
                    
                }
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(Image.FromFile(@"archivo.bmp"), 10,0);

        }

        private void btnEvent1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTODO.Text == "")
                {

                }
                else
	            {
                    Lbtotal.Text = string.Format("CAMBIO:${0:#.00}MXM", Convert.ToDouble(Convert.ToDouble(txtTODO.Text) - total));
                    String query = "INSERT INTO ventas (id_venta, Fecha_venta, id_empleado, total_venta) VALUES ( NULL, NOW(), 1," + total + ");";
                    cn_.con.Open();
                    cmd = new MySqlCommand(query, cn_.con);
                    cmd.ExecuteNonQuery();
                    query = "SELECT lAST_INSERT_ID() ventas;";
                    cmd = new MySqlCommand(query, cn_.con);
                    cn_.con.Close();
                    cn_.con.Open();
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    query = "INSERT INTO ventas_detalles(id_venta, cantidad, codigo, total) VALUES ";
                    for (int i = 0; i < DgridVenta.Rows.Count; i++)
                    {
                        id_ticket = dr.GetInt16(0);
                        query += "(" + dr.GetInt16(0) + "," + DgridVenta[0, i].Value.ToString() + "," + DgridVenta[4, i].Value.ToString() + "," + DgridVenta[3, i].Value.ToString() + ")";
                        if (i == DgridVenta.Rows.Count - 1)
                        {
                            query += ";";
                        }
                        else
                        {
                            query += ",";
                        }

                    }
                    dr.Close();
                    cmd = new MySqlCommand(query, cn_.con);
                    cmd.ExecuteNonQuery();
                    ImprimirTicket();
                    DgridVenta.Rows.Clear();
                    txtTODO.Clear();
                    cn_.con.Close();
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }



        }

        private void BtnEsconder_Click(object sender, EventArgs e)
        {
            if (pnlComida.Width == 55)
            {
                pnlComida.Width = Convert.ToInt32(this.Width * 0.50);
                
            }
            else
            {
                pnlComida.Width = 55;
            }
        }

        

        private void btnEvent3_Click(object sender, EventArgs e)
        {
            if (pnlBebidas.Width == 55)
            {
                pnlBebidas.Width = Convert.ToInt32(this.Width * 0.50);
                pnlComida.Width = 55;
                pnlComida.Visible = false;
                pnlBebidas.Visible = true;
            }
            else
            {
                pnlBebidas.Width = 55;
                pnlBebidas.Visible = false;
                pnlComida.Visible = true;
            }
        }

        private void btnEvent2_Click(object sender, EventArgs e)
        {
            pnlComida.Visible = true;
            if (pnlComida.Width == 55)
            {
                pnlComida.Width = Convert.ToInt32(this.Width * 0.50);
                pnlBebidas.Visible = false;
                pnlBebidas.Width = 55;
            }
            else
            {
                pnlComida.Width = 55;
            }
        }

       

        private void btnDogoSencillo_Click(object sender, EventArgs e)
        {
            if (PnlDogoSencillo.Visible == false)
            {
                PnlDogoSencillo.Visible = true;
                pnlChileDogo.Visible = false;
                pnlDobleWini.Visible = false;
                pnlMomia.Visible = false;
                pnlPapas.Visible = false;
                pnlDogoMomia.Visible = false;
            }
            else
            {
                PnlDogoSencillo.Visible = false;
            }
        }

        private void BtnDobleWini_Click(object sender, EventArgs e)
        {
            if (pnlDobleWini.Visible == false)
            {
                pnlDobleWini.Visible = true;
                PnlDogoSencillo.Visible = false;
                pnlChileDogo.Visible = false;
                pnlMomia.Visible = false;
                pnlPapas.Visible = false;
                pnlDogoMomia.Visible = false;
            }
            else
            {
                pnlDobleWini.Visible = false;
            }
        }

        private void BtnDogoMomia_Click(object sender, EventArgs e)
        {
            if (pnlDogoMomia.Visible == false)
            {
                pnlDogoMomia.Visible = true;
                PnlDogoSencillo.Visible = false;
                pnlChileDogo.Visible = false;
                pnlDobleWini.Visible = false;
                pnlMomia.Visible = false;
                pnlPapas.Visible = false;
                
            }
            else
            {
                pnlDogoMomia.Visible = false;
            }
        }

        private void btnChileDogo_Click(object sender, EventArgs e)
        {
            if (pnlChileDogo.Visible == false)
            {
                pnlChileDogo.Visible = true;
                PnlDogoSencillo.Visible = false;
                pnlDobleWini.Visible = false;
                pnlMomia.Visible = false;
                pnlPapas.Visible = false;
                pnlDogoMomia.Visible = false;
            }
            else
            {
                pnlChileDogo.Visible = false;
            }
        }

        private void BtnMomia_Click(object sender, EventArgs e)
        {
            if (pnlMomia.Visible == false)
            {
                pnlMomia.Visible = true;
                PnlDogoSencillo.Visible = false;
                pnlChileDogo.Visible = false;
                pnlDobleWini.Visible = false;
                pnlPapas.Visible = false;
                pnlDogoMomia.Visible = false;
            }
            else
            {
                pnlMomia.Visible = false;
            }
        }

        private void BtnPapas_Click(object sender, EventArgs e)
        {
            if (pnlPapas.Visible == false)
            {
                pnlPapas.Visible = true;
                PnlDogoSencillo.Visible = false;
                pnlChileDogo.Visible = false;
                pnlDobleWini.Visible = false;
                pnlMomia.Visible = false;
                pnlDogoMomia.Visible = false;
            }
            else
            {
                pnlPapas.Visible = false;
            }
        }
        
        private void btnDS1_Click(object sender, EventArgs e)
        {
            int num = 1;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio,total,codigo });
            Total();

        }

        private void btnborrar_Click(object sender, EventArgs e)
        {
            
            if (DgridVenta.Rows.Count > 0)
            {
               
                DgridVenta.Rows.RemoveAt(pos);
                pos = 0;
                Total();
            }
        }

        private void btnDS2_Click(object sender, EventArgs e)
        {
            int num = 3;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio, total, codigo });
            Total();

        }

        private void btnDW1_Click(object sender, EventArgs e)
        {
            int num = 4;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio, total, codigo });
            Total();
        }

        private void btnDW2_Click(object sender, EventArgs e)
        {
            int num = 5;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio, total, codigo });
            Total();
        }

        private void btnCD1_Click(object sender, EventArgs e)
        {
            int num = 6;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio, total, codigo });
            Total();
        }

        private void btnCD2_Click(object sender, EventArgs e)
        {
            int num = 7;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio, total, codigo });
            Total();
        }

        private void btnDM1_Click(object sender, EventArgs e)
        {
            int num = 8;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio, total, codigo });
            Total();
        }

        private void btnDM2_Click(object sender, EventArgs e)
        {
            int num = 9;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio, total, codigo });
            Total();
        }

        private void btnM1_Click(object sender, EventArgs e)
        {
            int num = 10;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio, total, codigo });
            Total();
        }

        private void btnM2_Click(object sender, EventArgs e)
        {
            int num = 11;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio, total, codigo });
            Total();
        }

        private void btnP1_Click(object sender, EventArgs e)
        {
            int num = 12;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio, total, codigo });
            Total();
        }

        private void btnP2_Click(object sender, EventArgs e)
        {
            int num = 12;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 1, producto, precio, total, codigo });
            Total();
        }

        private void DgridVenta_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnborrar.Enabled = true;
            pos = DgridVenta.CurrentRow.Index;
        }

        private void BtnEsconder2_Click(object sender, EventArgs e)
        {
            if (pnlBebidas.Width == 55)
            {
                pnlBebidas.Width = Convert.ToInt32(this.Width * 0.50);
            }
            else
            {
                pnlBebidas.Width = 55;
            }
        }

        private void pcbCocacola_Click(object sender, EventArgs e)
        {
            if (tlCoca.Visible == false)
            {
                tlCoca.Visible = true;
                tlAgua.Visible = false;
                tlhorchata.Visible = false;
                tlLimonada.Visible = false;
                tlsprite.Visible = false;
            }
            else
            {
                tlCoca.Visible = false;
            }
        }

        private void pcbSprite_Click(object sender, EventArgs e)
        {
            if (tlsprite.Visible == false)
            {
                tlCoca.Visible = false;
                tlAgua.Visible = false;
                tlhorchata.Visible = false;
                tlLimonada.Visible = false;
                tlsprite.Visible = true;
            }
            else
            {
                tlsprite.Visible = false;
            }
        }

        private void PcbLimonada_Click(object sender, EventArgs e)
        {
            if (tlLimonada.Visible == false)
            {
                tlCoca.Visible = false;
                tlAgua.Visible = false;
                tlhorchata.Visible = false;
                tlLimonada.Visible = true;
                tlsprite.Visible = false;
            }
            else
            {
                tlLimonada.Visible = false;
            }
        }

        private void pcbHorchata_Click(object sender, EventArgs e)
        {
            if (tlhorchata.Visible == false)
            {
                tlCoca.Visible = false;
                tlAgua.Visible = false;
                tlhorchata.Visible = true;
                tlLimonada.Visible = false;
                tlsprite.Visible = false;
            }
            else
            {
                tlhorchata.Visible = false;
            }
        }

        private void pcbAgua_Click(object sender, EventArgs e)
        {
            if (tlAgua.Visible == false)
            {
                tlCoca.Visible = false;
                tlAgua.Visible = true;
                tlhorchata.Visible = false;
                tlLimonada.Visible = false;
                tlsprite.Visible = false;
            }
            else
            {
                tlAgua.Visible = false;
            }
        }

        private void btnSprite3_Click(object sender, EventArgs e)
        {
            int num = 19;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnCoca1_Click(object sender, EventArgs e)
        {
            int num = 14;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { 14, producto, precio, total, codigo });
            Total();
        }

        private void btnCoca2_Click(object sender, EventArgs e)
        {
            int num = 15;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnCoca3_Click(object sender, EventArgs e)
        {
            int num = 16;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnSprite1_Click(object sender, EventArgs e)
        {
            int num = 17;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnSprite2_Click(object sender, EventArgs e)
        {
            int num = 18;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnLimonada1_Click(object sender, EventArgs e)
        {
            int num = 20;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnLimonada2_Click(object sender, EventArgs e)
        {
            int num = 21;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnLimonada3_Click(object sender, EventArgs e)
        {
            int num = 22;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnHorchata1_Click(object sender, EventArgs e)
        {
            int num = 23;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnHorchata2_Click(object sender, EventArgs e)
        {
            int num = 24;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnHorchata3_Click(object sender, EventArgs e)
        {
            int num = 25;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnAgua1_Click(object sender, EventArgs e)
        {
            int num = 26;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnAgua2_Click(object sender, EventArgs e)
        {
            int num = 27;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }

        private void btnAgua3_Click(object sender, EventArgs e)
        {
            int num = 28;
            btn.btnCardatos(num);
            codigo = btn.cod;
            producto = btn.pro;
            precio = btn.pre;
            DgridVenta.Rows.Add(new object[] { num, producto, precio, total, codigo });
            Total();
        }
    }
}
