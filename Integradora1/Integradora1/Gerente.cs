using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp;
using iTextSharp.text;


namespace Integradora1
{
    public partial class Gerente : Form
    {
        ErrorProvider ErrorProvider1 = new ErrorProvider();
        private string imagen;
        private string imagen2;
        private String nombre;
        public Gerente()
        {
            InitializeComponent();

        }
        public Gerente(String nombre)
        {
            InitializeComponent();
            this.nombre = nombre;

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Gerente_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Usuario conectado " + nombre;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = DateTime.Now.ToLongTimeString();
            toolStripStatusLabel3.Text = DateTime.Now.ToLongDateString();
        }

        private void btnProducto_Click(object sender, EventArgs e)
        {
            if (pnlProducto.Width == 700)
            {
                pnlProducto.Width = 2;
                pnlUsuarios.Width = 2;
            }
            else
            {
                pnlProducto.Width = 700;
                lbproducto.Visible = true;
                DgridProducto.Visible = true;
                lbproducto.Visible = true;
                dGridUsuario.Visible = false;
            }
            Producto.Cargadatos(DgridProducto);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pnlUsuarios.Width == 700)
            {
                pnlUsuarios.Width = 2;
                pnlProducto.Width = 2;
            }
            else
            {
                pnlUsuarios.Width = 700;
                pnlProducto.Width = 700;
                lbUsuario.Visible = true;
                dGridUsuario.Visible = true;
                lbproducto.Visible = false;

            }
            Usuarios.Cargadatos(dGridUsuario);

        }


        private void dGridUsuario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {

                if (String.IsNullOrEmpty(dGridUsuario[8, e.RowIndex].Value as String))
                {
                    txtid.Text = dGridUsuario[0, e.RowIndex].Value.ToString();
                    txtap1.Text = dGridUsuario[1, e.RowIndex].Value.ToString();
                    txtap2.Text = dGridUsuario[2, e.RowIndex].Value.ToString();
                    txtnombre.Text = dGridUsuario[3, e.RowIndex].Value.ToString();
                    txtnivel.Text = dGridUsuario[4, e.RowIndex].Value.ToString();
                    txtusuario.Text = dGridUsuario[5, e.RowIndex].Value.ToString();
                    txtPass.Text = dGridUsuario[6, e.RowIndex].Value.ToString();
                    txtmail.Text = dGridUsuario[7, e.RowIndex].Value.ToString();
                }
                else
                {
                    txtid.Text = dGridUsuario[0, e.RowIndex].Value.ToString();
                    txtap1.Text = dGridUsuario[1, e.RowIndex].Value.ToString();
                    txtap2.Text = dGridUsuario[2, e.RowIndex].Value.ToString();
                    txtnombre.Text = dGridUsuario[3, e.RowIndex].Value.ToString();
                    txtnivel.Text = dGridUsuario[4, e.RowIndex].Value.ToString();
                    txtusuario.Text = dGridUsuario[5, e.RowIndex].Value.ToString();
                    txtPass.Text = dGridUsuario[6, e.RowIndex].Value.ToString();
                    txtmail.Text = dGridUsuario[7, e.RowIndex].Value.ToString();
                    txtPadusuario.Text = dGridUsuario[8, e.RowIndex].Value.ToString();
                    pcbFoto.Image = System.Drawing.Image.FromFile(dGridUsuario[8, e.RowIndex].Value.ToString());
                }


            }


        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            MysqlUsuario act = new MysqlUsuario();
            try
            {
                act.actualizar(Convert.ToInt16(txtid.Text), txtap1.Text, txtap2.Text, txtnombre.Text, Convert.ToInt16(txtnivel.Items.Count.ToString()), txtusuario.Text, txtPass.Text, txtmail.Text, imagen);
                Usuarios.Cargadatos(dGridUsuario);
                txtid.Clear();
                txtap1.Clear();
                txtap2.Clear();
                txtnombre.Clear();
                txtusuario.Clear();
                txtPass.Clear();
                
                txtmail.Clear();
                pcbFoto.Image = null;
                imagen = null;
            }
            catch (Exception)
            {

                MessageBox.Show("Error Al intentar actualizar la informacion");
            }

        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            MysqlUsuario del = new MysqlUsuario();

