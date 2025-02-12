﻿

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Entities;
using System.Data;
using System.Data.SqlClient;
// para usar el msgbox como depurador
using System.Windows.Forms;

namespace BugTracker.DataAccessLayer
{
    public class BugDao
    {
        public Bug GetBugById(int idBug)
        {
            var strSql = String.Concat("SELECT bug.id_bug, ",
                                      "        bug.titulo,",
                                      "        bug.descripcion,",
                                      "        bug.fecha_alta,",
                                      "        bug.id_usuario_responsable,",
                                      "        responsable.usuario as responsable,  ",
                                      "        bug.id_usuario_asignado,",
                                      "        asignado.usuario as asignado, ",
                                      "        bug.id_producto,",
                                      "        producto.nombre as producto, ",
                                      "        bug.id_prioridad,",
                                      "        prioridad.nombre as prioridad, ",
                                      "        bug.id_criticidad,",
                                      "        criticidad.nombre as criticidad, ",
                                      "        bug.id_estado,",
                                      "        estado.nombre as estado",
                                       "   FROM Bugs as bug",
                                       "   LEFT JOIN Usuarios as responsable ON responsable.id_usuario = bug.id_usuario_responsable",
                                       "   LEFT JOIN Usuarios as asignado ON asignado.id_usuario = bug.id_usuario_asignado",
                                       "  INNER JOIN Productos as producto ON producto.id_producto = bug.id_producto",
                                       "  INNER JOIN Prioridades as prioridad ON  prioridad.id_prioridad = bug.id_prioridad",
                                       "  INNER JOIN Criticidades as criticidad ON criticidad.id_criticidad = bug.id_criticidad",
                                       "  INNER JOIN Estados as estado ON estado.id_estado = bug.id_estado",
                                       " WHERE bug.borrado=0 AND bug.id_bug = " + idBug.ToString());

            return MappingBug(DBHelper.GetDBHelper().ConsultaSQL(strSql).Rows[0]);
        }

        public IList<Bug> GetBugByFilters(Dictionary<string, object> parametros)
        {
            List<Bug> listadoBugs = new List<Bug>();

            var strSql = String.Concat("SELECT bug.id_bug, ",
                                      "        bug.titulo,",
                                      "        bug.descripcion,",
                                      "        bug.fecha_alta,",
                                      "        bug.id_usuario_responsable,",
                                      "        responsable.usuario as responsable,  ",
                                      "        bug.id_usuario_asignado,",
                                      "        asignado.usuario as asignado, ",
                                      "        bug.id_producto,",
                                      "        producto.nombre as producto, ",
                                      "        bug.id_prioridad,",
                                      "        prioridad.nombre as prioridad, ",
                                      "        bug.id_criticidad,",
                                      "        criticidad.nombre as criticidad, ",
                                      "        bug.id_estado,",
                                      "        estado.nombre as estado",
                                      "   FROM Bugs as bug",
                                      "   LEFT JOIN Usuarios as responsable ON responsable.id_usuario = bug.id_usuario_responsable",
                                      "   LEFT JOIN Usuarios as asignado ON asignado.id_usuario = bug.id_usuario_asignado",
                                      "  INNER JOIN Productos as producto ON producto.id_producto = bug.id_producto",
                                      "  INNER JOIN Prioridades as prioridad ON  prioridad.id_prioridad = bug.id_prioridad",
                                      "  INNER JOIN Criticidades as criticidad ON criticidad.id_criticidad = bug.id_criticidad",
                                      "  INNER JOIN Estados as estado ON estado.id_estado = bug.id_estado",
                                      "  WHERE bug.borrado=0 AND 1=1 ");

            if (parametros.ContainsKey("fechaDesde") && parametros.ContainsKey("fechaHasta"))
                strSql += " AND (fecha_alta>=@fechaDesde AND fecha_alta<=@fechaHasta) ";
            if (parametros.ContainsKey("idPrioridad"))
                strSql += " AND (prioridad.id_prioridad=@idPrioridad) ";
            if (parametros.ContainsKey("idCriticidad"))
                strSql += " AND (criticidad.id_criticidad=@idCriticidad) ";
            if (parametros.ContainsKey("idProducto"))
                strSql += " AND (producto.id_producto=@idProducto) ";
            if (parametros.ContainsKey("idEstado"))
                strSql += " AND (estado.id_estado=@idEstado)  ";
            if (parametros.ContainsKey("idUsuarioAsignado"))
                strSql += " AND (id_usuario_asignado=@idUsuarioAsignado) ";
            strSql += " ORDER BY bug.fecha_alta DESC";

            var resultadoConsulta = (DataRowCollection) DBHelper.GetDBHelper().ConsultaSQLConParametros(strSql, parametros).Rows;

            foreach (DataRow row in resultadoConsulta)
            {
                listadoBugs.Add(MappingBug(row));
            }

            return listadoBugs;
        }



