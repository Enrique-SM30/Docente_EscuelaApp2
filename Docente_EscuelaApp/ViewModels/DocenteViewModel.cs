using Docente_EscuelaApp.Models;
using Docente_EscuelaApp.Services;
using Docente_EscuelaApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Docente_EscuelaApp.ViewModels
{
    public class DocenteViewModel : INotifyPropertyChanged
    {
        DocentesService docentesService = new();
        public Usuario LoginUser { get; set; } = new Usuario();
        public Docente DocenteActual { get; set; } = new Docente();
        public Grupo GrupoActual { get; set; } = new();
        public Calificacion CalificacionActual { get; set; } = new();
        public Alumno AlumnoActual { get; set; } = new();
        public string Error { get; set; }

        public ObservableCollection<Grupo> MisGrupos { get; set; }
        public ObservableCollection<Alumno> Alumnos { get; set; }
        public ObservableCollection<Calificacion> Calificaciones { get; set; }

        public ICommand LoginCommand { get; set; }
        public ICommand VerAlumnosCommand { get; set; }
        public ICommand VerCalificacionesCommand { get; set; }
        public ICommand VerAgregarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand ActualizarGruposCommand { get; set; }


        public DocenteViewModel()
        {
            LoginCommand = new Command(Login);
            VerAlumnosCommand = new Command(VerAlumnos);
            VerCalificacionesCommand = new Command(VerCalificaciones);
            VerAgregarCommand = new Command<Calificacion>(VerAgregar);
            AgregarCommand = new Command(Agregar);
            ActualizarGruposCommand = new Command(ActualizarGrupos);
        }

        private async void Agregar()
        {
            CalificacionActual.IdDocente = DocenteActual.Id;
            CalificacionActual.IdPeriodo = 2;
            CalificacionActual.IdAlumno = AlumnoActual.Id;
            CalificacionActual.IdAsignatura = DocenteActual.IdAsigantura;
            if(CalificacionActual.Calificacion1 != 0)
            {
                if(await docentesService.Insert(CalificacionActual))
                {
                    ActualizarCalificaciones();
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Seleccione una calificación", "OK");
            }
        }

        private async void VerAgregar(Calificacion calif)
        {
            CalificacionActual = calif;
            await Application.Current.MainPage.Navigation.PushAsync(new AgregarView() { BindingContext = this });
        }

        private async void VerAlumnos()
        {
            if(GrupoActual!= null)
            {
                Alumnos = new ObservableCollection<Alumno>(await docentesService.GetAlumnos(GrupoActual.Id));
                await Application.Current.MainPage.Navigation.PushAsync(new GruposView() { BindingContext = this });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Seleccione un grupo", "OK");
            }
        }
        async void ActualizarCalificaciones()
        {
            Calificaciones = new ObservableCollection<Calificacion>(await docentesService.GetCalificaciones(new Calificacion
            {
                IdAlumno = AlumnoActual.Id,
                IdDocente = DocenteActual.Id,
                IdAsignatura = DocenteActual.IdAsigantura,
                IdPeriodo = 2
            }));
            int[] utotales = { 1, 2, 3, 4, 5 };
            int[] aa = Calificaciones.Select(x => x.Unidad).ToArray();
            var p = utotales.Except(aa).ToArray();
            byte faltantes = (byte)(5 - Calificaciones.Count);
            for (int i = 0; i < faltantes; i++)
            {
                Calificaciones.Add(new Calificacion
                {
                    IdAlumno = AlumnoActual.Id,
                    IdDocente = DocenteActual.Id,
                    IdAsignatura = DocenteActual.IdAsigantura,
                    Calificacion1 = 0,
                    Unidad = p[i],
                    IdPeriodo = 2
                });
            }
            Calificaciones = new ObservableCollection<Calificacion>(Calificaciones.OrderBy(x => x.Unidad));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Calificaciones)));
        }
        private async void VerCalificaciones()
        {
            if (AlumnoActual != null)
            {
                ActualizarCalificaciones();
                await Application.Current.MainPage.Navigation.PushAsync(new CalificacionesView() { BindingContext = this });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Seleccione un alumno", "OK");
            }

        }

        private async void Login()
        {
            try
            {
                DocenteActual = await docentesService.Login(LoginUser);
                MisGrupos = new ObservableCollection<Grupo>(await docentesService.GetGrupos(DocenteActual.Id));
                if (DocenteActual != null)
                    App.Current.MainPage = new NavigationPage(new PerfilView()) { BindingContext = this };
                
            }
            catch (Exception e )
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
                Error = e.Message;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
            }

        }

        private async void ActualizarGrupos()
        {
            MisGrupos = new ObservableCollection<Grupo>(await docentesService.GetGrupos(DocenteActual.Id));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MisGrupos)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