            if (MessageBox.Show("Seguro que Deseas borrar este usuario", "Usuario", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                MessageBox.Show("Error Al intentar borrar");
            }
            else
            {
                try
                {

                    del.delet(Convert.ToInt32(txtid.Text));
                    Usuarios.Cargadatos(dGridUsuario);
                    txtid.Clear();
                    txtap1.Clear();
                    txtap2.Clear();
                    txtnombre.Clear();
                    txtusuario.Clear();
                    txtPass.Clear();
                    
                    txtmail.Clear();
                    pcbFoto.Image = null;
                    imagen = null;
                }
                catch (Exception)
                {
                    MessageBox.Show("Error Al intentar borrar");
                }
            }




        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            MysqlUsuario add = new MysqlUsuario();
            try
            {
                if (!Validaciones.NombrePersonal(txtap2.Text))
                {
                    ErrorProvider1.SetError(txtap2, "Error solo letras porfavor, No puede estar vacio este campo ");
                    txtap2.SelectAll();
                }
                else
                {
                    ErrorProvider1.SetError(txtap2,"");
                    if (!Validaciones.NombrePersonal(txtnombre.Text))
                    {
                        ErrorProvider1.SetError(txtnombre, "Error solo letras porfavor, No puede estar vacio este campo ");
                        txtnombre.SelectAll();
                    }
                    else
                    {
                        ErrorProvider1.SetError(txtnombre,"");
                        if (!Validaciones.Usuario(txtusuario.Text))
                        {
                            ErrorProvider1.SetError(txtusuario, "Error solo letras y numeros porfavor, No puede estar vacio este campo ");
                            txtnombre.SelectAll();
                        }
                        else
                        {
                            ErrorProvider1.SetError(txtusuario,"");
                            if (!Validaciones.Contraseña(txtPass.Text))
                            {
                                ErrorProvider1.SetError(txtPass, "Error solo letras y numeros porfavor, No puede estar vacio este campo ");
                                txtPass.SelectAll();
                            }
                            else
                            {
                                ErrorProvider1.SetError(txtPass, "");
                                txtid.Clear();
                                add.add(txtap1.Text, txtap2.Text, txtnombre.Text, Convert.ToInt16(txtnivel.Items.Count.ToString()), txtusuario.Text, txtPass.Text, txtmail.Text, imagen);
                                Usuarios.Cargadatos(dGridUsuario);
                                txtap1.Clear();
                                txtap2.Clear();
                                txtnombre.Clear();
                                txtusuario.Clear();
                                txtPass.Clear();
                                txtmail.Clear();
                                pcbFoto.Image = null;
                                imagen = null;
                            }
                            
                        }
                       
                    }
                   
                }
               

            }
            catch (Exception)
            {
                MessageBox.Show("Error al agregar");
            }

        }

        private void pcbFoto_Click(object sender, EventArgs e)
        {
            CimgUsuario.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|bmp files(*.bmp)|*.bmp|gif files (*.gif)|*.gif|jped files (*.jpeg)|*.jpeg";
            CimgUsuario.FileName = "";
            CimgUsuario.Title = "Seleccione foto de perfil";
            CimgUsuario.ShowDialog();
            if (File.Exists(CimgUsuario.FileName))
            {
                pcbFoto.Image = System.Drawing.Image.FromFile(CimgUsuario.FileName);
                imagen = CimgUsuario.FileName;
            }

        }

