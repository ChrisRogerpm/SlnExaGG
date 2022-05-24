using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TamayoConde_IIU.Funciones;

namespace TamayoConde_IIU.Models
{
    public class Movimiento
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int Tipo { get; set; }
        public string Fecha { get; set; }
        public int SaldoAnterior { get; set; }
        public int Cantidad { get; set; }
        public int Entrada { get; set; }
        public int Salida { get; set; }
        public int SaldoActual { get; set; }
        public string NombreProducto { get; set; }
        public string NombreCategoria { get; set; }
        public string TipoNombre { get; set; }


        string _conexion = string.Empty;

        public Movimiento()
        {
            _conexion = ConfigurationManager.ConnectionStrings["ModeloDatos"].ConnectionString;
        }

        public List<Movimiento> MovimientoListar()
        {
            List<Movimiento> list = new List<Movimiento>();

            string consulta = @"select
                    m.id,
                    p.nombre as nombreProducto,
                    c.nombre as nombreCategoria,
                    m.fecha,
                    (
                    case m.tipo
                    when 1 then 'Entrada'
                    when 0 then 'Salida'
                    end
                    ) as tipoNombre,
                    m.saldoAnterior,
                    m.entrada,
                    m.salida,
                    m.saldoActual
            from movimiento as m
            inner join producto as p on p.id = m.idProducto
            inner join categoria as c on c.id = p.idCategoria";

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
                                Movimiento objMovimiento = new Movimiento
                                {
                                    Id = Utilitarios.ValidarInteger(dr["id"]),
                                    NombreProducto = Utilitarios.ValidarStr(dr["nombreProducto"]),
                                    NombreCategoria = Utilitarios.ValidarStr(dr["nombreCategoria"]),
                                    Fecha = Utilitarios.ValidarDate(dr["fecha"]).ToString(),
                                    TipoNombre = Utilitarios.ValidarStr(dr["tipoNombre"]),
                                    SaldoAnterior = Utilitarios.ValidarInteger(dr["saldoAnterior"]),
                                    Entrada = Utilitarios.ValidarInteger(dr["entrada"]),
                                    Salida = Utilitarios.ValidarInteger(dr["salida"]),
                                    SaldoActual = Utilitarios.ValidarInteger(dr["saldoActual"])
                                };
                                list.Add(objMovimiento);
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

        public bool MovimientoRegistrar(Movimiento obj)
        {
            bool respuesta = false;

            string consulta = @"INSERT INTO movimiento (idProducto,tipo,saldoAnterior,entrada,salida,saldoActual) values (@p0,@p1,@p2,@p3,@p4,@p5)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    Movimiento objDetalle = new Movimiento();
                    objDetalle = this.MovimientoDetalle(obj.IdProducto);

                    query.Parameters.AddWithValue("@p0", Utilitarios.ValidarInteger(obj.IdProducto));
                    query.Parameters.AddWithValue("@p1", Utilitarios.ValidarInteger(obj.Tipo));
                    query.Parameters.AddWithValue("@p2", Utilitarios.ValidarInteger(objDetalle.SaldoAnterior));
                    if (obj.Tipo == 1)
                    {
                        obj.Salida = 0;
                        obj.Entrada = obj.Cantidad;
                        obj.SaldoActual = objDetalle.SaldoAnterior + obj.Cantidad;
                    }
                    else
                    {
                        obj.Entrada = 0;
                        obj.Salida = obj.Cantidad;
                        obj.SaldoActual = objDetalle.SaldoAnterior - obj.Cantidad;
                    }
                    query.Parameters.AddWithValue("@p3", Utilitarios.ValidarInteger(obj.Entrada));
                    query.Parameters.AddWithValue("@p4", Utilitarios.ValidarInteger(obj.Salida));
                    query.Parameters.AddWithValue("@p5", Utilitarios.ValidarInteger(obj.SaldoActual));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
            }
            return respuesta;
        }

        public Movimiento MovimientoDetalle(int IdProducto)
        {
            Movimiento obj = new Movimiento();
            string consulta = @"select sum(m.saldoActual) as totalSuma from movimiento as m where m.idProducto = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", Utilitarios.ValidarInteger(IdProducto));

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                obj.SaldoAnterior = Utilitarios.ValidarInteger(dr["totalSuma"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return obj;
        }

        public bool MovimientoEliminar(Movimiento obj)
        {
            bool respuesta = false;

            string consulta = @"DELETE FROM movimiento where id = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", Utilitarios.ValidarInteger(obj.Id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
            }
            return respuesta;
        }

        public List<Movimiento> MovimientoListarFiltrar(int Filtro,int IdProducto)
        {
            List<Movimiento> list = new List<Movimiento>();
            //string filtroCustom = Filtro == 0 ? "" : "where m.tipo = @p0";
            string consulta = "";

            if (Filtro == 3)
            {
                consulta = @"select
                    m.id,
                    p.nombre as nombreProducto,
                    c.nombre as nombreCategoria,
                    m.fecha,
                    (
                    case m.tipo
                    when 1 then 'Entrada'
                    when 0 then 'Salida'
                    end
                    ) as tipoNombre,
                    m.saldoAnterior,
                    m.entrada,
                    m.salida,
                    m.saldoActual
            from movimiento as m
            inner join producto as p on p.id = m.idProducto
            inner join categoria as c on c.id = p.idCategoria
            where m.idProducto = @p1
            ";
            }
            else
            {
                consulta = @"select
                    m.id,
                    p.nombre as nombreProducto,
                    c.nombre as nombreCategoria,
                    m.fecha,
                    (
                    case m.tipo
                    when 1 then 'Entrada'
                    when 0 then 'Salida'
                    end
                    ) as tipoNombre,
                    m.saldoAnterior,
                    m.entrada,
                    m.salida,
                    m.saldoActual
            from movimiento as m
            inner join producto as p on p.id = m.idProducto
            inner join categoria as c on c.id = p.idCategoria            
            where m.tipo = @p0 and m.idProducto = @p1

            ";
            }

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", Utilitarios.ValidarInteger(Filtro));
                    query.Parameters.AddWithValue("@p1", Utilitarios.ValidarInteger(IdProducto));

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Movimiento objMovimiento = new Movimiento
                                {
                                    Id = Utilitarios.ValidarInteger(dr["id"]),
                                    NombreProducto = Utilitarios.ValidarStr(dr["nombreProducto"]),
                                    NombreCategoria = Utilitarios.ValidarStr(dr["nombreCategoria"]),
                                    Fecha = Utilitarios.ValidarDate(dr["fecha"]).ToString(),
                                    TipoNombre = Utilitarios.ValidarStr(dr["tipoNombre"]),
                                    SaldoAnterior = Utilitarios.ValidarInteger(dr["saldoAnterior"]),
                                    Entrada = Utilitarios.ValidarInteger(dr["entrada"]),
                                    Salida = Utilitarios.ValidarInteger(dr["salida"]),
                                    SaldoActual = Utilitarios.ValidarInteger(dr["saldoActual"])
                                };
                                list.Add(objMovimiento);
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