        public IList<Bug> GetBugByFiltersCondiciones(String condiciones)
        {
            List<Bug> listadoBugs = new List<Bug>();
            String sqlcondiciones = condiciones;

            var strSql = String.Concat("SELECT bug.id_bug, ",
                                      "        bug.titulo,",
                                      "        bug.descripcion,",
                                      "        bug.fecha_alta,",
                                      "        bug.id_usuario_responsable,",
                                      "        responsable.usuario as responsable,  ",
                                      "        bug.id_usuario_asignado,",
                                      "        asignado.usuario as asignado, ",
                                      "        bug.id_producto,",
                                      "        producto.nombre as producto, ",
                                      "        bug.id_prioridad,",
                                      "        prioridad.nombre as prioridad, ",
                                      "        bug.id_criticidad,",
                                      "        criticidad.nombre as criticidad, ",
                                      "        bug.id_estado,",
                                      "        estado.nombre as estado",
                                      "   FROM Bugs as bug",
                                      "   LEFT JOIN Usuarios as responsable ON responsable.id_usuario = bug.id_usuario_responsable",
                                      "   LEFT JOIN Usuarios as asignado ON asignado.id_usuario = bug.id_usuario_asignado",
                                      "  INNER JOIN Productos as producto ON producto.id_producto = bug.id_producto",
                                      "  INNER JOIN Prioridades as prioridad ON  prioridad.id_prioridad = bug.id_prioridad",
                                      "  INNER JOIN Criticidades as criticidad ON criticidad.id_criticidad = bug.id_criticidad",
                                      "  INNER JOIN Estados as estado ON estado.id_estado = bug.id_estado",
                                      "  WHERE bug.borrado=0 AND 1=1 ");

            strSql += sqlcondiciones;

            //sin parametros
            strSql += "ORDER BY bug.fecha_alta DESC";


            var resultadoConsulta = (DataRowCollection)DBHelper.GetDBHelper().ConsultaSQL(strSql).Rows;

            foreach (DataRow row in resultadoConsulta)
            {
                listadoBugs.Add(MappingBug(row));
            }

            return listadoBugs;
        }





        private Bug MappingBug(DataRow row)
        {
            Bug oBug = new Bug();
            oBug.IdBug = Convert.ToInt32(row["id_bug"].ToString());
            oBug.Titulo = row["titulo"].ToString();
            oBug.Descripcion = row["descripcion"].ToString();
            oBug.FechaAlta = Convert.ToDateTime(row["fecha_alta"].ToString());
            oBug.Producto = new Producto();
            oBug.Producto.IdProducto = Convert.ToInt32(row["id_producto"].ToString());
            oBug.Producto.Nombre = row["producto"].ToString();

            oBug.Prioridad = new Prioridad();
            oBug.Prioridad.IdPrioridad = Convert.ToInt32(row["id_prioridad"].ToString());
            oBug.Prioridad.Nombre = row["prioridad"].ToString();

            oBug.Criticidad = new Criticidad();
            oBug.Criticidad.IdCriticidad = Convert.ToInt32(row["id_criticidad"].ToString());
            oBug.Criticidad.Nombre = row["criticidad"].ToString();


            oBug.Estado = new Estado();
            oBug.Estado.IdEstado = Convert.ToInt32(row["id_estado"].ToString());
            oBug.Estado.Nombre = row["estado"].ToString();

            oBug.UsuarioResponsable = new Usuario();
            oBug.UsuarioResponsable.IdUsuario = Convert.ToInt32(row["id_usuario_responsable"].ToString());
            oBug.UsuarioResponsable.NombreUsuario = row["responsable"].ToString();

            oBug.UsuarioAsignado = new Usuario();
            oBug.UsuarioAsignado.IdUsuario = Convert.ToInt32(row["id_usuario_asignado"].ToString());
            oBug.UsuarioAsignado.NombreUsuario = row["asignado"].ToString();


            return oBug;
        }


        public bool createBugConHistorial(Bug oBug)
        {
            DataManager dm = new DataManager();
            try
            {
                //Select @@identity obtiene el identity insertado
                //valor de id estado, id responsable y asignado fijos
                string sql = "INSERT INTO Bugs(titulo,descripcion,id_producto,id_prioridad,id_criticidad,fecha_alta, id_estado,id_usuario_responsable,id_usuario_asignado,borrado) " +
                            "   VALUES("+  
                            "'"+oBug.Titulo +"'"+","+
                            "'"+oBug.Descripcion +"'"+","+
                            oBug.Producto.IdProducto +","+
                            oBug.Prioridad.IdPrioridad + "," +
                            oBug.Criticidad.IdCriticidad + "," +
                             "getdate(),"+"1,"+"1,"+"1,"+"0"+")";
               
                dm.Open();
                dm.BeginTransaction();

                //Ejecuto el insert del bug
                dm.EjecutarSQL(sql);
                MessageBox.Show(sql, "muestro la sentencia insert", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var newId = dm.ConsultaSQLScalar(" SELECT @@IDENTITY");
                MessageBox.Show(Convert.ToString(newId), "muestro el identity", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Guarda en id_bug el identity generado

                oBug.IdBug = Convert.ToInt32(newId);
                
                string sqlhisto = "INSERT INTO BugsHistorico(id_bug,titulo,descripcion,id_producto,id_prioridad,id_criticidad, id_estado, fecha_historico)" + "   VALUES(" + oBug.IdBug + "," + "'" + oBug.Titulo + "'" + "," + "'" + oBug.Descripcion + "'" + "," + oBug.Producto.IdProducto + "," + oBug.Prioridad.IdPrioridad + "," + oBug.Criticidad.IdCriticidad + "," + 1 + "," + "GETDATE()" + ")";
                MessageBox.Show(sqlhisto, "muestro la sentencia insert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Guarda en id_bug el identity generado
                dm.EjecutarSQL(sqlhisto);

                               dm.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dm.Rollback();
                return false;
            }
            finally
            {
                // Cierra la conexión 
                dm.Close();
            }

        }

    }

}