        private void BtnTxt_Click(object sender, EventArgs e)
        {
            Save.Filter = "txt files (*.txt)|*.txt";
            Save.Title = "Seleccione donde lo desea guardar";
            Save.ShowDialog();

            if (Save.FileName != "")
            {
                using (StreamWriter sr = File.CreateText(Save.FileName))
                {
                    sr.WriteLine("ID\tPATERNO\tMATERNO\tNOMBRE\tNIVEL\tUSUARIO\tPASSWORD\tEMAIL\tIMAGEN");
                    for (int i = 0; i < dGridUsuario.RowCount; i++)
                    {
                        if (String.IsNullOrEmpty(dGridUsuario[8, i].Value as String))
                        {
                            sr.WriteLine(dGridUsuario[0, i].Value.ToString() + "\t" + dGridUsuario[1, i].Value.ToString() + "\t" + dGridUsuario[2, i].Value.ToString() + "\t" + dGridUsuario[3, i].Value.ToString() + "\t" + dGridUsuario[4, i].Value.ToString() + "\t" +
                            dGridUsuario[5, i].Value.ToString() + "\t" + dGridUsuario[6, i].Value.ToString() + "\t" + dGridUsuario[7, i].Value.ToString());

                        }
                        else
                        {
                            sr.WriteLine(dGridUsuario[0, i].Value.ToString() + "\t" + dGridUsuario[1, i].Value.ToString() + "\t" + dGridUsuario[2, i].Value.ToString() + "\t" + dGridUsuario[3, i].Value.ToString() + "\t" + dGridUsuario[4, i].Value.ToString() + "\t" +
                            dGridUsuario[5, i].Value.ToString() + "\t" + dGridUsuario[6, i].Value.ToString() + "\t" + dGridUsuario[7, i].Value.ToString() + "\t" + dGridUsuario[8, i].Value.ToString());

                        }

                    }

                }
            }

        }

