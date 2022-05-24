using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TamayoConde_IIU.Funciones;

namespace TamayoConde_IIU.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }

        string _conexion = string.Empty;

        public Producto()
        {
            _conexion = ConfigurationManager.ConnectionStrings["ModeloDatos"].ConnectionString;
        }

        public List<Producto> ProductoListar()
        {
            List<Producto> list = new List<Producto>();

            string consulta = @"select p.id,p.nombre from producto as p";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Producto objProducto = new Producto
                                {
                                    Id = Utilitarios.ValidarInteger(dr["id"]),
                                    Nombre = Utilitarios.ValidarStr(dr["nombre"])
                                    
                                };
                                list.Add(objProducto);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }

            return list;
        }
    }
}