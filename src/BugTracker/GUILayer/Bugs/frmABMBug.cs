using BugTracker.BusinessLayer;
using BugTracker.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BugTracker.GUILayer.Bugs
{
    public partial class frmABMBug : Form
    {

        private PrioridadService prioridadService;
        private ProductoService productoService;
        private CriticidadService criticidadService;

        private Bug oBugSeleccionado;
        private BugService oBugService;
        private FormMode formMode = FormMode.insert;

        public enum FormMode
        {
            insert,
            update,
            delete
        }
        public frmABMBug()
        {
            InitializeComponent();

            oBugService = new BugService();
            criticidadService = new CriticidadService();
            productoService = new ProductoService();
            prioridadService = new PrioridadService();

        }

        public void InicializarFormulario(FormMode op, Bug bugSeleccionado)
        {
            formMode = op;
            oBugSeleccionado = bugSeleccionado;
        }

        private void FrmABMBug_Load(object sender, EventArgs e)
        {
            LlenarCombo(cboPrioridad, prioridadService.ObtenerTodos(), "Nombre", "IdPrioridad");
            LlenarCombo(cboCriticidad, criticidadService.ObtenerTodos(), "Nombre", "IdCriticidad");
            LlenarCombo(cboProducto, productoService.ObtenerTodos(), "Nombre", "IdProducto");
            switch (formMode)
            {
                case FormMode.insert:
                    {
                        this.Text = "Nuevo Bug";
                        break;
                    }

                case FormMode.update:
                    {
                        this.Text = "Actualizar Bug";
                        // Recuperar Bug seleccionado en la grilla 
                        MostrarDatos();
                        txtTitulo.Enabled = true;
                        txtDescripcion.Enabled = true;
                        cboPrioridad.Enabled = true;
                        cboCriticidad.Enabled = true;
                        cboProducto.Enabled = true;
                        break;
                    }

            }


        }

        private void MostrarDatos()
        {
            if (oBugSeleccionado != null)
            {
                txtTitulo.Text = oBugSeleccionado.Titulo;
                txtDescripcion.Text = oBugSeleccionado.Descripcion;

                cboPrioridad.SelectedValue = oBugSeleccionado.Prioridad.IdPrioridad;
                cboCriticidad.SelectedValue = oBugSeleccionado.Criticidad.IdCriticidad;
                cboProducto.SelectedValue = oBugSeleccionado.Producto.IdProducto;
            }
        }

        private void LlenarCombo(ComboBox cbo, Object source, string display, String value)
        {
            cbo.DataSource = source;
            cbo.DisplayMember = display;
            cbo.ValueMember = value;
            cbo.SelectedIndex = -1;
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {


                switch (formMode)
                {
                    case FormMode.insert:
                        {
                            if (ValidarCampos())
                            {
                                var oBug = new Bug();
                                oBug.Titulo = txtTitulo.Text;
                                oBug.Descripcion = txtDescripcion.Text;
                                oBug.Prioridad = new Prioridad();
                                oBug.Prioridad.IdPrioridad = (int)cboPrioridad.SelectedValue;
                                oBug.Criticidad = new Criticidad();
                                oBug.Criticidad.IdCriticidad = (int)cboCriticidad.SelectedValue;
                                oBug.Producto = new Producto();
                                oBug.Producto.IdProducto = (int)cboProducto.SelectedValue;

                                if (oBugService.crearBugconHistorial(oBug))
                                {
                                    MessageBox.Show("Bug con historial insertado!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                }
                            }

                            break;
                        }

                    case FormMode.update:
                        {
                            if (ValidarCampos())
                            {
                                oBugSeleccionado.Titulo = txtTitulo.Text;
                                oBugSeleccionado.Descripcion = txtDescripcion.Text;

                                oBugSeleccionado.Prioridad.IdPrioridad = (int)cboPrioridad.SelectedValue;
                                oBugSeleccionado.Criticidad.IdCriticidad = (int)cboCriticidad.SelectedValue;
                                oBugSeleccionado.Producto.IdProducto = (int)cboProducto.SelectedValue;

                             // if (oBugService.ActualizarBug(oBugSeleccionado))
                               // {
                              //      MessageBox.Show("Bug actualizado!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                               //     this.Dispose();
                               // }
                            //    else
                             //       MessageBox.Show("Error al actualizar el Bug!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           }

                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el usuario! " + ex.Message + ex.InnerException.Message + ex.StackTrace, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ValidarCampos()
        {
            return true;
        }
    }
}