        private void DgridProducto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (String.IsNullOrEmpty(DgridProducto[4, e.RowIndex].Value as String))
                {
                    txtCodigo.Text = DgridProducto[0, e.RowIndex].Value.ToString();
                    txtProducto.Text = DgridProducto[1, e.RowIndex].Value.ToString();
                    txtPrecio.Text = DgridProducto[2, e.RowIndex].Value.ToString();
                    txtDecripcion.Text = DgridProducto[3, e.RowIndex].Value.ToString();
                }
                else
                {
                    txtCodigo.Text = DgridProducto[0, e.RowIndex].Value.ToString();
                    txtProducto.Text = DgridProducto[1, e.RowIndex].Value.ToString();
                    txtPrecio.Text = DgridProducto[2, e.RowIndex].Value.ToString();
                    txtDecripcion.Text = DgridProducto[3, e.RowIndex].Value.ToString();
                    txtPad.Text = DgridProducto[4, e.RowIndex].Value.ToString();
                    pcbProducto.Image = System.Drawing.Image.FromFile(DgridProducto[4, e.RowIndex].Value.ToString());
                }

            }
        }

        private void pcbProducto_Click(object sender, EventArgs e)
        {
            CimgProducto.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|bmp files(*.bmp)|*.bmp|gif files (*.gif)|*.gif|jped files (*.jpeg)|*.jpeg";
            CimgProducto.FileName = "";
            CimgProducto.Title = "Seleccione foto de perfil";
            CimgProducto.ShowDialog();
            if (File.Exists(CimgProducto.FileName))
            {
                pcbProducto.Image = System.Drawing.Image.FromFile(CimgProducto.FileName);
                imagen2 = CimgProducto.FileName;
                MessageBox.Show(CimgProducto.FileName);
            }
        }

        private void btnPborrar_Click(object sender, EventArgs e)
        {
           
            MysqlProducto del = new MysqlProducto();
            if (MessageBox.Show("Seguro que Deseas borrar este Producto", "Producto", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                MessageBox.Show("Error Al intentar borrar");
            }
            else
            {
              
                try
                {
                    long Codigo = Convert.ToInt64(txtCodigo.Text);
                    del.delet(Codigo);
                    Producto.Cargadatos(DgridProducto);
                    txtCodigo.Clear();
                    txtProducto.Clear();
                    txtPrecio.Clear();
                    txtDecripcion.Clear();
                    pcbProducto.Image = null;
                    imagen2 = null;
                }
                catch (Exception)
                {
                    MessageBox.Show("Error Al intentar borrar");
                }
            }
        }

        private void btnPagregar_Click(object sender, EventArgs e)
        {
            
            MysqlProducto add = new MysqlProducto();
            try
            {
                if (!Validaciones.NombrePersonal(txtProducto.Text))
                {
                    ErrorProvider1.SetError(txtProducto, "Error solo letras porfavor, No puede estar vacio este campo ");
                    txtProducto.Focus();
                }
                else
                {
                    ErrorProvider1.SetError(txtProducto, "");
                    if (!Validaciones.numeros(txtPrecio.Text))
                    {
                        ErrorProvider1.SetError(txtPrecio, "Error solo numeros porfavor, No puede estar vacio este campo ");
                        txtPrecio.SelectAll();
                    }
                    else
                    {
                        ErrorProvider1.SetError(txtPrecio, "");
                       
                        txtCodigo.Clear();
                        add.add(txtProducto.Text, Convert.ToInt32(txtPrecio.Text), txtDecripcion.Text, imagen2);
                        Producto.Cargadatos(DgridProducto);

                        txtProducto.Clear();
                        txtPrecio.Clear();
                        txtDecripcion.Clear();
                        pcbProducto.Image = null;
                        imagen2 = null;
                    }
                   
                }
                

            }
            catch (Exception)
            {

                MessageBox.Show("Intenta Con otro Id de Producto");
            }
        }

        private void btnPmodificar_Click(object sender, EventArgs e)
        {
            MysqlProducto mod = new MysqlProducto();
            try
            {
                long id = Convert.ToInt64(txtCodigo.Text);
                mod.actualizar(id, txtProducto.Text, Convert.ToInt16(txtPrecio.Text), txtDecripcion.Text, imagen2);
                Producto.Cargadatos(DgridProducto);

            }
            catch (Exception)
            {

                MessageBox.Show("Error Al intentar actualizar la informacion");
            }
        }

        private void btnPtxt_Click(object sender, EventArgs e)
        {
            Save.Filter = "txt files (*.txt)|*.txt";
            Save.Title = "Seleccione donde lo desea guardar";
            Save.ShowDialog();

            if (Save.FileName != "")
            {
                using (StreamWriter sr = File.CreateText(Save.FileName))
                {
                    sr.WriteLine("CODIGO\tPRODUCTO\tPRECIO\tDESCRIPCION\tIMAGEN");
                    for (int i = 0; i < DgridProducto.RowCount; i++)
                    {
                        if (String.IsNullOrEmpty(DgridProducto[4, i].Value as String))
                        {
                            sr.WriteLine(DgridProducto[0, i].Value.ToString() + "\t" + DgridProducto[1, i].Value.ToString() + "\t" + DgridProducto[2, i].Value.ToString() + "\t" + DgridProducto[3, i].Value.ToString());
                        }
                        else
                        {
                            sr.WriteLine(DgridProducto[0, i].Value.ToString() + "\t" + DgridProducto[1, i].Value.ToString() + "\t" + DgridProducto[2, i].Value.ToString() + "\t" + DgridProducto[3, i].Value.ToString() + "\t" + DgridProducto[4, i].Value.ToString());
                        }


                    }

                }
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            txtCodigo.Clear();
            txtProducto.Clear();
            txtPrecio.Clear();
            txtDecripcion.Clear();
            pcbProducto.Image = null;
            imagen2 = null;
        }

        private void Btnlimpuarusu_Click(object sender, EventArgs e)
        {
            txtid.Clear();
            txtap1.Clear();
            txtap2.Clear();
            txtnombre.Clear();
            txtusuario.Clear();
            txtPass.Clear();
            
            txtmail.Clear();
            pcbFoto.Image = null;
            imagen = null;
        }

        private void btnUsql_Click(object sender, EventArgs e)
        {
            Save.Filter = "sql files (*.sql)|*.sql";
            Save.Title = "Seleccione donde lo desea guardar";
            Save.ShowDialog();

            if (Save.FileName != "")
            {
                using (StreamWriter sr = File.CreateText(Save.FileName))
                {
                    sr.WriteLine("USE punto_venta_bienheladas;");
                    for (int i = 0; i < dGridUsuario.RowCount; i++)
                    {
                        if (String.IsNullOrEmpty(dGridUsuario[8, i].Value as String))
                        {
                            sr.WriteLine("INSERT INTO usuarios (Id_usuario, Ap1 , Ap2, Nombre, Nivel, Usuario, Password, mail, Foto) VALUES ('" + dGridUsuario[0, i].Value.ToString() + "','" + dGridUsuario[1, i].Value.ToString() + "','" + dGridUsuario[2, i].Value.ToString() + "','"
                                + dGridUsuario[3, i].Value.ToString() + "','" + dGridUsuario[4, i].Value.ToString() + "','" + dGridUsuario[5, i].Value.ToString() + "','" + dGridUsuario[6, i].Value.ToString() + "','" + dGridUsuario[7, i].Value.ToString() + "'," + "NULL" + ");\n");
                        }
                        else
                        {
                            sr.WriteLine("INSERT INTO usuarios (Id_usuario, Ap1 , Ap2, Nombre, Nivel, Usuario, Password, mail, Foto) VALUES ('" + dGridUsuario[0, i].Value.ToString() + "','" + dGridUsuario[1, i].Value.ToString() + "','" + dGridUsuario[2, i].Value.ToString() + "','"
                                + dGridUsuario[3, i].Value.ToString() + "','" + dGridUsuario[4, i].Value.ToString() + "','" + dGridUsuario[5, i].Value.ToString() + "','" + dGridUsuario[6, i].Value.ToString() + "','" + dGridUsuario[7, i].Value.ToString() + "','" + dGridUsuario[8, i].Value.ToString().Replace(@"\", @"\\") + "');\n");

                        }

                    }

                }
            }
        }

        private void BtnPsql_Click(object sender, EventArgs e)
        {
            Save.Filter = "SQL files (*.sql)|*.sql";
            Save.Title = "Seleccione donde lo desea guardar";
            Save.ShowDialog();

            if (Save.FileName != "")
            {
                using (StreamWriter sr = File.CreateText(Save.FileName))
                {
                    sr.WriteLine("USE punto_venta_bienheladas;");
                    for (int i = 0; i < DgridProducto.RowCount; i++)
                    {
                        if (String.IsNullOrEmpty(DgridProducto[4, i].Value as String))
                        {
                            sr.WriteLine("INSERT INTO producto (id_producto, producto, precio, descripcion, foto) VALUES ('" + DgridProducto[0, i].Value.ToString() + "','" + DgridProducto[1, i].Value.ToString() + "','" + DgridProducto[2, i].Value.ToString() + "','" + DgridProducto[3, i].Value.ToString() + "'," + "NULL);\n");

                        }
                        else
                        {
                            sr.WriteLine("INSERT INTO producto (id_producto, producto, precio, descripcion, foto) VALUES ('" + DgridProducto[0, i].Value.ToString() + "','" + DgridProducto[1, i].Value.ToString() + "','" + DgridProducto[2, i].Value.ToString() + "','" + DgridProducto[3, i].Value.ToString() + "','" + DgridProducto[4, i].Value.ToString().Replace(@"\", @"\\") + "');\n");
                        }


                    }

                }
            }
        }

        private void btnPpdf_Click(object sender, EventArgs e)
        {
            Save.Filter = "pdf files (*.pdf)|*.pdf";
            Save.Title = "Seleccione donde lo desea guardar";
            Save.ShowDialog();
            if (Save.FileName != "")
            {

                Document pdf = new Document();
                try
                {
                    PdfWriter.GetInstance(pdf, new FileStream(Save.FileName, FileMode.Create));
                    pdf.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
                    pdf.Open();
                    PdfPTable Tabla = new PdfPTable(5);
                    PdfPCell Titulo = new PdfPCell(new Phrase("Reporte de Producto", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Codigo = new PdfPCell(new Phrase("Codigo", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Producto = new PdfPCell(new Phrase("Producto", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Precio = new PdfPCell(new Phrase("Precio", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Descripcion = new PdfPCell(new Phrase("Descripcion", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Foto = new PdfPCell(new Phrase("Foto"));
                    Titulo.HorizontalAlignment = 1;
                    Codigo.HorizontalAlignment = 1;
                    Precio.HorizontalAlignment = 1;
                    Descripcion.HorizontalAlignment = 1;
                    Foto.HorizontalAlignment = 1;
                    Titulo.Colspan = 5;
                    Tabla.AddCell(Titulo);
                    Tabla.AddCell(Codigo);
                    Tabla.AddCell(Producto);
                    Tabla.AddCell(Precio);
                    Tabla.AddCell(Descripcion);
                    Tabla.AddCell(Foto);
                    for (int i = 0; i < DgridProducto.Rows.Count; i++)
                    {
                        Tabla.AddCell(DgridProducto[0, i].Value.ToString());
                        Tabla.AddCell(DgridProducto[1, i].Value.ToString());
                        Tabla.AddCell(DgridProducto[2, i].Value.ToString());
                        Tabla.AddCell(DgridProducto[3, i].Value.ToString());


                        if (String.IsNullOrEmpty(DgridProducto[4, i].Value as String))
                        {
                            Tabla.AddCell("");
                        }
                        else
                        {

                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(DgridProducto[4, i].Value.ToString());
                            PdfPCell foto = new PdfPCell(img);
                            img.ScaleAbsolute(32f, 32f);
                            foto.HorizontalAlignment = 1;
                            Tabla.AddCell(foto);

                        }


                    }
                    pdf.Add(Tabla);
                    pdf.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error al intentar imprimir pdf" + ex);
                }
            }
        }

        private void btnUpdf_Click(object sender, EventArgs e)
        {

            Save.Filter = "pdf files (*.pdf)|*.pdf";
            Save.Title = "Seleccione donde lo desea guardar";
            Save.ShowDialog();
            if (Save.FileName != "")
            {

                Document pdf = new Document();
                try
                {
                    PdfWriter.GetInstance(pdf, new FileStream(Save.FileName, FileMode.Create));
                    pdf.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
                    pdf.Open();
                    PdfPTable Tabla = new PdfPTable(8);
                    PdfPCell Titulo = new PdfPCell(new Phrase("Reporte de usuario", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell id = new PdfPCell(new Phrase("id", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Paterno = new PdfPCell(new Phrase("Paterno", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Materno = new PdfPCell(new Phrase("Materno", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Nombre = new PdfPCell(new Phrase("Nombre", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Nivel = new PdfPCell(new Phrase("Nivel", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Usuarios = new PdfPCell(new Phrase("Usuarios", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Email = new PdfPCell(new Phrase("Email", FontFactory.GetFont(FontFactory.TIMES_BOLD)));
                    PdfPCell Foto = new PdfPCell(new Phrase("Foto"));

                    Titulo.HorizontalAlignment = 1;
                    id.HorizontalAlignment = 1;
                    Paterno.HorizontalAlignment = 1;
                    Materno.HorizontalAlignment = 1;
                    Nombre.HorizontalAlignment = 1;
                    Nivel.HorizontalAlignment = 1;
                    Usuarios.HorizontalAlignment = 1;
                    Email.HorizontalAlignment = 1;
                    Foto.HorizontalAlignment = 1;
                    Titulo.Colspan = 8;
                    Tabla.AddCell(Titulo);
                    Tabla.AddCell(id);
                    Tabla.AddCell(Paterno);
                    Tabla.AddCell(Materno);
                    Tabla.AddCell(Nombre);
                    Tabla.AddCell(Nivel);
                    Tabla.AddCell(Usuarios);
                    Tabla.AddCell(Email);
                    Tabla.AddCell(Foto);
                    for (int i = 0; i < dGridUsuario.Rows.Count; i++)
                    {
                        Tabla.AddCell(dGridUsuario[0, i].Value.ToString());
                        Tabla.AddCell(dGridUsuario[1, i].Value.ToString());
                        Tabla.AddCell(dGridUsuario[2, i].Value.ToString());
                        Tabla.AddCell(dGridUsuario[3, i].Value.ToString());
                        Tabla.AddCell(dGridUsuario[4, i].Value.ToString());
                        Tabla.AddCell(dGridUsuario[5, i].Value.ToString());
                        Tabla.AddCell(dGridUsuario[7, i].Value.ToString());

                        if (String.IsNullOrEmpty(dGridUsuario[8, i].Value as String))
                        {
                            Tabla.AddCell("");
                        }
                        else
                        {

                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(dGridUsuario[8, i].Value.ToString());
                            PdfPCell foto = new PdfPCell(img);
                            img.ScaleAbsolute(32f, 32f);
                            foto.HorizontalAlignment = 1;
                            Tabla.AddCell(foto);

                        }


                    }
                    pdf.Add(Tabla);
                    pdf.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error al intentar imprimir pdf" + ex);
                }
            }
        }

        private void btnUcsv_Click(object sender, EventArgs e)
        {
            Save.Filter = "csv files (*.csv)|*.csv";
            Save.Title = "Seleccione donde lo desea guardar";
            Save.ShowDialog();

            if (Save.FileName != "")
            {
                using (StreamWriter sr = File.CreateText(Save.FileName))
                {
                    for (int i = 0; i < dGridUsuario.RowCount; i++)
                    {
                        if (String.IsNullOrEmpty(dGridUsuario[8, i].Value as String))
                        {
                            sr.WriteLine(dGridUsuario[0, i].Value.ToString() + "," + dGridUsuario[1, i].Value.ToString() + "," + dGridUsuario[2, i].Value.ToString() + "," + dGridUsuario[3, i].Value.ToString() + "," + dGridUsuario[4, i].Value.ToString() + "," +
                            dGridUsuario[5, i].Value.ToString() + "," + dGridUsuario[6, i].Value.ToString() + "," + dGridUsuario[7, i].Value.ToString() + ";");

                        }
                        else
                        {
                            sr.WriteLine(dGridUsuario[0, i].Value.ToString() + "," + dGridUsuario[1, i].Value.ToString() + "," + dGridUsuario[2, i].Value.ToString() + "," + dGridUsuario[3, i].Value.ToString() + "," + dGridUsuario[4, i].Value.ToString() + "," +
                            dGridUsuario[5, i].Value.ToString() + "," + dGridUsuario[6, i].Value.ToString() + "," + dGridUsuario[7, i].Value.ToString() + "," + dGridUsuario[8, i].Value.ToString() + ";");

                        }

                    }

                }
            }
        }

        private void BtnPcsv_Click(object sender, EventArgs e)
        {
            Save.Filter = "csv files (*.csv)|*.csv";
            Save.Title = "Seleccione donde lo desea guardar";
            Save.ShowDialog();

            if (Save.FileName != "")
            {
                using (StreamWriter sr = File.CreateText(Save.FileName))
                {

                    for (int i = 0; i < DgridProducto.RowCount; i++)
                    {
                        if (String.IsNullOrEmpty(DgridProducto[4, i].Value as String))
                        {
                            sr.WriteLine(DgridProducto[0, i].Value.ToString() + "," + DgridProducto[1, i].Value.ToString() + "," + DgridProducto[2, i].Value.ToString() + "," + DgridProducto[3, i].Value.ToString() + ";");
                        }
                        else
                        {
                            sr.WriteLine(DgridProducto[0, i].Value.ToString() + "," + DgridProducto[1, i].Value.ToString() + "," + DgridProducto[2, i].Value.ToString() + "," + DgridProducto[3, i].Value.ToString() + "," + DgridProducto[4, i].Value.ToString() + ";");
                        }


                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Save.Filter = "xls files (*.xls)|*.xls";
            Save.Title = "Seleccione donde lo desea guardar";
            Save.ShowDialog();
            if (Save.FileName != "")
            {
                try
                {
                    Microsoft.Office.Interop.Excel.Application app;
                    Microsoft.Office.Interop.Excel.Workbook libro;
                    Microsoft.Office.Interop.Excel.Worksheet hojas;
                    app = new Microsoft.Office.Interop.Excel.Application();
                    libro = app.Workbooks.Add();
                    hojas = (Microsoft.Office.Interop.Excel.Worksheet)libro.Worksheets[1];
                    Microsoft.Office.Interop.Excel.Range Rango;
                    for (int i = 1; i < DgridProducto.Columns.Count; i++)
                    {
                        hojas.Cells[1, i] = DgridProducto.Columns[i - 1].HeaderText.ToString();
                        Rango = hojas.Cells[1, i];
                        Rango.Font.Bold = true;
                        Rango.Font.Name = "Century Gothic";
                        Rango.Interior.Color = Color.Gray;
                    }
                    for (int i = 0; i < DgridProducto.Rows.Count; i++)
                    {
                        for (int j = 0; j < DgridProducto.Columns.Count; j++)
                        {
                            if (DgridProducto.Rows[i].Cells[j].Value == null)
                            {
                              
                                hojas.Cells[2 + i, j + 1] = DgridProducto[j, i].Value = null;
 
                            }
                            else
                            {
                                hojas.Cells[2 + i, j + 1] = DgridProducto[j, i].Value.ToString();
                            }
                            
                        }
                    }
                    libro.SaveAs(Save.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                    libro.Close(true);
                    MessageBox.Show("XLS se genero Exitosamente");

                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                    
                }
            }
        }

        private void btnUexcel_Click(object sender, EventArgs e)
        {
            Save.Filter = "xls files (*.xls)|*.xls";
            Save.Title = "Seleccione donde lo desea guardar";
            Save.ShowDialog();
            if (Save.FileName != "")
            {
                try
                {
                    Microsoft.Office.Interop.Excel.Application app;
                    Microsoft.Office.Interop.Excel.Workbook libro;
                    Microsoft.Office.Interop.Excel.Worksheet hojas;
                    app = new Microsoft.Office.Interop.Excel.Application();
                    libro = app.Workbooks.Add();
                    hojas = (Microsoft.Office.Interop.Excel.Worksheet)libro.Worksheets[1];
                    Microsoft.Office.Interop.Excel.Range Rango;
                    for (int i = 1; i < dGridUsuario.Columns.Count; i++)
                    {
                        hojas.Cells[1, i] = dGridUsuario.Columns[i - 1].HeaderText.ToString();
                        Rango = hojas.Cells[1, i];
                        Rango.Font.Bold = true;
                        Rango.Font.Name = "Century Gothic";
                        Rango.Interior.Color = Color.Gray;
                    }
                    for (int i = 0; i < dGridUsuario.Rows.Count; i++)
                    {
                        for (int j = 0; j < dGridUsuario.Columns.Count; j++)
                        {
                            if (dGridUsuario.Rows[i].Cells[j].Value == null)
                            {
                                hojas.Cells[2 + i, j + 1] = dGridUsuario[j, i].Value = null;
                            }
                            else
                            {
                                hojas.Cells[2 + i, j + 1] = dGridUsuario[j, i].Value.ToString();

                            }
                        }
                    }
                    libro.SaveAs(Save.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                    libro.Close(true);
                    MessageBox.Show("XLS se genero Exitosamente");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Save_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
