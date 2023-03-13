using Docente_EscuelaApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docente_EscuelaApp.Services
{
    public class DocentesService
    {
        HttpClient client;
        public DocentesService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri("https://docenteprimaria.sistemas19.com/")
            };
        }
        public event Action<List<string>> Error;
        public async Task<Docente> Login(Usuario user)
        {
            string json = JsonConvert.SerializeObject(user);
            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("api/docente/usuario", stringContent);
            json = await result.Content.ReadAsStringAsync();
            Docente s = JsonConvert.DeserializeObject<Docente>(json);
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Usuario o contraseña incorrectos");
            }
            return s; 
        }

        public async Task<List<Grupo>> GetGrupos(int id)
        {
            HttpResponseMessage result = await client.GetAsync("api/docente/grupos/"+id); //Peticion

            if (result.IsSuccessStatusCode) //Verificar repsuesra exitosa 200
            {
                var json = await result.Content.ReadAsStringAsync();

                List<Grupo> lista = JsonConvert.DeserializeObject<List<Grupo>>(json);

                return lista;
            }
            return new List<Grupo>();
        }

        public async Task<List<Alumno>> GetAlumnos(int id)
        {
            HttpResponseMessage result = await client.GetAsync("api/docente/alumnos/" + id); //Peticion

            if (result.IsSuccessStatusCode) //Verificar repsuesra exitosa 200
            {
                var json = await result.Content.ReadAsStringAsync();

                List<Alumno> lista = JsonConvert.DeserializeObject<List<Alumno>>(json);

                return lista;
            }
            return new List<Alumno>();
        }

        public async Task<List<Calificacion>> GetCalificaciones(Calificacion c)
        {
            string json = JsonConvert.SerializeObject(c);
            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("api/docente/calificaciones", stringContent);
            json = await result.Content.ReadAsStringAsync();
            List<Calificacion> s = JsonConvert.DeserializeObject<List<Calificacion>>(json);
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Ha ocurrido un error: {result.StatusCode}\n{s}");
            }
            return s;
        }

        public async Task<bool> InsertCalificaciones(Calificacion c)
        {
            string json = JsonConvert.SerializeObject(c);
            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("api/docente/calificar", stringContent);
            json = await result.Content.ReadAsStringAsync();
            List<Calificacion> s = JsonConvert.DeserializeObject<List<Calificacion>>(json);
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Ha ocurrido un error: {result.StatusCode}\n{s}");
            }
            return true;
        }

        public async Task<bool> Insert(Calificacion c)
        {
            var json = JsonConvert.SerializeObject(c);
            var response = await client.PostAsync("api/docente/calificar", new StringContent(json, Encoding.UTF8,
                "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errores = await response.Content.ReadAsStringAsync();
                LanzarErrorJson(errores);
                return false;
            }
            return true;
        }
        void LanzarErrorJson(string json)
        {
            List<string> obj = JsonConvert.DeserializeObject<List<string>>(json);
            if (obj != null)
            {
                Error?.Invoke(obj);
            }
        }
    }
}
